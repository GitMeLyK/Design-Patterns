using System;
using System.Dynamic;
using Autofac;
using ImpromptuInterface;
using JetBrains.Annotations;
using static System.Console;

namespace DotNetDesignPatternDemos.Behavioral.NullObject
{
    /*
     * In questo esempi vediamo come si comprta un Modello di NullObject presupponendo
     * come componente A il Conto Bancario classe BankAccount e il componente Opzionale
     * B usato all'interno per fare dei log diagnostici ConsoleLog.
     * Nell'esempio ben spiegato è come si vede nell'utilizzo si un Null previsto come
     * componente B in A ogni volta che A usa B deve sapere in anticipo se B è nullo e 
     * questo per ogni volta che lo si usa, quindi questo tipo di design in anticipo per
     * il controllo richiede molte volte lo stesso controllo del caso di utilizzo oltre che
     * nel caso di uso di un container per le DI non utilizzi la possibilità di fornire un
     * valore null per il tipo.
     * 
     */


    public interface ILog
    {
        void Info(string msg);
        void Warn(string msg);
    }

    // Componente B
    class ConsoleLog : ILog
    {
        public void Info(string msg)
        {
            WriteLine(msg);
        }

        public void Warn(string msg)
        {
            WriteLine("WARNING: " + msg);
        }
    }

    // Componente A
    public class BankAccount
    {
        private ILog log;
        private int balance;


        // Con questo abbiamo un design in aticipio e secondo il principio
        // di aperto chiuso deleghiamo all'esterno della classe il trattamento
        // di questi tipi NullObject ma non è questo che definisce il principio per questo
        // modello in quanto deleghiamo sempre al programmatore di tenere in considerazione
        // che questo oggetto può essere nullo e lo stiamo solo suggerendo, e ovviamente il
        // caso non si risolve in modo trasparente secondo questo principio in quanto come
        // abbiamo detto il programmatore in tutti i punti del codice è obbligato a controllare
        // il tipo se nullo come nel caso del metodo Deposit(..) che per scrivere una riga deve
        // attenersi sempre a scrivere log?.Info(...) e in tutti gli altri casi. Questo è il problema
        // dei design in anticipo 
        /*
         * public BankAccount([CanBeNull] ILog log)
            {
                this.log = log;
            }
        */

        public BankAccount(ILog log)
        {
            this.log = log;
        }

        public void Deposit(int amount)
        {
            balance += amount;
            // check for null everywhere delegando al programmatore sempre questo controllo
            // nel caso si ovvi al primo costruttore commentato.
            // log?.Info($"Deposited ${amount}, balance is now {balance}");
            log.Info($"Deposited ${amount}, balance is now {balance}");
        }

        public void Withdraw(int amount)
        {
            if (balance >= amount)
            {
                balance -= amount;
                // check for null everywhere delegando al programmatore sempre questo controllo
                // nel caso si ovvi al primo costruttore commentato.
                // log?.Info($"Withdrew ${amount}, we have ${balance} left");
                log.Info($"Withdrew ${amount}, we have ${balance} left");
            }
            else
            {
                // check for null everywhere delegando al programmatore sempre questo controllo
                // nel caso si ovvi al primo costruttore commentato.
                //log?.Warn($"Could not withdraw ${amount} because balance is only ${balance}");
                log.Warn($"Could not withdraw ${amount} because balance is only ${balance}");
            }
        }
    }

    // Il Componente NullObject Gemello di B ma vuoto per essere cisto come
    // un oggetto di tipo NullObject cioè vuoto di funzionalità
    public sealed class NullLog : ILog
    {
        public void Info(string msg){}
        public void Warn(string msg){}
    }

    // Il nostro modello di trattamento di questi tipi che si occupa
    // essenzialmente di creare l'istanza di questo oggetto rispetto a T
    // che in questo caso può essere o ConsoleLog o NullLog e definire l'istanza
    // del tipo appropriato al caso tramite questo factory dinamico con luso della
    // dll ImpromptuInterface.
    // Usando questo approccio tramite il DLR che viene implementato in questa speciale
    // libreria e in questo metodo si paga il prezzo delle prestazioni dato da questa modalità
    // di esecuzione dinamica con i metodi invocati piuttosto che eseguiti direttamente.
    public class Null<T> : DynamicObject where T : class
    {
        public static T Instance
        {
            get
            {
                if (!typeof(T).IsInterface)
                    throw new ArgumentException("I must be an interface type");

                return new Null<T>().ActLike<T>();
            }
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            result = Activator.CreateInstance(binder.ReturnType);
            return true;
        }

        private class Empty { }
    }

    public class Demo
    {
        static void Main()
        {

            var cb = new ContainerBuilder();
            cb.RegisterType<BankAccount>();

            // In un tipo preventivo che accetti null e tutte le 
            // su conseguenze di controllo in tutti posti lo avremmo registrato
            // in questo modo dove il componente B non usato alla fine viene
            // accettato e tutti i metodi che usano B in A faranno i previi controlli 
            // del caso e in tutti i casi dove lo si vorrebbe usare a condizione che non si nullo.
            //cb.Register(ctx => new BankAccount(null));

            // Nel Caso invece di un NullObject con la classe NullLog che
            // esponse tutti i Metodi di Log ma non fanno nulla la situazione
            // in CI diventa in questo modo:
            cb.RegisterType<BankAccount>();
            cb.RegisterType<NullLog>().As<ILog>();

            // Quindi possiamo registrare un account bancario
            using (var c = cb.Build())
            {
                var BAcc = c.Resolve<BankAccount>();
                BAcc.Deposit(100);
                // E tutto funziona in quanto abbiamo preventivamente
                // usato il NullLog per una istanza vuota di un oggetto
                // Log, e quindi non darà eccezione in tutti i punti dove
                // verrà usato.
            }

            // Istanze del log Non null e Null Object o null da passare al BanckAccount
            //var log = new ConsoleLog();
            //ILog log = null;
            //var log = new NullLog();

            // O Istanza di un Tipo NullObject tramite il Factory dinamico con l'auslio di ImpromptuInterface
            var log = Null<ILog>.Instance;

            var ba = new BankAccount(log);
            ba.Deposit(100);
            ba.Withdraw(200);

        }
    }
}