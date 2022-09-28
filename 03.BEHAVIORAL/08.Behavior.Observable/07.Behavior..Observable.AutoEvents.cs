using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;

namespace DotNetDesignPatternDemos.Behavioral.Observer.ContainerWireup
{
    /* 
     * In questo esempio definiamo un comportamento per il Modello Observer un pò
     * diverso rispetto alla classica modalità di sottoscrizione.
     * Per dire in parole povere facciamo si che non sia necessario per gli Observing
     * cioè quegli oggetti che si sottoscrivono agli eventi o implementino un pattern
     * di interfacce Rx per sottoscriversi. Ma usiamo altri strumenti per automatizzare
     * la sottoscrizione agli eventi utilizzando approcci come il Container in DI o le
     * reflection o gli Attributi del metodo, in alcuni casi come lo IOC può sembrare
     * complicato questo codice, ma visto come organizzazione del codice è migliorativo
     * avendo in un singolo punto la dichiarazione nel sistema per tutti gli oggetti che
     * devono sottoscriversi e per quali tipi di evento.
     * In questo esempio viene costruito per fare in modo di automatizzare questo disegno
     * per la sottoscrizione delle interfacce tutte ereditate dalla base IEvent che definiscono
     * il tipo di evento e l'handle gestore dell'evento, in quest modo le due classi sottoposte
     * ad essere osservate dai sottoscrittori diventano classi che implementazno quel determinato
     * evento, e fatto ciò lo IOC ha la possibilità centralmente di sottoscrivere i componenti
     * da un unico punto a gestire le notifiche che arrivano da queste classi.
     * Nel Main quindi vediamo la rosa di composizione che serve a fare un giro tramite le 
     * reflection sul codice dei componenti per registrare gli event nelle classi.
     * Uno dei problemi relativi a questo approccio è che se per esempio vogliamo fornire a una
     * delle classi Observer sottoscrittori un altro metodo che deve accedere a quell'event che
     * sarà dedotto solo nel container non c'è possibilità di ottenerlo se non continuando a 
     * scriver codice dinamico tramite reflection quindi un ipotetico IDisposable che ha la necessità
     * di usare l'oggetto event interno non sà nel codice della classe che è ancora presente anche se
     * lo sarà in fase successiva con la ricostruzione del codice della classe. Stessa cosa per
     * avere una autoregistrazione agli eventi in modo dinamico e automatico da questo approccio ma
     * vogliamo applicarlo solo a determinate classi e non ad altre possiamo solo o flaggare in qualche
     * modo le classi nel codice che vogliamo sottoporre al processo di ricostruzione dinamico del 
     * codice e non toccare le altre.
     */

    public interface IEvent { }

    // Per l'observer l'evento in cui è possibile sottoscriversi
    public interface ISend<TEvent> where TEvent : IEvent
    {
        event EventHandler<TEvent> Sender;
    }

    // Per l'observing l'handle dell'evento in ascolto
    public interface IHandle<TEvent> where TEvent : IEvent
    {
        void Handle(object sender, TEvent args);
    }

    // Il tipo di evento con gli attributi che si porta dietro
    public class ButtonPressedEvent : IEvent
    {
        public int NumberOfClicks;
    }

    // Observed
    public class Button : ISend<ButtonPressedEvent>
    {
        public event EventHandler<ButtonPressedEvent> Sender;

        // L'evento invocato
        public void Fire(int clicks)
        {
            Sender?.Invoke(this, new ButtonPressedEvent
            {
                NumberOfClicks = clicks
            });
        }
    }

    // Observing
    public class Logging : IHandle<ButtonPressedEvent>
    {
        // La notifica 
        public void Handle(object sender, ButtonPressedEvent args)
        {
            Console.WriteLine(
              $"Button clicked {args.NumberOfClicks} times");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var cb = new ContainerBuilder();
            var ass = Assembly.GetExecutingAssembly();

            // register publish interfaces
            cb.RegisterAssemblyTypes(ass)
              .AsClosedTypesOf(typeof(ISend<>))
              .SingleInstance();

            // register subscribers
            // L'automazione della registrazione agli eventi del Observed avviene
            // da questo unico punto trramite un espresione che prende in carico
            // tutte le classi del sitema che implementano il tipo IHandle
            // e tramite il metodo OnActivated dello IOC che è usato al momento in
            // cui dopo il Build per risolvere i componenti nel Container usa questa
            // funzione inserita come lamda di funzione per non avere un metodo a parte
            // e dentro questa funzione attraverso l'uso delle reflection opera nell'istanza
            // che si sta creando per il componente rischiesto controllando sempre che
            // sia un istanza di un componente Observer che implementi appunto la IHandle<>
            // e lo contorna attraverso la funzione CreateDelegate(....) e poi l'AddEventHandler(...)
            // per sottoscrivere l'evento prima di utilizzare il componente nel contesto.
            cb.RegisterAssemblyTypes(ass)
              .Where(t =>
                t.GetInterfaces()
                  .Any(i =>
                    i.IsGenericType &&
                    i.GetGenericTypeDefinition() == typeof(IHandle<>)))
              .OnActivated(act =>
              {
                  var instanceType = act.Instance.GetType();
                  var interfaces = instanceType.GetInterfaces();
                  foreach (var i in interfaces)
                  {
                      if (i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IHandle<>))
                      {
                          // IHanlde<Foo>
                          var arg0 = i.GetGenericArguments()[0];
                          // ISend<Foo> construnct
                          var senderType = typeof(ISend<>).MakeGenericType(arg0);
                          // Ogni singolo ISend<Foo> nel container
                          // Per ogni tipo enumerabile nel container creeremo un tipo geneerico
                          // come sarebbe nel codice IEnumerable<ISend<Foo>>
                          var allSenderTypes = typeof(IEnumerable<>).MakeGenericType(senderType);
                          // Ecco che adesso posso usare il resolve per iterare tra tutti
                          // gli elementi nel container e ottenere il contesto della classe
                          var allServices = act.Context.Resolve(allSenderTypes);
                          foreach (var service in (IEnumerable)allServices)
                          {
                              // E per ogni classe che otteniamo e che ha il metodo Handle
                              // forniamo il delegato al metodo Handle istanziato e quindi
                              // sottoscritto al metodo Sender dell'oggetto Observed
                              var eventInfo = service.GetType().GetEvent("Sender");
                              var handleMethod = instanceType.GetMethod("Handle");
                              var handler = Delegate.CreateDelegate(eventInfo.EventHandlerType, null, handleMethod);
                              eventInfo.AddEventHandler(service, handler);
                          }
                      }
                  }
              })
              .SingleInstance()
              .AsSelf();


            var container = cb.Build();

            // E' qui che il metodo .OnActivated() opera per inserire nel codice
            // in modo automatico le parti di codice necessarie alla registrazione
            // dell'evento.
            var button = container.Resolve<Button>();
            var logging = container.Resolve<Logging>();

            // Adesso la classe Observed scatena l'evento e le
            // istanze sottoscritte agli eventi riceveranno notifica.
            button.Fire(1);
            button.Fire(2);
        }
    }
}