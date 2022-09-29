using System;

namespace DotNetDesignPatternDemos.Concurrency.Lock.DoubleCheck
{
    /*
     * In questo esempio il secondo modello concorrente per avere un accesso condiviso
     * tra più thread viene sviluppato con un tipo di oggetto che è di tipo immutabile.
     * Il modello di bloccaggio ricontrollato verifica il criterio di bloccaggio 
     * prima di acquisire una serratura. In questo modo, possiamo evitare operazioni 
     * non necessarie.
     * Questo modello viene in genere utilizzato nella versione multithread del 
     * modello singleton. Il modello singleton mira a garantire che una classe abbia 
     * una sola istanza, inoltre fornisce un punto di accesso statico globale.
     * 
     *  MS1.png Il diagramma classi seguente mostra la struttura del modello singleton.
     *  
     *  Nel codice riportato di seguito viene illustrato un esempio di singleton a thread singolo. 
     *  Il metodo statico getInstance crea un'istanza solo quando la variabile di istanza è null.
     *  
     *  Nel primo pezzo di codice la classe è senza una gestione di più thread mentre nel secondo
     *  si vede come è trattato l'oggetto per lavorare su più thread.
     *  
     *  Nella seconda implementazione del codice, se l'istanza è stata creata, 
     *  in realtà non è necessario sincronizzare il metodo getInstance poiché possiamo 
     *  restituire la stessa istanza a chiamate simultanee da thread diversi.
     *  
     *  Si supponga che ci siano due thread che chiamano il metodo getInstance nell'esempio 
     *  precedente allo stesso tempo, solo un thread può recuperare l'istanza anche se 
     *  l'istanza è stata creata. Questo non è efficiente.
     *  
     *  Per risolvere questo problema, possiamo creare l'istanza solo quando è necessario. 
     *  Se l'istanza esiste, è sufficiente restituirla.
     *  
     */

    /* Java code
     * Double Check
     
        // Single
        //single thread version
        class Singleton {
          private Singleton instance;

          public static Singleton getInstance() {
            if (instance == null) {
              instance = new Singleton();
            }
            return instance;
          }
        }

        // In un ambiente multithread, se due thread chiamano contemporaneamente 
        // il metodo getInstance, entrambi potrebbero scoprire che l'istanza non 
        // è stata creata. Quindi possono essere create due istanze.
        // Per rendere la classe singleton di cui sopra thread-safe, possiamo usare 
        // la parola chiave "sincronizzata" in Java.

        // Correct
        // Correct but possibly expensive multiple threaded version
        class Singleton {
          private Singleton instance;

          public static synchronized Singleton getInstance() {
            if (instance == null) {
              Instance = new Singleton();
            }
            return instance;
          }
        }

        // La versione corretta stabilisce che deve restituire l'istanza solo
        // quando necessara, questo è il codice completo corretto per un istanza
        // che deve lavorare su più thread in contemporanea.
        // Utilizzato un secondo null selezionato perché un altro thread ha acquisito 
        // il blocco prima che l'istanza potesse essere stata creata.

        // Double checked locking version
        class Singleton {
          private Singleton instance;

          public static Singleton getInstance() {
            if (instance == null) {
              Synchronized(Singleton.class) {
    
                * if another thread acquired the lock before,
                 * the instance may have been created 
                 if (instance == null) {
                  Instance = new Singleton();
                }
              }
            }
            return instance;
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
