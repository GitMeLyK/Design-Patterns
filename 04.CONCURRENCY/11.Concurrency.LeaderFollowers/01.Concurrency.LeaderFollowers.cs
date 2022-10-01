using System;

namespace DotNetDesignPatternDemos.Concurrency.LeaderFollowers
{
    /*
     *  L'obiettivo di TheLeader/Followerspattern è fornire una soluzione per elaborare 
     *  più eventi contemporaneamente, come nelle applicazioni server multi-thread. 
     *  In uno scenario di server tipico, un volume elevato di eventi come CONNECT arriva 
     *  contemporaneamente da più origini di eventi (come più handle di socket TCP/IP). 
     *  Potrebbe non essere possibile (parlando di prestazioni) associare un thread separato 
     *  a ogni singolo handle di socket a causa dell'overhead correlato alla concorrenza 
     *  come il cambio di testo e problemi di sincronizzazione. 
     *  
     *  Il sistema non sarebbe scalabile. Una caratteristica fondamentale del modello 
     *  Leader/Follower è quindi demultiplare le associazioni tra thread e sorgenti di eventi.
     *  
     *  La struttura Leader/Followerspattern è un pool di theads per condividere un insieme 
     *  di sorgenti di eventi in modo efficiente, demultiplexing a turno gli eventi che arrivano 
     *  su queste sorgenti di eventi e inviando in modo sincrono gli eventi ai servizi 
     *  applicativi che li elaborano. 
     *  
     *  Solo un thread nel pool (il leader) può attendere che si verifichi un evento mentre 
     *  gli altri thread (i follower) sono in coda in attesa. 
     *  Quando un thread rileva un evento, prima promuove un follower come nuovo leader e 
     *  quindi assume il ruolo di thread di elaborazione che invia l'evento a un gestore 
     *  di eventi specifico dell'applicazione. 
     *  
     *  E' importante notare come processi multipli disposti in più thread sono concorrenti 
     *  ed uno solo farà da Thread Leader aspettando nuovi eventi. Dopodichè 
     *  i processi ricevono un evento nel proprio handle per il completamento,
     *  può rientrare nel pool di thread e attendere di elaborare un altro evento.
     *  Un thread di elaborazione può diventare immediatamente leader se non è presente alcun thread leader corrente.
     *  In caso contrario, il thread di elaborazione ritorna interrompe la riproduzione dell'effetto 
     *  di un thread follower.
     
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
