using System;

namespace DotNetDesignPatternDemos.Architectural.ActiveRecord
{
    /*
     * Il modello di active record  è un approccio all'accesso ai dati in un database. 
     * 
     * Una tabella o una visualizzazione di database viene racchiusa in una classe. 
     * Pertanto, un'istanza oggetto è legata a una singola riga della tabella. 
     * Dopo la creazione di un oggetto, una nuova riga viene aggiunta alla tabella al momento del 
     * salvataggio. 
     * 
     * Qualsiasi oggetto caricato ottiene le proprie informazioni dal database. 
     * 
     * Quando un oggetto viene aggiornato, viene aggiornata anche la riga corrispondente nella tabella. 
     * 
     * La classe wrapper implementa i metodi o le proprietà della funzione di accesso per ogni colonna 
     * della tabella o della visualizzazione.
     * 
     * Questo modello è comunemente usato dagli strumenti di persistenza degli oggetti e nella 
     * mappatura relazionale oggetto (ORM). 
     * 
     * In genere, le relazioni tra chiavi esterne verranno esposte come istanza oggetto del tipo 
     * appropriato tramite una proprietà.
     * 
     * - Implementazioni
     * 
     * Le implementazioni del concetto possono essere trovate in vari framework per molti ambienti 
     * di programmazione. Ad esempio, se in un database è presente una tabella con colonne 
     * (tipo stringa) e (tipo di numero) e il modello Active Record è implementato nella classe , 
     * lo pseudocodice parts name price Part
     * 
     *      part = new Part()
     *      part.name = "Sample part"
     *      part.price = 123.45
     *      part.save()
     *      
     * creerà una nuova riga nella tabella con i valori specificati ed è approssimativamente 
     * equivalente al comando SQL parts.
     * 
     *      INSERT INTO parts (name, price) VALUES ('Sample part', 123.45);
     * 
     * Al contrario, la classe può essere utilizzata per eseguire query sul database:
     * 
     *      b = Part.find_first("name", "gearbox")
     *      
     * Questo troverà un nuovo oggetto basato sulla prima riga corrispondente della tabella 
     * la cui colonna ha il valore "gearbox". 
     * 
     * Il comando SQL utilizzato potrebbe essere simile al seguente, a seconda dei dettagli 
     * di implementazione SQL del database: 
     * 
     *      SELECT * FROM parts WHERE name = 'gearbox' LIMIT 1; -- MySQL or PostgreSQL
     *      
     * Testabilità
     * 
     * A causa dell'accoppiamento dell'interazione del database e della logica dell'applicazione 
     * quando si utilizza il modello di record attivo, l'unit test di un oggetto activerecord 
     * senza un database diventa difficile. 
     * 
     * Questi effetti negativi sulla testabilità del modello di record attivo possono essere 
     * ridotti utilizzando framework di simulazione o di iniezione di dipendenza per sostituire 
     * il livello di dati reale con uno simulato.
     * 
     * Principio della separazione delle responsabilità 
     * 
     * Un'altra critica al modello di active record è che, anche a causa del forte accoppiamento 
     * tra interazione del database e logica dell'applicazione, un oggetto active record non 
     * segue il principio di responsabilità singola e la separazione delle preoccupazioni rispetto 
     * all'architettura a più livelli che affronta correttamente queste pratiche. 
     * 
     * Per questo motivo, il modello di Active Record è il migliore e più spesso impiegato in 
     * applicazioni semplici che sono tutti form-over-data con funzionalità CRUD o solo come 
     * parte di un'architettura. 
     * 
     * In genere quella parte è l'accesso ai dati e perché diversi ORM implementano il modello 
     * di active record.
     * 
     * E' in definitiva un oggetto che esegue il wrapping di una riga in una tabella o 
     * visualizzazione di database, incapsula l'accesso al database e aggiunge logica 
     * di dominio a tali dati.
     * 
     *      // Figura 1 : activeRecordSketch.gif
     * 
     * Un oggetto trasporta sia dati che comportamento. Gran parte di questi dati è persistente 
     * e deve essere archiviata in un database. 
     * 
     * Active Record utilizza l'approccio più ovvio, inserendo la logica di accesso ai dati 
     * nell'oggetto dominio. 
     * 
     * In questo modo tutte le persone sanno come leggere e scrivere i propri dati da e verso 
     * il database.
     * 
     * - Implementazione
     * 
     * Ruby
     * ----
     * Primo tra tutti è famoso per il grande utilizzo che se ne fà in Ruby on Rails, è
     * presente appunto nel framework di questo linguaggio per l'accesso ai dati e viene
     * definitio come ORM a se stante e in relazione al Mapping del Modello come M di MVC.
     * Il progetto in Ruby è cresciuto negli anni e quello che si deve sapere per Ruby
     * di questo modello di progettazione e di come ne abbia fatto un framework a se stante
     * è unna logica di questo linguaggio.
     * Il progetto è consultabile sul loro sito al seguente indirizzo:
     * 
     *      https://guides.rubyonrails.org/active_record_basics.html
     * 
     * Prendiamo dal loro sito i tre punti chiave di come viene definito per loro il modello
     * di progettazione Active Record e come lo hanno implementato.:
     * 
     *      1 Che cos'è Active Record?
     *      Active Record è la M in MVC - il modello - che è il livello del sistema responsabile 
     *      della rappresentazione dei dati e della logica aziendale. 
     *      Active Record facilita la creazione e l'utilizzo di oggetti business i cui dati 
     *      richiedono l'archiviazione permanente in un database. 
     *      È un'implementazione del modello Active Record che a sua volta è una descrizione 
     *      di un sistema di mappatura relazionale degli oggetti.
     *      
     *      1.1 Il modello di Active Record
     *      Active Record è stato descritto da Martin Fowler nel suo libro Patterns of 
     *      Enterprise Application Architecture. In Active Record, gli oggetti trasportano 
     *      sia dati persistenti che comportamenti che operano su tali dati. 
     *      Active Record ritiene che garantire la logica di accesso ai dati come parte 
     *      dell'oggetto istruirà gli utenti di tale oggetto su come scrivere e leggere 
     *      dal database.
     *      
     *      1.2 Object Relational Mapping (ORM)
     *      Object Relational Mapping, comunemente indicato come la sua abbreviazione ORM, 
     *      è una tecnica che collega gli oggetti multimediali di un'applicazione alle 
     *      tabelle in un sistema di gestione di database relazionali. 
     *      Utilizzando ORM, le proprietà e le relazioni degli oggetti in un'applicazione 
     *      possono essere facilmente archiviate e recuperate da un database senza scrivere 
     *      istruzioni SQL direttamente e con meno codice di accesso al database complessivo.
     *      
     *      1.3 Active Record come framework ORM
     *      Active Record ci offre diversi meccanismi, il più importante dei quali è la 
     *      capacità di:
     *          - Rappresentare i modelli e i relativi dati.
     *          - Rappresentano le associazioni tra questi modelli.
     *          - Rappresentare le gerarchie di ereditarietà attraverso modelli correlati.
     *          - Convalidare i modelli prima che vengano resi persistenti nel database.
     *          - Eseguire operazioni di database in modo orientato agli oggetti.
     * 
     *      Etc.
     *      
     *      Nota.: E' importante notare che Active Record in Ruby è Database First e cioè
     *      che la mappatura degli oggetti presenti nel DB parte dal concetto che lo schema
     *      del db è predominante rispetto al codice, e viene usato dal progetto Active Record
     *      per mappare partendo dallo schema del DB i modelli nel codice.
     *      
     * Possiamo quindi concludere che in Rail l'idea di usare il Patter Active Record è in 
     * definitiva una loro soluzione all'accesso ai dati in modo convenzionale a come potrebbe
     * essere inteso in qualsiasi altro ORM e i dettagli implementativi sono rilevanti nel loro
     * progetto e nelle loro specifiche seguite dai programmatori Ruby.
     * 
     * Java
     * ----
     * In Java le cose sono un pò differenti e non è presente di norma un Progetto rilevante
     * e inserito nel contesto del framework per l'accesso ai dati con questo pattern Active Record.
     * Piuttosto in Java e come vedremo in c# i framework per l'Object Relational Mapping sono
     * a scelta dell'utente e possono essere implementati secondo le specifiche dello specifico ORM.
     * Ad esempio in Java è famoso collante per l'accesso e la relazione di mappatura degli oggetti 
     * del database il progetto open source Hybernate che è un vero e proprio ORM.
     * Esiste però la possibilità intrinseca di questo ORM appunto Hybernate di usare delle
     * estensioni per la mappatura in generale e volendo in questo fare un esempio di estensione
     * per il mapping dallo schema del db verso i modelli associati usare ad esempio Quarkus 
     * che offre un'estensione chiamata Panache rendendo le diverse mappature quando si esegue 
     * in un contesto reattivo e con lo stesso modello che usa active record in rails. 
     * Esistono poi altri concetti chiave su come usare l'approccio al pattern Active Record, e
     * se per qualsiasi motivo trovi che il modello Active Record (l'implementazione predefinita 
     * di Ruby On Rails) si adatta meglio alla tua applicazione rispetto al modello Data Mapper 
     * (quello predefinito che segue Hibernate), allora probabilmente potresti usare qualcosa come 
     * JavaLite o ActiveJPA o anche i modelli Spring JDBC (menzionati da @Vivek, ma dovrai prima 
     * implementare il modello ActiveRecord con esso), e quindi usarlo standalone (al di fuori 
     * di GORM) o forse anche implementare la propria implementazione GORM del modello ActiveRecord 
     * utilizzando uno di essi. 
     * 
     * Esiste poi in Java un progetto noto per questo tipo di implementazione Active Record e
     * fortemente ispirato al progetto Ruby che lo incorpora, che è ActiveJDBC, qui è possibile
     * appunto usare questa soluzione senza usare un ORM e adattarlo come se lo fosse anche
     * se non è propriamente da definire tale. Questo Progetto è open source ed è liberamente
     * utilizzabile e distribuito su GitHub al seguente indirizzo.:
     * 
     *      https://github.com/comtel2000/activejdbc
     *      
     * Anche qui possiamo concludere che il Modello di progettazione architettonico per l'accesso
     * e la mappatura dello schema DB è rilevante sia che si usi un ORM proprietario con le 
     * proprie logiche e le estensioni che abbiamo citato, sia che usarlo a se stante.
     * 
     * 
     * .Net
     * ----
     * In c# .Net come per Java non ci sono particolari differenze, anche qui è richiesto per 
     * l'accesso ai dati e il mappig in relazione ad essi sul modello un progetto ORM separato da
     * utilizzare e inserire nel proprio progetto di sviluppo.
     * Particolare attenzione per .Net è presente di norma un ORM conosciuto e molto utilizzato
     * che è stato sviluppato e sponsorizzato da Microsoft indipendente dal Framework ma contestuale
     * e molto pratico che è Entity Framework EF, questo di suo ha tutte le carte in regola per
     * essere antagonista a Hybernate sotto java, ma non è esclusivo nella scelta architetturale.
     * Difatti è possibile usare in .Net anche altri tipi di ORM disponibili come open source e
     * commerciali tra cui citiamo NHybernate che è il clone di Hybernate sotto java ed è molto
     * seguito anche lui e anche altri progetti più lite come Dapper. 
     * E dato che abbiamo citato NHybernate è importante sapere che anche per questo progetto 
     * sotto .net è possibile usare come estensione il progetto Castle per il pattern Active Record
     * che è open source e liberamente utilizzabile, scaricabile al seguente indirizzo.:
     * 
     *      https://github.com/castleproject-deprecated/ActiveRecord
     *     
     * purtroppo non ha avuto il successo e non è più seguito dalla comunità, ma rimane come
     * alternativa e può essere ancora usato. Altrimenti un altro progetto interessante che
     * è più consolidato rispetto ad Active Record di castle è usare Fluent-nhibernate.
     * 
     *      https://github.com/nhibernate/fluent-nhibernate
     *      
     * Non per questo utilizzare un ORM o utilizzare un ORM accompagnato da una delle estensioni
     * che utilizzano il pattern Active Record o utilizzare esclusivamente uno dei moduli 
     * proposti nel web che usano sempre questa metodologia di Active Record, non risolve la
     * maggior parte degli aspetti implementativi di sistemi distribuiti, dove è sempre
     * meglio accostarsi a mio parere ad un buon ORM infatti da molti viene definito come un
     * Anti Pattern.
     * 
     * Entity Framework di suo non ha possibilità di estendere il suo aspetto di mapping sui
     * Modelli come per HYbernate che può usare tramite estensioni per usare un modello Active Record,
     * ma piuttosto parte da due presupposti ormai consolidati per definire il primo punto 
     * del framework che è quello di mappare queste entità sui modelli approcciando con metodologie
     * sia Code First che Db First. Nel primo caso si parte dal codice e viene ricostrutito
     * tramite le migrazioni lo schema sul DB di destinazione nel secondo come per Active Record di
     * Ruby partendo dallo schema del DB per ottenere le classi POCO di uso come modelli per 
     * le entità nel framework. A tal proposito si consiglia sempre usare il primo approccio
     * che è più consono agli sviluppatori, a meno che non si ha già un grosso DB già fatto
     * per farne reverse engineering e ottenere le classi con poco sforzo, ma conviene sempre
     * oi instradarare una volta fatto il primo passo instradare sempre verso la rimodellazione
     * ripartendo dal codice.
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
