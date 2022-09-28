using System;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using JetBrains.Annotations;
using MediatR;

namespace DotNetDesignPatternDemos.Behavioral.MediatorDll.MediatR
{

    /*
     * In questo esempio affrontiamo quello che è l'uso di uno dei più usati
     * componenti per l'implementazione di un Mediator, questo progetto sviluppato
     * da tempo è scaribile nel sito di github o includerlo nel progetto tramite il nuget
     * si chiama MediatR. Questa libreria si occupa fondamentlamente di liberarci dal compito
     * di costrutire una classe ad hoc centralizzata per usare un componente centrale come
     * Mediatore e ne fornisce quelle che sono le basi per una classe che deve solo ereditare
     * e implementare le funzionalità più consone al proprio progetto.
     * 
     */

    // Il componente attore che deve dare una risposta
    public class PongResponse
    {
        public DateTime Timestamp;

        public PongResponse(DateTime timestamp)
        {
            Timestamp = timestamp;
        }
    }

    // Il componente attore che implemeta IRequest di MediatR
    public class PingCommand : IRequest<PongResponse>
    {
        // nothing here
    }

    // L'Handler per i componenti sottoscrittori che fanno parte
    // del mediator e implementano per la DI la registrazione di Mediator di MediatR
    [UsedImplicitly]
    public class PingCommandHandler : IRequestHandler<PingCommand, PongResponse>
    {
        // Nel bus degli eventi di MediatR questo è l'handle ricevente dalla richiesta in corso
        // da parte di un componete in questo caso PingCommand che è di tipo IRequest
        [System.Diagnostics.CodeAnalysis.SuppressMessage("AsyncUsage", "AsyncFixer01:Unnecessary async/await usage", Justification = "<In sospeso>")]
        public async Task<PongResponse> Handle(PingCommand request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(new PongResponse(DateTime.UtcNow))
              .ConfigureAwait(false);
        }
    }

    public class Demo
    {
        public static async Task Main()
        {
            var builder = new ContainerBuilder();
            // Il Mediator di MediatR da registrare nel contesto corrente
            // Come singleton per thread separati
            builder.RegisterType<Mediator>()
              .As<IMediator>()
              .InstancePerLifetimeScope(); // singleton

            // Il ServiceFactory di MediatR per farsi restuire
            // staticamente un oggetto funzionale di componente
            // Questo artefatto fa parte di Autofac per risolvere
            // i componenti nel contesto tramite questo delegato.
            builder.Register<ServiceFactory>( context =>
            {
                var c = context.Resolve<IComponentContext>();
                return t => c.Resolve(t);
            });

            // Una registrazione generalizzata di tutti i componenti in questo progetto
            // senza dover registrali uno ad uno, registrerà il Ping e il Pong per lo specifico.
            builder.RegisterAssemblyTypes( typeof(Demo).Assembly)
              .AsImplementedInterfaces();

            var container = builder.Build();

            // Il mediator di MediatR
            var mediator = container.Resolve<IMediator>();

            // Il componente inviato al Mediator su cui verrà eseguita l'operazionie Pong
            PongResponse response = await mediator.Send(new PingCommand());
            Console.WriteLine($"We got a pong at {response.Timestamp}");
            
        }
    }
}