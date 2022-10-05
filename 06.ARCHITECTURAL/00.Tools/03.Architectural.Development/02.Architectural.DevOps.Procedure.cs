using System;

namespace DotNetDesignPatternDemos.Architectural.Development.Procedure
{

    /*
     *  Oltre a stabilire una cultura DevOps, i team applicano l'approccio DevOps implementando alcune 
     *  procedure nell'intero ciclo di vita dell'applicazione. 
     *  
     *  Alcune procedure contribuiscono all'accelerazione, all'automazione e al miglioramento di una 
     *  fase specifica. Altre procedure sono relative a più fasi e aiutano i team a creare processi 
     *  semplici che contribuiscono al miglioramento della produttività. 
     *
     *  -- Controllo della versione
     *  
     *  Il controllo della versione consiste nella procedura di gestione del codice in versioni 
     *  diverse, ovvero nel controllo delle revisioni e della cronologia delle modifiche per 
     *  semplificare la revisione e il ripristino del codice. 
     *  
     *  Questo approccio viene in genere implementato con sistemi di controllo della versione, 
     *  ad esempio Git, che consentono a più sviluppatori di collaborare alla creazione del codice. 
     *  
     *  Questi sistemi offrono un processo chiaro per l'unione delle modifiche al codice 
     *  apportate negli stessi file, la gestione dei conflitti e il rollback delle modifiche 
     *  a stati precedenti.
     *  
     *  L'uso del controllo della versione è una procedura fondamentale per DevOps e aiuta i team 
     *  di sviluppo a collaborare, dividere le attività di codifica tra i membri del team e 
     *  archiviare tutto il codice per semplificarne il ripristino in caso di necessità.
     *  
     *  Il controllo della versione è anche un elemento necessario in altri approcci, ad 
     *  esempio l'integrazione continua e l'infrastruttura come codice.
     *  
     *  - Agile Software Development
     *  
     *  Agile è un approccio per lo sviluppo software che enfatizza la collaborazione tra team, 
     *  il feedback da clienti e utenti e una capacità elevata di adattamento alle modifiche 
     *  tramite brevi cicli di rilascio. 
     *  
     *  I team che applicano l'approccio Agile forniscono modifiche e miglioramenti continui 
     *  ai clienti, raccolgono il rispettivo feedback, quindi apprendono e apportano modifiche 
     *  in base ai desideri e alle esigenze dei clienti. 
     *  
     *  Agile è significativamente diverso da altri framework più tradizionali come la metodologia 
     *  a cascata, che include lunghi cicli di rilascio definiti da fasi sequenziali. 
     *  
     *  Kanban e Scrum sono due framework molto diffusi associati ad Agile.
     *  
     *  - Infrastruttura come codice
     *  
     *  L'infrastruttura come codice definisce le risorse e le topologie del sistema in un modo 
     *  descrittivo che consente ai team di gestire tali risorse con lo stesso approccio usato 
     *  per il codice. 
     *  
     *  Queste definizioni possono essere anche archiviate e sottoposte a controllo della versione 
     *  in sistemi di controllo della versione, dove possono essere revisionati e ripristinati, 
     *  esattamente come il codice.
     *  
     *  L'uso dell'infrastruttura come codice aiuta i team a distribuire risorse di sistema in 
     *  modo affidabile, ripetibile e controllato. 
     *  
     *  L'infrastruttura come codice contribuisce anche all'automazione del codice e riduce il 
     *  rischio di errore umano, in particolare per ambienti complessi di grandi dimensioni. 
     *  
     *  Questa soluzione ripetibile e affidabile per la distribuzione di ambienti permette ai 
     *  team di mantenere ambienti di sviluppo e test identici a quelli di produzione. 
     *  
     *  Anche la duplicazione degli ambienti in data center diversi e piattaforme cloud 
     *  diverse risulta più semplice e più efficiente.
     *  
     *  - Gestione della configurazione
     *  
     *  Per gestione della configurazione si intende la gestione dello stato delle risorse in 
     *  un sistema, inclusi server, macchine virtuali e database. 
     *  
     *  Grazie agli strumenti di gestione della configurazione, i team possono implementare 
     *  modifiche in modo controllato e sistematico, riducendo il rischio di modifiche alla 
     *  configurazione del sistema. 
     *  
     *  I team usano gli strumenti di gestione della configurazione per tenere traccia dello 
     *  stato del sistema ed evitare le deviazioni dalla configurazione, ovvero la deviazione 
     *  della configurazione di una risorsa del sistema nel tempo rispetto allo stato desiderato 
     *  definito in modo specifico per tale risorsa.
     *  
     *  In combinazione con l'infrastruttura come codice, la definizione e la configurazione del 
     *  sistema sono facili da modellizzare e automatizzare, permettendo ai team di gestire 
     *  ambienti complessi su larga scala.
     *  
     *  - Monitoraggio continuo
     *  
     *  Per monitoraggio continuo si intende la visibilità completa in tempo reale delle 
     *  prestazioni e dell'integrità dell'intero stack di applicazioni, dall'infrastruttura 
     *  sottostante che esegue l'applicazione ai componenti software di livello superiore. 
     *  
     *  La visibilità è costituita dalla raccolta di telemetria e metadati, oltre alla configurazione 
     *  di avvisi per condizioni predefinite che richiedono l'attenzione di un operatore. 
     *  
     *  La telemetria comprende dati sugli eventi e log raccolti da diverse parti del sistema e 
     *  archiviati in posizioni in cui è possibile analizzarli e sottoporli a query.
     *  
     *  I team DevOps a prestazioni elevate si assicurano di configurare avvisi significativi di 
     *  utilità pratica e di raccogliere dati di telemetria avanzati, per poter ottenere 
     *  informazioni dettagliate da quantità elevatissime di dati. 
     *  
     *  Queste informazioni dettagliate aiutano i team ad attenuare i problemi in tempo reale e 
     *  a scoprire come migliorare l'applicazione nei cicli di sviluppo futuri.
     *
     *  Tools
     *  
     *  I team hanno a disposizione molti strumenti DevOps per favorire una cultura DevOps 
     *  nell'organizzazione. La maggior parte dei team si affida ad alcuni strumenti, creando 
     *  toolchain ottimali per le proprie esigenze per ogni fase del ciclo di vita 
     *  dell'applicazione. Benché l'adozione di uno strumento specifico o una tecnologia 
     *  non corrisponda all'adozione di DevOps, quando la cultura DevOps è presente e i 
     *  processi sono definiti, le persone possono implementare e semplificare le procedure DevOps 
     *  se scelgono gli strumenti appropriati. 
     *  
     *  In questo possiamo vedere l'offerta di Azure per questa metodologia al seguente indirizzo.:
     *  
     *  https://azure.microsoft.com/it-it/solutions/devops/#products
     *  
     *  Architetture della soluzione DevOps
     *  
     *  
     *      In Azure sono presenti questi scenari comuni per DevOps.
     *  
     *      - CI/CD per Macchine virtuali di Azure
     *  
     *      Azure è un cloud di livello superiore per ospitare macchine virtuali che eseguono 
     *      Windows o Linux. Indipendentemente dal linguaggio che usi per sviluppare le applicazioni, 
     *      ad esempio ASP.NET, Java, Node.js o PHP, ti servirà una pipeline di integrazione 
     *      continua e distribuzione continua (CI/CD) per effettuare automaticamente il push 
     *      delle modifiche nelle macchine virtuali.
     *      
     *      // Figura 1 : sa-1.png
     *      
     *      - Integrazione continua/Distribuzione continua Java con Jenkins e app Web di Azure
     * 
     *      Servizio app di Azure è un modo facile e veloce per creare app Web usando Java, 
     *      Node.js, PHP o ASP.NET, nonché il supporto per runtime di linguaggi personalizzati 
     *      tramite Docker. Offri rapidamente valore ai clienti usando il Servizio app di Azure 
     *      con una pipeline di CI/CD per eseguire automaticamente il push di ogni modifica nel 
     *      Servizio app di Azure.
     *      
     *      // Figura 1 : sa-2.png
     *      
     *      - DevOps per il servizio Azure Kubernetes
     * 
     *      Bilancia la velocità e la sicurezza e distribuisci rapidamente codice su larga scala 
     *      usando DevOps con il servizio Azure Kubernetes. Puoi applicare tutele ai processi di 
     *      sviluppo usando CI/CD con controlli dei criteri dinamici e puoi accelerare i cicli di 
     *      feedback con il monitoraggio costante. Usa Azure Pipelines per una distribuzione 
     *      veloce, assicurando al tempo stesso l'applicazione di criteri essenziali con Criteri 
     *      di Azure. Azure ti offre la visibilità in tempo reale per le pipeline di compilazione 
     *      e di rilascio e la possibilità di applicare con facilità il controllo della conformità 
     *      e le riconfigurazioni.
     *      
     *  DevOps e il cloud
     *  
     *  
     *  L'adozione del cloud ha trasformato in modo significativo il modo in cui i team creano, 
     *  distribuiscono e gestiscono le applicazioni. 
     *  
     *  L'adozione di DevOps inoltre offre ai team maggiori opportunità di miglioramento delle 
     *  procedure e del servizio clienti.
     *  
     *      - Flessibilità cloud
     *      
     *      Grazie alla possibilità di effettuare rapidamente il provisioning e configurare 
     *      ambienti cloud in più aree con risorse illimitate, i team ottengono flessibilità 
     *      per la distribuzione delle app. 
     *      Invece di dover acquistare, configurare e mantenere server fisici, i team creano 
     *      ora ambienti cloud complessi in pochi minuti e quindi li arrestano quando non sono 
     *      più necessari.
     *      
     *      - Kubernetes
     *      
     *      Sempre più applicazioni usano la tecnologia basata su contenitori, quindi Kubernetes 
     *      sta diventando la soluzione leader di settore per l'orchestrazione di contenitori su 
     *      larga scala. L'automazione dei processi di creazione e distribuzione di contenitori 
     *      tramite pipeline CI/CD e del monitoraggio di tali contenitori in produzione sta 
     *      diventando essenziale nell'epoca di Kubernetes.
     *      
     *      - Elaborazione serverless
     *      
     *      Grazie allo spostamento della maggior parte delle attività di gestione dell'infrastruttura 
     *      al provider di servizi cloud, i team possono concentrarsi sulle app invece che 
     *      sull'infrastruttura sottostante. 
     *      L'elaborazione serverless permette di eseguire le applicazioni senza dover configurare 
     *      e gestire server. Alcune opzioni riducono la complessità e il rischio dello sviluppo e 
     *      delle operazioni, un esempio sono le Functions.
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
