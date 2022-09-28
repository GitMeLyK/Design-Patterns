using System;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetDesignPatternDemos.Creational.Singleton.PerThread
{
    /*
     * In questo caso utilizziamo una Singola Istanza per Thread piuttosto che
     * per l'intera applicazione, questo permette per un Thread di riutilizzare
     * la stessa istanza e per un altro Thread avere una propria istanza.
     * ** Questi sono casi particolari dove il Singleton ha una gestione diversa
     *    per ogni istanza del suo scopo di utilizzo.
     *    /
     */


    public sealed class PerThreadSingleton
    {
        private readonly static ThreadLocal<PerThreadSingleton> threadInstance
          = new (
            () => new PerThreadSingleton());

        public int Id;

        private PerThreadSingleton()
        {
            Id = Thread.CurrentThread.ManagedThreadId;
        }

        public static PerThreadSingleton Instance => threadInstance.Value;
    }

    public class Demo
    {
        public static void Main15(string[] args)
        {
            var t1 = Task.Factory.StartNew(() =>
            {
                Console.WriteLine($"t1: " + PerThreadSingleton.Instance.Id);
                Console.ReadKey();
            });
            var t2 = Task.Factory.StartNew(() =>
            {
                Console.WriteLine($"t2: " + PerThreadSingleton.Instance.Id);
                Console.WriteLine($"t2 again: " + PerThreadSingleton.Instance.Id);
                Console.ReadKey();
            });
            Task.WaitAll(t1, t2);
        }


    }
}