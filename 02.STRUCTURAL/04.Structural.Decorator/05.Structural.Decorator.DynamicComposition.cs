using System;
using System.Runtime.Serialization;
using System.Text;
using static System.Console;

namespace DotNetDesignPatternDemos.Structural.Decorator.DynamicComposition
{
    /*
     * Questo esempio affronta la possibilità di applicate un modello
     * dinamico per la composizione del decoratore.
     * In questo caso trattando le forme geometriche che ereditano tutte da Shape
     * vengono tratate in modo dinamico per il colore e per la trasparenza
     * dove in modo Composito vengono trattate anche le proprietà come colore
     * e trasparenza come fossero Shape (Forme geometriche che non lo sono) ma
     * al fine ultimo per permetterci di combinare quindi Comporre (Composito)
     * gli elementi , che come vedremo potranno essere sovrapposti, come nell'esempio
     * che vediamo dove la trasparenza o il colore è parte del cerchio o del quadrato
     * e quindi possiamo avere qualcosa di finale come
     * TransparentShape<ColoredShape<Square>> blackHalfSquare che compone il Trasparente
     * su un tipo Colore per un tipo di forma geometrica Quadrato (Questo grazie ai generics)
     * che fanno uso del tipo per essere dinamici al cospetto della composizione.
     * Quindi sovrapporre a quello che è una forma (cerchio o quadrato) un altro tipo
     * che viene espresso come forma (colore e trasparenza) anche se eredita da shape 
     * ma non è uno shape è legittimo comunque farlo per poter comporre il decoratore
     * e sovrapporlo uno sull'altro in modo composito.
     */

    /*************************************************/

    public abstract class Shape
    {
        public virtual string AsString() => string.Empty;
    }

    public class Circle : Shape
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

    public class Square : Shape
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

    // dynamic
    public class ColoredShape : Shape
    {
        private readonly Shape shape;
        private readonly string color;

        public ColoredShape(Shape shape, string color)
        {
            this.shape = shape ?? throw new ArgumentNullException(paramName: nameof(shape));
            this.color = color ?? throw new ArgumentNullException(paramName: nameof(color));
        }

        public override string AsString() => $"{shape.AsString()} has the color {color}";
    }

    public class TransparentShape : Shape
    {
        private readonly Shape shape;
        private readonly float transparency;

        public TransparentShape(Shape shape, float transparency)
        {
            this.shape = shape ?? throw new ArgumentNullException(paramName: nameof(shape));
            this.transparency = transparency;
        }

        public override string AsString() => $"{shape.AsString()} has {transparency * 100.0f} transparency";
    }

    public class ColoredShape<T> : Shape where T : Shape, new()
    {
        private readonly string color;
        private readonly T shape = new ();

        public ColoredShape() : this("black")
        {

        }

        public ColoredShape(string color) // no constructor forwarding
        {
            this.color = color ?? throw new ArgumentNullException(paramName: nameof(color));
        }

        public override string AsString()
        {
            return $"{shape.AsString()} has the color {color}";
        }
    }

    public class TransparentShape<T> : Shape where T : Shape, new()
    {
        private readonly float transparency;
        private readonly T shape = new();

        public TransparentShape(float transparency)
        {
            this.transparency = transparency;
        }

        public override string AsString()
        {
            return $"{shape.AsString()} has transparency {transparency * 100.0f}";
        }
    }

    public class Demo
    {
        static void Main60(string[] args)
        {
            var square = new Square(1.23f);
            WriteLine(square.AsString());

            var redSquare = new ColoredShape(square, "red");
            WriteLine(redSquare.AsString());

            var redHalfTransparentSquare = new TransparentShape(redSquare, 0.5f);
            WriteLine(redHalfTransparentSquare.AsString());

            // static
            ColoredShape<Circle> blueCircle = new("blue");
            WriteLine(blueCircle.AsString());

            TransparentShape<ColoredShape<Square>> blackHalfSquare = new(0.4f);
            WriteLine(blackHalfSquare.AsString());
        }
    }
}