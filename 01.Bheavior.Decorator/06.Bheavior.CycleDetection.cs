using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using static System.Console;

namespace DotNetDesignPatternDemos.Structural.Decorator.CycleDetection
{
    /*
     * Questo esempio come nel precedente opera su delle forme geometriche
     * e usa il modello dinamico come prima per comporle, ma in questo caso
     * specializzando la classe ShapeDecorator : Shape con delle policy dove
     * impediamo alcuni comportamenti al decoratore. Per spiegare meglio
     * se le classi si sovrappongono come colore e trasparenza ad esempio
     * non ha senso sovrappore colore su colore su colore o trasparenza
     * su trasparenza etc. Quindi decidiamo di allineare queste composizioni
     * in modo da non incorrere nel consumo delle api per questo decoratore
     * in modo disordinato usando appunto un modo per questi decoratori dinamici
     * di non applicare queste sovrpposizioni e impedire l'utente di adottarle.
     * Adottando questa classe di Politica nell'esempio viene chiamata ShapeDecoratorCyclePolicy
     * di tipo astratta  vediamo che non ci releghiamo al costruttore per impedire
     * la sovrapposizione e gestire l'insieme sovrapposto e anche per tipi diversi che 
     * aumenterebbe in modo esponenziale il codice, ma adottiamo quella che è una
     * strategia (argomento di pattern trattato più avanti) per usare politiche di
     * sovrapposizioni coerenti impedendo per la stessa sovrapposizione dello stesso tipo
     * di andare avanti a runtime.
     */

    /*************************************************/

    public abstract class Shape
    {
        public virtual string AsString() => string.Empty;
    }

    public sealed class Circle : Shape
    {
        private float radius;

        public Circle() : this(0)
        {
        }

        public Circle(float radius)
        {
            this.radius = radius;
        }

        public void Resize(float factor)
        {
            radius *= factor;
        }

        public override string AsString() => $"A circle of radius {radius}";
    }

    public sealed class Square : Shape
    {
        private readonly float side;

        public Square() : this(0)
        {
        }

        public Square(float side)
        {
            this.side = side;
        }

        public override string AsString() => $"A square with side {side}";
    }

    /****************** POLICY ************************/

    // Politiche gestite per la non sovrapposizione di elementi
    // senza un corretto ordine applicato al tipo.
    // Tramite queste politiche gestiamo cosa in modo compisito
    // uò essere sovrapposto uno sull'altro e come intercettarlo e gestirlo.
    public abstract class ShapeDecoratorCyclePolicy
    {
        // Tipi accettati nella composizione ad essere sovrapposti
        public abstract bool TypeAdditionAllowed(Type type, IList<Type> allTypes);
        
        // Tipi accettati per l'applicazione
        public abstract bool ApplicationAllowed(Type type, IList<Type> allTypes);
    }

    // Tramite l'adozione di questa classe concreta in linea di
    // sovrapposizione di decoratori viene sollevata una eccezione.
    public class ThrowOnCyclePolicy : ShapeDecoratorCyclePolicy
    {
        private bool handler(Type type, IList<Type> allTypes)
        {
            if (allTypes.Contains(type))
                throw new InvalidOperationException(
                  $"Cycle detected! Type is already a {type.FullName}!");
            return true;
        }

        public override bool TypeAdditionAllowed(Type type, IList<Type> allTypes)
        {
            return handler(type, allTypes);
        }

        public override bool ApplicationAllowed(Type type, IList<Type> allTypes)
        {
            return handler(type, allTypes);
        }
    }

    // Tramite l'adozione di questa classe concreta in linea di
    // sovrapposizione di decoratori viene accettata la sovrapposizione.
    public class AbsorbCyclePolicy : ShapeDecoratorCyclePolicy
    {
        public override bool TypeAdditionAllowed(Type type, IList<Type> allTypes)
        {
            return true;
        }

        public override bool ApplicationAllowed(Type type, IList<Type> allTypes)
        {
            return !allTypes.Contains(type);
        }
    }

    public class CyclesAllowedPolicy : ShapeDecoratorCyclePolicy
    {
        public override bool TypeAdditionAllowed(Type type, IList<Type> allTypes)
        {
            return true;
        }

        public override bool ApplicationAllowed(Type type, IList<Type> allTypes)
        {
            return true;
        }
    }

    /****************** POLICY ************************/


    // Il decorator inteso come aggregato di decoratori di Base
    public abstract class ShapeDecorator : Shape
    {
        protected internal readonly List<Type> types = new();
        protected internal Shape shape;

        public ShapeDecorator(Shape shape)
        {
            this.shape = shape;
            if (shape is ShapeDecorator sd)
                types.AddRange(sd.types);
        }
    }

    // Il decorator genereico di base per l'applicazione delle politiche di sovrapposizione
    // che tramite un generico ricorsivo attua a controllare se stesso se è sovrapponibile.
    public abstract class ShapeDecorator<TSelf, TCyclePolicy> : ShapeDecorator
      where TCyclePolicy : ShapeDecoratorCyclePolicy, new()
    {
        protected readonly TCyclePolicy policy = new();

        public ShapeDecorator(Shape shape) : base(shape)
        {
            if (policy.TypeAdditionAllowed(typeof(TSelf), types))
                types.Add(typeof(TSelf));
        }
    }

    // can determine one policy for all classes
    // Questa classe in aggiunta se si vuole avere un controllo globale dell'applicazione
    // della policy a livello di decorator sulla classe
    public class ShapeDecoratorWithPolicyThrow<T>
      : ShapeDecorator<T, ThrowOnCyclePolicy>
    {
        public ShapeDecoratorWithPolicyThrow(Shape shape) : base(shape)
        {
        }
    }

    // Con questa politica non permettiamo nello stesso ciclo di sovrapporre
    // due elementi che già sono stati usati...
    public class ShapeDecoratorWithPolicyAbsorb<T>
        : ShapeDecorator<T, AbsorbCyclePolicy>
    {
        public ShapeDecoratorWithPolicyAbsorb(Shape shape) : base(shape)
        {
        }
    }


    // dynamic
    public class ColoredShape
      //: ShapeDecorator<ColoredShape, AbsorbCyclePolicy>   // o dalla classe policy
      : ShapeDecoratorWithPolicyAbsorb<ColoredShape>        // o dalla classe alias come punto globale
    {

        private readonly string color;

        public ColoredShape(Shape shape, string color) : base(shape)
        {
            this.color = color;
        }

        public override string AsString()
        {
            var sb = new StringBuilder($"{shape.AsString()}");

            if (policy.ApplicationAllowed(types[0], types.Skip(1).ToList()))
                sb.Append($" has the color {color}");

            return sb.ToString();
        }
    }


    public class TransparentShape //: Shape
      //: ShapeDecorator<ColoredShape, AbsorbCyclePolicy>   // o dalla classe policy
      : ShapeDecoratorWithPolicyThrow<TransparentShape>     // o dalla classe alias come punto globale
    {
        //private Shape shape;
        private readonly float transparency;

        //public TransparentShape(Shape shape, float transparency)
        public TransparentShape(Shape shape, float transparency) : base(shape)
        {
            // Questo potrebbe essere un modo per impedire la sovrapposizione
            // ma non è il modo corretto in quanto se provassimo a fare una sovrapposizione
            // di una forma colorata su una forma di trasparenza che a sua volta è su una forma
            // colorata non potremmo avere e sollevare questa ecceione e ci ritroveremmo
            // ad una sovrapposizione di due colori non voluti. Ecco che dobbiamo complicare
            // un pò il codice di composizione impedendo tramite calssi di politica decorativa.
            //if (shape is TransparentShape)
            //    throw new Exception("Not valid");
            //this.shape = shape ?? throw new ArgumentNullException(paramName: nameof(shape));
            this.transparency = transparency;
        }

        public override string AsString() => $"{shape.AsString()} has {transparency * 100.0f} transparency";
    }


    public class Demo
    {
        static void Main70(string[] args)
        {
            var circle = new Circle(2);
            var colored1 = new ColoredShape(circle, "red");
            var colored2 = new ColoredShape(colored1, "blue");

            WriteLine(circle.AsString());
            WriteLine(colored1.AsString());
            WriteLine(colored2.AsString());


        }
    }
}