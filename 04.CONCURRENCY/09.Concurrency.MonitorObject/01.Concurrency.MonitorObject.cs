using System;

namespace _08.Concurrency.MonitorObject
{
    class Program
    {
        /*
         * In molte applicazioni ci sono più thread che invocano metodi su un oggetto 
         * che ne modificano lo stato interno. In questi casi spesso dobbiamo assicurarci 
         * di sincronizzare e programmare l'accesso a questi metodi. 
         * Contrariamente a un oggetto attivo, il monitor non dispone di un proprio thread 
         * di controllo. Eachmethod viene eseguito nel thread del client e l'accesso viene 
         * bloccato fino al ritorno del themethod. 
         * 
         * Inoltre, per prevenire modifiche simultanee incontrollate (condizioni di gara), 
         * è possibile eseguire solo un metodo sincronizzato all'interno del monitor in 
         * qualsiasi momento.
         * 
         * Il Monitor Object risolve questo problema implementando la seguente funzionalità:
         *  
         *  1. L'interfaccia dell'oggetto dovrebbe definire i suoi limiti di sincronizzazione 
         *     e solo un metodo alla volta dovrebbe essere attivo all'interno dello stesso oggetto.
         *  
         *  2. Gli oggetti dovrebbero essere responsabili di garantire che tutti i loro metodi che 
         *     richiedono la sincronizzazione siano serializzati in modo trasparente senza che il 
         *     client ne sappia nulla. Le operazioni vengono invocate come normali chiamate di 
         *     metodo ma garantite che si escludono a vicenda. La sincronizzazione delle condizioni
         *     viene realizzata utilizzando le primitive wait e signal (ad es.wait() enotify() 
         *     nel linguaggio Java).
         *  
         *  3. Se il metodo di un oggetto deve bloccarsi durante l'esecuzione (ad esempio per 
         *     attendere che una condizione diventi vera), non dovrebbe bloccare il thread di 
         *     controllo. In tal caso, altri client dovrebbero essere in grado di accedere 
         *     all'oggetto per prevenire un deadlock e sfruttare i meccanismi di concorrenza 
         *     disponibili sull'hardware.
         *   
         *   4. Quando un metodo interrompe volontariamente il suo filo di controllo, 
         *     le invarianti devono sempre valere; deve lasciare il suo oggetto in uno stato 
         *     stabile.
         *     
         *  Per soddisfare queste proprietà, l'oggetto Monitor sincronizza l'accesso ai suoi metodi.
         *  Per serializzare l'accesso simultaneo allo stato di un oggetto, ogni oggetto Monitor 
         *  contiene un blocco di monitoraggio. 
         *  I metodi sincronizzati possono determinare le circostanze in cui sospendono e 
         *  riprendono la loro esecuzione, in base a una o più condizioni di monitoraggio 
         *  associate a un oggetto Monitor.
         *  
         *      1. L'oggetto monitor stesso che espone i metodi ai client noti per essere 
         *          sincronizzati. Ogni metodo viene eseguito nel thread del client, 
         *          l'Oggetto Monitor stesso non ha il proprio thread di controllo.
         *          
         *      2. Metodi sincronizzati di cui solo uno può essere eseguito in qualsiasi momento. 
         *         Implementano le funzioni thread-safe esportate dall'interfaccia di un oggetto 
         *         Monitor.
         *         
         *      3. Il monitor condition in combinazione con il blocco del monitor determina 
         *         se il metodo asincrono deve sospendere o riprendere l'elaborazione.
         *         
         *  La parola chiave sincronizzata in Java Synchronization assicura che un gruppo di 
         *  istruzioni (blocco asincrono) venga eseguito atomicamente per quanto riguarda tutti 
         *  i thread sincronizzati. 
         *  La sincronizzazione non risolve il problema di quale thread esegue per primo le 
         *  istruzioni: è il primo arrivato, il primo servito. Il linguaggio Java non fornisce 
         *  meccanismi di controllo della concorrenza "convenzionali", come mutex e semafori, 
         *  agli sviluppatori di applicazioni. Prima di poter inserire un blocco sincronizzato, 
         *  un thread deve acquisire la proprietà del monitor per quel blocco. 
         *  Una volta che il thread ha acquisito la proprietà del monitor, nessun altro thread 
         *  sincronizzato sullo stesso monitor può accedere a quel blocco (o qualsiasi altro 
         *  blocco o metodo sincronizzato sullo stesso monitor). 
         *  Il thread proprietario del monitor esegue tutte le istruzioni nel blocco, quindi 
         *  rilascia automaticamente la proprietà del monitor all'uscita dal blocco. 
         *  A quel punto, un altro thread in attesa di entrare nel blocco può acquisire la 
         *  proprietà del monitor.
         *         
         */

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
