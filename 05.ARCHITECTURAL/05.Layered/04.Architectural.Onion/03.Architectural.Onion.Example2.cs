using System;

namespace DotNetDesignPatternDemos.Architectural.Onion.Example_2
{
    /*
        * Per questo esempio di Architettura basata sul modello di progettazione Onion
        * riporteremeno un esempio completo per asp.net core e sul quale adottiamo una
        * infrastruttura a cipolla basata su 3 livelli per un servizio basato solo su
        * un set di Api pubbliche WebApi.
        * 
        *      -    Livello di dominio
        *      -    Livello di servizio
        *      -    Livello di persistenza (repository)
        * 
        * Sviluppato in Asp.net core 6 per una soluzione solo WebApi.
        * 
        * In ellegato trovate il progetto di esempio 
        *      
        *      // OnionArchitectureInAspNetCore6WebAPI-master.zip
        *      
        */


    class Program { 

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }

    }
}
