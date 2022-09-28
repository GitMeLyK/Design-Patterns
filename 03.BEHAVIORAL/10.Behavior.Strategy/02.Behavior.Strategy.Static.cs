using System;
using System.Collections.Generic;
using System.Text;
using static System.Console;

namespace DotNetDesignPatternDemos.Behavioral.Strategy.Static
{
    /*
     * Seguendo l'esempio precedente per la strategia di applicare classi
     * separate da istanziare a runtime rispetto alla scelta strategica ha
     * fatto si che vediamo come il designtime del textprocessor tiene in
     * considerazione per l'algoritmo di tenere un elenco indipendente dall'output
     * ci ha peremesso nel sistema di decidere cosa vogliamo in output.
     * In questo esempio che è praticamente identico nel modello strategico
     * di processare il testo per un Output specifico ci siamo avvalsi dei Generics
     * dove abbiamo usato l'accortezza di definire cosa può essere processato e
     * non in modo informale ma con una politica dove obbliga che l'istanza
     * per il textprocessor sia di un tipo compatibile con la trattazione della
     * lista interna degli elementi da renderizzare e quindi definendo che
     * il tipo sia un tipo che implementa solo quel tipo di interfaccia e non altro
     * e che abbia il costruttore di default per autoistanziare il tipo, per cui
     * la classe text processor ha una firma equivalente a 
     *  where LS : IListStrategy, new()
     * che ne definisce appunto la politica di utilizzo previa condizione e in 
     * questo modo il text processor può fare a meno del metodo 
     *      public void SetOutputFormat(OutputFormat format)
     * avendo internamente istanziato il tipo di output corretto tramite l'attributo
     *      private IListStrategy listStrategy = new LS()
     * che resta staticamente disponibile nel contesto del textprocessor per l'output 
     * definito nel tipo di classe da usre per questo generico.
     * Ecco che il modello di Strategia si comleta con i vincoli usati dalla classe Generic
     * e ci permette di usare lo Stesso tipo di Operazione Il TextProcessor per avere
     * due Output differenti.
     */

    // I Due formati possibili in Output
    public enum OutputFormat
    {
        Markdown,
        Html
    }

    // Il modello Strategy prevede un interfaccia di
    // implementazione per avere la possibilità di emettere un elenco nei vari formati.
    // Nel Dml vediamo che composto da tag di apertura contenuto e chiusura
    // <ul><li>Foo</li></ul> quindi è necessario incorporare i tag di apertura
    // e quelli di chiusura e il contenuto item.
    public interface IListStrategy
    {
        void Start(StringBuilder sb);
        void End(StringBuilder sb);
        void AddListItem(StringBuilder sb, string item);
    }

    // L'implementazione strategica per la lista degli
    // elementi dml nel testo processato per ottenere il MarkDown
    public class MarkdownListStrategy : IListStrategy
    {
        public void Start(StringBuilder sb)
        {
            // markdown doesn't require a list preamble
        }

        public void End(StringBuilder sb)
        {

        }

        public void AddListItem(StringBuilder sb, string item)
        {
            sb.AppendLine($" * {item}");
        }
    }

    // L'implementazione strategica per la lista degli
    // elementi dml nel testo processato per ottenere l'Html
    public class HtmlListStrategy : IListStrategy
    {
        public void Start(StringBuilder sb)
        {
            sb.AppendLine("<ul>");
        }

        public void End(StringBuilder sb)
        {
            sb.AppendLine("</ul>");
        }

        public void AddListItem(StringBuilder sb, string item)
        {
            sb.AppendLine($"  <li>{item}</li>");
        }
    }

    // a.k.a. policy
    public class TextProcessor<LS> where LS : IListStrategy, new()
    {
        private StringBuilder sb = new StringBuilder();
        private IListStrategy listStrategy = new LS();

        public void AppendList(IEnumerable<string> items)
        {
            listStrategy.Start(sb);
            foreach (var item in items)
                listStrategy.AddListItem(sb, item);
            listStrategy.End(sb);
        }

        public override string ToString()
        {
            return sb.ToString();
        }
    }

    class Demo
    {
        static void Main(string[] args)
        {
            // Il textprocessor generico per il MarkDown
            var tp = new TextProcessor<MarkdownListStrategy>();
            tp.AppendList(new[] { "foo", "bar", "baz" });
            WriteLine(tp);

            // La politica e il modello strategy in questo modo
            // impedisce la riassegnazione all'istanza tp di essere
            // riassegnata come nuovo modello strategico, quindi 
            // incorriamo in errore se provassimo a fare.:
            //      tp = new TextProcessor<HtmlListStrategy>();
            // in quanto già definita nella Strategia del trattamento
            // dell'output definito con i vincoli della classe Generica.
            // Come non si può neanche fare un istanza non definita da
            // usare tramite una variabile del tipo
            //  TextProcessor<IListStrategy>
            // in quanto deve prevedere un costruttore di default new()
            // dal vincolo della politica del TextProcessor Generico.


            // Il textprocessor generico per l' Html, in questo
            // caso può diventare un altro tipo di output perchè
            // viene definita una nuova istanza di un oggetto a quella
            // nuova strategia da trattare.
            var tp2 = new TextProcessor<HtmlListStrategy>();
            tp2.AppendList(new[] { "foo", "bar", "baz" });
            WriteLine(tp2);

            // Per l'uso tramite DI si potrebbe pensare ad usare l'istanza
            // del processore con il tipo previsto in questo modo.:
            //  cb.register<MarkDownListStrategy>().As<IListStrategy>()
            // e in questo modo ha un punto centrale nel contenitore per
            // usare in modo dinamico e che puoi in qualsiasi momento passare
            // da uno all'altro

        }
    }
}