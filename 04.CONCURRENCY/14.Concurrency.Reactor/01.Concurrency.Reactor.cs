using System;

namespace DotNetDesignPatternDemos.Concurrency.Reactor
{
    /*
     *  Il modello Reactor separa completamente il codice specifico dell'applicazione 
     *  dall'implementazione del reattore, il che significa che i componenti dell'applicazione 
     *  possono essere suddivisi in parti modulari e riutilizzabili.
     *  
     *  In allegato il documento completo PDF a questo progetto. Reactor-Siemes.pdf
     *  
     *  E nella prima parte del secondo allegato tpd_reactor_proactor.pdf viene trattato
     *  e come e quali sono le differenza per l'altro modello simile il proactor.
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
