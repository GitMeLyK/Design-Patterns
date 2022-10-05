using System;

namespace DotNetDesignPatternDemos.Architectural.Collaboration
{
    class Program
    {
        /*
         * Con metodologia agile (o sviluppo agile del software, in inglese agile software development, 
         * abbreviato in ASD), nell'ingegneria del software, si indica un insieme di metodi di sviluppo 
         * del software emersi a partire dai primi anni 2000 e fondati su un insieme di principi comuni, 
         * direttamente o indirettamente derivati dai principi del 
         * "Manifesto per lo sviluppo agile del software" 
         * (Manifesto for Agile Software Development, impropriamente chiamato anche "Manifesto Agile") 
         * pubblicato nel 2001 da Kent Beck, Robert C. Martin, Martin Fowler e altri.
         * 
         * Sinossi generale
         * 
         * I metodi agili si contrappongono al modello a cascata (waterfall model) e altri modelli 
         * di sviluppo tradizionali, proponendo un approccio meno strutturato e focalizzato 
         * sull'obiettivo di consegnare al cliente, in tempi brevi e frequentemente 
         * (early delivery / frequent delivery), software funzionante e di qualità. 
         * 
         * Fra le pratiche promosse dai metodi agili ci sono la formazione di team di sviluppo 
         * piccoli, poli-funzionali e auto-organizzati, lo sviluppo iterativo e incrementale, 
         * la pianificazione adattiva, e il coinvolgimento diretto e continuo del cliente nel 
         * processo di sviluppo.
         * 
         * La gran parte dei metodi agili tenta di ridurre il rischio di fallimento sviluppando 
         * il software in finestre di tempo limitate chiamate iterazioni che, in genere, durano 
         * qualche settimana. 
         * 
         * Ogni iterazione è un piccolo progetto a sé stante e deve contenere tutto ciò che è 
         * necessario per rilasciare un piccolo incremento nelle funzionalità del software: 
         * 
         *      pianificazione (planning), analisi dei requisiti, progettazione, 
         *      implementazione, test e documentazione.
         *      
         * Anche se il risultato di ogni singola iterazione non ha sufficienti funzionalità da 
         * essere considerato completo deve essere pubblicato e, nel susseguirsi delle iterazioni, 
         * deve avvicinarsi sempre di più alle richieste del cliente. Alla fine di ogni iterazione 
         * il team deve rivalutare le priorità di progetto.
         * 
         * I metodi agili preferiscono la comunicazione in tempo reale, preferibilmente faccia a 
         * faccia, a quella scritta (documentazione). 
         * 
         * Il team agile è composto da tutte le persone necessarie per terminare il progetto 
         * software. Come minimo il team deve includere i programmatori ed i loro clienti 
         * (con clienti si intendono le persone che definiscono come il prodotto dovrà 
         * essere fatto: 
         *      
         *      possono essere dei product manager, dei business analysts, 
         *      o dei clienti veri e propri).
         * 
         * Il manifesto
         * 
         * La formalizzazione dei principi su cui si basano le metodologie agili è stata oggetto 
         * del lavoro di un gruppo di progettisti software e guru dell'informatica che si sono 
         * spontaneamente riuniti nell'Agile Alliance. Il documento finale di questo lavoro è 
         * stato poi sottoscritto da un nutrito gruppo di questi professionisti, molti dei quali 
         * hanno anche sviluppato alcune delle metodologie agili più famose.
         * 
         * Obiettivi
         * 
         * L'obiettivo è la piena soddisfazione del cliente e non solo l'adempimento di un contratto.
         * 
         * Il corretto uso di queste metodologie, inoltre, può consentire di abbattere i costi e 
         * i tempi di sviluppo del software, aumentandone la qualità.
         * 
         * Essa è esplosa proprio in concomitanza con la crisi successiva al boom di Internet 
         * prendendo spunto dai metodi applicati in piccole software house.
         * 
         * Valori
         * 
         * I valori su cui si basa una metodologia agile che segua i punti indicati dal Manifesto 
         * Agile sono quattro.
         * 
         *      Si ritengono importanti:
         *      
         *          Gli individui e le interazioni più che i processi e gli strumenti
         *          Il software funzionante più che la documentazione esaustiva
         *          La collaborazione col cliente più che la negoziazione dei contratti
         *          Rispondere al cambiamento più che seguire un piano
         *          
         * Ovvero, fermo restando il valore delle voci a destra, si considerano più importanti 
         * le voci a sinistra.
         * 
         * Pratiche
         * 
         *  Le singole pratiche applicabili all'interno di una metodologia agile sono decine e 
         *  dipendono essenzialmente dalle necessità dell'azienda e dall'approccio del project 
         *  manager. 
         *  
         *  Nella scelta però bisogna tenere conto delle caratteristiche di ogni pratica per i 
         *  benefici che apporta e le conseguenze che comporta. Ad esempio, in Extreme Programming, 
         *  si supplisce alla mancanza assoluta di qualsiasi forma di progettazione e documentazione 
         *  con lo strettissimo coinvolgimento del cliente nello sviluppo e con la programmazione in 
         *  coppia.
         *  
         *  Le pratiche più diffuse tra cui scegliere sono simili fra di loro e possono essere 
         *  raggruppate in categorie:
         *  
         *  Automazione - 
         *      Se l'obiettivo delle metodologie agili è concentrarsi sulla programmazione senza 
         *      dedicarsi alle attività collaterali, allora queste ultime possono essere eliminate 
         *      o automatizzate; la seconda soluzione è migliore perché si può, ad esempio, 
         *      eliminare la documentazione aumentando il testing, ma non si possono eliminare 
         *      entrambe; quindi si sceglie che strada si vuole percorrere e si fa in modo di 
         *      utilizzare strumenti per automatizzare il maggior numero possibile di attività 
         *      collaterali;
         *  
         *  Coinvolgimento del cliente - 
         *      Il coinvolgimento del cliente è qui indicato singolarmente perché ci sono differenti 
         *      gradi di coinvolgimento possibili; ad esempio in Extreme Programming il coinvolgimento 
         *      è totale, il cliente partecipa persino alle riunioni settimanali dei programmatori; 
         *      in altri casi, il cliente è coinvolto in una prima fase di progettazione e poi non più; 
         *      in altri ancora il cliente partecipa indirettamente e viene usato come tester della 
         *      versione rilasciata;
         *  
         *  Comunicazione stretta - 
         *      Secondo Alistair Cockburn, probabilmente il primo teorico delle metodologie agili, 
         *      questo è l'unico vero aspetto nodale che rende agile una metodologia. 
         *      Per comunicazione stretta si intende la comunicazione interpersonale, fra tutti gli 
         *      attori del progetto, cliente compreso. Ciò serve ad avere una buona analisi dei 
         *      requisiti ed una proficua collaborazione fra programmatori anche in un ambito di 
         *      quasi totale assenza di documentazione;
         *  
         *  Consegne frequenti - 
         *      Effettuare rilasci frequenti di versioni intermedie del software permette di 
         *      ottenere più risultati contemporaneamente: si ricomincia l'iterazione avendo già a 
         *      disposizione un blocco di codice funzionante in tutti i suoi aspetti, si offre al 
         *      cliente "qualcosa con cui lavorare" e lo si distrae così da eventuali ritardi nella 
         *      consegna del progetto completo, si usa il cliente come se fosse un test visto che 
         *      utilizzerà il software e riscontrerà eventuali anomalie, si ottengono dal cliente 
         *      informazioni più precise sui requisiti che probabilmente non sarebbe riuscito ad 
         *      esprimere senza avere a disposizione utilità e carenze del progetto;
         *  
         *  Cultura di Team (One-team culture) - 
         *      Fondamentale nel seguire approcci Agili è la collaborazione e l'approccio mentale e pratico 
         *      del team di sviluppo stesso. Il criterio di lavoro più adatto sarebbe quello di abbandonare 
         *      la tradizionale blaming culture (che prevede la penalizzazione o la premiazione del singolo 
         *      individuo che commetta un errore, oppure che si distinguesse dagli altri per meriti) 
         *      orientandosi invece verso un modus operandi 'di gruppo', in trasparenza ed onestà, 
         *      che andrà a premiare (o viceversa) il gruppo stesso unicamente sulla base del raggiungimento 
         *      degli obiettivi di team (previsti per quell'intervallo temporale);
         *  
         *  Facilitated Workshop - 
         *      Una pratica a supporto dei principi di comunicazione e collaborazione, unitamente al 
         *      mantenimento del focus sugli obiettivi di business. 
         *      Questa tecnica consiste nel prevedere incontri (workshop) facilitati durante il progetto: 
         *      la presenza di un facilitatore neutrale (facilitator) garantirà il successo del meeting, 
         *      mantenendo costantemente l'incontro in linea con i suoi obiettivi, mantenendo il contesto 
         *      adatto (libertà di parola, assenza di pressioni tra i partecipanti, decisioni non forzate, 
         *      ecc.), e garantendo che vengano trasmesse a tutte le parti interessate tutte le 
         *      informazioni necessarie sia precedenti che derivanti (follow-up) dal workshop stesso;
         *  
         *  Formazione di una squadra e Proprietà del codice - 
         *      La formazione del team di sviluppo è condizionata dalla scelta sulla gerarchia interna, 
         *      ma segue regole precise che permettono di ottenere un team produttivo nell'ambito della 
         *      metodologia scelta; la scelta dei membri del team è condizionata anche alla scelta della 
         *      proprietà del codice, che può essere individuale o collettiva; 
         *      nel primo caso la responsabilità sullo sviluppo è individuale, nel secondo dipende da 
         *      tutto il team e quindi dal project manager;
         *  
         *  Gerarchia - 
         *      La scelta di creare una struttura gerarchica all'interno del team di sviluppo dipende molto 
         *      dall'approccio del project manager, in ogni caso si ha una conseguenza non secondaria facendo 
         *      questa scelta; se si decide per una struttura gerarchica ad albero e frammentata si ottiene 
         *      la possibilità di gestire un numero molto alto di programmatori e di lavorare a diversi 
         *      aspetti del progetto parallelamente; se si decide per una totale assenza di gerarchia si 
         *      avrà un team di sviluppo molto compatto e motivato, ma necessariamente piccolo in termini 
         *      di numero di programmatori;
         *  
         *  Iterative development - 
         *      Un'importante pratica attraverso la quale la soluzione da consegnare si evolve da quella 
         *      che era soltanto "un'idea" (un concetto, una proposta, un insieme di esigenze) fino a 
         *      divenire un prodotto di valore per il cliente. 
         *      L'Iterative development funziona attraverso cicli di azioni/attività che non cambiano, 
         *      ma che ripetendosi ciclicamente portano la soluzione 'grezza' a raffinarsi fino a 
         *      diventare il prodotto finale;
         *  
         *  Miglioramento della conoscenza - 
         *      Nata con l'avvento della programmazione Object-Oriented, non è altro che la presa di 
         *      coscienza della produzione di conoscenza che si fa in un'azienda man mano che si 
         *      produce codice; questa conoscenza prodotta non deve andare perduta ed è per far ciò 
         *      che si sfruttano spesso le altre pratiche, come la comunicazione stretta o la condivisione 
         *      della proprietà del codice;
         *  
         *  Modellizzazione - 
         *      L'utilizzo di modelli, rappresentazioni visuali di problemi o soluzioni, tracciati, 
         *      prototipi o modelli in scala (in generale) supporta le metodologie agili;
         *  
         *  Pair programming - 
         *      Lo sviluppo viene fatto da coppie di programmatori che si alternano alla tastiera;
         *  
         *  Prioritization - 
         *      Lo sviluppo della soluzione può cominciare solo dopo aver messo in priorità gli obiettivi, 
         *      dai quali deriveranno i requirements e le features (caratteristiche o funzionalità del 
         *      prodotto) da consegnare per mezzo del progetto; una pratica ben nota è la tecnica 
         *      MoSCoW (Must - Should - Could - Won't have);
         *  
         *  Progettazione e documentazione - 
         *      Pensare che le metodologie leggere eliminino la progettazione e la documentazione 
         *      è un errore, in effetti non è così, le metodologie leggere introducono un'iterazione 
         *      nel ciclo di vita del progetto; quanta progettazione fare e quanta documentazione 
         *      produrre, escludendo i casi estremi, è una scelta lasciata a chi gestisce il 
         *      progetto e spesso i teorici dell'Agile Alliance avvisano che è un errore trascurare 
         *      o addirittura omettere queste due fasi;
         *  
         *  Refactoring - 
         *      La ristrutturazione di parti di codice mantenendone invariato l'aspetto e il 
         *      comportamento esterno;
         *  
         *  Retroingegneria - 
         *      Ossia ottenere, spesso in maniera automatica, la documentazione a partire dal codice 
         *      già prodotto; è una delle pratiche più diffuse e più controverse, diffusa perché 
         *      permette un guadagno enorme in termini di tempo, ma controversa perché spesso la 
         *      documentazione prodotta è inutilizzabile oppure è prodotta solo per una richiesta 
         *      burocratica del cliente e non verrà mai realmente utilizzata;
         *  
         *  Semplicità - 
         *      Uno dei punti chiave delle metodologie leggere, direttamente mutuato dalla programmazione 
         *      Object-Oriented, è la semplicità; semplicità nel codice, semplicità nella documentazione, 
         *      semplicità nella progettazione, semplicità nella modellazione; i risultati così ottenuti 
         *      sono una migliore leggibilità dell'intero progetto ed una conseguente facilitazione nelle 
         *      fasi di correzione e modifica;
         *      
         *  Test - 
         *      Pratica diffusissima anche prima della nascita delle metodologie leggere, ha 
         *      prodotto una letteratura vastissima ed una serie di approcci differenti come il 
         *      Rapid Testing o il Pair Testing; nell'ambito delle metodologie leggere vengono 
         *      spesso utilizzati insieme tre tipi di test differenti: i test funzionali, utilizzati 
         *      per verificare che il software faccia effettivamente ciò che è previsto debba fare, 
         *      i test unitari, utilizzati per verificare che ogni pezzo di codice funzioni 
         *      correttamente, e i test indiretti effettuati inconsciamente dal cliente ogni volta 
         *      che gli si consegna una versione;
         *  
         *  Test Driven Development - 
         *      Una tipologia di approccio al Testing da eseguire durante il nostro progetto, che prevede 
         *      la scelta e definizione dei veri e propri test che dovranno essere superati dal prodotto 
         *      (cioè dalla soluzione e dalle sue features) prima di andare a sviluppare la soluzione 
         *      stessa. Il concetto, in poche parole, è molto semplice: si vuole evitare il rischio di 
         *      andare a sviluppare qualcosa che poi non si riesce a testare;
         *  
         *  Timeboxing - 
         *      Pratica fondamentale dei metodi Agile che consiste nel suddividere il progetto in 
         *      intervalli temporali ben precisi, della durata di pochi giorni o settimane 
         *      -ad esempio gli Sprint (SCRUM) o le Structured Timebox (AGILEPM)- 
         *      entro i quali consegnare delle features, parallelamente ad intervalli temporali 
         *      di durata superiore (settimane o mesi) chiamati Incrementi, alla fine dei quali 
         *      avviene la vera e propria consegna della soluzione finale, o di una parte della stessa, 
         *      utilizzabile effettivamente dal cliente (che ne trarrà il valore aspettato);
         *      
         *  Versioning (Controllo della versione) - 
         *      Una delle conseguenze dirette dell'iterazione nella produzione è la necessità di 
         *      introdurre un modello, un metodo, uno strumento, per il controllo delle versioni 
         *      del software prodotto e rilasciato; uno degli strumenti più diffusi e maggiormente 
         *      suggeriti per ottemperare automaticamente a questa pratica è il CVS.
         *      
         *  Metodologie
         *  
         *  In senso lato il termine "agile" indica tutte quelle metodologie di sviluppo leggere 
         *  e flessibili, che rompono con la precedente tradizione di ingegneria del software 
         *  (modello a cascata, modello a spirale, etc.) basata su una raccolta delle specifiche 
         *  e su una strutturazione sequenziale dello sviluppo software. 
         *  
         *  Le metodologie agili consentono invece di rivedere di continuo le specifiche 
         *  adeguandole durante l'avanzamento dello sviluppo del software, mediante un 
         *  framework iterativo e incrementale, e un forte scambio di informazioni e di 
         *  pareri tra gli sviluppatori e con il committente. 
         *  
         *  Esempi di metodologie e framework agili:
         *  
         *      Agile Unified Process,
         *      Adaptive Software Development,
         *      Crystal (famiglia),
         *      Dynamic Systems Development Method,
         *      Extreme programming,
         *      Feature Driven Development,
         *      Lean software development,
         *      Scrum.
         *      
        */

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
