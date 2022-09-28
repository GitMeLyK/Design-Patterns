using System;
//using System.Activities.Statements;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetDesignPatternDemos.Behavioral.State.Classic
{
    /*
     * In questo esempio viene implentato un esempio abbastanza conosciuto
     * in questo modello di progettazione State. Quello che viene proposto
     * è una vera e propria macchina a stati finiti dove il componente 
     * principale è una classe astratta chiamata State dove lo stato finito
     * viene usato nei metodi On e Off che devono essere reimplementatio nelle
     * classi discrete dedicate per ogni stato la Prima classe OnState e la seconda
     * OffState che sono usate come Macchine di Stato finito che sono usate dalla
     * classe Target di funzionamento chiamata Switch.
     * Ongi classe usata come macchina di Stato è per definizione una classe che
     * implementa tutte quelle funzioni che girano intorno allo stato da definire
     * per questo tipo di comportamento che agiscono sulla classe target per rimpiazzare
     * il memmbro dedicato allo stato mutandolo nello stato successivo obbligatorio
     * per ricambiare nuovamente lo stato. Come si vede nella macchina di stato il metodo
     * implmentato da due stati On Off sono rispettivamente il nuovo On attesto o il nuovo Off
     * atteso, per dirla Breve la classe Off ha solo un metodo per cambiare il suo stato 
     * chiama On() e la classe di stato On avrà il suo metodo per mutare lo stato Off().
     * Lo stato viene cambiato nello switch da questi metodi per commutare e mutare la
     * classe astratta che detiene tutte e due gli stati virtual non previsti o come si
     * dice che non mutano lo stato se non implmentati.
     */

    // La classe Target che usa gli stati Finiti 
    public class Switch
    {
        // Qui c'è un pliformismo perhcè si inizia con Off
        // ma restuisce uno base State che è ereditato cnahe per On
        // e nel momento delle azioni offerte dallo switch mutano il comportamento successivo.
        public State State = new OffState();

        // Passiamo alla macchina di stato il nostro oggetto
        // che sarà usato per rimpiazzare lo State successivo con un Tipo OnState della
        // classe corrente e quindi chiude così la macchina di stato
        public void On() { State.On(this); }

        // Passiamo alla macchina di stato il nostro oggetto
        // che sarà usato per rimpiazzare lo State successivo con un Tipo OffState della
        // classe corrente e quindi chiude così la macchina di stato
        public void Off() { State.Off(this); }
    }

    // Per le Macchine di Stato come dvono essere implmentate
    public abstract class State
    {
        // Le virtual che identificano lo Stato da reimplementare
        public virtual void On(Switch sw)
        {
            // Se non implementato lo stato sarà definito come non stato
            Console.WriteLine("Light is already on.");
        }

        public virtual void Off(Switch sw)
        {
            // Se non implementato lo stato sarà definito come non stato
            Console.WriteLine("Light is already off.");
        }
    }

    // Macchina di Stao per lo Stato di ON
    public class OnState : State
    {
        public OnState()
        {
            // Se si instanzia per primo lo stato non deve mutare nella classe target Switch
            Console.WriteLine("Light turned on.");
        }

        // Lo Stato successivo da assumere richiamabile nel contesto
        public override void Off(Switch sw)
        {
            Console.WriteLine("Turning light off...");
            // Cambio di stato dove la variabile di Stato nell Switch Muta
            sw.State = new OffState();
        }
    }

    // Macchina di Stao per lo Stato di OFF
    public class OffState : State
    {
        public OffState()
        {
            // Se si instanzia per primo lo stato non deve mutare nella classe target Switch
            Console.WriteLine("Light turned off.");
        }

        // Lo Stato successivo da assumere richiamabile nel contesto
        public override void On(Switch sw)
        {
            Console.WriteLine("Turning light on...");
            // Cambio di stato dove la variabile di Stato nell Switch Muta
            sw.State = new OnState();
        }
    }

    class Program
    {
        public static void Main(string[] args)
        {
            // Il Nostro Switch
            var ls = new Switch();
            
            // Il mutamento dello Stato avviene internamente con le Macchine di Sto per 
            // il momento succesivo che sarà rimpiazzato con la macchina di Stato per Off
            ls.On();

            // Il mutamento dello Stato avviene internamente con le Macchine di Sto per 
            // il momento succesivo che sarà rimpiazzato con la macchina di Stato per On
            ls.Off();

            // Qui provando a richiamare nuovamente lo stato di off
            // il metodo virtual interverrà per segnalare il non stato da cambiare
            // in quanto lo stato successivo previsto di On non implementa il virtual OFF.
            ls.Off();

        }
    }
}
