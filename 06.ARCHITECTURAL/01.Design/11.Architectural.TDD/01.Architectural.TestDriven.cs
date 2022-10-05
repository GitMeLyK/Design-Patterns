using System;

namespace DotNetDesignPatternDemos.Architectural.TestDriven
{
    /*
     *  Il TDD si articola in brevi cicli che constano di tre fasi principali. 
     *  
     *      Fase rossa
     *      
     *      Nel TDD, lo sviluppo di una nuova funzionalità comincia sempre con la stesura di un 
     *      test automatico volto a validare quella funzionalità, ovvero verificare se il software 
     *      la esibisce. Poiché l'implementazione non esiste ancora, la stesura del test è 
     *      un'attività creativa, in quanto il programmatore deve stabilire in quale forma la 
     *      funzionalità verrà esibita dal software e comprenderne e definirne i dettagli. 
     *      Perché il test sia completo, deve essere eseguibile e, quando viene eseguito, 
     *      produrre un esito negativo. 
     *      
     *      In molti contesti, questo implica che debba essere realizzato una bozza minimale del 
     *      codice da testare, necessario per garantire la compilazione e l'esecuzione del test. 
     *      
     *      Una volta che il nuovo test è completo e può essere eseguito, dovrebbe fallire. 
     *      La fase rossa si conclude quando c'è un nuovo test che può essere eseguito e fallisce.
     * 
     *      Fase verde
     *      
     *      Nella fase successiva, il programmatore deve scrivere la quantità minima di codice 
     *      necessaria per passare il test che fallisce. Non è richiesto che il codice scritto 
     *      sia di buona qualità, elegante, o generale; l'unico obiettivo esplicito è che funzioni, 
     *      ovvero passi il test. In effetti, è esplicitamente vietato dalla pratica del TDD lo 
     *      sviluppo di parti di codice non strettamente finalizzate al superamento del test. 
     *      Quando il codice è pronto, il programmatore esegue nuovamente tutti i test disponibili 
     *      sul software modificato (non solo quello che precedentemente falliva). 
     *      
     *      In questo modo, il programmatore ha modo di rendersi conto immediatamente se la nuova 
     *      implementazione ha causato fallimenti di test preesistenti, ovvero ha causato regressioni 
     *      o peggioramenti nel codice. 
     *      
     *      La fase verde termina quando tutti i test sono vengono passati con successo.
     *      
     *      Refactoring
     *      
     *      Quando il software passa tutti i test, il programmatore dedica una certa quantità di 
     *      tempo a farne refactoring, ovvero a migliorarne la struttura attraverso un 
     *      procedimento basato su piccole modifiche controllate volte a eliminare o ridurre 
     *      difetti oggettivamente riconoscibili nella struttura interna del codice. 
     *      
     *      Esempi tipici di azioni di refactoring includono la scelta di identificatori più 
     *      espressivi, eliminazione di codice duplicato, semplificazione e razionalizzazione 
     *      dell'architettura del sorgente (p.es. in termini della sua organizzazione in classi), 
     *      e così via. La letteratura sul TDD fornisce numerose linee guida sia specifiche che 
     *      generali sul modo corretto di fare refactoring[8][9] In ogni caso, l'obiettivo del 
     *      refactoring non è quello di ottenere del codice "perfetto", ma solo di migliorarne 
     *      la struttura, secondo la cosiddetta "regola dei Boy Scout": 
     *      "lascia l'area dove ti sei accampato più pulita di come l'hai trovata". 
     *      
     *      Dopo ciascuna azione di refactoring, i test automatici vengono nuovamente eseguiti 
     *      per accertarsi che le modifiche eseguite non abbiano introdotto errori.
     *
     * Il principio fondamentale del TDD è che lo sviluppo vero e proprio deve avvenire solo allo 
     * scopo di passare un test automatico che fallisce. In particolare, questo vincolo è inteso 
     * a impedire che il programmatore sviluppi funzionalità non esplicitamente richieste, e che 
     * il programmatore introduca complessità eccessiva in un progetto, per esempio perché prevede 
     * la necessità di generalizzare l'implementazione in un futuro più o meno prossimo. 
     * 
     * In questo senso il TDD è in stretta relazione con numerosi principi della programmazione 
     * agile e dell'extreme programming, come il principio KISS (Keep It Simple, Stupid), 
     * il principio YAGNI (You aren't gonna need it), e il mandato agile di minimizzare il lavoro 
     * incompiuto.
     * 
     * I cicli TDD sono intesi come cicli di breve durata, al termine di ciascuno dei quali il 
     * programmatore ha realizzato un piccolo incremento di prodotto (con i relativi test 
     * automatici), un altro concetto tipico delle metodologie agili.
     * 
     * L'applicazione reiterata del refactoring al termine di ogni ciclo ha lo scopo di creare 
     * codice di alta qualità e buone architetture in modo incrementale, tenendo però separati 
     * l'obiettivo di costruire software funzionante (fase verde) e quello di scrivere "buon codice"
     * (fase grigia). 
     * 
     * La breve durata dei cicli TDD tende anche a favorire lo sviluppo di componenti di piccole 
     * dimensioni e ridotta complessità.
     * 
     * Vantaggi
     * 
     * L'applicazione del TDD porta in generale allo sviluppo di un numero maggiore di test, e 
     * a una maggiore copertura di test del software prodotto, rispetto alla pratica tradizionale 
     * di sviluppare i test dopo l'implementazione.
     * 
     * In parte, questo è dovuto al fatto che in contesti non TDD il management tende a spingere i 
     * programmatori a passare all'implementazione di nuove funzionalità a scapito del completamento 
     * dei test. I programmatori che usano il TDD su progetti nuovi hanno, in genere, meno necessità 
     * di usare il debugger, essendo in grado di risolvere più efficacemente eventuali errori 
     * annullando immediatamente le modifiche che li hanno causati.
     * 
     * 
     * Scrivendo i test prima del codice, si utilizza il programma prima ancora che venga 
     * realizzato. Ci si assicura, inoltre, che il codice prodotto sia testabile singolarmente. 
     * È dunque obbligatorio avere una visione precisa del modo in cui verrà utilizzato il 
     * programma prima ancora d'essere implementato. 
     * 
     * Così facendo si evitano errori concettuali durante la realizzazione dell'implementazione, 
     * senza che si siano definiti gli obiettivi. Inoltre, i test consentono agli sviluppatori 
     * di avere maggior fiducia durante il refactoring del codice, in quanto già sanno che i test 
     * funzioneranno quando richiesto; pertanto, possono permettersi di effettuare cambiamenti 
     * radicali di design, stando certi che alla fine otterranno un programma che si comporterà 
     * sempre alla stessa maniera (essendo i test sempre verificati).
     * 
     * L'uso del Test Driven Development permette non solo di costruire il programma assieme ad 
     * una serie di test di regressione automatizzabili, ma anche di stimare in maniera più 
     * precisa lo stato d'avanzamento dello sviluppo di un progetto.
     * 
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
