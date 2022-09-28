using System;
using System.Security.Cryptography;
using static System.Console;

namespace DotNetDesignPatternDemos.Behavioral.ChainOfResponsibility.MethodChain
{
    /*
     * Nell'esempio corrente il modello della Catena di Responsabilità cade in un ipotetico 
     * gioco di carte che ricevono le Creature del gioco sotto forma di bonus e che ne aumentano
     * il sistema di difesa o di attacco.
     * La classe cardine CreatureModifier che si occupa di fornire una base per tutti gli elementi
     * della catena di responsabilità da applicare.
     * Ogni carta rappresenta nella catena di responsabilità un momento in 
     * cui trattare con l'oggetto sottostante ed applicare l'azione tramite l'handler degli eventi
     * che come vedremo non è un esecuzione immediata ma bensì è una azione nella sequenza che viene
     * salvata e invocata al momento in cui il programma richiede espressamente di applicare tutta la
     * code, in questo caso nel main abbiamo root.Hanlde();
     * Nel momento che alla creatura applichiamo un nuovo modifictore abbiamo un riferimento al 
     * modificatore successivo e questo definisce a una lista collegata chiamata linked list.
     * Ecco perchè il metodo Add per aggiungere un nuovo modificatore alla classe modifier che 
     * internamente tiene il riferimento alla creatura a cui si sta applicando il modificatore.
     */

    public class Creature
    {
        public string Name;
        public int Attack, Defense;

        // Nuova creatura con attacco e difesa iniziali
        public Creature(string name, int attack, int defense)
        {
            Name = name ?? throw new ArgumentNullException(paramName: nameof(name));
            Attack = attack;
            Defense = defense;
        }

        public override string ToString()
        {
            return $"{nameof(Name)}: {Name}, {nameof(Attack)}: {Attack}, {nameof(Defense)}: {Defense}";
        }
    }

    // Questa è nel contesto la classe che si occupa del modello di
    // progettazione per la catena di responsabilità che farà da base
    // per ogni azione utilizzabile nel contesto dei bonus per l'attacco e la difesa
    public class CreatureModifier
    {
        protected Creature creature;
        // La nostra catena tramite una linkedlist di nodi succesivi
        protected CreatureModifier next;

        public CreatureModifier(Creature creature)
        {
            this.creature = creature ?? throw new ArgumentNullException(paramName: nameof(creature));
        }

        public void Add(CreatureModifier cm)
        {
            if (next != null) next.Add(cm);
            else next = cm;
        }

        public virtual void Handle() => next?.Handle();
    }

    // Un tipo di carta che non dà bonus specifici
    public class NoBonusesModifier : CreatureModifier
    {
        public NoBonusesModifier(Creature creature) : base(creature)
        {
        }

        public override void Handle()
        {
            // nothing
            WriteLine("No bonuses for you!");
        }
    }

    // Un altro tipo di carta che permette di aumentare il valore di attacco della creatura
    public class DoubleAttackModifier : CreatureModifier
    {
        public DoubleAttackModifier(Creature creature) : base(creature)
        {
        }

        public override void Handle()
        {
            WriteLine($"Doubling {creature.Name}'s attack");
            creature.Attack *= 2;
            base.Handle();
        }
    }

    // Un altro tipo di carta che permette di aumentare il valore di difesa della creatura
    public class IncreaseDefenseModifier : CreatureModifier
    {
        public IncreaseDefenseModifier(Creature creature) : base(creature)
        {
        }

        public override void Handle()
        {
            WriteLine("Increasing goblin's defense");
            creature.Defense += 3;
            base.Handle();
        }
    }

    public class Demo
    {
        static void Main111(string[] args)
        {
            var goblin = new Creature("Goblin", 2, 2);
            WriteLine(goblin);

            // La catena di responsabilità gestita per il goblin corrente
            var root = new CreatureModifier(goblin);

            // Aggiungiamo alla catena il nostro bonus vuoto per il Next successivo
            root.Add(new NoBonusesModifier(goblin));

            // E anche qui aggiungiamo una potenza di attacco per il succevo next nella catena
            WriteLine("Let's double goblin's attack...");
            root.Add(new DoubleAttackModifier(goblin));

            // E anche qui aggiungiamo una difesa per il succevo next nella catena
            WriteLine("Let's increase goblin's defense");
            root.Add(new IncreaseDefenseModifier(goblin));

            // eventually... ( Alla fine facciamo scattare l'handle del goblic che percorrerà l'intera catena )
            root.Handle();
            WriteLine(goblin);
        }
    }
}