using System;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetDesignPatternDemos.Architectural.Interceptor
{
    /*
     *  Gli aspetti chiave del modello sono che la modifica è trasparente e 
     *  utilizzata automaticamente. In sostanza, il resto del sistema non 
     *  deve sapere che qualcosa è stato aggiunto o modificato e può continuare 
     *  a funzionare come prima. 
     *  
     *  Per facilitare questo, è necessario implementare un'interfaccia predefinita 
     *  per l'estensione, è necessario un qualche tipo di meccanismo di dispacciamento 
     *  in cui vengono registrati gli intercettori (questo può essere dinamico, 
     *  in fase di esecuzione o statico, ad esempio tramite file di configurazione) 
     *  e vengono forniti oggetti di contesto, che consentono l'accesso allo stato 
     *  interno del framework. 
     * 
     *  Uso tipico
     *  
     *  Gli utenti tipici di questo modello sono i server Web (come menzionato sopra), gli oggetti
     *  e il middleware orientato ai messaggi
     *  
     *  Un esempio di implementazione di questo modello è l'interfaccia javax.servlet.Filter, 
     *  che fa parte di Java Platform, Enterprise Edition.
     *  
     *  La programmazione orientata agli aspetti (AOP) può anche essere utilizzata in 
     *  alcune situazioni per fornire la capacità di un intercettore, sebbene AOP 
     *  non utilizzi gli elementi tipicamente definiti per il modello dell'intercettore.
     *  
     *  
     *  File allegato FaultyMiddleware.zip in allegato per testare.
     *  
     *  Qui riportiamo un esempio di questo modello in .net per lavarel.
     *  
     *  Un esempio quasi reale di utilizzo dell'Interceptor Pattern per sfruttare nuovi 
     *  comportamenti senza modificare le implementazioni.
     *  
     *  Caso d'uso reale per il modello intercettore. Per semplicità, il codice visualizzato 
     *  non è quello di produzione effettivo, ma è fortemente basato su di esso.
     *  In questo articolo non troverai l'explanaition di cosa consiste il modello intercettore, 
     *  né cosa sia Dependency Injection. È necessario che il lettore conosca già quei concetti. 
     *  
     *  Chi non ha mai invocato un middleware può lanciare qui la prima pietra.
     *  
     *  Lo chiamo un middleware qualsiasi pezzo di software che gira ovunque e comunque, 
     *  ed è stato fatto da qualsiasi. Non ho bisogno di sapere come è fatto, solo che 
     *  ho bisogno di invocare un servizio e mi dà una risposta, a volte ...
     *  
     *  Concentriamoci su questi casi.
     *  
     *      - A volte, viene distribuita una nuova versione e il servizio è inattivo.
     *      - A volte, c'è un bug e il servizio esplode.
     *      - A volte, il server è così sovraccarico, il servizio scade.
     *      - A volte, perdiamo la rete e non possiamo raggiungere il servizio.
     *      
     *  Bene... probabilmente ci sono 1001 motivi per cui alcuni servizi falliscono dandoci 
     *  una risposta adeguata.
     *  
     *  La maggior parte degli errori sono transitori. Ok, abbiamo fallito l'invocazione, riproviamo?
     *  Questa è la risposta giusta. Proviamo di nuovo, fino a un certo numero di tentativi. 
     *  Se il problema è stato casuale e / o causato da un picco, invocare il servizio più di 
     *  una volta può migliorarci un certo tasso di successo.
     *  
     *  NOTA: tenere presente che una delle procedure consigliate per i sistemi distribuiti 
     *  consiste nel consentire la correzione dei messaggi senza errori.
     *  
     *      Nella classe che simula il servizio difettoso farà si che
     *      Questo falso servizio farà esplodere casualmente il 30% delle sue invocazioni. 
     *      Questo non è perfetto, ma si adatta alle nostre esigenze. 
     *      Diamo un'occhiata a come il client consumerà questo servizio.
     *  
     *      nella classe client consumer approcciamo al servizio
     *      
     *  Stiamo passando un oggetto sul costruttore solo per ottenere alcune statistiche alla fine 
     *  della nostra demo. Questo client richiama solo i servizi e tiene traccia di ogni esecuzione 
     *  e che ha provocato successo o errore.StatsCounter
     *  
     *  E infine, ecco il nostro programma di test. Simuliamo 1000 esecuzioni. 
     *  Non è davvero un grande numero, ma è sufficiente per ottenere alcuni dati.
     *  
     *  In circa 29 secondi, abbiamo richiamato 1000 volte e ottenuto 321 errori. 
     *  È come il 32,1% di errori per coloro che conoscono la matematica semplice 
     *  (per coloro che non si limitano a credermi sulla parola). 
     *  Questo era il risultato atteso.Service.GetMyDate
     *  
     *  Riprova, e ancora, e ancora...
     *  
     *  Se l'errore è infatti transitorio, possiamo evitarlo se riproviamo l'invocazione.
     *  Al contrario, se l'errore è causato da dati errati che inviamo erroneamente, 
     *  riprovare non ci farà bene. 
     *  È qui che distinguiamo gli errori dell'applicazione o gli errori di comunicazione. 
     *  Per semplicità, considereremo solo quello successivo.
     *  
     *  Ok, allora iniziamo la festa. Dobbiamo ripetere l'invocazione del servizio se abbiamo 
     *  qualche eccezione. Un modo per farlo è codificare la logica di ripetizione 
     *  sull'implementazione client. Ma cosa succede se otteniamo più servizi? 
     *  Cosa succede se otteniamo più azioni da chiamare? No... 
     *  Non è questa la strada da percorrere.
     *  E se potessimo mettere un pezzo trasparente tra il codice che richiama il client 
     *  e il client stesso? Beh, in realtà possiamo. Si chiama intercettore!
     *  
     *  Per usare un intercettore, prima chiamiamo un mio amico. Iniezione di dipendenza!
     *  
     *  Per il bene della lunghezza di questo articolo, non dettaglierò cosa sia DI. 
     *  Inoltre, per questo esempio, userò e pacchetti per la gestione di DI e Interception, 
     *  ma puoi facilmente passare al contenitore DI di tua scelta. 
     *  La cosa importante qui è come i pezzi lavorano insieme, non cosa li incolla.NInject 
     *  NInject.Extensions.Interception.LinFu
     *  
     *  Facciamo il refactoring del nostro e estraiamo le interfacce sui metodi. 
     *  Quindi legheremo le interfacce e le implementazioni. 
     *  Oh aspetta, ho detto interfacce?
     *  Giusto, affinché l'intercettazione funzioni al meglio, dobbiamo lavorare con 
     *  le interfacce e non con i tipi concreti stessi. 
     *  Ma questa è già la tua pratica comune, giusto?
     *  
     *  Quindi, ecco il collante:
     *  
     *  public class Module : NinjectModule
     *  {
     *      public override void Load()
     *      {
     *          Kernel.Bind<StatsCounter>().ToConstant(new StatsCounter());
     *          Kernel.Bind<INaiveClient>().To<NaiveClient>().Intercept().With<RetryInterceptor>();
     *      }
     *  }
     *  
     *  
     *  Ecco il nuovo tentativo: la classe che vediamo nel codice RetryInterceptor
     *  
     *  In termini semplici, sta invocando il fino a quando non ha successo o raggiungiamo 
     *  il massimo dei tentativi.
     *  La magia è qui. Dai documenti: invocation.Proceed()invocation.Proceed()
     *  
     *  Abbiamo anche bisogno di cambiare il nostro programma di test, per usare la magia DI.  
     *  
     *  Guarda al Main2
     *  
     *  Questa volta, abbiamo completato le 1000 invocazioni in 50 secondi, ma abbiamo avuto solo 
     *  28 errori. Questo è il tasso di errore del 2,8% (di nuovo, fidati della mia matematica). 
     *  È un numero davvero più bello del tasso di errore del 32,1% dal nostro ultimo esempio, 
     *  e "non abbiamo cambiato nulla" sul nostro client (abbiamo solo estratto un'interfaccia 
     *  da esso, che dovrebbe già essere una pratica). Abbiamo appena inserito un pezzo centrale 
     *  che estende il comportamento predefinito. Il tempo più alto può essere facilmente giustificato 
     *  da tentativi che sono stati fatti. Sono state fatte altre 417 invocazioni per realizzare 
     *  questi tentativi.
     *  
     *  Nota anche il numero di Execution Fail è leggermente più alto. 
     *  Questo anche perché stiamo facendo più invocazioni.
     *  
     *  Abbiamo già apportato le modifiche richieste quando abbiamo introdotto per la 
     *  prima volta l'intercettore.
     *  Nulla ci impedisce di aggiungere un nuovo pezzo tra e lasciare che quel pezzo 
     *  decida se abbiamo davvero bisogno di invocare il servizio o conosciamo già il 
     *  risultato e lo emettiamo subito.
     *  
     *  Per simulare una cache, ho creato un semplice provider di cache.
     *  
     *  public interface ICacheProvider
     *  {
     *      bool TryGet(object key, out object value);
     *      void Set(object key, object value);
     *  }
     *  
     *  Ho anche creato un che memorizza solo i valori su un file . Tieni presente che questa cache 
     *  non è di qualità di produzione. 
     *  È solo una prova accademica del concetto per questo articolo.PoorMansCacheProvider Hastable
     *  Ecco l'intercettore della cache:
     *  
     *   guarda Classe CacheInterceptor:IInterceptor
     *   
     *  Su di esso, sto solo generando una chiave che mi consente di differenziare le invocazioni 
     *  di azione (anche così, in questo esempio abbiamo ottenuto solo 1) e anche differenziare 
     *  le invocazioni per parametri.
     *  
     *  Se la chiave esiste, restituiamo il valore che abbiamo ottenuto in negozio. 
     *  In caso contrario, procediamo la catena di esecuzione e salviamo il risultato.
     *  
     *  La maggior parte del duro lavoro è fatto, 
     *  ma c'è ancora un dettaglio sulla configurazione dei nostri intercettori.
     *  
     *  public class Module : NinjectModule
     *  {
     *      public override void Load()
     *      {
     *          Kernel.Bind<StatsCounter>().ToConstant(new StatsCounter());
     *          Kernel.Bind<ICacheProvider>().ToConstant(new PoorMansCacheProvider());
     *          var binding = Kernel.Bind<INaiveClient>().To<NaiveClient>();
     *          binding.Intercept().With<CacheInterceptor>().InOrder(1);
     *          binding.Intercept().With<RetryInterceptor>().InOrder(2);
     *      }
     *  }
     *  
     *  Abbiamo richiamato il servizio 1000 volte in 1,6 secondi. 
     *  Questo è un enorme aumento delle prestazioni. 
     *  Per quanto riguarda i numeri risultanti:
     *      tasso di errore dello 0,15%. Stiamo aumentando le prestazioni e nel processo 
     *      abbiamo aumentato la resilienza. Non avendo bisogno di invocare il servizio 
     *      così tante volte, abbiamo aumentato la resilienza di un ordine di grandezza 
     *      significativo.
     *      
     *  Notare l'esito positivo dell'esecuzione: 30. Questo 30 è le diverse invocazioni 
     *  che abbiamo. Per il lettore più acuto, stiamo invocando il servizio in questo modo:
     *  
     *  client.GetMyDate(DateTime.Today.AddDays(i % 30));
     *  
     *  Ciò fa sì che la cache memorizzi 30 valori diversi, quindi le 30 chiamate riuscite.
     *  Per raggiungere quei 30 successi, abbiamo dovuto chiamare il servizio 45 volte.
     *  1 volta su quei 1000, non siamo riusciti a ottenere alcuna risposta su nessuno 
     *  dei tentativi prima che il risultato fosse memorizzato nella cache.
     *  
     *  Conclusione
     *  Non so voi, ma questo è un modo davvero pulito per migliorare le prestazioni di 
     *  un'applicazione.
     *  La parte fantastica di questa serie è che non abbiamo dovuto scherzare 
     *  sull'implementazione del client. Abbiamo estratto un'interfaccia, ma questo è tutto, 
     *  e dovrebbe essere già il tuo modo di fare le cose.
     *  
     *  
     *  Come abbiamo visto in questo esempio passo passo viene definito questo middleware che
     *  usa il contesto del servizio per anteporre concretamente un thread per fare
     *  cose e monitorarle nel servizio in execuzione, quindi abbiamo un componente isolato
     *  e separato dal resto del programma, ma si interpone nella sezione middleware del 
     *  servizio in esecuaione.
     *  
     *  
     */

    // Classe simula servizio difettoso
    public class Service
    {
        private static readonly Random Rand = new Random();

        public string GetMyDate(DateTime dateTime)
        {
            //Some work
            Thread.Sleep(2);

            if (Rand.NextDouble() <= .3)
            {
                throw new Exception("Fault!");
            }
            return string.Format("My date is {0}", dateTime);
        }
    }

    // Classe client Consumer
    public class NaiveClient
    {
        private StatsCounter _counter;

        public NaiveClient(StatsCounter counter)
        {
            _counter = counter;
        }

        public string GetMyDate(DateTime date)
        {
            var faultService = new Service();
            try
            {
                _counter.TotalExecutions++;
                var mydate = faultService.GetMyDate(date);
                _counter.ExecutionSuccess++;
                return mydate;
            }
            catch (Exception)
            {
                _counter.ExecutionError++;
                throw;
            }
        }
    }

    // Fake class Guardare l'esempio in allegato per il test completo
    public class watchFake
    {
        public void Start() { }
        public void Stop() { }
    }

    // Fake class Guardare l'esempio in allegato per il test completo
    public class StatsCounter
    {
        public int TotalExecutions;
        public int ExecutionError;
        public int ExecutionSuccess;

        public watchFake Stopwatch { get; internal set; }
        public int TotalSuccess { get; internal set; }
        public int TotalError { get; internal set; }

        internal void PrintStats()
        {
            throw new NotImplementedException();
        }
    }

    // Class RetryInterceptor 
    /*
    public class RetryInterceptor : IInterceptor
    {
        private readonly StatsCounter _counter;
        private const int Tries = 3;

        public RetryInterceptor(StatsCounter counter)
        {
            _counter = counter;
        }

        public void Intercept(IInvocation invocation)
        {
            var tryNumber = 0;
            do
            {
                try
                {
                    invocation.Proceed();
                    return;
                }
                catch (Exception ex)
                {
                    tryNumber++;
                    if (tryNumber == Tries)
                    {
                        throw;
                    }
                }
            } while (true);
        }
    }
    */

    // Classe cache interceptor
    /*
    public class CacheInterceptor : IInterceptor
    {
        private readonly ICacheProvider _cacheProvider;

        public CacheInterceptor(ICacheProvider cacheProvider)
        {
            _cacheProvider = cacheProvider;
        }

        public void Intercept(IInvocation invocation)
        {
            var arguments = invocation.Request.Arguments;
            var methodName = invocation.Request.Method.Name;
            // create an identifier for the cache key
            var key = methodName + "_" + string.Join("", arguments.Select(a => a ?? ""));
            object value;
            if (_cacheProvider.TryGet(key, out value))
            {
                invocation.ReturnValue = value;
                return;
            }

            invocation.Proceed();

            _cacheProvider.Set(key, invocation.ReturnValue);
        }
    }
    */

    class Program
    {
        const int TimesToInvoke = 1000;

        static void Main(string[] args)
        {
            var counter = new StatsCounter();
            var client = new NaiveClient(counter);
            counter.Stopwatch.Start();
            for (var i = 0; i < TimesToInvoke; i++)
            {
                try
                {
                    client.GetMyDate(DateTime.Today.AddDays(i % 30));
                    counter.TotalSuccess++;
                }
                catch (Exception ex)
                {
                    counter.TotalError++;
                }
            }
            counter.Stopwatch.Stop();
            counter.PrintStats();
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }

        const int TimesToInvoke = 1000;

        static void Main2(string[] args)
        {
            /*
            var kernel = new StandardKernel();
            kernel.Load(new Module());

            var counter = kernel.Get<StatsCounter>();
            var client = kernel.Get<INaiveClient>();
            counter.Stopwatch.Start();
            for (var i = 0; i < TimesToInvoke; i++)
            {
                try
                {
                    client.GetMyDate(DateTime.Today.AddDays(i % 30));
                    counter.TotalSuccess++;
                }
                catch (Exception ex)
                {
                    counter.TotalError++;
                }
            }
            counter.Stopwatch.Stop();

            counter.PrintStats();
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
            */
        }
    }
}
