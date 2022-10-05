using System;

namespace DotNetDesignPatternDemos.DataAccess.ORM.Data
{
    /*
     * L'Object Relational Mapping come tecnica di programmazione affrontata nel precedente
     * esempio dimostra come il pattern esegue la procedua di map di una entità rispetto
     * ad un connubio con una sorgente XML.
     * 
     * Ma per fare la stessa cosa verso un DB che fornisce schemi di database relazionale
     * già pronti ed eseguire il mapping tra questi oggetti dello schema come tabelle indici
     * chiavi relazioni diventa un compito arduo implementare tutto questo ma fortunatamente
     * esistono già prodotti sviluppati intorno a questo pattern e sono definiti appunto
     * librerie di classe ORM.
     * 
     * Sono liberie specializzate secondo questo modello ed ognuna offre un parco di funzionalità
     * aggiuntive oltre il modello concettuale per l'automazione e organizzazione delle entità
     * mappate.
     * 
     * I principali vantaggi nell'uso di un tale sistema sono i seguenti.
     * 
     *      -   Il superamento (più o meno completo) dell'incompatibilità di fondo tra il 
     *          progetto orientato agli oggetti ed il modello relazionale sul quale è basata 
     *          la maggior parte degli attuali RDBMS utilizzati.
     *          
     *      -   Un'elevata portabilità rispetto alla tecnologia DBMS utilizzata: 
     *          cambiando DBMS non devono essere riscritte le routine che implementano lo 
     *          strato di persistenza; generalmente basta cambiare poche righe nella 
     *          configurazione del prodotto per l'ORM utilizzato.
     *          
     *      -   Drastica riduzione della quantità di codice sorgente da redigere; 
     *          l'ORM maschera dietro semplici comandi le complesse attività di creazione, 
     *          prelievo, aggiornamento ed eliminazione dei dati (dette CRUD - Create, Read, 
     *          Update, Delete). 
     *          Tali attività occupano di solito una buona percentuale del tempo di stesura, 
     *          testing e manutenzione complessivo. 
     *          Inoltre, sono per loro natura molto ripetitive e, dunque, favoriscono la 
     *          possibilità che vengano commessi errori durante la stesura del codice che le 
     *          implementa.
     *          
     *      -   Suggerisce la realizzazione dell'architettura di un sistema software mediante 
     *          approccio stratificato, tendendo pertanto ad isolare in un solo livello la 
     *          logica di persistenza dei dati, a vantaggio della modularità complessiva del sistema.
     *          
     * I prodotti per l'ORM attualmente più diffusi offrono spesso nativamente funzionalità 
     * che altrimenti andrebbero realizzate manualmente dal programmatore:
     * 
     *      -   Caricamento automatico del grafo degli oggetti secondo i legami di associazione 
     *          definiti a livello di linguaggio. Il caricamento di un'ipotetica istanza della 
     *          classe Studente, potrebbe automaticamente produrre il caricamento dei dati 
     *          collegati sugli esami sostenuti. 
     *          Tale caricamento, in più, può avvenire solo se il dato è effettivamente richiesto 
     *          dal programma, ed è altrimenti evitato (tecnica nota con il nome di 
     *          lazy-initialization).
     *          
     *      -   Gestione della concorrenza nell'accesso ai dati durante conversazioni. 
     *          Conflitti durante la modifica di un dato da parte di più utenti in contemporanea, 
     *          possono essere automaticamente rilevati dal sistema ORM.
     *          
     *      -   Meccanismi di caching dei dati. Per esempio, se accade che uno stesso dato venga 
     *          prelevato più volte dal RDBMS, il sistema ORM può fornire automaticamente un 
     *          supporto al caching che migliori le prestazioni dell'applicazione e riduca il 
     *          carico sul sistema DBMS.
     *      
     *      -   Gestione di una conversazione mediante uso del design pattern Unit of Work, che 
     *          ritarda tutte le azioni di aggiornamento dei dati al momento della chiusura della 
     *          conversazione; in questo modo le richieste inviate al RDBMS sono quelle 
     *          strettamente indispensabili (per es. viene eseguita solo l'ultima di una serie 
     *          di update su uno stesso dato, oppure non viene eseguita affatto una serie di 
     *          update su un dato che in seguito viene eliminato); inoltre il colloquio con il 
     *          DBMS avviene mediante composizione di query multiple in un unico statement, 
     *          limitando così al minimo il numero di round-trip-time richiesti e, conseguentemente,
     *          i tempi di risposta dell'applicazione.
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
