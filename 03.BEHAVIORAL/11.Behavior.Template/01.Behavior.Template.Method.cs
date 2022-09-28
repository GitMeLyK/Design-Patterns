using static System.Console;

namespace DotNetDesignPatternDemos.Behavioral.TemplateMethod
{

    /*
     * Nel modello di progettazione Template viene accorpato l'insieme delle
     * funzioni da implementare come scheletro per le classi concrete, e quindi
     * abbiamo la classe base che funge appunto da modello per le classi derivate.
     * In questo esempio la classe che usa questo modello di progettazione è la
     * classe astratta Game che ha lo scheletro per le classi concrete che vogliono
     * implementare un qualche tipo di gioco. Quindi Abbiamo un template che è la base
     * e tutti i metodi astratti che devono essere presenti nelle classi derivate per
     * lo schema quindi l'algoritmo del gioco, come ad esempio il cambio turno la proprietà
     * se è il vincitore e tutto ciò che è poi logica dello specifico gioco da implementare.
     * Ma per il modello template la cosa importante è che il metodo che comunque deve sollevare
     * le classi specifiche dall'orchestrare le situazioni di gioco quindi il loro algoritmo
     * deve essere orchestrato direttamente e il più possibile nella classe template di base Game.
     * In questo caso il Run è operativo e viene eseguito nella classe Base, infatti il metodo
     * userà i metodi implementati delle classi derivate per azionare il gioco cambiare turno
     * e proclamare il vincitore.
     */

    // Classe Model 
    public abstract class Game
    {

        // Il metodo principale oggetto dell'orchestrazione del gioco
        // che è fondamentale per definire il comportamento dell'algoritmo
        // del gioco per tutti allo stesso modo, per tutti intendiamo tutte 
        // le tipologie di gioco che derivano da questo template di base.
        public void Run()
        {
            Start();
            while (!HaveWinner)
                TakeTurn();
            WriteLine($"Player {WinningPlayer} wins.");
        }

        protected abstract void Start();
        protected abstract bool HaveWinner { get; }
        protected abstract void TakeTurn();
        protected abstract int WinningPlayer { get; }

        // Eventuali stati del gioco se non specifici
        // del gioco derivato possono essere usate come
        // attributi della classe base corrente.
        protected int currentPlayer;
        protected readonly int numberOfPlayers;

        public Game(int numberOfPlayers)
        {
            this.numberOfPlayers = numberOfPlayers;
        }
    }

    // Classe concreta con l'implmenetazione dello scheletro
    // applicativo.
    // simulate a game of chess
    public class Chess : Game
    {

        // La base individua per l'algoritmo il numero 
        // di giocatori previsti per il gico degli schacchi 
        // implementato in qusta specifica classe
        public Chess() : base(2)
        {
        }

        // L'insieme dei metodi astratti implementati per questo tipo di gioco

        // Lo start è parte dell'algoritmo del gioco base
        protected override void Start()
        {
            WriteLine($"Starting a game of chess with {numberOfPlayers} players.");
        }

        // La vincita è parte dell'algoritmo del gioco base
        protected override bool HaveWinner => turn == maxTurns;

        // Il cambio turno è parte dell'algoritmo del gioco base
        protected override void TakeTurn()
        {
            WriteLine($"Turn {turn++} taken by player {currentPlayer}.");
            currentPlayer = (currentPlayer + 1) % numberOfPlayers;
        }

        // Il vincitore è parte dell'algoritmo del gioco base
        protected override int WinningPlayer => currentPlayer;

        // Le variabili interne usate solo da questo schema di gioco implementato
        private int maxTurns = 10;
        private int turn = 1;
    }

    public class Demo
    {
        static void Main(string[] args)
        {
            var chess = new Chess();
            chess.Run();
        }
    }
}