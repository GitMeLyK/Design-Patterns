using System.Collections.Generic;
using System.Linq;
using JetBrains.dotMemoryUnit;
using NUnit.Framework;

namespace DotNetDesignPatternDemos.Structural.Flyweight.Users
{

    using System;
    using static System.Console;

    /*
     * In questo esempio viene riportato una serie di test dove
     * attraverso dell'ottimizzazione delle stringhe che internamente
     * il framework fà possiamo realemente capire l'ottimizzazione
     * di quello che succede al di sotto.
     * Questo dll JetBrains.dotMemoryUnit ci permette di capire quanta memoria viene usata
     * per la memorizzazione delle stringhe in questo caso.
     * Viene ceato uno scenario in cui le stringhe non vengono internate
     * ma verranno solo usate. User
     * Viene ceato uno scenario in cui le stringhe vengono internate
     * e verranno maniplate in uno store prima di essere conservate.
     * 
     * Nel primo caso memorizzando 100 stringhe per il nome e 100 per il cognome non abbiamo
     * una vera e propria ottimizzazione che occuperebbe molto più spazio rispetto al 
     * secondo metodo che ottimizza utilizzando un indice per individuare possibili doppioni
     * in una lista di stringhe conservata internamente.
     * 
     */

    // Scenario uno dove le stringhe che usano nome e cognome non sono manipolate
    public class User // 1655033 spazio occupato
    {
        private readonly string fullName;

        public User(string fullName)
        {
            this.fullName = fullName;
        }
    }

    // Scenario due dove le stringhe che usano nome e cognome sono manipolate
    public class User2 // 1296991  ( meno uso di risorse in memoria )
    {
        static readonly List<string> strings = new ();
        private readonly int[] names;

        public User2(string fullName)
        {
            // In questo caso l'approccio è di usare un indice per i doppioni che
            // la lista di stringhe si trova ad avere...
            int getOrAdd(string s)
            {
                int idx = strings.IndexOf(s);
                if (idx != -1) return idx;
                else
                {
                    strings.Add(s);
                    return strings.Count - 1;
                }
            }

            names = fullName.Split(' ').Select(getOrAdd).ToArray();
        }

        public string FullName => string.Join(" ", names);
    }

    [TestFixture]
    public class Demo
    {
        static void Main8(string[] args)
        {

        }

        // Forza il Global Collector del framework a completare le
        // oprazioni in coda e dopo finalizzato lo riporta a uno stato
        // iniziale pulito per individuare poi quanta memoria è in uso.
        public void ForceGC()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

        public static string RandomString()
        {
            Random rand = new ();
            return new string(
              Enumerable.Range(0, 10).Select(i => (char)('a' + rand.Next(26))).ToArray());
        }

        [Test]
        public void TestUser()
        {
            var users = new List<User>();

            // Generiamo 100 stringhe diverse per il nome e cognome
            var firstNames = Enumerable.Range(0, 100).Select(_ => RandomString());
            var lastNames = Enumerable.Range(0, 100).Select(_ => RandomString());

            // Costruiamo 100 oggetti nominativo di tipo User
            foreach (var firstName in firstNames)
                foreach (var lastName in lastNames)
                    users.Add(new User($"{firstName} {lastName}"));

            ForceGC();

            // e vedaimo in memoria come si comporta il framework
            dotMemory.Check(memory =>
            {
                WriteLine(memory.SizeInBytes);
            });
        }

        [Test]
        public void TestUser2()
        {
            var users = new List<User2>();

            // Generiamo 100 stringhe diverse per il nome e cognome
            var firstNames = Enumerable.Range(0, 100).Select(_ => RandomString());
            var lastNames = Enumerable.Range(0, 100).Select(_ => RandomString());

            // Costruiamo 100 oggetti nominativo di tipo User2
            foreach (var firstName in firstNames)
                foreach (var lastName in lastNames)
                    users.Add(new User2($"{firstName} {lastName}"));

            ForceGC();

            // e vedaimo in memoria come si comporta il framework
            dotMemory.Check(memory =>
            {
                WriteLine(memory.SizeInBytes);
            });
        }
    }
}