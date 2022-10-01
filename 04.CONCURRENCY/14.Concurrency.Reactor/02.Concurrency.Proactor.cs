using System;

namespace DotNetDesignPatternDemos.Concurrency.Proactor
{
    /*
     *  Il modello Proactor rispetto al Reactor separa completamente il codice specifico dell'applicazione 
     *  dall'implementazione del reattore, il che significa che i componenti dell'applicazione 
     *  possono essere suddivisi in parti modulari e riutilizzabili ma in modo multiplexer.
     *       
     *       
     *  In allegato il documento completo PDF a questo progetto. tpd_reactor_proactor.pdf
     *  dove vengono illustrate le caratteristiche e l'implemtnailità di questi modelli simili.
     *  Nella seconda parte si parla appunto del Proactor.
     *  
     *  -------------------------------------------------------------------------------------
     * 
     */

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
