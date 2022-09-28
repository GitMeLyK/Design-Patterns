using System;
using System.Collections.Generic;
using System.Text;
using static System.Console;

namespace DotNetDesignPatternDemos.Behavioral.Strategy.Dynamic
{
    /*
     * Il modello di progettazione Strategy è molto semplice da implementare e tutto
     * cià che si deve fare è togliere una parte di un algoritmo e cercare di sostituire 
     * le diverse implementazioni di quell'algortimo come parte del progetto generale.
     * In questo esempio vedremo come creare una pagina di esempio e contrassegare le
     * pagine in formati. Quindi intendiamo una sorta di elaboratore di testi che ha due
     * formati di output diversi dove avremo al contempo una enumerazione per il formato di 
     * output e due opzioni possibili.
     * Nell'esempio vediamo l'algoritmo espresso nelle carte per comporre le liste quindi
     * a designtime abbiamo queste classi che sanno come accodare gli elementi nell'elenco
     * rispettifamente al proprio formato. Nella classe textprocessor utilizziamo come 
     * nella scelta a runtime vengono usati o il primo specifico metodo o il secondo che
     * istanziano la classe per comporre un elenco, e che in questo modo ci permette di
     * trattare l'output attraverso il metodo classico         
     *      public void SetOutputFormat(OutputFormat format)
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

        // Il primo argomento contiene gli ulteriori tag di aprtura e chiusura
        // ripassando dalla composizione di questo elenco, il secondo è l'oggetto
        // vero e prorio che in questo caso è il contenuto testuale.
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

        // L'elemento inserito nei tag di tipo markdown
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

        // L'elemento inserito nei tag di tipo html
        public void AddListItem(StringBuilder sb, string item)
        {
            sb.AppendLine($"  <li>{item}</li>");
        }
    }

    // Il processore testuale
    public class TextProcessor
    {
        private StringBuilder sb = new StringBuilder();
        private IListStrategy listStrategy;

        // Si definisce di usare l'algoritmo nel formato a runtime
        public void SetOutputFormat(OutputFormat format)
        {
            switch (format)
            {
                case OutputFormat.Markdown:
                    listStrategy = new MarkdownListStrategy();
                    break;
                case OutputFormat.Html:
                    listStrategy = new HtmlListStrategy();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(format), format, null);
            }
        }

        // Il metodo a design time inserirà a prescindere
        // dall'output gli elementi per l'elenco
        public void AppendList(IEnumerable<string> items)
        {
            listStrategy.Start(sb);
            foreach (var item in items)
                listStrategy.AddListItem(sb, item);
            listStrategy.End(sb);
        }

        public StringBuilder Clear()
        {
            return sb.Clear();
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
            var tp = new TextProcessor();

            // Quindi la strategia opera per l'output 1
            tp.SetOutputFormat(OutputFormat.Markdown);
            tp.AppendList(new[] { "foo", "bar", "baz" });
            WriteLine(tp);

            tp.Clear();

            // Quindi la strategia opera per l'output 2
            tp.SetOutputFormat(OutputFormat.Html);
            tp.AppendList(new[] { "foo", "bar", "baz" });
            WriteLine(tp);
        }
    }
}