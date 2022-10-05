using System;

namespace DotNetDesignPatternDemos.Architectural.Development
{
    /*
     * In uno sviluppo di progettazione per un ambiente di sviluppatori l'architettura dei
     * metodi di sviluppo non basta, cioè non portare un team o interi team ad essere 
     * collaborativi e coordinati tra di loro può portare sicuramente a un disastro dell'impianto
     * stesso. 
     * 
     * Creare processi aziendali che coordino questi gruppi e ogni singola persona a usare
     * processi di automazione per i repository del codice i test i deploy e lasciare ognuno a
     * libero arbitrio di fare questi tipi di operazioni non è una buona forma architetturale
     * da adottare.
     * 
     * Ecco quindi che entrare nella mentalità di questo modello di architettura che definisce 
     * i cicli e le misure da usare per lo sviluppo e messa in produzione e anche le sue future
     * evoluioni ci porta a questa metodologia DevOps.
     * 
     * Vantaggi di DevOps
     * 
     * I team che adottano la cultura, le procedure e gli strumenti DevOps ottengono prestazioni 
     * elevate e creano più rapidamente prodotti, incrementando la soddisfazione dei clienti. 
     * 
     * I miglioramenti a livello di collaborazione e produttività sono essenziali anche per 
     * raggiungere obiettivi aziendali come questi:
     * 
     *      Accelerazione del time-to-market
     *      Adattamento al mercato e alla competizione
     *      Conservazione della stabilità e dell'affidabilità del sistema
     *      Miglioramento del tempo medio per il ripristino
     * 
     * DevOps e il ciclo di vita dell'applicazione
     * 
     * DevOps influenza il ciclo di vita dell'applicazione nelle fasi di pianificazione, 
     * sviluppo, distribuzione e operatività. 
     * 
     * Ogni fase è basata sulle altre e nessuna fase è assegnata a un ruolo specifico. 
     * 
     * In una cultura DevOps effettiva ogni ruolo è coinvolto in qualche misura in ogni fase.
     * 
     *      // Figura 1 : lifecycle.png
     *      
     * Piano
     * 
     *      Durante la fase di pianificazione i team DevOps concepiscono, definiscono e descrivono le 
     *      funzionalità delle applicazioni e dei sistemi da creare. 
     *      Tengono traccia dell'avanzamento a livelli ridotti ed elevati di granularità, dalle 
     *      attività di un singolo prodotto alle attività relative a portfolio o a più prodotti. 
     *      
     *      La creazione di backlog, la verifica dei bug, la gestione di Agile Software Development 
     *      con Scrum, l'uso delle lavagne Kanban e la visualizzazione dello stato con i dashboard 
     *      sono alcuni dei modi in cui i team DevOps pianificano con flessibilità e visibilità.
     * 
     * Sviluppare
     * 
     *      La fase di sviluppo include tutti gli aspetti della codifica, tra cui scrittura, test, 
     *      revisione e integrazione del codice da parte dei membri del team, oltre all'inserimento 
     *      del codice in artefatti della compilazione che possono essere distribuiti in diversi 
     *      ambienti. 
     *      
     *      I team DevOps si impegnano per innovare rapidamente senza sacrificare la qualità, la 
     *      stabilità e la produttività. 
     *      
     *      A tale scopo usano strumenti a produttività elevata, automatizzano i passaggi 
     *      ripetitivi e manuali ed eseguono l'iterazione in piccoli incrementi tramite test 
     *      automatizzati e integrazione continua.
     *      
     * Distribuzione
     * 
     *      Il recapito è il processo di distribuzione di applicazioni negli ambienti di produzione 
     *      in modo coerente e affidabile. 
     *      
     *      La fase di recapito include anche la distribuzione e la configurazione dell'infrastruttura 
     *      di base completamente regolamentata che costituisce tali ambienti.
     *      
     *      Durante la fase di recapito i team definiscono un processo di gestione del rilascio 
     *      con fasi di approvazione manuale chiare. 
     *      
     *      I team configurano anche attività di controllo automatizzate che spostano le applicazioni 
     *      da una fase all'altra fino alla disponibilità per i clienti. 
     *      
     *      L'automazione di questi processi li rende scalabili, ripetibili e controllati. 
     *      In questo modo i team che adottano DevOps possono eseguire frequentemente la distribuzione 
     *      con facilità e in tutta sicurezza.
     *      
     * Operazioni
     * 
     *      La fase operativa prevede la manutenzione, il monitoraggio e la risoluzione dei 
     *      problemi delle operazioni negli ambienti di produzione. 
     *      
     *      Durante l'adozione delle procedure DevOps i team si impegnano per assicurare 
     *      l'affidabilità del sistema e la disponibilità elevata e cercano di ridurre a zero 
     *      il tempo di inattività, rafforzando al tempo stesso la sicurezza e la governance. 
     *      
     *      I team DevOps cercano di identificare i problemi prima che influiscano sull'esperienza 
     *      dei clienti e di attenuare rapidamente i problemi quando si verificano. 
     *      
     *      Questo livello di vigilanza richiede telemetria avanzata, avvisi di utilità pratica e 
     *      visibilità completa delle applicazioni e del sistema sottostante.
     * 
     * Cultura DevOps
     * 
     *  Benché l'adozione delle procedure DevOps automatizzi e ottimizzi i processi tramite la 
     *  tecnologia, tutto è basato sulla cultura interna dell'organizzazione e sulle persone che 
     *  contribuiscono al lavoro. 
     *  
     *  Per stabilire una cultura DevOps sono necessarie profonde modifiche al modo in cui per 
     *  persone lavorano e collaborano. 
     *  
     *  Quando le organizzazioni scelgono di adottare una cultura DevOps possono tuttavia creare 
     *  l'ambiente per lo sviluppo di team a prestazioni elevate.
     *  
     *  - Collaborazione, visibilità e allineamento
     *  
     *      Uno degli aspetti fondamentali di una cultura DevOps ottimale è costituito dalla 
     *      collaborazione tra team, che inizia dalla visibilità. 
     *      
     *      Diversi team, ad esempio quelli addetti a sviluppo e operazioni IT, devono condividere 
     *      i rispettivi processi DevOps, le priorità e le preoccupazioni. 
     *      
     *      I team devono inoltre pianificare insieme il lavoro e allineare obiettivi e indicatori 
     *      di successo correlati al business.
     *      
     *   - Cambiamenti a livello di ambito e responsabilità
     *   
     *      Grazie all'allineamento, i team acquisiscono la proprietà e vengono coinvolti in fasi 
     *      aggiuntive del ciclo di vita, non solo nelle fasi centrali per i rispettivi ruoli. 
     *      
     *      Gli sviluppatori diventano ad esempio responsabili non solo dell'innovazione e della 
     *      qualità della fase di sviluppo, ma anche delle prestazioni e della stabilità offerte 
     *      dalle loro modifiche nella fase operativa. Al tempo stesso i responsabili delle 
     *      operazioni IT devono includere governance, sicurezza e conformità nella fase di 
     *      pianificazione e sviluppo.
     *      
     *   - Cicli di rilascio più brevi
     *   
     *      I team DevOps rimangono flessibili rilasciando software in brevi cicli. I cicli di 
     *      rilascio più brevi semplificano la pianificazione e la gestione dei rischi perché 
     *      il progresso è incrementale e ciò riduce anche l'impatto sulla stabilità del sistema. 
     *      
     *      La riduzione del ciclo di rilascio permette inoltre alle organizzazioni di adattarsi 
     *      e reagire all'evoluzione delle esigenze dei clienti e alla pressione della competizione.
     *      
     *   - Apprendimento continuo
     *   
     *      I team DevOps a prestazioni elevate si concentrano sulla crescita. Falliscono e 
     *      rispondono immediatamente agli errori e incorporano le lezioni apprese nei processi, 
     *      migliorando continuamente, incrementando la soddisfazione dei clienti e accelerando 
     *      l'innovazione e l'adattabilità al mercato. 
     *      
     *      DevOps è un percorso e favorisce quindi una crescita continua.
     *      
     *  Questo è il manifesto completo di questa metodologia Devops.
     */

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
