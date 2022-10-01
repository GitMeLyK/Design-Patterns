using System;

namespace DotNetDesignPatternDemos.Architectural.Interceptor
{
    /*
     *  Gli aspetti chiave del modello sono che la modifica è trasparente e 
     *  utilizzata automaticamente. In sostanza, il resto del sistema non 
     *  deve sapere che qualcosa è stato aggiunto o modificato e può continuare 
     *  a funzionare come prima. 
     *  
     *  Per facilitare questo, è necessario implementare un'interfaccia predefinita 
     *  per l'estensione, è necessario un qualche tipo di meccanismo di dispacciamento 
     *  in cui vengono registrati gli intercettori (questo può essere dinamico, 
     *  in fase di esecuzione o statico, ad esempio tramite file di configurazione) 
     *  e vengono forniti oggetti di contesto, che consentono l'accesso allo stato 
     *  interno del framework. 
     * 
     *  Uso tipico
     *  
     *  Gli utenti tipici di questo modello sono i server Web (come menzionato sopra), gli oggetti
     *  e il middleware orientato ai messaggi
     *  
     *  Un esempio di implementazione di questo modello è l'interfaccia javax.servlet.Filter, 
     *  che fa parte di Java Platform, Enterprise Edition.
     *  
     *  La programmazione orientata agli aspetti (AOP) può anche essere utilizzata in 
     *  alcune situazioni per fornire la capacità di un intercettore, sebbene AOP 
     *  non utilizzi gli elementi tipicamente definiti per il modello dell'intercettore.
     *  
     */

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
