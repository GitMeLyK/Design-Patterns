using System;

namespace DotNetDesignPatternDemos.Concurrency.Join.Barriers
{
    /*
     *  Parallel
    *   Nel calcolo parallelo, una barriera è un tipo di metodo di sincronizzazione. 
    *   Una barriera per un gruppo di thread o processi nel codice sorgente significa 
    *   che qualsiasi thread/processo deve fermarsi a questo punto e non può procedere 
    *   fino a quando tutti gli altri thread / processi non raggiungono questa barriera.
    *
    *   Molte routine collettive e linguaggi paralleli basati su direttive impongono 
    *   barriere implicite. Ad esempio, un ciclo di attività parallelo in Fortran 
    *   con OpenMP non potrà continuare su alcun thread fino al completamento 
    *   dell'ultima iterazione. Questo è nel caso in cui il programma si basi sul 
    *   risultato del ciclo immediatamente dopo il suo completamento. 
    *   Nel passaggio dei messaggi, qualsiasi comunicazione globale 
    *   (come la riduzione o la dispersione) può implicare una barriera.
    *   
    *   Concurrency 
    *   
    *   Nel calcolo concorrente, una barriera può essere in uno stato rialzato o abbassato. 
    *   Il termine fermo è talvolta usato per riferirsi a una barriera che inizia nello stato 
    *   sollevato e non può essere rialzata una volta che è nello stato abbassato. 
    *   Il termine chiavistello per il conto alla rovescia viene talvolta utilizzato 
    *   per riferirsi a un fermo che viene automaticamente abbassato una volta che è 
    *   arrivato un numero predeterminato di thread/processi.
    *
    *   La barriera di base ha principalmente due variabili, una delle quali registra lo 
    *   stato di passaggio/arresto della barriera, l'altra mantiene il numero totale di 
    *   fili che sono entrati nella barriera. 
    *   Lo stato barriera è stato inizializzato per essere "fermato" dai primi fili che 
    *   entrano nella barriera. Ogni volta che un thread entra, in base al numero di thread 
    *   già presenti nella barriera, solo se è l'ultimo, il thread imposta lo stato barriera 
    *   in modo che sia "pass" in modo che tutti i thread possano uscire dalla barriera. 
    *   D'altra parte, quando il filo in entrata non è l'ultimo, è intrappolato nella barriera 
    *   e continua a testare se lo stato barriera è cambiato da "stop" a "pass", 
    *   e esce solo quando lo stato barriera cambia in "pass". 
    *   
    *   Nel codice C riportato di seguito viene illustrata questa procedura
    *   
    *   il potenziale problema è:
    *       A causa di tutti i thread che accedono ripetutamente alla variabile globale 
    *       per pass/stop, il traffico di comunicazione è piuttosto elevato, il che diminuisce 
    *       la scalabilità.
    *   Questo problema può essere risolto raggruppando i thread e utilizzando la barriera 
    *   multilivello, ad esempio combinando la barriera ad albero. 
    *   Anche le implementazioni hardware possono avere il vantaggio di una maggiore scalabilità.
    *   
    *   Guardare al secondo esempio C
    */

    /*
     * Example C simple (Concurrency)
     * 
        class SymmetricBarrier {
        public readonly Synchronous.Channel Arrive;
            public SymmetricBarrier(int n) {
                // create j and init channels (elided)
                var pat = j.When(Arrive);
                for (int i = 1; i < n; i++) pat = pat.And(Arrive);
                pat.Do(() => { });
            }
        }
    */

    /*
     * Example 1 C (parallel)
     * 
    struct barrier_type
    {
        // how many processors have entered the barrier
        // initialize to 0
        int arrive_counter;
        // how many processors have exited the barrier
        // initialize to p
        int leave_counter;
        int flag;
        std::mutex lock;
    };

    // barrier for p processors
    void barrier(barrier_type* b, int p)
    {
        b->lock.lock();
        if (b->arrive_counter == 0)
        {
            b->lock.unlock();
            while (b->leave_counter != p); // wait for all to leave before clearing
            b->lock.lock();
            b->flag = 0; // first arriver clears flag
        }
        b->arrive_counter++;
        if (b->arrive_counter == p) // last arriver sets flag
        {
            b->arrive_counter = 0;
            b->leave_counter = 0;
            b->flag = 1;
        }
        b->lock.unlock();

        while (b->flag == 0); // wait for flag
        b->lock.lock();
        b->leave_counter++;
        b->lock.unlock();
    }
    */

    /*
     * Multililevel (Parallel)
     * Example 2 C
    struct barrier_type
    {
        int counter; // initialize to 0
        int flag; // initialize to 0
        std::mutex lock;
    };

    int local_sense = 0; // private per processor

    // barrier for p processors
    void barrier(barrier_type* b, int p)
    {
        local_sense = 1 - local_sense;
        b->lock.lock();
        b->counter++;
        int arrived = b->counter;
        if (arrived == p) // last arriver sets flag
        {
            b->lock.unlock();
            b->counter = 0;
            // memory fence to ensure that the change to counter
            // is seen before the change to flag
            b->flag = local_sense;
        }
        else
        {
            b->lock.unlock();
            while (b->flag != local_sense); // wait for flag
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
