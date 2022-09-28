using System;
using static System.Console;

namespace DotNetDesignPatternDemos.Structural.Proxy.Protection
{
    /*
     * In questa tipo di pattern come accennato possono trovarsi diverse 
     * situazioni di implmentazione per un proxy, in questo caso abbiamo
     * un esempio di Protection Proxy, che significa questo in parole povere
     * significa che un set di api possono essere chiamati in una soluzione
     * ma a salvaguardia delle chiamate importanti e protette viene in aiuto
     * questo tipologia particolare di proxy impedendo ai chiamanti di accedere
     * se non con i permessi particolari codificati dalla rpovenienza della chiamata
     * o da una previa autenticazione ad essere eseguiti.
     * Nell'esempio è presente un iterfaccia auto e quello che sta a monte è una 
     * classe proxy che intercetta tutte le chiamate api pubbliche CarProxy che nel
     * suo costruttore definisce un autista che sarà il nostro utente dove per poter
     * guidare questa auto implementata nella classe con il metodo Drive dovrà rispettare 
     * il limite di età preposto a poter eseguire il metodo Drive() del proxy che ricalcherà 
     * nella classe di destinazione vera e propria Car:ICar il metodo che verrà eseguito.
     * 
     */

    public interface ICar
    {
        void Drive();
    }

    public class Car : ICar
    {
        public void Drive()
        {
            WriteLine("Car being driven");
        }
    }

    // Il proxy verso la classe Car
    public class CarProxy : ICar
    {
        private readonly Car car = new ();
        private readonly Driver driver;

        public CarProxy(Driver driver)
        {
            this.driver = driver;
        }

        public void Drive()
        {
            // Il controllo di autorizzazione
            if (driver.Age >= 16)
                car.Drive();
            else
            {
                WriteLine("Driver too young");
            }
        }
    }

    // L'utente autista che esprime i suoi attributi
    public class Driver
    {
        public int Age { get; set; }

        public Driver(int age)
        {
            Age = age;
        }
    }

    public class Demo
    {
        static void Main3(string[] args)
        {
            // Non potrà eseguire il metodo a runtime.
            ICar car = new CarProxy(new Driver(12)); // 22
            car.Drive();
        }
    }
}