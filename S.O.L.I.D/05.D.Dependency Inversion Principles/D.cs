using System;
using System.Collections.Generic;
using System.Linq;
using static System.Console;

namespace DotNetDesignPatternDemos.SOLID.DependencyInversionPrinciple
{
    /*
     * Principio di Inversione di Dipendenza.
     * Con questo principio definiamo due regole importanti da tenere a mente
     *  1 I moduli di alto livello non dovrebbero dipendere dal livello basso; 
     *    entrambi dovrebbero dipendere dalle astrazioni
     *  2 Le astrazioni non dovrebbero dipendere dai dettagli; i dettagli dovrebbero 
     *    dipendere dalle astrazioni
     * 
     * Il principio è semplice anche se all'inizio potrebbe sapere di criptico.
     * Nell'esempio vediamo come modellare una relazione genealogica tra persone diverse
     * ed immaginiamo che nel sistema si possono eseguire query nel database genealogico.
     * Quindi nell'esempio abbiamo un enum che identifica il tipo di relazione tra persone
     * enum Relationship -> Genitore Figlio Fratello
     * dopodichè abbiamo una classe che si occupa di tenere in memoria queste relazioni
     * Relationships e infine una classe estrerna che si occupa di usare i metodi per la
     * ricerca delle relazioni Research.
     * Adesso se non usassimo questo principio la classe Research per fare ricerca sulla
     * storage delle relazioni Relationsships si aspetterebbe che nell'argomento venga
     * passato come riferimento un istanza di questo storage per fare la sua query quindi
     * potremmo avere un metodo simile a questo come si vede nell'esempio commentato.:
     *   public Research(Relationships relationships)
     * e quindi che cade il principio per cui la classe di alto livello alto Research deve
     * dipendere dalla classe di livello basso Relationships.
     * Nell'esempio per risolvere questo problema in seguito è stato quindi definita una
     * astrazione tramite un interfaccia IRelationshipBrowser che espone un metodo per cui
     * il metodo deve essere rivelato dal modulo a basso livello per fare quell'operazione
     * in questo caso di query.
     * Quindi vediamo che usando questo principio e tramite questa interfaccia di astrazione
     * IRelationshipBrowser il modulo di  basso livello implementerà questa interfaccia
     * e quindi sarà possibile dai moduli ad alto livello poter accedere al metodo senza
     * dipendere dalla classe Relationships per eseguire il metodo ma solo conoscendo ed 
     * usando questa interfaccia di astrazione, e come vediamo nella seconda funzione di
     * Research quella con questa firma
     *  public Research(IRelationshipBrowser browser)
     * vedaimo che per eseguirla il sistema deve passare solo qualsiasi tipo che implementi 
     * questa astrazione e non l'istanza vera e proria.
     *  Esiste poi la possibilità se poste queste classi di basso livello a liberare il contesto
     *  per le classi ad alto livello dalla dipendenza intrinseca usando appunto l'atrazione
     *  delle interfacce, di operare con Container di Dipendenze, questi operano tramite
     *  un costrutto di associazione e resolver, la prima parte si occupa di contestualizzare
     *  il Tipo associandolo a un Interfaccia per essere in seguito risolto per l'occorrenza
     *  dove serve, e il vantaggio di questo è definire per il sistema un punto unico di
     *  definizione per le nuove istanze dove servono ed auoistanziarsi prima dell'uso
     *  di quella particolare implementazione che riceve come argomento l'interfaccia e non
     *  il tipo.
     */

    // 
    // 

    // Genealogic relation
    public enum Relationship
    {
        Parent,
        Child,
        Sibling
    }

    public class Person
    {
        public string Name;
        // public DateTime DateOfBirth;
    }

    // Interfaccia per l'astrazione che esponse il metodo da usare
    // per la Query chiave secondo questo principio di inversione
    public interface IRelationshipBrowser
    {
        IEnumerable<Person> FindAllChildrenOf(string name);
    }

    // Classe storage per l'insieme delle relazioni
    public class Relationships : IRelationshipBrowser // low-level
    {
        private List<(Person, Relationship, Person)> relations
          = new List<(Person, Relationship, Person)>();

        // Metodo per inserire nell'insieme delle relazioni due relazioni
        // una per il Padre in relazione Verso il Figlio e una Del Figlio
        // in relazione verso il Padre
        public void AddParentAndChild(Person parent, Person child)
        {
            relations.Add((parent, Relationship.Parent, child));
            relations.Add((child, Relationship.Child, parent));
        }


        // Lista di relazioni tra persone diverse usata per la query senza
        // astrazione dove per poter fare query ha bisogno di questo campo pubblico.
        public List<(Person, Relationship, Person)> Relations => relations;

        // ****
        // Metodo implementato per l'astrazione
        // Chiave del metodo di astrazione per non far dipendere dall'intera classe in oggetto
        // con relativa implementazione del caso specifico per la ricerca... ed ottenere il risultato stesso
        public IEnumerable<Person> FindAllChildrenOf(string name)
        {
            return relations
              .Where(x => x.Item1.Name == name
                          && x.Item2 == Relationship.Parent).Select(r => r.Item3);
        }
    }

    // Modulo per la ricerca
    public class Research
    {
        // Dipendente da Relationships direttamente
        public Research(Relationships relationships)
        {
            //

            // high-level: find all of john's children
            //var relations = relationships.Relations;
            //foreach (var r in relations
            //  .Where(x => x.Item1.Name == "John"
            //              && x.Item2 == Relationship.Parent))
            //{
            //  WriteLine($"John has a child called {r.Item3.Name}");
            //}
        }

        // Non dipendente ma usando l'astrazione che ne esponse il metodo da usare
        public Research(IRelationshipBrowser browser)
        {
            foreach (var p in browser.FindAllChildrenOf("John"))
            {
                WriteLine($"John has a child called {p.Name}");
            }
        }

        static void Main(string[] args)
        {
            var parent = new Person { Name = "John" };
            var child1 = new Person { Name = "Chris" };
            var child2 = new Person { Name = "Matt" };

            // low-level module
            var relationships = new Relationships();
            relationships.AddParentAndChild(parent, child1);
            relationships.AddParentAndChild(parent, child2);

            // Non dipendendo da Relationships noi stiamo passando
            // un istanza che è compatibile con l'interfaccia 
            new Research(relationships);

        }
    }
}
