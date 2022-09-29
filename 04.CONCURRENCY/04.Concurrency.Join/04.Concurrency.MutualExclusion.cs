using System;

namespace DotNetDesignPatternDemos.Concurrency.Join.Mutual
{
    /*
     * Concurrency
     * In informatica, l'esclusione reciproca è una proprietà del controllo della concorrenza, 
     * che viene istituita allo scopo di prevenire le condizioni di gara. 
     * È il requisito che un thread di esecuzione non entri mai in una sezione critica mentre 
     * un thread di esecuzione simultaneo sta già accedendo a detta sezione critica, 
     * che si riferisce a un intervallo di tempo durante il quale un thread di esecuzione 
     * accede a una risorsa condivisa o memoria condivisa.
     * La risorsa condivisa è un oggetto dati, che due o più thread simultanei stanno tentando 
     * di modificare (in cui sono consentite due operazioni di lettura simultanee, 
     * ma non sono consentite due operazioni di scrittura simultanee o una lettura e una 
     * scrittura, poiché ciò porta a incoerenza dei dati). 
     * L'algoritmo di esclusione reciproca garantisce che se un processo sta già eseguendo 
     * un'operazione di scrittura su un oggetto dati [sezione critica] nessun altro 
     * processo/thread è autorizzato ad accedere/modificare lo stesso oggetto fino a quando 
     * il primo processo non ha terminato la scrittura sull'oggetto dati [sezione critica] 
     * e rilasciato l'oggetto per altri processi su cui leggere e scrivere.
     * 
     * Un semplice esempio del motivo per cui l'esclusione reciproca è importante nella pratica 
     * può essere visualizzato utilizzando un elenco collegato singolarmente di quattro elementi, 
     * in cui il secondo e il terzo devono essere rimossi. 
     * La rimozione di un nodo che si trova tra altri 2 nodi viene eseguita modificando il 
     * puntatore successivo del nodo precedente in modo che punti al nodo successivo 
     * (in altre parole, se nodo io viene rimosso, quindi il puntatore successivo del nodo 
     * i-1 · viene modificato in punta a nodo i+1, rimuovendo così dall'elenco collegato 
     * qualsiasi riferimento al nodo io). 
     * Quando un elenco collegato di questo tipo viene condiviso tra più thread di esecuzione, 
     * due thread di esecuzione possono tentare di rimuovere due nodi diversi contemporaneamente, 
     * un thread di esecuzione che modifica il puntatore successivo del nodo i-1 · 
     * per puntare al nodo i+1, mentre un altro thread di esecuzione cambia il puntatore 
     * successivo del nodo io per puntare al nodo i+2. 
     * Sebbene entrambe le operazioni di rimozione vengano completate correttamente, 
     * lo stato desiderato dell'elenco collegato non viene raggiunto: nodo i+1 rimane 
     * nell'elenco, perché il puntatore successivo del nodo i-1 · punta al nodo i+1.
     * 
     * Questo problema (denominato race condition) può essere evitato utilizzando il 
     * requisito dell'esclusione reciproca per garantire che non si verifichino aggiornamenti 
     * simultanei alla stessa parte dell'elenco.
     * 
     * Il problema che l'esclusione reciproca affronta è un problema di condivisione delle risorse: 
     *  come può un sistema software controllare l'accesso di più processi a una risorsa condivisa, 
     *  quando ogni processo ha bisogno del controllo esclusivo di quella risorsa mentre svolge il 
     *  suo lavoro? 
     *  La soluzione di esclusione reciproca rende disponibile la risorsa condivisa solo mentre il 
     *  processo si trova in un segmento di codice specifico chiamato sezione critica. 
     * Controlla l'accesso alla risorsa condivisa controllando ogni esecuzione reciproca di quella 
     * parte del suo programma in cui verrebbe utilizzata la risorsa.
     * 
     * Una soluzione efficace a questo problema deve avere almeno queste due proprietà:
     * 
     * Deve implementare l'esclusione reciproca: solo un processo può essere nella sezione critica alla volta.
     * Deve essere privo di deadlock: 
     *  se i processi stanno cercando di entrare nella sezione critica, uno di essi deve alla 
     *  fine essere in grado di farlo con successo, a condizione che nessun processo rimanga 
     *  permanentemente nella sezione critica.
    */

    /*
     * Code C
        class Lock {
            public readonly Synchronous.Channel Acquire;
            public readonly Asynchronous.Channel Release;
            public Lock() {
                // create j and init channels (elided)
                j.When(Acquire).And(Release).Do(() => { });
                Release(); // initially free
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
