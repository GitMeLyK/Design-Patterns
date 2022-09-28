using System.Collections.Generic;
using static System.Console;

namespace DotNetDesignPatternDemos.Behavioral.Memento.UndoRedo
{
    /*
     * Rispetto all'esempio precedente qui prendiamo in considerazione
     * l'idea di usare non il solo Restore per ripristinare al momento
     * tramite l'oggetto di stato precedentemente salvato in un deterimanto
     * periodo dell'esecuzione, ma si applicano i metodi Undo e Redo dell'operazione
     * avendo una lista di snapshot di stati alimentata per ogni azione di deposito
     * e prelievo che ne conserva i cambiamenti List<Memento> changes.
     * Con il Restore allo stato passato precedente questa volta viene anche lui accodato
     * alla lista degli snapshots degli stati precedenti perchè ha comportato un cambiamento
     * e l'undo invece prende dalla lista tramite il conteggio del numero dei cambiamenti
     * quello precedente e ripristina i valori del momento, mentre il redo tiene in considerazione
     * se siamo in uno stato di undo precedente e può riportarsi a quello succevvio dove si è 
     * fatto undo dell'operazione.  
     */

    // La classe oggetto che rappresenta i valori di stato
    // da usare come oggetto di ritorno al mittente.
    public class Memento
    {
        public int Balance { get; }

        public Memento(int balance)
        {
            Balance = balance;
        }
    }

    public class BankAccount
    {
        private int balance;
        // Snapshots di tutti i cambiamenti per ogni azione avvenuta su un determinato cambiamento
        private List<Memento> changes = new List<Memento>();
        private int current;

        public BankAccount(int balance)
        {
            this.balance = balance;
            // Accodiamo questo ulteriore cambio dell'oggetto
            changes.Add(new Memento(balance));
        }

        public Memento Deposit(int amount)
        {
            balance += amount;
            var m = new Memento(balance);
            // Accodiamo questo ulteriore cambio dell'oggetto
            changes.Add(m);
            // Incrementiamo il numero dei cambiamenti avvenuti
            ++current;
            return m;
        }

        public void Restore(Memento m)
        {
            if (m != null)
            {
                // Ripristiniamo il momento dello stato che è stato passato
                balance = m.Balance;
                // E definiamo che anche questo è un nuovo snapshot di cambiamento
                changes.Add(m);
                // E il numero delle operazioni è tornato indietro di uno
                current = changes.Count - 1;
            }
        }

        public Memento Undo()
        {
            if (current > 0)
            {
                // Riprendiamo tra gli snapshot lo stato precedente
                // e decrementiamo il current di 1 che definisce il numero di cambiamenti
                var m = changes[--current];
                // e riprisitiamo il momento storico
                balance = m.Balance;
                return m;
            }
            return null;
        }

        public Memento Redo()
        {
            if (current + 1 < changes.Count)
            {
                // Riprendiamo lo snapshot successivo in elenco dello stato che ha avuto un undo
                // e incrementiamo il current di una operazione fatta
                var m = changes[++current];
                // e rirpristiniamo l'undo precedente
                balance = m.Balance;
                return m;
            }
            return null;
        }

        public override string ToString()
        {
            return $"{nameof(balance)}: {balance}";
        }
    }

    public class Demo
    {
        static void Main(string[] args)
        {
            var ba = new BankAccount(100);
            ba.Deposit(50);
            ba.Deposit(25);
            WriteLine(ba);

            ba.Undo();
            WriteLine($"Undo 1: {ba}");
            ba.Undo();
            WriteLine($"Undo 2: {ba}");
            ba.Redo();
            WriteLine($"Redo 2: {ba}");
        }
    }
}