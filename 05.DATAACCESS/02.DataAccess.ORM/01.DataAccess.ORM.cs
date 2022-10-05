using System;

namespace DotNetDesignPatternDemos.DataAccess.ORM
{
    /*
     * L'Object Relational Mapping come tecnica di programmazione affronta diversi approcci
     * nella connessione verso i dati persistenti in una qualsiasi fonte dati astraendo per
     * lo sviluppatore l'accesso diretto al data access object ma interponendosi con esso,
     * fornendo metodi e api necessarie per la lettura e il salvataggio degli stessi.
     * 
     * Andiamo in dettaglio su un tipo di esempio base usato con il pattern ORM
     * 
     * Scopo
     * 
     *      Disaccoppia gli oggetti di dominio attivi dal modello di dati sottostante e dai 
     *      dettagli di accesso ai dati. 
     *      
     *      Un oggetto Object-Relational Map è responsabile del mapping dei dati relazionali a 
     *      concetti orientati agli oggetti, consentendone la modifica indipendentemente 
     *      dall'applicazione e dai relativi oggetti di dominio.
     *      
     * Scenario
     * 
     *      Nella figura seguente viene illustrato come una mappa relazionale oggetto 
     *      disaccoppia gli oggetti di dominio attivi dai dati e dal modello di dati sottostante:
     * 
     *      // Figura 1 : ObjectRelationMapOverview.gif
     *      
     * Un sofisticato sistema Object-Relational Map mapperebbe i dettagli del database utilizzando i 
     * metadati. 
     * 
     * Questo approccio versatile consente di modificare i metadati senza ricompilare o aggiornare 
     * l'applicazione. 
     * 
     * È comune archiviare i metadati di mapping nelle tabelle di database e definire strumenti di 
     * amministrazione per consentire agli utenti di visualizzare e aggiornare i dettagli del mapping 
     * senza dover comprendere la struttura dei metadati sottostante.
     * 
     * Il problema più fondamentale che una mappa relazionale a oggetti affronta è come mappare 
     * i concetti del database di relazioni a concetti orientati agli oggetti. 
     * 
     * Il mapping degli oggetti di dominio è responsabile della conversione di una riga da una 
     * determinata tabella a un oggetto che espone le colonne di tale riga come proprietà. 
     * 
     * Questa situazione è mostrata di seguito:
     * 
     *      // Figura 2 : RowToObjectMapping.gif
     *      
     * Nella tabella seguente viene illustrata una strategia comune per l'utilizzo di C# e ADO.NET
     * 
     *      Relational Database Concept	            Object-Oriented Concept
     *      Table	                                class.          An instance of this class is a domain object.
     *      Row	                                    DataRow         object encapsulated within a domain object
     *      Column	                                set/get         properties 
     * 
     * In pratica, le cose non sono così semplici. 
     * 
     * Ad esempio, una mappa relazionale oggetto spesso nasconde dati relazionali che un'applicazione 
     * potrebbe non aver bisogno, ad esempio colonne timestamp e altre colonne utilizzate 
     * esclusivamente per definire le relazioni tra tabelle. 
     * 
     * Una mappa relazionale oggetto può convertire i dati in entrambe le direzioni (ad esempio, 
     * BLOB in Oracle) e calcolare gli attributi per conto dell'applicazione.
     * 
     * Ad esempio, i passaggi per generare una fattura all'interno di un'applicazione di 
     * elaborazione degli ordini sarebbero:
     * 
     *  - L'applicazione richiede un ordine e la mappa relazionale Object restituisce 
     *    un oggetto Order.
     *  - L'applicazione utilizza l'oggetto Order restituito dalla Object-Relational Map 
     *    per ottenere l'articolo dell'ordine di ogni ordine. Gli elementi Order devono 
     *    essere implementati come proprietà dell'oggetto Order.
     *  - L'applicazione utilizza le proprietà dell'oggetto Order (ad esempio il costo per ogni 
     *    articolo dell'ordine) per calcolare il valore totale della fattura.
     *  - Si noti che l'applicazione opera esclusivamente utilizzando oggetti di dominio e non 
     *    fa alcun riferimento esplicito al modello di dati o alle funzioni di accesso ai dati 
     *    sottostanti.
     * 
     *  // Figura 3 : ObjectRelationMapStaticStructure.gif
     *  
     * La figura precedente rappresenta la struttura statica di una varietà di una classe 
     * Object-Relational Map. 
     * 
     * L'interfaccia IPersistenceManager definisce le operazioni di database in termini di 
     * oggetto di dominio generico - definisce le operazioni per la lettura/scrittura/eliminazione 
     * di oggetti di dominio. 
     * 
     * Questa interfaccia non espone alcun dettaglio del database in quanto questi sono curati 
     * dalla funzione di accesso ai dati. 
     * 
     * Un oggetto ConcretePersistenceManager implementa IPersistenceManager per un oggetto 
     * di dominio specifico. 
     * 
     * Un ConcretePersistenceManager legge/scrive/elimina gli oggetti di dominio utilizzando 
     * l'aiuto sia della classe MapMetadata per descrivere il mapping degli oggetti di dominio 
     * alla tabella di database relazionale che di IDataAccessor per accedere al database. 
     * 
     * Un ConcretePersistenceManager incapsula quindi l'accesso ai dati e i dettagli del 
     * modello di dati, nonché la mappatura degli oggetti di dominio.
     * 
     * La sequenza di operazioni inizia quando un'applicazione richiama l'operazione di lettura 
     * su un ConcretePersistenceManager:
     * 
     * L'implementazione di classi Object-Relational Map è in genere un'impresa non banale. 
     * 
     * Scrivere una mappa relazionale a oggetti efficiente, versatile e basata su metadati 
     * può essere un compito difficile. 
     * 
     * Tuttavia, ci sono molti prodotti commerciali che possono essere impiegati direttamente 
     * all'interno della tua applicazione.
     * 
     *      Figura 3 : // ObjectRelationMapSequence.gif
     *  
     * ConcretePersistenceManager trova i metadati pertinenti che descrivono i dettagli del 
     * mapping, ottiene i dati relazionali dal database e quindi crea un nuovo oggetto di 
     * dominio e inizializza utilizzando i dati relazionali e i metadati.
     * 
     * Esempio
     * 
     *  In questo esempio viene illustrata l'applicazione di elaborazione degli ordini descritta 
     *  nella sezione Scenario. 
     *  
     *  L'oggetto dominio attivo in questo esempio è OrderItem che rappresenta un singolo 
     *  elemento in un determinato ordine. 
     *  
     *  Il gestore di salvataggio permanente è responsabile del mapping del contenuto degli 
     *  oggetti OrderItem alla tabella [OrderItems]. In fase di esecuzione, il gestore della 
     *  salvataggio permanente crea un'istanza di un oggetto OrderItem per ogni riga della 
     *  tabella [OrderItems]. 
     *  
     *  Ogni proprietà in OrderItem corrisponde a una colonna della tabella [OrderItems]. 
     *  
     *  Nella definizione della classe OrderItem riportata di seguito, si noti che è definita 
     *  utilizzando concetti di dominio orientati agli oggetti puri e che non vi è alcun 
     *  riferimento al 
     *  
     *      // Code 1.
     *  
     *  La classe Order ha uno scopo simile a OrderItem in quanto rappresenta il concetto di 
     *  dominio dell'ordine di un cliente utilizzando la semantica orientata agli oggetti. 
     *  
     *  Anche in questo caso, la gestione della persistenza è responsabile del mapping del 
     *  contenuto degli oggetti Order alla tabella [Orders]
     *  
     *      // Code 2
     *  
     *  Si noti come l'oggetto Order faccia riferimento a una raccolta di OrderItems. 
     *  
     *  In questo modo viene modellata la relazione di aggregazione ONE-TO-MANY tra le tabelle 
     *  [Orders] e [OrderItems].
     *  
     *  Gli oggetti Object-Relational Map non definiscono alcun dettaglio di mapping. 
     *  
     *  Invece lo sviluppatore definisce questi dettagli di mapping utilizzando i metadati, 
     *  più comunemente in XML. Questo XML può quindi essere archiviato in file di configurazione 
     *  o, molto preferibilmente, in database. 
     *  
     *  Le applicazioni complesse potrebbero richiedere alcuni strumenti GUI personalizzati 
     *  per creare graficamente questi mapping, mentre le applicazioni meno complesse potrebbero 
     *  richiedere di codificare l'XML a mano. 
     *  
     *  Di seguito è riportato un possibile file XML per il mapping della tabella [OrderItems] 
     *  all'oggetto di dominio [Order]
     *  
     *      // Code 3
     *      
     *  In generale, è facile scrivere uno strumento (ad esempio un componente aggiuntivo di 
     *  Visual Studio.NET) in grado di leggere questo file di mapping XML e generare 
     *  automaticamente il codice precedentemente visualizzato per OrderItem e Order. 
     *  
     *  Questo strumento (o componente aggiuntivo) può effettivamente generare codice appropriato 
     *  e quindi aggiungerlo come nuovo file di progetto. 
     *  
     *  Gli utenti dovranno solo creare mapping XML e lo strumento si occupa della generazione 
     *  del codice.
     *  
     *  Il prossimo blocco di codice mostra come tutti questi pezzi si incastrano insieme. 
     *  
     *  L'applicazione inizializza un gestore di salvataggio permanente per leggere il file XML, 
     *  connettersi al database utilizzando l'elemento <Database> per ottenere i dati e infine 
     *  inizializzare gli oggetti di mapping appropriati come indicato dagli elementi 
     *  <MappingObject>.
     * 
     *      // Creare e inizializzare un gestore di persistenza 
     *      PersistenceManager pm = PersistenceManagerFactory.GetPersistenceManager();
     *      pm.Load( strMetadataFilePath );
     *      
     *      //Creare un nuovo oggetto order con un elenco di elementi
     *      Order ArrayList alOrderItems = new ArrayList();
     *      alOrderItems.Add( new OrderItem( ... ) );
     *      alOrderItems.Add( new OrderItem( ... ) );
     *      alOrderItems.Add( new OrderItem( ... ) );
     *      Order order = new Order( ..., alOrderItems)
     *      
     *      // Mantenere l'oggetto order nel database
     *      pm.PersisteObject( ordine );
     * 
     * 
     *  Applicabilità
     *  
     *      Utilizzare questo modello quando:
     *      
     *          - Si desidera nascondere il modello di dati fisici e la complessità dell'accesso 
     *            ai dati dalla logica dell'applicazione e dagli oggetti di dominio.
     *            
     *          - Si desidera incapsulare il mapping dominio-oggetto all'interno di un singolo 
     *            componente in modo da potersi adattare alle modifiche del modello di dati 
     *            senza modificare il codice dell'applicazione o le definizioni degli oggetti 
     *            di dominio.
     *            
     *          - Si desidera la versatilità necessaria per eseguire il mapping degli oggetti 
     *            di dominio a più modelli di dati senza modificare il codice dell'applicazione 
     *            o le definizioni degli oggetti di dominio. 
     *            Ciò consente di integrare l'applicazione con più modelli di dati 
     *            indipendentemente dalla loro definizione.
     * 
     * Strategie / Varianti
     * 
     *      Considerare queste strategie quando si progettano classi Object-Relational Map
     *      
     * Attributi non mappati
     * 
     *      In generale, gli oggetti di dominio che partecipano a Object-Relational Map 
     *      (come OrderItem e Order nell'esempio precedente) spesso corrispondono a una 
     *      riga in una tabella di database o in un join. 
     *      
     *      Tuttavia, non tutte le proprietà di un dominio oggetto sono archiviate in 
     *      un database. In alcuni casi, vengono calcolati o aggregati da campi diversi. 
     *      
     *      Questi attributi sono chiamati attributi non mappati perché non corrispondono 
     *      a un singolo dato relazionale. Gli attributi non mappati in genere non sono 
     *      presenti e generano problemi in quanto possono essere derivati da proprietà 
     *      (attribuite) già ottenute dall'oggetto dominio.
     *      
     * Identità dell'oggetto
     * 
     *      Affinché una persistenza riesca a mappare correttamente gli oggetti di dominio alle 
     *      righe corrispondenti in una tabella, è necessario definire la nozione di identità 
     *      su entrambi i lati della relazione. 
     *      
     *      Se una tabella definisce una chiave primaria, definisce già un'identità univoca per 
     *      le relative righe. Analogamente, se un oggetto di dominio definisce una o più 
     *      proprietà mappate alla chiave primaria della tabella, l'oggetto di dominio ha 
     *      un'identità. Due possibili motivi per definire l'identità dell'oggetto sono:
     *      
     *          - Possibilità di aggiornare singoli oggetti di dominio Nell'esempio sopra 
     *            riportato, per tenere conto del caso in cui un'applicazione deve aggiornare 
     *            un singolo OrderItem, è necessario introdurre un [OrderItems].[ OrderItemID] 
     *            come chiave primaria e la proprietà OrderItemID corrispondente nella classe 
     *            OrderItem. 
     *            Ciò consente alla persistenza di gestire i singoli oggetti di dominio OrderItem 
     *            senza dover inoltrare l'oggetto Order.
     *          - Oggetto dominio cache Un gestore di salvataggio permanente può memorizzare nella 
     *            cache gli oggetti di dominio a cui si accede frequentemente. Ciò consente un 
     *            accesso più rapido a questi oggetti di dominio. Questa varietà di meccanismi 
     *            di memorizzazione nella cache utilizza spesso un attributo identity come 
     *            chiave della cache poiché identifica in modo univoco un oggetto di dominio.
     *            
     * Relazioni di aggregazione
     *  
     *  Nei database, l'aggregazione comporta la corrispondenza di una chiave esterna in una 
     *  tabella a una chiave primaria in un'altra tabella. 
     *  
     *  Gli oggetti di dominio corrispondenti modellano relazioni simili, ma si traducono in 
     *  aggregazione di oggetti in cui un oggetto di dominio fa riferimento direttamente ad 
     *  altri (in genere tramite una raccolta). 
     *  
     *  È comune descrivere le relazioni tra tabelle come uno dei seguenti tipi:
     *  
     *  One-To-One
     *      Ogni riga della tabella T1 corrisponde esattamente a una riga in T2. 
     *      Allo stesso modo, ogni istanza dell'oggetto A corrisponde esattamente a 
     *      un'istanza dell'oggetto B.
     *  
     *  Uno-a-molti
     *      Ogni riga della tabella T1 corrisponde a zero o più righe in T2. 
     *      Analogamente, ogni istanza dell'oggetto A corrisponde a una raccolta di zero o 
     *      più istanze dell'oggetto B.
     *      
     *  Many-To-Many
     *      Qualsiasi numero di righe nella tabella T1 corrisponde a qualsiasi numero di 
     *      righe in T2. Analogamente, ogni istanza dell'oggetto A corrisponde a una 
     *      raccolta di istanze dell'oggetto B e ogni istanza dell'oggetto B corrisponde 
     *      a una raccolta di istanze dell'oggetto A. 
     *      Una strategia alternativa per la relazione molti-a-molti consiste nel definire 
     *      una terza tabella C che elenca le chiavi correlate di ogni tabella.
     *      
     * Indipendentemente dalla relazione, è responsabilità del responsabile della persistenza 
     * risolvere e gestire le istanze dell'oggetto B quando si occupa delle istanze dell'oggetto A. 
     * 
     * Nell'esempio precedente esiste una relazione uno-a-molti tra le classi Order e OrderItems. 
     * 
     * La classe Order espone questa relazione tramite la relativa proprietà di raccolta OrderItems.
     * 
     * 
     * Le stesse operazioni che utilizzano l'ereditarietà concreta o di tabelle di classi 
     * richiedono join o più operazioni di database. 
     * 
     * Tuttavia, l'ereditarietà di una singola tabella non consente di utilizzare in modo 
     * più efficiente l'archiviazione del database in quanto richiede che la tabella definisca 
     * l'unione di tutti gli attributi (colonne) all'interno di una gerarchia di ereditarietà.
     * 
     * La gestione della persistenza può implementare le operazioni di accesso ai dati 
     * corrispondenti utilizzando una o più delle seguenti strategie:
     * 
     *       - Utilizzare operazioni di join singolo 
     *          Questo è valido per le operazioni di lettura in cui il gestore della persistenza 
     *          emette una singola operazione di join nelle tabelle correlate. 
     *          Questa operazione di join restituisce tutti i dati necessari per popolare un set 
     *          di oggetti Order e gli OrderItems corrispondenti. 
     *          Questo approccio non può essere utilizzato con aggiornamenti ed eliminazioni.
     *          
     *       - Utilizzare più operazioni di query 
     *          Il gestore della persistenza può emettere due query, una per ogni tabella. 
     *          Questo funziona per letture, aggiornamenti ed eliminazioni.
     *          
     *       - Utilizzare l'inizializzazione pigra 
     *          Si tratta di un approccio alternativo in cui il gestore di persistenza 
     *          non popola gli oggetti di dominio aggregati  finché le applicazioni non 
     *          vi fanno riferimento. 
     *          Ad esempio, il gestore di salvataggio permanente esegue una query nella tabella 
     *          [Order] solo quando l'applicazione fa riferimento a un oggetto Order non mappato
     * 
     * Rapporti ereditari
     * 
     *  È comune che gli oggetti di dominio si relazionino tra loro per ereditarietà, ma i dati 
     *  relazionali non supportano un concetto simile. 
     *  
     *  Ad esempio, una classe Manager può estendere (o specializzare) una classe Employee 
     *  aggiungendo attributi che si applicano solo ai manager. 
     *  
     *  È comune per un gestore di persistenza mappare questi oggetti di dominio utilizzando 
     *  una di queste strategie:
     *  
     *      - Ereditarietà della tabella concreta 
     *          Ogni tipo di oggetto dominio concreto viene mappato 
     *          alla propria tabella. In questo esempio, è presente una tabella [Employee] 
     *          che contiene i dati per tutti i dipendenti che non sono manager e una 
     *          tabella [Manager] che contiene solo i dati per tutti i manager. 
     *          [Manager] contiene tutte le colonne in [Dipendente] e le colonne specifiche 
     *          del Manager.
     *          
     *      - Ereditarietà della tabella delle classi 
     *          Mappare ogni classe nella gerarchia di ereditarietà alla propria tabella. 
     *          In questo esempio, è presente una tabella [Employee] che contiene dati comuni 
     *          a dipendenti e manager e una tabella [Manager] che contiene dati specifici 
     *          solo per i manager. In questo approccio, [Manager] integra [Dipendente].
     *          
     *      - Ereditarietà di una singola tabella 
     *          Mappare tutti gli oggetti di dominio all'interno della stessa gerarchia alla 
     *          stessa tabella. In questo esempio è presente una singola tabella [Employee] 
     *          che contiene colonne mappate sia ai dipendenti che ai manager. 
     *          Questo tipo di tabelle dovrebbe sempre includere una colonna che identifichi 
     *          il tipo concreto per ogni riga. 
     *          Le colonne del manager devono essere impostate su null per le righe che 
     *          corrispondono ai dipendenti e viceversa.
     *          
     * L'ereditarietà di una singola tabella rende le query, l'aggiornamento batch e le 
     * eliminazioni batch più semplici ed efficienti da implementare poiché coinvolgono 
     * solo una singola tabella. 
     * 
     * Benefici
     * 
     *  Pulizia del codice dell'applicazione.
     *      Il codice dell'applicazione che funziona esclusivamente con gli oggetti di dominio è 
     *      molto più facile da comprendere e gestire il codice dell'applicazione che include il 
     *      modello di dati e i dettagli di accesso ai dati.
     *  
     *  Mappe a modelli di dati alternativi 
     *      I metadati possono essere commutati per utilizzare database diversi senza influire 
     *      sul codice dell'applicazione. L'incapsulamento dei dettagli del modello di dati 
     *      all'interno dei metadati consente di apportare modifiche, ad esempio la riorganizzazione 
     *      delle tabelle, lo spostamento su piattaforme diverse o persino lo spostamento in un 
     *      diverso tipo di archivio dati, ad esempio XML, senza dover ricompilare il 
     *      codice dell'applicazione.
     * 
     * Passività
     * 
     *  Dipendenza da prodotti commerciali di terze parti 
     *      Se si utilizza Object-Relational Map in modo estensivo, è probabile che si verifichino 
     *      sforzi di sviluppo significativi a meno che non ci si integri con un prodotto 
     *      Object-Relational Map commerciale o open source.
     *      
     *  Limita il controllo delle applicazioni dei dati
     *      Il codice dell'applicazione è limitato alle operazioni definite dall'interfaccia 
     *      IPersistenceManager. Poiché è ConcretePersistenceManager che incapsula i dettagli 
     *      di accesso ai dati, diventa più difficile ottimizzare le operazioni di database, 
     *      in particolare per i prodotti commerciali o open source.
     */

    /*
     * Code 1
     * 

    public class OrderItem
    {
        // Data members 
        private string m_strProductName;
        private long m_lOrderID;
        private long m_lProductID;
        private long m_lQuantity;
        private float m_fPrice;

        // Costruttore 
        public OrderItem( string name, long oid, long pid, long qty, float prc)
        {
            // Initialize member variables
        }

        // Properties. Utilizzato principalmente dal gestore di persistenza per impostare 
         * con i valori del database 
        string pubblica ProductName
        {
            get { return m_strProductName; }
            set { m_strProductName = (string)value; }
        }

        public long OrderID
        {
        get { return m_lOrderID ; }
        set { m_lOrderID = (long)value; }
        }

        public long ProductID { get { ... } set { ... } }
        public long Quantity { get { ... } set { ... } }
        public float Price { get { ... } set { ... } }

        // Altre funzioni pubbliche esposte da questo oggetto di dominio 
        ...
    }
    */

    /*
     * Code 2
     * 

        public class Order
        {
            //Data members 
            private DateTime m_dtOrderTime
            private long m_lOrderID;
            private float m_fTotal;
            //Ogni oggetto Order fa riferimento a una raccolta di OrderItems
            private ArrayList m_alOrderItems; 

            // Constructor 
            public Order(DateTime odt, long oid, float totl, ArrayList items)
            {
                // Inizializza variabili membro 
            }

            // Proprietà. Utilizzato principalmente dal gestore di persistenza per 
            // impostare con i valori del database 
            public DateTime OrderTime
            {
                get { return m_dtOrderTime; }
                set { m_dtOrderTime = (DateTime)value; }
            }

            public ArrayList OrderItems
            {
                get { return m_alOrderItems; }
                set { m_alOrderItems = (ArrayList)value; }
            }

            public long OrderID { get { ... } set { ... } }
            public float Total { get { ... } set { ... } }

            // Altre funzioni pubbliche esposte da questo oggetto di dominio 
            ...
        }
        */

    /*
     * Code 3
     * 
        <Root>
        <!-- Definizione archivio dati -->
        <DataStore>
        <Database server="..." database="..." uid="..." pwd="..." ></Database>
        </DataStore>

        definizioni di mapping <!-- - proprietà dell'oggetto al campo della tabella -->
        <MappingObjects>

        mapping <!-- per la classe OrderItem -->
        <MappingObject class ="OderItem" table="OrderItems">
        <Property name="ProductName" column="product_name" type="string"></Property>
        <Property name="OrderID" column="order_is" type="long"></Property>
        <Property name="ProductID" column="product_id" type="long" primarykey="true"></Property>
        <Property name="Quantity" column="quantity" type="long"></Property>
        <Property name="Price" column="price" type="float"></ Property>
        </MappingObject>

        mapping <!-- per la classe Order -->
        <MappingObject class="Order" table="Orders">
        <Property name="OrderTime" column="order_timestamp" type="DateTime"></Property>
        ...
        </MappingObject>

        </MappingObjects>
        </Root>         
    */


    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
