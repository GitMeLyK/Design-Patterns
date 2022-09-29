using System;

namespace DotNetDesignPatternDemos.Concurrency.Join.Dining
{
    /*
     * Concurrency
     * In informatica, il problema dei filosofi della cena è un problema di esempio spesso 
     * utilizzato nella progettazione di algoritmi concorrenti per illustrare problemi di 
     * sincronizzazione e tecniche per risolverli.
     * È stato originariamente formulato nel 1965 da Edsger Dijkstra come esercizio d'esame 
     * per studenti, presentato in termini di computer in competizione per l'accesso alle 
     * periferiche delle unità nastro. 
     * Poco dopo, Tony Hoare ha dato al problema la sua forma attuale in questo passaggio.:
     * 
     * Cinque filosofi cenano insieme allo stesso tavolo. 
     * Ogni filosofo ha il proprio posto a tavola. 
     * C'è una forchetta tra ogni piastra. 
     * Il piatto servito è una specie di spaghetti che deve essere mangiato con due forchette. 
     * Ogni filosofo può solo alternativamente pensare e mangiare. 
     * Inoltre, un filosofo può mangiare i suoi spaghetti solo quando ha sia una forchetta 
     *  sinistra che una destra. 
     * Quindi due forchette saranno disponibili solo quando i loro due vicini più vicini 
     *  stanno pensando, non mangiando. 
     * Dopo che un singolo filosofo ha finito di mangiare, metterà giù entrambe le forchette. 
     *  Il problema è come progettare un regime (un algoritmo simultaneo) tale che nessun 
     *  filosofo morirà di fame; cioè, ognuno può per sempre continuare ad alternarsi tra 
     *  mangiare e pensare, supponendo che nessun filosofo possa sapere quando gli altri 
     *  potrebbero voler mangiare o pensare (una questione di informazioni incomplete).
     *  
     *  La soluzione di Dijkstra usa un mutex, un semaforo per filosofo e una variabile 
     *  di stato per filosofo. 
     *  Questa soluzione è più complessa della soluzione di gerarchia delle risorse. 
     *  Questa è una versione C++ della soluzione di Dijkstra con le modifiche di Tanenbaum
     *  Esempio in C parte 1
     *  
     *  Soluzione gerarchia delle risorse
     *  Sebbene la soluzione della gerarchia delle risorse eviti i deadlock, non è sempre pratica, 
     *  soprattutto quando l'elenco delle risorse richieste non è completamente noto in anticipo. 
     *  Ad esempio, se un'unità di lavoro contiene le risorse 3 e 5 e quindi determina che ha 
     *  bisogno della risorsa 2, deve rilasciare 5, quindi 3 prima di acquisire 2 e quindi deve 
     *  riacquisire 3 e 5 in tale ordine. I programmi per computer che accedono a un gran numero 
     *  di record di database non funzionerebbero in modo efficiente se fosse loro richiesto di 
     *  rilasciare tutti i record con un numero superiore prima di accedere a un nuovo record, 
     *  rendendo il metodo poco pratico a tale scopo.
     *  
     *  Altre soluzioni sono possibili ma da valutare in base al contesto.:
     *  
     *  Arbitrator solution
     *  Limiting the number of diners in the table
     *  Chandy/Misra solution
     *  
    */

    /*
     *  Code C example simple solver 
     *  
            var j = Join.Create();
            Synchronous.Channel[] hungry;
            Asynchronous.Channel[] chopstick;
            j.Init(out hungry, n); j.Init(out chopstick, n);
            for (int i = 0; i < n; i++) {
                var left = chopstick[i];
                var right = chopstick[(i+1) % n];
                j.When(hungry[i]).And(left).And(right).Do(() => {
                eat(); left(); right(); // replace chopsticks
                });
            }     
     */

    /*
     * Dijkstra's solution (complex)
     * Dijkstra's solution uses one mutex, one semaphore per philosopher and one state variable per philosopher
     * Code C
     * 
        #include <iostream>
        #include <chrono>
        #include <thread>
        #include <mutex>
        #include <semaphore>
        #include <random>

        const int N = 5;          // number of philosophers
        enum {
          THINKING=0,             // philosopher is thinking
          HUNGRY=1,               // philosopher is trying to get forks
          EATING=2,               // philosopher is eating
        };  

        #define LEFT (i+N-1)%N    // number of i's left neighbor
        #define RIGHT (i+1)%N     // number of i's right neighbor

        int state[N];             // array to keep track of everyone's state
        std::mutex mutex_;        // mutual exclusion for critical regions
        std::binary_semaphore s[N]{0, 0, 0, 0, 0}; 
                                  // one semaphore per philosopher
        std::mutex mo;            // for synchronized cout

        int myrand(int min, int max) {
          static std::mt19937 rnd(std::time(nullptr));
          return std::uniform_int_distribution<>(min,max)(rnd);
        }

        void test(int i) {        // i: philosopher number, from 0 to N-1
          if (state[i] == HUNGRY 
           && state[LEFT] != EATING && state[RIGHT] != EATING) {
            state[i] = EATING;
            s[i].release();
          }
        }

        void think(int i) {
          int duration = myrand(400, 800);
          {
            std::lock_guard<std::mutex> gmo(mo);
            std::cout<<i<<" thinks "<<duration<<"ms\n";
          }
          std::this_thread::sleep_for(std::chrono::milliseconds(duration));
        }

        void take_forks(int i) {  // i: philosopher number, from 0 to N-1
          mutex_.lock();          // enter critical region
          state[i] = HUNGRY;      // record fact that philosopher i is hungry
          {
            std::lock_guard<std::mutex> gmo(mo);
            std::cout<<"\t\t"<<i<<" is hungry\n";
          }
          test(i);                // try to acquire 2 forks
          mutex_.unlock();        // exit critical region
          s[i].acquire();         // block if forks were not acquired
        } 

        void eat(int i) {
          int duration = myrand(400, 800);
          {
            std::lock_guard<std::mutex> gmo(mo);
            std::cout<<"\t\t\t\t"<<i<<" eats "<<duration<<"ms\n";
          }
          std::this_thread::sleep_for(std::chrono::milliseconds(duration));
        }

        void put_forks(int i) {   // i: philosopher number, from 0 to N-1
          mutex_.lock();          // enter critical region
          state[i] = THINKING;    // philosopher has finished eating
          test(LEFT);             // see if left neighbor can now eat
          test(RIGHT);            // see if right neighbor can now eat
          mutex_.unlock();        // exit critical region
        }

        void philosopher(int i) { // i: philosopher number, from 0 to N-1
          while (true) {          // repeat forever
            think(i);             // philosopher is thinking
            take_forks(i);        // acquire two forks or block
            eat(i);               // yum-yum, spaghetti
            put_forks(i);         // put both forks back on table
          }
        }

        int main() {
          std::cout<<"dp_14\n";

          std::thread t0([&] {philosopher(0);});
          std::thread t1([&] {philosopher(1);});
          std::thread t2([&] {philosopher(2);});
          std::thread t3([&] {philosopher(3);});
          std::thread t4([&] {philosopher(4);});
          t0.join();
          t1.join();
          t2.join();
          t3.join();
          t4.join();
        }
     */

    /*
     * Resource hierarchy solution
     * C++ 11
            #include <iostream>
            #include <chrono>
            #include <mutex>
            #include <thread>
            #include <random>
            #include <ctime>
            using namespace std;
            int myrand(int min, int max) {
              static mt19937 rnd(time(nullptr));
              return uniform_int_distribution<>(min,max)(rnd);
            }
            void philosophers(int ph, mutex& ma, mutex& mb, mutex& mo) {
              for (;;) {  // prevent thread from termination
                int duration = myrand(200, 800);
                {
                  // Block { } limits scope of lock
                  lock_guard<mutex> gmo(mo);
                  cout<<ph<<" thinks "<<duration<<"ms\n";
                }
                this_thread::sleep_for(chrono::milliseconds(duration));
                {
                  lock_guard<mutex> gmo(mo);
                  cout<<"\t\t"<<ph<<" is hungry\n";
                }
                lock_guard<mutex> gma(ma);
                this_thread::sleep_for(chrono::milliseconds(400));
                lock_guard<mutex> gmb(mb);
                duration = myrand(200, 800);
                {
                  lock_guard<mutex> gmo(mo);
                  cout<<"\t\t\t\t"<<ph<<" eats "<<duration<<"ms\n";
                }
                this_thread::sleep_for(chrono::milliseconds(duration));
              }
            }
            int main() {
              cout<<"dining Philosophers C++11 with Resource hierarchy\n";
              mutex m1, m2, m3;   // 3 forks are 3 mutexes
              mutex mo;           // for proper output
              // 3 philosophers are 3 threads
              thread t1([&] {philosopher(1, m1, m2, mo);});
              thread t2([&] {philosopher(2, m2, m3, mo);});
              thread t3([&] {philosopher(3, m1, m3, mo);});  // Resource hierarchy
              t1.join();  // prevent threads from termination
              t2.join();
              t3.join();
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
