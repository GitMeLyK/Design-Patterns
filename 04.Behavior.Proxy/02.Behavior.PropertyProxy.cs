using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Windows;
using static System.Console;


namespace DotNetDesignPatternDemos.Structural.Proxy.Property
{
    /*
     * Un roxy di proprietà è molto comune negli sviluppi e diventa
     * quasi naturale costruire questi design che quasi non ce ne accorgiamo.
     * Facendo un esempio di questi tipi di design vediamo che per una
     * ipotetic proprietà chiamata temperatura i valori possono essere espressi
     * in Celsius Fahrenhait o Kelvin quindi necessiti di un metodo che converti
     * questi valori in uscita attraverso una sorta di valore della proprietà.
     * Quindi ipotizzando un proxy di proprietà per il valore si assume che vi
     * sia un oggetto che esponga tale proprietà, e per fare ciò definisci e
     * costruisci questo oggetto per incapsulare la proprietà del valore mantenuto.
     * Nell'esempio l'oggetto viene chiamato Property<T> dove T è il tipo di valore
     * in modo generico per essere utilizzato dinamicamente da incapsulare nella
     * proprietà istanziata.
     * Nell'esempio attraverso l'uso della proprietà vediamo una cosa a cui prestare
     * attenzione, essendoci un override dell'operatore di conversione automatica del
     * tipo
     */

    public class Property<T> : IEquatable<Property<T>> where T : new()
    {
        // Incapsulare il valore per la proprietà corrente
        private T value;

        // Renderla disponibile nel contesto del dominio applicativo
        public T Value
        {
            get => value;
            set
            {
                if (Equals(this.value, value)) return;
                WriteLine($"Assigning value to {value}");
                this.value = value;
            }
        }

        // Il valore di default vuoto
        public Property() : this(default(T))
        {

        }

        public Property(T value)
        {
            this.value = value;
        }

        /* Override di operatori usabili in questa Proprietà */

        // Conversion proxata della conversione automatica del tipo
        public static implicit operator T(Property<T> property)
        {
            return property.value; // int n = p_int;
        }
        // Conversion proxata della conversione automatica del tipo
        public static implicit operator Property<T>(T value)
        {
            return new Property<T>(value); // Property<int> p = 123;
        }

        // Uguaglianza proxata del tipo
        public static bool operator ==(Property<T> left, Property<T> right)
        {
            return Equals(left, right);
        }

        // Disuguaglianza proxata del tipo
        public static bool operator !=(Property<T> left, Property<T> right)
        {
            return !Equals(left, right);
        }

        /* Override di uguaglianze per riferimento usabili in questa Proprietà */

        public bool Equals(Property<T> other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return EqualityComparer<T>.Default.Equals(value, other.value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Property<T>)obj);
        }

        // Con questo definiiamo il reale oggetto da cui trarne l'unicità di riferimento e istanza
        public override int GetHashCode()
        {
            return value.GetHashCode();
        }

    }

    // L'oggetto che inetrnamente ha la proprietà proxata
    public class Creature
    {
        // Il getter e setter interno da proxare sul valore da restituire
        private readonly Property<int> agility = new ();

        // Il Valore da restituire nel tipo previsto proxato sull'oggetto Proprerty che lo conserva
        // In c++ questo accorgimento non sarebbe stato necessario in quanto si potrebbe sovraccaricare
        // come modello l'operatore di assegnazione e otterremmo con una sola riga questo proxy di assegnazione.
        public int Agility
        {
            get => agility.Value;           // Restituire il valore dall'istanza della Property agility
            set => agility.Value = value;   // Setta il valore nell'istanza della Property agility
        }
    }

    public class Demo
    {
        static void Main9(string[] args)
        {
            Creature c = new ();
            // Senza proxy del metodo avremmo avuto come si vede a destra
            // commentato un istanza nuova ogni volta del valore da conservare
            // nella classe che fa uso di quella proprietà.
            c.Agility = 10; // c.set_Agility(10) xxxxxxxxxxxxx
                            // c.Agility = new Property<int>(10)
            
            // E questo adesso anche se assegniamo due volte 10 alla proprietà
            // si traduce a non avere due istanze diverse di valore di proprietà
            // perchè il proxy di chiamata nell'ggetto per il get e il set ha fatto si
            // che operiamo sempre sul valore e non sull'oggetto che tiene il valore.
            c.Agility = 10; 
        }
    }
}