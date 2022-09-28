using System;
using System.Runtime.Serialization;
using System.Text;
using static System.Console;

namespace DotNetDesignPatternDemos.Structural.Decorator.MultipleInheritanceWithInterface
{
    /*
     * In questo esempio ci comportiamo per il decoratore di fornire 
     * una classe che si occupa di emulare quello che in c++ è più 
     * facile ottenere la Multi ereditarietà, questo speciale pattern
     * che torna utile in molti casi purtroopo non è possibile usarlo
     * per c# o java, ma utilizzando un decorator è possibile ovviare
     * a questo tipo di casistiche che internament non faranno altro
     * che adottare più interfacce per emulare la multiereditarietà.
     * Un problema comune si ha quando tutte le clssi da incapsulare
     * nel decoratore hanno in sè delle proprietà uguali a quelle dell'altro
     * la cosa allora diventa un pò più complicata
     */

    /*************************************************/

    public interface ILizard
    {
        void Crawl();
        int Weight { get; set; }
    }

    public interface IBird
    {
        void Fly();
        int Weight { get; set; }

    }

    /*************************************************/

    public class Bird : IBird
    {
        public int Weight { get; set; }

        public void Fly()
        {
            WriteLine("Fly");
        }
    }


    public class Lizard : ILizard
    {
        public int Weight { get; set; }

        public void Crawl()
        {
            WriteLine("worming");
        }
    }

    // In qesto caso stiamo puntanndo tutto su un singolo punto
    // e ci rifacciamo al principio di singola responsabilità, non 
    // volendo cambiare il comportamento dei due oggetti da cui 
    // ereditiamo, ma piuttosto incapsularli e adottare i metodi che
    // espongono come interfaccia.
    public class Dragon : IBird,ILizard // : Lizard,Bird -> no multiple inheritance
    {
        private readonly Bird bird;
        private readonly Lizard lizard;
        int weight = 0;

        public Dragon()
        {
            this.bird = new Bird();
            this.lizard = new Lizard();
        }

        public Dragon(Bird bird, Lizard lizard)
        {
            this.bird = bird ?? throw new ArgumentNullException(paramName: nameof(bird));
            this.lizard = lizard ?? throw new ArgumentNullException(paramName: nameof(lizard));
        }

        public void Fly() {
            this.bird.Fly();
        }

        public void Crawl() {
            this.lizard.Crawl();
        }

        // Ma come si vedrà essendo che le due interfacce comportano per una
        // stessa proprietà il Weight in questo caso che c'è l'hanno tutti e
        // due il problema di ambiguità... dobbiamo usare i membri espliciti
        // public void Crawl() { lizard.Crawl();}
        // public void Fly(){ bird.Fly(); }
        int IBird.Weight { get => this.bird.Weight; set => this.bird.Weight=value; }
        int ILizard.Weight { get => this.lizard.Weight; set => this.lizard.Weight=value; }
    
        // Quindi per ovviare al fattore in comune per tutte e due gli elementi
        // una strada possibile è avere un atrbituo con lo stesso nome che si comporti
        // in base al tipo 
        public int Weight { get { return weight; } set { weight = value;
                this.bird.Weight = value;       // In questo modo su possibili cast dell'oggetto
                this.lizard.Weight = value;     // riflettiamo il valore assegnato dalla classe multiereditata.
            } }

    }

    static class Program
    {
        static void Main40(string[] args)
        {
            var d = new Dragon();
            d.Fly();
            d.Crawl();
            // Il problema si pone nel Weight dato che è uguale per ttte e due gli oggetti
            // ed essendo stato implementato in modo implicito sarà privato...
            d.Weight = 80;
        }
    }
}
