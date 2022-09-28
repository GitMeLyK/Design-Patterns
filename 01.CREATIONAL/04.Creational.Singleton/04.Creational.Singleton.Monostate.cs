using NUnit.Framework;
using System.Collections.Generic;
using static System.Console;

namespace DotNetDesignPatternDemos.Creational.Singleton.Monostate
{
    /*
     * Questo è un esempio di classe statica con attrbuti non statici che internamente
     * hanno proprietà statiche, serve per qundo si vogliono in un Singleton comunque
     * lascire liberi l'utente di usare il costruttore come se non fosse statica la classe
     * ma internamente è senza stato e quini singleton per tutti lo stesso.
     */


    public class ChiefExecutiveOfficer
    {
        private static string name;
        private static int age;

        public string Name
        {
            get => name;
            set => name = value;
        }

        public int Age
        {
            get => age;
            set => age = value;
        }

        public override string ToString()
        {
            return $"{nameof(Name)}: {Name}, {nameof(Age)}: {Age}";
        }
    }

    [TestFixture]
    public class SingletonMonostateTests
    {
        [Test]
        static void TestExampleMonostate()
        {
            ChiefExecutiveOfficer ceo = new ();
            ceo.Name = "Adam Smith";
            ceo.Age = 55;

            var ceo2 = new ChiefExecutiveOfficer();
            WriteLine(ceo2);
        }
    }
}