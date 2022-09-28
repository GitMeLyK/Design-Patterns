using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Stateless;
using static System.Console;

namespace DotNetDesignPatternDemos.Behavioral.State.StateMachineStateless
{
    /*
     * Negli esempi precedenti abbiamo usato il modello di progettazione a Stati con l'uso
     * di pattern conosciuti per la definizione del codice come deve essere strutturato con
     * classi che identificano lo Stato e un Componente che adotta questi stati per i momenti
     * successivi, e tutti gli esempi sono stati fatti senza particolari librerie già presenti
     * nelle community di sviluppo per usare meglio queste problematiche e sollevarci da diversi
     * problemi di implementazione su casi d'uso abbastanza complessi.
     * In questo caso e in questo esempio vedremo come usare una di queste librerie e come si
     * usa per usare questo modello.
     * Lo Stateless 4.0, che è una dll nota per questo aspetto della programmazione usa agevolare
     * la costruzione di Macchine di stato anche gerarchiche complesse con un approccio diverso al metodo che
     * abbiamo usato fino adesso. Questa dll offre molti modi in cui è possibile costruire una
     * macchina a stati finiti. In questo esempio viene preso lo stato della persona se ha le caratteristiche
     * per essere un individuo in salute per avere figli o è in uno stato che è già incinta e non può avere altri figli
     * al momento o che il suo stato di salute impedisce di avere figli.
     * I trigger in questo caso sono le azioni che può usare la persona per poter avere figli. Nell'esempio
     * quello che vogliamo vederee sono le possibili combinazioni che questa dll ci dà come ipotesi di condizioni
     * valide perchè uno stato precedente sia soggetto a condizioni o espressione per individuare
     * se ci sono le condizioni per accettare lo stato corrente da mutare e impostare quali sono le
     * possibili richieste successive per cambiare lo stato impostato come corrente.
     * La dll risolve un gran numero di possibilità per creare macchine di stato e richiede uno studio
     * approfondito a parte per vedere tutte le possibilità offerte.
     * In questo esempio usiamo solo sue di questi pattern tramite i metodi Permit e PermitIf ma ce ne sono
     * diversi e per i più disparati casi.
     */

    // I Tipi di Stato
    public enum Health
    {
        NonReproductive,
        Pregnant,
        Reproductive
    }

    // Trigger di stati transitivi
    public enum Activity
    {
        GiveBirth,          // Partorire
        ReachPuberty,       // Raggiungere la pubertà
        HaveAbortion,       // Abortire
        HaveUnprotectedSex, // Fare sesso non protetto
        Historectomy        // Isterectomia
    }

    class Demo
    {
        static void Main(string[] args)
        {

            // La macchina di stato quindi si configura per avere le condizioni
            // Salute e Attività da fare per dedurre cosa sarà lo stato o gli stati
            // da mutare successivamente. Si istanzia con uno stato di fatto iniziale
            // ad essere una persona che non è in salute al momento per fare figli
            // (Perchè ad esempio ancora non ha raggiunto la Pubertà)
            var stateMachine = new StateMachine<Health, Activity>(Health.NonReproductive);

            // Qui per lo stato NonReproductive è possibile avere
            // in successione un attività ReachPuberty per poter
            // cambiare lo stato in Reproductive
            stateMachine.Configure(Health.NonReproductive)
              .Permit(Activity.ReachPuberty, Health.Reproductive);

            // Qui per lo stato Reproductive è possibile avere
            // in successione un attività Historectomy per poter
            // cambiare lo stato in NonReproductive o se l'attività
            // HaveUnprotectedSex per essere Pregnant ha ParentsNotWatching su true
            stateMachine.Configure(Health.Reproductive)
              .Permit(Activity.Historectomy, Health.NonReproductive)
              .PermitIf(Activity.HaveUnprotectedSex, Health.Pregnant,
                () => ParentsNotWatching);

            // Qui per lo stato Pregnant è possibile avere
            // in successione un attività GiveBirthper poter
            // cambiare lo stato in Reproductive o per l'attività
            // HaveAbortion per poter cambiare lo stato in Reproductive
            stateMachine.Configure(Health.Pregnant)
              .Permit(Activity.GiveBirth, Health.Reproductive)
              .Permit(Activity.HaveAbortion, Health.Reproductive);

        }

        // Controllo condizionale per una mutazione di stato in base all'azione.
        // In questo caso ritorna un stato true se non ci sono parenti in giro
        // e la condizione è valida di avere un rapporto non protetto per passare
        // lo stato di salute in Incinta :) 
        public static bool ParentsNotWatching
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }
    }
}