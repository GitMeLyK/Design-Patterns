using System;
using System.Collections.Generic;
// Estensioni Rx
using System.Reactive.Linq;
using static System.Console;

namespace DotNetDesignPatternDemos.Behavioral.Observer.Interfaces
{
    /*
     * In questo esempio viene trattao il Modello observable con l'ausilio
     * di un particolare progetto conosciuto come Reactive o Rx utilizzando 
     * interfacce disponibili attraverso questa dll la IObservable<Event> e la IObserver<Event>.
     * Già da qui possiamo notare come vengono identificati i vari componenti nel
     * sistema, La classe o le classi che implmentano il proprio modo di rendersi
     * osservabili con i metodi esposti dell'interfaccia IObservable<Event> e tutti
     * gli oggetti che in qualche modo vogliono essere notificati da un particolare
     * tipo di evento che succede in queste classi e che ne implementatno i metodi
     * preposti a ricevere notifiche tramite l'interfaccia IObserver<Event>.
     * Quindi per cui nell'ordine delle cose questa dll Rx che introduce appunto delle
     * estensioni reattive e che definisce due tipi di interfacce da implementare per
     * gli attori Observable e Observer in questo esempio la classe Person per l'Observable
     * e la classe Demo per l'Observer fà si che la prima abbia in sè la giusta implementazione
     * nel trattamento di queste notifiche a cui si sottoscrivono avendo una lista di tutti
     * i sottoscrittori agli eventi e una sub class che definisce un oggetto di tipo sottoscrittore
     * con un suo ciclo di vita per cui Disposable che si rimuove da solo dall'elenco delle sottoscrizioni
     * per le notifiche al sistema. La seconda La Demo che implementa i tre metodi consoni a questo meccanismo
     * e cioè l'handle notificato dalla sottoscrizione OnNext l'handler notificato in caso di eccezioni non
     * gestite nella coda degli eventi OnError e l'Handler quando l'oggetto monitorato ha finito il suo
     * ciclo di vita e non invierà da quel momento più notifiche OnCoomplete.
     * E' importante notare che mentre le Interfacce reattive IObserver e IOBserved fanno parte del
     * framework e non c'è bisogno di fare aggiunte le estensioni reattive hanno bisogno del pacchetto
     * RX da importare con nuget per essere utilizzate, molte di queste estensioni facilitano lo sviluppo
     * del codice come nellesempio sotto senza per forza implementare funzioni o classi al momento della
     * notificare qualcosa ma potendo definendo il tipo di evento da gestire come tipo inserire una lambda per
     * fare qualcosa su quella notifica,          
     *          person.OfType<FallsIllEvent>()
     *         .Subscribe(args => WriteLine($"A doctor has been called to {args.Address}")); 
     * Per questa dll esistono una varietà di metodi a supporto del pattern Observable, come ad
     * esempio è possibile realizzare un sistema di BrokerEvent che come abbiamo visto nell'esempio
     * della squadra di calcio lavora tramite queste estensioni per gestire uninsieme di eventi, ma
     * anche come utilizzare un meccanismo interno per creare al volo degli Observable con metodi simili a questo
     *         Observable Sources = new Observable().Create<>()
     * o come convertire gli elementi interni di .net come delegati events Task<T> IEnumerables in sequenze
     * Observable che tornano utili avere appunto come sequenze.
     */

    public class Event{}

    // Il Tipo di Evento da gestire nelle notifiche
    // con tutte le proprietà del caso da notificare.
    public class FallsIllEvent : Event
    {
        public string Address;
    }

    // L'Observable
    public class Person : IObservable<Event>
    {
        // La lista di tutte la classi sottoposti a Observing nel
        // costurtto di questa implementazione reattiva
        private readonly HashSet<Subscription> subscriptions
          = new HashSet<Subscription>();

        // Il metodo per la sottoscrizione da parte dei componenti
        // che vogliono ricevere notifiche di cambiamenti di questo oggetto.
        public IDisposable Subscribe(IObserver<Event> observer)
        {
            var subscription = new Subscription(this, observer);
            subscriptions.Add(subscription);
            return subscription;
        }

        // Il Metodo che esegue il l'Handle del cambiamento di stato di questo oggetto
        public void CatchACold()
        {
            foreach (var sub in subscriptions)
                sub.Observer.OnNext(new FallsIllEvent { Address = "123 London Road" });
        }

        // Per la Collection Hashset delle sottoscrizioni questo è l'implementazione
        // nascosta del modello Observer in modo reattivo che restutisce un oggetto
        // sottoscrizione di tipo dereferenziabile dal Dispose, il quale una volta
        // finito il suo ciclo di vita cancella la sua stessa sottoscrizione dalla lista.
        private class Subscription : IDisposable
        {
            private Person person;
            public IObserver<Event> Observer;

            public Subscription(Person person, IObserver<Event> observer)
            {
                this.person = person;
                Observer = observer;
            }

            public void Dispose()
            {
                person.subscriptions.Remove(this);
            }
        }
    }

    // L'Observer
    public class Demo : IObserver<Event>
    {
        static void Main402(string[] args)
        {
            new Demo();
        }

        public Demo()
        {
            var person = new Person();
            var sub = person.Subscribe(this);

            // Con l'ausilio delle estensioni Reattive ci sottoscriviamo presso l'observer
            // per essere notificati nel metodo lambda in questo delegato a fare qualcosa.
            person.OfType<FallsIllEvent>()
              .Subscribe(args => WriteLine($"A doctor has been called to {args.Address}"));
        }

        // L'Handler impmentato nell'interfaccia reattiva che riceve sempre tutte
        // le notifiche al momento di qualche stato di cambiamento o di eventi generati
        // nell'Observer
        public void OnNext(Event value)
        {
            if (value is FallsIllEvent args)
                WriteLine($"A doctor has been called to {args.Address}");
        }

        // L'Hanlder che notifica una eccezione di stato nella coda degli eventi generati
        // dall'Observer e che non è stato gestito.
        public void OnError(Exception error) { }
        
        // Quando l'oggetto Observer è stato concluso nel suo Dispose per deferenziazione
        // la notifca prima della completa chiusura degli eventni che non sarà più attiva
        // per quella istanza.
        public void OnCompleted() { }
    }
}
