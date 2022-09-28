using System;
using System.Collections.Generic;

namespace DotNetDesignPatternDemos.Creational.Builder.Functional.WithExtension
{
    public class Person
    {
        public string Name, Position;
    }

    /*
     *  In questa modalità abbiamo un builder con functions 
     *  da asseganre nella configurazione
     *  utilizzando per la composizione le estensioni di classe.
     * */

    public sealed class PersonBuilder
    {
        public readonly List<Action<Person>> Actions
          = new ();

        public PersonBuilder Called(string name)
        {
            Actions.Add(p => { p.Name = name; });
            return this;
        }

        public Person Build()
        {
            var p = new Person();
            Actions.ForEach(a => a(p));
            return p;
        }
    }

    public static class PersonBuilderExtensions
    {
        public static PersonBuilder WorksAsA
          (this PersonBuilder builder, string position)
        {
            builder.Actions.Add(p =>
            {
                p.Position = position;
            });
            return builder;
        }
    }

    public class FunctionalBuilder
    {
        public static void Main41(string[] args)
        {
            var pb = new PersonBuilder();
            var person = pb.Called("Dmitri").WorksAsA("Programmer").Build();
        }
    }
}