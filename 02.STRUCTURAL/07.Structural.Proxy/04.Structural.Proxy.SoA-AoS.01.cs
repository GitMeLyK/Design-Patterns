using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Windows;
using static System.Console;


namespace DotNetDesignPatternDemos.Structural.Proxy.SoACompositeProxy
{
    /*
     * Un proxy è alle volte un modo per gestire il trattamento
     * di un altro tipo di designe pattern.
     * Nell'esempio qua sotto vediamo come il proxy interviene nel
     * contesto di un modello che abbiamo già visto per il trattamento
     * di array fissi al costruttore usato nel Modello Composito di
     * desgin pattern.
     * Quindi usando una struct interna che fa da proxy per l'enumeratore
     * esterno vediamo come la classe creatures riprende se stessa che contiene
     * internamente tutti gli elementi ordinati in array predeterminati al costruttore.
     * Quindi avremo array per ogni proprietà tutti disposti nell'indice dello stesso
     * ordine per quanti sono le creature trattate. Il proxy disponse quindi per 
     * riferimento le proprietà riprendendole dall'oggetto in cui è contenuto Creatures
     * e restituisce per qul determinato oggetto in base al suo indice i valori salvati.
     * Questo rende più veloce e con meno spreco di memoria l'accesso a una moltitudine 
     * di oggetti rispetto ad creare una lista di oggetti all'esterno.
     */

    class Creature
    {
        public byte Age = 0;
        public int X, Y = 0;
    }

    class Creatures
    {
        private readonly int size;
        private readonly byte[] age;
        private readonly int[] x, y;

        public Creatures(int size)
        {
            // Array fixed on costrunctor
            this.size = size;
            age = new byte[size];
            x = new int[size];
            y = new int[size];
        }

        // deep struct to use some proxy
        public struct CreatureProxy
        {
            private readonly Creatures creatures;
            private readonly int index;

            public CreatureProxy(Creatures creatures, int index)
            {
                this.creatures = creatures;
                this.index = index;
            }

            public ref byte Age => ref creatures.age[index];
            public ref int X => ref creatures.x[index];
            public ref int Y => ref creatures.y[index];
        }

        // Use objects dispose in mode composite to iterate
        public IEnumerator<CreatureProxy> GetEnumerator()
        {
            for (int pos = 0; pos < size; ++pos)
                yield return new CreatureProxy(this, pos);
        }
    }

    public class SoACompositeProxy
    {
        public static void Main25(string[] args)
        {
            // Without composite array
            var creatures = new Creature[100];
            foreach (var c in creatures)
            {
                c.X++; // not memory-efficient
            }

            // With Composite Array initial
            var creatures2 = new Creatures(100);
            foreach (var c in creatures2)
            {
                c.X++;
            }
        }
    }
}