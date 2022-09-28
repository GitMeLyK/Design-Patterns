using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetDesignPatternDemos.Behavioral.State.ExpressionBasedStateMachine
{
    /*
     * Nell'esempio precedente abbiamo visto uma Macchina di Stato completa
     * un pò goffa avendola fata girare in un ciclo infinito, ecco invece qui
     * con un buon esempio di espressioni possiamo switchare da uno stato all'altro
     * dove l'azione di Aprire o Chiudere il lucchetto fornito di chiave e dello stato
     * precedente presente nel componente valuterà la condizione ideale per lo sblocco
     * o il blocco o il fallimento. Valuterà per ogni condizione di stato attuale e
     * della presenza di una chiave o meno di apertura l'ipotesi dell'azione da fare.
     * Per esempio se io mi trovavo in uno Stato Closed (che è lo stato di partenza del baule e cioè chiuso senza chiave)
     * ed eseguo un azione di apertura Open e quindi valuto un espressione dove fornisco che Ho la chiave (bool = true)
     * che lo stato attuale del componente è solo Closed e cioè chiuso senza chiave la condizione risulta valida per portare lo stato
     * del componente nuovamente come baule aperto Open. Se riprovo a passare ad un altra azione come ad esempio chiudere il baule Close e passo
     * come argomento all'espressione questa volta ho la chiave (bool=true) e lo stato corrente che era Open
     * la condizione porterà lo stato del componente in Locked (baule chiuso con la chiave) quindi Locked diventa
     * per la prossima azione valida solo una azione di Open con chiave a true altrimenti fallisce l'azione.
     * Il rovescio della medaglia per questo modello trattandolo in questo esempio è che non esiste una
     * definizione formale di una macchina a stati disponibile per l'introspezione, il che significa che non
     * esiste una struttura dati che definisca chiaramente quali sono gli stati e quali sono le transizioni associate
     * e quali sono le condizioni di guardia che possa ispezionare esternamente un insieme di casi e quindi
     * fare una deduzione chiara e disegnata intorno al problema in modo che sia subito comprensibile.
     * D'altro canto la sintassi adottata è molto ben strutturata e chiara usando il pattern di matching con
     * gli switch e se hai macchine a stato molto semplici che non richiedono cose come la documnetazione e qunat'altro
     * allora puoi usare le espressioni da c# 8 in poi e rendere il tuo codice molto più chiaro e intuibile.
     */

    // I Tipi di Stato
    enum Chest
    {
        Open,
        Closed,
        Locked
    }

    // Le action 
    enum Action
    {
        Open,
        Close
    }

    // Lo switch che orchestra la macchina di stato
    public class SwitchExpressions
    {
        // La prima espressione per il cambiamento di stato
        // Metodo Switch con sintassi di espansione
        static Chest Manipulate(Chest chest,
          Action action, bool haveKey) =>
          (chest, action, haveKey) switch
          {
              // Una possibile condizione dove lo stato precedente
              // Era closed ma si sta tentando di aprire per il baule
              // con o senza la chiave porta il baule comunque nello stato in Open
              // Il trattino basso è di valore arbitrario True or False
              (Chest.Closed, Action.Open, _) => Chest.Open,
              // Una possibile condizione dove lo stato precedente
              // Era locked quindi chiuso con la chiave e si sta tentando 
              // di riaprirlo con la chiave porta lo stato in Open
              (Chest.Locked, Action.Open, true) => Chest.Open,
              // Una possibile condizione dove lo stato precedente
              // Era con il baule aperto Open e si sta per chiudere 
              // con la chiave porta lo stato in Locked
              (Chest.Open, Action.Close, true) => Chest.Locked,
              // Una possibile condizione dove lo stato precedente
              // del baule era aperto Open ma si sta provando a chiuderlo 
              // senza la chiave porta lo stato è solo chiuso non bloccato in Closed
              (Chest.Open, Action.Close, false) => Chest.Closed,

              // Se lo stato corrente è richiamato nuovamente non si fa nulla
              _ => chest
          };

        // La seconda espressione per il cambiamento di stato
        // Metodo Ordinario
        static Chest Manipulate2(Chest chest,
          Action action, bool haveKey)
        {
            switch (chest, action, haveKey)
            {
                // Una possibile condizione dove lo stato precedente
                // Era closed ma si sta tentando di aprire per il baule
                // con o senza la chiave porta il baule comunque nello stato in Open
                // Il trattino basso è di valore arbitrario True or False
                case (Chest.Closed, Action.Open, _):
                    return Chest.Open;
                // Una possibile condizione dove lo stato precedente
                // Era locked quindi chiuso con la chiave e si sta tentando 
                // di riaprirlo con la chiave porta lo stato in Open
                case (Chest.Locked, Action.Open, true):
                    return Chest.Open;
                // Una possibile condizione dove lo stato precedente
                // Era con il baule aperto Open e si sta per chiudere 
                // con la chiave porta lo stato in Locked
                case (Chest.Open, Action.Close, true):
                    return Chest.Locked;
                // Una possibile condizione dove lo stato precedente
                // del baule era aperto Open ma si sta provando a chiuderlo 
                // senza la chiave porta lo stato è solo chiuso non bloccato in Closed
                case (Chest.Open, Action.Close, false):
                    return Chest.Closed;
                // Se lo stato corrente è richiamato nuovamente segnaliamo
                default:
                    Console.WriteLine("Chest unchanged");
                    return chest;
            }
        }

        // Il sistema in azione
        public static void Main4(string[] args)
        {
            Chest chest = Chest.Locked;
            Console.WriteLine($"Chest is {chest}");

            // unlock with key
            chest = Manipulate(chest, Action.Open, true);
            Console.WriteLine($"Chest is now {chest}");

            // close it!
            chest = Manipulate(chest, Action.Close, false);
            Console.WriteLine($"Chest is now {chest}");

            // close it again!
            chest = Manipulate(chest, Action.Close, false);
            Console.WriteLine($"Chest is now {chest}");
        }
    }
}