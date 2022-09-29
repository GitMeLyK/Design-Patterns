using System;

namespace DotNetDesignPatternDemos.Concurrency.Join.Mutual
{
    /*
     * Concurrency
     * In informatica, il problema produttore-consumatore (noto anche come problema del buffer limitato) è una famiglia di problemi descritti da Edsger W. Dijkstra dal 1965. Anche nella produzione di beni, nella logistica 
     * o nella gestione della supply chain il problema è noto. Lo stoccaggio intermedio può essere posizionato nel processo di produzione. Con l'accettazione a breve termine delle merci, fungono da cuscinetto tra due stazioni di produzione. 
     * L'archiviazione provvisoria può avere capacità illimitata, ovvero buffer illimitato, oppure capacità 
     * limitata, ovvero buffer limitato. 
     * Se lo stoccaggio intermedio è pieno, la stazione di produzione a monte deve interrompere la produzione. Se lo stoccaggio intermedio è vuoto, la stazione di produzione a valle non ha nulla a che fare.
     * Dijkstra ha trovato la soluzione per il problema produttore-consumatore lavorando come 
     * consulente per i computer Electrologica X1 e X8: "Il primo utilizzo del produttore-consumatore 
     * è stato in parte software, in parte hardware: il componente che si occupava del trasporto delle 
     * informazioni tra negozio e periferica era chiamato 'un canale' ... 
     * La sincronizzazione era controllata da due semafori di conteggio in quello che ora conosciamo 
     * come l'accordo produttore/consumatore: l'unico semaforo che indicava la lunghezza della coda, 
     * veniva incrementato (in una V) dalla CPU e decrementato (in una P) dal canale, l'altro, 
     * contando il numero di completamenti non riconosciuti, veniva incrementato dal canale 
     * e decrementato dalla CPU. 
     * [Il secondo semaforo positivo solleverebbe il flag di interrupt corrispondente.]" [1]
     * 
     * Dijkstra ha scritto sul caso del buffer illimitato: 
     * "Consideriamo due processi, che sono chiamati rispettivamente 'produttore' e 'consumatore'. 
     * Il produttore è un processo ciclico e ogni volta che attraversa il suo ciclo produce una 
     * certa porzione di informazioni, che deve essere elaborata dal consumatore. 
     * Il consumatore è anche un processo ciclico e ogni volta che passa attraverso il suo ciclo, 
     * può elaborare la parte successiva di informazioni, come è stato prodotto 
     * dal produttore ... 
     * Supponiamo che i due processi siano collegati a questo scopo tramite un buffer con 
     * capacità illimitata". 
     * 
     * La soluzione buffer delimitata dal semaforo originale è stata scritta in stile ALGOL. 
     * Il buffer può memorizzare N porzioni o elementi. 
     * Il semaforo "numero di porzioni in coda" conta le posizioni riempite nel buffer, 
     * il semaforo "numero di posizioni vuote" conta le posizioni vuote nel buffer e 
     * la "manipolazione del buffer" del semaforo funziona come mutex per le operazioni 
     * put and get del buffer. 
     * Se il buffer è pieno, ovvero il numero di posizioni vuote è zero, 
     * il thread del produttore attenderà nell'operazione P(numero di posizioni vuote). 
     * Se il buffer è vuoto, ovvero il numero di porzioni in coda è zero, il thread consumer 
     * attenderà nell'operazione P(numero di porzioni in coda). 
     * Le operazioni V() rilasciano i semafori. 
     * Come effetto collaterale, un thread può passare dalla coda di attesa alla coda pronta. 
     * L'operazione P() riduce il valore del semaforo fino a zero. 
     * L'operazione V() aumenta il valore del semaforo.
     * 
     * Per approfondimenti con altre modalità di questo modello tramite Monitor o Semafori
     * e altre possibili soluzioni visitare la wiki a questo modello.
     * 
    */

    /*
     * Code c (example buffer)
         class Buffer<T> {
            public readonly Asynchronous.Channel<T> Put;
            public readonly Synchronous<T>.Channel Get;
            public Buffer() {
                Join j = Join.Create(); // allocate a Join object
                j.Init(out Put);
                // bind its channels
                j.Init(out Get);
                j.When(Get).And(Put).Do // register chord
                (t => { return t; });
            }
        }
    */

    /* Pseudo code
     * example
        begin integer number of queueing portions, number of empty positions,
              buffer manipulation;
              number of queueing portions:= 0;
              number of empty positions:= N;
              buffer manipulation:= 1;
              parbegin
              producer: begin
                      again 1: produce next portion;
                               P(number of empty positions);
                               P(buffer manipulation);
                               add portion to buffer;
                               V(buffer manipulation);
                               V(number of queueing portions); goto again 1 end;
              consumer: begin
                      again 2: P(number of queueing portions);
                               P(buffer manipulation);
                               take portion from buffer;
                               V(buffer manipulation) ;
                               V(number of empty positions);
                               process portion taken; goto again 2 end
              parend
        end
    */

    /* C++
     * con i semafori
        #include <thread>
        #include <mutex>
        #include <semaphore>

        std::counting_semaphore<N> number_of_queueing_portions{0};
        std::counting_semaphore<N> number_of_empty_positions{N};
        std::mutex buffer_manipulation;

        void producer() {
          for(;;) {
            Portion portion = produce_next_portion();
            number_of_empty_positions.acquire();
            {
              std::lock_guard<std::mutex> g(buffer_manipulation);
              add_portion_to_buffer(portion);
            }
            number_of_queueing_portions.release();
          }
        }

        void consumer() {
          for(;;) {
            number_of_queueing_portions.acquire();
            Portion portion;
            {
              std::lock_guard<std::mutex> g(buffer_manipulation);
              portion = take_portion_from_buffer();
            }
            number_of_empty_positions.release();
            process_portion_taken(portion);
          }
        }

        int main() {
          std::thread t1(producer);
          std::thread t2(consumer);
          t1.join();
          t2.join();
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
