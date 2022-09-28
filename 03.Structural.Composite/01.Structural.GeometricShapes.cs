using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace DotNetDesignPatternDemos.Structural.Composite.GeometricShapes
{
    /*
     * In questo eesempio vediamo che un ambiente grafico è spesso coinvolto
     * in quelle che sono gl aspetti di un pattern Composito dove diversi 
     * oggetti di natura diversa vengono accoumnati in questo caso in un
     * reggruppamento per essere poi aggiornati negli atrbiuti comuni a 
     * tutto l'insieme, cioè all'aggregato.
     * 
     * */


    // Quindi gli oggetti erederanno da questa classe di aggregazione
    // che al suo interno gestisce la collection degli oggetti d'insieme.
    public class GraphicObject
    {
        public virtual string Name { get; set; } = "Group";
        public string Color;
        private Lazy<List<GraphicObject>> children = new Lazy<List<GraphicObject>>();
        public List<GraphicObject> Children => children.Value;

        private void Print(StringBuilder sb, int depth)
        {
            sb.Append(new string('*', depth))
              .Append(string.IsNullOrWhiteSpace(Color) ? string.Empty : $"{Color} ")
              .AppendLine($"{Name}");
            // e printo nell'aggregato scorrendo l'intero
            // albero di oggetti dinsieme compresi nella collection.
            foreach (var child in Children)
                child.Print(sb, depth + 1);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            Print(sb, 0);
            return sb.ToString();
        }
    }

    public class Circle : GraphicObject
    {
        public override string Name => "Circle";
    }

    public class Square : GraphicObject
    {
        public override string Name => "Square";
    }

    public class Demo
    {
        static void Main(string[] args)
        {
            // E come si vede l'aggragato sarà riempito da
            // più oggetti che erediteranno da esso stesso
            var drawing = new GraphicObject { Name = "My Drawing" };
            drawing.Children.Add(new Square { Color = "Red" });
            drawing.Children.Add(new Circle { Color = "Yellow" });

            var group = new GraphicObject();
            group.Children.Add(new Circle { Color = "Blue" });
            group.Children.Add(new Square { Color = "Blue" });
            drawing.Children.Add(group);

            // e 
            WriteLine(drawing);
        }
    }
}
