using System;

namespace DotNetDesignPatternDemos.Architectural.Clean.Example1
{
    /*
     * In questo esempio, tratteremo praticamente l'architettura pulita sotto ambiente .net. 
     * 
     * L'architettura pulita è un'architettura software che ci aiuta a tenere sotto controllo 
     * un intero codice applicativo. L'obiettivo principale dell'architettura pulita è il 
     * codice/logica, che è improbabile che cambi. Deve essere scritto senza alcuna dipendenza 
     * diretta. Ciò significa che se voglio cambiare il mio framework di sviluppo OR User 
     * Interface (UI) del sistema, il nucleo del sistema non deve essere modificato. 
     * 
     * Significa anche che le nostre dipendenze esterne sono completamente sostituibili.
     * 
     * L'architettura pulita ha un livello di dominio, un livello di applicazione, un livello di 
     * infrastruttura e un livello di framework. Il dominio e il livello dell'applicazione sono 
     * sempre il centro del progetto e sono noti come il nucleo del sistema. 
     * Il nucleo sarà indipendente dall'accesso ai dati e dai problemi infrastrutturali. 
     * Possiamo raggiungere questo obiettivo utilizzando le interfacce e l'astrazione all'interno 
     * del sistema principale, ma implementandole al di fuori del sistema principale.
     * 
     *      // Figura 1 : Clean Architecture in Aspnet Core Web API.jpg
     *      
     * Layer in un'architettura pulita:
     * 
     *  L'architettura pulita ha un livello di dominio, un livello di applicazione, 
     *  un livello di infrastruttura e un livello di presentazione. 
     *  Il dominio e il livello dell'applicazione sono sempre il centro del progetto e 
     *  sono noti come il nucleo del sistema.
     *  
     *  Nell'architettura Clean, tutte le dipendenze dell'applicazione sono 
     *  Independent/Inwards e il sistema Core non ha dipendenze da nessun altro livello del 
     *  sistema. Quindi, in futuro, se vogliamo cambiare il framework UI/OR del sistema, 
     *  possiamo farlo facilmente perché tutte le altre dipendenze del sistema non dipendono 
     *  dal nucleo del sistema.
     *      
     *      Livello di dominio
     *      Il livello di dominio nell'architettura pulita contiene la logica aziendale, 
     *      come le entità e le relative specifiche. 
     *      Questo livello si trova al centro dell'architettura in cui sono presenti 
     *      entità applicative, che sono le classi del modello di applicazione o le 
     *      classi del modello di database, utilizzando l'approccio code first nello 
     *      sviluppo dell'applicazione utilizzando Asp.net core queste entità vengono 
     *      utilizzate per creare le tabelle nel database.
     *      
     *      Livello di applicazione
     *      Il livello dell'applicazione contiene la logica di business. 
     *      Tutta la logica di business verrà scritta in questo livello. È in questo livello 
     *      che le interfacce dei servizi sono mantenute, separate dalla loro implementazione, 
     *      per l'accoppiamento allentato e la separazione delle preoccupazioni.
     *      
     *      Livello di infrastruttura
     *      Nel livello di infrastruttura, abbiamo oggetti modello manterremo tutte le migrazioni 
     *      di database e il contesto di database Oggetti in questo livello. 
     *      In questo livello, abbiamo i repository di tutti gli oggetti del modello di dominio.
     *      
     *      Livello di presentazione
     *      Nel caso del livello di presentazione API che ci presenta i dati dell'oggetto dal 
     *      database utilizzando la richiesta HTTP sotto forma di oggetto JSON. 
     *      Ma nel caso di applicazioni front-end, presentiamo i dati usando l'interfaccia utente 
     *      consumando l'APIS.
     *      
     * Vantaggi di Clean Architecture
     * 
     * 
     *   L'implementazione immediata è possibile implementare questa architettura con qualsiasi 
     *   linguaggio di programmazione.
     *   
     *   Il dominio e il livello dell'applicazione sono sempre il centro del progetto e sono noti 
     *   come il nucleo del sistema, motivo per cui il nucleo del sistema non dipende da sistemi 
     *   esterni.
     *   
     *   Questa architettura consente di modificare il sistema esterno senza influire sul nucleo 
     *   del sistema.
     *   
     *   In un ambiente altamente testabile, è possibile testare il codice in modo rapido e 
     *   semplice.
     *   
     *   È possibile creare un prodotto altamente scalabile e di qualità.
     *   
     * Implementazione Esempio in .Net
     * 
     * Innanzitutto, è necessario creare il progetto API di base Asp.net utilizzando Visual Studio. 
     * Dopodiché aggiungeremo il livello nella nostra soluzione, quindi dopo aver aggiunto tutti i 
     * livelli nel sistema la nostra struttura del progetto sarà così.
     * 
     *      // Figura 2 :   Clean Architecture in Aspnet Core Web API02.jpg 
     *      
     * Ora puoi vedere che la nostra struttura del progetto sarà come nella foto sopra.
     * 
     * Implementiamo il livello praticamente.
     * 
     * Livello di dominio
     *      Il livello di dominio nell'architettura pulita contiene la logica aziendale. 
     *      Come le entità e le relative specifiche, questo livello si trova al centro 
     *      dell'architettura in cui sono presenti le entità dell'applicazione, che sono 
     *      le classi del modello di applicazione o le classi del modello di database. 
     *      Utilizzando l'approccio code first nello sviluppo dell'applicazione, 
     *      utilizzando Asp.net core queste entità creeranno le tabelle nel database.
     *      
     * Implementazione del livello di dominio
     *      Innanzitutto, è necessario aggiungere il progetto di libreria al sistema, 
     *      quindi aggiungiamo il progetto di libreria al sistema.
     *      
     *      Scrivi fare clic sulla soluzione e quindi fare clic sull'opzione Aggiungi.
     *      
     *          // Figura 3 : Clean Architecture in Aspnet Core Web API03.jpg
     *          
     *      Denominare questo progetto Domain Layer.
     *      
     * Cartella Entità
     *      Innanzitutto, è necessario aggiungere la cartella Models che verrà utilizzata 
     *      per creare le entità del database. Nella cartella Models verranno create le 
     *      seguenti entità di database.
     *      
     *          // Figura 4 :   Clean Architecture in Aspnet Core Web API05.jpg
     *  
     * Cartella dell'interfaccia
     *      Questa cartella viene utilizzata per aggiungere le interfacce delle entità a cui 
     *      si desidera aggiungere i metodi specifici nell'interfaccia.
     *      
     *          // Figura 5 :   Clean Architecture in Aspnet Core Web API06.jpg
     * 
     * Cartella delle specifiche
     * 
     *      Questa cartella viene utilizzata per aggiungere tutte le specifiche. Prendiamo 
     *      l'esempio se si desidera che il risultato dell'API sia crescente OR, in ordine 
     *      decrescente, OPPURE si desideri il risultato nei criteri specifici, OPPURE si 
     *      desidera il risultato sotto forma di impaginazione, è necessario aggiungere la 
     *      classe di specifiche. 
     *      
     *          // Figura 6 :   Clean Architecture in Aspnet Core Web API07.jpg
     *          
     *          // Esempio di codice della classe Specification :   Code1
     * 
     * Livello di applicazione
     * 
     *      Il livello dell'applicazione contiene la logica di business. Tutta la logica di 
     *      business verrà scritta in questo livello. In questo livello, le interfacce dei 
     *      servizi sono tenute separate dalla loro implementazione per l'accoppiamento 
     *      libero e la separazione delle preoccupazioni.
     *      
     *      Ora aggiungeremo il livello dell'applicazione alla nostra applicazione.
     *      
     *      Ora lavoreremo sul livello dell'applicazione. Seguiremo lo stesso processo che 
     *      abbiamo fatto per il livello Dominio. Aggiungere il progetto di libreria nel 
     *      richiedente e assegnare un nome al livello repository del progetto.
     *      
     *      Ma qui, dobbiamo aggiungere il riferimento al progetto del livello Dominio nel 
     *      livello dell'applicazione. Fare clic con il pulsante destro del mouse sul progetto, 
     *      quindi fare clic sul pulsante Aggiungi dopodiché aggiungeremo i riferimenti al 
     *      progetto nel livello dell'applicazione.
     *      
     *      // Figura 6 :   Clean Architecture in Aspnet Core Web API08.jpg
     *      
     *      Ora selezionare il progetto principale per aggiungere il riferimento al progetto 
     *      del livello di dominio.
     *      
     *      // Figura 7 :   Clean Architecture in Aspnet Core Web API09.jpg
     *      
     *      Fare clic sul pulsante OK. Successivamente, il nostro riferimento al progetto 
     *      verrà aggiunto al nostro sistema.
     *      
     *      Ora creiamo le cartelle desiderate nei nostri progetti.
     *      
     * Servizi ICustom
     * 
     *      Aggiungiamo la cartella dei servizi ICustom nella nostra applicazione in questa 
     *      cartella. Aggiungeremo il Servizio ICustom Interfacciato che verrà Ereditato da 
     *      tutti i servizi che aggiungeremo nella nostra cartella Servizio Clienti.
     *      
     *      Aggiungiamo alcuni servizi al nostro progetto.
     *      Codice del servizio personalizzato
     *      Aggiungiamo il servizio token che eredita l'interfaccia del servizio token dal Core
     *      
     *      // Figura 8 : Clean Architecture in Aspnet Core Web API10.jpg
     * 
     *      // Codice dell'interfaccia ICustom  :   Code2
     *      
     * Cartella Servizi personalizzati
     * 
     *      Questa cartella verrà utilizzata per aggiungere i servizi personalizzati al 
     *      nostro sistema. Ci consente di creare alcuni servizi personalizzati per il 
     *      nostro progetto, in modo che il nostro concetto sia chiaro sui servizi personalizzati. 
     *      Tutti i servizi personalizzati erediteranno l'interfaccia dei servizi ICustom.
     *      Utilizzando tale interfaccia, aggiungeremo l'operazione CRUD nel nostro sistema.
     *      
     * Livello di infrastruttura
     * 
     *      Nel livello di infrastruttura, abbiamo oggetti modello manterremo tutte le migrazioni 
     *      di database e il contesto di database Oggetti in questo livello. In questo livello, 
     *      abbiamo i repository di tutti gli oggetti del modello di dominio.
     *      
     *      Ora aggiungeremo il livello di infrastruttura alla nostra applicazione.
     *      
     *      Ora lavoreremo sul layer dell'infrastruttura. Seguiremo lo stesso processo che 
     *      abbiamo fatto per il livello Dominio. Aggiungere il progetto di libreria nel 
     *      richiedente e assegnare un nome a tale livello di infrastruttura del progetto.
     *      
     *      Ma qui, dobbiamo aggiungere il riferimento al progetto del livello Dominio nel 
     *      livello dell'infrastruttura. Scrivi fare clic sul progetto e quindi fare clic 
     *      sul pulsante Aggiungi. Successivamente aggiungeremo i riferimenti al progetto 
     *      nel livello dell'infrastruttura.
     *      
     *          // Figura 11 :  Clean Architecture in Aspnet Core Web API11.jpg
     *      
     *      Ora seleziona il Core per aggiungere il riferimento al livello di 
     *      dominio nel nostro progetto.
     *      
     *          // Figura 12 :  Clean Architecture in Aspnet Core Web API12.jpg
     *          
     * Implementazione di Infrastructure Layer
     * 
     *      Implementiamo il livello di infrastruttura nel nostro sistema.
     *      Innanzitutto, è necessario aggiungere la cartella Dati al livello dell'infrastruttura.
     * 
     * Cartella dati
     * 
     *      Aggiungere la cartella Data nel livello di infrastruttura utilizzato per aggiungere 
     *      la classe di contesto del database. La classe di contesto del database viene 
     *      utilizzata per gestire la sessione con il database sottostante, che consente di 
     *      eseguire l'operazione CRUD.
     *      
     *      Nel nostro progetto, aggiungeremo la classe di contesto dell'archivio che gestirà 
     *      la sessione con il nostro database.
     *      
     *          // Code Database Context : Code3
     * 
     * Cartella Repository
     * 
     *      La cartella dei repository viene utilizzata per aggiungere i repository delle classi 
     *      di dominio, perché implementeremo il modello di repository nella nostra soluzione.
     *      
     *      Aggiungiamo alcuni repository alla nostra soluzione, in modo che il nostro concetto 
     *      sui repository sia chiaro.
     *      
     * Repository del carrello
     * 
     *      Creiamo il cestino come esempio per chiarire il concetto di repository. 
     *      Ciò erediterà l'interfaccia dell'interfaccia IBasket dal livello principale.
     *      
     *          // Code Repositories : Code 4
     *
     * Migrazioni
     * 
     *      Dopo aver aggiunto le proprietà DbSet, è necessario aggiungere la migrazione 
     *      utilizzando la console di gestione pacchetti ed eseguire il comando Add-Migration.
     *      add-migration ClearnArchitectureV1
     *      
     *          // Figura 13 : Clean Architecture in Aspnet Core Web API13.jpg
     *          
     *      Dopo aver eseguito il comando Add-Migration, nella cartella di migrazione sono presenti 
     *      le seguenti classi generate automaticamente:
     *      
     *          // Figura 14 : Clean Architecture in Aspnet Core Web API14.jpg
     *          
     *      Dopo aver eseguito i comandi, sarà necessario aggiornare il database eseguendo 
     *      il comando update-database
     *      
     * Livello di presentazione
     * 
     *      Il livello di presentazione è il livello finale, che presenta i dati all'utente 
     *      front-end su ogni richiesta HTTP.
     *      
     *      Nel caso del livello di presentazione API, ci presenta i dati dell'oggetto dal 
     *      database utilizzando la richiesta HTTP sotto forma di oggetto JSON. 
     *      Ma nel caso di applicazioni front-end, presentiamo i dati usando l'interfaccia 
     *      utente consumando l'APIS.
     *      
     * Cartella Estensioni
     * 
     *      Una cartella Estensioni viene utilizzata per i metodi/le classi di estensione. 
     *      Estendiamo le funzionalità. Il metodo di estensione C# è un metodo statico di 
     *      una classe statica, in cui il modificatore "this" viene applicato al primo parametro. 
     *      Il tipo del primo parametro sarà il tipo esteso. I metodi di estensione rientrano 
     *      nell'ambito solo quando si importa esplicitamente lo spazio dei nomi nel codice 
     *      sorgente con una direttiva using.
     *      
     *      Creiamo la classe statica ApplicationServicesExtensions, in cui creeremo il metodo 
     *      di estensione per la registrazione di tutti i servizi creati durante l'intero progetto.
     *      
     *      Ora dobbiamo aggiungere la dipendenza Injection di tutti i nostri servizi nelle classi 
     *      di estensioni. Quindi, useremo queste classi e metodi di estensioni nella nostra 
     *      classe di avvio.
     *      
     *      // Codice Estensione : Code 5
     *
     * Modificare la classe Startup.cs
     * 
     *      In Startup.cs Class useremo il nostro metodo di estensioni che abbiamo creato nelle 
     *      cartelle delle estensioni.
     *      
     *      // Codice Startup : Code 6
     *      
     * Helpers
     * 
     *      La cartella degli aiutanti è dove creiamo i profili di mappatura automatica, 
     *      utilizzati per mappare le entità tra loro.
     *      
     *      // Codice Helper : Code 7
     * 
     * Cartella degli oggetti di trasferimento dati (DTO)
     * 
     *      DTO è l'acronimo di data transfer object. Come suggerisce il nome, un DTO è 
     *      un oggetto creato per trasferire dati. Possiamo creare la classe di oggetti 
     *      di trasferimento dati per il mapping dei dati della richiesta in ingresso.
     *      
     * Codice dei DTO
     * 
     *      Cerchiamo di creare i DTO Basket per chiarire il concetto:
     * 
     *      // Codice DataAnnotations DTO : Code 8
     *
     * Controller
     * 
     *      I controller vengono utilizzati per gestire la richiesta HTTP. 
     *      Ora dobbiamo aggiungere il controller dello studente che interagirà con il 
     *      nostro livello di servizio e visualizzerà i dati agli utenti.
     *      
     *      In questo esempio, ci concentreremo sui controller Basket perché in tutto 
     *      l'articolo abbiamo parlato di Basket.
     *      
     *      // Codice Controller Basket: Code 9
     *      
     * Prodotto
     * 
     *      Ora eseguiremo il progetto e vedremo l'output usando Swagger.
     *      
     *      // Figura  19 : Clean Architecture in Aspnet Core Web API19.jpg
     *      
     * Conclusione
     * 
     *      In questo articolo è stata implementata l'architettura pulita utilizzando 
     *      l'approccio Entity Framework e Code First. Ora abbiamo la conoscenza di 
     *      come il livello comunica tra loro in architettura pulita e di come possiamo 
     *      scrivere il codice generico per il repository e i servizi di interfaccia. 
     *      Ora possiamo sviluppare il nostro progetto utilizzando un'architettura 
     *      pulita per progetti di sviluppo API O MVC Core Based.
     *      
     * In allegato è presente l'intero Sorgente in formato compresso.
     * 
     *      // File : Skinet2022-master.zip
     */

    /*
     * Code 1

        using System;
        using System.Collections.Generic;
        using System.Linq.Expressions;
        using System.Text;
        namespace Skinet.Core.Specifications {
            public interface ISpecifications < T > {
                Expression < Func < T,
                bool >> Criteria {
                    get;
                }
                List < Expression < Func < T,
                object >>> Includes {
                    get;
                }
                Expression < Func < T,
                object >> OrderBy {
                    get;
                }
                Expression < Func < T,
                object >> OrderByDescending {
                    get;
                }
                int Take {
                    get;
                }
                int Skip {
                    get;
                }
                bool isPagingEnabled {
                    get;
                }
            }
        }

        // Classe specifica di base
        using System;
        using System.Collections.Generic;
        using System.Linq.Expressions;
        using System.Text;
        namespace Skinet.Core.Specifications {
            public class BaseSpecification < T > : ISpecifications < T > {
                public Expression < Func < T,
                bool >> Criteria {
                    get;
                }
                public BaseSpecification() {}
                public BaseSpecification(Expression < Func < T, bool >> Criteria) {
                    this.Criteria = Criteria;
                }
                public List < Expression < Func < T,
                object >>> Includes {
                    get;
                } = new List < Expression < Func < T,
                object >>> ();
                public Expression < Func < T,
                object >> OrderBy {
                    get;
                    private set;
                }
                public Expression < Func < T,
                object >> OrderByDescending {
                    get;
                    private set;
                }
                public int Take {
                    get;
                    private set;
                }
                public int Skip {
                    get;
                    private set;
                }
                public bool isPagingEnabled {
                    get;
                    private set;
                }
                protected void AddInclude(Expression < Func < T, object >> includeExpression) {
                    Includes.Add(includeExpression);
                }
                public void AddOrderBy(Expression < Func < T, object >> OrderByexpression) {
                    OrderBy = OrderByexpression;
                }
                public void AddOrderByDecending(Expression < Func < T, object >> OrderByDecending) {
                    OrderByDescending = OrderByDecending;
                }
                public void ApplyPagging(int take, int skip) {
                    Take = take;
                    //Skip = skip;
                    isPagingEnabled = true;
                }
            }
        }     
     
     */

    /*
     * Code 2
     * 
     *      // ICustom
            using System;
            using System.Collections.Generic;
            using System.Text;
            using System.Threading.Tasks;
            namespace Skynet.Application.ICustomServices {
                public interface ICustomService < T > {
                    IEnumerable < T > GetAll();
                    void FindById(int Id);
                    void Insert(T entity);
                    Task < T > Update(T entity);
                    void Delete(T entity);
                }
            }

            // ICustomBasket
            using Skinet.Core.Entities;
            using System;
            using System.Collections.Generic;
            using System.Text;
            using System.Threading.Tasks;
            namespace Skinet.Core.Interfaces {
                public interface ICustomerBasket {
                    Task < CustomerBasket > GetBasketAsync(string basketId);
                    Task < CustomerBasket > UpdateBasketAsync(CustomerBasket basket);
                    Task < bool > DeleteBasketAsync(string basketId);
                }
            }

    */

    /*
     * Code 3
     * 
        // Code Database Context
        using Microsoft.EntityFrameworkCore;
        using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
        using Skinet.Core.Entities;
        using Skinet.Core.Entities.OrderAggregate;
        using System;
        using System.Linq;
        using System.Reflection;
        namespace Skinet.Infrastracture.Data {
            public class StoreContext: DbContext {
                public StoreContext(DbContextOptions < StoreContext > options): base(options) {}
                public DbSet < Products > Products {
                    get;
                    set;
                }
                public DbSet < ProductType > ProductTypes {
                    get;
                    set;
                }
                public DbSet < ProductBrand > ProductBrands {
                    get;
                    set;
                }
                public DbSet < Order > Orders {
                    get;
                    set;
                }
                public DbSet < DeliveryMethod > DeliveryMethods {
                    get;
                    set;
                }
                protected override void OnModelCreating(ModelBuilder modelBuilder) {
                    base.OnModelCreating(modelBuilder);
                    modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
                    if (Database.ProviderName == "Microsoft.EntityFramework.Sqlite") {
                        foreach(var entity in modelBuilder.Model.GetEntityTypes()) {
                            var properties = entity.ClrType.GetProperties().Where(p => p.PropertyType == typeof(decimal));
                            var dateandtimepropertise = entity.ClrType.GetProperties().Where(t => t.PropertyType == typeof(DateTimeOffset));
                            foreach(var property in properties) {
                                modelBuilder.Entity(entity.Name).Property(property.Name).HasConversion < double > ();
                            }
                            foreach(var property in dateandtimepropertise) {
                                modelBuilder.Entity(entity.Name).Property(property.Name).HasConversion(new DateTimeOffsetToBinaryConverter());
                            }
                        }
                    }
                }
            }
        }
     */

    /*
     * Code 4
        
        // Code Repositories
        using Skinet.Core.Entities;
        using Skinet.Core.Interfaces;
        using StackExchange.Redis;
        using System;
        using System.Collections.Generic;
        using System.Text;
        using System.Text.Json;
        using System.Threading.Tasks;
        namespace Skinet.Infrastracture.Repositories {
            public class BasketRepository: ICustomerBasket {
                private readonly IDatabase _database;
                public BasketRepository(IConnectionMultiplexer radis) {
                    _database = radis.GetDatabase();
                }
                public async Task < bool > DeleteBasketAsync(string basketId) {
                    return await _database.KeyDeleteAsync(basketId);
                }
                public async Task < CustomerBasket > GetBasketAsync(string basketId) {
                    var data = await _database.StringGetAsync(basketId);
                    return data.IsNullOrEmpty ? null : JsonSerializer.Deserialize < CustomerBasket > (data);
                }
                public async Task < CustomerBasket > UpdateBasketAsync(CustomerBasket basket) {
                    var created = await _database.StringSetAsync(basket.Id, JsonSerializer.Serialize(basket), TimeSpan.FromDays(15));
                    if (!created) {
                        return null;
                    }
                    return await GetBasketAsync(basket.Id);
                }
            }
        }
    */

    /*
     * Code 5
     * 
            using Microsoft.AspNetCore.Mvc;
            using Microsoft.Extensions.DependencyInjection;
            using Skinet.Application.CustomServices;
            using Skinet.Core.Interfaces;
            using Skinet.Errors;
            using Skinet.Infrastracture.Data;
            using Skinet.Infrastracture.Repositories;
            using Skinet.Infrastracture.SeedData;
            using System.Linq;
            namespace Skinet.Controllers.Extensions {
                public static class ApplicationServicesExtensions {
                    public static IServiceCollection AddApplicationServices(this IServiceCollection services) {
                        services.AddScoped < ITokenService, TokenService > ();
                        services.AddScoped < StoreContext, StoreContext > ();
                        services.AddScoped < StoreContextSeed, StoreContextSeed > ();
                        services.AddScoped < IProductRepository, ProductRepository > ();
                        services.AddScoped < ICustomerBasket, BasketRepository > ();
                        services.AddScoped < IUnitOfWork, UnitOfWork > ();
                        services.AddScoped < IOrderService, OrderService > ();
                        services.AddScoped(typeof(IGenericRepository < > ), typeof(GenericRepository < > ));
                        services.Configure < ApiBehaviorOptions > (options => options.InvalidModelStateResponseFactory = ActionContext => {
                            var error = ActionContext.ModelState.Where(e => e.Value.Errors.Count > 0).SelectMany(e => e.Value.Errors).Select(e => e.ErrorMessage).ToArray();
                            var errorresponce = new APIValidationErrorResponce {
                                Errors = error
                            };
                            return new BadRequestObjectResult(error);
                        });
                        return services;
                    }
                }
            }      
    */

    /*
     * Code 6
     * 
            using Microsoft.AspNetCore.Builder;
            using Microsoft.AspNetCore.Hosting;
            using Microsoft.Extensions.Configuration;
            using Microsoft.Extensions.DependencyInjection;
            using Microsoft.Extensions.Hosting;
            using Skinet.Core.Entities;
            using Skinet.Core.Interfaces;
            using Skinet.Infrastracture.Data;
            using Skinet.Infrastracture.Repositories;
            using Microsoft.EntityFrameworkCore;
            using Skinet.Infrastracture.SeedData;
            using Skinet.Helpers;
            using Skinet.ExceptionMiddleWare;
            using Microsoft.AspNetCore.Mvc;
            using System.Linq;
            using Skinet.Errors;
            using Microsoft.OpenApi.Models;
            using Skinet.Controllers.Extensions;
            using StackExchange.Redis;
            using Skinet.Infrastracture.identity;
            using Skinet.Extensions;
            namespace Skinet {
                public class Startup {
                    public IConfiguration Configuration {
                        get;
                    }
                    public Startup(IConfiguration configuration) {
                        Configuration = configuration;
                    }
                    // This method gets called by the runtime. Use this method to add services to the container.
                    public void ConfigureServices(IServiceCollection services) {
                        services.AddDbContext < StoreContext > (options => options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));
                        services.AddDbContext < IdentityContext > (options => options.UseSqlite(Configuration.GetConnectionString("DefaultIdentityConnection")));
                        services.AddSingleton < IConnectionMultiplexer > (c => {
                            var configuration = ConfigurationOptions.
                            Parse(Configuration.GetConnectionString("Radis"), true);
                            return ConnectionMultiplexer.Connect(configuration);
                        });
                        services.AddAutoMapper(typeof(MappingProfiles));
                        services.AddControllers();
                        services.AddApplicationServices();
                        services.AddSwaggerDocumentation();
                        services.AddIdentityService(Configuration);
                        services.AddCors(options => {
                            options.AddPolicy("CorsPolicy", policy => {
                                policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("*");
                            });
                        });
                    }
                    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
                    public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
                        app.UseMiddleware < ExceptionMiddle > ();
                        app.UseStatusCodePagesWithReExecute("/error/{0}");
                        app.UseCors("CorsPolicy");
                        app.UseHttpsRedirection();
                        app.UseSwaggerGen();
                        app.UseRouting();
                        app.UseStaticFiles();
                        app.UseAuthentication();
                        app.UseAuthorization();
                        app.UseEndpoints(endpoints => {
                            endpoints.MapControllers();
                        });
                    }
                }
            }
     */

    /*
     *  Code 7
     *  
            using AutoMapper;
            using Skinet.Core.Entities;
            using Skinet.Core.Entities.OrderAggregate;
            using Skinet.Dtos;
            namespace Skinet.Helpers {
                public class MappingProfiles: Profile {
                    public MappingProfiles() {
                        CreateMap < Products, ProductDto > ().
                        ForMember(d => d.ProductBrand, o => o.MapFrom(s => s.ProductBrand.Name)).ForMember(p => p.ProductType, pt => pt.MapFrom(p => p.ProductType.Name)).ForMember(p => p.PictureUrl, pt => pt.MapFrom < ProductUrlResolvers > ());
                        CreateMap < Core.Entities.Identity.Address, AddressDto > ();
                        CreateMap < CustomerBasket, CustomerbasketDto > ();
                        CreateMap < BasketItem, BasketItemDto > ();
                        CreateMap < AddressDto, Core.Entities.OrderAggregate.Address > ();
                    }
                }
            }
     */

    /*
     * Code 8
     * 
            using System.ComponentModel.DataAnnotations;
            namespace Skinet.Dtos {
                public class BasketItemDto {
                    public int Id {
                        get;
                        set;
                    }
                    public string Name {
                        get;
                        set;
                    }
                    public decimal Price {
                        get;
                        set;
                    }
                    public int Quantity {
                        get;
                        set;
                    }
                    public string PictureUrl {
                        get;
                        set;
                    }
                    public string Brand {
                        get;
                        set;
                    }
                    public string Type {
                        get;
                        set;
                    }
                }
            }    
    */


    /*
     * Code 9
     * 
            using AutoMapper;
            using Microsoft.AspNetCore.Http;
            using Microsoft.AspNetCore.Mvc;
            using Skinet.Core.Entities;
            using Skinet.Core.Interfaces;
            using Skinet.Dtos;
            using System.Threading.Tasks;
            namespace Skinet.Controllers {
                public class BasketController: BaseApiController {
                    private readonly ICustomerBasket _customerBasket;
                    private readonly IMapper _mapper;
                    public BasketController(ICustomerBasket customerBasket, IMapper mapper) {
                            _customerBasket = customerBasket;
                            _mapper = mapper;
                        }
                        [HttpGet(nameof(GetBasketElement))]
                    public async Task < ActionResult < CustomerBasket >> GetBasketElement([FromQuery] string Id) {
                            var basketelements = await _customerBasket.GetBasketAsync(Id);
                            return Ok(basketelements ?? new CustomerBasket(Id));
                        }
                        [HttpPost(nameof(UpdateProduct))]
                    public async Task < ActionResult < CustomerBasket >> UpdateProduct(CustomerBasket product) {
                            //var customerbasket = _mapper.Map<CustomerbasketDto, CustomerBasket>(product);
                            var data = await _customerBasket.UpdateBasketAsync(product);
                            return Ok(data);
                        }
                        [HttpDelete(nameof(DeleteProduct))]
                    public async Task DeleteProduct(string Id) {
                        await _customerBasket.DeleteBasketAsync(Id);
                    }
                }
            }
    */


    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
