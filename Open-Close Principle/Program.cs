using System;
using System.Collections.Generic;
using static System.Console;

namespace DotNetDesignPatternDemos.SOLID.OCP
{
    /*
     * Open Close Principio. Secondo quanto previsto nel principio in oggetto quello
     * che è necessario è definire un comportamento neutrale e interconnesso tra 
     * elementi che devono partecipare alla risoluzione di un problema.
     * In questo esempio lo scenario è diviso in due parti, il primo che non usa questo 
     * principio.
     * Nel primo caso abbiamo una classe Product che ha attributi per Categoria e Colore
     * e Taglia e viene richiesto dal Programma di poter filtrare un Insieme di Prodotti
     * messi nel sistema su una Collection o una Lista, quindi nel modo comune potresti
     * creare una classe ProductFilter che si occupa appunto di avere i metodi in sè che
     * riprendono la Lista passata nel primo argomento e per ogni tipo di attributo il
     * valore previsto dal filtro. Ma qua succede una cosa che non è da sottovalutare, la
     * cobinazione di questo porta ad avere ulteriori combinazioni di metodi che prevedano
     * altrettante risoluzioni di filtro, quindi per avere FilterByColorAndSize e ancora
     * FilterByColorAndSizeAndNCombination. Qui il problema si pone per questo principio 
     * che il Problema, perchè il principio di Open-Close è di mantenere le classi aperte
     * per le estensioni e non chiuse per fare refactoring di codice su ogni specifica che
     * arriva. Se per questo esempio la volta successiva si ha bisogno di un nuovo attributo
     * e quindi di poter filtrare per questo nuovo attributo e per tutte le combinazioni di
     * esso con gli altri, abbiamo costruito un codice di classe chiusa e vincolata, mentre 
     * se il principio di open close viene a delineare cosa è possibile fare per quel determinato
     * caso come in questo caso il filtro senza mettere mani alla classe ma portando fuori
     * dalla classe l'implementazione di uso con quello che la classe ha già.
     * In questo esempio vediamo nella seconda parte come il filtro è definitio all'esterno
     * del problema generalizzando per poter accettare nuovi filtri a runtime codificando
     * l'oggetto filtro affinchè sia combinabile con And logici di combinazione ottenendo cosi
     * la possibilità di non guardare a N problemi della stessa natura ma a estendere una
     * classe aperta a queste estensioni.
     * Per ottenere questo risultato in questo caso usiamo un Modello di progettazione che
     * è definito Modello di Specifiche e valuta Ogni scelta di un nuovo filtro come una
     * nuova Specifica quindi invece di aver una singola classe cumulativa di tutti i filtri appunto
     * ProductFilter, cominciamo a definire due interfacce base ISpecification e la IFilter
     * la prima stabilisce in sostanza se un determinato Prodotto soddisfa determinati criteri,
     * quindi funziona come un predicato funzionante su qualsiasi tipo T, e risponde a un unica
     * domanda con il metodo da implementare IsSatisfied(T t) questo per fa si che un determinato
     * elemento di tipo T soddisfi effettivamente alcuni criteri, latra interfaccia invece serve
     * a fornire di un Metodo per la questione in atto che può essere richiamato per eseguire
     * il filtro secondo un determinato tipo T specificato, e fornisce appunto il metodo 
     * Filter(IEnumerable<T> items, ISpecification<T> spec) per passare la lista e le specifiche
     * del filtro da applicare. Dopodichè si definiscono per ogni attributo della classe Product
     * coinvolto nelle specifiche richieste del programma per il filtraggio delle classi di specifica
     * da usare per applicare la funzione del filtro, quindi per il Colore avremo una classe
     * ColorSpecification per la taglia una classe SizeSpecification etcc. e tutti rispondereanno
     * all'espressione se il valore del filtro da applicare IsSatsfied cioè se è uguale a quello che
     * stiamo monitorando nella lista è da includere o escludere per il comportamenteo del filtro.
     * Fatto questo adesso risolviamo il problema combinatorio degli attributi da sottoporre a filtro
     * estendendo ulteriorarmente e quindi tirando fuori il problema dal contesto con una nuova specifica
     * richiesta senz preoccuparci stavolta di avere tutte le combinatorie ma usando un tipo particolare
     * di specifica chiamata AndSpecification che ci dà la possibilità di combinare la prima specifica
     * con una seconda e terza specifica in modo combinatorio e questa istanziata appunto con la prima
     * e la succesiva specifica risponderà sempre nel Metodo IsSatisfied(p) confrontando con un && logico
     * i valori assunti dal filtro con quelli presenti nel prodotto attraverso questa semplice espressione
     * first.IsSatisfied(p) && second.IsSatisfied(p).
     * Infine non ci rimane che avere la classe BetterFilter : IFilter<Product> per operare propriamente
     * sul filtro che sarà sull'insieme dei prodotti a prescindere da quale specifa di attributo di voglia
     * trattare il filtro o filtro combinato. 
     * 
     * in questo esempio anche se articolato e completo avendo usato un modello di progettazione per specifiche,
     * ha solo voluto affrontare il principio di Open Closed facendo si non pensare a costrutire Classi oggetto
     * chiuse e otenzialmente grandi per risolvere problemi comuni che possono essere presi all'esterno estendendo
     * le possibilità della classe in questo caso era la classe ProductFilter senza prima aver valutaro la possibilità
     * di usare un Modello di progettazione in grado di ricolvere il problema al di fuori del contesto.
     */

    // Tipi di colore previsti per l'attributo in Product
    public enum Color { Red, Green, Blue }

    // Tipi di Taglie previsti per l'attributo Size
    public enum Size { Small, Medium, Large, Yuge }

    // La classe Product con i suoi Attributi
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

    // Classe che si occupa per il Filtraggio di questi attrbiuti ( Metodo Classico )
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
        bool IsSatisfied(Product p);
    }

    public interface IFilter<T>
    {
        IEnumerable<T> Filter(IEnumerable<T> items, ISpecification<T> spec);
    }

    // Classi che servono per le specifiche da usare per il filtro
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

    // combinator
    public class AndSpecification<T> : ISpecification<T>
    {
        private readonly ISpecification<T> first, second;

        public AndSpecification(ISpecification<T> first, ISpecification<T> second)
        {
            this.first = first ?? throw new ArgumentNullException(paramName: nameof(first));
            this.second = second ?? throw new ArgumentNullException(paramName: nameof(second));
        }

        public bool IsSatisfied(Product p)
        {
            return first.IsSatisfied(p) && second.IsSatisfied(p);
        }
    }

    // Classe accessoria Filtro per i Prodotti
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
