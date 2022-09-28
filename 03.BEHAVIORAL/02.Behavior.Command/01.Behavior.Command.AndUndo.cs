using System;
using System.Collections.Generic;
using System.Linq;
using static System.Console;

namespace DotNetDesignPatternDemos.Behavioral.Command.CommandAndUndo
{
    /*
     *  Questo esempio già usato in altri contesti di questa soluzione veniva 
     *  usato come conto bancario con azioni di deposito e prelievo. In questo
     *  oltre a fare queste operazioni aggiungiamo il contesto di Command.
     *  Questo differisce da quello precedente che conteneva solo i metodi
     *  all'interno dell'oggetto per essere eseguiti. Significa in parole povere
     *  che l'azione veniva eseguita e ne perdevamo la corrrispondenza di cosa
     *  si era fatto. In questo caso vediamo che l'azione attraverso una classe
     *  Command opera nel contesto dell'esecuzione e lavora come fosse un oggetto
     *  da eseguire e cioè che ha in sè tutte le informazioni che gli servono per
     *  essere istanziato ed eseguito.
     *  Come si vede nell'esempio Ogni singolo comando inserito nella lista verrà
     *  eseguito al momento successivo perchè in sè ha tutto quello che serve già
     *  come oggetto di comando per esegure quella determinata azione.
     *  In particolare lo stesso comando ha in sè anche il modo tramite le stesse info
     *  di sapere come ripristinare l'operazione appena effettuata.
     */


    public class BankAccount
    {
        private int balance;
        private int overdraftLimit = -500;

        public void Deposit(int amount)
        {
            balance += amount;
            WriteLine($"Deposited ${amount}, balance is now {balance}");
        }

        public bool Withdraw(int amount)
        {
            if (balance - amount >= overdraftLimit)
            {
                balance -= amount;
                WriteLine($"Withdrew ${amount}, balance is now {balance}");
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            return $"{nameof(balance)}: {balance}";
        }
    }

    // L'interfaccia da implementare per i comandi
    // disponibili da eseguire nel contesto.
    public interface ICommand
    {
        void Call();
        void Undo();
    }

    // Il Command per l'operazione di deposito e prelievo
    // usato per l'appunto come oggetto che tiene le informazioni
    // relative al comando da eseguire.
    public class BankAccountCommand : ICommand
    {
        private BankAccount account;

        public enum Action
        {
            Deposit, Withdraw
        }

        private Action action;
        private int amount;
        private bool succeeded;

        // L'istanza del comando che ha in se adesso il tipo di azione da fare
        // l'importo da considerare e per quale account usarlo nella banca.
        public BankAccountCommand(BankAccount account, Action action, int amount)
        {
            this.account = account ?? throw new ArgumentNullException(paramName: nameof(account));
            this.action = action;
            this.amount = amount;
        }

        // E' qui che viene eseguito il comando corrente 
        // definendo argomenti aggiuntivi dopo l'operazione
        // come lo stato si success o altro.
        public void Call()
        {
            switch (action)
            {
                case Action.Deposit:
                    account.Deposit(amount);
                    succeeded = true;
                    break;
                case Action.Withdraw:
                    succeeded = account.Withdraw(amount);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        // Ed è qui che lo stesso comando che ha in se
        // tutte le informazioni per essre ripristinato
        // esegue l'operazione al contrario per annullare
        // l'imporito rispetto a se stesso quando ha fatto il call.
        public void Undo()
        {
            if (!succeeded) return;
            switch (action)
            {
                case Action.Deposit:
                    account.Withdraw(amount);
                    break;
                case Action.Withdraw:
                    account.Deposit(amount);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    class Demo
    {
        static void Main199(string[] args)
        {
            var ba = new BankAccount();

            // L'elenco dei comandi in successione da eseguire per effettuare
            // le operazioni del caso
            List<BankAccountCommand> commands = new() 
              {
                new BankAccountCommand(ba, BankAccountCommand.Action.Deposit, 100),
                new BankAccountCommand(ba, BankAccountCommand.Action.Withdraw, 1000)
              };

            WriteLine(ba);

            // Si eseguiranno le operazioni in coda
            foreach (var c in commands)
                c.Call();

            WriteLine(ba);

            // Si eseguiranno nuovamente le operazioni in coda per l'undo
            // (che equivale a rimettere o togliere gli importi delle operazioni precedenti)
            foreach (var c in Enumerable.Reverse(commands))
                c.Undo();

            WriteLine(ba);
        }
    }
}