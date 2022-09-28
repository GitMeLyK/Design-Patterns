using System;
using System.Security.Cryptography;
using static System.Console;

namespace DotNetDesignPatternDemos.Behavioral.ChainOfResponsibility.BrokerChain
{
    /*
     * Nell'esempio precedente il modello della Catena di Responsabilità di un ipotetico 
     * gioco di carte che ricevono le Creature del gioco sotto forma di bonus e che ne aumentano
     * il sistema di difesa o di attacco.
     * Questa volta però definito in un contesto dove il CQS quindi il command e la query risultino
     * effettivamente separate per la catena di responsabilità a cui il personaggio del gioco deve
     * essere sottoposto nel contesto dell'intero gioco.
     * Il punto centrale del modello questa volta è il Game, questa classe si occupa di prendere
     * in pancia tutte le query ( catena di eventi per le responsabilità ) ed invocarle per le 
     * Creature al momento della lettura delle proprietà getter per l'attacco e la difesa.
     * Questa classe Game diventa una sorta di mediatore tra la classe Game e la classe Creature,
     * appunto per questo viene usato questo modello Maediator di cui è trattato l'argomento in
     * questa soluzione. La classe Query seaparata dal command che viene invocato dalla classe Game
     * si occupa di essere un tipo predefinito per le query da raccogliere nella catena di responsabiità.
     * Ricapitolando abbiamo il mediatore centrale cioè il Game che si occupa di dare alcune info
     * ai personaggi del gioco che parteciparno fornendo un API di query per richiedere i valori
     * di attacco e difesa del proprio personaggio, e questo metodo viene usato come evento di .net.
     * Dopodichè abbiamo la classe Query per rappresentare il contesto di cui vogliamo interrogare
     * l'elenco delle responsabilità espresse in ModifierCreature per l'attacco e la difesa, cioè 
     * è una classe specializzata a creare una query di comando per l'interrogazione nella lista
     * delle responsabilità del personaggio. E fondamentalmente questi personaggi per i valori di 
     * attacco e difesa che sono proprietà private in quanto non settabili dall'esterno se non con
     * carichi accodati di responsabilità espressi come bonus nelle classe AttackModifier e DefenseModifier
     * che una volta nel gioco saranno interrogati per questa lista di modificatori(responsabilià) e
     * utilizzati per lo scopo del gioco di rappresentare per quella determinata creatura il suo
     * attuale valore di attacco e di difesa calcolato da ogni singolo valore nella lista in cascata.
     * A differenza dell'esempio precedente che predeva come radice (root) la classe non astratta
     * CreatureModifier per la LinkedList dell'insieme delle reponsabilità da applicare qui non esiste
     * un elenco collegato e la classe CreatureModifier è astratta per tutti i modificatori derivanti
     * rendeno un contesto nell'applicare la catena utilizzando il costrutto using(..) nel richiamare
     * il personaggio interrogare il suo grado di potenza innestato dentro altri using(..) e risalire
     * con il dispose per eliminare di volta in volta il contesto di quella responsabilità.
     * Con questo Broker di eventi usando l'annidamento di scopi abbiamo uno dentro l'altro il suseguirsi
     * dei comandi da applicare tenendo conto prima del dispose dei valori presenti nella coda degli eventi.
     */

    // command query separation is being used here

    public class Query
    {
        public string CreatureName;

        public enum Argument
        {
            Attack, Defense
        }

        public Argument WhatToQuery;

        public int Value; // bidirectional

        public Query(string creatureName, Argument whatToQuery, int value)
        {
            CreatureName = creatureName ?? throw new ArgumentNullException(paramName: nameof(creatureName));
            WhatToQuery = whatToQuery;
            Value = value;
        }
    }

    public class Game // mediator pattern
    {
        // La catena degli eventi che susseguiranno l'uno dopo latro per 
        // ottenere il valore rispetto  al momento della richiesta
        public event EventHandler<Query> Queries; // effectively a chain

        public void PerformQuery(object sender, Query q)
        {
            Queries?.Invoke(sender, q);
        }
    }

    public class Creature
    {
        private Game game;
        public string Name;
        // Valori non impostabili direttamente
        private int attack, defense;

        public Creature(Game game, string name, int attack, int defense)
        {
            this.game = game ?? throw new ArgumentNullException(paramName: nameof(game));
            this.Name = name ?? throw new ArgumentNullException(paramName: nameof(name));
            // Solo al momento iniziale
            this.attack = attack;
            this.defense = defense;
        }

        // Comando di Proprietà in lettura dopo l'interrogazione della query
        // che sommerà l'attuale stato di tutte le carte bonus di attacco e 
        // ne preleva il valore.
        public int Attack
        {
            get
            {
                var q = new Query(Name, Query.Argument.Attack, attack);
                game.PerformQuery(this, q);
                return q.Value;
            }
        }

        // Comando di Proprietà in lettura dopo l'interrogazione della query
        // che sommerà l'attuale stato di tutte le carte bonus di difesa e 
        // ne preleva il valore.
        public int Defense
        {
            get
            {
                var q = new Query(Name, Query.Argument.Defense, defense);
                game.PerformQuery(this, q);
                return q.Value;
            }
        }

        public override string ToString() // no game
        {
            // Fa si che leggendo le proprietà scorri attrraverso tutta la catena delle responsabilità 
            // con l'handler degli eventi che si susseguono per ogni responsabilità.
            return $"{nameof(Name)}: {Name}, {nameof(attack)}: {Attack}, {nameof(defense)}: {Defense}";
            //                                                 ^^^^^^^^ using a property    ^^^^^^^^^
        }
    }

    // Attraverso la classe base che istanzia il modificatore
    // verrà accodato all'insieme di query da sottoporre nella
    // catena delle responsabilità per ottenre in seguito nell'evento
    // vero e proprio il calcolo del modificatore (della responsabilità)
    public abstract class CreatureModifier : IDisposable
    {
        protected Game game;
        protected Creature creature;

        protected CreatureModifier(Game game, Creature creature)
        {
            this.game = game;
            this.creature = creature;
            game.Queries += Handle;
        }

        protected abstract void Handle(object sender, Query q);

        // Tutte le volte che si esce dall'istanza dell'oggetto
        // modificatore viene sottratta dalla catena delle responsabilità
        // la query evento precedentemente e in corso utilizzata.
        public void Dispose()
        {
            game.Queries -= Handle;
        }
    }

    public class DoubleAttackModifier : CreatureModifier
    {
        public DoubleAttackModifier(Game game, Creature creature) : base(game, creature)
        {
        }

        // Nella query degli eventi susseguiti questo è l'evento vero 
        // e proprio che si avvia per sommare in quessto caso il valore 
        // di attacco.
        protected override void Handle(object sender, Query q)
        {
            if (q.CreatureName == creature.Name &&
                q.WhatToQuery == Query.Argument.Attack)
                q.Value *= 2;
        }
    }

    public class IncreaseDefenseModifier : CreatureModifier
    {
        public IncreaseDefenseModifier(Game game, Creature creature) : base(game, creature)
        {
        }

        // Nella query degli eventi susseguiti questo è l'evento vero 
        // e proprio che si avvia per sommare in quessto caso il valore 
        // di difesa.
        protected override void Handle(object sender, Query q)
        {
            if (q.CreatureName == creature.Name &&
                q.WhatToQuery == Query.Argument.Defense)
                q.Value += 2;
        }
    }

    public class Demo
    {
        public static void Main(string[] args)
        {
            var game = new Game();
            var goblin = new Creature(game, "Strong Goblin", 3, 3);
            WriteLine(goblin);

            // Nel contesto del gioco aggiungiamo un modificatore (responsabile del valore di attacco)
            using (new DoubleAttackModifier(game, goblin))
            {
                WriteLine(goblin);
                // Nel contesto oltre allatacco aggiungiamo il valore di difesa
                using (new IncreaseDefenseModifier(game, goblin))
                {
                    WriteLine(goblin);
                } // elimina la difesa aggiunta

            }// elimina lattacco aggiunto

            // Ottenisamo lo stato iniziale di 3 in attacco e 3 in difesa.
            WriteLine(goblin);
        }
    }
}