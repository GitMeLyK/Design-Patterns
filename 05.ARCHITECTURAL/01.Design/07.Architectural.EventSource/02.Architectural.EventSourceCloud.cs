using System;

namespace DotNetDesignPatternDemos.Architectural.EventSource.OnCloud
{
    /*
     *  Qui viene tradtto e riportato il Modello di progettazione EventSource
     *  applicato in un contesto cloud quale Azure.
     *  
     *  Con questo modello di progettazine enterprise molti gli attori coinvolti ma 
     *  trasparenti allo sviluppatore porta la soluzione ad essere adottata in poco
     *  tempo e già infrastrutturata in questo sistema completo.
     *  
     *  Gli sviluppatori possono attraverso le API fornite da Azure platform accedere
     *  all'impianto e utilizzare questa metodologia per i prori scopi innestando e 
     *  modellando gli eventi necessari per il completamento delle proprie operazioni.
     *  
     *  ---------------------------------------------------------------------------------------
     * 
     * Anziché archiviare solo lo stato corrente dei dati in un dominio, utilizzare un archivio 
     * di sola aggiunta per registrare l'intera serie di azioni eseguite su tali dati. 
     * L'archivio funge da sistema di record e può essere utilizzato per materializzare 
     * gli oggetti di dominio. Ciò può semplificare le attività in domini complessi, 
     * evitando la necessità di sincronizzare il modello di dati e il dominio aziendale, 
     * migliorando al contempo le prestazioni, la scalabilità e la reattività. 
     * 
     * Può anche fornire coerenza per i dati transazionali e mantenere audit trail e cronologia 
     * completi che possono consentire azioni di compensazione.
     * 
     * Contesto del problema:
     * 
     *  La maggior parte delle applicazioni funziona con i dati e l'approccio tipico consiste 
     *  nel fatto che l'applicazione mantenga lo stato corrente dei dati aggiornandoli man mano 
     *  che gli utenti lavorano con essi. 
     *  Ad esempio, nel modello TRADIZIONALE di creazione, lettura, aggiornamento ed eliminazione (CRUD)
     *  un tipico processo di dati consiste nel leggere i dati dall'archivio, apportarvi alcune 
     *  modifiche e aggiornare lo stato corrente dei dati con i nuovi valori, spesso utilizzando 
     *  transazioni che bloccano i dati.
     *  
     *  L'approccio CRUD presenta alcune limitazioni:
     *  
     *      I sistemi CRUD eseguono operazioni di aggiornamento direttamente su un archivio dati, 
     *      che può rallentare le prestazioni e la reattività e limitare la scalabilità, 
     *      a causa del sovraccarico di elaborazione richiesto.
     *      
     *      In un dominio di collaborazione con molti utenti simultanei, i conflitti di 
     *      aggiornamento dei dati sono più probabili perché le operazioni di aggiornamento 
     *      avvengono su un singolo elemento di dati.
     *      
     *      A meno che non esista un meccanismo di controllo aggiuntivo che registra i dettagli 
     *      di ogni operazione in un registro separato, la cronologia viene persa.
     *      
     *  Soluzione
     *  
     *      Il modello Event Sourcing definisce un approccio alla gestione delle operazioni sui 
     *      dati guidati da una sequenza di eventi, ognuno dei quali viene registrato in un 
     *      archivio di sola aggiunta. Il codice dell'applicazione invia una serie di eventi 
     *      che descrivono in modo imperativo ogni azione che si è verificata sui dati 
     *      all'archivio eventi, dove vengono mantenuti. 
     *      
     *      Ogni evento rappresenta un insieme di modifiche ai dati (ad esempio ).AddedItemToOrder
     *      
     *      Gli eventi vengono mantenuti in un archivio eventi che funge da sistema di record 
     *      (l'origine dati autorevole) sullo stato corrente dei dati. 
     *      L'archivio eventi in genere pubblica questi eventi in modo che i consumatori possano 
     *      ricevere notifiche e possano gestirli se necessario. 
     *      
     *      I consumer potrebbero, ad esempio, avviare attività che applicano le operazioni negli 
     *      eventi ad altri sistemi o eseguire qualsiasi altra azione associata necessaria per 
     *      completare l'operazione. Si noti che il codice dell'applicazione che genera gli eventi 
     *      è disaccoppiato dai sistemi che sottoscrivono gli eventi.
     *      
     *      Gli usi tipici degli eventi pubblicati dall'archivio eventi sono quelli di mantenere 
     *      visualizzazioni materializzate delle entità man mano che le azioni nell'applicazione 
     *      le modificano e di integrarle con sistemi esterni. Ad esempio, un sistema può 
     *      mantenere una visualizzazione materializzata di tutti gli ordini dei clienti 
     *      utilizzata per popolare parti dell'interfaccia utente. Quando l'applicazione 
     *      aggiunge nuovi ordini, aggiunge o rimuove elementi nell'ordine e aggiunge informazioni 
     *      sulla spedizione, gli eventi che descrivono queste modifiche possono essere gestiti 
     *      e utilizzati per aggiornare la visualizzazione materializzata.
     *      
     *      Inoltre, in qualsiasi momento è possibile per le applicazioni leggere la cronologia 
     *      degli eventi e utilizzarla per materializzare lo stato corrente di un'entità riproducendo 
     *      e consumando tutti gli eventi correlati a tale entità. Ciò può verificarsi su richiesta 
     *      per materializzare un oggetto di dominio durante la gestione di una richiesta o tramite 
     *      un'attività pianificata in modo che lo stato dell'entità possa essere archiviato come 
     *      visualizzazione materializzata per supportare il livello di presentazione.
     *      
     *      La figura in allegato una panoramica del modello, incluse alcune delle opzioni per 
     *      l'utilizzo del flusso di eventi, ad esempio la creazione di una vista materializzata, 
     *      l'integrazione di eventi con applicazioni e sistemi esterni e la riproduzione di 
     *      eventi per creare proiezioni dello stato corrente di entità specifiche.
     *      
     *      Il modello di approvvigionamento eventi offre i vantaggi seguenti:
     *      
     *      Gli eventi sono immutabili e possono essere archiviati utilizzando un'operazione 
     *      di sola accodamento. L'interfaccia utente, il flusso di lavoro o il processo che 
     *      ha avviato un evento possono continuare e le attività che gestiscono gli eventi 
     *      possono essere eseguite in background. 
     *      
     *      Questo, combinato con il fatto che non ci sono contese durante l'elaborazione 
     *      delle transazioni, può migliorare notevolmente le prestazioni e la scalabilità 
     *      per le applicazioni, in particolare per il livello di presentazione o l'interfaccia utente.
     *      
     *      Gli eventi sono oggetti semplici che descrivono alcune azioni che si sono verificate, 
     *      insieme a tutti i dati associati necessari per descrivere l'azione rappresentata 
     *      dall'evento. 
     *      
     *      Gli eventi non aggiornano direttamente un archivio dati. 
     *      
     *      Vengono semplicemente registrati per la gestione al momento opportuno. 
     *      Ciò può semplificare l'implementazione e la gestione.
     *      
     *      Gli eventi hanno in genere un significato per un esperto di dominio, mentre la 
     *      mancata corrispondenza dell'impedenza relazionale degli oggetti può rendere 
     *      difficili la comprensione di tabelle di database complesse. 
     *      
     *      Le tabelle sono costrutti artificiali che rappresentano lo stato corrente del 
     *      sistema, non gli eventi che si sono verificati.
     *      
     *      L'approvvigionamento eventi può aiutare a impedire che gli aggiornamenti 
     *      simultanei causino conflitti perché evita la necessità di aggiornare 
     *      direttamente gli oggetti nell'archivio dati. 
     *      
     *      Tuttavia, il modello di dominio deve comunque essere progettato per proteggersi 
     *      da richieste che potrebbero causare uno stato incoerente.
     *      
     *      L'archiviazione di soli accodamenti degli eventi fornisce un audit trail che 
     *      può essere utilizzato per monitorare le azioni eseguite su un archivio dati, 
     *      rigenerare lo stato corrente come viste o proiezioni materializzate riproducendo 
     *      gli eventi in qualsiasi momento e assistere nel test e nel debug del sistema. 
     *      
     *      Inoltre, il requisito di utilizzare eventi di compensazione per annullare le 
     *      modifiche fornisce una cronologia delle modifiche che sono state invertite, 
     *      il che non sarebbe il caso se il modello memorizzasse semplicemente lo stato corrente. 
     *      
     *      L'elenco degli eventi può essere utilizzato anche per analizzare le prestazioni 
     *      delle applicazioni e rilevare le tendenze del comportamento degli utenti o per 
     *      ottenere altre informazioni aziendali utili.
     *      
     *      L'archivio eventi genera eventi e le attività eseguono operazioni in risposta 
     *      a tali eventi. Questo disaccoppiamento delle attività dagli eventi offre 
     *      flessibilità ed estensibilità. 
     *      
     *      Le attività conoscono il tipo di evento e i dati dell'evento, ma non l'operazione 
     *      che ha attivato l'evento. Inoltre, più attività possono gestire ogni evento. 
     *      
     *      Ciò consente una facile integrazione con altri servizi e sistemi che ascoltano 
     *      solo i nuovi eventi generati dall'archivio eventi. 
     *      
     *      Tuttavia, gli eventi di sourcing degli eventi tendono ad essere di livello molto 
     *      basso e potrebbe essere necessario generare invece eventi di integrazione specifici.
     *      
     *      L'approvvigionamento degli eventi viene comunemente combinato con il modello 
     *      CQRS eseguendo le attività di gestione dei dati in risposta agli eventi e 
     *      materializzando le visualizzazioni dagli eventi archiviati.
     *      
     *  Considerare i seguenti punti quando si decide come implementare questo modello:
     *  
     *      Il sistema sarà alla fine coerente solo durante la creazione di viste materializzate 
     *      o la generazione di proiezioni di dati riproducendo eventi. 
     *      C'è un certo ritardo tra un'applicazione che aggiunge eventi all'archivio eventi 
     *      come risultato della gestione di una richiesta, gli eventi pubblicati e i consumatori 
     *      degli eventi che li gestiscono. 
     *      
     *      Durante questo periodo, nuovi eventi che descrivono ulteriori modifiche alle entità 
     *      potrebbero essere arrivati nel negozio eventi.
     *      
     *  L'archivio eventi è la fonte permanente di informazioni e pertanto i dati degli eventi 
     *  non devono mai essere aggiornati. 
     *  
     *  L'unico modo per aggiornare un'entità per annullare una modifica consiste nell'aggiungere 
     *  un evento di compensazione all'archivio eventi. Se il formato (anziché i dati) 
     *  degli eventi persistenti deve cambiare, ad esempio durante una migrazione, può essere 
     *  difficile combinare gli eventi esistenti nell'archivio con la nuova versione. 
     *  
     *  Potrebbe essere necessario scorrere tutti gli eventi apportando modifiche in modo 
     *  che siano conformi al nuovo formato o aggiungere nuovi eventi che utilizzano il 
     *  nuovo formato. Prendere in considerazione l'utilizzo di un timbro di versione su 
     *  ogni versione dello schema di eventi per mantenere sia il vecchio che il nuovo 
     *  formato di evento.
     *  
     *  Le applicazioni multithread e più istanze di applicazioni potrebbero archiviare eventi 
     *  nell'archivio eventi. 
     *  
     *  La coerenza degli eventi nell'archivio eventi è fondamentale, così come l'ordine 
     *  degli eventi che influiscono su un'entità specifica (l'ordine in cui si verificano 
     *  le modifiche apportate a un'entità influisce sul suo stato corrente). 
     *  
     *  L'aggiunta di un timestamp a ogni evento può aiutare a evitare problemi. 
     *  
     *  Un'altra pratica comune consiste nell'annotare ogni evento risultante da una richiesta 
     *  con un identificatore incrementale. 
     *  
     *  Se due azioni tentano di aggiungere eventi per la stessa entità contemporaneamente, 
     *  l'archivio eventi può rifiutare un evento che corrisponde a un identificatore di 
     *  entità e a un identificatore di evento esistenti.
     *  
     *  Non esiste un approccio standard o meccanismi esistenti, ad esempio le query SQL, per 
     *  la lettura degli eventi per ottenere informazioni. 
     *  
     *  Gli unici dati che possono essere estratti sono un flusso di eventi che utilizza un 
     *  identificatore di evento come criterio. 
     *  
     *  L'ID evento viene in genere mappato a singole entità. 
     *  
     *  Lo stato corrente di un'entità può essere determinato solo riproducendo tutti 
     *  gli eventi ad essa correlati rispetto allo stato originale di tale entità.
     *  
     *  La lunghezza di ogni flusso di eventi influisce sulla gestione e sull'aggiornamento 
     *  del sistema. 
     *  
     *  Se i flussi sono di grandi dimensioni, è consigliabile creare snapshot a intervalli 
     *  specifici, ad esempio un numero specificato di eventi. 
     *  
     *  Lo stato corrente dell'entità può essere ottenuto dallo snapshot e riproducendo 
     *  tutti gli eventi che si sono verificati dopo quel momento.
     *  
     *  Anche se l'approvvigionamento degli eventi riduce al minimo la possibilità 
     *  di aggiornamenti in conflitto ai dati, l'applicazione deve comunque essere 
     *  in grado di gestire le incoerenze derivanti dalla coerenza finale e dalla 
     *  mancanza di transazioni. Ad esempio, un evento che indica una riduzione 
     *  dell'inventario delle scorte potrebbe arrivare nell'archivio dati mentre 
     *  viene effettuato un ordine per quell'articolo, con conseguente obbligo 
     *  di riconciliare le due operazioni avvisando il cliente o creando 
     *  un ordine arretrato.
     *  
     *  La pubblicazione dell'evento potrebbe essere almeno una volta, e quindi 
     *  i consumatori degli eventi devono essere idempotenti. 
     *  
     *  Non devono riapplicare l'aggiornamento descritto in un evento se l'evento 
     *  viene gestito più di una volta. Ad esempio, se più istanze di un consumer 
     *  mantengono e aggregano la proprietà di un'entità, ad esempio il numero totale 
     *  di ordini effettuati, solo una deve riuscire ad incrementare l'aggregazione 
     *  quando si verifica un evento effettuato un ordine. 
     *  
     *  Anche se questa non è una caratteristica chiave dell'approvvigionamento degli eventi, 
     *  è la solita decisione di implementazione.
     *  
     *  Quando utilizzare questo modello
     *  
     *  Utilizzare questo modello nei seguenti scenari:
     *  
     *      Quando si desidera acquisire l'intento, lo scopo o il motivo nei dati. 
     *      Ad esempio, le modifiche apportate a un'entità cliente possono essere acquisite 
     *      come una serie di tipi di eventi specifici, ad esempio Spostato a casa, 
     *      Account chiuso o Deceduto.
     *      
     *      Quando è fondamentale ridurre al minimo o evitare completamente il 
     *      verificarsi di aggiornamenti contrastanti dei dati.
     *      
     *      Quando si desidera registrare gli eventi che si verificano ed essere in grado 
     *      di riprodurli per ripristinare lo stato di un sistema, eseguire il rollback 
     *      delle modifiche o mantenere una cronologia e un log di controllo. 
     *      Ad esempio, quando un'attività prevede più passaggi, potrebbe essere necessario 
     *      eseguire azioni per ripristinare gli aggiornamenti e quindi riprodurre alcuni 
     *      passaggi per riportare i dati in uno stato coerente.
     *      
     *      Quando si utilizzano gli eventi è una caratteristica naturale del funzionamento 
     *      dell'applicazione e richiede poco sforzo aggiuntivo di sviluppo o implementazione.
     *      
     *      Quando è necessario disaccoppiare il processo di immissione o aggiornamento dei 
     *      dati dalle attività necessarie per applicare queste azioni. 
     *      
     *      Ciò potrebbe essere finalizzato a migliorare le prestazioni dell'interfaccia 
     *      utente o a distribuire eventi ad altri listener che eseguono un'azione quando 
     *      si verificano gli eventi. Ad esempio, l'integrazione di un sistema di buste paga 
     *      con un sito Web di invio delle spese in modo che gli eventi generati 
     *      dall'archivio eventi in risposta agli aggiornamenti dei dati effettuati nel 
     *      sito Web vengano utilizzati sia dal sito Web che dal sistema di gestione delle 
     *      retribuzioni.
     *      
     *      Quando si desidera flessibilità per poter modificare il formato dei modelli 
     *      materializzati e dei dati di entità in caso di modifica dei requisiti oppure, 
     *      se utilizzato in combinazione con CQRS, è necessario adattare un modello di 
     *      lettura o le viste che espongono i dati.
     *      
     *      Se utilizzato in combinazione con CQRS e la coerenza finale è accettabile durante 
     *      l'aggiornamento di un modello di lettura o l'impatto sulle prestazioni della 
     *      reidratazione di entità e dati da un flusso di eventi è accettabile.
     *      
     *  Questo modello potrebbe non essere utile nelle situazioni seguenti:
     *  
     *      Domini piccoli o semplici, sistemi che hanno poca o nessuna logica di business 
     *      o sistemi non di dominio che funzionano naturalmente bene con i tradizionali 
     *      meccanismi di gestione dei dati CRUD.
     *      
     *      Sistemi in cui sono richiesti coerenza e aggiornamenti in tempo reale delle 
     *      visualizzazioni dei dati.
     *      
     *      Sistemi in cui non sono necessari audit trail, cronologia e funzionalità per 
     *      eseguire il rollback e la riproduzione delle azioni.
     *      
     *      Sistemi in cui vi è solo una bassissima incidenza di aggiornamenti in conflitto 
     *      ai dati sottostanti. 
     *      Ad esempio, sistemi che aggiungono prevalentemente dati anziché aggiornarli.
     *
     *  Esempio
     *  
     *      Un sistema di gestione delle conferenze deve tenere traccia del numero di 
     *      prenotazioni completate per una conferenza in modo da poter verificare se 
     *      ci sono posti ancora disponibili quando un potenziale partecipante tenta 
     *      di effettuare una prenotazione. 
     *      
     *      Il sistema potrebbe memorizzare il numero totale di prenotazioni per una 
     *      conferenza in almeno due modi:
     *      
     *          -Il sistema potrebbe memorizzare le informazioni sul numero totale di 
     *           prenotazioni come entità separata in un database che contiene le informazioni 
     *           sulle prenotazioni. Man mano che le prenotazioni vengono effettuate o cancellate, 
     *           il sistema potrebbe aumentare o decrementare questo numero a seconda dei casi. 
     *           Questo approccio è semplice in teoria, ma può causare problemi di scalabilità 
     *           se un gran numero di partecipanti sta tentando di prenotare posti per un breve 
     *           periodo di tempo. Ad esempio, nell'ultimo giorno o giù di lì prima della chiusura 
     *           del periodo di prenotazione.
     *           
     *          -Il sistema potrebbe memorizzare informazioni su prenotazioni e cancellazioni 
     *           come eventi tenuti in un negozio di eventi. Potrebbe quindi calcolare il numero 
     *           di posti disponibili riproducendo questi eventi. 
     *           Questo approccio può essere più scalabile a causa dell'immutabilità degli eventi. 
     *           Il sistema deve solo essere in grado di leggere i dati dall'archivio eventi o di 
     *           aggiungere dati all'archivio eventi. 
     *           Le informazioni sugli eventi su prenotazioni e cancellazioni non vengono mai modificate.
     *           
     *           Nel diagramma in allegato viene illustrato come implementare il sottosistema 
     *           di prenotazione dei posti a sedere del sistema di gestione delle conferenze 
     *           utilizzando l'approvvigionamento degli eventi.
     *           
     *           Figura 3
     *           
     *  La sequenza di azioni per la prenotazione di due posti è la seguente:
     *  
     *      - L'interfaccia utente emette un comando per prenotare posti a sedere 
     *        per due partecipanti. Il comando viene gestito da un gestore di comandi 
     *        separato. Un elemento logico che viene disaccoppiato dall'interfaccia utente 
     *        ed è responsabile della gestione delle richieste inviate come comandi.
     *        
     *       - Un aggregato contenente informazioni su tutte le prenotazioni per la conferenza 
     *         viene costruito interrogando gli eventi che descrivono prenotazioni e cancellazioni. 
     *         Questa aggregazione è denominata, ed è contenuta in un modello di dominio che espone
     *         i metodi per l'esecuzione di query e la modifica dei dati nell'aggregazione.
     *         SeatAvailability
     *        
     *        -Alcune ottimizzazioni da considerare riguardano l'utilizzo di snapshot 
     *        (in modo che non sia necessario eseguire query e riprodurre l'elenco completo di 
     *        eventi per ottenere lo stato corrente dell'aggregazione) e la gestione di una copia 
     *        memorizzata nella cache dell'aggregazione.
     *        
     *       -Il gestore dei comandi richiama un metodo esposto dal modello di dominio per 
     *        effettuare le prenotazioni.
     *        
     *       -L'aggregazione registra un evento contenente il numero di posti riservati. 
     *       La prossima volta che l'aggregato applica gli eventi, tutte le prenotazioni verranno 
     *       utilizzate per calcolare quanti posti rimangono.
     *       SeatAvailability
     *       
     *       Il sistema aggiunge il nuovo evento all'elenco degli eventi nell'archivio eventi.
     *       
     *       Se un utente annulla una postazione, il sistema segue un processo simile, 
     *       ad eccezione del fatto che il gestore dei comandi emette un comando che genera 
     *       un evento di annullamento della postazione e lo aggiunge all'archivio eventi.
     *       
     *       Oltre a fornire maggiori possibilità di scalabilità, l'utilizzo di un negozio eventi 
     *       fornisce anche una cronologia completa, o audit trail, delle prenotazioni e 
     *       delle cancellazioni per una conferenza. Gli eventi nell'archivio eventi sono 
     *       il record accurato. Non è necessario mantenere gli aggregati in nessun altro 
     *       modo perché il sistema può facilmente riprodurre gli eventi e ripristinare lo 
     *       stato in qualsiasi momento.
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
