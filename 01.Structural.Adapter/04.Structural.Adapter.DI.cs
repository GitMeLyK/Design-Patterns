using System;
using System.Collections.Generic;
using Autofac;
using Autofac.Features.Metadata;

namespace DotNetDesignPatternDemos.Structural.Adapter.DI
{

    /*
     * In questo esempio vediamo un tipo particolare di adapter per lo specifico usando le DI
     * per utilizzare contesti dove l'adapter server in una determinata classe di istanza.
     * Vediamo in questo esempio come l'ausilio della DI nel conainer ha necessiarmente 
     * bisogno di regsitrare il tipo con l'implementazione di interfaccia con il comando
     * RegisterAdapter<IType,Type> perchè l'adapter possa funzionare correttamente nell'iniettarlo
     * al momento della classe che ne fa uso. Inoltre essendo stati utilizzati due adapter per 
     * lo specifico due tipi di bottoni da inrerire nel contesto dell'editor, se non utilizziamo
     * questo speciale tipo di registeradapter nel cointainer li vedrebbe come un singolo bottone
     * nell'insieme dell'ditor e non come due registrazioni diverse adattate alla stessa interfaccia
     * per il resolver delle istanze. Oltre questo è stato usato nel contesto del registeradapter
     * la generic di tipo Meta<> che definisce attributi ulteriore da trattare per ogni tipo di
     * bottone che ne usano lo stesso attributo in questo case Name.
     */


    public interface ICommand
    {
        void Execute();
    }

    // Un tipo di Command da usare
    public class SaveCommand : ICommand
    {
        public void Execute()
        {
            Console.WriteLine("Saving current file");
        }
    }

    // Un altro tipo di Command da usare
    public class OpenCommand : ICommand
    {
        public void Execute()
        {
            Console.WriteLine("Opening a file");
        }
    }

    // Il nostro Adapter che ridefinisce i command come button
    // da inserire nel probabile editor
    public class Button
    {
        private readonly ICommand command;

        // Il Metadato attribuito alla classe Buttton Adapter
        private readonly string name;

        // Il costruttore di default per l'istanza del Button
        // che adatta il Command come funzionalità del Button
        // e classificato con un ulteriore Metadato in questo caso il Name.
        public Button(ICommand command, string name)
        {
            if (command == null)
            {
                throw new ArgumentNullException(paramName: nameof(command));
            }
            this.command = command;
            this.name = name;
        }

        // Il Comando Riadattato
        public void Click()
        {
            command.Execute();
        }

        // Una funzionalità ulteriore non espressamente
        // del command ma di questo più specializzato per
        // l'editor di tipo Button, che fa uso del metadato
        // per fare in questo caso il print del suo nome.
        public void PrintMe()
        {
            Console.WriteLine($"I am a button called {name}");
        }
    }

    // La destinazione dei Button riadattati per l'editor
    // che fanno uso internamente dei propri command innestati.
    public class Editor
    {
        private readonly IEnumerable<Button> buttons;

        public IEnumerable<Button> Buttons => buttons;

        public Editor(IEnumerable<Button> buttons)
        {
            this.buttons = buttons;
        }

        public void ClickAll()
        {
            foreach (var btn in buttons)
            {
                btn.Click();
            }
        }
    }

    public class Adapters
    {
        static void Main_(string[] args)
        {
            // for each ICommand, a ToolbarButton is created to wrap it, and all
            // are passed to the editor
            var b = new ContainerBuilder();

            // Per il DI e regsitrare i Command a livello
            // applicativo dove definiamo al momento del resolver
            // gli attributi della classe adapter tramite il suo
            // Nome.
            b.RegisterType<OpenCommand>()
              .As<ICommand>()
              .WithMetadata("Name", "Open");
            b.RegisterType<SaveCommand>()
              .As<ICommand>()
              .WithMetadata("Name", "Save");
            //b.RegisterType<Button>();

            // qui utilizziamo lo speciale register di tipo adapter per il container
            // affinchè risolva che questa classe Button funga da Adapter per i Command
            // utilizzati e istanziati all'interno.
            b.RegisterAdapter<ICommand, Button>(cmd => new Button(cmd, ""));
            
            // E qui che essendo i due componenti adattatore per essere usati nell'editor
            // e che usano la stessa interfaccia con attributi della stessa definizione,
            // attributi che sono intesi nel contesto di un adattatore come metadati del
            // componente da adattare, e viene in soccorso la speciale definizione generica
            // del Metadato.
            b.RegisterAdapter<Meta<ICommand>, Button>(cmd =>
              new Button(cmd.Value, (string)cmd.Metadata["Name"]));
            
            b.RegisterType<Editor>();
            
            using (var c = b.Build())
            {
                var editor = c.Resolve<Editor>();
                editor.ClickAll();

                // problem: only one button se non ci fosse la registeradapter
                foreach (var btn in editor.Buttons)
                    btn.PrintMe();


            }
        }
    }
}