using System;

namespace DotNetDesignPatternDemos.Concurrency.Join.ReaderWriter
{
    /*
     * Parallel and Concurrency
     * In informatica, un lettore-scrittore (single-writer lock o multi-reader lock o push lock o un MRSW lock) 
     * è una primitiva di sincronizzazione che risolve uno dei problemi lettori-scrittori. 
     * Un blocco RW consente l'accesso simultaneo per le operazioni di sola lettura, 
     * le operazioni di scrittura richiedono l'accesso esclusivo. 
     * Ciò significa che più thread possono leggere i dati in parallelo, ma è necessario un blocco 
     * esclusivo per scrivere o modificare i dati. Quando uno scrittore sta scrivendo i dati, tutti 
     * gli altri scrittori o lettori saranno bloccati fino a quando lo scrittore non avrà finito di 
     * scrivere. Un uso comune potrebbe essere quello di controllare l'accesso a una struttura di dati 
     * in memoria che non può essere aggiornata atomicamente e non è valida (e non deve essere letta 
     * da un altro thread) fino al completamento dell'aggiornamento.
     * I Lock lettori-scrittori sono di solito costruite sopra dei mutex e delle variabili di condizione, 
     * o con uso di semafori sopra.
     * 
     * Alcuni blocchi RW consentono di aggiornare atomicamente il blocco dall'essere bloccato in 
     * modalità di lettura alla modalità di scrittura, oltre a essere declassato dalla modalità 
     * di scrittura alla modalità di lettura. I blocchi RW aggiornabili possono essere difficili 
     * da usare in modo sicuro, poiché ogni volta che due thread che contengono blocchi del lettore
     * tentano entrambi di eseguire l'aggiornamento ai blocchi dello scrittore, viene creato un 
     * deadlock che può essere rotto solo da uno dei thread che rilascia il blocco del lettore.
     * Il deadlock può essere evitato consentendo a un solo thread di acquisire il blocco in 
     * "modalità di lettura con l'intento di aggiornare alla scrittura" mentre non ci sono thread 
     * in modalità di scrittura e possibilmente thread diversi da zero in modalità di lettura.
     * 
     * Criteri di priorità
     * I blocchi RW possono essere progettati con diversi criteri di priorità per l'accesso del lettore 
     * rispetto a quello dello scrittore. Il lock può essere progettato per dare sempre la priorità ai 
     * lettori (preferendo la lettura), per dare sempre la priorità agli scrittori (che preferiscono 
     * la scrittura) o essere non specificato per quanto riguarda la priorità. 
     * Queste politiche portano a diversi compromessi per quanto riguarda la concorrenza.
     * 
     *   * I blocchi RW che preferiscono la lettura consentono la massima concorrenza, 
     *   ma possono portare in difetto le fasi di scrittura se la contesa è elevata.
     *   
     *   * I blocchi RW che preferiscono la scrittura evitano il problema all'uso dello scrittore 
     *   impedendo a qualsiasi nuovo lettore di acquisire il blocco se c'è uno scrittore in coda 
     *   e in attesa del blocco.
     * 
     *   * I  blocchi RW prioritari non specificati non forniscono alcuna garanzia per quanto 
     *   riguarda l'accesso in lettura rispetto all'accesso in scrittura. Una priorità non 
     *   specificata può in alcune situazioni essere preferibile se consente un'implementazione 
     *   più efficiente.
     *   
     *   Implementazione
     *   con Uso di due mutex
     * -----------------------------------
     *   * Inizializzare
     *      *      Impostare b su 0.
     *      *      r è sbloccato.
     *      *      g è sbloccato.
     *   * Inizia a leggere
     *      *      Blocco r.
     *      *       * Incremento b.
     *      *      Se b = 1, bloccare g.
     *      *      Sblocca r.
     *   * Fine lettura
     *      *      Blocco r.
     *      *       * Decremento b.
     *      *      Se b = 0, sbloccare g.
     *      *      Sblocca r.
     *   * Inizia a scrivere
     *      *      Blocco g.
     *   * Termina scrittura
     *      *      Sblocca g.
     * -----------------------------------
     *   
     *   Implementazione
     *   con l'utilizzo di una variabile di condizione e di un mutex
     * -----------------------------------
     *   * Inizia a leggere
     *      *      Blocco g
     *      *       * Mentre num_writers_waiting > 0 o writer_active:
     *      *      aspetta cond, g[a]
     *      *      Incrementa num_readers_active
     *      *      Sblocca g.
     *   * Fine lettura
     *      *       Blocco g
     *      *       Decremento num_readers_active
     *      *       Se num_readers_active = 0:
     *      *        * Notifica cond (broadcast)
     *      *       Sblocca g.
     *   * Inizia a scrivere
     *      *       Blocco g
     *      *       Incrementa num_writers_waiting
     *      *       Mentre num_readers_active > 0 o writer_active è vero:
     *      *        * aspetta cond, g
     *      *       Decremento num_writers_waiting
     *      *       Impostare writer_active su true
     *      *       Sblocca g.
     *   * Termina Scrittura
     *      *       Blocco g
     *      *       Impostare writer_active su false
     *      *       Notifica cond (broadcast)
     *      *       Sblocca g.
    */

    /*
     * Code C
        class ReaderWriterLock {
            private readonly Asynchronous.Channel idle;
            private readonly Asynchronous.Channel<int> shared;
            public readonly Synchronous.Channel AcqR, AcqW, RelR, RelW;
            public ReaderWriterLock() {
                // create j and init channels (elided)
                j.When(AcqR).And(idle).Do(() => shared(1));
                j.When(AcqR).And(shared).Do(n => shared(n+1));
                j.When(RelR).And(shared).Do(n => {
                if (n == 1) idle(); else shared(n-1);
            });
            j.When(AcqW).And(idle).Do(() => { });
            j.When(RelW).Do(() => idle());
            idle(); // initially free
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
