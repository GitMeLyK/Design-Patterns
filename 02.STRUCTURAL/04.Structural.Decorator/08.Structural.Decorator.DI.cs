using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using static System.Console;

namespace DotNetDesignPatternDemos.Structural.Decorator.DI
{
    /*
     * In questo esempio si opererà su delle forme di registro
     * dove viene presentato un servizio di registrazione e con
     * anche la possibilità di annullare il processo, tutto questo
     * tramite il Decoratore tipico. La classe implementerà un interfaccia
     * del servizio una volta implementato questo decoratore sarà iniettato
     * nel componente tramite DI.
     */

    public interface IReportingService
    {
        void Report();
    }

    // Il servizio di report
    public class ReportingService : IReportingService
    {
        public void Report()
        {
            Console.WriteLine("Here is your report");
        }
    }

    // Il decorator per catturare il serivizo di reportinf e wrapparlo
    // su quello da fare prima e quello da fare dopo l'esecuzione vera 
    // e propria del metodo Report()
    public class ReportingServiceWithLogging : IReportingService
    {
        private readonly IReportingService decorated;

        public ReportingServiceWithLogging(IReportingService decorated)
        {
            if (decorated == null)
            {
                throw new ArgumentNullException(paramName: nameof(decorated));
            }
            this.decorated = decorated;
        }

        // Il metodo wrappato per processare prima e dopo dell'esecuzione del metodo.
        public void Report()
        {
            Console.WriteLine("Commencing log...");
            decorated.Report();
            Console.WriteLine("Ending log...");
        }
    }

    public class Decorators
    {
        static void Main(string[] args)
        {
            var b = new ContainerBuilder();
            
            // Una volta resistrata nel container la classe servizio
            // che implementa l'interfaccia di reporting e usiamo per
            // questo tipo di risoluzione una chiave tramite .Named<>() che
            // verrà usata dal metodo per poter essere prelevato per la risoluzione.
            b.RegisterType<ReportingService>().Named<IReportingService>("reporting");
            
            // nel container viene iniettato il decoratore tramite lo speciale
            // metodo del DI RegisterDecorator<> che sarà utilizzato con il
            // suo costruttore per quelle classi in cui servirà, e come si vede 
            // il metodo si aspetta appunto la chiave della classe decorator registrata.
            b.RegisterDecorator<IReportingService>(
                (context, service) => new ReportingServiceWithLogging(service),
              "reporting");

            // open generic decorators also supported
            // b.RegisterGenericDecorator()

            using (var c = b.Build())
            {
                // Viene risolto il servizio (decorato)
                var r = c.Resolve<IReportingService>();
                // e viene eseguito il wrapper decorator che esegue qualcosa prima e qualcosa dopo.
                r.Report();
            }
        }
    }
}