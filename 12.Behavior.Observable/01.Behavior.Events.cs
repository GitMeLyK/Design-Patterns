using System;

namespace DotNetDesignPatternDemos.Behavioral.Observer
{
    /*
     * In questo esempio viene trattao il Modello observable con l'ausilizio
     * della keywork event che fa parte del linguaggio di sviluppo c#.
     * In questa tipologia classica ormai in questo linguaggio questo modo di
     * essere notificati attravenrso la keyword event è di semplice intuizione.
     * Abbiamo una classe che definiamo Observer che fà cose e ha in sè una 
     * dichiarazione di event pubblica, questa è usata in una o più punti del sistema
     * da parte di oggetti che vogliono essere notificati sul cambiamento di stato
     * di questo oggetto tramite la consueta sintassi come in questo caso person.FallsIll += CallDoctor;
     * e questo è possibile, essendo una lista interna del framework che ne tiene traccia di tutte
     * le sottoscrizioni, definirlo nel programma in tutti i punti desiderati e tutti gli oggetti
     * che puntao a questo evento di notifica riceveranno in sequenza nel proprio metodo delegato 
     * lo stato della modifica o del cambiamento.
     * Per far si che tutti vengano avvisati dallo stato di cambiamento e modifica dell'evento la classe
     * Observer in questo caso Person nei punti strategici dove servirà notificare tutti chiamerà
     * l'handle del gestore tramite la chiamata .Invoke(..)
     */

    // La classe per gli EventArgs da passare nella notifica di 
    // evento valorizzandola con le proprietà che servono al sottoscrittore
    // per leggere i cambiamenti avvenuti per quel determinato evento notificato.
    public class FallsIllEventArgs
    {
        public string Address;
    }

    // Observable
    public class Person
    {
        // Il metodo che quando viene eseguito per fare operazioni
        // di cambiamento di proprietà o altre cose notifica con l'invoke
        // tutti quelli che si sono sottoscritti al delegato event
        public void CatchACold()
        {
            FallsIll?.Invoke(this,
              new FallsIllEventArgs { Address = "123 London Road" });
        }

        // Proprietà di event pubblica per i sottoscrittori
        public event EventHandler<FallsIllEventArgs> FallsIll;
    }

    // Observing
    public class Demo
    {
        static void Main400()
        {
            var person = new Person();

            // Sottoscrizione tramite il metodo delegato alla proprietà event della classe observed
            person.FallsIll += CallDoctor;

            // Chiama il metodo per fare qualcosa che Innescherà l'evento per
            // notificare il metodo sottoscritto precedentemente a gestire 
            // l'informazione di cambiamento di stato.
            person.CatchACold();
        }

        // Il metodo delegato a ricevere la notifica di event sottoscritta
        private static void CallDoctor(object sender, FallsIllEventArgs eventArgs)
        {
            Console.WriteLine($"A doctor has been called to {eventArgs.Address}");
        }
    }
}