using System;

namespace DotNetDesignPatternDemos.Architectural.ModelViewController
{
    /*
     * Model-view-controller (MVC) è un modello architetturale software comunemente usato 
     * per lo sviluppo di interfacce utente che dividono la logica del programma correlata 
     * in tre elementi interconnessi. Questo viene fatto per separare le rappresentazioni 
     * interne delle informazioni dai modi in cui le informazioni vengono presentate e 
     * accettate dall'utente. 
     * 
     * Tradizionalmente utilizzato per le interfacce utente grafiche desktop (GUI), 
     * questo modello è diventato popolare per la progettazione di applicazioni web.  
     * I linguaggi di programmazione più diffusi hanno framework MVC che facilitano 
     * l'implementazione del modello.
     * 
     * Storia
     * 
     * Una delle intuizioni fondamentali nello sviluppo iniziale delle interfacce utente grafiche, 
     * MVC è diventato uno dei primi approcci per descrivere e implementare costrutti software in 
     * termini di responsabilità. 
     * Trygve Reenskaug ha creato MVC mentre lavorava su Smalltalk-79 come visiting scientist 
     * presso lo Xerox Palo Alto Research Center (PARC) alla fine del 1970. 
     * Voleva un modello che potesse essere utilizzato per strutturare qualsiasi programma 
     * in cui gli utenti interagiscono con un grande e contorto set di dati. 
     * Il suo design inizialmente aveva quattro parti: Model, View, Thing ed Editor. 
     * Dopo averne discusso con gli altri sviluppatori di Smalltalk, lui e il resto del 
     * gruppo hanno optato per Model, View e Controller. 
     * Nel loro progetto finale, un modello rappresenta una parte del programma in modo 
     * puramente e intuitivo. Una vista è una rappresentazione visiva di un modello, 
     * che recupera i dati dal modello per visualizzarli all'utente e passa le richieste 
     * avanti e indietro tra l'utente e il modello. Un controller è una parte organizzativa 
     * dell'interfaccia utente che dispone e coordina più viste sullo schermo e che riceve 
     * l'input dell'utente e invia i messaggi appropriati alle viste sottostanti. 
     * Questo progetto include anche un editor come un tipo specializzato di controller 
     * utilizzato per modificare una particolare vista e che viene creato tramite tale vista. 
     * Smalltalk-80 supporta una versione di MVC che si è evoluta da questa. 
     * Fornisce abstract e classi, nonché varie sottoclassi concrete di ciascuna che 
     * rappresentano diversi widget generici. In questo schema, a rappresenta un modo di 
     * visualizzare le informazioni all'utente e un modo in cui l'utente interagisce con un file . 
     * A è anche accoppiato a un oggetto modello, ma la struttura di tale oggetto è lasciata 
     * al programmatore dell'applicazione. L'ambiente Smalltalk-80 include anche un 
     * "MVC Inspector", uno strumento di sviluppo per visualizzare la struttura di un 
     * determinato modello, vista e controller affiancati. 
     * Nel 1988, un articolo su The Journal of Object Technology (JOT) di due ex 
     * dipendenti del PARC presentava MVC come un generale "paradigma e metodologia di 
     * programmazione" per gli sviluppatori di Smalltalk-80. Tuttavia, il loro schema 
     * differiva sia da Quello di Reenskaug et al. che da quello presentato dai libri 
     * di riferimento smalltalk-80. Hanno definito una vista come comprendente qualsiasi 
     * problema grafico, con un controller che è un oggetto più astratto, generalmente 
     * invisibile che riceve l'input dell'utente e interagisce con una o più viste e 
     * un solo modello. 
     * 
     * Il modello MVC si è successivamente evoluto, dando origine a varianti come modello 
     * gerarchico-view-controller (HMVC), model-view-adapter (MVA), model-view-presenter (MVP), 
     * model-view-viewmodel (MVVM) e altri che hanno adattato MVC a contesti diversi.
     * 
     * L'uso del modello MVC nelle applicazioni web è cresciuto dopo l'introduzione di 
     * WebObjects di NeXT nel 1996, che è stato originariamente scritto in Objective-C
     * (che ha preso in prestito pesantemente da Smalltalk) e ha contribuito a far rispettare 
     * i principi MVC. Successivamente, il modello MVC è diventato popolare tra gli 
     * sviluppatori Java quando WebObjects è stato portato su Java. I framework 
     * successivi per Java, come Spring (rilasciato nell'ottobre 2002), hanno continuato 
     * il forte legame tra Java e MVC.
     * 
     * Nel 2003, Martin Fowler ha pubblicato Patterns of Enterprise Application Architecture, 
     * che presentava MVC come un modello in cui un "controller di input" riceve una richiesta, 
     * invia i messaggi appropriati a un oggetto modello, prende una risposta dall'oggetto del 
     * modello e passa la risposta alla visualizzazione appropriata per la visualizzazione. 
     * Questo è vicino all'approccio adottato dal framework Ruby on Rails (agosto 2004), 
     * che prevede che il client invii richieste al server tramite una visualizzazione in-browser, 
     * in cui vengono gestite da un controller, che quindi comunica con gli oggetti modello appropriati. 
     * Il framework Django (luglio 2005, per Python) ha proposto una simile versione "MTV" 
     * (Model Template View) del modello, in cui una vista recupera i dati dai modelli e li passa 
     * ai modelli per la visualizzazione. 
     * Sia Rails che Django hanno debuttato con una forte enfasi sulla rapida implementazione, 
     * che ha aumentato la popolarità di MVC al di fuori dell'ambiente aziendale tradizionale 
     * in cui è stato a lungo popolare.
     * 
     * 
     */

/*
 * Componenti
 * 
 * Modello
 *      Componente centrale del modello. È la struttura dinamica dei dati dell'applicazione, 
 *      indipendente dall'interfaccia utente. Gestisce direttamente i dati, la logica e le 
 *      regole dell'applicazione.
 * Vista
 *      Qualsiasi rappresentazione di informazioni come un grafico, un diagramma o una tabella. 
 *      Sono possibili più visualizzazioni delle stesse informazioni, ad esempio un grafico a 
 *      barre per la gestione e una visualizzazione tabulare per i contabili.
 *      
 * Controllore
 *      Accetta l'input e lo converte in comandi per il modello o la vista. 
 *      Oltre a dividere l'applicazione in questi componenti, la progettazione modello-vista-controller definisce le interazioni tra di essi. 
 *          - Il modello è responsabile della gestione dei dati dell'applicazione.  
 *            Riceve l'input dell'utente dal controller.
 *          - La vista esegue il rendering della presentazione del modello in un formato 
 *            particolare.
 *          - Il controller risponde all'input dell'utente ed esegue interazioni sugli oggetti del 
 *            modello di dati. Il controller riceve l'input, facoltativamente lo convalida e quindi 
 *            passa l'input al modello.
 *            
 * Come con altri modelli software, MVC esprime il "nucleo della soluzione" a un problema, 
 * consentendone l'adattamento per ciascun sistema. 
 * Particolari progetti MVC possono variare in modo significativo rispetto alla descrizione 
 * tradizionale qui.
 * 
 */

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello World!");
    }
}
}
