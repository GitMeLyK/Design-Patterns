using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
//using DotNetDesignPatternDemos.Annotations;
using JetBrains.Annotations;

namespace DotNetDesignPatternDemos.Behavioral.Observer.Bidirectional
{
    /* Con questo esempio parliamo di un aspetto del modello Observer che se non
     * trattato nel modo giusto diventa illegibile quella che viene definita metodo
     * di Associazione Bidirezionale, e cioè in tutti quegli scenari che oltre a 
     * notificare un determinato stato verso i sottoscrittori dell'Observer ci sono
     * momenti in cui il processo deve avvenire in modo che anche un altro Observer
     * faccia uso di queste notifiche e allo stesso tempo cambiando il suo stato e
     * rinotificando quello che prima era il Mittente si incorre nel rischio di un
     * Loop Infinito. Per evitare questo l'attore centrale che orchestra questa tipo
     * di comunicazione e associazione in modo bidirezionale è necessario.
     * In questo esempio vediamo una classe Product che è il nostro primo Observed
     * e la classe Window è il nostro secondo elemento che è di tipo Observed anche lui,
     * e in tutte e due i casi sono acnhe Obserbale l'uno dell'atro, in quanto se 
     * proviamo a cambiare il nome del prodotto nella classe oggetto questa notifca
     * il cambio del nome alla classe window che a sua volta riscrive il valore 
     * nel compinente textbox che visualizza il nome appunto. E viceversa se l'utente
     * a runtime cambia il nome nella textbox avviene che la classe window riceve un
     * evento che deve notificare la classe Product per cambiare la propria proprietà
     * relativa al nome. Quindi per fare in modo che tutte e due le classi vadano
     * in overloop che l'evento si sussegue a catena da uno verso l'altro che notifica
     * e nuovamente dall'altro verso il primo che notifica anche lui, e per ovviare ad 
     * un sovraccarico dello stack tra i due che si notificano adottiamo sempre il 
     * controllo nel set prima del OnPropertyChanged() con l'if (value == productName) return;
     * e anche nell'altro allo stesso modo per il Name. 
     * Quindi si è possibile che i Due Observer evitino un Loop tramite questo controllo
     * priam di assegnare e lanciare il comand per l'Handle degli eventi successivi, ma
     * per evitare che queste dimenticanze o accorgimenti accadano allora è meglio trattare
     * secondo il principio della separazione delle responsabilità questo accorgimento 
     * all'esterno e per tutti gli elementi coinvolti. Fare questo significa quindi adottare
     * una classe ad hoc per gestire questa tipologia di comunicazione in  questo caso
     * la BidirectionalBinding che fondamentalmente convoglierà i due oggetti che fanno parte
     * di questo tipo di comportamento e sarà a conoscenza delle proprietà tra loro legate.
     */

    // Observed
    public class Product : INotifyPropertyChanged
    {
        private string name;

        public string Name
        {
            get => name;
            set
            {
                if (value == name) return; // critical to loop if not present
                name = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(
          [CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this,
              new PropertyChangedEventArgs(propertyName));
        }

        public override string ToString()
        {
            return $"Product: {Name}";
        }
    }

    // Observed 2
    public class Window : INotifyPropertyChanged
    {
        private string productName;

        public string ProductName
        {
            get => productName;
            set
            {
                if (value == productName) return; // critical to loop if not present
                productName = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public Window(Product product)
        {
            ProductName = product.Name;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(
          [CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this,
              new PropertyChangedEventArgs(propertyName));
        }

        public override string ToString()
        {
            return $"Window: {ProductName}";
        }
    }

    // L'implementazione per l'associazione bidirezinale
    public sealed class BidirectionalBinding : IDisposable
    {
        private bool disposed;

        public BidirectionalBinding(
          INotifyPropertyChanged first,  Expression<Func<object>> firstProperty,
          INotifyPropertyChanged second, Expression<Func<object>> secondProperty)
        {
            // Class Product e Window Observed
            // Property Name e ProductName linked

            // Si assicura che le espressioni di proprietà siano
            // in realtà espressioni membro X.foo() non è espressione membro
            // x.foo è un espressione prperty valida da sottoporre
            if (firstProperty.Body is MemberExpression firstExpr
                && secondProperty.Body is MemberExpression secondExpr)
            {
                if (firstExpr.Member is PropertyInfo firstProp
                    && secondExpr.Member is PropertyInfo secondProp)
                {
                    // Assegnazione all'Observer A per il set del valore verso il secondo
                    // nella almbda delegata al gestore di evento da usare per ricevere la
                    // notifica
                    first.PropertyChanged += (sender, args) =>
                    {
                        if (!disposed)
                        {
                            // Assegnazione tramite reflections
                            secondProp.SetValue(second, firstProp.GetValue(first));
                        }
                    };
                    // Stessa assegnazione allìObserver B per il set del valore verso il primo.
                    second.PropertyChanged += (sender, args) =>
                    {
                        if (!disposed)
                        {
                            // Assegnazione tramite reflections
                            firstProp.SetValue(first, secondProp.GetValue(second));
                        }
                    };
                }
            }
        }

        public void Dispose()
        {
            disposed = true;
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            var product = new Product { Name = "Book" };
            var window = new Window(product);


            // Nel primo caso con la sottoscrizione del componente a
            // verso il b e tenendo conto della condizione interna per 
            // evitare il loop abbiamo questa modalità non propriamente
            // bidirezionale ma relegata al sistema corrente

            // want to ensure that when product name changes
            // in one component, it also changes in another

            // product.PropertyChanged += (sender, eventArgs) =>
            // {
            //   if (eventArgs.PropertyName == "Name")
            //   {
            //     Console.WriteLine("Name changed in Product");
            //     window.ProductName = product.Name;
            //   }
            // };
            //
            // window.PropertyChanged += (sender, eventArgs) =>
            // {
            //   if (eventArgs.PropertyName == "ProductName")
            //   {
            //     Console.WriteLine("Name changed in Window");
            //     product.Name = window.ProductName;
            //   }
            // };

            // Nel secondo Case relegando tramite la classe apposita 
            // l'uso di eventi aggiornati in modo bidirezionale comunque
            // sia il valore che sta cambiando o da un Observer o dall'altro.
            using var binding = new BidirectionalBinding(
              product,
              () => product.Name,
              window,
              () => window.ProductName);

            // there is no infinite loop because of
            // self-assignment guard
            product.Name = "Table";
            Console.WriteLine(product);
            Console.WriteLine(window);

            window.ProductName = "Chair";

            Console.WriteLine(product);
            Console.WriteLine(window);
        }
    }
}