using System;

namespace DotNetDesignPatternDemos.Architectural.NLayered
{
    /*
     * Perché un'architettura a più livelli?
     * 
     *      L'utilizzo di un'architettura n-layered per le applicazioni ASP.NET offre una serie 
     *      di vantaggi, tra cui:
     *      
     *          - Separazione delle preoccupazioni: 
     *              Inserendo il codice in livelli separati, si separano le  
     *              varie parti dell'applicazione, ad esempio l'accesso ai dati, 
     *              la logica di business e l'interfaccia utente. 
     *              Ciò semplifica la progettazione  e la compilazione dell'applicazione 
     *              e rende possibile per gli sviluppatori in più discipline (database, 
     *              programmazione lato server, sviluppo frontend, progettazione) 
     *              per lavorare sull'applicazione in parallelo.
     *              
     *           - Astrazione: 
     *              Con un'architettura a più livelli è più facile guardare un'applicazione 
     *              completa e comprendere i ruoli e le responsabilità dei singoli livelli 
     *              e la relazione tra di essi. Ogni livello ha le sue responsabilità che 
     *              ti permettono di analizzarle in isolamento.
     *              
     *           - Testabilità: 
     *              Con un'architettura a più livelli, è molto più facile testare ogni livello 
     *              separatamente con unit test poiché ci sono meno dipendenze tra i vari livelli. 
     *              Ciò significa, ad esempio, che è possibile testare la logica di business o 
     *              l'interfaccia utente senza richiedere un database reale su cui eseguire il test.
     *              
     *           - Sostituibilità: - 
     *              Sarà più facile scambiare i livelli. Ad esempio, è possibile sostituire la 
     *              tecnologia di accesso ai dati senza influire sugli altri livelli più in alto 
     *              nello stack.
     *           
     *           - Riutilizzo: - 
     *              È possibile riutilizzare uno o più livelli in diverse applicazioni. 
     *              Vedrai i vantaggi di questo nella parte da 6 a 9 in cui gli stessi livelli 
     *              di accesso ai dati e di business vengono riutilizzati in quattro diverse 
     *              applicazioni frontend senza richiedere alcuna modifica ai livelli inferiori.
     *              
     *   Si noti che c'è una grande differenza tra N-Layer e N-Tiers. N-Layers si occupa di livelli
     *   software separati e consente di raggruppare il codice in modo logico all'interno 
     *   dell'applicazione. N-Tiers, d'altra parte, si occupa della posizione fisica dei componenti 
     *   software: ad esempio le macchine in cui viene eseguito il codice. 
     *   Questa esempio si occupa esclusivamente di N-Layer, anche se è possibile 
     *   riutilizzarne gran parte anche in un'applicazione A più livelli.
     *   
     *   --- Introduzione Esempio completo N-Layered ASP.NET ----
     *   
     *   Introduzione all'applicazione Contact Manager
     *   
     *   In questo esemio  utilizzerò un obiettivo di definire un Contact Manager:
     *   una semplice applicazione di gestione dei contatti che ti consente di gestire i
     *   tuoi contatti e i loro dati di contatto come numeri di telefono e indirizzi e-mail. 
     *   Tuttavia, anziché una singola applicazione demo Web Form, la nuova soluzione di esempio 
     *   include quattro diverse applicazioni frontend: un sito Web MVC 4 ASP.NET, un sito Web 
     *   Form, un progetto di servizio WCF e un'applicazione della riga di comando.
     *   
     *   Quando si avvia una delle due applicazioni Web (la versione MVC o Web Forms) viene 
     *   visualizzata la schermata iniziale con un breve testo di benvenuto. 
     *   Il menu Persone mostra un elenco con tutte le persone di contatto nel sistema con 
     *   collegamenti per modificarle ed eliminarle e per gestire i loro dati di contatto:
     *   
     *      // Figura 1.jpg
     *      
     *   Quando si fa clic su Modifica viene visualizzata la seguente pagina:
     *   
     *      // Figura 2.jpg
     *      
     *   Facendo clic su uno dei collegamenti di indirizzo per una persona di contatto nell'elenco 
     *   con persone (visibile nella Figura 1), viene visualizzata una schermata che consente di 
     *   gestire i dettagli dell'indirizzo. Nella Figura 3 viene illustrata la versione Web Form 
     *   della schermata Modifica indirizzo. L'utente ha già premuto Salva e la convalida 
     *   (dalla classe Address nel progetto Model) è stata avviata:
     *   
     *      // Figura 3.jpg
     *      
     *   Quando si fa clic sul collegamento Indirizzi e-mail o Numeri di telefono nell'elenco delle 
     *   persone di contatto, viene visualizzato un elenco di dettagli di contatto associati per 
     *   tale persona:
     *   
     *      // Figura 4.jpg
     *      
     *   Da qui, è possibile gestire i dati esistenti (modifica ed eliminazione) e creare nuovi 
     *   indirizzi e-mail per questo contatto.
     *   
     *   Il progetto WCF consente di eseguire metodi CRUD (Create, Read, Update and Delete) 
     *   contro le persone di contatto nel database in rete, utile per le interazioni 
     *   machine-to-machine.
     *   
     *   Infine, lo strumento da riga di comando mostra come importare dati da un'origine come 
     *   un file CSV per ottenere i dati esistenti nel database tramite l'API dell'applicazione.
     *   
     *   Come puoi vedere, la funzionalità è piuttosto semplice, il che rende più facile 
     *   concentrarsi sui concetti fondamentali. Tuttavia, durante la progettazione e la 
     *   creazione dell'applicazione di esempio non ho preso scorciatoie o cose semplificate 
     *   eccessivamente. Tutto ciò che vedi nella soluzione di esempio può essere utilizzato 
     *   per creare applicazioni Web reali su larga scala.
     *   
     * Cronologia dell'applicazione Contact Manager
     * 
     *  Questa è la terza versione dell'applicazione Contact Manager utilizzata per dimostrare 
     *  i concetti di progettazione N-Layer in ASP.NET. La prima versione è stata rilasciata 
     *  nel gennaio 2007 ed è arrivata come un singolo progetto di sito Web con tutta 
     *  l'interfaccia utente, l'accesso ai dati e la logica di business in un unico progetto. 
     *  La seconda versione è stata rilasciata nel novembre 2008. 
     *  È stato introdotto un progetto di sito Web per l'interfaccia utente e una serie di progetti
     *  di libreria di classi per il livello di logica di business, le entità, il livello di 
     *  accesso ai dati e la convalida.
     *  
     *  Il design precedente ha portato molti vantaggi in termini di separazione delle 
     *  preoccupazioni e del codice che era relativamente facile da capire e mantenere. 
     *  Tuttavia, ha avuto una serie di inconvenienti che lo hanno reso più difficile da 
     *  usare come ho imparato negli ultimi anni durante la creazione di siti Web e 
     *  applicazioni del mondo reale basati su questo design. Discuterò questi inconvenienti 
     *  nella prossima sezione. 
     *  La soluzione a questi inconvenienti è discussa nel resto di questa serie esempi.
     *  
     *  Margini di miglioramento
     *  
     *  Ecco un elenco di alcuni dei problemi che ho riscontrato durante la creazione 
     *  di applicazioni basate sul progetto precedente:
     *  
     *      - La soluzione richiedeva molto codice in ciascuno dei livelli. 
     *        Era necessario codice nell'oggetto dati "stupido", era necessaria 
     *        una classe Manager nel livello Business per la convalida delle regole di business, 
     *        era necessaria una classe Manager nel livello Data per l'accesso al database e 
     *        molto codice nelle stored procedure. 
     *        Probabilmente il più grande svantaggio di questo codice è che la maggior parte 
     *        di esso è ripetitivo, costringendoti a scrivere lo stesso codice più e più volte 
     *        per ciascuna delle tue entità principali implementate.
     *        
     *      - A causa dello stretto accoppiamento con il livello di database, è stata una sfida 
     *        testare sia il DAL che il codice che utilizza il livello di database, specialmente 
     *        se utilizzato in altre applicazioni come un sito Web ASP.NET MVC.
     *        
     *      - La soluzione richiedeva molte stored procedure, rendendo difficile la manutenzione 
     *        e i test. Per le operazioni CRUD semplici erano necessarie almeno quattro stored 
     *        procedure (GetItem, GetList, InsertUpdateItem e DeleteItem) mentre era necessario 
     *        ancora più codice per implementare scenari avanzati come il filtro e l'ordinamento.
     *        
     *       - L'aggiunta di membri alle entità dati è stata piuttosto difficile. Oltre ad 
     *         aggiungere il membro a una classe nel progetto BusinessEntities, era necessario 
     *         aggiungere il supporto per esso nelle varie classi Manager e nelle stored procedure. 
     *         Ciò significava molti aggiornamenti in molti posti diversi per qualcosa di semplice 
     *         come l'aggiunta di una nuova proprietà.
     *         
     *       - La soluzione conteneva molto codice per interagire con il database. Con i numerosi 
     *         sistemi ORM (Object Relational Mapping) disponibili oggi, non dovresti più scrivere 
     *         il tuo codice di accesso ai dati. Per ulteriori informazioni, 
     *         consultare: http://lostechies.com/jimmybogard/2012/07/24/dont-write-your-own-orm/.
     *         
     *       - Il framework utilizzava un proprio meccanismo di convalida. Mentre questo è servito 
     *         a me (e ad altri) bene nel corso degli anni, ora sono disponibili alternative 
     *         migliori che semplificano l'implementazione della convalida nelle entità aziendali. 
     *         Inoltre, framework come ASP.NET MVC ed Entity Framework (EF) hanno il supporto 
     *         integrato per questo nuovo meccanismo di convalida.
     *         
     *       - L'applicazione utilizzava un modello di progettazione anemica, in cui la logica 
     *         di business viene implementata in classi separate che modificano lo stato degli 
     *         oggetti del modello.  Questo è ora considerato un anti-modello.
     * 
     * Vedrai come sto affrontando queste preoccupazioni nella nuova versione dell'applicazione 
     * nei prossimi 10 step. Per darti un'idea di cosa aspettarti in questa serie, ecco un breve 
     * riassunto di ciascuno degli esempi:
     *  
     *  Parte 1 - Introduzione
     *      In questo esempio (che è quello che stai leggendo in questo momento), otterrai 
     *      una panoramica di alto livello dell'architettura e vedrai come ho impostato i 
     *      miei progetti, spazi dei nomi, classi ecc. Descriverò lo scopo e la responsabilità 
     *      di ciascuno dei progetti principali e come lavorano insieme.
     *      
     *  Parte 2 - Configurazione della soluzione in Visual Studio
     *      In questo articolo ti mostrerò come configurare la soluzione utilizzando Microsoft 
     *      Visual Studio 2012. Verrà illustrato come organizzare i progetti e la soluzione 
     *      su disco e come preparare la soluzione per l'integrazione con TFS in modo da 
     *      consentire un facile sviluppo e ramificazione del team. Ti mostrerò come usare 
     *      NuGet per aggiungere e gestire librerie di terze parti nei progetti.
     *  
     *  Parte 3 - Rendere testabile l'unità di progetto
     *      In questo articolo viene illustrato come aggiungere progetti di unit test alla 
     *      soluzione e come configurarli. Userò una libreria di terze parti chiamata 
     *      FluentAssertions per rendere i tuoi test più facili da scrivere e capire.
     *      
     *  Parte 4 - Implementazione di un modello
     *      In questo articolo verrà illustrato come configurare il modello di dominio per 
     *      l'applicazione. Prende in prestito pesantemente dall'applicazione originale 
     *      riutilizzando le classi principali del progetto BusinessEntities. 
     *      Questa parte si concentra esclusivamente sul modello di dominio, poiché 
     *      l'interazione con il database è gestita da un progetto Visual Studio separato 
     *      che utilizza EF Code First, descritto nella Parte 5.
     *      
     *  Parte 5 - Implementazione di un repository con Entity Framework 5 Code First
     *      In questo articolo verrà illustrato come utilizzare Entity Framework 5 Code First 
     *      per implementare un layer di accesso ai dati che esegue il mapping del modello a 
     *      un database sottostante (SQL Server). Ti mostrerò come utilizzare il modello di 
     *      repository per centralizzare il codice di accesso ai dati e renderlo disponibile 
     *      ad altri codici di chiamata. Questo articolo parla anche di convalida. 
     *      La convalida era una grande caratteristica della versione 3.5 del mio framework, 
     *      quindi ha senso implementarla anche nella nuova versione. Verrà illustrato come 
     *      implementare una strategia di convalida in qualche modo simile alla progettazione 
     *      precedente in quanto fornisce sia la convalida a livello di proprietà che di oggetto. 
     *      Tuttavia, l'utilizzo delle funzionalità predefinite di .NET Framework ed Entity 
     *      Framework renderà molto più semplice l'implementazione della stessa convalida in 
     *      altre applicazioni, ad esempio un sito MVC ASP.NET.
     *  
     *  Parte 6 - Mettere tutto insieme - Implementazione di un frontend
     *      MVC 4 In questo articolo vedrai come implementare un frontend MVC 4 utilizzando 
     *      il modello e i repository introdotti negli articoli precedenti. L'applicazione 
     *      demo consente di gestire le persone di contatto e i loro dettagli di contatto 
     *      come indirizzi, indirizzi e-mail e numeri di telefono. Verrà illustrato come 
     *      utilizzare Dependency Injection per inserire il repository e altre dipendenze 
     *      nei controller MVC e come i controller utilizzano il repository per ottenere 
     *      dati dentro e fuori dal database.
     *      
     * Parte 7 - Mettere tutto insieme - Implementazione di un frontend
     *      Web Forms 4.5 In questo articolo verrà illustrato come implementare un frontend 
     *      web Form ASP.NET 4.5 utilizzando il modello e i repository introdotti negli articoli 
     *      precedenti. Il frontend dell'applicazione è quasi lo stesso dell'applicazione MVC, 
     *      ma ora tutto è implementato utilizzando ASP.NET Web Form 4.5 e le nuove funzionalità 
     *      di associazione del modello introdotte in ASP.NET 4.5.
     *      
     * Parte 8 - Mettere tutto insieme - Implementazione di un frontend
     *      WCF 4.5 In questo articolo verrà illustrato come implementare un frontend del servizio 
     *      WCF 4.5 utilizzando il modello e i repository introdotti negli articoli precedenti. 
     *      Il servizio WCF consente alle applicazioni di chiamata di recuperare i contatti. 
     *      Inoltre, consente anche a un'applicazione chiamante di creare nuove persone di 
     *      contatto e modificare e / o eliminare quelle esistenti.
     *      
     * Parte 9 - Mettere tutto insieme - Importazione di dati dal vecchio database utilizzando l'API  
     *      In questo articolo viene illustrato come utilizzare l'API dell'applicazione per importare
     *      dati legacy da un'origine dati esistente, ad esempio un file CSV. Questo serve come 
     *      esempio per l'accesso ai dati utilizzando un'applicazione che non ha interfaccia 
     *      utente e che utilizza solo l'API dell'applicazione.
     *      
     * Parte 10 - Estensioni, strumenti e wrapping up
     *      Nella parte finale della serie ti mostrerò alcuni strumenti interessanti che puoi 
     *      usare quando crei applicazioni come ContactManager. Guarderò anche alcune estensioni 
     *      che potresti scrivere e poi riassumerò l'intera serie.
     *      
     * Nota: le parti 2 e 3 della serie contengono molte istruzioni pratiche e dettagliate 
     * in quanto questi articoli mostrano come configurare una soluzione come l'applicazione 
     * Spaanjaars.ContactManager da soli. 
     * 
     * È possibile utilizzare queste istruzioni praticamente così come sono per le proprie 
     * applicazioni. Le parti rimanenti della serie analizzano quindi il codice di lavoro 
     * per l'applicazione Spaanjaars.ContactManager che è possibile scaricare alla fine di 
     * ogni articolo. Mostrerò molto del codice in dettaglio e spiegherò come funziona, ma 
     * non troverai istruzioni dettagliate passo passo su come aggiungere il codice e i file 
     * ai vari progetti.
     * 
     * Panoramica dell'architettura
     * 
     *  In questa sezione ti darò una panoramica dell'applicazione completa. 
     *  
     *  Vedrai l'architettura principale, come ho impostato i vari progetti di Visual Studio 
     *  e come li ho collegati tra loro. Inoltre, vedrai molte delle classi importanti e altri 
     *  tipi all'interno di ciascuno dei progetti e imparerai a conoscere le loro responsabilità.
     *  
     *  Da un punto di vista di alto livello, l'architettura della soluzione Spaanjaars.ContactManagerV45 
     *  è la seguente:
     *  
     *      // Figura 5.jpg
     *      
     *  Le caselle blu nella parte inferiore rappresentano il livello di accesso ai dati, la casella 
     *  verde al centro rappresenta il livello aziendale e le caselle arancioni nella parte superiore 
     *  rappresentano l'interfaccia utente. Il livello aziendale contiene anche il modello con tutte 
     *  le entità principali, ma questo non è ancora mostrato in questo diagramma. 
     *  
     *  Vedrai più del modello nella Parte 4.
     *  
     *  Nella parte inferiore viene visualizzato un database sql Server che è, proprio come nella 
     *  serie precedente, il database relazionale utilizzato per l'applicazione. Sopra il database 
     *  è possibile vedere l'Entity Framework DbContext; la classe principale utilizzata per 
     *  Entity Framework 5 Code First che è quella che userò in questa serie di articoli. 
     *  Sopra questo è possibile vedere un livello contenente repository concreti che utilizzano 
     *  internamente DbContext di Entity Framework. Si noti che questa è solo una decisione di 
     *  implementazione. I repository concreti implementano le interfacce definite nel livello 
     *  verde Repository Interfaces, il che significa che è possibile scambiare i repository 
     *  concreti e Entity Framework con alternative; ad esempio, è possibile creare un repository 
     *  concreto che utilizza NHibernate o OpenAccess ORM di Telerik. 
     *  
     *  Le applicazioni dell'interfaccia utente visualizzate nella parte superiore del diagramma 
     *  non saprebbero mai di aver scambiato la tecnologia di accesso ai dati sottostante poiché 
     *  tutto ciò di cui sono a conoscenza sono le interfacce nel livello aziendale. 
     *  
     *  L'eccezione a questo è lo strumento di applicazione a riga di comando che vedrai nella 
     *  Parte 9 di questa serie. Poiché questa applicazione può essere considerata un'applicazione 
     *  una tantum o "usa e getta", non mi sono preoccupato di cercare di disaccoppiarla dai 
     *  repository concreti che utilizzano EF.
     *  
     *  Vedrai molto di più di questo nel resto di questa serie mentre scavo più a fondo nei vari 
     *  livelli e spiego come sono costruiti.
     *  
     *  Dal punto di vista di Visual Studio, l'applicazione ha l'aspetto seguente:
     *  
     *      // Figura 6.Jpg
     *      
     *  Si noti come sono state utilizzate le cartelle della soluzione di Visual Studio per 
     *  raggruppare i tipi di progetto correlati,ad esempio test e progetti frontend (UI). 
     *  In questo modo è più facile capire come è organizzata la soluzione e ti aiuta a mostrare 
     *  o nascondere rapidamente un particolare gruppo di progetti con cui stai lavorando.
     *  
     *  Nella parte inferiore del Esplora soluzioni puoi vedere tre progetti. 
     *  Il progetto Spaanjaars.Infrastructure contiene una serie di classi e interfacce 
     *  "idrauliche" utilizzate in tutta la soluzione. 
     *  
     *  Il progetto Spaanjaars.ContactManager45.Model contiene le classi di dominio principali, 
     *  ad esempio Person e Address, ed è in qualche modo simile al progetto BusinessEntities 
     *  della versione 3.5 del progetto N-Layer. Il progetto Repositories.EF contiene tutto 
     *  il codice per interagire con un database di SQL Server utilizzando Entity Framework 
     *  (EF) 5 Code First. Si noti che per i nomi dei progetti uso il modello: 
     *  Company.Project.Layer dove Company è il nome della tua azienda o del tuo cliente, 
     *  Project è il nome dell'applicazione e Layer specifica il tipo di progetto nello stack. 
     *  Ne vedete di più all'inizio della Parte 2.
     *  
     *  La cartella Frontend contiene quattro progetti dell'interfaccia utente o frontend: 
     *  uno che utilizza ASP.NET MVC 4, uno che utilizza ASP.NET Web Forms 4.5, uno che 
     *  utilizza WCF e uno strumento da riga di comando utilizzato per l'importazione di dati. 
     *  Vedrai questi progetti negli articoli successivi di questa serie. 
     *  
     *  Sotto il cofano, questi progetti fanno uso dei vari progetti Model e Repositories.
     *  
     *  Questo può sembrare un po 'travolgente, portandoti a chiederti perché hai bisogno 
     *  di così tanti progetti per un'applicazione relativamente semplice. 
     *  
     *  Se questo è il caso, è importante rendersi conto che in genere non hai bisogno di così 
     *  tanti progetti. Nella mia applicazione di esempio ho quattro diversi frontend, dimostrando 
     *  la progettazione N-Layer in vari tipi di applicazioni. 
     *  
     *  Inoltre, per questi progetti ho progetti di test separati, aumentando rapidamente il 
     *  numero totale di progetti. Per il mio nuovo design, il numero minimo di progetti 
     *  necessari è quattro: i tre progetti nella radice di Esplora soluzioni e almeno 
     *  un'applicazione frontend che utilizza questi tre progetti.
     *  
     *  Per informazioni sulla relazione tra questi progetti, considerare il diagramma modello 
     *  seguente che mostra le dipendenze dei due progetti Web frontend e WCF:
     *  
     *      // Figura 7.jpg
     *      
     *  In questa figura viene illustrato il modo in cui il progetto Model fa riferimento al 
     *  progetto Infrastructure e nient'altro (ad eccezione delle librerie .NET Framework, 
     *  ovviamente, che non sono mostrate in questo diagramma). 
     *  
     *  Il progetto Repositories.EF che utilizza Entity Framework (EF) fa riferimento al 
     *  progetto Model e al progetto Infrastructure. 
     *  
     *  I tre progetti Frontend (MVC, Web Forms e WCF) hanno un riferimento ai progetti Model 
     *  e Infrastructure e un riferimento al progetto Repositories.EF. 
     *  
     *  Quest'ultimo riferimento non è strettamente necessario in quanto tutto il codice nei 
     *  progetti dell'interfaccia utente si basa su interfacce definite nel progetto Model. 
     *  
     *  Utilizzando un framework Dependency Injection come Ninject o StructureMap è possibile 
     *  iniettare i tipi concreti nel progetto EF in fase di esecuzione, senza alcuna dipendenza 
     *  in fase di compilazione da questo progetto. Tuttavia, preferisco scrivere il codice del 
     *  programma di avvio automatico (codice che configura il framework Dependency Injection, 
     *  discusso in dettaglio nella Parte 6 e 8) nel mio progetto sui file di configurazione, 
     *  e quindi i progetti dell'interfaccia utente hanno ciascuno un riferimento al progetto EF. 
     *  Se volessi passare a un repository che utilizza un altro ORM come NHibernate o altre 
     *  tecnologie di accesso ai dati come ADO.NET, tutto ciò che dovrei fare è sostituire il 
     *  riferimento al progetto e riscrivere il codice del programma di avvio automatico. 
     *  
     *  Ne vedrai di più nella parte 5 quando verrà implementato il repository EF.
     *  
     *  Sebbene i dettagli dei singoli progetti siano discussi in modo molto dettagliato nel resto di questa serie di articoli, ecco una panoramica di come tutti i diversi progetti lavorano insieme:
     *  
     *      - Il progetto Model definisce tutte le entità principali e le relative regole di convalida. 
     *        Qui trovi classi come Person e EmailAddress. È inoltre disponibile un IPeopleRepository 
     *        che è un'interfaccia che definisce il contratto per l'utilizzo di oggetti Person. 
     *        I tipi nel progetto Model vengono utilizzati dai progetti dell'interfaccia utente. 
     *        Ad esempio, il progetto MVC utilizza Person per visualizzare informazioni sulle persone 
     *        nel sistema e accettare modifiche a tali oggetti (Insert, Update e Delete). 
     *        Questi tipi non vengono utilizzati direttamente dall'interfaccia utente (ad esempio 
     *        Visualizzazioni), ma vengono convertiti in Modelli di visualizzazione, come si vedrà 
     *        più avanti nella serie.
     *        
     *      - I progetti dell'interfaccia utente non accedono direttamente al database per ottenere 
     *        i propri dati. Invece, usano repository che a loro volta accedono al database. 
     *        Un repository semplifica la centralizzazione del codice di accesso ai dati e la sua 
     *        disponibilità ad altri codici di chiamata. Nella mia applicazione, il progetto Model 
     *        definisce il contratto per il repository che viene quindi implementato nel progetto 
     *        Repositories.EF. Questo progetto utilizza Entity Framework sotto il cofano per 
     *        ottenere dati all'interno e all'esterno del database.
     *        
     *       - MVC e altri progetti dell'interfaccia utente utilizzano un PeopleRepository concreto 
     *         del progetto Repositories.EF. Tuttavia, non dispongono di un collegamento hardcoded 
     *         a questa classe in quanto ciò renderebbe difficile sostituire EF con un'altra 
     *         tecnologia di database e unit test delle applicazioni dell'interfaccia utente. 
     *         Invece, i progetti dell'interfaccia utente funzionano con l'interfaccia 
     *         IPeopleRepository, mentre un'implementazione EF concreta viene fornita in fase di 
     *         esecuzione utilizzando un concetto chiamato Dependency Injection.
     *         
     *       - Il progetto Spaanjaars.Infrastructure fornisce servizi pipeline di basso livello 
     *         utilizzati da tutti gli altri progetti.
     *         
     *  I vari progetti di test hanno riferimenti ad altre parti dell'applicazione che stanno 
     *  testando. Ad esempio, il progetto di test di integrazione ha un riferimento al progetto 
     *  Repositories.EF in quanto accede a un database reale durante i test.
     *  
     *  Spaanjaars.Infrastruttura
     *  
     *      Questo è un progetto abbastanza semplice con solo poche classi, mostrato nella Figura 8:
     *      
     *          // Figura 8.jpg
     *       
     *  Come puoi vedere dal suo nome, questo progetto non è direttamente legato all'applicazione 
     *  ContactManager, invece, l'ho inserito nel più generale spazio dei nomi Spaanjaars.Infrastructure 
     *  (che potrebbe essere il nome della tua azienda o altro spazio dei nomi a livello di root che 
     *  potresti usare) in modo che possa essere facilmente riutilizzato su più progetti. 
     *  
     *  Questo progetto fornisce tre interfacce idrauliche utilizzate dal resto dell'applicazione. 
     *  La Figura 9 mostra il diagramma classi per questo progetto:
     *  
     *      // Figura 9.jpg
     *      
     *  L'interfaccia IRepository definisce il contratto che i repository concreti devono implementare. 
     *  Definisce i membri con cui interagisci per ottenere dati all'interno e all'esterno dell'origine 
     *  dati sottostante. Vedrai un'implementazione di questa interfaccia insieme alle interfacce 
     *  relative all'unità di lavoro nella Parte 5 quando vedrai come creare un repository concreto 
     *  usando Entity Framework. È facile creare il proprio repository destinato a un database o ORM 
     *  diverso. Tutto quello che deve fare è implementare questa interfaccia e quindi è possibile 
     *  collegarla a un'altra applicazione come un sito Web MVC ASP.NET pubblico.
     *  
     *   DomainObject<T> e ValueObject<T> sono le classi di base per le varie classi di dominio 
     *   nel progetto Model. DomainObject<T> è la classe base per le entità che hanno un'identità 
     *   e viene utilizzata da classi come Person. ValueObject<T> viene utilizzato da oggetti con 
     *   valore puro che non hanno una propria identità. Nell'applicazione di esempio Address è 
     *   stato implementato come ValueObject<T> per illustrare le differenze. 
     *   
     *   Ne vedrai di più nella Parte 3.
     *   
     *   Infine, nella Parte 5 verranno visualizzati i tipi nella cartella DataContextStorage e 
     *   verranno illustrati la durata di un contesto di oggetto Entity Framework.
     *   
     *   Spaanjaars.ContactManager45.Model
     *   
     *   Il progetto Model è in qualche modo simile al progetto BusinessEntities nella versione 
     *   .NET 3.5 di questa applicazione. Presenta i tipi principali dell'applicazione come 
     *   Person, Address e PhoneNumber. Presenta anche una serie di raccolte e alcune enumerazioni 
     *   per definire rispettivamente i tipi di record di contatto e le persone. 
     *   
     *   Ecco l'aspetto di Esplora soluzioni per il progetto:
     *   
     *      // Figura 10.jpg
     *      
     *   Notare i quattro tipi principali: Indirizzo, Indirizzo e-mail, Persona e Numero di 
     *   telefono. Se hai letto la serie di articoli precedenti, questi dovrebbero sembrare 
     *   tutti familiari (tranne che Person era precedentemente chiamato ContactPerson). 
     *   
     *   Per motivi dimostrativi, ho lasciato che Address ereditasse da ValueObject<T> il 
     *   che significa che è considerato un tipo di valore mentre tutte le altre classi 
     *   ereditano da DomainObject<T>. 
     *   
     *   Questi ultimi tre tipi hanno anche una controparte di raccolta che eredita dal tipo 
     *   CollectionBase<T> personalizzato e generico.
     *   
     *   L'interfaccia IPeopleRepository fornisce un contratto su cui lavorano le altre 
     *   applicazioni del progetto. Vedrai molto di più di questo nella Parte 4 e 5 di 
     *   questa serie.
     *   
     *   Nella Figura 11 viene illustrato il diagramma classi completo per il progetto Model. 
     *   
     *   Negli articoli successivi di questa serie scaverò più a fondo nei vari tipi e nei 
     *   loro membri.
     *   
     *      // Figura 11.jpg
     *      
     *  Spaanjaars.ContactManager45.Repositories.EF
     *  
     *   Questo progetto contiene tutta l'implementazione per l'utilizzo di persone di contatto 
     *   in un database di SQL Server utilizzando Entity Framework 5 Code First. 
     *   Nella Figura 12 viene illustrato Esplora soluzioni per questo progetto.
     *   
     *      // Figura 12.jpg
     *      
     *   Questo progetto contiene implementazioni concrete del repository e delle interfacce relative 
     *   all'unità di lavoro che hai visto in precedenza. 
     *   Inoltre, contiene una serie di classi relative all'impostazione di Entity Framework e 
     *   all'inizializzazione e alla configurazione del database utilizzando l'API fluent e le 
     *   classi strategiche di creazione del database. 
     *   
     *   Vedete come funziona tutto questo quando si configura Entity Framework nella Parte 5. 
     *   Per ora, ecco il diagramma classi completo:
     *   
     *      // Figura 13.jpg
     *      
     * Spaanjaars.ContactManager45.Web.Mvc
     *   
     *   Questo progetto contiene l'implementazione MVC 4 ASP.NET del frontend per lavorare con 
     *   le persone di contatto e i loro dati di contatto associati in un'applicazione web. 
     *   È discusso in dettaglio nella Parte 6 di questa serie di articoli. 
     *   Qui vedrai l'iniezione di dipendenze al lavoro quando i repository concreti per Entity 
     *   Framework (o qualsiasi altro tipo che crei) vengono iniettati nell'applicazione in fase 
     *   di esecuzione.
     *   
     * Spaanjaars.ContactManager45.Web.WebForms
     *   
     *   Questo progetto contiene l'implementazione Web Form del frontend per lavorare con le 
     *   persone di contatto e i dati di contatto associati in un'applicazione Web. 
     *   È discusso in dettaglio nella Parte 7 di questa serie di articoli.
     *   
     * Spaanjaars.ContactManager45.Web.Wcf
     * 
     *   Questo progetto contiene un servizio WCF per lavorare con le persone di contatto nel 
     *   sistema su servizi remoti. Il servizio WCF dispone di metodi per recuperare, aggiungere, 
     *   aggiornare ed eliminare persone dal sistema. Il progetto WCF è descritto in dettaglio 
     *   nella Parte 8 di questa serie di articoli.
     *   
     * Spaanjaars.ContactManager45.Import
     * 
     *   Questo progetto contiene uno strumento da riga di comando in grado di importare le 
     *   persone di contatto e i relativi dati di contatto da un file CSV. 
     *   Lo scopo di questo progetto è dimostrare come utilizzare l'API pubblica dell'applicazione 
     *   da altre applicazioni. La parte 9 di questa serie mostra come ho costruito lo strumento 
     *   di importazione.
     *   
     *   Oltre alle librerie di base e ai quattro progetti frontend, la soluzione contiene quattro 
     *   progetti di test, ben raggruppati in una cartella della soluzione denominata Test. 
     * 
     *   Vedrai come impostare i progetti di test nella Parte 3. I test vengono quindi aggiunti a 
     *  questi progetti nel resto degli articoli.
     *  
     * Spaanjaars.ContactManager45.Tests.Unit
     * 
     *   Questo progetto contiene unit test per la soluzione. Troverai alcuni test di base per 
     *   le entità e i loro membri, test per la convalida e altro ancora. La parte 3 di questa 
     *   serie scava più a fondo in questo progetto.
     *   
     * Spaanjaars.ContactManager45.Tests.Integration
     * 
     *   Poiché questa applicazione si basa fortemente su un database, ha senso disporre di 
     *   una serie di test di integrazione che utilizzano il database. In questo modo, è 
     *   possibile testare l'interazione dei vari componenti, nonché alcune logiche specifiche 
     *   del database come i vincoli univoci. Ancora una volta la Parte 3 di questa serie scava 
     *   più a fondo in questo progetto.
     *   
     * Spaanjaars.ContactManager45.Tests.Frontend.Mvc
     * 
     *   In questo progetto troverai una serie di test per il frontend MVC ASP.NET. Sebbene 
     *   lo scopo di questo articolo non sia quello di mostrare come scrivere unit test per 
     *   MVC o altri framework applicativi, i test in questo progetto servono a dimostrare 
     *   che con il framework presentato in questa serie, lo unit test è facile a causa del 
     *   modo in cui è possibile inserire tipi concreti utilizzando un framework di Dependency 
     *   Injection mentre i programmi applicativi su un'interfaccia. 
     *   In questo modo è molto più semplice testare i controller che hanno dipendenze da 
     *   componenti come i repository.
     *   
     * Spaanjaars.ContactManager45.Tests.Frontend.Wcf
     * 
     *   In questo progetto sono disponibili numerosi test per il progetto di servizi WCF. 
     *   Proprio come con il progetto MVC, sto usando Dependency Injection per disaccoppiare 
     *   i metodi di servizio dalle loro dipendenze come i repository per abilitare gli unit test.
     *   
     * Questa serie è intesa come introduzione all'architettura di applicazioni Web a più livelli 
     * utilizzando ASP.NET 4.5 e Entity Framework 5 Code First.
     * 
     * Ciò significa che scaverò il più a fondo possibile in queste tecnologie per spiegare 
     * l'argomento. 
     * 
     * Tuttavia, significa anche che non fornirò molti dettagli sui problemi collaterali. 
     * 
     * Ad esempio; Potrei utilizzare un framework open source per semplificare gli unit test 
     * nell'applicazione di esempio, ma non approfondirò i dettagli su come recuperare e 
     * installare questo framework o su come configurarlo per l'applicazione di esempio e 
     * su come usarlo.
     * 
     * Il file compresso del Progetto Soluzione presentato qui è 
     * 
     *      // NLayer-4.5.0.0-Source.zip
     * 
     * c'è anche la versione del Progetto usando come DI il framework Ninjecg piuttosto che StructureMap
     * 
     *      // NLayer-4.5.0.0-Source-with-ninject.zip
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
