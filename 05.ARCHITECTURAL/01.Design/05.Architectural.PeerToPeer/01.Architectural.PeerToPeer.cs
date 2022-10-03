using System;

namespace DotNetDesignPatternDemos.Architectural.PeerToPeer
{
    /*
     * Il modello di architettura software peer-to-peer. È comune tra le reti di condivisione 
     * file e crittografiche. 
     * Allo stesso modo, il modello non è troppo complicato. 
     * Tuttavia, è robusto e sicuro in quanto abbiamo percorsi di comunicazione strettamente definiti. 
     * Abbiamo anche ogni nodo che svolge il ruolo di client e server.
     * 
     * Il modello peer-to-peer definito
     * La descrizione del peer-to-peer è ancora meno complessa del client-server. 
     * Abbiamo due nodi con un collegamento bidirezionale tra di loro. 
     * Ogni nodo può fungere da client o da server. Pertanto, lo vediamo anche nei momenti 
     * in cui vogliamo mantenere sincronizzate due macchine per scopi di bilanciamento del 
     * carico e failover. 
     * La rete può espandersi a qualsiasi numero di nodi. 
     * Tuttavia, ogni nodo si connette a un altro sia come client che come server, quindi 
     * la comunicazione diretta è sempre disponibile.
     * 
     * Sulla stessa linea d'onda
     * Mentre ci sono molti vantaggi per una rete peer-to-peer, l'obiettivo principale è 
     * mantenere ogni nodo aggiornato con i dati. Tali dati possono essere un file, un elenco 
     * di transazioni o persino elenchi di autorizzazioni. Ogni dato nodo può interagire con 
     * numerosi altri nodi, ma tali interazioni andranno sempre in entrambe le direzioni. 
     * Pertanto, un aggiornamento sul nodo A può essere trasmesso ai nodi da B a Z, e quindi 
     * un aggiornamento su Z può uscire da A a Y.
     * 
     * Sfide
     * L'obiettivo del peer-to-peer è mantenere le cose sincronizzate. 
     * Ciò significa che vogliamo aggiornare gli altri nodi quando abbiamo una richiesta 
     * di modifica. 
     * Il lavoro necessario per eseguire tali aggiornamenti può inondare la rete e deve 
     * essere gestito. 
     * Un vecchio esempio sono i primi giochi di rete come Doom che inviavano troppi 
     * aggiornamenti alla rete. 
     * È possibile arrestare in modo anomalo la rete mentre i timeout dilagavano attraverso 
     * i nodi e hanno cercato di inviare nuovamente tali dati. 
     * Ci sono modelli per fare tali notifiche che fanno quesgto tipo di lavoro.
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
