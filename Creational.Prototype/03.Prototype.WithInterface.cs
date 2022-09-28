using System;
using static System.Console;

namespace DotNetDesignPatternDemos.Creational.Prototype.Interface
{

    // Come nel modello di clonazione precedente ma spcificndo un Interfaccia
    // di implementazione per il DeepCopy il risultato è che viene ben dichiarato
    // che la classe supporta la clonazione dell'intero grafo e non passiamo dai
    // costruttori er questo, ma ancora non è l'ideale su oggetti di grande volume, dove
    // sono presenti molti sottoelementi e grafi lunghi.

    public interface IPrototype<T>
    {
        T DeepCopy();
    }

    public class Address : IPrototype<Address>
    {
        public string StreetAddress, City, Country;

        public Address(string streetAddress, string city, string country)
        {
            StreetAddress = streetAddress ?? throw new ArgumentNullException(paramName: nameof(streetAddress));
            City = city ?? throw new ArgumentNullException(paramName: nameof(city));
            Country = country ?? throw new ArgumentNullException(paramName: nameof(country));
        }

        public Address(Address other)
        {
            StreetAddress = other.StreetAddress;
            City = other.City;
            Country = other.Country;
        }

        public Address DeepCopy()
        {
            return new Address(StreetAddress,City,Country);
        }

        public override string ToString()
        {
            return $"{nameof(StreetAddress)}: {StreetAddress}, {nameof(City)}: {City}, {nameof(Country)}: {Country}";
        }
    }

    public class Employee : IPrototype<Employee>
    {
        public string Name;
        public Address Address;

        public Employee(string name, Address address)
        {
            Name = name ?? throw new ArgumentNullException(paramName: nameof(name));
            Address = address ?? throw new ArgumentNullException(paramName: nameof(address));
        }

        public Employee(Employee other)
        {
            Name = other.Name;
            Address = new Address(other.Address);
        }


        // Implementiamo il metodo ma come si vede ancora dobbiamo
        // in modo noioso ricordarci tutti i sottoelementi.
        public Employee DeepCopy()
        {
            return new Employee(Name, Address.DeepCopy());
        }

        public override string ToString()
        {
            return $"{nameof(Name)}: {Name}, {nameof(Address)}: {Address}";
        }
    }

    public class CopyWithImplementation
    {
        static void Main33(string[] args)
        {
            var john = new Employee("John", new Address("123 London Road", "London", "UK"));

            var chris = john.DeepCopy();

            chris.Name = "Chris";
            WriteLine(john); // oops, john is called chris
            WriteLine(chris);


        }
    }
}
