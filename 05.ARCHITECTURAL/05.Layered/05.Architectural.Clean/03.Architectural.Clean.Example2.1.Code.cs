using System;

namespace DotNetDesignPatternDemos.Architectural.Clean.Example2
{
    /*
     * Si tratta di un modello di soluzione per la creazione di un'app a pagina singola (SPA) 
     * con Angular e ASP.NET Core seguendo i principi della Clean Architecture. 
     * 
     * Questo progetto template è un modello completo di Clean Architecture disponibile
     * come progetto open source sul sito
     *      https://github.com/jasontaylordev/CleanArchitecture
     *      
     * Scaricabile tramite NuGet e liberamente distribuibile.
     * 
     * Tecnologie
     * 
     *      ASP.NET Core 6
     *      Entity Framework Core 6
     *      Angular 13
     *      MediatR
     *      AutoMapper
     *      FluentValidation
     *      NUnit, FluentAssertions, Moq & Respawn
     * 
     * Introduzione
     * 
     *  Il modo più semplice per iniziare è installare il pacchetto NuGet ed eseguire:
     *  dotnet new ca-sln
     *  
     *  Il primo passo consiste nell'assicurarsi di soddisfare i seguenti prerequisiti:
     *      .NET Core SDK (3.1 o versione successiva)
     *      Node.js (6 o successivo)
     *  
     *  Controllare la versione di .NET Core eseguendo questo comando:
     *      dotnet --list-sdks
     *  Controllare la versione del nodo eseguendo questo comando:
     *      node -v
     *  
     *  Installare la versione più recente di .NET 6 SDK
     *  
     *  Installare l'ultima versione di Node.js LTS
     *  
     *  Esegui per installare il modello di progetto
     *      "dotnet new --install Clean.Architecture.Solution.Template"
     *  
     *  Creare una cartella per la soluzione e un CD 
     *  (il modello la utilizzerà come nome del progetto)
     *  
     *  Esegui per creare un nuovo progetto : 
     *    "dotnet new ca-sln"
     *    
     *      *  Questo comando creerà una nuova soluzione, automaticamente dotata di spazio dei 
     *      nomi utilizzando il nome della cartella padre. Ad esempio, se la cartella padre 
     *      è denominata Northwind, la soluzione sarà denominata Northwind.sln e lo spazio 
     *      dei nomi predefinito sarà .Northwind
     *    
     *  Passare al progetto e avviarlo utilizzando : 
     *    "src/WebUIdotnet run"
     *    
     *      * Avviare la soluzione da Visual Studio 2019 è banale, basta premere F5.
     *      Per avviare la soluzione utilizzando l'interfaccia della riga di comando 
     *      di .NET Core, sono necessari alcuni passaggi aggiuntivi. Puoi saperne di 
     *      più visitando il link sopra, ma includerò le informazioni qui per completezza.
     *      Innanzitutto, è necessaria una variabile di ambiente denominata con un valore: 
     *      ASPNETCORE_EnvironmentDevelopment 
     *      SET ASPNETCORE_Environment=Developmentexport ASPNETCORE_Environment=Development
     *      
     * Quindi, eseguire il comando seguente dalla cartella della soluzione:
     *      
     *      cd src/WebUI
     *      dotnet build
     *      
     * Quindi eseguire per avviare l'applicazione. Verrà visualizzato il seguente messaggio:
     *      
     *      dotnet run
     *      
     *      Now listening on: https://localhost:port
     *      La porta è di solito 5001. Aprire il sito Web accedendo a https://localhost:port. 
     *  
     *      // Figura 2 : Fig100.png
     *      
     * Struttura della soluzione
     * 
     *      Il modello di soluzione genera una soluzione multiprogetto. 
     *      Per una soluzione denominata Northwind, viene creata la seguente 
     *      struttura di cartelle:
     *      
     *      // Figura 3 : Fig103.png
     *  
     *  O Altrimenti in allgato  al progetto corrente in formato compresso 
     *  si può usare il progetto per fare un repository locale per una
     *  qualche installazione di NuGet server aziendale e rendere disponibile
     *  questo template in azienda.
     *  
     *      // File : CleanArchitecture-main
     *      
     *      
     * I nomi dei progetti all'interno di src si allineano strettamente ai livelli del 
     * diagramma Clean Architecture, con l'unica eccezione di WebUI, che rappresenta il 
     * livello Presentation.
     * 
     * Il progetto Domain rappresenta il livello Domain e contiene logica enterprise o 
     * domain e include entità, enumerazioni, eccezioni, interfacce, tipi e logica specifici 
     * del livello di dominio. Questo livello non ha dipendenze da nulla di esterno.
     * 
     * Il progetto Application rappresenta il livello Application e contiene tutta la logica 
     * di business. Questo progetto implementa CQRS (Command Query Responsibility Segregation), 
     * con ogni caso d'uso aziendale rappresentato da un singolo comando o query. 
     * Questo livello dipende dal livello Dominio ma non ha dipendenze da altri livelli o progetti. 
     * Questo livello definisce le interfacce implementate dai livelli esterni. 
     * Ad esempio, se l'applicazione deve accedere a un servizio di notifica, verrà aggiunta 
     * una nuova interfaccia all'applicazione e l'implementazione verrà creata all'interno 
     * dell'infrastruttura.
     * 
     * Il progetto Infrastruttura rappresenta il livello Infrastruttura e contiene classi per 
     * l'accesso a risorse esterne quali file system, servizi Web, SMTP e così via. Queste classi 
     * devono essere basate su interfacce definite all'interno del livello Application.
     * 
     * Il progetto WebUI rappresenta il livello Presentazione. Questo progetto è una SPA 
     * (single page app) basata su Angular 8 e ASP.NET Core. Questo livello dipende sia dai 
     * livelli Applicazione che Infrastruttura. Si noti che la dipendenza dall'infrastruttura 
     * è solo per supportare l'iniezione di dipendenze. Pertanto Startup.cs dovrebbe includere 
     * l'unico riferimento all'infrastruttura.
     * 
     * Test
     * 
     * La cartella dei test contiene numerosi progetti di unit test e di integrazione per 
     * consentire l'avvio rapido dell'operatività. I dettagli di questi progetti saranno 
     * esplorati in un post di follow-up. Nel frattempo, sentiti libero di esplorare e porre 
     * qualsiasi domanda di seguito.
     * 
     * Tecnologie
     * 
     * Oltre a .NET Core, all'interno di questa soluzione vengono utilizzate numerose tecnologie, 
     * tra cui:
     *      CQRS con MediatR
     *      Convalida con FluentValidation
     *      Mappatura oggetto-oggetto con AutoMapper
     *      Accesso ai dati con Entity Framework Core
     *      API Web con ASP.NET Core
     *      Interfaccia utente con Angular 8
     *      API aperta con NSwag
     *      Sicurezza con ASP.NET Core Identity + IdentityServer
     *      Test automatizzati con xUnit.net, Moq e Shouldly
     *
     
     *  Configurazione del database
     *  
     *  Il modello è configurato per l'utilizzo di un database in memoria per impostazione 
     *  predefinita. Ciò garantisce che tutti gli utenti siano in grado di eseguire la 
     *  soluzione senza dover configurare un'infrastruttura aggiuntiva (ad esempio.SQL Server).
     *  
     *  Se si desidera utilizzare SQL Server, sarà necessario aggiornare WebUI/appsettings.json 
     *  come segue:
     *      
     *      "UseInMemoryDatabase": false
     *      
     * Verificare che la stringa di connessione DefaultConnection all'interno di appsettings.json 
     * punti a un'istanza di SQL Server valida.
     * 
     * Quando si esegue l'applicazione, il database verrà creato automaticamente (se necessario) e 
     * verranno applicate le migrazioni più recenti.
     * 
     * Migrazioni di database
     * 
     * Per utilizzare le migrazioni, assicurarsi innanzitutto che "UseInMemoryDatabase" sia 
     * disabilitato, come descritto nella sezione precedente. Quindi, aggiungi i seguenti 
     * flag al tuo comando (i valori presuppongono che tu stia eseguendo dalla radice del repository)
     * dotnet-ef
     * 
     *      --project src/Infrastructure (facoltativo se in questa cartella)
     *      --startup-project src/WebUI
     *      --output-dir Persistence/Migrations
     * 
     * Ad esempio, per aggiungere una nuova migrazione dalla cartella principale:
     * 
     *      dotnet ef migrations add "SampleMigration" --project src\Infrastructure --startup-project src\WebUI --output-dir Persistence\Migrations
     * 
     * Panoramica
     * 
     *  Con Clean Architecture, i livelli Dominio e Applicazione sono al centro della progettazione. 
     *  Questo è noto come il nucleo del sistema.
     *  Il livello Dominio contiene la logica e i tipi dell'organizzazione e il livello 
     *  Applicazione contiene la logica e i tipi di business. 
     *  La differenza è che la logica aziendale potrebbe essere condivisa tra molti sistemi, 
     *  mentre la logica di business verrà in genere utilizzata solo all'interno di questo sistema.
     *  
     *  Il core non dovrebbe dipendere dall'accesso ai dati e da altri problemi di infrastruttura, 
     *  quindi tali dipendenze sono invertite. Ciò si ottiene aggiungendo interfacce o astrazioni 
     *  all'interno di Core che vengono implementate da livelli esterni a Core. 
     *  Ad esempio, se si desidera implementare il modello Repository, è necessario farlo 
     *  aggiungendo un'interfaccia all'interno di Core e aggiungendo l'implementazione all'interno 
     *  dell'infrastruttura.
     *  
     *  Tutte le dipendenze fluiscono verso l'interno e Core non ha dipendenza da nessun altro 
     *  livello. Infrastruttura e Presentazione dipendono dal Core, ma non l'una dall'altra.
     * 
     *      // Figura 1 : Fig1.png
     *      
     * Ciò si traduce in architettura e design che è:
     *      - Indipendente dai framework non richiede l'esistenza di alcuno strumento o framework
     *      - Testabile facile da testare - Core non ha dipendenze da nulla di esterno, quindi 
     *        scrivere test automatizzati è molto più facile
     *      - Indipendentemente dalla logica dell'interfaccia utente è tenuto fuori dall'interfaccia 
     *        utente, quindi è facile passare a un'altra tecnologia - in questo momento potresti 
     *        usare Angular, presto Vue, alla fine Blazor!
     *      - Indipendentemente dal database, i problemi di accesso ai dati sono separati in 
     *        modo pulito, quindi passare da SQL Server a CosmosDB o altro è banale
     *      - Indipendentemente da qualsiasi cosa esterna, infatti, Core è completamente isolato 
     *        dal mondo esterno – la differenza tra un sistema che durerà 3 anni e uno che 
     *        durerà 20 anni
     *      - Nel design di cui sopra, ci sono solo tre cerchi, potresti averne bisogno di più. 
     *        Pensa a questo come un punto di partenza. Ricorda solo di mantenere tutte le 
     *        dipendenze rivolte verso l'interno.
     *
     * Modello di soluzione
     * 
     *  Questo modello fornisce un approccio eccezionale alla creazione di soluzioni basate 
     *  su ASP.NET Core 3.1 e Angular 8 che seguono i principi di Clean Architecture. 
     *  Se Angular non fa per te, non preoccuparti, puoi rimuoverlo con facilità. 
     *  In questa sezione verrà installato il modello, verrà creata una nuova soluzione e 
     *  verrà esaminato il codice generato.
     *
     *  Dominio
     *          
     *          Questo conterrà tutte le entità, enum, eccezioni, interfacce, tipi e logica 
     *          specifici per il livello di dominio.
     *          
     *  Applicazione
     *  
     *          Questo livello contiene tutta la logica dell'applicazione. Dipende dal livello 
     *          di dominio, ma non ha dipendenze da altri livelli o progetti. 
     *          Questo livello definisce le interfacce implementate dai livelli esterni. 
     *          Ad esempio, se l'applicazione deve accedere a un servizio di notifica, 
     *          verrà aggiunta una nuova interfaccia all'applicazione e verrà creata 
     *          un'implementazione all'interno dell'infrastruttura.
     *          
     *  Infrastruttura
     *  
     *          Questo livello contiene classi per l'accesso a risorse esterne quali file system, 
     *          servizi Web, smtp e così via. Queste classi devono essere basate su interfacce 
     *          definite all'interno del livello dell'applicazione.
     *          
     *  WebUI
     *          
     *          Questo livello è un'applicazione a pagina singola basata su Angular 13 e 
     *          ASP.NET Core 6. Questo livello dipende sia dai livelli Applicazione che 
     *          Infrastruttura, tuttavia, la dipendenza dall'infrastruttura è solo per 
     *          supportare l'iniezione di dipendenze. Pertanto, solo l.cs attasia dovrebbe 
     *          fare riferimento all'infrastruttura.
     */

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
