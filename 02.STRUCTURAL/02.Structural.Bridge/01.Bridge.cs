using Autofac;
using static System.Console;

namespace DotNetDesignPatternDemos.Structural.Bridge
{

    /*
     * In questo esempio come vedremo per utilizzare un buon pattern bridge
     * facciamo si che la gerarchia dei vettori di tipo Circle Square etc che 
     * derivano tutti da un classe Shape e i due tipi di render VectorRender
     * e RasteerRender che implenetano il metodo comune RenderCircle o altri
     * Renderquare etc... non vengano gerarchicamente uno sotto l'altro ma
     * aggregati tramite la classe Bridge per ottenere le combinazioni.
     * Quindi possiamo dire che usando l'aggregazione possiamo avere i riferimenti
     * interni che puntano nella classe base portandoli a essere indipendenti
     * all'esterno e quindi disaccoppiando il contesto evitando cosi' inutili
     * gerarchie di classi ereditate per fare compiti diversi.
     * Dopodichè nell'esempio sotto si posono vedere i due modi per creare le 
     * istanze e dettare nel bridge il contesto di dominio in modo classico e
     * tramite dipendence injection
     */

    public interface IRenderer
    {
        void RenderCircle(float radius);
    }

    public class VectorRenderer : IRenderer
    {
        public void RenderCircle(float radius)
        {
            WriteLine($"Drawing a circle of radius {radius}");
        }
    }

    public class RasterRenderer : IRenderer
    {
        public void RenderCircle(float radius)
        {
            WriteLine($"Drawing pixels for circle of radius {radius}");
        }
    }

    public abstract class Shape
    {
        protected IRenderer renderer;

        // a bridge between the shape that's being drawn an
        // the component which actually draws it
        public Shape(IRenderer renderer)
        {
            this.renderer = renderer;
        }

        public abstract void Draw();
        public abstract void Resize(float factor);
    }

    // La classe principale che prende in pasto
    // i vari tipi di render nell'istanza della classe
    // render iniettata per quello scopo.
    public class Circle : Shape
    {
        private float radius;

        public Circle(IRenderer renderer, float radius) : base(renderer)
        {
            this.radius = radius;
        }

        public override void Draw()
        {
            renderer.RenderCircle(radius);
        }

        public override void Resize(float factor)
        {
            radius *= factor;
        }
    }

    public class Demo
    {
        static void Main(string[] args)
        {
            // Classic
            //var raster = new RasterRenderer();
            //var vector = new VectorRenderer();
            //var circle = new Circle(vector, 5, 5, 5);
            //circle.Draw();
            //circle.Resize(2);
            //circle.Draw();

            // Use Containe and Dipendence Injiection with Autofac
            var cb = new ContainerBuilder();

            // Per questo scopo vogliamo adottare Il VectorRender
            cb.RegisterType<VectorRenderer>().As<IRenderer>();
            
            // E utilizzare il render iniettato per disegnare nel
            // dispositvo appropriato il cerchio in questo caso.
            cb.Register((c, p) => new Circle(c.Resolve<IRenderer>(),
              p.Positional<float>(0)));

            // Con il Buil l'istanza del render di cui ci 
            // avvaliamo sarà risolta dal container iniettando
            // quella che abbiamo registrato per adottarla e
            // disegnerà con il metodo Draw appropriato sul 
            // dispositivo di render che abbiamo all'interno della
            // classe registrato e referenziato in questo caso per il VectorRender
            using (var c = cb.Build())
            {
                var circle = c.Resolve<Circle>(
                  new PositionalParameter(0, 5.0f)
                );
                circle.Draw();
                circle.Resize(2);
                circle.Draw();
            }
        }
    }
}