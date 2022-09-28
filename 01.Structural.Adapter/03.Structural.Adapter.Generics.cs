using System;
using System.Linq;

namespace DotNetDesignPatternDemos.Structural.Adapter
{

    /*
     * In questo esempio vediamo un tipo particolare di adapter per lo specifico usando i generics
     * che usano un modello consueto di definire tramite tipo un oggetto qualunque, solitamente 
     * questo tipo di adapter non è necessario in c++ mentre invece lo è in C e c#.
     * In questo esempio vogliamo rappresentare due vettori uno di tipo 2d e l'altro 3d, dove i punti
     * possono esseere espressi in double o decimal o integer o altri..
     * L'esempio della costruzione di classi concrete partendo dalla base di classi con i generici
     * ricorsivi permette di creare queste istanza appropriate attraverso l'adapter principale
     * che oltre a istanziare quindi uitilizzare il corretto tipo per il tipo di destinazione
     * l'evoluzione del vettore di base indipendetmenete dal tipo numerico adottato.
     */


    // Vector2f, Vector3i

    public interface IInteger
    {
        int Value { get; }
    }

    // Quindi per l'adapter veien intenzinalmente indentata con 
    // il namespace per rendere più sintatticamente eleggibile
    // per quale tipo di trasformazione vettoriale si vuole lavorare
    public static class Dimensions
    {
        public class Two : IInteger
        {
            public int Value => 2;
        }

        public class Three : IInteger
        {
            public int Value => 3;
        }
    }

    // Ed ecco quindi che la classe base utilizzerà 
    // il suo costruttore di default adattando al tipo 
    // di numero che si vuole usare per le nuove istanze
    public class Vector<TSelf, T, D>
      where D : IInteger, new()
      where TSelf : Vector<TSelf, T, D>, new()
    {
        protected T[] data;

        public Vector()
        {
            data = new T[new D().Value];
        }

        // Per non specificare quindi tutti i valori per tipo
        // viene usato il paradigma dei parametri passati come
        // argomenti dinamici
        public Vector(params T[] values)
        {
            var requiredSize = new D().Value;
            data = new T[requiredSize];

            var providedSize = values.Length;

            for (int i = 0; i < Math.Min(requiredSize, providedSize); ++i)
                data[i] = values[i];
        }

        // O per usare in modo fluente la classe viene
        // inserito un factory per l'istanze che servono
        public static TSelf Create(params T[] values)
        {
            var result = new TSelf();
            var requiredSize = new D().Value;
            result.data = new T[requiredSize];

            var providedSize = values.Length;

            for (int i = 0; i < Math.Min(requiredSize, providedSize); ++i)
                result.data[i] = values[i];

            return result;
        }

        // La classe a questo punto sarà indicizzata
        // per l'intero set di vettori che si stanno
        // creando.
        public T this[int index]
        {
            get => data[index];
            set => data[index] = value;
        }

        public T X
        {
            get => data[0];
            set => data[0] = value;
        }
    }

    // In questo caso la classe concreta invece si
    // occupa di specializzare la classe base per i tipi di  
    // vettori che sono con la numerazion in float.
    // Questo approccio attraverso la tecnica avanzata dei
    // generics ricorsivi e con l'ausilio del where che obbliga
    // che il tipo passato sia ereditato da quello base Vector
    // permette di creare un adattatore vincolato al tipo di destinazione.
    public class VectorOfFloat<TSelf, D>
      : Vector<TSelf, float, D>
      where D : IInteger, new()
      where TSelf : Vector<TSelf, float, D>, new()
    {
    }

    // In questo caso la classe concreta invece si
    // occupa di specializzare la classe base per i tipi di  
    // vettori che sono con la numerazion in integer ma definendo.
    // come adattoare un comportamento specifico per l'operatore in overriding
    // usato per la matematica vettoriale di posizione
    public class VectorOfInt<D> : Vector<VectorOfInt<D>, int, D>
      where D : IInteger, new()
    {
        public VectorOfInt()
        {
        }

        public VectorOfInt(params int[] values) : base(values)
        {
        }

        public static VectorOfInt<D> operator +
          (VectorOfInt<D> lhs, VectorOfInt<D> rhs)
        {
            var result = new VectorOfInt<D>();
            var dim = new D().Value;
            for (int i = 0; i < dim; i++)
            {
                result[i] = lhs[i] + rhs[i];
            }

            return result;
        }
    }

    // Per ereditarietà invece qui ci troviamo
    // a definire una classe concreta er l'utilizo
    // di interi dalla base senza particolari accorgimenti
    // nell'adattare questo tipo di vettore nelle istruzioni.
    public class Vector2i : VectorOfInt<Dimensions.Two>
    {
        public Vector2i()
        {
        }

        public Vector2i(params int[] values) : base(values)
        {
        }
    }

    // Stesso per il tipi di vettore definiti in uno spazio
    // tridimensianale
    public class Vector3f
      : VectorOfFloat<Vector3f, Dimensions.Three>
    {
        public override string ToString()
        {
            return $"{string.Join(",", data)}";
        }
    }

    class Demo
    {
        public static void Main(string[] args)
        {
            var v = new Vector2i(1, 2);
            v[0] = 0;

            var vv = new Vector2i(3, 2);

            // Tramite l'overriding del'opratore adattato 
            // per questi calcoli (distanz di due punti)
            var result = v + vv;

            // Tramite la classe concreta e con la l'ausilio dei
            // generici ricorsivi per avere indipendemente
            // dai parametri che poi si adatteranno al tipo passato.
            Vector3f u = Vector3f.Create(3.5f, 2.2f, 1);


        }
    }
}