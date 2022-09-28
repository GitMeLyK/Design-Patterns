using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
//using DotNetDesignPatternDemos.Annotations;
//using MoreLinq;

namespace DotNetDesignPatternDemos.Behavioral.Observer.PropertyDependencies
{
    /* Con questo esempio parliamo di un aspetto del modello Observer che relativamente
     * alle notifiche molto utili nell'uso di interfaccie grafiche ci si scontra sempre
     * con un determinato problema relativo alle dipendenze tra le diverse proprietà.
     * Non è un problema o un aspetto da trascurare e non ha sempre soluzioni facili da
     * gestire. In questo esempio facciamo in modo di affrontare il problema anche se
     * potrebbe essere una delle soluzioni ma non sempre per tutti i programmi ci troviamo
     * suq questo problema e lo possiamo aggiustare allo stesso modo.
     * Il problema in questo esempio si trova nel punto in cui si vuole notificare che la persona
     * ha votato perchè le condizioni delle varie proprietà come l'età e il comune siano
     * rispettati. Questo vuol dire che se ogni proprietà come l'età la cittadinanza etc
     * influiscono sullo stesso risultato ogni cambiamento di queste comporta più notifiche
     * uguali a susseguirsi. Un altro problema è lo stesso per lo stesso campo, perchè anche
     * se in questo caso cambiassimo l'età della persona da 14 a 15 e il minimo per votare
     * è 16 anche qui avremmo una notifica di stato come cambiato in CanVote per dire che 
     * la persona può votare. Quindi impossibilitare le notifiche su un cambiamento di stato
     * d'insieme di stati se la condizione non si avvera è inutile, cioè non torna in un sistema
     * completo come quello delle interfaccie dove ci sono molte condizioni che devono essere
     * valutate per portare una notifica sul Form.
     * In questo esempio la classe base PropertyNotificationSupport fà da resolver di queste
     * situazioni di visione e calcolo del set di proprietà coinvolte dove internamente farà
     * in modo di gestire l'intero grafico delle dipendenze, quindi ha in sè un dizionario che
     * sarà valorizzato tramite reflection a contenere tutti i nomi delle proprietà coinvolte
     * Quindi noto il problema CanVote -> Age,Citizen definirà che OnPorpertyChange interno
     * è abilitato a notificare il sistema che la Persona ha l'età per votare.
     * Nell'oggeto Observer è presente una proprietà dinamica CanVote che non ha un risultato
     * di proprietà preso da una variabile interna, ma otterrà il risultato da una espressione
     * da calcolare al momento che si accederà con il getter. Questa proprietà è resa grazie
     * all'utilizzo di conservazione dinamica di una espressione dentro un campo che sarà 
     * eseguita al momento dell'interrogazione dove l'espressione è attraversata per essere 
     * usata con i nuovi valori attualemten presenti nel grafo delle dipendenze.
     */

    // Classe base per la gestione delle notifiche valutate da un insieme
    // di proprietà o da una condizione da rispettare per un determinato campo.
    public class PropertyNotificationSupport : INotifyPropertyChanged
    {
        private readonly Dictionary<string, HashSet<string>> affectedBy
          = new Dictionary<string, HashSet<string>>();

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged
          ([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

            // Qui viene valutato l'insieme delle chiavi per avviare in seguto
            // l'espressione che valuta se notificare l'evento CanVote
            foreach (var affected in affectedBy.Keys)
                if (affectedBy[affected].Contains(propertyName))
                    OnPropertyChanged(affected);
        }

        /********************************************************************/

        // La property dinamica che conserva l'espressione da calcolare solo al momento
        // in cui la notifica che passa le proprietà modificate deve interrogare lo stato
        // attualemte nella condizione in cui è e non da valori ridefiniti in proprietà
        // ancora non notificate. Questa restiuisce una delega di una espressione binari
        // usata per fare il calcolo.
        protected Func<T> property<T>(string name, Expression<Func<T>> expr)
        {
            Console.WriteLine($"Creating computed property for expression {expr}");

            var visitor = new MemberAccessVisitor(GetType());
            visitor.Visit(expr);

            if (visitor.PropertyNames.Any())
            {
                if (!affectedBy.ContainsKey(name))
                    affectedBy.Add(name, new HashSet<string>());

                foreach (var propName in visitor.PropertyNames)
                    if (propName != name)
                        affectedBy[name].Add(propName);
            }

            return expr.Compile();
        }

        // Di supporto alla property dinamica questo è accessorio per 
        // attraversare l'albero dell'espressione passata come argomento da calcolare
        // interrogando i campi previsti essendo presenti nella lista dei campi in uso.
        private class MemberAccessVisitor : ExpressionVisitor
        {
            private readonly Type declaringType;

            public readonly IList<string> PropertyNames = new List<string>();

            public MemberAccessVisitor(Type declaringType)
            {
                this.declaringType = declaringType;
            }

            public override Expression Visit(Expression expr)
            {
                if (expr != null && expr.NodeType == ExpressionType.MemberAccess)
                {
                    var memberExpr = (MemberExpression)expr;
                    if (memberExpr.Member.DeclaringType == declaringType)
                    {
                        PropertyNames.Add(memberExpr.Member.Name);
                    }
                }

                return base.Visit(expr);
            }
        }

        /********************************************************************/

    }

    // Il nostro Observed che implementa il supporto alla gestione
    // delle notifiche mirate ad un risultato calcolato dall'insieme
    // delle proprietà.
    public class Person : PropertyNotificationSupport
    {
        private int age;
        private bool citizen;

        public int Age
        {
            get => age;
            set
            {
                if (value == age) return;
                age = value;
                OnPropertyChanged();
            }
        }

        public bool Citizen
        {
            get => citizen;
            set
            {
                if (value == citizen) return;
                citizen = value;
                OnPropertyChanged();
            }
        }

        // La funzione delegante che viene restituita dalla
        // property e trasforma questo calcolo in un espressione
        // è settabile come labda di espressione da tenere in considerazione
        // quando al momento di dare notifica questa è accettata come condizione.
        // usando il metodo sotto con una lambda già espressa il cambiamento notificato
        // sarebbe sempre riportato al valore prima della modifica e non a quello in
        // tempo reale dopo avvenuta la modifica notificata.
        //public bool CanVote => Age >= 16;

        // La lamda impostata dalla funzione property nel costruttore che 
        // definisce il risultato come valido se l'espressione ha attraverso
        // le proprietà ottenuto una condizione valida.
        private readonly Func<bool> canVote;        

        // Il getter del risultato che eseguirà la lambda settata
        public bool CanVote => canVote();

        public Person()
        {
            // assegna la property non ancora calcolata per la notifica.
            // semplicemente è un costrutto per una property dinamica di
            // campo calcolato quando serve.
            canVote = property(nameof(CanVote), () => Citizen && Age >= 16);
        }
    }

    public class Demo
    {
        static void Main(string[] args)
        {
            var p = new Person();
            p.PropertyChanged += (sender, eventArgs) =>
            {
                Console.WriteLine($"{eventArgs.PropertyName} has changed");
            };
            p.Age = 15; // should not really affect CanVote :)
            p.Citizen = true;
        }
    }
}