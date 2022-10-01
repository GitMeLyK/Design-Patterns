using System;

namespace DotNetDesignPatternDemos.Concurrency.ThreadPool
{
    /*
     * La dimensione di un pool di thread è il numero di thread tenuti in riserva per l'esecuzione 
     * di attività. 
     * Di solito è un parametro sintonizzabile dell'applicazione, regolato per ottimizzare le 
     * prestazioni del programma. 
     * Decidere la dimensione ottimale del pool di thread è fondamentale per ottimizzare le 
     * prestazioni.
     * 
     * Uno dei vantaggi di un pool di thread rispetto alla creazione di un nuovo thread per 
     * ogni attività è che il sovraccarico di creazione e distruzione del thread è limitato 
     * alla creazione iniziale del pool, il che può comportare prestazioni migliori e una 
     * migliore stabilità del sistema. 
     * Creare e distruggere un thread e le risorse associate può essere un processo costoso 
     * in termini di tempo. 
     * Un numero eccessivo di thread in riserva, tuttavia, spreca memoria e il cambio di 
     * contesto tra i thread eseguibili richiede penalizzazioni delle prestazioni. 
     * Una connessione socket a un altro host di rete, che potrebbe richiedere molti cicli 
     * di CPU per cadere e ristabilirsi, può essere mantenuta in modo più efficiente 
     * associandola a un thread che vive nel corso di più di una transazione di rete.
     * 
     * L'utilizzo di un pool di thread può essere utile anche mettendo da parte il tempo 
     * di avvio del thread. 
     * Esistono implementazioni di pool di thread che rendono banale mettere in coda il lavoro, 
     * controllare la concorrenza e sincronizzare i thread a un livello superiore rispetto 
     * a quello che può essere fatto facilmente quando si gestiscono manualmente i thread. [4][5] 
     * In questi casi i benefici prestazionali dell'uso possono essere secondari.
     * 
     * In genere, un pool di thread viene eseguito in un singolo computer. 
     * Tuttavia, i pool di thread sono concettualmente correlati alle server farm in cui 
     * un processo master, che potrebbe essere un pool di thread stesso, distribuisce le 
     * attività ai processi di lavoro in computer diversi, al fine di aumentare la velocità 
     * effettiva complessiva. Problemi paralleli imbarazzanti sono altamente suscettibili 
     * a questo approccio. 
     * 
     * Il numero di thread può essere regolato dinamicamente durante il ciclo di vita 
     * di un'applicazione in base al numero di attività in attesa. 
     * Ad esempio, un server Web può aggiungere thread se arrivano numerose richieste 
     * di pagine Web e può rimuovere i thread quando tali richieste si assottigliano. 
     * Il costo di avere un pool di thread più grande è un maggiore utilizzo delle risorse. 
     * L'algoritmo utilizzato per determinare quando creare o distruggere i thread influisce 
     * sulle prestazioni complessive:
     *  La creazione di troppi thread consente di sprecare risorse e costa tempo alla creazione 
     *  dei thread inutilizzati.
     *  Distruggere troppi thread richiede più tempo in seguito quando li si crea di nuovo.
     *  La creazione di thread troppo lentamente potrebbe comportare prestazioni client scadenti 
     *  (lunghi tempi di attesa).
     *  Distruggere i thread troppo lentamente può affamare altri processi di risorse. 
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
