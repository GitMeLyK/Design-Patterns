using System;

namespace DotNetDesignPatternDemos.Concurrency.GuardedSuspension
{
    class Program
    {
        /*
         * In questo esempio il modello Di GuardedSuspension applicato per un implementazione che
         * si basa su GuardedQueue, che ha due metodi: get e put, la condizione 
         * è che non possiamo ottenere dalla coda vuota, quindi quando il thread tenta di 
         * rompere la condizione invochiamo il metodo di attesa di Object su di lui e quando 
         * un altro thread mette un elemento nella coda avvisa quello in attesa che ora 
         * può ottenere dalla coda. 
         * 
         * TODO: Trasformare l'esempio da Java in c#
         * 
         */

        /* Java Code
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
