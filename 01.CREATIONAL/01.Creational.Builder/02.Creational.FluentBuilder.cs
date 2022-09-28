using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace DotNetDesignPatternDemos.Creational.FluentBuilder
{

    class HtmlBuilder : DotNetDesignPatternDemos.Creational.Builder.HtmlBuilder
    {

        public HtmlBuilder(string rootName) : base(rootName)
        {
        }

        // E con questo restituiamo this per rendere fluente il codice di creazione del nostro oggetto
        public HtmlBuilder AddChildFluent(string childName, string childText)
        {
            var e = new Builder.HtmlElement(childName, childText);
            root.Elements.Add(e);
            return this;
        }

        public override string ToString()
        {
            return root.ToString();
        }

        public new void Clear()
        {
            root = new Builder.HtmlElement { Name = base.rootName };
        }

        Builder.HtmlElement root = new ();

    }

    public class Demo
    {
        static void Main4(string[] args)
        {
            // if you want to build a simple HTML paragraph using StringBuilder
            var hello = "hello";
            var sb = new StringBuilder();
            sb.Append("<p>");
            sb.Append(hello);
            sb.Append("</p>");
            WriteLine(sb);

            // now I want an HTML list with 2 words in it
            var words = new[] { "hello", "world" };
            sb.Clear();
            sb.Append("<ul>");
            foreach (var word in words)
            {
                sb.AppendFormat("<li>{0}</li>", word);
            }
            sb.Append("</ul>");
            WriteLine(sb);

            // ordinary non-fluent builder
            var builder = new HtmlBuilder("ul");
            builder.AddChild("li", "hello");
            builder.AddChild("li", "world");
            WriteLine(builder.ToString());

            // fluent builder
            sb.Clear();
            builder.Clear(); // disimpegna il costruttore dall'oggetto che sta costruendo, quindi...
            builder.AddChildFluent("li", "hello").AddChildFluent("li", "world");
            WriteLine(builder);
        }
    }
}
