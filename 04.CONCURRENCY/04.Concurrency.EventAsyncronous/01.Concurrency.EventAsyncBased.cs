using System;

namespace DotNetDesignPatternDemos.Concurrency.EventBasedAsyncronous
{
    /*
     * La classe fittizia dispone di due metodi, entrambi i quali supportano le invocazioni 
     * sincrone e asincrone. Gli overload sincroni si comportano come qualsiasi chiamata al 
     * metodo ed eseguono l'operazione sul thread chiamante; se l'operazione richiede molto tempo, 
     * potrebbe verificarsi un notevole ritardo prima che la chiamata ritorni. 
     * Gli overload asincroni avvieranno l'operazione su un altro thread e quindi torneranno 
     * immediatamente, consentendo al thread chiamante di continuare mentre l'operazione viene 
     * eseguita "in background".AsyncExample
     * 
     * Overload di metodi asincroni
     *      Esistono potenzialmente due overload per le operazioni asincrone: 
     *        invocazione singola e invocazione multipla. 
     *      È possibile distinguere questi due moduli in base alle firme dei metodi: 
     *      il modulo di chiamata multipla ha un parametro aggiuntivo chiamato. 
     *      Questo modulo consente al codice di chiamare più volte senza attendere 
     *      il completamento di eventuali operazioni asincrone in sospeso. 
     *      Se, d'altra parte, si tenta di chiamare prima che una chiamata precedente 
     *      sia stata completata, il metodo genera un'eccezione 
     *       InvalidOperationException.userStateMethod1Async(string param, object userState)
     *      
     * Monitoraggio delle operazioni in sospeso
     *      Se si utilizzano gli overload di chiamata multipla, il codice dovrà tenere 
     *      traccia degli oggetti (ID attività) per le attività in sospeso. 
     *      Per ogni chiamata a , in genere si genera un nuovo oggetto univoco e lo si 
     *      aggiunge a una raccolta. Quando l'attività corrispondente a questo oggetto 
     *      genera l'evento di completamento, l'implementazione del metodo di completamento 
     *      esaminerà AsyncCompletedEventArgs.UserState e lo rimuoverà dalla raccolta. 
     *      Utilizzato in questo modo, il parametro assume il ruolo di ID 
     *       userStateMethod1Async(string param, object userState)
     *      
     * Annullamento delle operazioni in sospeso
     *      È importante poter annullare le operazioni asincrone in qualsiasi momento prima 
     *      del loro completamento. Le classi che implementano il modello asincrono basato 
     *      su eventi avranno un metodo (se è presente un solo metodo asincrono) o un metodo 
     *      MethodNameAsyncCancel (se sono presenti più metodi asincroni).CancelAsync
     *      I metodi che consentono più invocazioni accettano un parametro, che può essere 
     *      utilizzato per tenere traccia della durata di ogni attività. 
     *      Accetta un parametro, che consente di annullare particolari attività in 
     *       userStateCancelAsyncuserState
     *      I metodi che supportano solo una singola operazione in sospeso alla volta, 
     *      ad esempio , non sono annullabili Method1Async(string param)
     *      
     *      Il parametro per gli overload a chiamata multipla consente di distinguere 
     *      tra operazioni asincrone. Si fornisce un valore univoco (ad esempio, un GUID o 
     *      un codice hash) per ogni chiamata a , e quando ogni operazione viene completata, 
     *      il gestore eventi può determinare quale istanza dell'operazione ha generato l'evento 
     *      di completamento. 
     *       serStateMethod1Async(string param, object userState)
     * 
     *  Per la ricezione di aggiornamenti di stato di avanzamento come eventi incrementali
     *  è possibile far aderire le classi al modello asincrono che fornirà questi tipi di eventi
     *  come ad esempio 
     *      ProgressChangedEventArgs ProgressChanged
     *      ProgressChangedEventArgs ProgressPercentage 
     *  o usare le proprietà implenetate da queste prorpietà come
     *      ProgressChangedEventArgs UserState 
     *      
     *  Come si vede in questo esempio fittizio e dalle spiegazioni prese dal sito di microsoft
     *  questo modello di progettazione per i modelli di Eventi Asincroni di base è di facile
     *  attuazione grazie alle ultime versioni del linguaggio che adottano nel frameowrk stesso
     *  tutte le convenzioni per implementare questo modello basato su eventi asincroni.
     */

    /* Classe fittizia di esempio */

    /*
    public class AsyncExample
    {
        // Synchronous methods.  
        public int Method1(string param);
        public void Method2(double param);

        // Asynchronous methods.  
        public void Method1Async(string param);
        public void Method1Async(string param, object userState);
        public event Method1CompletedEventHandler Method1Completed;

        public void Method2Async(double param);
        public void Method2Async(double param, object userState);
        public event Method2CompletedEventHandler Method2Completed;

        public void CancelAsync(object userState);

        public bool IsBusy { get; }

        // Class implementation not shown.  
    }
    */

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
