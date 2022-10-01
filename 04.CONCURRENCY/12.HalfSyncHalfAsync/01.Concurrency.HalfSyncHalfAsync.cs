using System;

namespace DotNetDesignPatternDemos.Concurrency.HalfSyncHalfAsync
{
    /*
     *  I sistemi simultanei spesso contengono una combinazione di servizi sincroni e asincroni. 
     *  A livello di sistema si preferisce la programmazione asincrona per migliorare le 
     *  prestazioni complessive. Al contrario, uno sviluppatore di applicazioni desidera 
     *  semplificare il proprio sforzo di programmazione e quindi utilizzare l'elaborazione 
     *  sincrona per ridurre la complessità. La necessità di semplicità e prestazioni elevate 
     *  è spesso contraddittoria ma essenziale in molti sistemi concorrenti. 
     *  Ad esempio, si consideri un kernel del sistema operativo che gestisce gli interrupt 
     *  attivati ​​dalle interfacce di rete in modo asincrono, ma i servizi di applicazioni 
     *  di livello superiore utilizzano le chiamate di sistema synchron-ousread() e write(). 
     *  La sfida è comunque rendere questi servizi intercomunicanti.
     *  
     *  Per fornire una tale architettura che consenta ai servizi di elaborazione sincrona e 
     *  asincrona di comunicare tra loro, il modello Half-Sync/Half-Async pattern scompone 
     *  i servizi nel sistema in livelli, aggiungendo un livello di accodamento tra i livelli 
     *  sincrono e asincrono. Sia il livello asincrono che quello sincrono nel modello 
     *  Half-Sync/Half-Async interagiscono tra loro passando messaggi tramite questo livello 
     *  di accodamento. 
     *  
     *  Un esempio reale di un tale livello di queing è il livello socket in molti derivati ​​UNIX, 
     *  che funge da punto di buffering e notifica tra il processo dell'applicazione sincrono e 
     *  i servizi hardware asincroni basati su interrupt nel kernel.
     *  
     *  Il modello Half-Sync/Half-Async ha i seguenti partecipanti all'esempio UNIX sopra menzionato:
     *  
     *  1. Le attività nel livello delle attività sincrone (ad es. processi applicativi) eseguono 
     *     operazioni di I/O di alto livello che trasferiscono i dati in modo sincrono alla coda dei 
     *     messaggi nel livello di Accodamento. 
     *     Le attività in questo livello sono Oggetti Attivi che hanno il proprio thread di controllo
     *     durante l'esecuzione di operazioni di I/O.
     *     
     *  2. Il livello di accodamento (ad es. Sockets) fornisce la sincronizzazione e il buffering 
     *     tra i livelli asincroni e sincroni.
     *     
     *  3. Le attività nel livello delle attività asincrone (ad es. Kernel) gestiscono gli eventi 
     *     dalle origini degli eventi esterni come le interfacce di rete. 
     *     Questi sono Oggetti Passivi che non hanno un proprio thread di controllo.
     *     
     *  L'Half-Sync/Half-Async offre il vantaggio di attività semplificate di livello superiore 
     *  e un unico punto per la comunicazione tra i livelli, il livello di accodamento.
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
