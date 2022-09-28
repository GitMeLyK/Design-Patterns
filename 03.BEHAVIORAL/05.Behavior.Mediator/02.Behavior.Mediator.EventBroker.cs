using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Autofac;


namespace DotNetDesignPatternDemos.Behavioral.ObjectMediator.ImplementingObservable.Broker
{

    /*
     * In questo esempio affrontiamo quello che è un Mediator combinato
     * tramite un gestore di eventi per essere di tipo Observable, questo 
     * è un esempio inizialmente più complesso ma usa la possibilità di 
     * avere una lista di eventi che dovranno susseguirsi a un determinato
     * cambio di stato da parte del sottoscrittore nel Mediator.
     * Ogni componente Attore si sottoscrive al Mediator utilizzando un delegato
     * di evento che sarà usato dal mediator per raccogliere appunto un Broker di eventi
     * da far avviare al momento di un azione che succede nel gioco.
     * Il Coach il Ref e il Player che derivano da Actor sono tutti componenti del sistema
     * con logiche diverse di inrpretare l'azione nel gioco.
     */

    // Il Compnente Attore di Base per i Tipi di comppnente che fanno parte del sistema
    public class Actor
    {
        protected EventBroker broker;

        public Actor(EventBroker broker)
        {
            this.broker = broker ?? throw new ArgumentNullException(paramName: nameof(broker));
        }
    }

    // Il Coach che è un componente del sistema gioco
    public class FootballCoach : Actor
    {
        public FootballCoach(EventBroker broker) : base(broker)
        {
            // Si sottoscrive al bus degli eventi nel Mediator con una funzione evento passata per riferimento
            // questo evento si consuma nel mediator appena succede che un Player fa un gol
            broker.OfType<PlayerScoredEvent>()
              .Subscribe(
                ps =>
                {
                    if (ps.GoalsScored < 3)
                        Console.WriteLine($"Coach: well done, {ps.Name}!");
                    else
                        Console.WriteLine($"Coach: WAUU well done, {ps.Name}!");
                }
              );

            // Si sottoscrive al bus degli eventi nel Mediator con una funzione evento passata per riferimento
            // questo evento si consuma nel mediator appena succede che un Player fa un fallo
            broker.OfType<PlayerSentOffEvent>()
              .Subscribe(
                ps =>
                {
                    if (ps.Reason == "violence")
                        Console.WriteLine($"Coach: How could you, {ps.Name}?");
                });
        }
    }

    // Il Ref è un altro componente del sistema per il gioco 
    public class Ref : Actor
    {
        public Ref(EventBroker broker) : base(broker)
        {
            // Questo si sottoscrive al bus degli eventi del Mediator controllando
            // che levento scatenzante sia punteggio o fallo sul sistema di gioco.
            broker.OfType<PlayerEvent>()
              .Subscribe(e =>
              {
                  if (e is PlayerScoredEvent scored)
                      Console.WriteLine($"REF: player {scored.Name} has scored his {scored.GoalsScored} goal.");
                  if (e is PlayerSentOffEvent sentOff)
                      Console.WriteLine($"REF: player {sentOff.Name} sent off due to {sentOff.Reason}.");
              });
        }
    }

    // Il Palyer è un componente anch'esso del sistema gioco
    public class FootballPlayer : Actor
    {
        private IDisposable sub;
        public string Name { get; set; } = "Unknown Player";
        public int GoalsScored { get; set; } = 0;

        public void Score()
        {
            GoalsScored++;
            // Handle - Usa il mediator per notificare un nuovo evento per il goal fatto ( a 3 goal diventa campione )
            broker.Publish(new PlayerScoredEvent { Name = Name, GoalsScored = GoalsScored });
        }

        public void AssaultReferee()
        {
            // Handle - Usa il mediator per notificare un nuovo evento nel sistema per un fallo subito
            broker.Publish(new PlayerSentOffEvent { Name = Name, Reason = "violence" });
        }

        // Il costruttore di default che identifica per nome il giocatore e lo assegna
        // al Mediator di brokeraggio
        public FootballPlayer(EventBroker broker, string name) : base(broker)
        {
            if (name == null){
                throw new ArgumentNullException(paramName: nameof(name));
            }
            Name = name;

            // Delegate - Assegna al Medaitor un delegato di evento al momento che segna sul bus degli eventi
            broker.OfType<PlayerScoredEvent>()
              .Where(ps => !ps.Name.Equals(name))
              .Subscribe(ps => Console.WriteLine($"{name}: Nicely scored, {ps.Name}! It's your {ps.GoalsScored} goal!"));

            // Delegate - Assegna al Medaitor un delegato di evento al momento che riceve un fallo sul bus degli veneti
            sub = broker.OfType<PlayerSentOffEvent>()
              .Where(ps => !ps.Name.Equals(name))
              .Subscribe(ps => Console.WriteLine($"{name}: See you in the lockers, {ps.Name}."));
        }
    }

    // La classe base per gli eventi del gioco da accodare al Broker di eventi
    public class PlayerEvent
    {
        public string Name { get; set; }
    }

    // Un tipo di evento che viene usato per lo Scored che impostano il numero di gol
    public class PlayerScoredEvent : PlayerEvent
    {
        public int GoalsScored { get; set; }
    }

    // Un tipo di evento che viene usato per i Falli che impostano una reazione
    public class PlayerSentOffEvent : PlayerEvent
    {
        public string Reason { get; set; }
    }

    // Il Mediator implementato come Observable e che raccoglie gli eventi (fa da broker)
    public class EventBroker : IObservable<PlayerEvent>
    {
        // Tramite il System.Reactive (installato) questo diventa un repository
        // di delegati per tutti gli eventi dei componenti.
        private readonly Subject<PlayerEvent> subscriptions = new Subject<PlayerEvent>();
        
        // Il Bus degli eventi per l'aggiunta da un componente di un delegato Osservabile dal broker di eventi
        // per la sottoscrizione ad una particolare azione da monitorare.
        public IDisposable Subscribe(IObserver<PlayerEvent> observer) {
            return subscriptions.Subscribe(observer);
        }

        // L'handle dell'esecuzione degli eventi in coda
        public void Publish(PlayerEvent pe)
        {
            // Notifica a tutti i sottoscrittori osservati l'arrivo di 
            // uno specifico elemento nella sequenza.
            subscriptions.OnNext(pe);
        }
    }

    public class Demo
    {
        static void MainC(string[] args)
        {
            var cb = new ContainerBuilder();
            // Risolve per il Mediator implementato come Observable
            cb.RegisterType<EventBroker>().SingleInstance();
            // Risolve il componente che fa parte del sistema
            cb.RegisterType<FootballCoach>();
            cb.RegisterType<Ref>();
            // Risolve il compnente  Player tramite il suo costruttore che utilizza i due argomenti
            // come parametri per creare un istanza, il primo l'EventBroker già registrato e il secondo un parametro di tipo stringa.
            cb.Register((c, p) => new FootballPlayer(c.Resolve<EventBroker>(), p.Named<string>("name")));

            using (var c = cb.Build())
            {
                var referee = c.Resolve<Ref>(); // order matters here!
                var coach = c.Resolve<FootballCoach>();
                // Istanza dei players con il parametro name necessario
                var player1 = c.Resolve<FootballPlayer>(new NamedParameter("name", "John"));
                var player2 = c.Resolve<FootballPlayer>(new NamedParameter("name", "Chris"));
                
                // Punteggio ( il Bus degli eventi passerà dal player al ref al coach )
                player1.Score();
                player2.Score();
                player1.Score();
                player1.Score(); // only 2 notifications
                player1.AssaultReferee();
                player2.Score();
            }
        }
    }
}