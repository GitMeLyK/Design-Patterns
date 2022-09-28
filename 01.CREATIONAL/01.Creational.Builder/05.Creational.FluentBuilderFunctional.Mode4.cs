using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNetDesignPatternDemos.Creational.Builder.Functional.OpenClosedWithExtensionAndGeneralize
{
    public class Person
    {
        public string Name, Position;
    }

    /*
     *  In questa modalità che utilizza il principio di Open Closed
     *  abbiamo un builder con functions da asseganre nella configurazione
     *  utilizzando per la composizione le estensioni di classe.
     *  Ma Generalizzando la classe e adottando Le Generic Ricorsive.
     * */

    public abstract class FunctionalBuilder<TSubject,TSelf>
        where TSelf: FunctionalBuilder<TSubject, TSelf>
        where TSubject: new()
    {
        public readonly List<Func<Person, Person>> Actions
            = new ();

        /*
        public TSelf Called(string name)
            => Do(p => p.Name = name);
        */

        public TSelf Do(Action<Person> action)
            => AddAction(action);
        public Person Build()
            => Actions.Aggregate(new Person(), (p, f) => f(p));

        private TSelf AddAction(Action<Person> action)
        {
            Actions.Add(p => { action(p); return p; });
            return (TSelf)this;
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

    public sealed class PersonBuilder
        : FunctionalBuilder<Person,PersonBuilder>
    {
        public PersonBuilder Called(string name)
            => Do(p => p.Name = name);
    }

    public static class PersonBuilderExtensions
    {
        public static PersonBuilder WorksAsA
          (this PersonBuilder builder, string position)
            => builder.Do(p => p.Position = position);
    }


    public class FunctionalBuilder
    {
        public static void Main61(string[] args)
        {
            var pb = new PersonBuilder()
                .Called("Dmitri")
                .WorksAsA("Developer")
                .Build();
        }
    }
}