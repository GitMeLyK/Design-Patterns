using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using static System.Console;


namespace DotNetDesignPatternDemos.Behavioral.State.HandMade
{
    /*
     * Nell'esempio precedente abbiamo visto uma Macchina di Stato completa
     * e fatta nel modo classico, abbiamo visto come può sembrare un pò confusa
     * nel codice ma la logica della separazione degli stati in classi separate
     * e coordinate dai Metodi per definire lo stato contrario al momento dell'invocazione.
     * Questi sono i principi visti nel primo esempio, ma vediamo in questo esempio in
     * una situazione di stato più complessa e con anche gli stati transitivi come gestire
     * in modo più ordinato il codice di questo modello di progettazione a Stati.
     * In questo caso non usiamo ancora librerie esterne, ci sananno un mucchio di stati
     * e transizioni di stato orchestrando il tutto in modo ottimale.
     * In questa simulazione esiste un telefono e i suoi stati, attesa staccato connesso e
     * dei trigger ( transizioni dello stato ) che fondamentalmente sono eventi che si
     * verificano nel sistema e ne monitorano lo stato in esecuzione comportandosi di 
     * conseguenza,come per esempio il telefono è nello stato connected e un altra 
     * chiamata arriva inserisce un nuovo stato di notifica che avvia ad esempio una
     * segreteria per lascaire il messaggio o esempi simili combinati in diverse situazioni
     * con cui il telefono opera. Il risultato è una macchina a stati comlto semplice per
     * il sistema che deve cambiare da uno stato all'altro rispetto alle possibilità offerte
     * dalla scelta di una transizione di stato chiamata trigger per passare allo stato 
     * successivo. L'esempio è senza l'ausilizio di librerie esterne e serve a dare un concetto
     * approssimativo, è naturale che ogni transazione di stato sia accompagnata da un metodo
     * prima di passare allo stato successivo il componente che al di sotto si occuperà di fare
     * le giuste considerazioni con il dispositivo su cui sta operando.
     */

    // I Tipi di Stato
    public enum State
    {
        OffHook,
        Connecting,
        Connected,
        OnHold
    }

    // I Tipi di Stato transitivi
    public enum Trigger
    {
        CallDialed,
        HungUp,
        CallConnected,
        PlacedOnHold,
        TakenOffHold,
        LeftMessage
    }

    class Demo
    {
        // Quindi passeremo da uno stato ad un insieme di possibili
        // stati dipendenti da questi trigger
        private static Dictionary<State, List<(Trigger, State)>> rules
          = new Dictionary<State, List<(Trigger, State)>>
          {
              // Disconnesso se arriva una chiamata si Connette
              [State.OffHook] = new List<(Trigger, State)> {
                (Trigger.CallDialed, State.Connecting)
              },
              // Connettiti se arriva il comando Riattacca imposta Sganciato
              // Connettiti se arriva il comando Connettiti imposta Connesso
              [State.Connecting] = new List<(Trigger, State)>
              {
                (Trigger.HungUp, State.OffHook),
                (Trigger.CallConnected, State.Connected)
              },
              // Connesso se arriva un messaggio Sgancia
              // Connesso se arriva riattacca Sgancia
              // Connesso se arriva metti in attesa setta In Attesa
              [State.Connected] = new List<(Trigger, State)>
              {
                (Trigger.LeftMessage, State.OffHook),
                (Trigger.HungUp, State.OffHook),
                (Trigger.PlacedOnHold, State.OnHold)
              },
              // In Attesa se arriva Togli l'attesa Connetti
              // In Attesa se arriva riattacca Sgancia
              [State.OnHold] = new List<(Trigger, State)>
              {
                (Trigger.TakenOffHold, State.Connected),
                (Trigger.HungUp, State.OffHook)
              }
          };

        static void Main2(string[] args)
        {
            var state = State.OffHook;

            // Orchestriamo tra gli stati, rimanendo in 
            // un ciclo infinito per simulare l'attesa
            // continua di un componente e i suoi stati oprativi.
            while (true)
            {
                WriteLine($"The phone is currently {state}");
                WriteLine("Select a trigger:");

                // foreach to for
                for (var i = 0; i < rules[state].Count; i++)
                {
                    var (t, _) = rules[state][i];
                    WriteLine($"{i}. {t}");
                }

                // Aspetto il cambiamento di stato di transizione che l'utente vuole usare
                int input = int.Parse(Console.ReadLine());

                // Ottengo lo Stato attuale
                var (_, s) = rules[state][input];

                // Cambio lo stato
                state = s;
            };

            /*
             * Il risultato previsto sarà così.:
             * The phone is currently OffHook       (Quindi parte come disconnesso)
             * 0. CallDialed                        (Unico trigger possibile fare una chiamata)
             * 0
             * Th phone i currently Connecting      (Adesso il telefono è in Connessione)
             * 0. HungUp                            (Puoi riagganciare)
             * 1. CallConnected                     (O impostare come Connesso)
             * 1
             * The phone is surrently Connected     (Adesso risulta connesso)
             * 0. LeftMessage                       (Lascia un messaggio)
             * 1. HungUp                            (Riaggancia)
             * 2. PlaceOnHold                       (Metti in Attesa)
             * 2
             * The phone is currently OnHold        (Adesso il telefono è in Attesa)
             * 0. TakenOffHold                      (Togli l'attesa)
             * 1. HungUp                            (Riaggancia)
             * 0
             * The phone is surrently Connected     (Adesso risulta connesso)
             * 0. LeftMessage                       (Lascia un messaggio)
             * 1. HungUp                            (Riaggancia)
             * 2. PlaceOnHold                       (Metti in Attesa)
             * 0
             * The phone is currently OffHook       (Adesso è nuovamente libero e disconnesso)
             * 0. CallDialed                        (E' possibile fare nuovamente una chiamata)
             * ....
            */
        }
    }
}