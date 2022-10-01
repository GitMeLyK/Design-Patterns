using System;

namespace DotNetDesignPatternDemos.Concurrency.Join.Semaphore
{
    /*
     * Concurrency
     * In informatica, un semaforo è un tipo di dati variabile o astratto utilizzato per 
     * controllare l'accesso a una risorsa comune da più thread ed evitare problemi di sezione 
     * critici in un sistema simultaneo come un sistema operativo multitasking. 
     * I semafori sono un tipo di sincronizzazione primitiva. 
     * Un semaforo banale è una variabile semplice che viene modificata (ad esempio, incrementata 
     * o decrementata o commutata) a seconda delle condizioni definite dal programmatore.
     * 
     * Un modo utile per pensare a un semaforo come usato in un sistema del mondo reale è come 
     * una registrazione di quante unità di una particolare risorsa sono disponibili, insieme 
     * alle operazioni per regolare quel record in modo sicuro (cioè per evitare condizioni di 
     * gara) man mano che le unità vengono acquisite o diventano libere e, se necessario, 
     * attendere fino a quando un'unità della risorsa diventa disponibile.
     * 
     * I semafori sono uno strumento utile nella prevenzione delle condizioni di gara; tuttavia, 
     * il loro utilizzo non è affatto una garanzia che un programma sia libero da questi problemi. 
     * I semafori che consentono un conteggio arbitrario delle risorse sono chiamati semafori di 
     * conteggio, mentre i semafori che sono limitati ai valori 0 e 1 (o bloccati/sbloccati, 
     * non disponibili/disponibili) sono chiamati semafori binari e vengono utilizzati 
     * per implementare i blocchi.
     * 
     * Analogia
     * 
     * Supponiamo che una biblioteca abbia 10 sale di studio identiche, da utilizzare da uno 
     * studente alla volta. Gli studenti devono richiedere una stanza alla reception se desiderano 
     * utilizzare una sala studio. Se nessuna stanza è libera, gli studenti aspettano alla 
     * scrivania fino a quando qualcuno non rinuncia a una stanza. 
     * Quando uno studente ha finito di usare una stanza, lo studente deve tornare alla scrivania 
     * e indicare che una stanza è diventata libera.
     * 
     * Nell'implementazione più semplice, l'addetto alla reception conosce solo il numero di 
     * camere libere disponibili, che conoscono correttamente solo se tutti gli studenti usano 
     * effettivamente la loro stanza mentre si sono iscritti per loro e li restituiscono quando 
     * hanno finito. Quando uno studente richiede una stanza, l'impiegato diminuisce questo numero. 
     * Quando uno studente rilascia una stanza, l'impiegato aumenta questo numero. La stanza può 
     * essere utilizzata per tutto il tempo desiderato, e quindi non è possibile prenotare le 
     * camere in anticipo.
     * 
     * In questo scenario il porta-conteggio della reception rappresenta un semaforo di conteggio, 
     * le stanze sono la risorsa e gli studenti rappresentano processi/thread. 
     * Il valore del semaforo in questo scenario è inizialmente 10, con tutte le stanze vuote. 
     * Quando uno studente richiede una stanza, gli viene concesso l'accesso e il valore del 
     * semaforo viene modificato in 9. 
     * Dopo che arriva lo studente successivo, scende a 8, poi 7 e così via. 
     * Se qualcuno richiede una stanza e il valore corrente del semaforo è 0,è costretto ad 
     * aspettare che una stanza venga liberata (quando il conteggio viene aumentato da 0). 
     * Se una delle stanze è stata rilasciata, ma ci sono diversi studenti in attesa, 
     * è possibile utilizzare qualsiasi metodo per selezionare colui che occuperà la 
     * stanza (come FIFO o sceglierne uno a caso). 
     * E, naturalmente, uno studente deve informare l'impiegato di rilasciare la propria stanza 
     * solo dopo averla davvero lasciata, altrimenti, ci può essere una situazione imbarazzante 
     * quando tale studente è in procinto di lasciare la stanza (stanno imballando i loro libri 
     * di testo, ecc.) e un altro studente entra nella stanza prima di lasciarla.
     * 
     * I semafori di conteggio sono dotati di due operazioni, storicamente indicate come 
     * P e V. L'operazione V incrementa il semaforo S e l'operazione P lo decrementa.
     * 
     * Un modo semplice per comprendere le operazioni di attesa (P) e segnale (V) è:
     *  * wait: decrementa il valore della variabile semaforo di 1. 
     *    Se il nuovo valore della variabile semaforo è negativo, il 
     *    processo di attesa in esecuzione viene bloccato (cioè aggiunto alla coda del semaforo). 
     *    In caso contrario, il processo continua l'esecuzione, dopo aver utilizzato 
     *    un'unità della risorsa.
     *  * signal: incrementa il valore della variabile semaforo di 1. 
     *    Dopo l'incremento, se il valore di pre-incremento era negativo (ovvero ci sono 
     *    processi in attesa di una risorsa), trasferisce un processo bloccato dalla 
     *    coda di attesa del semaforo alla coda pronta.
     *    
    */

    /*
     * code C
     * 
        class Semaphore {
            public readonly Synchronous.Channel Acquire;
            public readonly Asynchronous.Channel Release;
            public Semaphore(int n) {
                // create j and init channels (elided)
                j.When(Acquire).And(Release).Do(() => { });
                for (; n > 0; n--) Release(); // initially n free
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
