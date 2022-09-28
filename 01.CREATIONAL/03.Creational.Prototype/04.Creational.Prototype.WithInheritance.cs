using System;
using static System.Console;

namespace DotNetDesignPatternDemos.Creational.Prototype.Inheritance
{

    // Come nel modello di clonazione precedente tramite Interfaccia
    // di implementazione per il DeepCopy dove sono presenti grafi che
    // usano l'eredità di altri oggetti padre il modello dell'interfaccia
    // dovrebbe differire e per passo usiamo un approccio dove ogni cotruttore
    // identifica la copia e ne ricopia tramite implementazione e chiamata
    // alla base della classe del costruttore relativo.
    // Qui abbiamo la pecca di avere dimenticanze e noiose ripetizioni nei
    // costruttori ereditati.


    public interface IDeepCopyable<T> 
    {
        public T DeepCopy();
    }

    /*
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
    */

    public class Address : IDeepCopyable<Address>
    {
        public string StreetName;
        public int HouseNumber;

        public Address(string streetName, int houseNumber)
        {
            StreetName = streetName;
            HouseNumber = houseNumber;
        }

        internal Address(){}

        public override string ToString()
        {
            return $"{nameof(StreetName)}: {StreetName}, {nameof(HouseNumber)}: {HouseNumber}";
        }

        // Tramite Interfaccia per come nell'esempio precedente.
        public Address DeepCopy()
        {
            return new Address(StreetName, HouseNumber);
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

        // Per ereditatrietà avremo una situazione noiosa come questa
        // che potrebbe ancora risultare confusa e soggetta ad errori.
        public Person DeepCopy()
        {
            return new Person( (string[])Names.Clone(), Address.DeepCopy());
        }

    }

    public class Employee : Person, IDeepCopyable<Employee>
    {
        public int Salary;

        // Approccio per ereditarietà dove i costruttori devono
        // inizializzare sempre la base di implementazione....
        public Employee(string[] names, Address address, int salary)
            : base(names,address)
        {
            Salary = salary;
        }

        public new Employee DeepCopy()
        {
            return new Employee((string[])Names.Clone(), Address.DeepCopy(), Salary);
        }

        public override string ToString()
        {
            return $"{base.ToString()}, {nameof(Salary)}: {Salary}";
        }
    }


    public static class Demo
    {
        static void Main42()
        {
            var john = new Employee(
                    new[] { "John", "Doe" },
                    new Address { HouseNumber = 123, StreetName = "London Road" },
                    321000);

            var copy = john.DeepCopy();

            copy.Names[1] = "Smith";
            copy.Address.HouseNumber++;
            copy.Salary = 123000;

            Console.WriteLine(john);
            Console.WriteLine(copy);
        }
    }
}