using System;

namespace DotNetDesignPatternDemos.Architectural.NTier
{
    /*
     *  I grandi prodotti sono spesso costruiti su un'architettura a più livelli o 
     *  un'architettura a più livelli, come viene spesso chiamata. 
     *  
     *  In Stackify, amiamo parlare dei numerosi strumenti, risorse e concetti che 
     *  possono aiutarti a costruire meglio. (dai un'occhiata a più dei nostri 
     *  suggerimenti e trucchi qui) Quindi, in questo post, discuteremo dell'architettura 
     *  a più livelli, di come funziona e di cosa devi sapere per creare prodotti migliori 
     *  utilizzando l'architettura a più livelli.
     *  
     *  Definizione di architettura a più livelli
     *  
     *  L'architettura a più livelli è anche chiamata architettura multilivello perché 
     *  il software è progettato per avere le funzioni di elaborazione, gestione dei 
     *  dati e presentazione fisicamente e logicamente separate. 
     *  
     *  Ciò significa che queste diverse funzioni sono ospitate su più computer o cluster, 
     *  garantendo che i servizi siano forniti senza che le risorse vengano condivise e, 
     *  come tali, questi servizi siano forniti alla massima capacità. 
     *  
     *  La "N" nel nome architettura a più livelli si riferisce a qualsiasi numero da 1.
     *  
     *  Non solo il tuo software guadagna dall'essere in grado di ottenere servizi al 
     *  miglior tasso possibile, ma è anche più facile da gestire. 
     *  
     *  Questo perché quando si lavora su una sezione, le modifiche apportate non influiranno 
     *  sulle altre funzioni. E se c'è un problema, puoi facilmente individuare dove ha origine.
     * 
     * Uno sguardo più approfondito all'architettura a più livelli
     * 
     *      L'architettura a più livelli comporterebbe la divisione di un'applicazione in 
     *      tre diversi livelli. Questi sarebbero i:
     *      
     *              Il livello logico
     *              Il livello di presentazione
     *              Il livello dati
     *              
     *      // Figura1.jpg
     *   
     * La posizione fisica separata di questi livelli è ciò che differenzia l'architettura a 
     * più livelli dal framework model-view-controller che separa solo i livelli di presentazione, 
     * logica e dati nel concetto. 
     * 
     * L'architettura a più livelli differisce anche dal framework MVC in quanto il primo ha un 
     * livello intermedio o un livello logico, che facilita tutte le comunicazioni tra i diversi 
     * livelli. 
     * 
     * Quando si utilizza il framework MVC, l'interazione che avviene è triangolare; invece di 
     * passare attraverso il livello logico, è il livello di controllo che accede ai livelli del 
     * modello e della vista, mentre il livello del modello accede al livello della vista. 
     * 
     * Inoltre, il livello di controllo crea un modello utilizzando i requisiti e quindi spinge 
     * tale modello nel livello di visualizzazione.
     * 
     * Questo non vuol dire che sia possibile utilizzare solo il framework MVC o l'architettura 
     * a più livelli. Ci sono molti software che riuniscono questi due framework. 
     * 
     * Ad esempio, è possibile utilizzare l'architettura a più livelli come architettura 
     * generale o utilizzare il framework MVC nel livello di presentazione.
     * 
     * Quali sono i vantaggi dell'architettura a più livelli?
     * 
     * L'utilizzo dell'architettura a più livelli per il software offre diversi vantaggi. 
     * Questi sono scalabilità, facilità di gestione, flessibilità e sicurezza.
     * Sicuro:              È possibile proteggere ciascuno dei tre livelli separatamente utilizzando metodi diversi.
     * Facile da gestire:   È possibile gestire ogni livello separatamente, aggiungendo o modificando ogni livello 
     *                      senza influire sugli altri livelli.
     * Scalabile:           Se è necessario aggiungere più risorse, è possibile farlo per livello, 
     *                      senza influire sugli altri livelli.
     * Flessibile:          Oltre alla scalabilità isolata, è anche possibile espandere ogni 
     *                      livello in qualsiasi modo richiesto dai requisiti.
     *                      
     * In breve, con l'architettura a più livelli, è possibile adottare nuove tecnologie e 
     * aggiungere più componenti senza dover riscrivere l'intera applicazione o riprogettare 
     * l'intero software, rendendo così più facile la scalabilità o la manutenzione. 
     * 
     * Nel frattempo, in termini di sicurezza, è possibile archiviare informazioni sensibili o 
     * riservate nel livello logico, tenendole lontane dal livello di presentazione, rendendole 
     * così più sicure.
     * 
     * Altri vantaggi includono:
     * 
     *  -   Sviluppo più efficiente.                L'architettura a più livelli è molto amichevole per lo 
     *                                              sviluppo, poiché team diversi possono lavorare su ciascun 
     *                                              livello. In questo modo, puoi essere sicuro che i 
     *                                              professionisti della progettazione e della presentazione 
     *                                              lavorino sul livello di presentazione e che gli esperti 
     *                                              del database lavorino sul livello dati.
     * - Facile da aggiungere nuove funzionalità.   Se si desidera introdurre una nuova funzionalità, 
     *                                              è possibile aggiungerla al livello appropriato 
     *                                              senza influire sugli altri livelli.
     * - Facile da riutilizzare.                    Poiché l'applicazione è suddivisa in livelli indipendenti, 
     *                                              è possibile riutilizzare facilmente ogni livello per altri 
     *                                              progetti software. Ad esempio, se si desidera utilizzare 
     *                                              lo stesso programma, ma per un set di dati diverso, è 
     *                                              sufficiente replicare i livelli di logica e presentazione 
     *                                              e quindi creare un nuovo livello dati.
     *
     * Come funziona ed esempi di architettura a più livelli
     * 
     *  Quando si tratta di architettura a più livelli, un'architettura a tre livelli è 
     *  abbastanza comune. In questa configurazione sono disponibili il livello presentazione 
     *  o GUI, il livello dati e il livello logico applicazione.
     *  
     *  Livello logico dell'applicazione. 
     *      Il livello logico dell'applicazione è dove avviene tutto il "pensiero" e sa cosa 
     *      è consentito dalla tua applicazione e cosa è possibile, e prende altre decisioni. 
     *      Questo livello logico è anche quello che scrive e legge i dati nel livello dati.
     *      
     *  Livello dati. 
     *      Il livello dati è dove vengono archiviati tutti i dati utilizzati nell'applicazione. 
     *      È possibile archiviare in modo sicuro i dati su questo livello, effettuare transazioni 
     *      e persino cercare tra volumi e volumi di dati in pochi secondi.
     *      
     *  Livello di presentazione. 
     *      Il livello di presentazione è l'interfaccia utente. Questo è ciò che l'utente del 
     *      software vede e con cui interagisce. Qui è dove inseriscono le informazioni necessarie. 
     *      Questo livello funge anche da intermediario tra il livello dati e l'utente, passando le 
     *      diverse azioni dell'utente al livello logico.
     *     
     * Immagina di navigare sul tuo sito web preferito. Il livello di presentazione è 
     * l'applicazione Web visualizzata. Viene visualizzato in un browser Web a cui si 
     * accede dal computer e dispone di codici CSS, JavaScript e HTML che consentono 
     * di dare un senso all'applicazione Web. Se è necessario accedere, il livello di 
     * presentazione mostrerà le caselle per nome utente, password e pulsante di invio. 
     * Dopo aver compilato e quindi inviato il modulo, tutto ciò verrà trasmesso al 
     * livello logico. Il livello logico avrà JSP, Java Servlets, Ruby, PHP e altri programmi. 
     * Il livello logico verrebbe eseguito in un server Web. E in questo esempio, 
     * il livello dati sarebbe una sorta di database, come un database MySQL, NoSQL o PostgreSQL. 
     * Tutti questi vengono eseguiti su un server di database separato. 
     * Anche le applicazioni Rich Internet e le app mobili seguono la stessa architettura 
     * a tre livelli.
     * 
     * E ci sono modelli di architettura a più livelli che hanno più di tre livelli. 
     * Esempi sono le applicazioni con questi livelli:
     * 
     *      Servizi, ad esempio servizi di stampa, directory o database
     *      Dominio aziendale: il livello che ospiterebbe Java, DCOM, CORBA e altri oggetti 
     *      server applicazioni.
     *      Livello di presentazione
     *      Livello client – o i thin client
     *      
     * Un buon esempio è quando si dispone di un'architettura orientata ai servizi aziendali. 
     * Il bus di servizio aziendale o ESB sarebbe presente come livello separato per facilitare 
     * la comunicazione del livello di servizio di base e del livello di dominio aziendale.
     * 
     * Considerazioni sull'utilizzo dell'architettura a più livelli per le applicazioni
     * 
     *  Poiché lavorerai con diversi livelli, devi assicurarti che la larghezza di banda 
     *  e l'hardware della rete siano veloci. In caso contrario, le prestazioni dell'applicazione 
     *  potrebbero essere lente. Inoltre, ciò significherebbe che dovresti pagare di più per la rete, 
     *  l'hardware e la manutenzione necessaria per assicurarti di avere una migliore larghezza 
     *  di banda di rete.
     *  
     *  Inoltre, utilizzare il minor numero possibile di livelli. 
     *  Ricorda che ogni livello che aggiungi al tuo software o progetto significa 
     *  un ulteriore livello di complessità, più hardware da acquistare, nonché costi 
     *  di manutenzione e distribuzione più elevati. Per dare un senso alle applicazioni 
     *  a più livelli, dovrebbe avere il numero minimo di livelli necessari per godere 
     *  comunque della scalabilità, della sicurezza e di altri vantaggi offerti 
     *  dall'utilizzo di questa architettura. Se sono necessari solo tre livelli, 
     *  non distribuire quattro o più livelli.
     *  
     *   Si noti che c'è una grande differenza tra N-Layer e N-Tiers. N-Layers si occupa di livelli
     *   software separati e consente di raggruppare il codice in modo logico all'interno 
     *   dell'applicazione. N-Tiers, d'altra parte, si occupa della posizione fisica dei componenti 
     *   software: ad esempio le macchine in cui viene eseguito il codice. 
     *   Questa esempio si occupa esclusivamente di N-Layer, anche se è possibile 
     *   riutilizzarne gran parte anche in un'applicazione A più livelli.
     *  
     *  ----------------------------------------------------------------------------------
     *  
     *  Nell'esempio in allegato possiamo vedre come l'applicazione è disposta su tre livelli
     *  dove ogni livello è a seguito di quello che viene più esterno dove si accettano le richieste
     *  da client disposti nel web in questo caso a quello più interno applicativo che si occupa
     *  di tutta la parte logica applicativa e di servizio e comunica a sua volta con il 
     *  terzo strato quello dei dati DataAccess.
     *  Il DataAccess stesso usa altri due strati di collaborazione per fare le sue operazioni
     *  ed avere le definizioni di Entities per la parte Core e Service per la parte Shared.
     *  
     *      // Figura1.jpg
     *      
     *  La componentistica completa di questo esempio usa le seguenti teconologie.:
     *  
     *  .NET 6
     *  ASP.NET Core 6
     *  Swagger (Documentation)
     *  Entity Framework Core (SQL Server)
     *  ASP.NET Core Identity (SQL Server)
     *  AutoMapper
     *  FluentValidation
     *  NUnit (Integration tests)
     *  XUnit (Unit tests)
     *  FluentAssertion (Testing projects)
     *  NBuilder (Testing projects)
     *      
     *  Scaricare il file in allegato per avere il Progetto completo da testare.
     *  
     *      // N-Tier-Architecture-master.zip
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
