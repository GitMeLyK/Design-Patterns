using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNetDesignPatternDemos.Creational.Builder.Functional.OpenClosedWithExtension
{
    public class Person
    {
        public string Name, Position;
    }

    /*
     *  In questa modalità che utilizza il principio di Open Closed
     *  abbiamo un builder con functions da asseganre nella configurazione
     *  utilizzando per la composizione le estensioni di classe.
     * */

    public sealed class PersonBuilder
    {
        public readonly List<Func<Person, Person>> Actions
            = new ();

        public PersonBuilder Called(string name)
            => Do(p => p.Name = name);

        public PersonBuilder Do(Action<Person> action)
            => AddAction(action);
        public Person Build()
            => Actions.Aggregate(new Person(), (p, f) => f(p));

        private PersonBuilder AddAction(Action<Person> action)
        {
            Actions.Add(p => { action(p); return p; });
            return this;
        }

        /*
        public Person Build()
        {
            var p = new Person();
            Actions.ForEach(a => a(p));
            return p;
        }
        */
    }

    public static class PersonBuilderExtensions
    {
        public static PersonBuilder WorksAsA
          (this PersonBuilder builder, string position)
            => builder.Do(p => p.Position = position);
    }


    public class FunctionalBuilder
    {
        public static void Main51(string[] args)
        {
            var pb = new PersonBuilder();
            var person = pb.Called("Dmitri")
                .WorksAsA("Developer")
                .Build();
        }
    }
}