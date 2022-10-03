using System;

namespace DotNetDesignPatternDemos.Architectural.ModelViewPresentation
{
    /*
     * Il modello software model-view-presenter ha avuto origine nei primi anni 1990 presso 
     * Taligent, una joint venture di Apple, IBM e Hewlett-Packard. 
     * MVP è il modello di programmazione sottostante per lo sviluppo di applicazioni 
     * nell'ambiente CommonPoint basato su C++ di Taligent. 
     * Il modello è stato successivamente migrato da Taligent a Java e reso popolare 
     * in un documento di Taligent CTO Mike Potel. 
     * Dopo l'interruzione di Taligent nel 1998, Andy Bower e Blair McGlashan di Dolphin 
     * Smalltalk hanno adattato il modello MVP per formare la base per il loro framework 
     * di interfaccia utente Smalltalk. [4] 
     * Nel 2006, Microsoft ha iniziato a incorporare MVP nella documentazione e 
     * negli esempi per la programmazione dell'interfaccia utente in .NET Framework. 
     * 
     * L'evoluzione e le molteplici varianti del modello MVP, inclusa la relazione 
     * di MVP con altri modelli di progettazione come MVC, sono discusse in dettaglio 
     * in un articolo di Martin Fowler e un altro di Derek Greer. 
     * 
     * MVP è un modello architetturale dell'interfaccia utente progettato per facilitare gli 
     * unit test automatizzati e migliorare la separazione dei problemi nella logica di presentazione:
     *  - Il modello è un'interfaccia che definisce i dati da visualizzare o altrimenti agire 
     *    nell'interfaccia utente.
     *  - La visualizzazione è un'interfaccia passiva che visualizza i dati (il modello) 
     *    e instrada i comandi utente (eventi) al relatore per agire su tali dati.
     *  - Il relatore agisce sul modello e sulla vista. Recupera i dati dai 
     *     repository (il modello) e li formatta per la visualizzazione nella vista.
     *  In genere, l'implementazione della visualizzazione crea un'istanza dell'oggetto relatore 
     *  concreto, fornendo un riferimento a se stesso. Nel codice C# seguente viene illustrato 
     *  un semplice costruttore di visualizzazione, in cui ConcreteDomainPresenter implementa 
     *  l'interfaccia IDomainPresenter.
     *  
     *  Il grado di logica consentito nella vista varia tra le diverse implementazioni. 
     *  Ad un estremo, la vista è interamente passiva, inoltrando tutte le operazioni di 
     *  interazione al relatore. In questa formulazione, quando un utente attiva un metodo 
     *  evento della vista, non fa altro che richiamare un metodo del relatore che non ha 
     *  parametri e nessun valore restituito. Il relatore recupera quindi i dati dalla vista 
     *  tramite metodi definiti dall'interfaccia di visualizzazione. 
     *  Infine, il relatore opera sul modello e aggiorna la vista con i risultati dell'operazione. 
     *  Altre versioni di model-view-presenter consentono una certa libertà rispetto alla classe 
     *  che gestisce una particolare interazione, evento o comando. 
     *  Questo è spesso più adatto per le architetture basate sul Web, in cui la vista, che 
     *  viene eseguita sul browser di un client, può essere il posto migliore per gestire una 
     *  particolare interazione o comando.
     *  
     *  Da un punto di vista di layering, la classe presenter potrebbe essere considerata 
     *  come appartenente al livello dell'applicazione in un sistema di architettura 
     *  multilivello, ma può anche essere vista come un proprio livello di presentazione 
     *  tra il livello dell'applicazione e il livello dell'interfaccia utente.
     *  
     *  L'ambiente .NET supporta il modello MVP in modo molto simile a qualsiasi altro ambiente 
     *  di sviluppo. Lo stesso modello e la stessa classe presenter possono essere utilizzati 
     *  per supportare più interfacce, ad esempio un'applicazione Web ASP.NET, un'applicazione 
     *  Windows Form o un'applicazione Silverlight. Il relatore ottiene e imposta le informazioni 
     *  da/verso la vista tramite un'interfaccia a cui può accedere il componente dell'interfaccia 
     *  (vista).
     *  
     *  Oltre a implementare manualmente il modello, è possibile utilizzare un framework 
     *  model-view-presenter per supportare il modello MVP in modo più automatizzato.
     */

    public interface IDomainView{ }

    internal interface IDomainPresenter{}

    public class ConcreteDomainPresenter: IDomainPresenter
    {
        private DomainView domainView;

        public ConcreteDomainPresenter(DomainView domainView)
        {
            this.domainView = domainView;
        }
    }

    public class DomainView : IDomainView
    {
        private IDomainPresenter _domainPresenter = null;

        /// <summary>Constructor.</summary>
        public DomainView()
        {
            _domainPresenter = new ConcreteDomainPresenter(this);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
