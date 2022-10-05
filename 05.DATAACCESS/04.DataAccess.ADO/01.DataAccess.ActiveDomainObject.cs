using System;

namespace DotNetDesignPatternDemos.DataAccess.ActiveObjectDomain
{

    /*
     *   Il modello di accesso ai dati utilizzato Active Object Domain usato in
     *   questo contesto fornisce un pattern di sviluppo in tutte quelle situazioni
     *   che c'è bisogno di incapsulare il modello di dati e i dettagli di accesso ai 
     *   dati all'interno di un oggetto di dominio pertinente. 
     *   
     *   In altre parole, un oggetto dominio attivo astrae la semantica dell'archivio dati 
     *   sottostante (ad esempio, SQL Server) e della tecnologia di accesso ai dati sottostante 
     *   (ad esempio, ADO.NET) e fornisce una semplice interfaccia programmatica per il recupero 
     *   e il funzionamento dei dati.
     *   
     *   Attenzione a non commettere similutidini con gli acronimi ADO pattern per Active Domain Object
     *   di cui stiamo parlando e ADO o ADO.NET che stà per ActivX Object Data e ne abbiamo parlato di 
     *   come è l'evoluzione di DAO sotto ambiente Microsoft.
     *   
     *   Scenario
     *      
     *      Si supponga che i dati siano stati ottenuti utilizzando un oggetto della funzione 
     *      di accesso ai dati. Le applicazioni che manipolano direttamente questi dati si 
     *      prestano a diversi problemi:
     *      
     *          -   Il codice dell'applicazione è direttamente associato a un modello di dati.
     *          
     *          -   Le modifiche o gli aggiornamenti del modello di dati a una nuova piattaforma 
     *              di database possono causare incompatibilità per il codice che si basa su 
     *              funzionalità di database specifiche.
     *              
     *          -   La combinazione di codice di dominio dell'applicazione con codice di 
     *              manipolazione dei dati riduce la gestibilità della leggibilità.
     *              
     *      L'oggetto dominio attivo (o il componente logico di accesso ai dati) risolve questi 
     *      problemi incapsulando un modello di dati e codice per modificare tale modello di dati 
     *      in un set di oggetti di dominio. L'applicazione utilizza quindi l'oggetto dominio 
     *      attivo per accedere e modificare i dati.
     *      
     *  Il termine attivo si riferisce agli oggetti di dominio che fanno molto di più che 
     *  rappresentare semplicemente i dati. Espongono operazioni logiche per occuparsi delle 
     *  interazioni di database pertinenti. 
     *  
     *  Alcune di queste operazioni logiche (denominate utilizzando la terminologia di dominio e 
     *  non la terminologia di accesso ai dati) includono:
     *  
     *      - Inizializza:  inizializza l'oggetto dominio leggendo i dati da una o più tabelle.
     *      - Aggiorna:     aggiorna il contenuto dell'oggetto dominio dal database per garantire 
     *                      la coerenza.
     *      - Salva:        salva le modifiche al contenuto dell'oggetto di dominio inserendole 
     *                      o aggiornandole.
     *      - Elenco:       esegue query e restituisce dati
     *      
     *          // Figura 1 : ActiveDomainObjectStaticStructure.gif
     *          
     *      Un oggetto ActiveDomainObject definisce gli attributi logici e le operazioni che 
     *      rappresentano i concetti di dominio. Implementa le proprie operazioni interagendo 
     *      con un oggetto Data Accessor per accedere al database. 
     *      
     *      Gestisce anche la conversione dei dati tra il suo modulo di database e il suo modulo 
     *      di domanda.
     *      
     *          // Figura 2 : ActiveDomainObjectSequence.gif
     *          
     *  Un'applicazione crea un'istanza di un oggetto ActiveDomainObject, che si inizializza con 
     *  i dati del database. 
     *  
     *  Una volta inizializzato un oggetto ActiveDomainObject, l'applicazione può accedere alle 
     *  relative proprietà. 
     *  
     *  L'applicazione può quindi chiedere ad ActiveDomainObject di salvarne lo stato nel database.
     *  
     *  Esempio
     *  
     *   In questo esempio viene definito un oggetto dominio attivo che rappresenta un cliente:
     *   
     *      // Code 1
     *   
     *  Nota come Customer raccoglie i dati degli indirizzi da drCustomer per formare un oggetto Address.
     *  L'indirizzo può essere considerato come un oggetto di dominio, ma non è attivo nel senso che fa 
     *  realmente parte degli oggetti del Cliente; sono gli oggetti Customer che gestiscono il mapping 
     *  delle proprietà trovate negli oggetti Address. 
     *  
     *  Ciò consente inoltre all'oggetto Address di essere riutilizzabile da altre parti dell'applicazione.
     *  Si noti inoltre che ConnectionStringFactory incapsula la gestione delle stringhe di connessione per 
     *  Customer e altri oggetti di dominio attivi.
     *  
     *  Nell'esempio completo riportato di seguito viene utilizzato CollectionBase per creare un 
     *  insieme specializzato che accetta solo oggetti di tipo Customer anziché accettare ed 
     *  esporre oggetti contenuti come Object.
     *  
     *      // Code 2
     * 
     *  Applicabilità
     *  
     *      Utilizzare questo modello quando:
     *              
     *              - Si desidera nascondere il modello di dati e i dettagli di accesso ai dati 
     *                dalla logica dell'applicazione. Ciò mantiene la logica dell'applicazione 
     *                più pulita e più focalizzata sul business.
     *                
     *              - Si desidera nascondere il modello di dati e i dettagli di accesso ai dati 
     *                per mascherare incoerenze e oscurità
     *                
     * Strategie / Varianti
     * 
     *      La motivazione principale per la definizione di oggetti di dominio attivi è quella 
     *      di semplificare la scrittura e la manutenzione del codice dell'applicazione, pertanto 
     *      ha senso adattare gli oggetti di dominio ai concetti dell'applicazione. 
     *      Considerare queste strategie quando si progetta un oggetto dominio attivo.
     *      
     *      - Oggetti e tabelle di dominio 
     *        Gli oggetti di dominio non corrispondono necessariamente direttamente a una singola 
     *        tabella. La modalità di allineamento degli oggetti di dominio alle tabelle dipende 
     *        dalla granularità richiesta dalle applicazioni. Ad esempio, un oggetto dominio 
     *        Customer può corrispondere direttamente alla tabella [Customer], mentre un oggetto 
     *        Order può corrispondere a un join tra le tabelle [Order] e [OrderDetails].
     *        
     *       - Oggetti di dominio e proprietà e colonne
     *       È stato menzionato nella sezione prerequisiti che un oggetto di dominio attivo 
     *       corrisponde a un componente logico di accesso ai dati che incapsula il modello 
     *       di dati e i dettagli di accesso ai dati all'interno di un oggetto di dominio 
     *       pertinente. L'oggetto dominio attivo deve esporre i propri dati utilizzando 
     *       alcune o tutte le proprietà seguenti:
     *       
     *          - Proprietà per rappresentare l'intero dato
     *            Spesso corrisponde a una singola proprietà che restituisce un dataSet o 
     *            DataTable ADO.NET.
     *            
     *          - Proprietà per rappresentare una riga specifica Spesso corrisponde a una 
     *            singola proprietà che restituisce un ADO.NET DataRow.
     *            
     *          - Proprietà per rappresentare singole colonne all'interno di una singola riga 
     *            di tabella. Le proprietà degli oggetti di dominio (proprietà set e get in C#) 
     *            non corrispondono necessariamente direttamente alle colonne ottenute dai primi 
     *            due tipi di proprietà.
     *  
     *  Se le applicazioni richiedono solo determinate proprietà, fornire solo quelle. 
     *  Considerare inoltre se la forma fisica dei dati è adatta per le applicazioni. 
     *  Ad esempio, una colonna BLOB può essere esposta come proprietà MemoryStream di C#. 
     *  Può anche avere senso formare una singola proprietà da più colonne.
     *  
     *      - Logica di dominio 
     *        Gli oggetti di dominio possono definire il comportamento. 
     *        In effetti, l'esposizione di operazioni logiche comuni da un oggetto di dominio 
     *        è spesso meno soggetta a errori rispetto all'esposizione delle proprietà e 
     *        all'aspettativa che il codice dell'applicazione implementi la stessa logica. 
     *        È inoltre possibile che non si espongano affatto determinate proprietà se le 
     *        operazioni sugli oggetti di dominio sono sufficienti per l'applicazione. 
     *        Ad esempio, un oggetto dominio Customer viene mappato alle tabelle [Customer] 
     *        e [Address]. Anziché esporre le proprietà per recuperare ogni colonna dell'indirizzo 
     *        del cliente, è possibile definire un'operazione GetAddress per concatenare le colonne 
     *        dell'indirizzo in un formato di indirizzo standard.
     *        
     *      - Definire una strategia di gestione delle connessioni coerente 
     *        Definire un oggetto factory stringa di connessione accessibile a livello globale e 
     *        che distribuisca una stringa di connessione a qualsiasi oggetto di dominio che ne 
     *        richieda una. In questo modo è possibile assegnare una stringa di connessione a tutti 
     *        gli oggetti di dominio.
     *        
     * Benefici
     * 
     *      - Pulizia del codice dell'applicazione 
     *          Il codice dell'applicazione che funziona con gli oggetti di dominio è molto più 
     *          semplice e pulito rispetto al codice che contiene il modello di dati e i dettagli 
     *          di accesso ai  dati.
     *   
     *      - Disaccoppia il codice dell'applicazione dal modello di dati 
     *          Questo modello consente di modificare facilmente il modello di dati nelle fasi 
     *          successive. 
     *          
     *      - Raggruppa il codice di accesso ai dati correlato in un singolo componente
     *          Poiché questo modello incapsula i dettagli di accesso ai dati, semplifica 
     *          l'identificazione e la riparazione dei difetti di accesso ai dati.
     *          
     * Passività
     * 
     *  - Distribuisce l'accesso ai dati su più oggetti di dominio 
     *      Ogni oggetto di dominio è responsabile della propria implementazione dell'accesso ai 
     *      dati. La distribuzione di questa responsabilità su più oggetti di dominio significa 
     *      che le modifiche alle strategie di accesso ai dati influiscono su ogni componente. 
     *      Ad esempio, ogni oggetto di dominio come Customer e Product può inizializzare una 
     *      funzione di accesso dati passando al costruttore della funzione di accesso dati una 
     *      stringa di connessione. È possibile decidere di modificare questa impostazione e passare 
     *      invece la stringa di connessione direttamente a ciascun metodo di accesso dati. 
     *      È probabile che questa modifica influisca su ogni oggetto di dominio.
     *      
     *  - Limita il controllo delle applicazioni dell'accesso ai dati 
     *      Il codice dell'applicazione che interagisce con l'oggetto dominio è limitato alle 
     *      proprietà e ai metodi esposti da questi oggetti di dominio. Se questi oggetti di 
     *      dominio non sono ben progettati, potrebbe essere necessario ricorrere a soluzioni 
     *      alternative non naturali o imbarazzanti.
     *      
     */

    /*
     * Code 1
     * 

            public class Customer
            {
                // Data members 
                private string               m_strConnection = "";        // Used by the data accessor object to access database
                private System.Data.DataRow  drCustomer      = null;
                private Address              obAddress       = null;

                // Constructors 
                public Customer()
                {
                    // Acquire the connection string from a connection-string factory-object
                    m_strConnection = ConnectionStringFactory.GetConnectionString();
                }

                // Data access methods 
                public System.Data.DataRow GetCustomer( int nID )
                {
                    // Use data access helper components to retrieve customer data via a stored procedure
                    drCustomer = ...
        
                    // Construct an Address object from customer data
                    obAddress = ...
 
                    // Return customer information
                    return drCustomer;
                }

                public int CreateCustomer( string strName, Address obAddress, string DateTime dtDOB )
                {
                    // Use data access helper components to create a new customer entry in the database
                    // and return customer ID
                }

                public void DeleteCustomer( int nCustomerID )
                {
                    // Use data access helper components to delete customer from the database
                }

                public void UpdateCustomer( DataRow drCustomer )
                {
                    // Use data access helper components to update customer in the database
                }

                // Properties 
                public string     Name         { get { return (string)drCustomer["Name"]; }
                public DateTime   MemberSince  { get { return (DateTime)drCustomer["RegistrationDate"]; }
                public bool       IsActive     { get { return (bool)drCustomer["Active"]; } }
                public Address    Address      { get { return obAddress; } }
            }

            public class Address
            {
                // Data members 
                private string         strAddress1 = "";
                private string         strAddress2 = "";
                private string         strCity     = "";
                private string         strPostCode = "";
                private string         strCountry  = "";

                // Constructors 
                public Address (string add1, string add2, string city, string pc, string cntry )
                {
                    // Populate data members
                }

                // Properties 
                public string Address1 { get {return strAddress1; } }
                public string Address1 { get {return strAddress2; } }
                public string City     { get {return strCity; } }
                public string PostCode { get {return strPostCode; } }
                public string Country  { get {return strCountry; } }
            }

            public class ConnectionStringFactory
            {
                public static string GetConnectionString()
                {
                    // Retrieve connection string from somewhere - config file, etc.
                }
            }

            // This Application-code block illustrates how to create and initialize a Customer object
            Customer obCustomer = new Customer();
            Address obAddress = new Address( ... );
            obCustomer.CreateCustomer( "Yazan", obAddress, dtDON ); 

    */

    /*
     * Code 2
     * 

        // CustomerCollection class that only contain objects of type Employee
        public class CustomerCollection : System.Collections.CollectionBase
        {
            // Constructors 
            public CustomerCollection() {}

            // IList interface implementation 

            // The Add method declaration permits only Customer objects to be added
            public void Add( Customer obCust )
            {
                this.List.Add( obCust );
            }

            // Implement a Remove method
            public void Remove( int nIndex )
            {
                // Check if index is valid
                if ((nIndex < 0) || (nIndex > this.Count - 1))
                    throw new ArgumentOutOfRangeException( "nIndex", "Index is not valid");

                this.List.RemoveAt( nIndex );
            }

            // Implement an indexer that returns only Employee objects (and not object of type Object)
            public Customer this[int nIndex]
            {
                get { return (Customer)this.List[nIndex]; }
                set { List[nIndex] = value; }
            }
        }

        private void CollectionBase_Click(object sender, System.EventArgs e)
        {
            // Create some employee obejcts
            Customer obCust1 = new Customer();
            obCust1.CreateCustomer( ... );

            Customer obCust2 = new Customer();
            obCust2.CreateCustomer( ... );

            // Create our specialized collection
            CustomerCollection coll = new CustomerCollection();

            coll.Add( obCust1 );
            coll.Add( obCust2 );
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
