using System;

namespace DotNetDesignPatternDemos.DataAccess.DAO.ADO
{
    /*
     * In casa microsoft il modello di accesso ai dati incluso sia nei vecchi prodotti
     * come visual basic e poi in .net fino all'ultimo nato .net core.
     * 
     * Mostreremo un riepilogo dell'evoluzione di Microsoft ActiveX Data Objects, 
     * spiegando alcuni concetti di database e infine forniremo alcuni esempi di codice.
     * 
     * Un DBMS (Database Management System) è un sistema software in grado di gestire raccolte 
     * di dati di grandi dimensioni, condivise e persistenti, e di garantirne l'affidabilità e 
     * la privacy. 
     * 
     * ODBC (Open Database Connectivity) è un DBMS di Microsoft. 
     * 
     * ODBC è una specifica per un'API che definisce un set standard di routine con cui 
     * un'applicazione può accedere ai dati in un'origine dati.
     * 
     * I sistemi software dedicati specificamente alla gestione dei dati esistono solo da 
     * 40 anni, e ancora alcune applicazioni non ne fanno uso. 
     * 
     * In assenza di software specifici, la gestione dei dati viene eseguita per mezzo di 
     * linguaggi di programmazione tradizionali, ad esempio C, Fortran o Visual Basic. 
     * Visual Basic 3.0 è stata la prima versione con funzionalità di accesso al database. 
     * 
     * Data Access Object (DAO) consentiva ai programmatori di accedere ai database locali 
     * nel formato Microsoft Jet Database Engine, che erano principalmente file ISAM 
     * (Indexed Sequential Access Method). 
     * 
     * Dopo DAO sono arrivati RDO e poi ActiveX Data Objects (ADO). 
     * 
     * Queste tecnologie di accesso ai dati sono state progettate per un paradigma client/server. 
     * 
     * Tuttavia la tendenza del calcolo distribuito ha costretto lo sviluppo di una nuova tecnologia 
     * per risolvere i problemi di manipolazione dei dati su un'architettura a più livelli. 
     * 
     * ADO.NET è l'evoluzione di ADO e i suoi componenti sono benn progettati per funzionare 
     * correttamente su un'architettura a più livelli.
     * 
     * La tabella seguente è un riepilogo dell'evoluzione degli oggetti di database per accedere 
     * ai dati da Microsoft:
     * 
     *  -- DAO	--
     *      Oggetti di accesso ai dati	La prima interfaccia orientata agli oggetti che ha esposto 
     *      il modulo di gestione di database Microsoft Jet che consentiva agli sviluppatori che 
     *      utilizzavano Visual Basic di connettersi direttamente alle tabelle di Access e ad 
     *      altri database tramite ODBC. 
     *      È ideale per database di piccole dimensioni in distribuzioni locali e applicazioni a 
     *      sistema singolo.
     *      
     *  -- RDO  -- ·	
     *      Oggetti dati remoti	Un'interfaccia di accesso ai dati orientata agli oggetti per ODBC 
     *      combinata con la semplice funzionalità di DAO che consente un'interfaccia a quasi 
     *      tutta la bassa potenza e flessibilità di ODBC. RDO non può accedere a Jet dei database 
     *      ISAM in modo efficiente. RDO fornisce gli oggetti, le proprietà e i metodi necessari 
     *      per accedere agli aspetti più complessi delle stored procedure e dei set di risultati 
     *      complessi.
     *      
     *  -- ADO --	
     *      Oggetti dati Microsoft ActiveX	ADO è il successore di DAO/RDO. 
     *      ADO è il consolidamento di quasi tutte le funzionalità di DAO e RDO.
     *      ADO include principalmente funzionalità in stile RDO per interagire con le origini dati 
     *      OLE DB, oltre alla comunicazione remota e alla tecnologia DHTML.
     *  
     *  -- ADO MD --	
     *      Microsoft ActiveX Data Objects Multidimensionale, fornisce un facile accesso ai dati 
     *      multidimensionali da linguaggi quali Microsoft Visual Basic e Microsoft Visual C++. 
     *      ADO MD estende Microsoft ActiveX Data Objects (ADO) per includere oggetti specifici 
     *      dei dati multidimensionali, ad esempio gli oggetti CubeDef e Cellset. 
     *      Per utilizzare ADO MD, il provider deve essere un provider di dati multidimensionale 
     *      (MDP) come definito dalla specifica OLE DB per OLAP. 
     *      Gli MDP presentano i dati in visualizzazioni multidimensionali anziché in provider 
     *      di dati tabulari (TDP) che presentano i dati in visualizzazioni tabulari. 
     *      Con ADO MD è possibile esplorare lo schema multidimensionale, eseguire query su un 
     *      cubo e recuperare i risultati.
     *      
     * -- ADOX -- 	
     *      Estensioni di Microsoft ActiveX Data Objects per il linguaggio e la sicurezza della 
     *      definizione dei dati.
     *      È un'estensione degli oggetti ADO e del modello di programmazione. 
     *      ADOX include oggetti per la creazione e la modifica di schemi, nonché la sicurezza. 
     *      Poiché si tratta di un approccio basato su oggetti alla manipolazione dello schema, 
     *      è possibile scrivere codice che funzionerà con varie origini dati indipendentemente 
     *      dalle differenze nelle sintassi native. 
     *      ADOX è una libreria complementare agli oggetti ADO principali. 
     *      Espone oggetti aggiuntivi per la creazione, la modifica e l'eliminazione di oggetti 
     *      dello schema, ad esempio tabelle e procedure. Include inoltre oggetti di sicurezza 
     *      per gestire utenti e gruppi e per concedere e revocare autorizzazioni sugli oggetti.
     *      
     *  -- RDS --	
     *      Servizio dati remoto. È possibile spostare i dati da un server a un'applicazione 
     *      client o a una pagina Web, modificare i dati sul client e restituire gli aggiornamenti 
     *      al server in un unico round trip.
     *      
     *  -- ADO.NET --	
     *      Oggetti dati Microsoft ActiveX .NET ADO.NET è interamente basato su XML. 
     *      ADO.NET fornisce un accesso coerente alle origini dati, ad esempio Microsoft SQL 
     *      Server, nonché alle origini dati esposte tramite OLE DB e XML. 
     *      Le applicazioni consumer di condivisione dati possono utilizzare ADO.NET per 
     *      connettersi a queste origini dati e recuperare, modificare e aggiornare i dati.
     *      ADO.NET in modo pulito l'accesso ai dati dalla manipolazione dei dati in componenti 
     *      discreti che possono essere utilizzati separatamente o in tandem. 
     *      ADO.NET include provider di dati .NET per la connessione a un database, 
     *      l'esecuzione di comandi e il recupero dei risultati. Tali risultati vengono elaborati 
     *      direttamente o inseriti in un oggetto DataSet ADO.NET per essere esposti all'utente 
     *      in modo ad hoc, combinati con dati provenienti da più origini o remoti tra i livelli. 
     *      L'oggetto DataSet ADO.NET può essere utilizzato anche indipendentemente da un provider 
     *      di dati .NET per gestire i dati locali nell'applicazione o provenienti da XML.
     *      Le classi ADO.NET si trovano in System.Data.dll e sono integrate con le classi XML 
     *      disponibili in System.Xml.dll. 
     *      Quando si compila codice che utilizza lo spazio dei nomi System.Data, fare riferimento 
     *      sia a System.Data.dll che a System.Xml.dll. 
     *      Rispetto ad ADO non esiste un oggetto Recordset.
     *      In ADO.NET esistono quattro classi che leggono e scrivono dati da origini dati:
     *          - Connessione .- Connettiti all'origine dati
     *          - Comando .- Esegui stored procedure
     *          - DataAdapter .- Collega DataSet al database
     *          - DataReader .- Forward/only, read/only cursore
     *  
     *  I RecordSet ADO sono uno strumento utile per la modifica dei dati in uno scenario Windows, 
     *  uno scenario basato su COM, e vengono utilizzati anche con ASP. 
     *  Tuttavia, recordset perde il suo fascino quando si desidera ottenere l'interoperabilità 
     *  delle applicazioni su Internet. 
     *  
     *  ADO.NET ha diviso il RecordSet in tre diversi oggetti, DataSet, DataReader e DataAdapter. 
     *  
     *  Questi nuovi oggetti sono centrati su XML, quindi forniscono nuove funzionalità per creare 
     *  lo sviluppo di applicazioni distribuite. 
     *  
     *  Alcune di queste funzionalità sono l'accesso disconnesso ai dati relazionali tramite DataSet 
     *  o l'utilizzo di oggetti XML tramite XSLT e XPath.
     *  
     *  Questa è una tabella delle differenze nel componente di usare la connessione ai dati.:
     *  
     *      DAO             ADO (ADODB)             Note
     *      DBEngine        None
     *      Workspace       None
     *      Database        Connection
     *      Recordset       Recordset
     *      Dynaset-Type    Keyset                 Retrieves a set of pointers to the records in the recordset.
     *      Snapshot-Type   Static                  Both retrieve full records, but a Static recordset can be updated.
     *      Table-Type      Keyset...               with adCmdTableDirect option.
     *      Field           Field                   When referred to in a recordset.
     * 
     *  Esempi e differenze 
     *  
     *  DAO     ->  Open a Recordset
     *          
     *          Dim db as Database
     *          Dim rs as DAO.Recordset
     *          Set db = CurrentDB()
     *          Set rs = db.OpenRecordset("Employees")
     *          
     *          ->  Edit Recordset
     *          
     *          rs.Edit 
     *          rs("TextFieldName") = "NewValue"
     *          rs.Update
     * 
     *  ADO     -> Open a Recordset
     *  
     *          Dim rs as New ADODB.Recordset
     *          rs.Open "Employees", CurrentProject.Connection, adOpenKeySet, adLockOptimistic
     *          
     *          ->  Edit Recordset
     *          
     *          rs("TextFieldName") = "NewValue" 
     *          rs.Update
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
