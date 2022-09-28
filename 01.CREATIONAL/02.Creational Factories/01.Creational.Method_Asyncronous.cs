using System;
using System.Threading.Tasks;
using static System.Console;

namespace DotNetDesignPatternDemos.Creational.Factories.Asyncronous
{

    public class Foo
    {
        private Foo()
        {
            //await Task.Delay(1000); // Semplicemente non lo puoi fare
        }

        // Usi un inizializzatore
        public async Task<Foo> InitAsync()
        {
            await Task.Delay(1000);
            return this;
        }

        // Factory (abbiamo reso il costruttore privato)
        public static Task<Foo> CreateFoo()
        {
            // Semplicemente usiamo ed annotiamo una sola volta il modo 
            // in cui costrutire e inizializzare il metodo async per foo
            var result = new Foo();
            return result.InitAsync(); // usiamo l'inziaizlizatore del task solo qui
        }


    }

    class Demo
    {
        static async Task Main(string[] args)
        {

            // Nel caso reale senza factory...
            // var foo = new Foo();
            //await foo.InitAsync(); // Nel caso reale dobbiamo sempre ricordarci di usare l'inizilizzatore

            //.. altrimeni ovviamo con il Facotry.
            var foo = await Foo.CreateFoo();

            WriteLine(foo);
        }
    }
}
