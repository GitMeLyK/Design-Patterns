using System;

namespace DotNetDesignPatternDemos.Architectural.PublisherSubscriber
{
    /*
     *  Abilitare un'applicazione per annunciare eventi a più utenti interessati in 
     *  modo asincrono, senza associare i mittenti ai destinatari.
     *  
     *  Nelle applicazioni distribuite e/o basate su cloud, i componenti del sistema spesso 
     *  devono fornire informazioni ad altri componenti quando si verificano eventi.
     *  
     *  La messaggistica asincrona è un modo efficace per disaccoppiare i mittenti 
     *  dai consumatori ed evitare di bloccare il mittente per attendere una risposta. 
     *  Tuttavia, l'utilizzo di una coda di messaggi dedicata per ogni consumatore non 
     *  è scalabile in modo efficace per molti consumatori. Inoltre, alcuni dei consumatori 
     *  potrebbero essere interessati solo a un sottoinsieme delle informazioni. 
     *  
     *  Come può il mittente annunciare eventi a tutti i consumatori interessati 
     *  senza conoscere la loro identità?
     *  
     *  Introdurre un sottosistema di messaggistica asincrona che includa quanto segue:
     *  
     *      - Canale di messaggistica di input utilizzato dal mittente. 
     *        Il mittente impacchetta gli eventi in messaggi, utilizzando un formato di messaggio 
     *        noto, e invia questi messaggi tramite il canale di input. 
     *        Il mittente in questo modello è anche chiamato editore.
     *        
     *      - Un canale di messaggistica di output per consumatore. 
     *        I consumatori sono noti come abbonati.
     *        
     *      - Un meccanismo per copiare ogni messaggio dal canale di input ai canali di output 
     *        per tutti gli abbonati interessati a quel messaggio. 
     *        Questa operazione viene in genere gestita da un intermediario, ad esempio un 
     *        broker di messaggi o un bus di eventi.
     *        
     *      Nota: Un messaggio è un pacchetto di dati. Un evento è un messaggio che notifica 
     *            ad altri componenti una modifica o un'azione che ha avuto luogo.
     *            
     *  Nel diagramma in allegato vengono illustrati i componenti logici di questo modello.
     *  
     *  La messaggistica pub/sub presenta i seguenti vantaggi:
     *  
     *      - Disaccoppia i sottosistemi che devono ancora comunicare. 
     *        I sottosistemi possono essere gestiti in modo indipendente e i messaggi 
     *        possono essere gestiti correttamente anche se uno o più ricevitori sono offline.
     *        
     *      - Aumenta la scalabilità e migliora la reattività del mittente. 
     *        Il mittente può inviare rapidamente un singolo messaggio al canale di input, 
     *        quindi tornare alle sue responsabilità di elaborazione principali. 
     *        L'infrastruttura di messaggistica è responsabile di garantire che i messaggi 
     *        vengano recapitati agli abbonati interessati.
     *      
     *      - Migliora l'affidabilità. La messaggistica asincrona consente alle applicazioni 
     *        di continuare a funzionare senza problemi con carichi maggiori e di gestire gli 
     *        errori intermittenti in modo più efficace.
     *        
     *      - Consente l'elaborazione differita o programmata. Gli abbonati possono attendere 
     *        di raccogliere i messaggi fino alle ore non di punta oppure i messaggi possono 
     *        essere instradati o elaborati in base a una pianificazione specifica.
     *        
     *      - Consente un'integrazione più semplice tra sistemi che utilizzano piattaforme, 
     *        linguaggi di programmazione o protocolli di comunicazione diversi, nonché tra 
     *        sistemi locali e applicazioni in esecuzione nel cloud.
     *        
     *      - Facilita i flussi di lavoro asincroni in un'azienda.
     *      
     *      - Migliora la testabilità. I canali possono essere monitorati e i messaggi possono 
     *        essere ispezionati o registrati come parte di una strategia di test di integrazione 
     *        complessiva.
     *        
     *      - Fornisce la separazione delle preoccupazioni per le applicazioni. Ogni applicazione 
     *        può concentrarsi sulle proprie funzionalità principali, mentre l'infrastruttura di 
     *        messaggistica gestisce tutto il necessario per instradare in modo affidabile i 
     *        messaggi a più utenti.
     *        
     * Questioni e considerazioni
     * 
     * Considerare i seguenti punti quando si decide come implementare questo modello:
     * 
     *      - Tecnologie esistenti. Si consiglia vivamente di utilizzare i prodotti e i 
     *        servizi di messaggistica disponibili che supportano un modello di pubblicazione-sottoscrizione,
     *        anziché crearne uno personalizzato. In Azure è consigliabile usare Bus di servizio, Hub eventi 
     *        o Griglia eventi. Altre tecnologie che possono essere utilizzate per la messaggistica pub/sub 
     *        includono Redis, RabbitMQ e Apache Kafka.
     *        
     *      - Gestione degli abbonamenti. L'infrastruttura di messaggistica deve fornire meccanismi 
     *        che i consumatori possono utilizzare per abbonarsi o annullare l'iscrizione ai canali 
     *        disponibili.
     *        
     *      - Sicurezza. La connessione a qualsiasi canale di messaggi deve essere limitata dai 
     *        criteri di sicurezza per impedire l'intercettazione da parte di utenti o applicazioni 
     *        non autorizzati.
     *        
     *      - Sottoinsiemi di messaggi. Gli abbonati sono in genere interessati solo al 
     *        sottoinsieme dei messaggi distribuiti da un editore. I servizi di messaggistica 
     *        spesso consentono ai sottoscrittori di restringere il set di messaggi ricevuti da:
     *        
     *          - Argomenti. Ogni argomento ha un canale di output dedicato e ogni consumatore 
     *            può iscriversi a tutti gli argomenti pertinenti.
     *            
     *          - Filtraggio dei contenuti. I messaggi vengono ispezionati e distribuiti in base 
     *            al contenuto di ciascun messaggio. Ogni abbonato può specificare il contenuto a 
     *            cui è interessato.
     *       
     *     - Abbonati con caratteri jolly. Prendi in considerazione la possibilità di 
     *       consentire agli abbonati di iscriversi a più argomenti tramite caratteri jolly.
     *       
     *     - Comunicazione bidirezionale. I canali in un sistema di pubblicazione-sottoscrizione 
     *       sono trattati come unidirezionali. Se un sottoscrittore specifico deve inviare 
     *       il riconoscimento o comunicare lo stato all'editore, prendere in considerazione 
     *       l'utilizzo del modello di richiesta/risposta. Questo modello utilizza un canale 
     *       per inviare un messaggio all'abbonato e un canale di risposta separato per 
     *       comunicare con l'editore.
     *       
     *     - Ordinazione dei messaggi. L'ordine in cui le istanze consumer ricevono i messaggi 
     *       non è garantito e non riflette necessariamente l'ordine in cui i messaggi sono stati 
     *       creati. Progettare il sistema in modo da garantire che l'elaborazione dei messaggi 
     *       sia idempotente per eliminare qualsiasi dipendenza dall'ordine di gestione dei 
     *       messaggi.
     *       
     *     - Priorità del messaggio. Alcune soluzioni potrebbero richiedere che i messaggi 
     *       vengano elaborati in un ordine specifico. Il modello Coda di priorità fornisce 
     *       un meccanismo per garantire che messaggi specifici vengano recapitati prima di altri.
     *       
     *     - Messaggi velenosi. Un messaggio non valido o un'attività che richiede l'accesso 
     *       a risorse non disponibili può causare l'errore di un'istanza del servizio. 
     *       Il sistema dovrebbe impedire che tali messaggi vengano restituiti alla coda. 
     *       Invece, acquisisci e archivia i dettagli di questi messaggi altrove in modo che 
     *       possano essere analizzati se necessario.
     *       
     *     - Messaggi ripetuti. Lo stesso messaggio potrebbe essere inviato più di una volta. 
     *       Ad esempio, il mittente potrebbe non riuscire dopo aver pubblicato un messaggio. 
     *       Quindi una nuova istanza del mittente potrebbe avviarsi e ripetere il messaggio. 
     *       L'infrastruttura di messaggistica deve implementare il rilevamento e la rimozione 
     *       dei messaggi duplicati (noto anche come de-duping) in base agli ID dei messaggi al 
     *       fine di fornire il recapito dei messaggi al massimo una volta.
     *       
     *     - Scadenza del messaggio. Un messaggio potrebbe avere una durata limitata. 
     *       Se non viene elaborato entro questo periodo, potrebbe non essere più rilevante e 
     *       dovrebbe essere scartato. Un mittente può specificare un'ora di scadenza come parte 
     *       dei dati nel messaggio. Un destinatario può esaminare queste informazioni prima di 
     *       decidere se eseguire la logica di business associata al messaggio.
     *       
     *     - Pianificazione dei messaggi. Un messaggio potrebbe essere temporaneamente sottoposto 
     *       a embargo e non deve essere elaborato fino a una data e un'ora specifiche. 
     *       Il messaggio non dovrebbe essere disponibile per un destinatario fino a questo momento.
     *       
     * Quando utilizzare questo modello?
     * 
     *  Utilizzare questo modello quando:
     *  
     *      - Un'applicazione deve trasmettere informazioni a un numero significativo di 
     *        consumatori.
     *        
     *      - Un'applicazione deve comunicare con una o più applicazioni o servizi sviluppati 
     *        in modo indipendente, che possono utilizzare piattaforme, linguaggi di 
     *        programmazione e protocolli di comunicazione diversi.
     *        
     *      - Un'applicazione può inviare informazioni ai consumatori senza richiedere risposte 
     *        in tempo reale da parte dei consumatori.
     *        
     *      - I sistemi integrati sono progettati per supportare un eventuale modello di coerenza 
     *        per i loro dati.
     *        
     *      - Un'applicazione deve comunicare informazioni a più utenti, che possono avere 
     *        requisiti di disponibilità o pianificazioni dei tempi di attività diversi rispetto 
     *        al mittente.
     * 
     * Questo modello potrebbe non essere utile quando:
     * 
     *      - Un'applicazione ha solo pochi consumatori che hanno bisogno di informazioni 
     *        significativamente diverse dall'applicazione di produzione.
     *        
     *      - Un'applicazione richiede un'interazione quasi in tempo reale con i consumatori.
     * 
     *  In allegato la figura 2 fa vedre uno schema di questo modello di progettazione applicato
     *  in modo enterprise con l'ausilio del cloud offerto da azure.
     *  
     *  Questo spunto fa parte del cosiddetto insieme di Patterns Cloud.
     *  
     *  Visitare il sito per avere tutti i modelli relativi.:
     *  
     *  Cloud Design Patterns : https://learn.microsoft.com/en-us/azure/architecture/patterns/
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
