using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetDesignPatternDemos.Concurrency.MessageDesignPattern
{
    /*
     * Il Message Designa Pattern già descritto come Modello Architetturale, nella
     * modellazione di infrastrtutture basate su cloud è necessario definire che
     * i pattern per la gestione di più Thread concorrenti o in alcuni casi Paralleli
     * seguono la stessa dinamica del broker di messaggi come era stato riportato in 
     * quell'esempio ma tenendo conto dei fattori principi dei modelli concorrenti
     * basati su più accessi contemporanei alla stessa fonte.
     * 
     * In Microsoft e per dotnet questi pattern sono stati ampiamente consolidati
     * nel framework e vengono ad essere definiti pattern di sviluppo per questa
     * metodologia di modelli enteprise di rete.
     * 
     * La natura distribuita delle applicazioni cloud richiede un'infrastruttura di messaggistica 
     * che colleghi i componenti e i servizi, idealmente in modo vagamente accoppiato al fine 
     * di massimizzare la scalabilità. 
     * La messaggistica asincrona è ampiamente utilizzata e offre molti vantaggi, ma comporta 
     * anche sfide come l'ordinamento dei messaggi, la gestione dei messaggi velenosi, 
     * l'idempotenza e altro ancora.
     *
     *  Asynchronous Request-Reply (https://learn.microsoft.com/en-us/azure/architecture/patterns/async-request-reply)
     * 	Disaccoppiare l'elaborazione back-end da un host frontend, in cui l'elaborazione 
     * 	back-end deve essere asincrona, ma il frontend ha ancora bisogno di una risposta chiara.
     * 	
     * 	Claim Check (https://learn.microsoft.com/en-us/azure/architecture/patterns/claim-check)
     * 	Dividi un messaggio di grandi dimensioni in un controllo delle richieste di risarcimento 
     * 	e un payload per evitare di sovraccaricare un bus di messaggi.
     * 	
     * 	Choreography (https://learn.microsoft.com/en-us/azure/architecture/patterns/choreography)
     * 	Chiedi a ciascun componente del sistema di partecipare al processo decisionale sul flusso 
     * 	di lavoro di una transazione aziendale, invece di fare affidamento su un punto centrale di 
     * 	controllo.
     * 	
     * 	Competing Consumers (https://learn.microsoft.com/en-us/azure/architecture/patterns/competing-consumers)
     * 	Consentire a più utenti in modo concorrente di elaborare i messaggi ricevuti sullo stesso canale di 
     * 	messaggistica.
     * 	
     * 	Pipes and Filters (https://learn.microsoft.com/en-us/azure/architecture/patterns/pipes-and-filters)
     * 	Suddividere un'attività che esegue un'elaborazione complessa in una serie di elementi 
     * 	separati che possono essere riutilizzati.
     * 	
     * 	Priority Queue (https://learn.microsoft.com/en-us/azure/architecture/patterns/priority-queue)
     * 	Assegna priorità alle richieste inviate ai servizi in modo che le richieste con una 
     * 	priorità più alta vengano ricevute ed elaborate più rapidamente rispetto a quelle con 
     * 	una priorità inferiore.
     * 	
     * 	Publisher-Subscriber (https://learn.microsoft.com/en-us/azure/architecture/patterns/publisher-subscriber)
     * 	Abilitare un'applicazione per annunciare eventi a più utenti interessati in modo asincrono,
     * 	senza associare i mittenti ai destinatari.
     * 	
     * 	Queue-Based Load Leveling (https://learn.microsoft.com/en-us/azure/architecture/patterns/queue-based-load-leveling)
     * 	Utilizzare una coda che funga da buffer tra un'operazione e un servizio richiamato per 
     * 	attenuare i carichi pesanti intermittenti.
     * 	
     * 	Scheduler Agent Supervisor (https://learn.microsoft.com/en-us/azure/architecture/patterns/scheduler-agent-supervisor)
     * 	Coordinare un set di azioni in un set distribuito di servizi e altre risorse remote.
     * 	
     * 	Sequential Convoy (https://learn.microsoft.com/en-us/azure/architecture/patterns/sequential-convoy)
     * 	Elaborare un insieme di messaggi correlati in un ordine definito, senza bloccare 
     * 	l'elaborazione di altri gruppi di messaggi.
     * 
     *  Tutti questi pattern sono metodi di applcazione e studio per evolvere soluzioni enterprise basate
     *  su cloud per messaggistiche tra componenti di rete e come vengono trattati in modo concorrente
     *  nei backend o parallelamente.
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
