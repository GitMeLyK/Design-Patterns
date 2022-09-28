using System;
using static System.Console;

namespace DotNetDesignPatternDemos.Creational.Prototype.Inheritance.Auto
{

    // Come nel modello di clonazione precedente tramite Interfaccia
    // di implementazione per il DeepCopy dove sono presenti grafi che
    // usano l'eredità di altri oggetti padre il modello dell'interfaccia
    // dovrebbe differire e per passo usiamo un approccio dove ogni cotruttore
    // identifica la copia dei membri in modo quasi automatizzato evitandoci
    // dimenticanze e lunghe e noiose righe di codice per il DeepCopy
    //** In una struttura a grafo molto grande è più conveniente usare questo aproccio
    // che permette copie ben strutturarte nella logica impdedeno inutili ripetizioni ed errori.

    // Si definisce un interfaccia che implementi T e new 
    // per l'automaitsco e il DeepCopy già come corpo ereditato ed implementato per tutti.
    public interface IDeepCopyable<T> where T : new()
    {
        void CopyTo(T target);

        public T DeepCopy()
        {
            T t = new T();
            CopyTo(t);
            return t;
        }
    }

    public class Address : IDeepCopyable<Address>
    {
        public string StreetName;
        public int HouseNumber;

        public Address()
        {

        }

        public Address(string streetName, int houseNumber)
        {
            StreetName = streetName;
            HouseNumber = houseNumber;
        }

        public override string ToString()
        {
            return $"{nameof(StreetName)}: {StreetName}, {nameof(HouseNumber)}: {HouseNumber}";
        }

        // Con il CopyTo per riopiare i sottosttributi da un target
        public void CopyTo(Address target)
        {
            target.StreetName = StreetName;
            target.HouseNumber = HouseNumber;
        }
    }



    public class Person : IDeepCopyable<Person>
    {
        public string[] Names;
        public Address Address;

        public Person()
        {

        }

        public Person(string[] names, Address address)
        {
            Names = names;
            Address = address;
        }

        public override string ToString()
        {
            return $"{nameof(Names)}: {string.Join(",", Names)}, {nameof(Address)}: {Address}";
        }

        // Per le classi che deriveranno da questa viene definito
        // il CopyTo per i suoi attributi di questa classe base
        public virtual void CopyTo(Person target)
        {
            target.Names = (string[])Names.Clone();
            // Il DeepCopy adesso è usato tramite l'estensione per richiamre
            // il corpo del DeepCopy dell'interfaccia implementata (e non farà chiamate ricorsive grazie a questo)
            target.Address = Address.DeepCopy();
        }
    }

    public class Employee : Person, IDeepCopyable<Employee>
    {
        public int Salary;

        // Ereditando da Person ridefiniamo il CopyTo da usare
        // per copiare gli attributi della classe corrente compreso
        // quelli della base Person
        public void CopyTo(Employee target)
        {
            base.CopyTo(target);
            target.Salary = Salary;
        }

        public override string ToString()
        {
            return $"{base.ToString()}, {nameof(Salary)}: {Salary}";
        }
    }

    // Si definisce come estensione per non ripetere l'implmenetazione
    // del DeepCopy
    public static class DeepCopyExtensions
    {
        public static T DeepCopy<T>(this IDeepCopyable<T> item)
          where T : new()
        {
            return item.DeepCopy();
        }

        // PUNTO * questo permette in modo esplicitio per tutti gli oggetti 
        // Person di copiare acnhe il contenuto della classe base altrimenti
        // il clone sarebbe parziale per le altre..
        public static T DeepCopy<T>(this T person)
          where T : Person, new()
        {
            // Importante perchè funzioni il corpo nell'interfaccia
            // ad usare il metodo di default corretto facendo il cast .
            return ((IDeepCopyable<T>)person).DeepCopy();
        }
    }

    public static class Demo
    {
        static void Main()
        {
            var john = new Employee();
            john.Names = new[] { "John", "Doe" };
            john.Address = new Address { HouseNumber = 123, StreetName = "London Road" };
            john.Salary = 321000;
            // PUNTO * Questo usa il secondo riferimento del metoro dell'estensione
            // altrimenti copierebbe parzialmente dalla classe base, ed il metodo
            // essendo dichiarato direttamente where Person permette di copiare 
            // dalla classe derivata compresa quella base
            var copy = john.DeepCopy();

            copy.Names[1] = "Smith";
            copy.Address.HouseNumber++;
            copy.Salary = 123000;

            Console.WriteLine(john);
            Console.WriteLine(copy);
        }
    }
}