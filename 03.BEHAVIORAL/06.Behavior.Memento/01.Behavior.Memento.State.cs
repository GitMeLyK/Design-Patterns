using static System.Console;

namespace DotNetDesignPatternDemos.Behavioral.Memento
{
    /*
     * Per questo particolare modello usiamo un esempio già visto dove 
     * usavamo prima il Command per avere le operazioni di Undo e Redo
     * questa volta salvaimo lo stato dell'oggetto corrente dopo l'esecuzione
     * di un determinato metodo e lo restituiamo al mittente per essere
     * utilizzato come token di stato corrente dell'oggetto appena modificato.
     * In questo modo chi esegue i metodi di Deposito e Prelievo viene restituito
     * un oggetto di stato con il valore delle proprietà del BanckAccount e ogni
     * valore catturato rappresenta un momento dell'operazione che in successione
     * può nuovamente essere ripristinata riapplicando il valore di Stato tramite
     * il metodo Restore che prende in sè lo stato dell'oggetto che rappresenta i valori
     * da ripristinare e li riapplica.
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

        public BankAccount(int balance)
        {
            this.balance = balance;
        }

        public Memento Deposit(int amount)
        {
            balance += amount;
            // Restuisce lo stato corrente dell'oggetto
            return new Memento(balance);
        }

        public void Restore(Memento m)
        {
            // Ripristina lo stato corrente dell'oggetto
            balance = m.Balance;
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
            // m1 e m2 sono gli snapshot di stato 
            // corrente dell'oggetto cone le modifiche avvenute.
            var m1 = ba.Deposit(50); // 150
            var m2 = ba.Deposit(25); // 175
            WriteLine(ba);

            // restore to m1
            ba.Restore(m1);
            WriteLine(ba);

            ba.Restore(m2);
            WriteLine(ba);
        }
    }
}