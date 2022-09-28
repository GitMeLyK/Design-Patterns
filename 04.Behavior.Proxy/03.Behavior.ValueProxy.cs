using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Windows;
using static System.Console;


namespace DotNetDesignPatternDemos.Structural.Proxy.Value
{
    /*
     * Un proxy di valore non è un tipo molto comune ma per determinati sviluppi diventa
     * un passaggio obbligato da apprendere.
     * Come abbiamo visto nel precedente i proxy sono preposti ad avere molte
     * caratteristiche e molti metodi di diverso tipo di funzionalità, in questo
     * caso invece abbiamo un proxy che lavora ed è costruito intorno ad un tipo primitivo.
     * La domanda del perchè costruire un proxy intorno a un int o un float viene subito
     * rappresentata nel cosa vogliamo effettivamente avere come valore e per come abbiamo fatto
     * per la proprietà proxy anche qui ne creiamo una cosa simile dove il tipo conservato
     * di valore è un tipo primitivo su cui applicare calcoli di valore effettivo per il
     * salvataggio all'interno della proprietà e la restituzione del valore di uscita a quello
     * di provenienza.
     * Nel caso dell'esempio la proprietà chiamata Percentage in sè contiene il valore primitvo 
     * in float e usa gli override degli operatori sempre sui valori effettivi, ma in questo
     * caso sovviene una classe di estensione per il float che farà appunto da proxy di valore
     * per usare nel Float primitivo il metodo di Percentuale appropriato all'uso e scopo.
     */

    [DebuggerDisplay("{value*100.0f}%")]
    public struct Percentage
    {
        private readonly float value;

        internal Percentage(float value)
        {
            this.value = value;
        }

        //    public static implicit operator Percentage(float value)
        //    {
        //      return new Percentage(value);
        //    }

        public static float operator *(float f, Percentage p)
        {
            return f * p.value;
        }

        public static Percentage operator +(Percentage a, Percentage b)
        {
            return new Percentage(a.value + b.value);
        }

        public static implicit operator Percentage(int value)
        {
            return value.Percent();
        }

        public bool Equals(Percentage other)
        {
            return value.Equals(other.value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Percentage other && Equals(other);
        }

        public override int GetHashCode()
        {
            return value.GetHashCode();
        }

        public override string ToString()
        {
            return $"{value * 100}%";
        }

        public static bool operator ==(Percentage left, Percentage right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Percentage left, Percentage right)
        {
            return !(left == right);
        }
    }

    // Proxy del valore per restituire il fattore di calcolo del valore interno primitivo float
    public static class PercentageExtensions
    {
        public static Percentage Percent(this int value)
        {
            return new Percentage(value / 100.0f);
        }

        public static Percentage Percent(this float value)
        {
            return new Percentage(value / 100.0f);
        }
    }


    class Program
    {
        public static void Main14(string[] args)
        {
            // Ecco quindi come abbiamo esteso il nostro float
            // proxandolo in entrata e in uscita per il fattore di calcolo
            // da usare.
            Console.WriteLine(10f * 5.Percent());
            Console.WriteLine(2.Percent() + 3.Percent());
        }
    }
}