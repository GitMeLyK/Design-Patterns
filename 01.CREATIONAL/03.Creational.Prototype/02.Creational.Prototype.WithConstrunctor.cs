using System;
using static System.Console;

namespace DotNetDesignPatternDemos.Creational.Prototype
{

    // Come nel modello di clonazione che si usa in C++ quello 
    // dell'approccio di clonare tramite copia dell'oggetto esistente
    // passato nel costruttore è un attività più logica e consona a
    // fare copia del grafo dell'oggetto con tutti i i suoi sottoelementi Deep Copy.

    public class Address
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

        public override string ToString()
        {
            return $"{nameof(StreetAddress)}: {StreetAddress}, {nameof(City)}: {City}, {nameof(Country)}: {Country}";
        }
    }

    public class Employee
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

        public override string ToString()
        {
            return $"{nameof(Name)}: {Name}, {nameof(Address)}: {Address}";
        }
    }

    public class CopyConstructors
    {
        static void Main24(string[] args)
        {
            var john = new Employee("John", new Address("123 London Road", "London", "UK"));

            //var chris = john;
            Employee chris = new (john);

            chris.Name = "Chris";
            WriteLine(john); // oops, john is called chris
            WriteLine(chris);


        }
    }
}
