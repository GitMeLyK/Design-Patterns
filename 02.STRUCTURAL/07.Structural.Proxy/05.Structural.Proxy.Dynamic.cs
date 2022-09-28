using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using ImpromptuInterface;
using static System.Console;


namespace DotNetDesignPatternDemos.Structural.Proxy.Dynamic
{
    /*
     * Come per altri componenti scegliere tra un modello statico o 
     * uno dinamico anche il proxy può essere trattato in queste modalità.
     * Nel caso di un proxy trattato come modello dinamico quello che si va
     * a fare è costruire questo proxy a runtime trovando un buon compromesso
     * con le prestazioni. 
     * In questo esempio in un ipotetico conto bancario viene gestito per i movimenti
     * il deposito e il prelievo. Non vogliamo implementare ogni singolo proxy su ogni
     * operazione, (in molti casi succede che ti trovi nella stessa condizione dove hai
     * necessariamente bisogno di implementare per ogni metodo di un oggetto molti proxy di
     * accesso ai dati) quindi la migliore strada è appunto usare un proxy dinamico.
     * Usiamo per aiutare lo sbroglio di questo modello una libreria che ci tornerà utile
     * quando usiamo questa modalità, la ImpromptuInterface (Interfaccia Improvvisata) che 
     * è molto utile quando ottenuto un pacchetto vogliamo generare una interfaccia appropriata.
     * Nell'esempio per ogni operazioni vogliamo tenere traccia di cisascun metodo chiamato e per
     * fare questo avremo un implementazione in una classe chiama Log<T> che eredita dall'oggetto
     * dinamico l'interfaccia e si preoccuperà di richiamare in modo proxato i rispettivi metodi
     * dell'oggetto originale come se stessimo lavorando nell'istanza sull'oggetto originale stesso
     * mentre invece siamo sull'oggetto log.
     */

    public interface IBankAccount
    {
        void Deposit(int amount);
        bool Withdraw(int amount);
        string ToString();
    }

    public class BankAccount : IBankAccount
    {
        private  int balance;
        private readonly int overdraftLimit = -500;

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

    // La classe registro che eredita dall'oggetto dinamico
    // dove internamente il modello factory creerà oggetti al
    // volo delle chiamate verso le operazioni di DynamicObject.
    public class Log<T> : DynamicObject where T : class, new()
    {
        private readonly T subject;
        private readonly Dictionary<string, int> methodCallCount =
          new ();

        protected Log(T subject)
        {
            this.subject = subject ?? throw new ArgumentNullException(paramName: nameof(subject));
        }

        // factory method
        public static I As<I>(T subject) where I : class
        {
            if (!typeof(I).IsInterface)
                throw new ArgumentException("I must be an interface type");

            // duck typing here!
            // Grazie alla dll ImpromptuInterface facciamo si che venga restituita
            // l'oggetto dinamico come se fosse implmentato dal Tipo I e che fa sì che
            // quando invochi i membri in modo dinamico attraversoil TryInvokeMember è
            // come se lo facesse realmente.
            return new Log<T>(subject).ActLike<I>();
        }

        public static I As<I>() where I : class
        {
            if (!typeof(I).IsInterface)
                throw new ArgumentException("I must be an interface type");

            // duck typing here!
            return new Log<T>(new T()).ActLike<I>();
        }

        // Con questo catturiamo ogni chiamata ai metodi quindi proxiamo su ogni metodo
        // e nel comportamento di questa classe di log operiamo per registrare l'azione
        // nella registo locale.
        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            try
            {
                // logging
                WriteLine($"Invoking {subject.GetType().Name}.{binder.Name} with arguments [{string.Join(",", args)}]");

                // more logging
                if (methodCallCount.ContainsKey(binder.Name)) methodCallCount[binder.Name]++;
                else methodCallCount.Add(binder.Name, 1);

                result = subject.GetType().GetMethod(binder.Name).Invoke(subject, args);
                return true;
            }
            catch
            {
                result = null;
                return false;
            }
        }

        public string Info
        {
            get
            {
                var sb = new StringBuilder();
                foreach (var kv in methodCallCount)
                    sb.AppendLine($"{kv.Key} called {kv.Value} time(s)");
                return sb.ToString();
            }
        }

        // will not be proxied automatically
        // L'override si preoccupa di riportare le info sullo stato
        // dell'oggetto originale chiamato e non su questa classe stessa, quindi
        // anche qui proxiamo la chiamata tostring a comportarsi come se fosse
        // la chiamta tostring dell'oggetto trattato dinamicamente da questo proxy.
        public override string ToString()
        {
            return $"{Info}{subject}";
        }
    }

    public class Demo
    {
        static void Main34(string[] args)
        {
            //var ba = new BankAccount();
            // Proxiamo tutti i metodi dinamicamente del BanckAccount
            // per usare il log di registro delle chiamate e con riferimento
            // all'interfaccia creata al volo per le opportune proprietà esposte
            // dall'oggetto.
            var ba = Log<BankAccount>.As<IBankAccount>();
            
            // Quindi anche se ba è un oggetto dinamico che traspette le chiamate 
            // al soggetto reale cin cui stiamo chiamando il metodo reale avendo
            // ricostruito la chiamata intrappolata dalla classe proxy log tornerà
            // a noi l'interfaccia con l'elenco dei metodi come se stessimo lavorando
            // sull'istanza dell'oggetto reale ma siamo sempre su Log
            ba.Deposit(100);
            ba.Withdraw(50);

            // Come noteremo qui se provassimao a scrivere WriteLine(ba.info) possiamo
            // vedere che abbiamo un riferimento all'oggetto Log in quanto stiamo sempre
            // operando tramite l'interfaccia creata al volo dell'oggetto originale BankAccount
            // e l'unico modo per ottenere le Info dinamiche che riporteranno i dati dell'oggetto
            // originale è intervenire in override dul tostring del Log che si adopera a trattare
            // le info dell'oggetto originale proxato.
            WriteLine(ba.ToString());
        }
    }
}