using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNetDesignPatternDemos.Creational.Builder.Functional
{
    public class Person
    {
        public string Name, Position;
    }

    /*
     *  In questa modalità senza trick strani abbiamo un builder con functions
     *  da asseganre nella configurazione.
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

    public class FunctionalBuilder
    {
        public static void Main31(string[] args)
        {
            var pb = new PersonBuilder();
            var person = pb.Called("Dmitri")
                .Build();
        }
    }
}