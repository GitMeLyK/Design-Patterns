using System;

namespace DotNetDesignPatternDemos.DataAccess.ORM.EF
{
    /*
     *  Per questa tecnica di programmazione non potevamo non citare in questo
     *  contesto come esempio l'Entity Framework di Microsoft che rimane l'ORM
     *  di default da usare con framework di sviluppo sotto .NET
     *  
     *  Entity Framework non è necessario se si vuole sotto il framework usare 
     *  un ORM diverso lo si può fare e sono disponibili diversi altri tipi
     *  come ad esempio NHybernate che è il fratello di Hybernate sotto Java
     *  o Dapper o FluentOrm e tanti altri, ma fondamentalmente spiccano per 
     *  scelte architetturali quando le necessità di implementazione devono 
     *  essere snelle e leggere per software di non troppa complessità o per
     *  scelte aziendali da sviluppatori che già avevano skill su Hybernate.
     *  
     *  Che cos'è Entity Framework?
     *  
     *  Prima di .NET 3.5, gli sviluppatori scrivevano spesso codice ADO.NET o 
     *  Enterprise Data Access Block per salvare o recuperare i dati dell'applicazione 
     *  dal database sottostante. 
     *  
     *  Aprivano una connessione al database, creavano un DataSet per recuperare o inviare 
     *  i dati al database, convertivamo i dati dal DataSet in oggetti .NET o viceversa per 
     *  applicare regole di business. 
     *  
     *  Questo è stato un processo ingombrante e soggetto a errori. 
     *  
     *  Microsoft ha fornito un framework denominato "Entity Framework" per automatizzare 
     *  tutte queste attività correlate al database per l'applicazione.
     *  
     *  Entity Framework è un framework ORM open source per applicazioni .NET supportate da 
     *  Microsoft. 
     *  
     *  Consente agli sviluppatori di lavorare con i dati utilizzando oggetti di classi 
     *  specifiche del dominio senza concentrarsi sulle tabelle e sulle colonne di database 
     *  sottostanti in cui sono archiviati questi dati. 
     *  
     *  Con Entity Framework, gli sviluppatori possono lavorare a un livello più elevato 
     *  di astrazione quando si occupano di dati e possono creare e mantenere applicazioni 
     *  orientate ai dati con meno codice rispetto alle applicazioni tradizionali.
     *  
     *  Definizione ufficiale: 
     *      "Entity Framework è un mapper relazionale a oggetti (O/RM) che consente 
     *       agli sviluppatori .NET di lavorare con un database utilizzando oggetti 
     *       .NET. Elimina la necessità della maggior parte del codice di accesso ai 
     *       dati che gli sviluppatori di solito devono scrivere".
     *       
     *  Nella figura seguente viene illustrato dove Entity Framework si inserisce nell'applicazione.
     *  
     *      // Figura 1 : ef-in-app-architecture.png
     *      
     *  Caratteristiche di Entity Framework
     *  
     *      Multipiattaforma: 
     *      EF Core è un framework multipiattaforma che può essere eseguito su Windows, Linux e Mac.
     *      
     *      Modellatura: 
     *      EF (Entity Framework) crea un EDM (Entity Data Model) basato su entità POCO (Plain Old CLR Object) 
     *      con proprietà get/set di diversi tipi di dati. 
     *      Utilizza questo modello quando esegue query o salva i dati dell'entità nel database sottostante.
     *      
     *      Query: 
     *      EF consente di utilizzare query LINQ (C#/VB.NET) per recuperare i dati dal database sottostante. 
     *      Il provider di database tradurrà queste query LINQ nel linguaggio di query specifico del 
     *      database (ad esempio.SQL per un database relazionale). 
     *      EF ci consente inoltre di eseguire query SQL non elaborate direttamente nel database.
     *      
     *      Rilevamento delle modifiche: 
     *      EF tiene traccia delle modifiche apportate alle istanze delle entità (valori delle 
     *      proprietà) che devono essere inviate al database.
     *      
     *      Risparmio: 
     *      EF esegue i comandi INSERT, UPDATE e DELETE nel database in base alle modifiche 
     *      apportate alle entità quando si chiama il metodo. EF fornisce anche il metodo asincrono.
     *      SaveChanges() SaveChangesAsync()
     *      
     *      Concorrenza: 
     *      EF utilizza la concorrenza ottimistica per impostazione predefinita per proteggere 
     *      le modifiche di sovrascrittura apportate da un altro utente da quando i dati sono 
     *      stati recuperati dal database.
     *      
     *      Transazioni: 
     *      EF esegue la gestione automatica delle transazioni durante l'esecuzione di query o 
     *      il salvataggio dei dati. Fornisce inoltre opzioni per personalizzare la gestione 
     *      delle transazioni.
     *      
     *      Memorizzazione nella cache: 
     *      EF include il primo livello di memorizzazione nella cache immediatamente. 
     *      Pertanto, le query ripetute restituiranno i dati dalla cache anziché colpire il 
     *      database.
     *      
     *      Convenzioni integrate: 
     *      EF segue le convenzioni sul modello di programmazione della configurazione e include 
     *      una serie di regole predefinite che configurano automaticamente il modello EF.
     *      
     *      Configurazioni: 
     *      EF ci consente di configurare il modello EF utilizzando gli attributi di annotazione 
     *      dei dati o l'API Fluent per ignorare le convenzioni predefinite.
     *      
     *      Migrazioni: EF fornisce un set di comandi di migrazione che possono essere eseguiti 
     *      nella console di Gestione pacchetti NuGet o nell'interfaccia della riga di comando 
     *      per creare o gestire lo schema del database sottostante.
     *      
     * Entity Framework Ultime versioni
     * 
     *  Microsoft ha introdotto Entity Framework nel 2008 con .NET Framework 3.5
     *  . Da allora, ha rilasciato molte versioni di Entity Framework. 
     *  Attualmente, esistono due versioni più recenti di Entity Framework: EF 6 ed EF Core. 
     *  La tabella seguente elenca importanti differenze tra EF 6 e EF Core.
     *  
     *          // Figura 2 : ef6-vs-efcore.png
     *          
     *      EF 6 cronologia delle versioni
     *      Versione EF	Anno di rilascio	.NET Framework
     *      EF 6 ·	2013	.NET 4.0 e .NET 4.5, VS 2012
     *      EF 5 ·	2012	.NET 4.0, VS 2012
     *      EF 4,3	2011	.NET 4.0, VS 2012
     *      EF 4,0	2010	.NET 4.0, VS 2010
     *      EF 1.0 (o 3.5)	2008	.NET 3.5 SP1, VS 2008
     *
     *  Note sulle release e maggiori dettagli implementativi al seguente indirizzo.:
     *  
     *      https://learn.microsoft.com/it-it/ef/ef6/what-is-new/?redirectedfrom=MSDN
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
