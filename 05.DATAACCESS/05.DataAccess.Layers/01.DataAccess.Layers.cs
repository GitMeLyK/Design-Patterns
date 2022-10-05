using System;

namespace DotNetDesignPatternDemos.DataAccess.Layers
{

    /*
     *  Il modello di accesso ai dai in strati (layers) conforma come in un modello 
     *  archittetturale ortogonale le funzionalità di applicazioni che accedono ai 
     *  dati con livelli crescenti di astrazione.
     * 
     * Scenario
     * 
     *  I modelli precedenti descrivevano strategie per disaccoppiare i concetti di dettagli 
     *  di accesso ai dati, modello di dati e mapping relazionale a oggetti dal codice 
     *  dell'applicazione. 
     *  
     *  Questi concetti sono ortogonali perché ognuno può essere affrontato separatamente 
     *  senza influenzare l'altro. 
     *  
     *  Il modello di dati di un'applicazione è spesso ortogonale ai suoi meccanismi di 
     *  accesso ai dati. 
     *  
     *  Il modello di dati è la struttura statica dei dati, mentre l'accesso ai dati illustra 
     *  come spostare i dati dall'archivio dati al codice dell'applicazione. 
     *  
     *  La modifica del modello di dati in genere non richiede una modifica nel modello 
     *  di dati e in genere è vero l'inverso.
     *  
     *  Il disaccoppiamento dei componenti ortogonali è quasi sempre una buona idea perché 
     *  ti dà la libertà di modificarli in modo indipendente. I componenti a grana più fine 
     *  possono anche essere ortogonali. 
     *  
     *  Ad esempio, il codice all'interno di una funzione di accesso ai dati spesso affronta 
     *  due problemi distinti: 
     *      - la gestione delle connessioni che comporta l'inizializzazione delle connessioni 
     *      - il corretto raggruppamento delle connessioni e la generazione di istruzioni 
     *        ADO.NET (SQL) per l'accesso ai dati. 
     * 
     *  Questa è pochissima sovrapposizione tra la soluzione per questi due problemi, quindi ha senso 
     *  disaccoppiarli in moduli separati.
     *  
     *  E' generalmente affermato, che i componenti software sono ortogonali se affrontano 
     *  problemi completamente disgiunti e possono essere assemblati in qualsiasi ordine per 
     *  costruire una soluzione globale. 
     *  
     *  Tuttavia, è importante notare che l'ortogonalità non preclude la dipendenza. 
     *  
     *  In altre parole, solo perché due moduli risolvono problemi completamente diversi, 
     *  ciò non significa che non si possa costruire l'uno senza l'altro.
     *  
     *  Il modello Layers descrive come impilare più componenti ortogonali insieme per 
     *  formare un'applicazione robusta e gestibile. 
     *  
     *  Un livello è un insieme di componenti che implementano un'astrazione software. 
     *  
     *  È possibile impilare layer astratti che decompongono in modo incrementale una 
     *  soluzione fino alla sua implementazione fisica finale. 
     *  
     *  I livelli gestiscono efficacemente la mappatura da concetti astratti a 
     *  un'implementazione concreta in fasi, risolvendo il problema un pezzo alla volta. 
     *  
     *  Nella figura seguente viene illustrato uno stack di livelli tra la logica 
     *  dell'applicazione e l'accesso fisico al database:  
     *      
     *      // Figura 1 : Layers.jpg
     * 
     *  Il modello di cui sopra può quindi essere generalizzato come segue:
     *   
     *      // Figura 2 : LayersGeneral.jpg
     *      
     * Come con qualsiasi componente ortogonale, è sempre una buona pratica disaccoppiare i livelli 
     * in modo che dipendano solo da astrazioni di livello inferiore, piuttosto che dai dettagli di 
     * implementazione. 
     * 
     * Quando si definisce un'astrazione (interfaccia) per ogni livello, è possibile visualizzare 
     * un'implementazione a livello singolo come adattatore da un'astrazione all'altra.
     * 
     * Il codice di accesso ai dati è suscettibile di costruire con i livelli perché ci sono così 
     * tanti aspetti ortogonali in esso. 
     * 
     * Ad esempio, i seguenti dettagli sono candidati per l'implementazione all'interno di un 
     * livello separato:
     * 
     *      - Mappatura degli oggetti di dominio 
     *          La mappatura tra dati relazionali e oggetti di dominio deve essere eseguita 
     *          all'interno di un singolo livello.
     *          
     *      - Conversione dei dati 
     *          I dati dell'applicazione spesso devono essere convertiti in un formato di dati 
     *          utilizzabile dal database e viceversa, ad esempio TEXT, NTEXT, BLOB, CLOB, ecc. 
     *          Se esiste la possibilità di supportare piattaforme di database diverse, è 
     *          opportuno definire un livello separato dedicato alle conversioni del formato dei dati.
     *      
     *      - Mappatura delle operazioni dei dati 
     *          Le operazioni di database logiche suggerite dal modello di accesso ai dati devono 
     *          essere implementate in un livello separato.
     *          
     *      - La gestione delle risorse del database di gestione delle risorse è un buon candidato 
     *          per l'isolamento in un livello separato perché le ottimizzazioni delle risorse 
     *          possono avere un effetto significativo sulle prestazioni del sistema. 
     *          Sono incluse strategie per il pool di connessioni, la memorizzazione nella cache 
     *          delle istruzioni e i timeout delle risorse. 
     *          Se non si isolano queste operazioni all'interno del proprio livello separato, 
     *          i componenti di altri livelli dovranno assorbire queste operazioni. 
     *          Ovviamente questo ha l'effetto di rendere il codice più complesso e difficile 
     *          da ottimizzare.
     *          
     *      - Memorizzazione nella cache 
     *          La memorizzazione nella cache dei dati a cui si accede di frequente può migliorare 
     *          notevolmente le prestazioni. Come altre ottimizzazioni, la memorizzazione nella 
     *          cache introduce complessità, quindi è un buon candidato per l'isolamento in un 
     *          livello separato. Le operazioni di memorizzazione nella cache in genere definiscono 
     *          le stesse operazioni di input e output, ma intercettano alcune operazioni di input 
     *          quando i dati memorizzati nella cache sono già disponibili.
     *         
     *      - Autorizzazione
     *          È possibile implementare l'autorizzazione a livello di applicazione all'interno 
     *          di un livello software confrontando le operazioni con un set di regole di 
     *          autorizzazione definite da un amministratore.
     *          
     *      - Registrazione
     *          È possibile utilizzare un livello separato per la registrazione per intercettare 
     *          tutte le richieste e le risposte per fornire un approccio di registrazione coerente. 
     *          Potrebbe anche essere utile definire più livelli di registrazione che tengano traccia 
     *          delle operazioni mentre passano attraverso diversi livelli di astrazione.
     *     
     *      // Figura 3 : LayersStatic.gif
     *      
     *  Il codice dell'applicazione funziona in termini di Layer 1, il livello di accesso ai dati 
     *  più astratto. 
     *  
     *  Ogni strato di M implementa la sua interfaccia e delega le chiamate all'interfaccia successiva 
     *  e meno astratta M + 1. 
     *  
     *  Lo strato N (non mostrato) è l'abstract del contratto di locazione e interagisce direttamente 
     *  con il fornitore di ADO.NET. 
     *  
     *  Questa struttura statica può essere soggetta ad alcune varianti:
     *  
     *      - Descrivere un livello con più interfacce in quanto potrebbe non essere possibile 
     *        definire un livello in termini di una singola interfaccia.
     *        
     *      - Definisci un'astrazione utilizzando una classe anziché un'interfaccia, in particolare 
     *        per quegli oggetti che rappresentano strutture di dati semplici come una semplice riga 
     *        o anche un'eccezione.
     *        
     *      - Utilizzare una stratificazione non rigorosa poiché i livelli non devono formare una 
     *      struttura lineare rigorosa. Ad esempio, potresti trovare conveniente saltare un livello 
     *      in determinate occasioni, specialmente per operazioni ottimizzate.
     * 
     *  Il diagramma di sequenza del modello Livelli è mostrato di seguito:
     *  
     *      // Figura 4 : LayersSequence.gif
     *      
     *  Il codice dell'applicazione richiama un'interfaccia sul livello 1 che delega a 
     *  un'interfaccia sul livello di calcestruzzo 2 e così via fino al raggiungimento 
     *  dell'ultimo strato di calcestruzzo. 
     *  
     *  In realtà, la maggior parte delle implementazioni di accesso ai dati a più livelli 
     *  non sono così semplici : ad esempio, è abbastanza comune che una singola chiamata 
     *  all'istanza di livello M concreta venga mappata a più chiamate al livello successivo 
     *  (livello M + 1)
     *  
     *  Esempio
     *  
     *      Il livello 3 è il livello della funzione di accesso ai dati. 
     *      L'interfaccia IDataAccess definisce l'astrazione di questo livello utilizzando 
     *      operazioni di database logiche e non espone alcuna semantica dell'interfaccia 
     *      SQL o a livello di chiamata. 
     *      
     *      Questo livello è il livello più in basso ed è implementato in termini di 
     *      ADO.NET fornitore effettivo:
     *  
     *      // Code 1
     *      
     *      Il livello 1 è il livello dell'oggetto business ed è responsabile dell'implementazione 
     *      di varie operazioni aziendali. 
     *      
     *      Il livello 2 è il livello di oggetti di dominio attivo responsabile del mapping dei 
     *      dati fisici agli oggetti di dominio (business). 
     *      
     *      Questo livello delega tutte le chiamate al database al livello 1 per accedere al 
     *      database:
     *      
     *      // Code 2
     *      
     *      Nota come la funzione SubmitOrder accede alla funzionalità di livello 2 (che accede 
     *      alla funzionalità di livello 1) per recuperare le informazioni sui clienti e sugli ordini:
     *      
     *      // Code 3
     *      
     * Applicabilità
     * 
     *  Utilizzare questo modello quando:
     *  
     *      - Si desidera disaccoppiare il modello di dati, i dettagli di accesso ai dati o 
     *        qualsiasi altra funzionalità ortogonale che si prevede di modificare in modo 
     *        indipendente.
     *        
     *      - Si desidera definire più livelli incrementali di astrazione per semplificare 
     *        lo sviluppo e la manutenzione.
     *        
     *      - Si desidera prototipare o costruire un sistema gradualmente utilizzando 
     *        l'implementazione stubbed o semplificata del livello e quindi compilare 
     *        implementazioni più scalabili o ottimizzate più avanti nel processo di sviluppo.
     * 
     * Strategie / Varianti
     * 
     *  Considerate queste strategie quando progettate con il modello Livelli:
     *  
     *      - Implementazioni di stub layer per la prototipazione 
     *          La suddivisione delle funzionalità dell'applicazione in livelli consente di definire 
     *          stub (interfacce) per i livelli e questo a sua volta consente di sviluppare componenti 
     *          contemporaneamente o creare versioni non ottimizzate per le prime dimostrazioni. 
     *          I livelli di stub sono anche utili per costruire l'intera struttura a strati 
     *          all'inizio del processo di sviluppo. 
     *          Questo è importante in quanto garantisce che tutti i livelli interagiscano come 
     *          previsto. A volte si verificano interdipendenze di livello impreviste che richiedono 
     *          di ridefinire uno o più livelli. 
     *          È molto più facile risolvere questo tipo di problema prima di creare una quantità 
     *          significativa di codice in ogni livello.
     *      
     *      - Isolare l'inizializzazione del livello 
     *          Le implementazioni del livello di accoppiamento avvengono passando un'istanza 
     *          ConcreteLayer M+1 come parametro del costruttore a un'istanza ConcreteLayer M 
     *          o chiamando una proprietà su un'istanza ConcreteLayer M. 
     *          A quel punto, ConcreteLayer M salva un riferimento a ConcreteLayer M+1 in modo 
     *          che possa richiamare le sue operazioni. 
     *          Con l'aumentare del numero di livelli, diventa più difficile tenere traccia 
     *          della combinazione e dell'ordine dei livelli. 
     *          Pertanto, isolare tutte le inizializzazioni dei livelli all'interno di un 
     *          singolo modulo che serve anche a descrivere la struttura di stratificazione 
     *          dell'applicazione. Se è necessario riorganizzare i livelli e quindi le relative 
     *          inizializzazioni, è possibile farlo all'interno del modulo di inizializzazione.
     *          
     * Benefici
     * 
     *  - I livelli di decomposizione della progettazione software sono una strategia conveniente 
     *  per scomporre un progetto software di grandi dimensioni in componenti più piccoli e più 
     *  gestibili.  Ciò consente di risolvere problemi su larga scala all'inizio del ciclo di 
     *  sviluppo e di  ottimizzare i dettagli in un secondo momento.
     *  
     *  - Modularizzazione delle feature 
     *  La definizione dei livelli consente di crearli e gestirli contemporaneamente e in modo 
     *  indipendente.
     *  
     *  - Incapsulamento dei dettagli 
     *  Ogni livello incapsula l'implementazione di una o più funzionalità di accesso ai dati. 
     *  Ciò solleva altri livelli da qualsiasi dipendenza, rendendo l'accoppiamento dei livelli 
     *  più flessibile e l'implementazione più mirata.
     *  
     *  - Pluggability
     *  La definizione di astrazioni di livelli chiari consente di collegare una o più 
     *  implementazioni di un particolare livello durante le inizializzazioni di runtime. 
     *  Questa può essere una potente strategia per supportare più semantiche e topologie 
     *  di accesso ai dati diverse con un unico set di interfacce.
     *  
     * Passività
     * 
     *  Interazione dei livelli e complessità dell'inizializzazione
     *  
     *  I livelli interagiscono tra loro tramite interfacce. 
     *  
     *  Man mano che si creano più livelli, è necessario mantenere le loro interazioni per 
     *  garantire che la soluzione sia ancora praticabile. 
     *  
     *  Queste interazioni diventano più complesse con più livelli, specialmente se la 
     *  struttura del livello non è lineare.
     *  
     */

    /*
     * Code 1
     * 
     * 
           // LAYER 3 

            public interface IDataAccess
            {
                void ExecuteNonQuery(string strConnectio, string strSQL);
                object ExecuteScalar(string strConnectio, string strSQL);
                DataSet ExecuteDataSet(string strConnectio, string strSQL);
                DataReader ExecuteDataReader(string strConnectio, string strSQL);
            }

            public class DataAccessSQLServer : IDataAccess
            {
                // IDataAccess implementation 
                void ExecuteNonQuery(string strConnectio, string strSQL)
                { // Perform required data access using SQL Server ADO.NET Provider  }

                object ExecuteScalar(string strConnectio, string strSQL)
                {  // Perform required data access using SQL Server ADO.NET Provider  }

                DataSet ExecuteDataSet(string strConnectio, string strSQL)
                { // Perform required data access using SQL Server ADO.NET Provider  }

                DataReader ExecuteDataReader(string strConnectio, string strSQL)
                { // Perform required data access using SQL Server ADO.NET Provider }
            }

            public class DataAccessOracle : IDataAccess
            {
                // IDataAccess implementation 
                void ExecuteNonQuery(string strConnectio, string strSQL)
                { // Perform required data access using Oracle ADO.NET Provider }
            ...
        }

        
    */

    /*
     * Code 2
     * 

            // LAYER 2 

            public interface ICustomerDataAccess
            {
                DataRow GetCustomer(int nID);
                void DeleteCustomer(int nID);
                void CreateCustomer(string strFirstName, string strLastName);
            }

            public class CustomerDataAccess : ICustomerDataAccess
            {
                // ICustomerDataAccess implementation 
                DataRow GetCustomer(int nID)
                {
                    // Do some pre-processing, for example creating parameters for a stored procedure
                    // that will be called via the data accessor

                    // Get data access component Note how we get an interface 'instance' and access the subsequent
                    // data access layer via its interface
                    IDataAccess access = GetDataAccessor(Accessor.SQLServer);
                    DataSet ds = access.ExecuteDataSet(strConnectionString, "spGetCustomer", aParams);

                    // Return data
                    return ds.Tables[0].Rows[0];
                }

                void DeleteCustomer(int nID)
                {
                    // Do some pre-processing

                    // Get an interface 'instance' and access the subsequent data access layer via its interface
                }

                void CreateCustomer(string strFirstName, string strLastName)
                {
                    // Same logic as above ... 
                }
            }

            public interface IOrderDataAccess
            {
                void UpdateOrder(int nID);
                void DeleteOrder(int nID);
                void CreateOrder(string strFirstName, string strLastName);

            }

            public class OrderDataAccess : IOrderDataAcess
            {
                // IOrderDataAccess implementation 

                public void AddOrder( ... )
                {
                    // Do some pre-processing, for example creating parameters for a stored procedure
                    // that will be called via the data accessor

                    // Get data access component Note how we get an interface 'instance' and access the subsequent
                    // data access layer via its interface
                    IDataAccess access = GetDataAccessor(Accessor.SQLServer);
                    access.ExecuteDataSet(strConnectionString, "spAddOrder", aParams);
                }
            }

     */

    /*
     * Code 3
     * 

           // LAYER 1 

           public interface IOrder
           {
               void SubmitOrder( ... );
               void CancelOrder( ... );
               void AmendOrder( ... )
           }

           public class Order : IOrder
           {
               // IOrder implementation 
               void SubmitOrder( int nOrderID, int nCustomerID )
               {
                   // Create a new order
                   OrderDataAccess obOrder = new OrderDataAccess();
                   obOrder.AddOrder( nOrderID );

                   // Get customer identified by nCustomerID and link to this order
                   CustomerDataAccess obCust = new CustomerDataAccess();
                   DataRow roCust = obCust.GetCustomer( nCustomerID );

                   ... 

               }

               void CancelOrder( ... ) { ... }
               void AmendOrder( ... )  { ... }
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
