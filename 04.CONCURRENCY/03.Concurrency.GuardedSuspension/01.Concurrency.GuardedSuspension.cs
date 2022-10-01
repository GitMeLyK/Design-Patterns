using System;

namespace DotNetDesignPatternDemos.Concurrency.GuardedSuspension
{

    /*
    * In questo esempio il modello Di GuardedSuspension applicato per un implementazione che
    * si basa su GuardedQueue, che ha due metodi: get e put, la condizione 
    * è che non possiamo ottenere dalla coda vuota, quindi quando il thread tenta di 
    * rompere la condizione invochiamo il metodo di attesa di Object su di lui e quando 
    * un altro thread mette un elemento nella coda avvisa quello in attesa che ora 
    * può ottenere dalla coda. 
    * 
    *   ● Questo modello di progettazione utilizza clausole try/catch perché an
    *     InterruptedException può essere generata quando viene invocato wait().
    *     
    *   ● Il metodo wait() viene chiamato nella clausola try se la precondizione non è soddisfatta
    *   
    *   ● Il metodo notification()/notifyAll() viene chiamato per aggiornare uno o tutti gli 
    *     altri thread che è successo qualcosa all'oggetto.
    *     
    *   ● I file notification()/notifyAll() sono solitamente usati per dire agli altri thread che 
    *     il file lo stato dell'oggetto è stato modificato.
    *     
    *     --- ---- --- 
    *     
    *     public void guardedJoy() {
    *       // Simple loop guard. Wastes
    *       // processor time. Don't do this!
    *       while(!joy) {}
    *       System.out.println("Joy has been achieved!"); <---  ***
    *     }
    *     
    *     *** Cosa c'è di sbagliato in questo codice?
    *         Supponiamo, per esempio guardedJoy() è un metodo che non deve procedere fino 
    *         a quando non è condiviso un variabile che è stata impostata da un altro Tread. 
    *         Un tale metodo potrebbe, in teoria, semplicemente scorrere fino a alla condizione 
    *         in cui è soddisfatta. Questo può sprecare molti cicli della CPU!
    *     
    *     --- ---- --- 
    *     
    *     --- ---- --- 
    *     public synchronized guardedJoy() {
    *       while(!joy) {
    *           try {
    *               wait();     <---  ***
    *           } catch (InterruptedException e) {}
    *       }
    *       System.out.println("Joy and efficiency have been achieved!");
    *     }
    *     public synchronized notifyJoy() {
    *       joy = true;
    *       notifyAll();
    *     }
    *     
    *     *** Invece di eseguire il loop su a variabile condivisa per vedere se il presupposto 
    *         della Joy() è vero ed è stato soddisfatto, utilizza per Joy un Thread separato.
    *         L'invocazione wait() blocca il file thread fino a quando non riceve una notifica
    *         risposta. Se la precondizione è la Joy, allora uguale, vero, può continuare
    *         l'esecuzione del codice.
    *   --- ---- --- 
    *   
    *   ● Una protezione più efficiente richiama Object.wait per sospendere il thread corrente.
    *     L'invocazione di wait non ritorna fino a quando un altro thread non ha emesso a 
    *     notifica che potrebbe essersi verificato un evento speciale, anche se no
    *     necessariamente l'evento che questo thread sta aspettando:
    *     
    *   ● Questo blocca il thread, impedendogli di eseguire ulteriormente il codice fino a 
    *     quando non viene eseguito riceve la notifica di procedere.
    *     
    *   Guarded Suspension Pattern: Brief Synopsis wait(), notify(), notifyAll()
    *     
    *   ● Wait, Notify e NotifyAll sono i metodi finali della classe Object.
    *   
    *   ● wait() è una funzione sovraccarica disponibile in tre varianti: 
    *       wait(), wait(long timeout, int nanos), attendere (lungo timeout). 
    *     Ciò consente di specificare per quanto tempo il thread è disposto ad attendere.
    *     
    *   ● Il richiamo di questi metodi fa sì che il thread corrente (chiamato T) si metta 
    *   in uno stato di attesa per questo oggetto e quindi di rinunciare a tutte le richieste 
    *   di sincronizzazione su questo oggetto. Thread T diventa disabilitato per scopi di 
    *   pianificazione dei thread e rimane inattivo fino a una delle quattro cose accade:
    *       
    *       ○ Qualche altro thread richiama il metodo di notifica per questo oggetto e il thread 
    *         T lo è scelto arbitrariamente come Thread da risvegliare.
    *       
    *       ○ Qualche altro thread richiama il metodo notificationAll() per questo oggetto.
    *       
    *       ○ Qualche altro thread interrompe il thread T.
    *       
    *       ○ La quantità specificata di tempo reale è trascorsa, più o meno. Se il timeout 
    *         è zero, invece, quindi il tempo reale non viene preso in considerazione e il thread 
    *         attende semplicemente fino a quando non viene notificato.
    *     
    *  Guarded Suspension Pattern:  notify(), notifyAll()
    *  
    *   ● notify() - Riattiva un singolo thread in attesa del monitor dell'oggetto lock.
    *   
    *   ● Se più di un thread è in attesa del blocco del monitor dell'oggetto, la notifica
    *     il metodo riattiva un thread arbitrario che è in attesa sul monitor dell'oggetto lock.
    *     
    *   ● notificationAll() - Riattiva tutti i thread in attesa del blocco del monitor dell'oggetto
    *  
    *  Note Negative:
    *  
    *  ● Poiché sta bloccando, il modello di sospensione protetto viene generalmente utilizzato 
    *    solo quando lo sviluppatore sa che ha la chiamata al metodo verrà sospesa per un periodo 
    *    finito e ragionevole periodo di tempo. Se il blocco può essere per un periodo di tempo 
    *    indefinito, utilizzare il Balking Design.
    *    
    *  ● Se una chiamata al metodo viene sospesa per troppo tempo, il il programma generale 
    *    rallenterà o si fermerà, in attesa del presupposto per essere soddisfatto.
    *  
    *  ● Se più thread sono in attesa di accedere allo stesso metodo, il file Guarded Design 
    *    Pattern non seleziona quale thread eseguirà il metodo successivo.
    *  
    *  ● La notifica del thread non è sotto il controllo dei programmatori
    *  
    *  ● Per poter scegliere quale thread verrà eseguito successivamente, utilizzare il
    *    Modello di progettazione dello Scheduler invece.
    *    ** Il modello di progettazione dell'utilità di pianificazione appunto Scheduler 
    *       Controlla l'ordine di attesa dei thread può eseguire codice a thread singolo 
    *       in un programma a più thread. 
    *       Gli Svantaggi di questo però sono :
    *           Aggiunge molto sovraccarico durante la sincronizzazione e il metodo può 
    *           essere eseguito.
    *  ● Invoca sempre wait() all'interno di un ciclo che verifica il condizione attesa. 
    *    Non dare per scontato che il l'interruzione era per la particolare condizione in cui eri
    *    in attesa, o che la condizione è ancora vera perché il notifica non è specifico del thread.
    *    
    * Il modello di progettazione Two-Phase può essere utilizzato con il design corrente.
    * 
    */

    /* 
    * Java Code
         
        public class GuardedQueue {
            private static final Logger LOGGER = LoggerFactory.getLogger(GuardedQueue.class);
            private final Queue<Integer> sourceList;

          public GuardedQueue() {
            this.sourceList = new LinkedList<>();
          }

        * @return last element of a queue if queue is not empty
        public synchronized Integer get()
        {
            while (sourceList.isEmpty())
            {
                try
                {
                    LOGGER.info("waiting");
                    wait();
                }
                catch (InterruptedException e)
                {
                    e.printStackTrace();
                }
            }
            LOGGER.info("getting");
            return sourceList.peek();
        }

         * @param e number which we want to put to our queue
        public synchronized void put(Integer e)
        {
            LOGGER.info("putting");
            sourceList.add(e);
            LOGGER.info("notifying");
            notify();
        }
    }
    */

    class Program
    {

        static void Main(string[] args)
        {

        /*
         * CODE Java
        * Example pattern execution
        * @param args - command line args

        GuardedQueue guardedQueue = new GuardedQueue();
        ExecutorService executorService = Executors.newFixedThreadPool(3);

        //here we create first thread which is supposed to get from guardedQueue
        executorService.execute(() -> {
              guardedQueue.get();
            }
        );
   
        try {
          Thread.sleep(2000);
        } catch (InterruptedException e) {
          e.printStackTrace();
        }
        executorService.execute(() -> {
              guardedQueue.put(20);
            }
        );
        executorService.shutdown();
        try {
          executorService.awaitTermination(30, TimeUnit.SECONDS);
        } catch (InterruptedException e) {
          e.printStackTrace();
        }
      }

             * */


            Console.WriteLine("Hello World!");
        }
    
    }
}
