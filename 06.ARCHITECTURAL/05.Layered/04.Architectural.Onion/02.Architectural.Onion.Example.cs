using System;

namespace DotNetDesignPatternDemos.Architectural.Onion.Example
{
    /*
        * Per questo esempio di Architettura basata sul modello di progettazione Onion
        * riporteremeno un esempio completo per asp.net core e sul quale adottiamo una
        * infrastruttura a cipolla basata su 4 livelli.
        * 
        *      -    Livello di dominio
        *      -    Livello di servizio
        *      -    Livello di infrastruttura
        *      -    Livello di presentazione
        *      
        *      // Figura 1 : onion_architecture.jpeg
        *      
        *  Concettualmente, possiamo considerare che i livelli Infrastruttura e Presentazione si 
        *  trovano allo stesso livello della gerarchia.
        *  
        *  Ora, andiamo avanti e guardiamo ogni livello con più dettagli per vedere perché lo 
        *  stiamo introducendo e cosa creeremo all'interno di quel livello.
        *  
        *  Vantaggi dell'architettura Onion
        *  
        *  Diamo un'occhiata a quali sono i vantaggi dell'architettura Onion e perché 
        *  vorremmo implementarla nei nostri progetti.
        *  
        *  Tutti i livelli interagiscono tra loro rigorosamente attraverso le interfacce 
        *  definite nei livelli sottostanti. 
        *  
        *  Il flusso delle dipendenze è verso il nucleo della Cipolla. 
        *  Spiegheremo perché questo è importante nella prossima sezione.
        *  
        *  L'utilizzo dell'inversione delle dipendenze in tutto il progetto, a seconda delle astrazioni 
        *  (interfacce) e non delle implementazioni, ci consente di cambiare l'implementazione in fase 
        *  di esecuzione in modo trasparente. 
        *  
        *  Dipendiamo dalle astrazioni in fase di compilazione, il che ci dà contratti rigorosi con 
        *  cui lavorare, e ci viene fornita l'implementazione in fase di esecuzione.
        *  
        *  La testabilità è molto alta con l'architettura Onion perché tutto dipende dalle 
        *  astrazioni. 
        *  
        *  Le astrazioni possono essere facilmente derise con una libreria di derisione come Moq. 
        *  Per ulteriori informazioni sugli unit test dei progetti in ASP.NET Core, consulta 
        *  questo articolo Test dei controller MVC in ASP.NET Core.
        *  
        *  Possiamo scrivere la logica di business senza preoccuparci di nessuno dei dettagli 
        *  di implementazione. Se abbiamo bisogno di qualcosa da un sistema o servizio esterno, 
        *  possiamo semplicemente creare un'interfaccia per esso e consumarlo. 
        *  
        *  Non dobbiamo preoccuparci di come verrà attuato. Gli strati più alti della cipolla 
        *  si occuperanno di implementare quell'interfaccia in modo trasparente.
        *  
        *  Flusso di dipendenze
        *  
        *  L'idea principale alla base dell'architettura Onion è il flusso di dipendenze, o 
        *  meglio il modo in cui i livelli interagiscono tra loro. Più profondo è lo strato 
        *  che risiede all'interno della cipolla, meno dipendenze ha.
        *  
        *  Il livello Dominio non ha dipendenze dirette dai livelli esterni. È isolato, in un 
        *  certo senso, dal mondo esterno. I livelli esterni sono tutti autorizzati a fare 
        *  riferimento ai livelli che si trovano direttamente sotto di loro nella gerarchia.
        *  
        *  Possiamo concludere che tutte le dipendenze nell'architettura Onion fluiscono verso l'interno. 
        *  Ma dovremmo chiederci, perché è importante?
        *      Il flusso di dipendenze determina ciò che un determinato livello nell'architettura 
        *      Onion può fare. Poiché dipende dai livelli sottostanti nella gerarchia, può chiamare 
        *      solo i metodi esposti dai livelli inferiori.
        *      
        *      Possiamo usare gli strati inferiori dell'architettura Onion per definire contratti 
        *      o interfacce. I livelli esterni dell'architettura implementano queste interfacce. 
        *      Ciò significa che nel livello Dominio, non ci preoccupiamo dei dettagli dell'infrastruttura 
        *      come il database o i servizi esterni.
        *      
        *      Utilizzando questo approccio, possiamo incapsulare tutta la ricca logica di 
        *      business nei livelli Dominio e Servizio senza dover mai conoscere i dettagli 
        *      dell'implementazione. Nel livello Servizio, dipenderemo solo dalle interfacce 
        *      definite dal livello sottostante, che è il livello Dominio.
        *      
        * Basta teoria, vediamo un po' di codice. Abbiamo già preparato un progetto funzionante 
        * per te e esamineremo ciascuno dei progetti nella soluzione e parleremo di come si 
        * adattano all'architettura Onion.
        *   
        * Struttura della soluzione
        *      
        *      // Figura 2 : solution_structure.png
        *      
        * Diamo un'occhiata alla struttura della soluzione che utilizzeremo:
        * 
        *  Struttura della soluzione del progetto di architettura Onion in Visual Studio.
        *  
        *  Come possiamo vedere, consiste nel progetto, che è la nostra applicazione ASP.NET Core, 
        *  e sei librerie di classi. Il progetto manterrà l'implementazione del livello dominio. 
        *  La e sarà la nostra implementazione a livello di servizio. Il progetto sarà il nostro 
        *  livello infrastruttura e il progetto sarà l'implementazione del livello di presentazione.
        *  Web Domain Services Services.Abstractions Persistence Presentation
        *  
        *  Livello di dominio
        *  
        *  Il livello Dominio è al centro dell'architettura Onion. In questo livello, in genere 
        *  definiremo gli aspetti principali del nostro dominio:
        *
        *  Entità
        *  Interfacce del repository
        *  Eccezioni
        *  Servizi di dominio
        *  
        *  Questi sono solo alcuni degli esempi di ciò che potremmo definire nel livello Dominio. 
        *  Possiamo essere più o meno severi, a seconda delle nostre esigenze. 
        *  Dobbiamo renderci conto che tutto è un compromesso nell'ingegneria del software.
        *  
        *  Iniziamo guardando le classi di entità e , sotto la cartella:
        *  Owner Account Entities
        *  
        *      public class Owner 
        *      { 
        *          public Guid Id { get; set; }
        *          public string Name { get; set; }
        *          public DateTime DateOfBirth { get; set; }
        *          public string Address { get; set; }
        *          public ICollection<Account> Accounts { get; set; }
        *      }
        *  
        *      public class Account 
        *      {
        *          public Guid Id { get; set; }
        *          public DateTime DateCreated { get; set; }
        *          public string AccountType { get; set; }
        *          public Guid OwnerId { get; set; }
        *      }
        *      
        *  Le entità definite nel livello Dominio acquisiranno le informazioni importanti 
        *  per descrivere il dominio problematico.
        *  
        *  A questo punto, dovremmo chiederci che dire del comportamento? 
        *  Un modello di dominio anemico non è una brutta cosa?
        *  Dipende. Se si dispone di una logica di business molto complessa, avrebbe senso 
        *  incapsularla all'interno delle nostre entità di dominio. Ma per la maggior parte 
        *  delle applicazioni, di solito è più facile iniziare con un modello di dominio più 
        *  semplice e introdurre complessità solo se richiesto dal progetto.
        *  
        *  Successivamente, esamineremo il e le interfacce all'interno della cartella:
        *  IOwnerRepository IAccountRepository Repositories
        *  
        *      public interface IOwnerRepository
        *      {
        *          Task<IEnumerable<Owner>> GetAllAsync(CancellationToken cancellationToken = default);
        *          Task<Owner> GetByIdAsync(Guid ownerId, CancellationToken cancellationToken = default);
        *          void Insert(Owner owner);
        *          void Remove(Owner owner);
        *      }
        *
        *      public interface IAccountRepository
        *      {
        *          Task<IEnumerable<Account>> GetAllByOwnerIdAsync(Guid ownerId, CancellationToken cancellationToken = default);
        *          Task<Account> GetByIdAsync(Guid accountId, CancellationToken cancellationToken = default);
        *          void Insert(Account account);
        *          void Remove(Account account);
        *      }
        *
        *  All'interno della stessa cartella, possiamo anche trovare l'interfaccia :IUnitOfWork
        *  
        *      public interface IUnitOfWork
        *      {
        *          Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        *      }
        *
        * Si noti che l'argomento viene impostato come valore facoltativo e gli viene assegnato il 
        * valore. Con questo approccio, se non forniamo un valore effettivo , ci verrà fornito un 
        * valore. In questo modo, possiamo garantire che le nostre chiamate asincrone che 
        * utilizzano il funzioneranno sempre.
        * CancellationToken defaultCancellationToken CancellationToken.None CancellationToken
        * 
        * Eccezioni di dominio
        * Ora, diamo un'occhiata ad alcune delle eccezioni personalizzate che abbiamo all'interno 
        * della cartella.
        * Exceptions
        * 
        * C'è una classe astratta :BadRequestException
        * 
        *      public abstract class BadRequestException : Exception
        *      {
        *          protected BadRequestException(string message)
        *              : base(message)
        *          {
        *          }
        *      }
        * 
        * E la classe astratta :NotFoundException
        * 
        *      public abstract class NotFoundException : Exception
            {
                protected NotFoundException(string message)
                    : base(message){}
            }
        * 
        * Esistono anche un paio di classi di eccezioni che ereditano dalle eccezioni astratte 
        * per descrivere scenari specifici che possono verificarsi nell'applicazione:
        * 
        *      public sealed class AccountDoesNotBelongToOwnerException : BadRequestException
        *      {
        *          public AccountDoesNotBelongToOwnerException(Guid ownerId, Guid accountId)
        *              : base($"The account with the identifier {accountId} does not belong to the owner with the identifier {ownerId}")
        *          {
        *          }
        *      }
        *
        *      public sealed class OwnerNotFoundException : NotFoundException
        *      {
        *          public OwnerNotFoundException(Guid ownerId)
        *              : base($"The owner with the identifier {ownerId} was not found.")
        *          {
        *          }
        *      }
        *
        *      public sealed class AccountNotFoundException : NotFoundException
        *      {
        *          public AccountNotFoundException(Guid accountId)
        *              : base($"The account with the identifier {accountId} was not found.")    
        *          {
        *          }
        *      }
        *
        * Queste eccezioni saranno gestite dagli strati superiori della nostra architettura. 
        * Li useremo in un gestore di eccezioni globale che restituirà il codice di stato HTTP 
        * corretto in base al tipo di eccezione generata.
        *
        *  A questo punto, sappiamo come definire il livello Dominio. Detto questo, possiamo 
        *  passare al livello Di servizio e vedere come usarlo per implementare la logica di 
        *  business effettiva.
        *  
        *  Livello di servizio
        *  
        *  Il livello Di servizio si trova proprio sopra il livello Dominio, il che significa 
        *  che ha un riferimento al livello Dominio. Il livello Servizio è diviso in due progetti 
        *  Services AbstractionsServices
        *  
        *  Nel progetto è possibile trovare le definizioni per le interfacce di servizio che 
        *  andranno a incapsulare la logica di business principale. Inoltre, stiamo usando il 
        *  progetto per definire gli oggetti di trasferimento dati (DTO) che utilizzeremo con 
        *  le interfacce del servizio.
        *  Services AbstractionsContracts
        *  
        *  Diamo prima un'occhiata alle interfacce e alle interfacce:
        *  IOwnerService IAccountService
        *  
        *      public interface IOwnerService
        *      {
        *          Task<IEnumerable<OwnerDto>> GetAllAsync(CancellationToken cancellationToken = default);
        *          Task<OwnerDto> GetByIdAsync(Guid ownerId, CancellationToken cancellationToken = default);
        *          Task<OwnerDto> CreateAsync(OwnerForCreationDto ownerForCreationDto, CancellationToken cancellationToken = default);
        *          Task UpdateAsync(Guid ownerId, OwnerForUpdateDto ownerForUpdateDto, CancellationToken cancellationToken = default);
        *          Task DeleteAsync(Guid ownerId, CancellationToken cancellationToken = default);
        *      }
        *
        *      public interface IAccountService
        *      {
        *          Task<IEnumerable<AccountDto>> GetAllByOwnerIdAsync(Guid ownerId, CancellationToken cancellationToken = default);
        *          Task<AccountDto> GetByIdAsync(Guid ownerId, Guid accountId, CancellationToken cancellationToken);
        *          Task<AccountDto> CreateAsync(Guid ownerId, AccountForCreationDto accountForCreationDto, CancellationToken cancellationToken = default);
        *          Task DeleteAsync(Guid ownerId, Guid accountId, CancellationToken cancellationToken = default);
        *      }
        *
        *  Inoltre, possiamo vedere che esiste un'interfaccia che funge da wrapper attorno alle due interfacce che abbiamo creato in precedenza:
        *  IServiceManager
        *  
        *      public interface IServiceManager
        *      {
        *          IOwnerService OwnerService { get; }
        *          IAccountService AccountService { get; }
        *      }
        *
        *  Successivamente, vedremo come implementare queste interfacce all'interno del progetto.
        *  Iniziamo con il :ServicesOwnerService
        *  
        *      // Code 1
        *  
        *  Quindi ispezioniamo la classe:AccountService
        *  
        *      // Code 2
        *  
        *  E infine il :ServiceManager
        *  
        *      // Code 3
        *      
        *  La parte interessante con l'implementazione è che stiamo sfruttando la potenza della 
        *  classe per garantire l'inizializzazione pigra dei nostri servizi. 
        *  Ciò significa che le nostre istanze di servizio verranno create solo quando vi 
        *  accediamo per la prima volta e non prima. 
        *  ServiceManagerLazy
        *  
        *  Qual è la motivazione per dividere il livello Servizio?
        *  
        *  Perché stiamo attraversando così tanti problemi per dividere le nostre interfacce e 
        *  implementazioni di servizio in due progetti separati?
        *  
        *  Come puoi vedere, contrassegniamo le implementazioni del servizio con la parola 
        *  chiave, il che significa che non saranno disponibili pubblicamente al di fuori 
        *  del progetto. D'altra parte, le interfacce di servizio sono pubbliche.
        *  internalServices
        *  
        *  Vi ricordate quello che abbiamo detto sul flusso delle dipendenze?
        *  
        *  Con questo approccio, siamo molto espliciti su ciò che gli strati più alti della 
        *  cipolla possono e non possono fare. È facile perdere qui che il progetto non ha 
        *  un riferimento al progetto.
        *  Services AbstractionsDomain
        *  
        *  Ciò significa che quando un livello superiore fa riferimento al progetto, sarà in 
        *  grado di chiamare solo i metodi esposti da questo progetto. Vedremo perché questo 
        *  è molto utile in seguito quando arriveremo al livello Presentazione.
        *  Services Abstractions
        *  
        *  Livello di infrastruttura
        *  
        *  Il livello Infrastruttura dovrebbe preoccuparsi di incapsulare tutto ciò che riguarda 
        *  i sistemi o i servizi esterni con cui la nostra applicazione interagisce. 
        *  Questi servizi esterni possono essere:
        *      Banca dati
        *      Provider di identità
        *      Coda di messaggistica
        *      Servizio di posta elettronica
        *      
        *  Ci sono altri esempi, ma si spera che tu abbia l'idea. Stiamo nascondendo tutti i 
        *  dettagli di implementazione nel livello Infrastruttura perché si trova nella parte 
        *  superiore dell'architettura Onion, mentre tutti i livelli inferiori dipendono dalle 
        *  interfacce (astrazioni).
        *  
        *  Innanzitutto, esamineremo il contesto del database di Entity Framework nella classe:
        *  RepositoryDbConext
        *  
        *      public sealed class RepositoryDbContext : DbContext
        *      {
        *          public RepositoryDbContext(DbContextOptions options)
        *              : base(options)
        *          {
        *          }
        *
        *          public DbSet<Owner> Owners { get; set; }
        *
        *          public DbSet<Account> Accounts { get; set; }
        *
        *          protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        *              modelBuilder.ApplyConfigurationsFromAssembly(typeof(RepositoryDbContext).Assembly);
        *      }
        *  
        * Come puoi vedere, l'implementazione è estremamente semplice. Tuttavia, nel metodo, stiamo 
        * configurando il contesto del database in base alle configurazioni dell'entità dallo stesso 
        * assembly.
        * OnModelCreating
        * 
        * Successivamente, esamineremo le configurazioni delle entità che implementano 
        * l'interfaccia . Possiamo trovarli all'interno della cartella:
        * IEntityTypeConfiguration<T> Configurations
        * 
        *      // Code 4
        *      
        * Ottimo, ora che il contesto del database è configurato, possiamo passare ai repository.
        * Esamineremo le implementazioni del repository all'interno della cartella. 
        * I repository stanno implementando le interfacce che abbiamo definito nel progetto:
        * RepositoriesDomain
        * 
        *      // Code 5
        * 
        * Ottimo, abbiamo finito con il livello Infrastruttura. Ora abbiamo solo un altro livello 
        * rimasto per completare la nostra implementazione dell'architettura Onion.
        * 
        * Livello di presentazione
        * 
        * Lo scopo del livello Presentazione è quello di rappresentare il punto di ingresso al 
        * nostro sistema in modo che i consumatori possano interagire con i dati. 
        * Possiamo implementare questo livello in molti modi, ad esempio creando un'API REST, 
        * gRPC, ecc.
        * 
        * Stiamo utilizzando un'API Web creata con ASP.NET Core per creare un 
        * set di endpoint API RESTful per modificare le entità di dominio e consentire ai 
        * consumatori di recuperare i dati.
        * 
        * Tuttavia, faremo qualcosa di diverso da quello a cui sei normalmente abituato quando 
        * crei API Web. Per convenzione, i controller sono definiti nella cartella all'interno 
        * dell'applicazione Web.
        * 
        * Perché questo è un problema? Poiché ASP.NET Core utilizza l'inserimento delle 
        * dipendenze ovunque, è necessario disporre di un riferimento a tutti i progetti nella 
        * soluzione dal progetto di applicazione Web. Questo ci permette di configurare i nostri 
        * servizi all'interno della classe.
        * ControllersStartup
        * 
        * Mentre questo è esattamente ciò che vogliamo fare, introduce un grosso difetto di 
        * progettazione. Cosa impedisce ai nostri controllori di iniettare tutto ciò che vogliono 
        * all'interno del costruttore? Niente!
        * 
        * Controller puliti
        * 
        * Con l'approccio standard ASP.NET Core, non possiamo impedire a nessuno di iniettare 
        * tutto ciò di cui ha bisogno all'interno di un controller. Quindi, come possiamo 
        * imporre alcune regole più severe su ciò che i controllori possono fare?
        * 
        * Ti ricordi come abbiamo diviso il livello di servizio in progetti e ? 
        * Questo era un pezzo del puzzle.
        * Services AbstractionsServices
        * 
        * Stiamo creando un progetto chiamato e dandogli un riferimento al pacchetto NuGet in 
        * modo che abbia accesso alla classe. Quindi possiamo creare i nostri controller 
        * all'interno di questo progetto.
        * PresentationMicrosoft AspNetCore.Mvc.CoreControllerBase
        * 
        * Diamo un'occhiata all'interno della cartella del progetto:
        * OwnersController Controllers
        * 
        * E diamo anche un'occhiata al :
        * AccountsController
        * 
        *   // Code 6
        *
        * Ormai dovrebbe essere ovvio che il progetto avrà solo un riferimento al progetto. 
        * E poiché il progetto non fa riferimento a nessun altro progetto, abbiamo imposto 
        * una serie molto rigorosa di metodi che possiamo chiamare all'interno dei nostri 
        * controllori.
        * Presentation Services.AbstractionServices.Abstractions
        * 
        * L'ovvio vantaggio dell'architettura Onion è che i metodi del nostro controller 
        * diventano molto sottili. Solo un paio di righe di codice al massimo. 
        * Questa è la vera bellezza dell'architettura Onion. 
        * Abbiamo spostato tutta la logica di business importante nel livello Servizio.
        * 
        * Ottimo, abbiamo visto come implementare il livello Presentazione.
        * 
        * Ma come useremo il controller se non è nell'applicazione Web? 
        * Bene, passiamo alla sezione successiva per scoprirlo.
        * 
        * Costruire la cipolla ( I Livelli)
        * 
        * Complimenti se sei arrivato fin qui. Ti abbiamo mostrato come implementare il 
        * livello Dominio, il Livello Servizio e il Livello Infrastruttura. Inoltre, 
        * abbiamo mostrato l'implementazione del livello Presentazione disaccoppiando 
        * i controller dall'applicazione Web principale.
        * 
        * Rimane solo un piccolo problema. L'applicazione non funziona affatto! 
        * Non abbiamo visto come collegare nessuna delle nostre dipendenze.
        * 
        * Configurazione dei Servizi
        * 
        * Viene illustrato come vengono registrate tutte le dipendenze del servizio richieste 
        * all'interno della classe per .NET 5 nel progetto. Daremo un'occhiata al metodo:
        * StartupWebConfigureServices
        * 
        *   // Code 7
        *   
        * Senza questa riga di codice, l'API Web non funzionerebbe. Questa riga di codice 
        * troverà tutti i controller all'interno del progetto e li configurerà con il framework. 
        * Saranno trattati come se fossero definiti convenzionalmente.
        * Presentation
        * 
        * Ottimo, abbiamo visto come abbiamo cablato tutte le dipendenze della nostra 
        * applicazione. Tuttavia, ci sono ancora un paio di cose di cui occuparsi.
        * 
        * Creazione di un gestore di eccezioni globali
        * 
        * Ricordi che abbiamo due classi di eccezioni astratte e all'interno del livello Dominio? 
        * Vediamo come possiamo farne buon uso.
        * BadRequestException NotFoundException
        * 
        * Esamineremo la classe del gestore delle eccezioni globale , che può essere trovata 
        * all'interno della cartella:
        * ExceptionHandlingMiddleware Middlewares
        *   
        *   // Code 8
        *   
        * Si noti che viene creata un'espressione di commutazione attorno all'istanza di eccezione e 
        * quindi viene eseguita una corrispondenza di pattern in base al tipo di eccezione. 
        * Quindi, stiamo modificando il codice di stato HTTP della risposta a seconda del tipo 
        * di eccezione specifico.
        * 
        * Successivamente, dobbiamo registrare il con la pipeline middleware ASP.NET Core affinché 
        * funzioni correttamente:
        * ExceptionHandlingMiddleware
        * 
        * ...
        * app.UseMiddleware<ExceptionHandlingMiddleware>();
        * ...
        * 
        * Dobbiamo anche registrare la nostra implementazione middleware all'interno del metodo 
        * della classe:
        * ConfigureService Startup
        * 
        * ...
        * services.AddTransient<ExceptionHandlingMiddleware>();
        * ...
        * 
        * oppure in .NET 6:
        * ...
        * builder.Services.AddTransient<ExceptionHandlingMiddleware>();
        * ...
        * 
        * Senza registrare il con il contenitore delle dipendenze, otterremmo un'eccezione di 
        * runtime e non vogliamo che ciò accada.
        * ExceptionHandlingMiddleware
        * 
        * Gestione delle migrazioni di database
        * 
        * Esamineremo un ultimo miglioramento del progetto, che lo rende più facile da usare 
        * per tutti, e poi abbiamo finito.
        * 
        * Per semplificare il download del codice dell'applicazione ed essere in grado di 
        * eseguire l'applicazione localmente, stiamo usando Docker. Con stiamo avvolgendo 
        * la nostra applicazione ASP.NET Core all'interno di un contenitore Docker. 
        * Stiamo anche utilizzando per raggruppare il nostro contenitore di applicazioni 
        * Web con un contenitore che esegue l'immagine del database PostgreSQL. 
        * In questo modo, non avremo bisogno di avere PostgreSQL installato sul nostro sistema.
        * DockerDocker Compose
        * 
        * Tuttavia, poiché l'applicazione Web e il server di database verranno eseguiti 
        * all'interno dei contenitori, come creeremo il database effettivo per l'applicazione 
        * da utilizzare?
        * 
        * È possibile creare uno script di inizializzazione, connettersi al contenitore 
        * Docker mentre è in esecuzione il server di database ed eseguire lo script. 
        * Ma questo è un sacco di lavoro manuale, ed è soggetto a errori. 
        * Fortunatamente, c'è un modo migliore.
        * 
        * Per farlo in modo elegante, useremo le migrazioni di Entity Framework Core ed 
        * eseguiremo le migrazioni dal nostro codice all'avvio dell'applicazione. 
        * Per vedere come abbiamo raggiunto questo obiettivo, dai un'occhiata alla 
        * classe nel progetto:
        * Program Web
        * 
        *       public class Program
        *       {
        *           public static async Task Main(string[] args)
        *           {
        *               var webHost = CreateHostBuilder(args).Build();
        *               await ApplyMigrations(webHost.Services);
        *               await webHost.RunAsync();
        *           }
        *
        *           private static async Task ApplyMigrations(IServiceProvider serviceProvider)
        *           {
        *               using var scope = serviceProvider.CreateScope();
        *               await using RepositoryDbContext dbContext = scope.ServiceProvider.GetRequiredService<RepositoryDbContext>();
        *               await dbContext.Database.MigrateAsync();
        *           }
        *
        *           public static IHostBuilder CreateHostBuilder(string[] args) =>
        *               Host.CreateDefaultBuilder(args)
        *                   .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());
        *       }
        *       
        *  La cosa grandiosa di questo approccio è che le migrazioni verranno applicate automaticamente 
        *  quando creiamo nuove migrazioni, più avanti lungo la strada. 
        *  Non dobbiamo pensarci in futuro.
        *  
        *  Possiamo quindi aprire il browser all'indirizzo , dove possiamo trovare l'interfaccia utente:
        *  https://localhost:5001/swaggerSwagger
        *  Qui possiamo testare i nostri endpoint API e verificare se tutto funziona correttamente.
        *  
        *  Conclusione
        *  
        *  In questo articolo, abbiamo imparato a conoscere l'architettura Onion. 
        *  Abbiamo spiegato la nostra visione dell'architettura suddividendola nei livelli Dominio, 
        *  Servizio, Infrastruttura e Presentazione.
        *  
        *  Abbiamo iniziato con il livello Dominio, dove abbiamo visto le definizioni per le 
        *  nostre entità e le interfacce e le eccezioni del repository.
        *  
        *  Poi abbiamo visto come è stato creato il livello Service, dove stiamo incapsulando 
        *  la nostra logica di business.
        *  
        *  Successivamente, abbiamo esaminato il livello Infrastruttura, in cui sono posizionate 
        *  le implementazioni delle interfacce del repository, nonché il contesto del database EF.
        *  
        *  Infine, abbiamo visto come il nostro livello Presentazione viene implementato come 
        *  progetto separato disaccoppiando i controller dall'applicazione Web principale. 
        *  Quindi, abbiamo spiegato come possiamo connettere tutti i livelli utilizzando 
        *  un'API Web ASP.NET Core.
        *  
        *  // Il file di progetto completo con tutti i sorgenti sono in allegato 
        *  // in formato compresso
        *  
        *   // onion-architecture-aspnetcore-main.zip
        *  
        */

    /*
     * Code 1
     *
        internal sealed class OwnerService : IOwnerService
        {
            private readonly IRepositoryManager _repositoryManager;

            public OwnerService(IRepositoryManager repositoryManager) => _repositoryManager = repositoryManager;

            public async Task<IEnumerable<OwnerDto>> GetAllAsync(CancellationToken cancellationToken = default)
            {
                var owners = await _repositoryManager.OwnerRepository.GetAllAsync(cancellationToken);

                var ownersDto = owners.Adapt<IEnumerable<OwnerDto>>();

                return ownersDto;
            }

            public async Task<OwnerDto> GetByIdAsync(Guid ownerId, CancellationToken cancellationToken = default)
            {
                var owner = await _repositoryManager.OwnerRepository.GetByIdAsync(ownerId, cancellationToken);

                if (owner is null)
                {
                    throw new OwnerNotFoundException(ownerId);
                }

                var ownerDto = owner.Adapt<OwnerDto>();

                return ownerDto;
            }

            public async Task<OwnerDto> CreateAsync(OwnerForCreationDto ownerForCreationDto, CancellationToken cancellationToken = default)
            {
                var owner = ownerForCreationDto.Adapt<Owner>();

                _repositoryManager.OwnerRepository.Insert(owner);

                await _repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);

                return owner.Adapt<OwnerDto>();
            }

            public async Task UpdateAsync(Guid ownerId, OwnerForUpdateDto ownerForUpdateDto, CancellationToken cancellationToken = default)
            {
                var owner = await _repositoryManager.OwnerRepository.GetByIdAsync(ownerId, cancellationToken);

                if (owner is null)
                {
                    throw new OwnerNotFoundException(ownerId);
                }

                owner.Name = ownerForUpdateDto.Name;
                owner.DateOfBirth = ownerForUpdateDto.DateOfBirth;
                owner.Address = ownerForUpdateDto.Address;

                await _repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);
            }

            public async Task DeleteAsync(Guid ownerId, CancellationToken cancellationToken = default)
            {
                var owner = await _repositoryManager.OwnerRepository.GetByIdAsync(ownerId, cancellationToken);

                if (owner is null)
                {
                    throw new OwnerNotFoundException(ownerId);
                }

                _repositoryManager.OwnerRepository.Remove(owner);

                await _repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);
            }
        }
    */

    /*
     * Code 2
     * 
    internal sealed class AccountService : IAccountService
    {
        private readonly IRepositoryManager _repositoryManager;

        public AccountService(IRepositoryManager repositoryManager) => _repositoryManager = repositoryManager;

        public async Task<IEnumerable<AccountDto>> GetAllByOwnerIdAsync(Guid ownerId, CancellationToken cancellationToken = default)
        {
            var accounts = await _repositoryManager.AccountRepository.GetAllByOwnerIdAsync(ownerId, cancellationToken);

            var accountsDto = accounts.Adapt<IEnumerable<AccountDto>>();

            return accountsDto;
        }

        public async Task<AccountDto> GetByIdAsync(Guid ownerId, Guid accountId, CancellationToken cancellationToken)
        {
            var owner = await _repositoryManager.OwnerRepository.GetByIdAsync(ownerId, cancellationToken);

            if (owner is null)
            {
                throw new OwnerNotFoundException(ownerId);
            }

            var account = await _repositoryManager.AccountRepository.GetByIdAsync(accountId, cancellationToken);

            if (account is null)
            {
                throw new AccountNotFoundException(accountId);
            }

            if (account.OwnerId != owner.Id)
            {
                throw new AccountDoesNotBelongToOwnerException(owner.Id, account.Id);
            }

            var accountDto = account.Adapt<AccountDto>();

            return accountDto;
        }

        public async Task<AccountDto> CreateAsync(Guid ownerId, AccountForCreationDto accountForCreationDto, CancellationToken cancellationToken = default)
        {
            var owner = await _repositoryManager.OwnerRepository.GetByIdAsync(ownerId, cancellationToken);

            if (owner is null)
            {
                throw new OwnerNotFoundException(ownerId);
            }

            var account = accountForCreationDto.Adapt<Account>();

            account.OwnerId = owner.Id;

            _repositoryManager.AccountRepository.Insert(account);

            await _repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);

            return account.Adapt<AccountDto>();
        }

        public async Task DeleteAsync(Guid ownerId, Guid accountId, CancellationToken cancellationToken = default)
        {
            var owner = await _repositoryManager.OwnerRepository.GetByIdAsync(ownerId, cancellationToken);

            if (owner is null)
            {
                throw new OwnerNotFoundException(ownerId);
            }

            var account = await _repositoryManager.AccountRepository.GetByIdAsync(accountId, cancellationToken);

            if (account is null)
            {
                throw new AccountNotFoundException(accountId);
            }

            if (account.OwnerId != owner.Id)
            {
                throw new AccountDoesNotBelongToOwnerException(owner.Id, account.Id);
            }

            _repositoryManager.AccountRepository.Remove(account);

            await _repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
    */

    /*
     * Code 3
     * 
     *  public sealed class ServiceManager : IServiceManager
     *  {
     *      private readonly Lazy<IOwnerService> _lazyOwnerService;
     *      private readonly Lazy<IAccountService> _lazyAccountService;
     *
     *      public ServiceManager(IRepositoryManager repositoryManager)
     *      {
     *          _lazyOwnerService = new Lazy<IOwnerService>(() => new OwnerService(repositoryManager));
     *          _lazyAccountService = new Lazy<IAccountService>(() => new AccountService(repositoryManager));
     *      }
     *
     *      public IOwnerService OwnerService => _lazyOwnerService.Value;
     *
     *      public IAccountService AccountService => _lazyAccountService.Value;
     *  }
     * 
    */

    /*
     * Code 4
     * 
     *  internal sealed class OwnerConfiguration : IEntityTypeConfiguration<Owner>
     *  {
     *      public void Configure(EntityTypeBuilder<Owner> builder)
     *      {
     *          builder.ToTable(nameof(Owner));
     *
     *          builder.HasKey(owner => owner.Id);
     *
     *          builder.Property(account => account.Id).ValueGeneratedOnAdd();
     *
     *          builder.Property(owner => owner.Name).HasMaxLength(60);
     *
     *          builder.Property(owner => owner.DateOfBirth).IsRequired();
     *
     *          builder.Property(owner => owner.Address).HasMaxLength(100);
     *
     *          builder.HasMany(owner => owner.Accounts)
     *              .WithOne()
     *              .HasForeignKey(account => account.OwnerId)
     *              .OnDelete(DeleteBehavior.Cascade);
     *      }
     *  } 
     *
     *  internal sealed class AccountConfiguration : IEntityTypeConfiguration<Account>
     *  {
     *      public void Configure(EntityTypeBuilder<Account> builder)
     *      {
     *          builder.ToTable(nameof(Account));
     *
     *          builder.HasKey(account => account.Id);
     *
     *          builder.Property(account => account.Id).ValueGeneratedOnAdd();
     *
     *          builder.Property(account => account.AccountType).HasMaxLength(50);
     *
     *          builder.Property(account => account.DateCreated).IsRequired();
     *      }
     *  }
     *
     */

    /*
     * Code 5
     * 
        internal sealed class OwnerRepository : IOwnerRepository
        {
            private readonly RepositoryDbContext _dbContext;

            public OwnerRepository(RepositoryDbContext dbContext) => _dbContext = dbContext;

            public async Task<IEnumerable<Owner>> GetAllAsync(CancellationToken cancellationToken = default) =>
                await _dbContext.Owners.Include(x => x.Accounts).ToListAsync(cancellationToken);

            public async Task<Owner> GetByIdAsync(Guid ownerId, CancellationToken cancellationToken = default) =>
                await _dbContext.Owners.Include(x => x.Accounts).FirstOrDefaultAsync(x => x.Id == ownerId, cancellationToken);

            public void Insert(Owner owner) => _dbContext.Owners.Add(owner);

            public void Remove(Owner owner) => _dbContext.Owners.Remove(owner);
        }

        internal sealed class AccountRepository : IAccountRepository
        {
            private readonly RepositoryDbContext _dbContext;

            public AccountRepository(RepositoryDbContext dbContext) => _dbContext = dbContext;

            public async Task<IEnumerable<Account>> GetAllByOwnerIdAsync(Guid ownerId, CancellationToken cancellationToken = default) =>
                await _dbContext.Accounts.Where(x => x.OwnerId == ownerId).ToListAsync(cancellationToken);

            public async Task<Account> GetByIdAsync(Guid accountId, CancellationToken cancellationToken = default) =>
                await _dbContext.Accounts.FirstOrDefaultAsync(x => x.Id == accountId, cancellationToken);

            public void Insert(Account account) => _dbContext.Accounts.Add(account);

            public void Remove(Account account) => _dbContext.Accounts.Remove(account);
        }

    */

    /*
     * Code 6
     * 
            [ApiController]
            [Route("api/owners")]
            public class OwnersController : ControllerBase
            {
                private readonly IServiceManager _serviceManager;

                public OwnersController(IServiceManager serviceManager) => _serviceManager = serviceManager;

                [HttpGet]
                public async Task<IActionResult> GetOwners(CancellationToken cancellationToken)
                {
                    var owners = await _serviceManager.OwnerService.GetAllAsync(cancellationToken);

                    return Ok(owners);
                }

                [HttpGet("{ownerId:guid}")]
                public async Task<IActionResult> GetOwnerById(Guid ownerId, CancellationToken cancellationToken)
                {
                    var ownerDto = await _serviceManager.OwnerService.GetByIdAsync(ownerId, cancellationToken);

                    return Ok(ownerDto);
                }

                [HttpPost]
                public async Task<IActionResult> CreateOwner([FromBody] OwnerForCreationDto ownerForCreationDto)
                {
                    var ownerDto = await _serviceManager.OwnerService.CreateAsync(ownerForCreationDto);

                    return CreatedAtAction(nameof(GetOwnerById), new { ownerId = ownerDto.Id }, ownerDto);
                }

                [HttpPut("{ownerId:guid}")]
                public async Task<IActionResult> UpdateOwner(Guid ownerId, [FromBody] OwnerForUpdateDto ownerForUpdateDto, CancellationToken cancellationToken)
                {
                    await _serviceManager.OwnerService.UpdateAsync(ownerId, ownerForUpdateDto, cancellationToken);

                    return NoContent();
                }

                [HttpDelete("{ownerId:guid}")]
                public async Task<IActionResult> DeleteOwner(Guid ownerId, CancellationToken cancellationToken)
                {
                    await _serviceManager.OwnerService.DeleteAsync(ownerId, cancellationToken);

                    return NoContent();
                }
            }

            [ApiController]
            [Route("api/owners/{ownerId:guid}/accounts")]
            public class AccountsController : ControllerBase
            {
                private readonly IServiceManager _serviceManager;

                public AccountsController(IServiceManager serviceManager) => _serviceManager = serviceManager;

                [HttpGet]
                public async Task<IActionResult> GetAccounts(Guid ownerId, CancellationToken cancellationToken)
                {
                    var accountsDto = await _serviceManager.AccountService.GetAllByOwnerIdAsync(ownerId, cancellationToken);

                    return Ok(accountsDto);
                }

                [HttpGet("{accountId:guid}")]
                public async Task<IActionResult> GetAccountById(Guid ownerId, Guid accountId, CancellationToken cancellationToken)
                {
                    var accountDto = await _serviceManager.AccountService.GetByIdAsync(ownerId, accountId, cancellationToken);

                    return Ok(accountDto);
                }

                [HttpPost]
                public async Task<IActionResult> CreateAccount(Guid ownerId, [FromBody] AccountForCreationDto accountForCreationDto, CancellationToken cancellationToken)
                {
                    var response = await _serviceManager.AccountService.CreateAsync(ownerId, accountForCreationDto, cancellationToken);

                    return CreatedAtAction(nameof(GetAccountById), new { ownerId = response.OwnerId, accountId = response.Id }, response);
                }

                [HttpDelete("{accountId:guid}")]
                public async Task<IActionResult> DeleteAccount(Guid ownerId, Guid accountId, CancellationToken cancellationToken)
                {
                    await _serviceManager.AccountService.DeleteAsync(ownerId, accountId, cancellationToken);

                    return NoContent();
                }
            }
    */

    /*
     * Code 7
     * 
            public void ConfigureServices(IServiceCollection services)
            {
                services.AddControllers()
                    .AddApplicationPart(typeof(Presentation.AssemblyReference).Assembly);

                services.AddSwaggerGen(c =>
                    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Web", Version = "v1" }));

                services.AddScoped<IServiceManager, ServiceManager>();

                services.AddScoped<IRepositoryManager, RepositoryManager>();

                services.AddDbContextPool<RepositoryDbContext>(builder =>
                {
                    var connectionString = Configuration.GetConnectionString("Database");

                    builder.UseNpgsql(connectionString);
                });

                services.AddTransient<ExceptionHandlingMiddleware>();
            }

            /// Per .Net 6 aggiungiamo un codice leggermente divereso.:

            ....
            builder.Services.AddControllers()
                .AddApplicationPart(typeof(Presentation.AssemblyReference).Assembly);
    
            builder.Services.AddSwaggerGen(c =>
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Web", Version = "v1" }));
    
            builder.Services.AddScoped<IServiceManager, ServiceManager>();
    
            builder.Services.AddScoped<IRepositoryManager, RepositoryManager>();
    
            builder.Services.AddDbContextPool<RepositoryDbContext>(builder =>
            {
                var connectionString = Configuration.GetConnectionString("Database");
                builder.UseNpgsql(connectionString);
            });
            ....

            builder.Services.AddTransient<ExceptionHandlingMiddleware>();

            // La parte più importante del codice è:

            builder.Services.AddControllers()
            .AddApplicationPart(typeof(Presentation.AssemblyReference).Assembly);

    */

    /*
     * Code 8
     * 
            internal sealed class ExceptionHandlingMiddleware : IMiddleware
            {
                private readonly ILogger<ExceptionHandlingMiddleware> _logger;

                public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger) => _logger = logger;

                public async Task InvokeAsync(HttpContext context, RequestDelegate next)
                {
                    try
                    {
                        await next(context);
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e, e.Message);

                        await HandleExceptionAsync(context, e);
                    }
                }

                private static async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
                {
                    httpContext.Response.ContentType = "application/json";

                    httpContext.Response.StatusCode = exception switch
                    {
                        BadRequestException => StatusCodes.Status400BadRequest,
                        NotFoundException => StatusCodes.Status404NotFound,
                        _ => StatusCodes.Status500InternalServerError
                    };

                    var response = new
                    {
                        error = exception.Message
                    };

                    await httpContext.Response.WriteAsync(JsonSerializer.Serialize(response));
                }
            }

        */


    class Program { 

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }

    }
}
