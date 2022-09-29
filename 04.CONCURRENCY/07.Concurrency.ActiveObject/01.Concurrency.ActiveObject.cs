using System;

namespace DotNetDesignPatternDemos.Concurrency.ActiveObject
{
    /*
     *  Il modello di progettazione Active Object separa l'esecuzione del metodo dalla 
     *  chiamata al metodo per migliorare la concorrenza e semplificare l'accesso sincronizzato 
     *  agli oggetti che risiedono nei propri thread di controllo
     * 
     *  Questo modello viene utilizzato in molte applicazioni per gestire più richieste client 
     *  contemporaneamente e quindi per migliorare la qualità del servizio. 
     *  Questo pattern è chiamato Active Object riferendosi al fatto che l'oggetto concorrente 
     *  risiede nel proprio thread di controllo indipendentemente dal thread di controllo del 
     *  client che ha invocato uno dei suoi metodi. 
     *  
     *  Ciò significa che la chiamata al metodo e l'esecuzione effettiva del metodo si 
     *  verificano in due thread diversi. Al client, tuttavia, sembra invocare un metodo 
     *  ordinario. Invece, il metodo proxy costruisce un oggetto request e lo inserisce in 
     *  un elenco di attivazione. Questo elenco non solo contiene gli oggetti della richiesta 
     *  del metodo inseriti dal proxy, ma tiene anche traccia di quale richiesta del metodo 
     *  può essere eseguita. L'oggetto di richiesta del metodo contiene le informazioni 
     *  per eseguire il metodo desiderato in un secondo momento; vale a dire i parametri 
     *  della richiesta e qualsiasi altra informazione di contesto.
     *  
     *  Utilizzando l'elenco di attivazione, i thread del proxy e del servant possono essere 
     *  eseguiti contemporaneamente. Solo l'elenco di attivazione deve essere serializzato 
     *  per proteggerlo dall'accesso simultaneo utilizzando un modello di controllo della 
     *  concorrenza come l'oggetto Monitor. Uno scheduler in esecuzione nel proprio thread 
     *  richiama quindi il metodo corrispondente sul server utilizzando le informazioni 
     *  memorizzate nella richiesta del metodo oggetto prende dall'attivazione list 
     *  connection.
     *  
     *  Poiché il proxy non può restituire il risultato della chiamata al metodo, viene invece 
     *  restituito un segnaposto (se il metodo è una chiamata bidirezionale). 
     *  Un futuro è un meccanismo che può essere utilizzato per fornire valori che verranno 
     *  risolti a un altro valore in modo asincrono. 
     *  
     *  Il proxy può restituire un futuro al suo chiamante e il server calcolerà il valore 
     *  che il futuro si risolverà in un altro thread di controllo. 
     *  Il futuro offre un modo per chiedere il valore effettivo. 
     *  Sono possibili due situazioni:
     * 
     *  1. Il futuro è già risolto. Cioè, il metodo di calcolo del valore è terminato e ha 
     *     archiviato il risultato in futuro. Chiedere il valore del futuro darà immediatamente 
     *     il risultato.
     *  
     *  2. Il futuro non si è risolto. In questo caso, il thread corrente viene bloccato fino 
     *     a quando non si risolve il futuro. Il thread corrente è effettivamente sincronizzato 
     *     con il thread che calcola il valore. Ma fintanto che la logica del programma fa il 
     *     nostro uso immediato del risultato, il thread corrente non è bloccato.
     * 
     * Ricapitolando, il modello di concorrenza dell'oggetto attivo è costituito dai seguenti 
     * partecipanti 
     * 
     *      –A proxy che pubblicizza l'interfaccia di un oggetto attivo visibile ai thread 
     *       del client.
     *       
     *      –Il server fornisce l'implementazione dei metodi definiti nell'interfaccia proxy.
     *      
     *      –Un elenco di attivazione serializzato che contiene gli oggetti di richiesta del metodo 
     *        inseriti dal proxy e consente al servo di funzionare contemporaneamente.
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
