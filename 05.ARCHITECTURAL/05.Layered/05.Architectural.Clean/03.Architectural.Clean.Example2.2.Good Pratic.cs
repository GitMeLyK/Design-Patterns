using System;

namespace DotNetDesignPatternDemos.Architectural.Clean.Example2.BestPratic
{
    /*
     * Rispetto al modello di template presente nel precedente esempio è opportuno
     * tenere queste buone pratiche a corredo del progetto per non incorrere in 
     * quelle che sono errori di design del modello architetturale CleanArchitect.
     * 
     * Good Pratic
     * 
     * 1 -Il livello di dominio dovrebbe essere indipendente dai problemi di accesso ai dati. 
     *    Il livello di dominio dovrebbe cambiare solo quando qualcosa all'interno del dominio 
     *    cambia, non quando cambia la tecnologia di accesso ai dati. 
     *    Ciò garantisce che il sistema sarà più facile da mantenere anche in futuro poiché 
     *    le modifiche alle tecnologie di accesso ai dati non avranno alcun impatto sul dominio 
     *    e viceversa.
     *    
     *    Questo è spesso un problema quando si creano sistemi che sfruttano Entity Framework, 
     *    poiché è comune aggiungere annotazioni di dati al modello di dominio. 
     *    Le annotazioni di dati, ad esempio gli attributi Required o MinLength, supportano 
     *    la convalida e consentono a Entity Framework di mappare gli oggetti nel modello 
     *    relazionale. Nell'esempio seguente, le annotazioni di dati vengono utilizzate 
     *    all'interno del modello di dominio:
     *    
     *    Cattivo esempio: il dominio è ingombro di annotazioni di dati
     *    // FIgura 1 : domain-layer-1.png
     *    
     *    Come puoi vedere nell'esempio precedente, il dominio è ingombro di annotazioni di dati. 
     *    Se la tecnologia di accesso ai dati cambia, probabilmente dovremo cambiare tutte le 
     *    entità poiché tutte le entità avranno annotazioni di dati. 
     *    Nell'esempio seguente, rimuoveremo le annotazioni dei dati dall'entità e utilizzeremo 
     *    invece un tipo di configurazione speciale:
     *    
     *    Buon esempio: il dominio è snello, la configurazione per l'entità è contenuta all'interno 
     *    di un tipo di configurazione separato
     *    // FIgura 2 : domain-layer-2.png
     *    // FIgura 3 : domain-layer-3.png
     *    // FIgura 4 : domain-layer-4.png
     *    
     *    Questo è un grande miglioramento! Ora l'entità del cliente è snella e la configurazione 
     *    può essere aggiunta al livello di persistenza, completamente separato dal dominio. 
     *    Ora il dominio è indipendente dai problemi di accesso ai dati.
     *    
     *    // Figura 5 : CA_Animation_4.gif
     *    
     * 2- Mantieni la logica di business fuori dal livello di presentazione?
     * 
     *    È comune che la logica di business venga aggiunta direttamente al livello di 
     *    presentazione. Quando si compilano sistemi MVC ASP.NET, ciò significa in genere 
     *    che la logica di business viene aggiunta ai controller come nell'esempio seguente:
     *    
     *    Cattivo esempio - Sebbene questa applicazione abbia chiaramente livelli di repository 
     *    e logica di business, la logica che orchestra queste dipendenze è nel controller ASP.NET 
     *    ed è difficile da riutilizzare:
     *    
     *      // Figura 6 : business-logic-presentation-layer-bad.png
     *      
     *    La logica nel controller precedente non può essere riutilizzata, ad esempio, da una nuova 
     *    applicazione console. Questo potrebbe andare bene per sistemi banali o piccoli, ma sarebbe 
     *    un errore per i sistemi aziendali. È importante assicurarsi che una logica come questa sia 
     *    indipendente dall'interfaccia utente in modo che il sistema sia facile da mantenere ora e 
     *    in futuro. Un ottimo approccio per risolvere questo problema è quello di utilizzare il 
     *    modello di mediazione con CQRS.
     *    
     *    CQRS è l'acronimo di Command Query Responsibility Segregation. È un modello che ho 
     *    sentito descrivere per la prima volta da Greg Young. Al suo centro c'è l'idea che è 
     *    possibile utilizzare un modello diverso per aggiornare le informazioni rispetto al 
     *    modello utilizzato per leggere le informazioni ... 
     *    
     *    C'è spazio per notevoli variazioni qui. I modelli in memoria possono condividere lo 
     *    stesso database, nel qual caso il database funge da comunicazione tra i due modelli. 
     *    Tuttavia, possono anche utilizzare database separati, trasformando in modo efficace 
     *    il database del lato query in un database di report in tempo reale. 
     *    
     *    CQRS significa una chiara separazione tra comandi (operazioni di scrittura) e query 
     *    (operazioni di lettura). CQRS può essere utilizzato con architetture complesse come 
     *    Event Sourcing, ma i concetti possono essere applicati anche ad applicazioni più 
     *    semplici con un singolo database.
     *    
     *    MediatR è una libreria .NET open source di Jimmy Bogard che fornisce un approccio 
     *    elegante e potente per la scrittura di CQRS, rendendo più facile scrivere codice pulito.
     *    
     *    Per ogni comando o query, si crea una classe di richiesta specifica che definisce 
     *    in modo esplicito l'"input" necessario per richiamare l'operazione.
     *    
     *      // Figura 7 : business-logic-presentation-layer-simple.png
     *      
     *    Quindi l'implementazione di tale comando o query viene implementata in una classe handler. 
     *    La classe del gestore viene creata da un contenitore Dependency Injection, quindi è 
     *    possibile utilizzare una qualsiasi delle dipendenze configurate (Repository, Entity 
     *    Framework, servizi e così via).
     *    
     *      // Figura 8 : business-logic-presentation-layer-handler.png
     *      
     *    Questo approccio porta molti vantaggi:
     *    
     *      Ogni comando o query rappresenta un'operazione atomica e ben definita come 
     *      "Ottieni i dettagli del mio ordine" (Query) o "Aggiungi prodotto X al mio ordine" 
     *      (Comando)
     *      
     *      Nelle API Web, questo incoraggia gli sviluppatori a tenere la logica fuori dai 
     *      controller. Il ruolo dei controllori si riduce a "Ricevi una richiesta dal web 
     *      e invia immediatamente a MediatR". Questo aiuta a implementare la regola 
     *      "Thin controllers": https://rules.ssw.com.au/do-you-use-thin-controllers-fat-models-and-dumb-views. 
     *      Quando la logica è in un controller, l'unico modo per richiamarla è tramite 
     *      richieste Web. La logica in un gestore mediatore può essere richiamata da qualsiasi 
     *      processo in grado di creare l'oggetto richiesta appropriato, ad esempio lavoratori 
     *      in background, programmi console o hub SignalR
     *      
     *      Mediator fornisce anche un semplice sistema pub/sub che consente di implementare 
     *      esplicitamente gli "effetti collaterali" come gestori aggiuntivi e separati. 
     *      Questo è ottimo per operazioni correlate o basate su eventi, ad esempio "Aggiorna 
     *      l'indice di ricerca dopo che una modifica al prodotto è stata salvata nel database"
     *      L'utilizzo di una classe di gestore specifica per ogni operazione significa che 
     *      esiste una configurazione di dipendenza specifica per ogni comando o query
     *      
     *      Gli sviluppatori spesso implementano interfacce e astrazioni tra i livelli delle 
     *      loro applicazioni. Esempi di questo potrebbero includere un IMessageService per 
     *      l'invio di e-mail o un'interfaccia IRepository per l'accesso al database astratto. 
     *      Queste tecniche astraggono dipendenze esterne specifiche come "Come salvare un'entità 
     *      ordine nel database" o "Come inviare un messaggio di posta elettronica". 
     *      Abbiamo assistito a molte applicazioni con livelli di repository puliti e ignoranti 
     *      di persistenza, ma poi con codice spaghetti disordinato in cima per le effettive 
     *      operazioni di logica di business. I comandi e le query MediatR sono migliori 
     *      nell'astrazione e nell'orchestrazione di operazioni di livello superiore come 
     *      "Completa il mio ordine" che possono o meno utilizzare astrazioni di livello inferiore. 
     *      L'adozione di MediatR incoraggia il codice pulito dall'alto verso il basso e aiuta gli 
     *      sviluppatori a "cadere nella fossa del successo"
     *      
     *      La creazione anche di una semplice app con questo approccio semplifica la 
     *      considerazione di architetture più avanzate come l'approvvigionamento di eventi. 
     *      Hai chiaramente definito "Quali dati posso ottenere" e "Quali operazioni posso 
     *      eseguire". Sei quindi libero di iterare l'implementazione migliore per fornire le 
     *      operazioni definite. I gestori MediatR sono facili da deridere e unit test
     *      
     *      I gestori MediatR sono facili da deridere e unit test
     *      
     *      L'interfaccia per i gestori MediatR incoraggia l'implementazione di metodi asincroni 
     *      di best practice con supporto per token di cancellazione.
     *      
     *      MediatR introduce un sistema di comportamento della pipeline che consente di iniettare 
     *      su misura l'invocazione del gestore. Ciò è utile per implementare problemi trasversali 
     *      come la registrazione, la convalida o la memorizzazione nella cache
     *      
     *      Buon esempio - MediatR semplifica le dipendenze iniettate nel controller. La richiesta 
     *      Web in arrivo viene semplicemente mappata direttamente a una richiesta MediatR che 
     *      orchestra tutta la logica per questa operazione. L'implementazione e le dipendenze 
     *      necessarie per completare "GetItemForEdit" sono libere di cambiare senza dover 
     *      modificare la classe del controller
     *      
     *      // Figura 4 : business-logic-presentation-layer-good.png
     *      
     * 3 - Sai come migliorare la reperibilità delle tue richieste MediatR?
     * 
     *      Quando si utilizza MediatR all'interno di un controller ASP.NET, è tipico 
     *      vedere azioni come le seguenti:
     *      
     *      Un tipico controller ASP.NET che utilizza Mediator
     *      // Figura 5 : improve-mediatr-typical.png
     *      
     *      Nell'esempio precedente, l'API contiene un'azione Create che include un 
     *      parametro CreateProductCommand. Questo comando è una richiesta di creazione di 
     *      un nuovo prodotto e la richiesta è associata a un gestore di richieste sottostante. 
     *      La richiesta viene inviata utilizzando MediatR con il metodo chiamata _mediator. 
     *      Invia(comando). MediatR corrisponderà la richiesta al gestore della richiesta 
     *      associato e restituirà la risposta (se presente). In un'implementazione tipica, 
     *      il gestore di richieste e richieste sarebbe contenuto in file separati:
     *      
     *      Cattivo esempio - La richiesta è contenuta in CreateProductCommand.cs
     *      // Figura 6 : improve-mediatr-bad.png
     *      
     *      Esempio non valido: il gestore delle richieste è contenuto in 
     *      CreateProductCommandHandler.cs
     *      // Figura 7 : improve-mediatr-bad-2.png
     *      
     *      Nell'implementazione precedente, il gestore delle richieste è separato in modo netto 
     *      dalla richiesta. Tuttavia, questa separazione comporta una riduzione della rilevabilità 
     *      del gestore. Ad esempio, uno sviluppatore che esamina l'azione in questa prima figura 
     *      potrebbe essere interessato alla logica alla base del comando. Quindi, premono F12 
     *      per andare alla definizione e possono vedere la richiesta (CreateProductCommand), ma 
     *      non la logica poiché è contenuta nel gestore della richiesta (CreateProductCommandHandler). 
     *      Lo sviluppatore deve quindi passare al gestore utilizzando Esplora soluzioni o alcune 
     *      procedure guidate della tastiera. Ciò presuppone che lo sviluppatore abbia familiarità 
     *      con la progettazione e sappia che esiste un gestore di richieste sottostante che contiene 
     *      la logica di interesse. 
     *      
     *      Possiamo evitare questi problemi e migliorare la reperibilità utilizzando invece il 
     *      seguente approccio:
     *      
     *      Buon esempio - La nidificazione del gestore delle richieste all'interno della richiesta 
     *      migliora l'individuabilità del comando e della logica di comando associata
     *      // Figura 8 : improve-mediatr-good.png
     *      
     *      Nell'esempio precedente il gestore della richiesta viene nidificato all'interno della 
     *      richiesta, migliorando la rilevabilità del comando e della logica di comando associata.
     *      
     * 4 - Conoscete la differenza tra gli oggetti di trasferimento dati e i modelli di visualizzazione? 
     *      
     *      I DTO (Data Transfer Objects) e i modelli di visualizzazione (VM) non sono lo stesso 
     *      concetto! La differenza principale è che mentre le VM possono incapsulare il comportamento, 
     *      le DTO no.
     *      
     *      Lo scopo di un DTO è il trasferimento di dati da una parte all'altra di un'applicazione. 
     *      Poiché i DTO non incapsulano il comportamento, possono essere facilmente serializzati 
     *      e deserializzati in altri formati, ad esempio JSON, XML e così via.
     *      
     *      Lo scopo di una vm è anche il trasferimento di dati, tuttavia le VM possono incapsulare 
     *      il comportamento. Questo comportamento è utile, ad esempio, quando si crea un'applicazione 
     *      WPF + MVVM, ma non così utile quando si crea una SPA, poiché non è possibile serializzare 
     *      il comportamento e passarlo da ASP.NET MVC al client.
     *      
     * 5 - Conosci l'approccio migliore per convalidare le richieste dei tuoi clienti?
     * 
     *      Quando si creano API Web, è importante convalidare ogni richiesta per garantire che soddisfi 
     *      tutte le condizioni preliminari previste. Il sistema deve elaborare richieste valide ma 
     *      restituire un errore per eventuali richieste non valide. Nel caso di ASP.NET Titolari, 
     *      tale convalida potrebbe essere implementata come segue:
     *      
     *      Cattivo esempio - Gestione della convalida delle richieste all'interno del controller
     *      // Figura 8 : validate-client-requests-bad.png
     *      
     *      Nell'esempio precedente, la convalida dello stato del modello viene utilizzata per garantire 
     *      che la richiesta venga convalidata prima di essere inviata tramite MediatR. 
     *      Sono sicuro che ti starai chiedendo- perché questo è un cattivo esempio? 
     *      Perché nel caso della creazione di prodotti, vogliamo convalidare ogni richiesta di creare 
     *      un prodotto, non solo quelle che arrivano attraverso l'API Web. Ad esempio, se creiamo 
     *      prodotti utilizzando un'applicazione console che richiama direttamente il comando, 
     *      dobbiamo assicurarci che anche tali richieste siano valide. 
     *      
     *      Quindi chiaramente la responsabilità per la convalida delle richieste non appartiene 
     *      all'interno dell'API Web, ma piuttosto a un livello più profondo, idealmente appena prima 
     *      che la richiesta venga eseguita.
     *      
     *      Un approccio per risolvere questo problema consiste nello spostare la convalida a 
     *      livello di applicazione, convalidando immediatamente prima dell'esecuzione della 
     *      richiesta. Nel caso dell'esempio precedente, questo potrebbe essere implementato come 
     *      segue:
     *      
     *      Esempio OK - Convalida gestita manualmente all'interno del gestore delle richieste 
     *      Per garantire che tutte le richieste siano convalidate
     *      // Figura 9 : validate-client-requests-ok.png
     *      
     *      L'implementazione di cui sopra risolve il problema. Se la richiesta proviene dall'API 
     *      Web o da un'app console, verrà convalidata prima che si verifichi un'ulteriore elaborazione. 
     *      Tuttavia, il codice di cui sopra è boilerplate e dovrà essere ripetuto per ogni singola 
     *      richiesta che richiede la convalida. 
     *      
     *      E, naturalmente, funzionerà solo se lo sviluppatore si ricorda di includere il controllo 
     *      di convalida in primo luogo!
     *      
     *      Fortunatamente, se stai seguendo i nostri consigli e combinando CQRS con MediatR 
     *      puoi risolvere questo problema incorporando il seguente comportamento nella tua 
     *      pipeline MediatR:
     *      
     *      Buon esempio: convalida automatica di tutte le richieste utilizzando un 
     *      comportamento della pipeline MediatR
     *      // Figura 9 : validate-client-requests-good.png
     *      
     *      Questa classe RequestValidationBehavior convaliderà automaticamente tutte le richieste 
     *      in arrivo e genererà un'eccezione ValidationException se la richiesta non è valida. 
     *      Questo è l'approccio migliore e più semplice poiché le richieste esistenti e le nuove 
     *      richieste aggiunte in seguito verranno convalidate automaticamente. 
     *      Ciò è possibile grazie alla potenza dei comportamenti della pipeline MediatR. 
     *      La documentazione per MediatR include una sezione sui comportamenti; 
     *      https://github.com/jbogard/MediatR/wiki/Behaviors. Consulta questa documentazione per 
     *      capire come migliorare i comportamenti dei gestori delle richieste e come registrare i 
     *      comportamenti della pipeline.
     *      
     *      L'unico passaggio che rimane è gestire eventuali eccezioni di convalida. 
     *      All'interno dell'app console, sarà sufficiente un blocco try catch. 
     *      L'azione intrapresa all'interno del blocco di cattura dipenderà ovviamente dalle 
     *      esigenze. All'interno dell'API Web, utilizzare un exceptionFilterAttribute per 
     *      rilevare queste eccezioni e convertirle in un risultato BadRequest come segue:
     *      
     *      Buon esempio: utilizzare un oggetto ExceptionFilterAttribute per rilevare e 
     *      gestire le eccezioni all'interno dell'API Web
     *      // Figura 10 : validate-client-requests-good-2.png
     *      
     * 5 - Sai quando usare gli oggetti Value ?
     * 
     *      Quando si definisce un dominio, le entità vengono create e sono costituite da proprietà 
     *      e metodi. Le proprietà rappresentano lo stato interno dell'entità e i metodi sono le 
     *      azioni che possono essere eseguite. Le proprietà utilizzano in genere tipi primitivi 
     *      quali stringhe, numeri, date e così via.
     *      
     *      Ad esempio, si consideri un account AD. Un account ACTIVE Directory è costituito 
     *      da un nome di dominio e da un nome utente, ad esempio SSW\Jason. 
     *      È una stringa, quindi usare il tipo di stringa ha senso. O lo fa?
     *      
     *      Esempio non valido - Memorizzazione di un account AD come stringa 
     *      (l'account AD è un tipo complesso)
     *      // Figura 11 : when-use-value-bad.png.png
     *      
     *      Un account Active Directory è un tipo complesso. Solo alcune stringhe sono account 
     *      ACTIVE directory validi. A volte si desidera la rappresentazione di stringa (SSW\Jason), 
     *      a volte è necessario il nome di dominio (SSW) e talvolta solo il nome utente (Jason). 
     *      Tutto ciò richiede logica e convalida e la logica e la convalida non possono essere 
     *      fornite dal tipo primitivo stringa. 
     *      
     *      Chiaramente, ciò che è richiesto è un tipo più complesso come un oggetto valore.
     *      
     *      Buon esempio - Archiviazione di un account Active Directory come oggetto valore 
     *      per supportare la logica e la convalida
     *      // Figura 12 : when-use-value-good.png
     *      
     *      L'implementazione sottostante per la classe AdAccount è la seguente:
     *      
     *      Buon esempio - L'implementazione dell'oggetto valore AdAccount supporta la 
     *      logica e la convalida
     *      // Figura 13 : when-use-value-good-2.png
     *      
     *      Il tipo adAccount si basa sul tipo ValueObject.
     *      Lavorare con gli account AD sarà ora facile. 
     *      Puoi creare un nuovo AdAccount con il metodo factory per come segue:
     *      
     *      Object Value Type
     *      // Figura 14 : when-use-value-eg-1.png
     *      
     *      Il metodo di fabbrica For garantisce che possano essere costruiti solo account 
     *      AD validi e per stringhe di account AD non valide, le eccezioni sono significative, 
     *      ovvero AdAccountInvalidException anziché IndexOutOfRangeException .
     *      
     *      Dato un account con nome AdAccount, puoi accedere:
     *      
     *       - Il nome di dominio che utilizza; conto. Dominio
     *       - Il nome utente che utilizza; conto. Nome
     *       - Il nome completo dell'account utilizzando; ToString()
     *       
     *     L'oggetto value supporta anche gli operatori di conversione impliciti ed espliciti. 
     *     Si può:
     *       Conversione implicita da AdAccount a stringa usando; (stringa)account
     *       Converti in modo esplicito da stringa a  AdAccount utilizzando; (AdAccount)"SSW\Jason"
     *       
     *     Se si utilizza Entity Framework Core, è inoltre necessario configurare il tipo 
     *     come segue:
     *     
     *     Utilizzo di Entity Framework Core per configurare gli oggetti valore come tipi di 
     *     entità di proprietà
     *     // Figura 15 : when-use-value-eg-2.png
     *     
     *     Con la configurazione precedente in atto, EF Core denominerà le colonne del database per 
     *     le proprietà del tipo di entità di proprietà come AdAccount_Domain e AdAccount_Name. 
     *     Per ulteriori informazioni sui tipi di entità di proprietà, consulta la documentazione 
     *     di EF Core.
     *     
     *     La prossima volta che si crea un'entità, considerare attentamente se il tipo che si 
     *     sta definendo è un tipo primitivo o un tipo complesso. I tipi primitivi funzionano 
     *     bene per l'archiviazione di stati semplici come il nome o il conteggio degli ordini, 
     *     i tipi complessi funzionano meglio quando si definiscono tipi che includono logica 
     *     complessa o convalida come indirizzi postali o di posta elettronica. 
     *     
     *     L'utilizzo di un oggetto value per incapsulare la logica e la convalida semplificherà 
     *     la progettazione complessiva.
     *     
     *     
     *     
     *      











     *    



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
