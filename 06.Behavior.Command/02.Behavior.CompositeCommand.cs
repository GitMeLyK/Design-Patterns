using System;
using System.Collections.Generic;
using System.Linq;
using static System.Console;

namespace DotNetDesignPatternDemos.Behavioral.Command.Composite
{
    /*
     *  Parlando del modello Command e di come in definitiva ha la 
     *  possibilità di avere una lista di comandi da eseguire al momento
     *  giusto non possiamo non parlare dello stesso modello e applicare
     *  un modello composito. Questo è rilevante in quanto si trova in molti
     *  programmi che utilizzano questo modello.
     *  Nell'esempio come anche in una situazione reale vogliamo fare in modo
     *  che un conto bancario abbia la possibilità di effettuare dei trasferimenti
     *  da un conto all'altro.
     */

    public class BankAccount
    {
        private int balance;
        private int overdraftLimit = -500;

        public BankAccount(int balance = 0)
        {
            this.balance = balance;
        }

        public void Deposit(int amount)
        {
            balance += amount;
            Console.WriteLine($"Deposited ${amount}, balance is now {balance}");
        }

        public bool Withdraw(int amount)
        {
            if (balance - amount >= overdraftLimit)
            {
                balance -= amount;
                Console.WriteLine($"Withdrew ${amount}, balance is now {balance}");
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


    // Il Command da implementare
    public abstract class Command
    {
        public abstract void Call();
        public abstract void Undo();
        public bool Success;
    }


    // La classe command che avrà in sè tutte le informazioni
    // riguardo all'operazione da esegure per il deposito e il prelievo
    public class BankAccountCommand : Command
    {
        private BankAccount account;

        public enum Action
        {
            Deposit, Withdraw
        }

        private Action action;
        private int amount;
        private bool succeeded;

        public BankAccountCommand(BankAccount account, Action action, int amount)
        {
            this.account = account;
            this.action = action;
            this.amount = amount;
        }

        // L'azione vera e propria da eseguire al momento richiesto
        public override void Call()
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

        // La stessa operazione con il trattamento delle info per ripristinare
        // gli importi rispetto a queando ha fatto l'operazione di prelievo o deposito.
        public override void Undo()
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

    // La classe base di tipo composito che comportandosi come una Lista
    // invece che operare su ogni singola azione opererà per tutto l'insieme
    // Combinazione di comandi
    abstract class CompositeBankAccountCommand : List<BankAccountCommand>, ICommand
    {
        // Ogni command nella lista interna del modello composito
        // della classe veràà eseguito in successione.
        public virtual void Call()
        {
            // Cicla su se stesso essendo una lista
            ForEach(cmd => cmd.Call());
        }

        // Ogni command nella lista interna del modello composito
        // della classe veràà eseguito in successione inversa per 
        // ottenere l'undo del command composito corrente.
        public virtual void Undo()
        {
            // Cicla su stesso essendo una lista ma in reverse.
            foreach (var cmd in
              ((IEnumerable<BankAccountCommand>)this).Reverse())
            {
                cmd.Undo();
            }
        }

    }

    // Quindi possiamo ottenere un Command per il trasferimento
    // sapendo che due operazioni si susseguono per ottenere un trasferimento
    // e informando il Command quale attore (from to) opera per il prelievo e il deposito.
    class MoneyTransferCommand : CompositeBankAccountCommand
    {
        public MoneyTransferCommand(BankAccount from, BankAccount to, int amount)
        {
            AddRange(new[]
                {
                    new BankAccountCommand(from, BankAccountCommand.Action.Withdraw, amount),
                    new BankAccountCommand(to, BankAccountCommand.Action.Deposit, amount)
              });
        }

        // In modo composito il Call opererà nel suo insieme come se fosse 
        // un singolo comando a sua volta.
        public override void Call()
        {
            bool ok = true;
            foreach (var cmd in this)
            {
                if (ok)
                {
                    cmd.Call();
                    ok = cmd.Success;
                }
                else
                {
                    cmd.Success = false;
                }
            }
        }
    
    }

    class Demo
    {
        static void Main200(string[] args)
        {
            var ba = new BankAccount();
            var cmdDeposit = new BankAccountCommand(ba,
              BankAccountCommand.Action.Deposit, 100);
            var cmdWithdraw = new BankAccountCommand(ba,
              BankAccountCommand.Action.Withdraw, 1000);
            cmdDeposit.Call();
            cmdWithdraw.Call();
            WriteLine(ba);
            cmdWithdraw.Undo();
            cmdDeposit.Undo();
            WriteLine(ba);


            var from = new BankAccount();
            from.Deposit(100);
            var to = new BankAccount();

            var mtc = new MoneyTransferCommand(from, to, 1000);
            mtc.Call();


            // Deposited $100, balance is now 100
            // balance: 100
            // balance: 0

            WriteLine(from);
            WriteLine(to);
        }
    }
}