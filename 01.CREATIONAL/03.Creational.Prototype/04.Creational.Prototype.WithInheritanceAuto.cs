using System;
using static System.Console;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace DotNetDesignPatternDemos.Creational.Prototype
{

    // Utilizzando un estensione per la serializzazione e deserializzazione
    // il DeepCopy diventa più facile per alcuni aspetti ma devono essere
    // rispettate condizioni come nel caso binario dove c'è bisogno di 
    // decorare tutte le classi coinvolte con l'attributo Serialize mentre in 
    // formato xml o altri necessita di avere per tutte le classi un costruttore
    // di base pubblico senza argomenti.

    // Si definisce una classse di estenzione che implementi l DeepCopy
    // per la serailizzazione e deserializzazione affinchè si ottengano copie
    // in memoria come in questo caso.
    public static class ExtensionMethods
    {
        public static T DeepCopy<T>(this T self)
        {
            MemoryStream stream = new ();
            BinaryFormatter formatter = new ();
            formatter.Serialize(stream, self);
            stream.Seek(0, SeekOrigin.Begin);
            object copy = formatter.Deserialize(stream);
            stream.Close();
            return (T)copy;
        }

        // Mentre se si vuole serializzare in xml avremo quasto altro metodo di estensione
        // ma necessita di costurttori di base con 0 argomenti per tutte le classi coinvolte.
        public static T DeepCopyXml<T>(this T self)
        {
            using (var ms = new MemoryStream())
            {
                XmlSerializer s = new (typeof(T));
                s.Serialize(ms, self);
                ms.Position = 0;
                return (T)s.Deserialize(ms);
            }
        }
    }

    //[Serializable] // this is, unfortunately, required ( per la serializzazione binaria)
    public class Foo
    {
        public uint Stuff;
        public string Whatever;

        public override string ToString()
        {
            return $"{nameof(Stuff)}: {Stuff}, {nameof(Whatever)}: {Whatever}";
        }
    }

    public static class CopyThroughSerialization
    {
        static void Main52()
        {
            Foo foo = new () { Stuff = 42, Whatever = "abc" };

            //Foo foo2 = foo.DeepCopy(); // crashes without [Serializable]
            Foo foo2 = foo.DeepCopyXml();

            foo2.Whatever = "xyz";
            WriteLine(foo);
            WriteLine(foo2);
        }
    }

}
