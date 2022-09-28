using System;
using System.Collections.Generic;
using System.Linq;

// todo: this is somehow buggy in recording
namespace DotNetDesignPatternDemos.Behavioral.Strategy
{
    /*
     * Il modello Strategy ben si adatta quando nel sistema esistono
     * componenti che implementano i metodi di Confronto IEquatable
     * e Comparazione IComparable, grazie a questi è possibile avere
     * i costrutti per confrontare l'oggetto e la sua istanza che siano
     * sottoposti a confronto per gli attributi se sono uguali o a 
     * comparazione se sono due istanze differenti a confronto o è lo 
     * stesso oggetto.
     * In questo esempio la classe person che appunto si qualifica implementata
     * di queste interfacce accessorie e ridefinisce con l'override gli operatori
     * di euqality e compare come == != oltre che ad avere i metodi per il confronoto 
     * e il compare previsti dalle interfacce.
     * Bene adesso il Modello di Strategia per questo componente si pone in 
     * mezzo a un problema di esempio come l'implementazione di un metodo di
     * Sorting chiamando per l'elenco la funzione Sort(). Questo funzione accessoria
     * dei tipi List Collection etc non ha in sè il modo di sapere come saranno
     * comparati o confrontati questi oggetti per fare una lista ordinata secondo
     * un determinato criterio e quindi è necessario fornire una Strategia appunto
     * che fornisca a questo metodo il modo di usare questi oggetti nell'enumerarli
     * per metterli in ordine. Quindi la classe Person può usare una strategia predefinita
     * per tutti i tipi oppure una strategia personalizzata, per quella effettiva appunto
     * uso implementare le Intefacce IComparable e IEquatable con il codice necessario
     * per il confronto e la comparazione e quel punto il Sort() sa come deve orindare
     * perchè usa queste stesse interfacce per Operare Startegicamente sull'insieme delle
     * istanze e potendone richiamare i metodi approprieti per fare comparazione per il
     * Sorting e gli altri metodi usati per le Liste e tutti i tipi IEnumeble.
     * 
     */

    // La classe oggetto per COnfronto e Comparazione
    class Person : IEquatable<Person>, IComparable<Person>
    {
        public int Id;
        public string Name;
        public int Age;

        // ******************* IComparable<T> e IComparable ************************

        public int CompareTo(Person other)
        {
            // Grazie a questo viene restituita una relazione di ordinamento.
            if (ReferenceEquals(this, other)) return 0; // Uguale istanza non porta prima le'elemtno
            if (ReferenceEquals(null, other)) return 1; // Istanza nulla la porta prima rispetto all'atro
            return Id.CompareTo(other.Id);              // L'Id  -1 è inferiore lo porta prima altrimenti dopo
        }

        public int CompareTo(object obj)
        {
            // Grazie a questo viene restituita una relazione di ordinamento.
            if (ReferenceEquals(this, obj)) return 0; // Uguale istanza non porta prima le'elemtno
            if (ReferenceEquals(null, obj)) return 1; // Istanza nulla la porta prima rispetto all'atro
            // Dato che questo è tipitzzato debolmente confrontiamo pure che sia del tipo giusto
            // L'Id  -1 è inferiore lo porta prima altrimenti dopo
            return obj is Person other ? Id.CompareTo(other.Id) : throw new ArgumentException();              // L'Id  -1 è inferiore lo porta prima altrimenti dopo
        }

        // ******************* IComparable<T> e IComparable ************************

        // ******************* IEquatable<T> e Iequatable   ************************

        public bool Equals(Person other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Person)obj);
        }

        // ******************* IEquatable<T> e Iequatable   ************************

        public Person(int id, string name, int age)
        {
            Id = id;
            Name = name;
            Age = age;
        }

        // L'id Univoco per l'istanza univoca
        public override int GetHashCode()
        {
            return Id;
        }

        // ******************* Overrire degli operatori   ************************

        public static bool operator ==(Person left, Person right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Person left, Person right)
        {
            return !Equals(left, right);
        }

        // ******************* Overrire degli operatori   ************************

        // Il nostro metodo personalizzato di ordinamento usato come link
        // statico nella proprietà NameComparer da usare per la valutazione
        // personalizzata di elenchi in base al nome dell'oggetto. La funzione
        // è in delega a una qualsiasi funzione di ordinamento presente negli
        // oggetti List o Array o chiunque preveda ordinamenti con argomenti delegati
        // e che usino in modo strategico l'implementazione di questo oggetto fornito
        // delle interfacce necessarie di comparazione e confronto.
        private sealed class NameRelationalComparer : IComparer<Person>
        {
            public int Compare(Person x, Person y)
            {
                if (ReferenceEquals(x, y)) return 0;
                if (ReferenceEquals(null, y)) return 1;
                if (ReferenceEquals(null, x)) return -1;
                return string.Compare(x.Name, y.Name,
                  StringComparison.Ordinal);
            }
        }

        // Il link di supporto alla strategia di comparare l'ordinamento in base
        // al nome degli oggetti correnti usato in modo statico per renderlo 
        // disponibile come link strategico nel sistema in elaborazione ovviando
        // a costrutirlo in lambda al momento ma prelevandolo direttamente dall'oggetto
        // di contesto in cui si sta lavorando sul suo insieme in momenti in cui nel
        // sistema lo colleziona in liste o array.
        public static IComparer<Person> NameComparer { get; }
          = new NameRelationalComparer();
    }

    public class ComparisonStrategies
    {
        public static void Main(string[] args)
        {
            var people = new List<Person>();


            // Grazie al fatto che strtegicamente abbiamo implementato
            // gli oggetti Person tramte le interfacce di Comparazione
            // e Confronto che la List adesso può usare in modo corretto
            // il Sort tramite l'Id univoco per default usato per avere
            // l'Hash univoco dell'oggetto rispetto agli altri, e volendo
            // anche usare il Sort che accetta un delegato a una funzione
            // in questo caso una lambda che viene valutata durante l'esecuzione
            // del metodo per fare un confronto non più con il suo ID ma
            // dal nome di ogni oggetto in cui sta iterando

            // Default Strategy
            // equality == != and comparison < = >
            people.Sort(); // meaningless by default

            // Strategy by lambda
            // Qui è una stategia personalizzata tramie l'espressione lambda a funzione
            // sort by name with a lambda
            people.Sort((x, y) => x.Name.CompareTo(y.Name));

            // Strategy by link static
            // Oltre al fatto che possiamo qui decidere la strategia, possiamo anche
            // specializzare la classe stessa a farci dei collegamenti statici a strategie
            // previste nell'oggetto stesso per l'ordinamento, com in questo caso che
            // piuttosto che elaborare espressioni di volta in volta facciamo usare il
            // delegato a una funzione interna dell'oggetto per la suo scopo di ordinamento
            // chiamando con nomi ben chiari allo scopo NameComparer sarà una funzione
            // di ordinamento in base al nome e non più in base all'id di default che la 
            // classe oggetto offre.
            people.Sort(Person.NameComparer);

        }
    }
}