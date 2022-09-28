using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using static System.Console;

namespace DotNetDesignPatternDemos.SOLID.SRP
{
    /*
     *  Principio di singola responsabilità. E in questo esempio vediamo come
     *  portare dei contesti secondo questo principio al di fuori di classi che
     *  non se ne devono occupare direttamente.
     *  La classe Journal si occupa di mantenere una collection di elementi stringa
     *  e secondo questo principio non si deve occupare anche della serializzazione
     *  per esermpio come non dovrebbe occuparsi di salvare caricare gli elementi in
     *  memoria. Quindi si separa il contesto dell'oggetto in questione e cioè tutti
     *  i metodi relativi a questo comportamento e si dedica una classe a occuparsi
     *  di fornire questi metodi accessori richiamabili per quel determinato oggetto
     */

    // Class Object concrete
    public class Journal
    {
        private readonly List<string> entries = new ();

        private static int count = 0;

        public int AddEntry(string text)
        {
            entries.Add($"{++count} : {text}");
            return count;// memento pattern!
        }

        public void RemoveEntry(int index)
        {
            entries.RemoveAt(index);
        }

        public override string ToString()
        {
            return string.Join(Environment.NewLine,entries);
        }

        // breaks single responsibility principle
        /* public void Save(string filename)
         {
             File.WriteAllText(filename, ToString);
         }

         public static Journal Load (string filename)
         {
             return null;
         }

         public static Journal Load(Uri uri)
         {
             return null;
         }
        */

    }

    // handles the responsibility of persisting objects
    public class Persistance
    {
        public void SaveToFile(Journal journal, string filename, bool overwrite = false)
        {
            if (overwrite || !File.Exists(filename)){
                File.WriteAllText(filename, journal.ToString());
            }
        }
    }


    class Demo
    {
        static void Main(string[] args)
        {
            var j = new Journal();
            j.AddEntry("Primo Entry");
            j.AddEntry("Secondo Entry");
            WriteLine(j);

            Persistance p = new ();
            string filename = @"c:\temp\journal.txt";
            p.SaveToFile(j, filename, true);
            Process.Start ($"notepad", $"{filename}" );
        }

    }
}
