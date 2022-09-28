using static System.Console;

namespace DotNetDesignPatternDemos.Behavioral.TemplateMethod.Functional
{
    using System;

    /*
     * Rispetto all'esempio precedente che come prevede il modello Template
     * si ha una classe base dove si eredita l'aspetto quindi si definisce lo scheletro
     * e poi si implmenetao le classi concrete. In questo esempio viene usato
     * un modo di programmazione alternativo quello Funzionale, dove la classe base
     * sarà una classe statica che expone il modello quindi lo scheletro applicativo,
     * ma non sarà presente una classe concreta per usare il modello, piuttosto saranno
     * passate al costruttore tutti i metodi richiesti per il Run che deve orchestrare
     * su questi metodi per avviare l'algoritmo del gioco in essere.
     * L'aspetto funzionale che passa queindi le Action o le Func<> come funzioni da usare
     * vengono impilate nel run come richiesto e avviano le fasi del gioco.
     */


    // Classe Model 
    public static class GameTemplate
    {

        // Il metodo principale oggetto dell'orchestrazione del gioco
        // che è fondamentale per definire il comportamento dell'algoritmo
        // del gioco ma in questo caso con tutte le funzioni passate come
        // argomenti necessarie ad essere usate nel sistema che sta definendo
        // il comportamento concreto del gioco da fare.
        public static void Run(
          Action start,
          Action takeTurn,
          Func<bool> haveWinner,
          Func<int> winningPlayer)
        {
            start();
            while (!haveWinner())
                takeTurn();
            WriteLine($"Player {winningPlayer()} wins.");
        }
    }

    public class Demo2
    {
        static void Main(string[] args)
        {
            // Quelle che erano le variabili del template
            // o delle classi concrete ereditate dal Modello
            // vengono dichiarate al momento
            var numberOfPlayers = 2;
            int currentPlayer = 0;
            int turn = 1, maxTurns = 10;

            // La funzione di start da passare come argomento funzionale
            void Start()
            {
                WriteLine($"Starting a game of chess with {numberOfPlayers} players.");
            }

            // La funzione di HaveWinner da passare come argomento funzionale
            bool HaveWinner()
            {
                return turn == maxTurns;
            }

            // La funzione di Cambio TUrno da passare come argomento funzionale
            void TakeTurn()
            {
                WriteLine($"Turn {turn++} taken by player {currentPlayer}.");
                currentPlayer = (currentPlayer + 1) % numberOfPlayers;
            }

            // La funzione di vittoria del giocatore da passare come argomento funzionale
            int WinningPlayer()
            {
                return currentPlayer;
            }

            // E in modo stgatico e con passaggio delle funzioni otteniamo il gioco
            // specifico dichiarato qua dentro.
            GameTemplate.Run(Start, TakeTurn, HaveWinner, WinningPlayer);
        }
    }
}