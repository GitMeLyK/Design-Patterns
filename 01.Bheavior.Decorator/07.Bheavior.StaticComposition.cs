using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using static System.Console;

namespace DotNetDesignPatternDemos.Structural.Decorator.StaticComposition
{
    /*
     * Nel precedente esempio si è operati su delle forme geometriche
     * e usa il modello dinamico come prima per comporle. ma in questo caso
     * specializzando la classe ShapeDecorator : Shape con delle policy dove
     * impediamo alcuni comportamenti al decoratore. 
     * in linguaggi come c++ è comune ereditare da modelli mentre in c# 
     * questo non lo si può fare
     * public class ColoredShape<T> : T // in c# ereditare da un modello per il tipo non è fattibile.
     * questo in c++ comunemente è chiamato modello di modello
     * e come si vede nell'esempio 
     *             var circle = new TransparentShape<ColoredShape<Circle>>(0.4f);
     * alla fine fà più o meno capire che non è fattibile.
     */

    /*************************************************/

    // Partendo quindi da una classe astratta
    public abstract class Shape
    {
        public virtual string AsString() => string.Empty;
    }

    public sealed class Circle : Shape
    {
        private float radius;

        // Il costruttore di default a raggio 0
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

        // Il costruttore di default con un solo lato
        public Square() : this(0)
        {
        }

        public Square(float side)
        {
            this.side = side;
        }

        public override string AsString() => $"A square with side {side}";
    }


    // CRTP cannot be done ( Come abbiamo detto questo del modello che erediti su altro modello non si può fare )
    //public class ColoredShape2<T> : T where T : Shape { }

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

    // I generics che implementao i propri elementi di base

    public class ColoredShape<T> : Shape where T : Shape, new()
    {
        private readonly string color;
        private readonly T shape = new ();

        // Per il new() abbiamo bisogno di un costruttore vuoto
        // e che sia predefinito per il fattore di eredità
        public ColoredShape() : this("black")
        {

        }

        public ColoredShape(string color) // no constructor forwarding
        {
            this.color = color ?? throw new ArgumentNullException(paramName: nameof(color));
        }

        // Ecco che qui adesso ho la possibilità di sottporlo ad override
        public override string AsString()
        {
            return $"{shape.AsString()} has the color {color}";
        }
    }

    public class TransparentShape<T> : Shape where T : Shape, new()
    {
        private readonly float transparency;
        private readonly T shape = new ();

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
        static void Main80(string[] args)
        {

            var circle = new TransparentShape<ColoredShape<Circle>>(0.4f);
            
            // Ecco perchè in una composizione statica non possiamo avere quello che
            // in c++ si può fare e cioè non è possibile usare l'ereditarietà per ottenere
            // il corrispettivo costruttore e diventa arduo. Ed ' per questo che l'approccio
            // statico alla composizione del decoratore in c# purtroppo non è fattibile.
            // ** circle.Radius = 10;


        }
    }
}