using System;
using System.Collections.Generic;

namespace DotNetDesignPatternDemos.Concurrency.DoubleCheckedLocking
{

    /*
     *  TODO: Completare la spiegazione di questo pattern con un esempio completo.
     * 
     * 
     */


    // Classe di esempio con utilizza uno DoubleChecked Lock in modo errato
    public static class ValuateList
    {
        private static readonly object _lock = new object();
        private static volatile IDictionary<string, object> _cache =
            new Dictionary<string, object>();

        public static object Create(string key)
        {
            object val;
            if (!_cache.TryGetValue(key, out val))
            {
                lock (_lock)
                {
                    if (!_cache.TryGetValue(key, out val))
                    {
                        val = new object(); // factory construction based on key here.
                        _cache.Add(key, val);
                    }
                }
            }
            return val;
        }
    } 


    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
