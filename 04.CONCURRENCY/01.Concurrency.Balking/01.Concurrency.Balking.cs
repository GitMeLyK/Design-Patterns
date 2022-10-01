using System;

namespace DotNetDesignPatternDemos.Concurrency.Balking
{
    /*
     * ● Questo modello di progettazione software viene utilizzato per richiamare un'azione
     *   su un oggetto solo quando l'oggetto si trova in uno stato particolare.
     * ● Gli oggetti che utilizzano questo modello sono generalmente solo in a
     *   stato che è incline a esitare temporaneamente ma per an
     *   quantità di tempo sconosciuta.
     *   
     *   Nel codice di esempio nel metodo job c'è il return se la variabile di istanza booleana
     *   jobInProgress è impostato su false, quindi il job() ritornerà senza averlo eseguito 
     *   qualsiasi comando e quindi mantenendo l'oggetto dichiara lo stesso.
     *   Se la variabile jobInProgress è impostata su true, allora l'oggetto Esempio è in 
     *   lo stato corretto da eseguire codice aggiuntivo nel lavoro()
     *  
     *   In genere, un modello di balking viene utilizzato con un modello di esecuzione a 
     *   thread singolo per aiutare a coordinare il cambiamento di stato di un oggetto.
     *   
     *   Cos'è lo Schema di Single Thread Execution?
     *   
     *   ● Questo modello di progettazione descrive una soluzione per la concorrenza quando 
     *     è multipla e i lettori e o più scrittori accedono a un'unica risorsa.
     *   ● I problemi più comuni in questa situazione sono stati gli aggiornamenti persi e
     *     letture incoerenti.
     *   ● In sostanza, l'esecuzione a thread singolo è un modello di progettazione di concorrenza
     *     che attua una forma di mutabilità condivisa, che è ancora molto facile da rovinare.
     *     
     *  Note negative:
     *  
     *  ● È considerato un anti-pattern, quindi non è un vero design pattern
     *  
     *  ● Poiché, il modello balking viene in genere utilizzato quando lo stato di un 
     *    oggetto potrebbe essere incline a esitare per un periodo di tempo indefinito, 
     *    allora non lo è consigliato da usare quando lo stato di un oggetto è incline a 
     *    esitare per a tempo relativo noto.
     *    
     *  ● Il modello di Guarded Suspension è una buona alternativa quando si tratta di 
     *    un oggetto lo stato è incline a esitare per un periodo di tempo finito noto.
     *  
     */


    /*
     * Code Java

    public class Example {
        private boolean jobInProgress = false;
     
        public void job() {
            synchronized(this) {
                if (jobInProgress) {
                    return;
                }
            jobInProgress = true;
        }
     
        // Code to execute job goes here
        // ...
        }

     void jobCompleted() {
        synchronized(this) {
            jobInProgress = false;
        }
     }

    }

    */

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
