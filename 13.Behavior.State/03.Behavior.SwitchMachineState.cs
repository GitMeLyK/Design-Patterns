using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetDesignPatternDemos.Behavioral.State.SwitchBasedStateMachine
{
    /*
     * Nell'esempio precedente abbiamo visto uma Macchina di Stato completa
     * ma con l'uso di transizioni di stato per passare secondo l'azione ad
     * uno stato dell'oggetto che diviene lo stato successivo.
     * Per maggiore ovvietà riportiamo un esempio ancora più semplice di una
     * possibile macchina di stato che deinfisce tre tipi di stato e nel sistema
     * si avvia l'iterazione con l'utente a scegliere lo stato e come avviene
     * il cambiamento dello stato del componente una volta effettuata la scelta,
     * cioè come sarà ronto il componente stesso ad accettare lo stato successivo
     * o a cadere in uno stato di Failure possibile dovuto a una scelta non prevista.
     * Si simula in questo caso senza transizioni di stato un componente Lucchetto
     * che partendo da una combinazione segreta deve essere aperto, lo stato cambia
     * rispetto alla combinazione se valida e definisce lo sblocco del lucchetto che
     * apre il lucchetto e ritorna, o se la combinazione non è quella prevista si 
     * mette in Bloccato passando dallo Stato Failure allo stato Locked.
     */

    // I Tipi di Stato
    enum State
    {
        Locked,
        Failed,
        Unlocked
    }

    // Il sistema in azione
    public class SwitchBasedDemo
    {
        static void Main3(string[] args)
        {
            // La combinazione del lucchetto prevista
            string code = "1234";

            // Si parte da uno stato chiuso
            var state = State.Locked;
            var entry = new StringBuilder();

            // L'iterazione con il componente partendo da uno stato di Locked
            while (true)
            {
                switch (state)
                {
                    case State.Locked:

                        // Attende una cobinazione valida prevista 1234
                        entry.Append(Console.ReadKey().KeyChar);

                        if (entry.ToString() == code)
                        {
                            state = State.Unlocked;
                            break;
                        }

                        // La combinazione non è valida
                        if (!code.StartsWith(entry.ToString()))
                        {
                            // Un modo errato di passare allo stato successivo
                            // forzato è quello di usare un comando goto in questo modo.:
                            // goto State.Failed;

                            // Piuttosto si definisce lo stato corrente e si aspetta
                            // che lo swith salti alla condizione di Failure che farà
                            // le sue cose e riporterà il lucchetto nello stato di Locked.
                            state = State.Failed;
                        }
                        break;

                    case State.Failed:
                        Console.CursorLeft = 0;
                        Console.WriteLine("FAILED");
                        entry.Clear();
                        state = State.Locked;
                        break;
                    
                    case State.Unlocked:
                        // Riceve lo stato di Unlocked combinazione valida stato Sbloccato
                        Console.CursorLeft = 0;
                        Console.WriteLine("UNLOCKED");
                        return;
                }
            }
        }
    }
}
