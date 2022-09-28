using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using static System.Console;

namespace DotNetDesignPatternDemos.Structural.Composite.SOLID.OCP
{
    /*
     * In questo esempio vediamo che il modello composito di trattamento
     * per gli aggregati e come abbiamo visto per accoumnare insieme su 
     * insiemi o singoli elementi, ci porta al principio dell'aspetto OC Aperto Chiuso
     * quindi rivedendo l'esempio già riportato nell'apetto dei principi di programmazione
     * SOLID nello specifico nel secondo paradigma OC, si vede come nell'esempio che
     * abbiamo in linea di principio un particolare oggetto di tipo T quindi quell'oggetto
     * potrebbe essere un Prodotto come qualcos'altro, e quindi stiamo creando combinazioni
     * in una specifica, e dobbiamo verificare se un determinato tipo ad esempio un prodotto
     * si adatta a deteminati criteri e in secondo luogo possiamo effettuare una specifica
     * che ne determini anche questo di criterio. Ecco quindi che in questo esempio si può
     * cambiare qualcosa per adattarlo ad un modello di progettazione composita dove possiamo
     * fare in modo che più secifiche vengano combinate tra loro e aggiungerne di nuove.
     * Fondamentalmente abbiamo fatto in modo di trasformare un concetto che si aspettava singole 
     * specifche in un modello coposito di aggregati di specifiche che sia una singola o una
     * collection dove l'interfaccia parte sempre dalla base che arriva un insieme per avere
     * sempre e comunque un interfaccia sempre uguale.
     * */

    public enum Color
    {
        Red, Green, Blue
    }

    public enum Size
    {
        Small, Medium, Large, Yuge
    }

    public class Product
    {
        public string Name;
        public Color Color;
        public Size Size;

        public Product(string name, Color color, Size size)
        {
            Name = name ?? throw new ArgumentNullException(paramName: nameof(name));
            Color = color;
            Size = size;
        }
    }

    public class ProductFilter
    {
        // let's suppose we don't want ad-hoc queries on products
        public IEnumerable<Product> FilterByColor(IEnumerable<Product> products, Color color)
        {
            foreach (var p in products)
                if (p.Color == color)
                    yield return p;
        }

        public static IEnumerable<Product> FilterBySize(IEnumerable<Product> products, Size size)
        {
            foreach (var p in products)
                if (p.Size == size)
                    yield return p;
        }

        public static IEnumerable<Product> FilterBySizeAndColor(IEnumerable<Product> products, Size size, Color color)
        {
            foreach (var p in products)
                if (p.Size == size && p.Color == color)
                    yield return p;
        } // state space explosion
          // 3 criteria = 7 methods

        // OCP = open for extension but closed for modification
    }

    // we introduce two new interfaces that are open for extension

    public interface ISpecification<T>
    {
        //bool IsSatisfied(Product p);
        // Qui cambia con il tipo invece Product p  diventa T t
        bool IsSatisfied(T t);

    }

    public interface IFilter<T>
    {
        IEnumerable<T> Filter(IEnumerable<T> items, ISpecification<T> spec);
    }

    public class ColorSpecification : ISpecification<Product>
    {
        private readonly Color color;

        public ColorSpecification(Color color)
        {
            this.color = color;
        }

        public bool IsSatisfied(Product p)
        {
            return p.Color == color;
        }
    }

    public class SizeSpecification : ISpecification<Product>
    {
        private readonly Size size;

        public SizeSpecification(Size size)
        {
            this.size = size;
        }

        public bool IsSatisfied(Product p)
        {
            return p.Size == size;
        }
    }

    // L'astrazione di questa classe in aggiunta mantiene in
    // sostanza una raccolta di specifiche, questo è il modello classico
    // di design composito dove hai qualcosa che espone interfacce di
    // un singolo oggetto pur non essendo un singolo oggetto
    public abstract class CompositeSpecification<T>: ISpecification<T>
    {
        // Questo è qualcosa che vogliamo poi ereditare
        protected readonly ISpecification<T>[] items;

        // Quindi tramite questo costruttore prendiamo un qualsiasi
        // numero di specifiche e le aggreghiamo in modo composito.
        public CompositeSpecification(params ISpecification<T>[] items)
        {
            this.items = items;
        }

        public abstract bool IsSatisfied(T t);
    
    }

    // combinator
    // quini introducendo un altra specifica comincerà tutto a 
    // prendere un aspetto composito. quella che era ISpecification<T> diventerà
    // CompositeSpecification<T>
    public class AndSpecification<T> : CompositeSpecification<T>
    {
        // Quindi la modifica viene apportata qui dove tutte le specifiche
        // funzionalità sono effettivamente contenute nella classe composita di base

        /*
        private ISpecification<T> first, second;

        public AndSpecification(ISpecification<T> first, ISpecification<T> second)
        {
            this.first = first ?? throw new ArgumentNullException(paramName: nameof(first));
            this.second = second ?? throw new ArgumentNullException(paramName: nameof(second));
        }
        */

        public AndSpecification(params ISpecification<T>[] items): base(items)
        {

        }

        // Qui cambia con il tipo invece Product p  diventa T t
        public override bool IsSatisfied(T t)
        {
            //return first.IsSatisfied(p) && second.IsSatisfied(p);
            // Diventa Any -> OrSpecification
            return items.All(i => i.IsSatisfied(t));
        }

    }

    public class BetterFilter : IFilter<Product>
    {
        public IEnumerable<Product> Filter(IEnumerable<Product> items, ISpecification<Product> spec)
        {
            foreach (var i in items)
                if (spec.IsSatisfied(i))
                    yield return i;
        }
    }

    public class Demo
    {
        static void Main(string[] args)
        {
            var apple = new Product("Apple", Color.Green, Size.Small);
            var tree = new Product("Tree", Color.Green, Size.Large);
            var house = new Product("House", Color.Blue, Size.Large);

            Product[] products = { apple, tree, house };

            var pf = new ProductFilter();
            WriteLine("Green products (old):");
            foreach (var p in pf.FilterByColor(products, Color.Green))
                WriteLine($" - {p.Name} is green");

            // ^^ BEFORE

            // vv AFTER
            var bf = new BetterFilter();
            WriteLine("Green products (new):");
            foreach (var p in bf.Filter(products, new ColorSpecification(Color.Green)))
                WriteLine($" - {p.Name} is green");

            WriteLine("Large products");
            foreach (var p in bf.Filter(products, new SizeSpecification(Size.Large)))
                WriteLine($" - {p.Name} is large");

            WriteLine("Large blue items");
            foreach (var p in bf.Filter(products,
              new AndSpecification<Product>(new ColorSpecification(Color.Blue), new SizeSpecification(Size.Large)))
            )
            {
                WriteLine($" - {p.Name} is big and blue");
            }
        }
    }

}
