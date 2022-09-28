using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Windows;
using static System.Console;

namespace DotNetDesignPatternDemos.Structural.Iterator.ArrayBackendProperties
{

    /*
     * In questo esempio viene presentato invece un Iterator dove viene
     * restituita la proprietà implementata di un oggetto.
     * Come per dire invece di enumerare strutture dati viene enumerato 
     * l'insieme delle proprietà di un oggetto e quindi è possibile settare
     * quella determinata proprietà o riprendere il valore.
     * Riprendendo l'esempio del gioco per computer dove i personaggi vengono
     * implementati come classi IEnumerable<int> che invece di enumerare liste
     * interne di strutture dati per essere attraversate come nell'esempio
     * precedente, abbiamo un indicizzatore su un campo di appoggio in questo caso
     * Strenght e un contesto di indicizzatore sul this[int index] che restituisce
     * lo int[] stats che è l'array delle tre proprietà per questa classe, facendo 
     * si che grazie al contesto di enumerazione si possa iterare sull'intero set
     * di proprietà per ottenere i valori come insieme ed effettuare calcoli e 
     * misure aggregate piuttosto che andare a fare calcoli con ogni singolo elemento.
     * Un altro dei vantaggi su questo approccio è che hai un array che fa da lista 
     * delle proprietà e in questo modo hai un tipo Observable, cioè la possibilità
     * al cambio di un singolo valore avere un metodo Reactive che possa intraprendere
     * altre azioni di contesto.
     */


    // La classe personaggio che implementa il tipo IEnumerable<int>
    // per poter accedere e scorre nell'insieme delle prorpeità di questo oggetto.
    public class Creature : IEnumerable<int>
    {
        // Il numero delle proprietà presenti in questo oggetto
        private int[] stats = new int[3];

        private const int strength = 0;

        // Il Campo di appoggio per l'indice da cui iniziare a enumerare 
        // per tutte le altre proprietà .
        public int Strength
        {
            get => stats[strength];
            set => stats[strength] = value;
        }

        public int Agility { get; set; }
        public int Intelligence { get; set; }

        /*
         *  Nel modo classico abbiamo questa ipotesi ma oni volta che
         *  aggiungi proprietà all'ggetto devi intervenire in più punti
         *  public double AverageStat {
         *      get {
         *          return (Strength + Agility + Intelligence) / 3.0;
         *      }
         *  }
         * */

        public double AverageStat =>
          stats.Average();

        public IEnumerator<int> GetEnumerator()
        {
            return stats.AsEnumerable().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int this[int index]
        {
            get { return stats[index]; }
            set { stats[index] = value; }
        }
    }

    public class Demo
    {
        static void Main300(string[] args)
        {
        }
    }
}