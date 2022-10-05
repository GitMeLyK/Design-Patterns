using System;

namespace DotNetDesignPatternDemos.DataAccess.DataAccessor
{
    /*
     *  La logica di questo pattern è facilmente intuibile e viene proposto un 
     *  codice di esempio pratico e intuibile.
     *  
     *  Scenario
     *  
     *      Si consideri un componente lato server utilizzato per aggiornare il database 
     *      di un'applicazione da un warehouse centrale: 
     *      
     *      i dati per ogni tabella devono essere estratti dal data warehouse e 
     *      aggiornati/inseriti nel database dell'applicazione. 
     *      
     *      Il codice del database richiede più operazioni fisiche del database e la 
     *      gestione delle risorse corrispondenti. 
     *      
     *      Se si mescola questo codice con la logica dell'applicazione, diventa 
     *      rapidamente disordinato e difficile da mantenere
     *      
     *      public void UpdateProducts()
     *      {
     *          // Get database connection string from somewhere
     *          // Get data warehouse connection string from somewhere
     *
     *          // Open database connection
     *          // If connection failed to launch an exception
     *
     *          // Open database connection // Open database warehouse connection
     *          // If connection failed to open an
     *
     *          // Ciclo di ogni prodotto e aggiornamento del database
     *
     *          // Se l'aggiornamento non riesce, registra l'errore e procedi al prodotto successivo
     *
     *          // Chiudi la connessione al data warehouse // Chiudi connessione al database
     *      }
     *      
     * I problemi più grandi sorgono se è necessario supportare più piattaforme di database 
     * (SQL Server, Oracle, Sybase) o incorporare ottimizzazioni come le connessioni al pool. 
     * 
     * La funzione di accesso dati risolve questo problema creando un'astrazione delle operazioni 
     * logiche che nasconde i dettagli di accesso ai dati di basso livello dal resto del codice 
     * dell'applicazione. 
     * 
     * Le operazioni logiche esposte dalla funzione di accesso dati dipendono dai requisiti 
     * dell'applicazione. 
     * 
     * Di seguito sono riportati alcuni consigli per incapsulare i dettagli di accesso ai dati 
     * fisici:
     * 
     *      - Esporre operazioni logiche e incapsulare operazioni fisiche.
     *        Ad esempio, per ADO.NET, sarà necessario esporre varianti di ExecuteDatset, 
     *        ExecuteScalar, ExecuteNonQuery ed ExecuteReader. 
     *        Il codice nell'esempio illustra questa idea.
     *        
     *      - Esporre risorse logiche e incapsulare risorse fisiche 
     *        Un esempio di risorsa di database è una connessione al database. 
     *        Se si consente alle applicazioni di gestire le proprie connessioni al database, 
     *        sarà difficile incorporare miglioramenti come il pool di connessioni. 
     *        Tuttavia, se si fornisce un handle di connessione logico, le applicazioni 
     *        potrebbero utilizzarlo per associare le proprie operazioni ai pool di 
     *        connessioni fisiche.
     *        
     *      - Normalizzare e formattare i dati
     *        I database spesso archiviano oggetti binari di grandi dimensioni (BLOB) come 
     *        matrice o flusso di byte non elaborati. L'implementazione della funzione di 
     *        accesso dati può essere responsabile della conversione dei dati dell'applicazione 
     *        in un BLOB prima di accedere al database.
     *        
     *      - Incapsulare i dettagli della piattaforma 
     *        Il comportamento dell'applicazione non deve cambiare se si tratta di accedere 
     *        a un database SQL Server o Oracle. In termini di ADO.NET, la funzione di accesso 
     *        dati può esporre le stesse operazioni logiche per ogni provider .NET. 
     *        Ad esempio, se la funzione di accesso dati espone un'operazione ExecuteScalar, 
     *        la funzione di accesso ai dati nasconderà i dettagli di questa chiamata per 
     *        SQL Server e Oracle mantenendola completamente trasparente per l'applicazione.
     *        
     *      - Incapsulare i dettagli dell'ottimizzazione 
     *        Il codice dell'applicazione non deve eseguire ottimizzazioni dell'accesso ai dati 
     *        come pool e cache. Tutte queste ottimizzazioni dovrebbero essere incapsulate dalla 
     *        funzione di accesso ai dati. 
     *        Ciò rende molto più facile gestire e modificare queste ottimizzazioni in futuro.
     *      
     *      // Figura 1 : DataAccessorStaticStructure.gif
     *      
     * L'interfaccia IDataAccessor definisce l'astrazione dell'accesso ai dati in termini di operazioni 
     * logiche che verranno utilizzate dal codice dell'applicazione. 
     * 
     * Mentre viene visualizzata un'interfaccia, è tipico raggruppare le operazioni correlate 
     * logicamente in interfacce separate, ad esempio IDataAccessorQuery per le operazioni di query, 
     * IDataAccessTransaction per le operazioni basate sulle transazioni e così via. 
     * 
     * OracleDataAccessor e SQLServerDataAccessor sono classi concrete che forniscono implementazioni 
     * effettive di operazioni logiche in termini di operazioni di database fisico. 
     * 
     * Ciascuna di queste classi concrete dipende quindi da una specifica tecnologia di database.
     * 
     *  // Code 1
     * 
     * Nota: alcune operazioni comuni a entrambe le classi concrete SQLServerDataAccessor e 
     * OracleDataAccessor possono essere fattorizzate in una classe base da cui entrambe le 
     * funzioni di accesso ai dati dovranno ereditare.
     * 
     * Applicabilità Utilizzare questo modello quando:
     * 
     *      Si desidera nascondere la complessità dell'accesso fisico ai dati e i problemi 
     *      della piattaforma dalla logica dell'applicazione. 
     *      Questo requisito è tipico delle applicazioni distribuite.
     *      
     *      Si desidera definire più implementazioni di accesso ai dati e scegliere tra 
     *      loro in fase di esecuzione. Ad esempio, la funzione di accesso dati deve offrire 
     *      la stessa interfaccia pur essendo in grado di accedere a SQL Server, Oracle, 
     *      Sybase e così via.
     * 
     *  Strategie / Varianti
     *  
     *  Considerare queste strategie quando si progetta un'interfaccia di accesso dati.
     *  
     *      - Definire operazioni logiche versatili 
     *        Nell'esempio fornito, una versione di overload di ogni operazione logica consente 
     *        l'esecuzione di più istruzioni SQL in una singola chiamata.
     *        
     *      - Incorpora miglioramenti e punti di ottimizzazione 
     *        I modelli di accesso ai dati semplificano l'integrazione di funzionalità e 
     *        ottimizzazioni aggiuntive del database nelle versioni successive. 
     *        Ad esempio, nell'esempio precedente, è possibile rilasciare la funzione di accesso 
     *        dati con supporto solo per SQL Server. Le funzioni di accesso ai dati per altre 
     *        piattaforme come Oracle e Sybase possono quindi essere rilasciate nelle versioni 
     *        successive.
     *        
     *      - Protezione contro l'utilizzo inefficiente delle applicazioni 
     *        Un'applicazione può tentare di aprire le connessioni il prima possibile anziché 
     *        il più tardi possibile, il che rappresenta un modo efficiente per gestire le 
     *        connessioni al database. Incapsulando le operazioni di connessione, è possibile 
     *        eliminare la possibilità di una gestione inefficiente della connessione da parte 
     *        dell'applicazione. 
     *        In altre parole, progettare la funzione di accesso ai dati in modo tale da rendere 
     *        difficile per il codice dell'applicazione creare operazioni di accesso ai dati 
     *        inefficienti.
     *        
     *      - Utilizzare le fabbriche di accesso dati
     *        Definire una fabbrica accessibile a livello globale per creare un'istanza di 
     *        implementazione della nuova funzione di accesso ai dati. 
     *        La factory può determinare quale implementazione della funzione di accesso dati 
     *        creare un'istanza in base a un argomento del costruttore.
     *        
     * Benefici
     * 
     *      - Pulizia del codice dell'applicazione 
     *        L'utilizzo di una funzione di accesso dati ben progettata renderà l'applicazione 
     *        più pulita e più facile da gestire.
     *        
     *      - Facilità di adozione di nuove piattaforme o funzionalità di database 
     *        Se il codice di accesso ai dati fisici è stato distribuito in tutta l'applicazione, 
     *        sarà difficile aggiungere il supporto per nuove funzionalità o utilizzare una nuova 
     *        piattaforma di database, poiché sarà necessario modificare tutto il codice dell'applicazione. 
     *        Al contrario, con le funzioni di accesso ai dati, tutte le modifiche o i miglioramenti 
     *        saranno isolati in componenti separati.
     *        
     *      - Incapsulamento delle strategie di ottimizzazione Il codice di accesso ai dati è in 
     *        genere un candidato principale per l'ottimizzazione e l'ottimizzazione.
     *        
     *      - Isolare tutti gli accessi ai dati in pochi componenti consente di applicare facilmente 
     *        la tecnica di ottimizzazione che può interessare l'intero sistema.
     *        
     *      - Supporto multipiattaforma 
     *        Il codice dell'applicazione può essere scambiato tra più implementazioni della 
     *        funzione di accesso ai dati (Oracle e SQL Server) senza dover modificare la 
     *        logica dell'applicazione.
     *        
     * Passività
     * 
     *      - Limiti controllo delle applicazioni di accesso ai dati 
     *        Il codice dell'applicazione è limitato alle operazioni logiche definite dalla 
     *        funzione di accesso ai dati. Se la funzione di accesso dati non è ben progettata, 
     *        il codice dell'applicazione potrebbe dover utilizzare alcune soluzioni alternative 
     *        scomode che possono portare a risultati imprevisti.
     *        
     */

    /*
     * Code 1
     * 

        * Dichiarazione dell'interfaccia IDataAccessor. 
        * Ogni operazione logica (ad esempio, ExecuteNonQuery) supporta istruzioni 
        * SQL singole e multiple 
        
        public interface IDataAccessor
        {
            // ExecuteNonQuery
            Int32 ExecuteNonQuery(String strCS, CommandType cmdType, String strCommandText);
            Int32[] ExecuteNonQuery(String strCS, CommandType[] acmdType, String[] astrCommandText);

            ExecuteDataset
            DataSet ExecuteDataset(String strCS, CommandType cmdType, String strCommandText);
            DataSet[] ExecuteDataset(String strCS, CommandType[] acmdType, String[] astrCommandText);

            ExecuteDataReader
            IDataReader ExecuteDataReader(String strCS, CommandType cmdType, String strCommandText);
            IDataReader[] ExecuteDataReader(String strCS, CommandType[] acmdType, String[] astrCommandText);

            ExecuteScalar
            Object ExecuteScalar(String strCS, CommandType cmdType, String strCommandText);
            Object[] ExecuteScalar(String strCS, CommandType[] acmdType, String[] astrCommandText);
        }

        public class OracleDataAccessor : IDataAccessor
        {
        ...

            // Oracle-implementation of IDataAccessor 

            // ExecuteNonQuery
            Int32 ExecuteNonQuery(String strCS, CommandType cmdType, String strCommandText)
            {
                // Use Oracle-specific ADO.NET classes such as OracleCommand, OracleDataAdapter, etc.
            }

            Int32[] ExecuteNonQuery(String strCS, CommandType[] acmdType, String[] astrCommandText, ref Array[] asqlParam)
            {
                 // Usa classi di ADO.NET specifiche di Oracle come OracleCommand, OracleDataAdapter, ecc. 
            }

        ...
        }

        public class SQLServerDataAccessor : IDataAccessor
        {

            // SQL Server-implementazione di IDataAccessor 

            // ExecuteNonQuery
            Int32 ExecuteNonQuery(String strCS, CommandType cmdType, String strCommandText)
            {
                // Utilizzare classi di ADO.NET specifiche di SQL Server come SqlCommand, SqlDataAdapter, ecc. 
            }

            Int32[] ExecuteNonQuery(String strCS, CommandType[] acmdType, String[] astrCommandText, ref Array[] asqlParam)
            {
                // Usa classi di ADO.NET specifiche di SQL Server come SqlCommand, SqlDataAdapter, ecc. 
            }

        ...
        }

        // Utilizzare una factory (non visualizzata) per creare una funzione di accesso dati di
        // SQLServer 
        IDataAccessor obSQLServer = new DBAccessFactory( eAccessor.SQLServer);

        // Dati di accesso
        obSQLServer.ExecuteNonQuery( strConn, CommandType.Text, "select * from T1" );

        // Utilizzare una factory (non visualizzata) per creare una funzione di accesso
        // dati Oracle 
        IDataAccessor obOracle = new DBAccessFactory( eAccessor.Oracle);

        // Dati di accesso
        obOracle.ExecuteNonQuery( strConn, CommandType.Text, "select * from T2" );

        */

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
