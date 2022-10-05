using System;

namespace DotNetDesignPatternDemos.Architectural.Model2.AspNetMvc4
{
    /*
     * ASP.NET MVC è l’implementazione in chiave Microsoft del pattern MVC, introdotto
     * negli anni ’80 per facilitare lo sviluppo di applicazioni caratterizzate da un numero
     * elevato di interfacce utente, con l’obiettivo di organizzarne al meglio la complessità.
     * 
     * Le applicazioni basate sul pattern MVC sono caratterizzate dalla divisione di tre
     * componenti principali all’interno del presentation layer, ognuno dei quali è
     * caratterizzato dall’occuparsi distintamente di una parte delle responsabilità. La view si
     * occupa semplicemente di effettuare il rendering dei dati ed è priva della logica che è
     * invece all’interno del controller. 
     * 
     * Controller e view comunicano tra loro attraverso il model, cioè la business logic 
     * dell’applicazione. Per questi motivi, la view non necessita di essere soggetta a 
     * testing, dato che la sua logica è semplice e la vera logica applicativa è contenuta 
     * all’interno del controller e del model, che invece possono essere testati per garantire 
     * che il codice sia funzionante.
     * 
     * Il valore aggiunto del pattern MVC (e quindi di ASP.NET MVC, che ne è un’implementazione) 
     * è dato dal fatto che che semplifica questi aspetti, grazie al disaccoppiamento. 
     * 
     * A voler essere pignoli, l’implementazione contenuta all’interno di ASP.NET MVC 4 è nota 
     * come Model 2 ed è una rivisitazione originariamente implementata dal framework Struts, 
     * disponibile per Java e più adatto all’ambito web. 
     * 
     * In tal senso, infatti, Model 2 implementa anche un meccanismo di associazione automatica 
     * tra URL e controller, attraverso un meccanismo di convenzioni, che approfondiremo nei 
     * capitoli dedicati a ASP.NET MVC.
     * 
     * Nota.:   ASP.NET Web Forms, invece, è la trasposizione del modello event-driven, particolarmente 
     *          noto a chi ha sviluppato con ambienti visuali, come Visual Studio. 
     *          In questo modello, ciascuna pagina è chiamata Web Form e fornisce un’infrastruttura
     *          (molto complessa), che consente di programmare gli oggetti contenuti nella pagina
     *          (chiamati controlli) attraverso un set di eventi. 
     *          Perché questo sia possibile, i controlli emettono codice HTML e JavaScript in 
     *          maniera automatica e hanno una componente di codice server side a cui sono 
     *          fortemente legati. 
     * 
     * Differenza tra ASP.NET MVC 4 e ASP.NET Web Forms 
     * 
     * Da un punto di vista pratico, ASP.NET MVC 4 e ASP.NET Web Forms sono differenziati anche dal 
     * ciclo di vita della richiesta a un URL: nel caso di ASP.NET MVC è molto semplificata e 
     * orientata a servire la risposta nel più breve tempo possibile, favorendo le performance 
     * sopra a qualsiasi altro aspetto, mentre la pipeline della richiesta di ASP.
     * 
     * NET Web Forms è molto ricca e pensata per la flessibilità e l’estendibilità, a scapito
     * delle performance pure.
     * 
     * Pur condividendo lo stesso runtime, la scelta degli architetti di ASP.NET MVC è stata
     * quella di evitare il più possibile l’utilizzo dell’approccio di ASP.NET Web Forms, dove
     * diversi componenti vengono messi in gioco per questione di estendibilità (HttpModule,
     * Response Filter, HttpHandler e altri aspetti), in favore di un modello più snello e 
     * semplificato, estendibile dallo sviluppatore solo seeffettivamente necessario. 
     * 
     * Questo è possibile anche perché l’approccio di ASP.NET Web Forms è quello di avere un 
     * contesto stateful (con gestione dello stato), mentre quello di ASP.NET MVC è di lasciare 
     * la pagina stateless (senza stato), come peraltro il protocollo HTTP prevede, demandando 
     * allo sviluppatore l’onere di implementare uno stato. 
     * 
     * Volendo semplificare il discorso, potremmo dire che ASP.NET MVC è l’ideale per avere il 
     * maggior controllo possibile, sia sul markup sia sulla testabilità del codice, attraverso
     * l’uso di unit testing. 
     * 
     * Rende più semplice, inoltre, la personalizzazione degli URL, attraverso il meccanismo di 
     * URL routing integrato, e l’utilizzo di AJAX, dato che i controller non sono accoppiati 
     * direttamente con l’HTML e, anzi, non sono obbligatoriamente orientati a un tipo di output 
     * in particolare. 
     * 
     * Questo si traduce, per esempio, nella possibilità di cambiare facilmente le view in base 
     * al device, senza toccare il controller, così da offrire una versione mobile del sito con 
     * minor sforzo.
     * 
     * Viceversa, ASP.NET Web Forms facilita l’utilizzo di controlli, offrendo un ricco set di
     * funzionalità già pronte e un altrettanto ricco ecosistema di terze parti che aggiungono
     * ulteriori controlli, con un supporto a design time e un modello orientato agli eventi che è
     * apprezzato da tanti sviluppatori che hanno meno dimestichezza con HTML, JavaScript e CSS.
     * 
     * In definitiva, ASP.NET Web Forms e ASP.NET MVC consentono di fare la stessa cosa, cioè 
     * creare applicazioni web basate su ASP.NET e il .NET Framework, semplicemente partendo 
     * da presupposti diversi. 
     * 
     * 
     * 
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
