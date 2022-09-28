using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using MoreLinq;
using NUnit.Framework;
using static System.Console;

namespace DotNetDesignPatternDemos.Creational.Singleton
{

    // In questo caso l'esmpio riportato di un accesso al database di tipo testo
    // permette di chiarire che il singleton è utile in quanto l'accesso al file
    // è necessario una sola volta per l'intera esecuzione del programma e non vi
    // è bisogno di caricarlo in memoria ogni volta creando un istanza e rileggendo
    // i dati, ma una volta istanziato la lettura al db avviene la prima volta e le volte 
    // successive in memoria abbiamo il set già pronto.
    // ** Con l'accesso Lazy l'istanza avviene solo al momento in cui è necessario e non all'avvio del programma.

    public interface IDatabase
    {
        int GetPopulation(string name);
    }

    public class SingletonDatabase : IDatabase
    {
        private readonly Dictionary<string, int> capitals;
        private static int instanceCount;
        public static int Count => instanceCount;

        private SingletonDatabase()
        {
            WriteLine("Initializing database");

            capitals = File.ReadAllLines(
              Path.Combine(
                new FileInfo(typeof(IDatabase).Assembly.Location).DirectoryName, "capitals.txt")
              )
              .Batch(2)
              .ToDictionary(
                list => list.ElementAt(0).Trim(),
                list => int.Parse(list.ElementAt(1)));
        }

        public int GetPopulation(string name)
        {
            return capitals[name];
        }

        // laziness + thread safety
        private readonly static Lazy<SingletonDatabase> instance = new (() =>
        {
            instanceCount++;
            return new SingletonDatabase();
        });

        public static IDatabase Instance => instance.Value;
    }

    public class SingletonRecordFinder
    {
        public int TotalPopulation(IEnumerable<string> names)
        {
            int result = 0;
            foreach (var name in names)
                result += SingletonDatabase.Instance.GetPopulation(name);
            return result;
        }
    }

    /// <summary>
    /// IMPORTANT: be sure to turn off shadow copying for unit tests in R#!
    /// Con questo test ci accertiamo che la istanza in singleton dell'accesso
    /// al database sia di fatto univoca e che come si vede viene creta la
    /// prima volta per via del Lazy e le volte successive riutilizzata.
    /// </summary>
    [TestFixture]
    public class SingletonTests
    {
        [Test]
        public void IsSingletonTest()
        {
            var db = SingletonDatabase.Instance;
            var db2 = SingletonDatabase.Instance;
            Assert.That(db, Is.SameAs(db2));
            Assert.That(SingletonDatabase.Count, Is.EqualTo(1));
        }

        [Test]
        public void SingletonTotalPopulationTest()
        {
            // testing on a live database
            var rf = new SingletonRecordFinder();
            var names = new[] { "Seoul", "Mexico City" };
            int tp = rf.TotalPopulation(names);
            Assert.That(tp, Is.EqualTo(17500000 + 17400000));
        }

    }

    public class Demo
    {
        //static void Main()
        //{
        //    var db = SingletonDatabase.Instance;

        //    // works just fine while you're working with a real database.
        //    var city = "Tokyo";
        //    WriteLine($"{city} has population {db.GetPopulation(city)}");

        //    // now some tests
        //}
    }
}
