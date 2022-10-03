using System;

namespace DotNetDesignPatternDemos.Architectural.MasterSlave
{
    /*
     * Il modello Master-Slave viene utilizzato anche per interfacce utente e server. 
     * In entrambi i casi il master ascolta i comandi provenienti dall'utente o dai client. 
     * Quando viene ricevuto un comando, viene avviato uno slave per eseguire il comando 
     * mentre il master riprende l'ascolto di altri comandi (ad esempio il comando 
     * "sospendi l'ultimo comando").
     * 
     * In questo esempio viene perfezionato il modello Master-Slave con il modello Factory.
     * 
     *      // Figura 1
     *      
     * Il modello architettonico master-slave viene utilizzato per migliorare l'affidabilità 
     * e le prestazioni del sistema dividendo il lavoro tra i componenti master e slave. 
     * Ogni componente ha responsabilità distinte. Tutti i componenti slave hanno un lavoro 
     * identico o almeno simile e tale lavoro deve essere definito prima del runtime. 
     * Questo modello non è un approccio divide et impera all'architettura; piuttosto, 
     * è quello in cui il lavoro degli slave è predefinito e deve essere coordinato. 
     * L'obiettivo del modello architettonico master-slave è migliorare l'efficienza del software.
     * 
     * Questo modello è un modo per svolgere lavori di grandi dimensioni. Al contrario, 
     * il modello client-server si concentra su più utenti e richieste. 
     * Il modello master-slave fornisce un esempio di delega del lavoro all'interno di un sistema.
     * 
     * Le richieste arrivano nel master. Quindi, il padrone divide il lavoro in pezzi 
     * che vengono coltivati agli schiavi. Quando uno slave completa il lavoro, i risultati 
     * vengono inviati al master. Il master mette quindi insieme i risultati e fornisce 
     * il risultato per la richiesta. Il lavoro svolto può essere computazionale, 
     * richieste di terze parti o incrociare più motori di persistenza.
     * 
     * Condivisione del carico
     * 
     * Il punto di forza di questo modello è la possibilità di condividere una richiesta 
     * tra le risorse. Tuttavia, si noti che questo non è gratuito. Viene svolto ulteriore 
     * lavoro per suddividere l'attività e unire i risultati. Questo da solo può essere 
     * ad alta intensità di risorse in base al lavoro. Pertanto, dovremmo usare questo 
     * modello solo con lavori di grandi dimensioni che hanno modi ben definiti per dividerli.
     * 
     * Un esempio è l'elaborazione di un insieme di dati. Ogni articolo all'interno della 
     * raccolta può essere spedito a un processo slave e quindi i risultati restituiti. 
     * Pertanto, il lavoro è suddiviso per elemento e possiamo distribuire il lavoro tra 
     * gli slave disponibili. Allo stesso modo, ci sono tipi di lavoro all'interno di 
     * una richiesta che creano buone linee per dividerla. Ad esempio, potrebbe essere 
     * necessaria la manipolazione dei dati, il recupero dell'archiviazione e i calcoli 
     * da eseguire. Ancora una volta, questi pezzi di lavoro possono essere inviati agli slave 
     * che sono più adatti per ogni tipo di lavoro.
     * 
     * Modelli diversi e obiettivi diversi
     * Potresti pensare che abbiamo descritto un modello a strati. Questo, ancora una 
     * volta, è un focus diverso su come la richiesta scorre attraverso il sistema. 
     * Questo modello è una singola richiesta di grandi dimensioni. Il modello a più 
     * livelli gestisce più richieste e si concentra sui passaggi necessari per 
     * qualsiasi richiesta, senza suddividere grandi pezzi di lavoro.
     * 
     * Sfide
     * Il modello master-slave non è adatto a molte attività. Ci saranno problemi a 
     * suddividere lo sforzo in pezzi più piccoli, oppure la richiesta potrebbe essere 
     * abbastanza piccola da non dover essere divisa. Ricorda che abbiamo un 
     * sovraccarico nel dividere il lavoro e unire i risultati. Tale costo può aumentare 
     * esponenzialmente il tempo e le risorse per un'attività se non eseguita correttamente.
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
