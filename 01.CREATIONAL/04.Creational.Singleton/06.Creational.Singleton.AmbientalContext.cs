using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetDesignPatternDemos.Creational.Singleton.AmbientStack
{
    /*
     * In questo caso utilizziamo una Singola Istanza con Contesto Ambientale
     * nell'esempio per dare un idea chiara viene utilizzata l'altezza del muro
     * che peer tutti gli ambienti è sempre uguale finche è in quell'ambente.
     * Il problema si pone se l'applicazione gira con più thread e si incorre nel
     * pericolo utilizzando l'attributo statico di non usare in Contesto appropriato
     * in questo caso per l'altezza del muro,,, quindi la classe di contesto deve girare
     * per un perido che si utilizza quella variabile static in un contesto e lo si fa
     * usando lo stantment using {} nel contesto e rendendo la classe statica di contesto
     * disposable...
     * Anche se con il Singleton qui non troviamo un vero e proprio uso di classi statiche
     * stiamo usando la pila di stato per gli ambienti di scopo e finalità in un singolo Thread
     * che fa cose, e questo è utile per ovvi motivi in quanto non permette di sovrapporre
     * valori di stato per l'ambiente statici di una classe che ne fa uso.
     */


    // non-thread-safe global context
    public sealed class BuildingContext : IDisposable
    {
        //public static int WallHeight; // diventa Stack<BuildingContext> stack...

        public int WallHeight = 0;
        public int WallThickness = 300; // etc.
        private static Stack<BuildingContext> stack
          = new Stack<BuildingContext>();

        // Quindi il costruttore inserirà nello stack
        // l'uso di quell'attributo di contesto pushandolo
        static BuildingContext()
        {
            // ensure there's at least one state
            stack.Push(new BuildingContext(0));
        }

        // O se passato come valore di altezza pushiamo l'intero stato di attributi
        public BuildingContext(int wallHeight)
        {
            WallHeight = wallHeight;
            stack.Push(this);
        }

        // Per avere il riferimento a quello stato nella pila lo facciamo da qui
        public static BuildingContext Current => stack.Peek();

        // e lo rimuoverà nel dispose alla fine dell'ambiente di stato
        public void Dispose()
        {
            // not strictly necessary
            if (stack.Count > 1)
                stack.Pop();
        }
    }

    public class Building
    {
        public readonly List<Wall> Walls = new List<Wall>();

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var wall in Walls)
                sb.AppendLine(wall.ToString());
            return sb.ToString();
        }
    }

    public struct Point
    {
        private int X, Y;

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return $"{nameof(X)}: {X}, {nameof(Y)}: {Y}";
        }
    }

    public class Wall
    {
        public Point Start, End;
        public int Height;

        public const int UseAmbient = Int32.MinValue;

        // public Wall(Point start, Point end, int elevation = UseAmbient)
        // {
        //   Start = start;
        //   End = end;
        //   Elevation = elevation;
        // }

        public Wall(Point start, Point end)
        {
            Start = start;
            End = end;
            //Elevation = BuildingContext.Elevation;
            Height = BuildingContext.Current.WallHeight;
        }

        public override string ToString()
        {
            return $"{nameof(Start)}: {Start}, {nameof(End)}: {End}, " +
                   $"{nameof(Height)}: {Height}";
        }
    }

    public class AmbientContextDemo
    {
        public static void Main(string[] args)
        {
            var house = new Building();

            // ground floor
            //var e = 0;
            house.Walls.Add(new Wall(new Point(0, 0), new Point(5000, 0)/*, e*/));
            house.Walls.Add(new Wall(new Point(0, 0), new Point(0, 4000)/*, e*/));

            // first floor
            //e = 3500;
            using (new BuildingContext(3500))
            {
                house.Walls.Add(new Wall(new Point(0, 0), new Point(5000, 0) /*, e*/));
                house.Walls.Add(new Wall(new Point(0, 0), new Point(0, 4000) /*, e*/));

                // stack inner
                using (new BuildingContext(3000))
                {
                    house.Walls.Add(new Wall(new Point(0, 0), new Point(3000, 0) /*, e*/));
                    house.Walls.Add(new Wall(new Point(0, 0), new Point(0, 3000) /*, e*/));
                }

                // return to ambient with 3500 height
                house.Walls.Add(new Wall(new Point(0, 0), new Point(0, 2000) /*, e*/));

            }

            // back to ground again
            // e = 0;
            house.Walls.Add(new Wall(new Point(5000, 0), new Point(5000, 4000)/*, e*/));

            Console.WriteLine(house);
        }
    }
}