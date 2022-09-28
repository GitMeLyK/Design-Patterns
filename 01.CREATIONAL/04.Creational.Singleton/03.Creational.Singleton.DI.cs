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

namespace DotNetDesignPatternDemos.Creational.Singleton.DI
{

    // In questo caso l'esmpio riportato di un accesso al database di tipo variabile
    // nel senso che non facciamo per l'istanza in sigleto di usare un tipo di database
    // ma dal singolo punto di ingresso del programma tramite Dipendence Injiection ci
    // occupiamo di configurare e registrare il tipo di database più consono e iniettarlo
    // laddove serve nell'accesso e consumer dei dati. 
    // Nell'erogare l'instaza tramite Dipiendence Injection tramite il Builder container
    // avremo la possibilià di dichiararlo appunto di tipo Singleton senza occuparci di aggiungere
    // metodi di istanza interne o altro come abbiamo fatto nell'esempio precedente.

    public interface IDatabase
    {
        int GetPopulation(string name);
    }

    public class SingletonDatabase : IDatabase
    {
        private readonly Dictionary<string, int> capitals;
        private static int instanceCount;  // 0
        public static int Count => instanceCount;

        private SingletonDatabase()
        {
            instanceCount++;
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

    // Un altro tipo di database per lo scopo di questo
    // esempio dove verrà usato per operare nel contesto
    // tramite DI dove serve, come si vede nel corpo non
    // c'è nessun riferiento al database Singleton come nella 
    // classe precedente di un db singleton, ma attraverso
    // la registrazione nel DI useremo il container che si occuperà
    // di iniettare nell'istanza di questa classe il Datasebase di tipo IDatabase.
    public class OrdinaryDatabase: IDatabase
    {

        private readonly Dictionary<string, int> capitals;

        private OrdinaryDatabase()
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

    }

    // Separate concern to finder with interrogation to singleton
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

    public class ConfigurableRecordFinder
    {
        private readonly IDatabase database;

        public ConfigurableRecordFinder(IDatabase database)
        {
            //this.database = database ?? throw new ArgumentNullException(paramName: nameof(database), "Error instance");
            this.database = database;
        }

        public int GetTotalPopulation(IEnumerable<string> names)
        {

            int result = 0;
            foreach (var name in names)
                result += database.GetPopulation(name);
            return result;
        }
    }

    // Implementazione fittizia di un db diverso
    public class DummyDatabase : IDatabase
    {
        public int GetPopulation(string name)
        {
            return new Dictionary<string, int>
            {
                ["alpha"] = 1,
                ["beta"] = 2,
                ["gamma"] = 3
            }[name];
        }
    }

    /// <summary>
    /// IMPORTANT: be sure to turn off shadow copying for unit tests in R#!
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

        [Test]
        public void DependantTotalPopulationTest()
        {
            var db = new DummyDatabase();
            var rf = new ConfigurableRecordFinder(db);
            Assert.That(
              rf.GetTotalPopulation(new[] { "alpha", "gamma" }),
              Is.EqualTo(4));
        }

        // stesso di quello sopra diversamente impostato...
        [Test]
        public void ConfigurablePopulationTest()
        {
            var rf = new ConfigurableRecordFinder(new DummyDatabase());
            var names = new[] { "alpfha", "gamma" };
            int tp = rf.GetTotalPopulation(names);
            Assert.That(tp, Is.EqualTo(4));
        }

        // In questo caso usiamo invece la Dipendence Injection per
        // fare un test separando il componente dalla sua vera natura.
        // Con l'Inversion Of Control non facciamo altro che utilizzare
        // un istanza a nostro piacimento di un determinato componente
        // in questo caso un DB Ordinario e registrarlo nel container
        // che farà funzione di istanziarlo per quella interfaccia a cui
        // abbiamo registrato.
        [Test]
        public void DIPopulationTest()
        {
            var CB = new ContainerBuilder();

            // Tramite Autofac registriamo che il Tipo Ordinario
            // di db viene usato come modello di intefaccia IDatabase
            // nelle occorrenze dove serve e identifichiamo che il Database
            // ordinario installato è tale che esiste una singola istanza
            // del database ordinario per tutte le attività del programma.
            CB.RegisterType<OrdinaryDatabase>()
                .As<IDatabase>()
                .SingleInstance(); // E qui esponsiamo l'istanaza come Singleton

            // Stessa cosa per il Concetto separato di classe che si occupa 
            // dei metodi di ricerca ma avendo un corpo con un costruttore che si aspetta
            // un qualsiasi database iniettabile che ha come interfaccia IDatabase
            CB.RegisterType<ConfigurableRecordFinder>();

            // Ecco quindi che da questo Single Point possiamo
            // sceglie di consumare e utilizzare il db a nostro piacimento
            using (var c = CB.Build())
            {
                // Quindi risolviamo per il Finder separato
                var rf = c.Resolve<ConfigurableRecordFinder>();
                var names = new[] { "alpfha", "gamma" };
                rf.GetTotalPopulation(names );
                Assert.That(rf, Is.EqualTo(4));

            }

        }


        // Test di verifica per l'uso del bulder con le classi sotto questo codice
        [Test]
        public void DIExampleBuild()
        {
            var db = SingletonDatabase.Instance;

            // works just fine while you're working with a real database.
            var city = "Tokyo";
            WriteLine($"{city} has population {db.GetPopulation(city)}");


            // Un esempio per capire il DI tramite IOC per un istanza Singleton di una classe
            var builder = new ContainerBuilder();
            builder.RegisterType<EventBroker>().SingleInstance();
            builder.RegisterType<Foo>();

            using (var c = builder.Build())
            {
                var foo1 = c.Resolve<Foo>();
                var foo2 = c.Resolve<Foo>();

                WriteLine(ReferenceEquals(foo1, foo2));
                WriteLine(ReferenceEquals(foo1.Broker, foo2.Broker));
            }

        }

    }

    // Un altro esempio di Classe per capire il DI
    public class Foo
    {
        public EventBroker Broker;

        public Foo(EventBroker broker)
        {
            Broker = broker ?? throw new ArgumentNullException(paramName: nameof(broker));
        }
    }

    public class EventBroker
    {

    }



}
