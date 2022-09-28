using MvvmHelpers;
using System;
//using System.Activities.Statements;
using System.Threading;
using System.Windows;
using static System.Console;

namespace DotNetDesignPatternDemos.Behavioral.Observer.WeakEventPattern
{
    /*
     * In questo esempio viene trattao il Modello observable con l'ausilio
     * di un particolare pattern il WeakEvent importante quando ci si trova
     * a utilizzare gli eventi associati a interfacce grafiche che fanno molto
     * uso di eventi definiti nei componenti visivi come ad esempio un click
     * su un componente Button di una UI in WPF, ma torna utile anche in altri
     * contesti dove è importante capire come event e tutti quelli che si aggiungono
     * sottoscrivendosi con i loro metodi delegati a event con il += rimangono 
     * ancora attivi anche dopo che l'oggetto ha eseguito il proprio Dispose dopo
     * la chiusura nel caso fi Form nella UI o la dereferenziazione in altri casi,
     * quindi diventa necessario capire ed applicare correttamente il momento prima
     * del Dispose della Classe Observer per liberare queste aree di memoria che 
     * altrimenti continuerebbero a eseguire il resto delle operazioni notificare non previste.
     * Questo esempio tramite la sequenza nel Main fa vedere come il gestore di memoria
     * del Global Collector di .net non ha liberato il gestore e quindi la memoria
     * dopo il Dispose della Classe Form senza WeekEvent e questo è un problema come
     * abbiamo detto perchè si incorre in eccezioni di tipo MemoryLeaks ma soprattutto
     * nei casi di UI come ancora il fatto che gli eventi esistono in memoria non chiudano
     * effettivamente la finestra anche se non visibile rendendola ancora presente nel sistema, 
     * e mostra invece tramite il ManagerWeak come risolvere questo problema in modo intrinseco
     * puntoando al gestore di Handle interno a questa libreria che si occuperà di liberare
     * la memoria e quindi chiamare il Dispose poco prima che il Garbage Collector faccia le sue
     * pulizie del caso. Questo componente in base se .net viene incorporato importando la dll
     * WindowsBase se in .net core utilizziamo invece MvvmHelpers.
      */


    // weak events help with this
    // L'Observed
    public class Button
    {
        public event EventHandler Clicked;

        public void Fire()
        {
            Clicked?.Invoke(this, EventArgs.Empty);
        }
    }

    // L'Observer che usa l'evento per essere notificato
    // ogni qualvolta in una UI viene cliccato un bottone
    // senza l'ausilio del WeakEvent
    public class Window
    {
        public Window(Button button)
        {
            // Il classico modo di sottoscriversi tramite il metodo delegato da aggiungere
            // al gestore degli event della classe target Observed
            button.Clicked += ButtonOnClicked;
        }

        // Il metodo delegato dalla sottoscrizione a ricevere la notifica e fare qualcosa
        private void ButtonOnClicked(object sender, EventArgs eventArgs)
        {
            WriteLine("Button clicked (Window handler)");
        }

        // Il Dispose del Form 
        ~Window()
        {
            WriteLine("Window finalized");
        }
    }

    // L'Observer che usa l'evento per essere notificato
    // ogni qualvolta in una UI viene cliccato un bottone
    // con l'ausilio del WeakEvent
    public class Window2
    {
        public Window2(Button button)
        {
            // Il modo di sottoscriversi tramite il metodo delegato da aggiungere
            // al gestore degli event della classe target Observed ma utilizzando
            // l'Handler sottostante dell'interfaccia WeakEventManager disponibile.

            // Framework 4.1 fino a 4.7
            //WeakEventManager<Button, EventArgs>
            //  .AddHandler(button, "Clicked", ButtonOnClicked);

            // .net core
            WeakEventManager weakEventManager = new WeakEventManager();
            weakEventManager.AddEventHandler<EventArgs>(ButtonOnClicked, "Clicked");

        }



        // Il Metodo delegato a fare qualcosa dopo aver ricevuto le notifiche
        private void ButtonOnClicked(object sender, EventArgs eventArgs)
        {
            WriteLine("Button clicked (Window2 handler)");
        }

        // Il Dispose del form 
        ~Window2()
        {
            WriteLine("Window2 finalized");
        }
    }

    public class Demo
    {
        static void Main401(string[] args)
        {
            // Come si presenta l'esempio nel primo
            // caso senza MangerWeakEvent e nel secondo 
            // poco prima del dispose della classe Form
            var btn = new Button();
            //var window = new Window(btn);
            var window = new Window2(btn);
            var windowRef = new WeakReference(window);
            btn.Fire();

            WriteLine("Setting window to null");
            window = null;

            FireGC();
            WriteLine($"Is window alive after GC? {windowRef.IsAlive}");

            btn.Fire();

            WriteLine("Setting button to null");
            btn = null;

            FireGC();
        }

        private static void FireGC()
        {
            WriteLine("Starting GC");
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            WriteLine("GC is done!");
        }
    }
}