using System;

namespace DotNetDesignPatternDemos.Architectural.Hexagonal.ExampleDotNet
{

    /*
     * In questo articolo verrà visualizzato un modello di soluzione API Web basato su 
     * Hexagonal Architecture con tutte le funzionalità essenziali che utilizzano .NET Core.
     * 
     * Introduzione
     * 
     *  Questo è un progetto di avvio che avrà tutte le cose essenziali integrate in esso. 
     *  Quando dobbiamo iniziare con un nuovo progetto, allora dovremmo pensare alle esigenze 
     *  aziendali. Quando il requisito aziendale è pronto, allora dobbiamo iniziare a 
     *  focalizzare il business e dovremmo essere pronti con le librerie di base/essenziali.
     *  
     *  Vedremo alcune importanti tecnologie in ordine, che dovrebbero essere integrate 
     *  al nostro progetto per il kick-off.
     *  
     * L'applicazione è implementata su architettura esagonale
     *  ASP.NET Core
     *  Web API
     *  Entityframework Core
     *  Gestione delle aspettative
     *  Unit test tramite NUnit
     *  Controllo delle versioni
     *  Interfaccia utente Swagger
     *  Registrazioni — seriLog
     *  Interfaccia utente dei controlli di integrità
     *  Autenticazione JWT
     *  
     *  Strati dell'architettura esagonale
     *  
     *      In questo approccio, possiamo vedere che tutti i livelli dipendono solo dai 
     *      livelli principali.
     *      
     *      Livello API di dominio
     *      Domain Api Layers (Core layer) è implementato al centro e non dipende mai 
     *      da nessun altro layer. Si tratta di un contratto per l'interazione a livello 
     *      di dominio (porte) in modo che gli adapter primari e secondari possano 
     *      implementare il contratto. 
     *      Questo è anche noto e DIP o Dependency Inversion Principle Domain layer
     *      
     *      Livelli di dominio (livello aziendale)
     *      Questi livelli hanno una logica di business e vengono mantenuti puliti senza 
     *      altre dipendenze.
     *      
     *      Livello Adattatore di riposo
     *      Adattatore rest noto anche come adattatore della porta sinistra e adattatore 
     *      primario in cui implementiamo un servizio riposante (ad esempio, GET, POST, 
     *      PUT, DELETE, ecc.)
     *      
     *      Livello Adattatore di persistenza
     *      Rest Adapter, noto anche come adattatore della porta destra e adattatore 
     *      secondario, è dove abbiamo implementato Entity framework core che già implementa 
     *      un modello di progettazione del repository. DbContext sarà UOW (Unit of Work) 
     *      e ogni DbSet è il repository. 
     *      Questo interagisce con il nostro database utilizzando i fornitori di dati
     *  
     *  Livello Bootstrap/Presentazione
     *  
     *  Questa è la build finale del progetto, dove tutto ha inizio.
     *  Fase 1
     *      Scaricare e installare Visual Studio Extension dal modello di progetto
     *      Scaricare e installare l'estensione di Visual Studio da Microsoft Marketplace.
     *      // Figura 1 : Hexagonal Architecture In ASP.NET Core1.jpg
     *      
     *  Fase 2 - Crea progetto
     *      Selezionare il tipo di progetto come WebAPI e selezionare Architettura esagonale.
     *      // Figura 2 : Hexagonal Architecture In ASP.NET Core2.jpg
     *      
     *  Fase 3 - Selezionare il modello di progetto di architettura esagonale
     *      Selezionare il tipo di progetto come API Web e selezionare Architettura esagonale.
     *      // Figura 3 : Hexagonal Architecture In ASP.NET Core3.jpg
     *      
     *  Fase 4 Schema del progetto
     *      // Figura 4 : Hexagonal Architecture In ASP.NET Core4.png
     *      
     *  Fase 5
     *      Compilare ed eseguire l'applicazione
     *      Interfaccia utente di controllo dello stato
     *      Passare a Controlli integrità https://localhost:44377/healthcheck-ui e assicurarsi 
     *      che tutto sia verde.
     *      ** Cambia il numero di porta in base alla tua applicazione
     *      // Figura 5 : Hexagonal Architecture In ASP.NET Core5.jpg    
     *  
     *  Interfaccia utente Swagger
     *  
     *      https://localhost:44377/OpenAPI/index.html dell'interfaccia utente Swagger
     *      ** Cambia il numero di porta in base alla tua applicazione
     *      // Figura 6 : Hexagonal Architecture In ASP.NET Core6.jpg
     *      
     *  Domain Api layer code snippets
     *
     *   Per prima cosa vedremo il livello di dominio, qui creeremo entità in base al requisito. 
     *   In questo esempio, abbiamo creato Deal Model..
     *   public class BaseEntity  
     *   {  
     *       [Key]  
     *       public int Id { get; set; }  
     *   }  
     *
     *   public class Deal : BaseEntity  
     *   {  
     *         public string Name { get; set; }  
     *         public string Description { get; set; }  
     *   }  
     *   
     *   Domain layer code snippets
     *
     *   In questo livello, abbiamo creato DealDomain che deriva dall'interfaccia IRequestDeal..
     *   public class DealDomain : IRequestDeal where T : class  
     *   {  
     *         private readonly DbSet table;  
     *
     *         public DealDomain(ApplicationDbContext dbContext)  
     *         {  
     *               ApplicationDbContext _dbContext;  
     *               _dbContext = dbContext;  
     *               table = _dbContext.Set();  
     *         }  
     *         public T GetDeal(int id)  
     *         {  
     *               return table.Find(id);  
     *         }  
     *
     *         public List GetDeals()  
     *         {  
     *               return table.ToList();  
     *         }  
     *   }   
     *   Persistence layer code snippets
     *
     *   Nel livello di Persistenza , implementeremo EntityFrameworkCore creando ApplicationDbContext.
     *   public class ApplicationDbContext : DbContext  
     *   {  
     *         public ApplicationDbContext()  
     *         {  
     *         }  
     *         public ApplicationDbContext(DbContextOptions options) : base(options)  
     *         {  
     *         }  
     *         public DbSet Deals { get; set; }  
     *         public override Task SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())  
     *         {  
     *               return base.SaveChangesAsync(cancellationToken);  
     *         }  
     *   }   
     *   Rest adapter code snippets
     *
     *   Nell'adattatore Rest, implementiamo Api creando Controller utilizzando DealController.
     *   [ApiController]  
     *   [Route("api/v{version:apiVersion}/[controller]")]  
     *   public class DealController : ControllerBase  
     *   {  
     *         private readonly IRequestDeal _requestDeal;  
     *
     *         public DealController(IRequestDeal requestDeal)  
     *         {  
     *               _requestDeal = requestDeal;  
     *         }  
     *
     *         // GET: api/deal  
     *         [HttpGet]  
     *         public IActionResult Get()  
     *         {  
     *               var result = _requestDeal.GetDeals();  
     *               return Ok(result);  
     *         }  
     *
     *         // GET: api/deal/1  
     *         [HttpGet]  
     *         [Route("{id}", Name = "GetDeal")]  
     *         public IActionResult Get(int id)  
     *         {  
     *               var result = _requestDeal.GetDeal(id);  
     *               return Ok(result);  
     *         }  
     *       }  
     *
     * Quale problema risolve questa soluzione?
     * 
     *  Una soluzione di app include tutte le librerie essenziali con le best practice, 
     *  che aiuteranno ad avviare rapidamente il progetto. Lo sviluppatore può concentrarsi 
     *  sui requisiti aziendali e creare entità. 
     *  Questo aiuta a risparmiare molto tempo di sviluppo.
     *  
     *  Di seguito sono riportate alcune librerie essenziali che sono già incluse 
     *  in un progetto con best practice basate sull'architettura esagonale
     *  
     *  Entityframework Core
     *  Gestione delle aspettative
     *  Unit test tramite NUnit
     *  Controllo delle versioni
     *  Interfaccia utente Swagger
     *  Registrazioni — seriLog
     *  Interfaccia utente dei controlli di integrità
     *  Autenticazione JWT
     *  
     *  In che modo questo aiuta qualcun altro?
     *  
     *      Questa soluzione aiuta gli sviluppatori che possono risparmiare tempo di sviluppo 
     *      e concentrarsi sui moduli aziendali. E se lui / lei ha meno esperienza, aiuta a 
     *      mantenere le migliori pratiche nel progetto (come il codice pulito)
     *      
     *      Come funziona effettivamente il codice?
     *      
     *      Questo è un modello di progetto ospitato in marketplace.visualstudio.com. 
     *      Scarica questa estensione dal marketplace e installala in Visual Studio. 
     *      Durante la creazione del progetto selezionare questo modello.
     *      
     *  Conclusione
     *  
     *  In questo esempio, abbiamo visto cos'è l'architettura esagonale. 
     *  Il modello di progetto ci aiuterà ad avviare rapidamente l'applicazione.
     *  
     *  In allegato è presente il file compresso dell'esempio in questione.
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
