using System;

namespace DotNetDesignPatternDemos.Architectural.NakedObjects
{
    /*
     *  Naked Objects è un modello architettonico utilizzato nell'ingegneria del software. 
     *  È definito da tre principi:
     *  
     *      - Tutta la logica di business deve essere incapsulata negli oggetti di dominio. 
     *        Questo principio non è unico per gli oggetti nudi; è un forte impegno per 
     *        l'incapsulamento.
     *        
     *      - L'interfaccia utente deve essere una rappresentazione diretta degli oggetti 
     *        di dominio, con tutte le azioni utente costituite dalla creazione, dal recupero 
     *        o dal richiamo di metodi sugli oggetti di dominio. 
     *        Questo principio non è unico per il naked objects: 
     *          è un'interpretazione di un'interfaccia utente orientata agli oggetti.
     *      
     *      - La caratteristica innovativa del modello di oggetto nudo nasce combinando 
     *      il 1 ° e il 2 ° principio in un 3 ° principio:
     *        L'interfaccia utente è creata in modo completamente automatico a partire 
     *        dalle definizioni degli oggetti di dominio. 
     *        Questo può essere fatto usando la riflessione o la generazione di codice sorgente.
     *        
     * Vantaggi
     * 
     * La tesi di Pawson sostiene quattro benefici per il modello:
     * 
     *  - Un ciclo di sviluppo più veloce, perché ci sono meno livelli da sviluppare. 
     *    In una progettazione più convenzionale, lo sviluppatore deve definire e 
     *    implementare tre o più livelli separati: 
     *      - il livello oggetto dominio, 
     *      - il livello presentazione e 
     *      - gli script di attività o processo che collegano i due. 
     *      
     *    Se il modello di oggetti naked è combinato con la mappatura relazionale degli 
     *    oggetti o con un database di oggetti, è possibile creare tutti i livelli del sistema 
     *    dalle sole definizioni degli oggetti di dominio; 
     *    tuttavia, questo non fa parte del modello di oggetti nudi di per sé. 
     *    
     *    La tesi include un caso di studio che confronta due diverse implementazioni della 
     *    stessa applicazione: una basata su un'implementazione convenzionale a "4 strati"; 
     *    l'altro usando oggetti naked.
     *    
     *    
     * - Maggiore agilità, riferendosi alla facilità con cui un'applicazione può essere modificata 
     *   per adattarsi a futuri cambiamenti nei requisiti aziendali. 
     *   
     *   In parte ciò deriva dalla riduzione del numero di strati sviluppati che devono essere 
     *   mantenuti in sincronia. 
     *   
     *   Tuttavia, viene anche affermato che la corrispondenza 1:1 forzata tra la presentazione 
     *   utente e il modello di dominio, impone una modellazione di oggetti di qualità superiore, 
     *   che a sua volta migliora l'agilità.
     *   
     * - Uno stile di interfaccia utente più potente. 
     *   Questo vantaggio è in realtà attribuibile all'interfaccia utente orientata agli oggetti 
     *   risultante (OOUI), piuttosto che agli oggetti naked di per sé, anche se si sostiene che 
     *   gli oggetti nudi rendono molto più facile concepire e implementare un OOUI.
     *   
     *   Analisi dei requisiti più semplice. L'argomento qui è che con il modello naked objects, 
     *   gli oggetti di dominio formano un linguaggio comune tra utenti e sviluppatori e che 
     *   questo linguaggio comune facilita il processo di discussione dei requisiti - 
     *   perché non ci sono altre rappresentazioni da discutere. 
     *   In combinazione con il ciclo di sviluppo più rapido, diventa possibile prototipare 
     *   applicazioni funzionali in tempo reale.
     *   
     * Uso
     * 
     *  Il Dipartimento della Protezione Sociale (DSP) (precedentemente noto come 
     *  Dipartimento per gli Affari Sociali e Familiari) in Irlanda ha costruito una suite di 
     *  applicazioni aziendali utilizzando il modello naked objects. 
     *  
     *  Nell'ambito del suo programma SDM (Service Delivery Modernisation), il DSP ha progettato 
     *  una nuova architettura aziendale sia per soddisfare i nuovi requisiti aziendali pianificati 
     *  sia per fornire una maggiore agilità a lungo termine. 
     *  
     *  Il modello naked objects costituisce un elemento chiave dell'architettura SDM. [4] 
     *  Nel novembre 2002 il DSP ha presentato una nuova domanda per sostituire il suo sistema 
     *  esistente per la gestione degli assegni familiari. 
     *  
     *  Si ritiene che questa sia la prima applicazione operativa del modello di oggetti naked, 
     *  ovunque. 
     *  
     *  L'esperienza del DSP nella costruzione di questa prima applicazione, comprese le reazioni 
     *  degli utenti all'interfaccia utente radicale, è ampiamente documentata nella tesi 
     *  di Pawson,[1] e più recentemente in una presentazione al QCon London 2011. 
     *  
     *  Uno degli aspetti più sorprendenti dell'esperienza DSP è stato il modo in cui la tecnica 
     *  Naked Objects ha permesso il riutilizzo molto attivamente. 
     *  
     *  Una volta che un oggetto di dominio, come un Cliente, era stato definito per una 
     *  "applicazione", poteva essere (è stato) facilmente adattato con il minimo di modifiche e 
     *  aggiunte per l'uso altrove. 
     *  
     *  Ciò suggerisce che l'approccio potrebbe diventare uno dei preferiti nei circoli 
     *  governativi, dove il riutilizzo è visto come una potente tecnica per abbattere 
     *  i sistemi a silos. 
     *  
     *  La politica del "governo trasformazionale" del Regno Unito è particolarmente desiderosa 
     *  di vedere il riutilizzo diventare un requisito standard dei nuovi sistemi governativi, 
     *  sia consumando altri componenti del sistema governativo sia rendendone disponibili di 
     *  nuovi per altri. 
     *  
     *  Questo riutilizzo è spesso visto in termini di servizi, ma gli oggetti potrebbero 
     *  essere un approccio altrettanto potente.
     *  
     *  L'iniziale "Naked Object Architecture" del DSP è stata sviluppata da un appaltatore 
     *  esterno, ma l'architettura è stata successivamente riqualificata attorno al Naked Objects 
     *  Framework che ora costituisce la base per lo sviluppo futuro delle applicazioni, 
     *  come confermato nella richiesta di gare d'appalto per un programma quadriennale di 
     *  ulteriori applicazioni da costruire utilizzando oggetti naked.
     *  
     *  Relazione con altre idee
     *  
     *  Il modello di oggetti naked ha rilevanza per diverse altre discipline e / o tendenze, 
     *  tra cui:
     *  
     *      - Meccanismi di object storage
     *      Il mapping relazionale a oggetti, i database a oggetti e la persistenza degli oggetti 
     *      sono tutti interessati a eliminare la necessità di scrivere un livello di accesso ai 
     *      dati convenzionale sotto gli oggetti di dominio. 
     *      Questi modelli sono complementari e potenzialmente sinergici con il modello naked 
     *      objects, che si occupa di eliminare la necessità di scrivere livelli sopra gli 
     *      oggetti di dominio.
     *      
     *      - Sviluppo software agile
     *      Naked objects è compatibile con la tendenza verso metodologie di sviluppo agile 
     *      in molti modi diversi, ma soprattutto con lo sviluppo iterativo a grana fine. 
     *      L'esperienza DSP (descritta sopra) è stata probabilmente anche la più grande 
     *      applicazione di tecniche di sviluppo software agile all'interno di un'organizzazione 
     *      del settore pubblico, in tutto il mondo. 
     *      
     *     - Progettazione basata sul dominio
     *     La progettazione basata sul dominio è l'idea che un modello di dominio (oggetto) in 
     *     evoluzione debba essere utilizzato come meccanismo per aiutare a esplorare i requisiti 
     *     piuttosto che viceversa. 
     *     Il fatto che un sistema di oggetti naked forza la corrispondenza diretta tra 
     *     l'interfaccia utente e il modello di dominio rende più facile tentare la progettazione 
     *     guidata dal dominio e rende i vantaggi più visibili. 
     *     
     *  

Architettura model-driven (MDA)
Sebbene gli oggetti nudi non siano conformi alla rigorosa definizione di MDA, condividono molti degli stessi obiettivi. Dan Haywood ha sostenuto che gli oggetti nudi sono un approccio più efficace per raggiungere tali obiettivi. [10]

Oggetti riposanti
Standard per la creazione di un'interfaccia RESTful da un modello a oggetti di dominio. Sebbene la specifica Restful Objects non indichi che l'interfaccia deve essere generata riflettente dal modello di dominio, tale possibilità esiste.
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
