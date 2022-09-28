using System;
using System.Runtime.Serialization;
using System.Text;
using static System.Console;

namespace DotNetDesignPatternDemos.Structural.Decorator.MultipleInheritanceWithInterfaceWithMemberDefaults
{
    /*
     * Come nell'esempio in precedenza per la multiereditarietà nel nuovo
     * linguaggio di c# dal 9 in poi abbiamo la possibilità intrinseca nelle
     * interfacce di implementare di default dei metodi. Questo ci dà un opportunità
     * in più nel codice di strutturarlo con comportamenti di default laddove
     * come nel caso in corso abbiamo proprietà uguali tra gli oggetti implementati
     * e istanziati allinterno per essere incapsulati ed usati come fossero un unica
     * oggetto.
     */

    /*************************************************/

    public interface ICreature
    {
        int Age { get; set; }
    }

    public interface ILizard : ICreature
    {

        // Usiamo quindi l'implementazione nel corpo
        // dell'interfaccia per eseguire codice di default.
        void Crawl()
        {
            if (Age >= 10)
                WriteLine("Worming");
        }

        int Weight { get; set; }
    }

    public interface IBird : ICreature
    {
        // Usiamo quindi l'implementazione nel corpo
        // dell'interfaccia per eseguire codice di default.
        void Fly()
        {
            if (Age >= 10)
                WriteLine("Fly");
        }

        int Weight { get; set; }

    }

    /*************************************************/

    public class Bird : IBird
    {
        int ICreature.Age { get; set; }

        public int Weight { get; set; }

        public void Fly()
        {
            WriteLine("Fly");
        }
    }


    public class Lizard : ILizard
    {
        int ICreature.Age { get; set; }

        public int Weight { get; set; }

        public void Crawl()
        {
            WriteLine("worming");
        }
    }

    public class Organism{}

    // In qesto caso stiamo puntanndo tutto su un singolo punto
    // e ci rifacciamo al principio di singola responsabilità, non 
    // volendo cambiare il comportamento dei due oggetti da cui 
    // ereditiamo, ma piuttosto incapsularli e adottare i metodi che
    // espongono come interfaccia.
    // Ma in questo caso adottiamo anche che espande il concetto di
    // creatura che ha un metodo a default già nellinterfaccia.
    public class Dragon : Organism, IBird,ILizard,ICreature // : Lizard,Bird -> no multiple inheritance
    {
        private readonly Bird bird = null;
        private readonly Lizard lizard;
        int weight = 0;

        public Dragon() { }
        /*
        {
            this.bird = new Bird();
            this.lizard = new Lizard();
        }
        */

        public Dragon(Bird bird, Lizard lizard)
        {
          //  this.bird = bird ?? throw new ArgumentNullException(paramName: nameof(bird));
          //  this.lizard = lizard ?? throw new ArgumentNullException(paramName: nameof(lizard));
        }

        /*
        public void Fly() {
            this.bird.Fly();
        }

        public void Crawl() {
            this.lizard.Crawl();
        }
        */

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

        public int Age { get; set; }

    }

    static class Program
    {
        static void Main50(string[] args)
        {
            var d = new Dragon { Age = 5 };
            //d.Fly();
            //d.Crawl();

            // Il problema si pone nel Weight dato che è uguale per ttte e due gli oggetti
            // ed essendo stato implementato in modo implicito sarà privato...
            d.Weight = 80;

            // Nel caso del codice esplicito nell'intefaccia da usare a default
            // il costrutto potrebbe essere una condizione del genere anche se non
            // ha molto senso in questo esempio rende l'idea però del possibile uso
            // corretto dell'usare il metodo di default.
            if (d is IBird bird)
                bird.Fly();  // verrà usato il metodo di default

            if (d is ILizard liz)
                liz.Crawl();  // verrà usato il metodo di default

            // In modo esplicito si può chiamare il metodo di default in qesto modo.:
            ((ILizard)d).Crawl(); // perchè ILizard esegue il metodo di default.


        }
    }
}
