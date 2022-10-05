using System;

namespace DotNetDesignPatternDemos.Architectural.Documental
{
    /*
     * Nessuno standard per documentare il software...
     * 
     * Quando pensiamo alla documentazione API, la risposta più ovvia è OpenAPI/Swagger. 
     * Purtroppo, non è così facile quando parliamo di architettura.
     * 
     * In realtà, ci sono molti standard per un documento di architettura software. 
     * Un architetto software utilizzerà UML per questo, un altro adotterà un approccio 
     * completamente diverso. Tuttavia, c'è sempre una cosa in comune (ovviamente). 
     * 
     * La documentazione dell'architettura richiede più di un singolo tipo di diagramma 
     * per essere completa in qualsiasi caso d'uso.
     * 
     * Per questo motivo, è bene avere più strumenti possibili tra cui scegliere. 
     * Ecco perché a The Software House abbiamo deciso di andare con una combinazione di:
     * 
     *      diagramma dell'architettura di alto livello (C4),
     *      
     *      diagramma dello schema di database (ERD),
     *      
     *      diagramma di flusso,
     *      
     *      diagramma di sequenza.
     *      
     * Ovviamente, raramente li usiamo tutti contemporaneamente – dipende dal sistema 
     * architettonico che stiamo progettando – ma il nostro minimo è avere un diagramma C4.
     * 
     * Cos'è C4?
     * 
     * 
     * C4 è un modello che è il più vicino ad essere chiamato standard di documentazione 
     * dell'architettura. È un set di 3 diagrammi principali e 1 opzionale:
     * 
     *      contesto
     *      contenitore
     *      componente
     *      codice
     * 
     * Ogni diagramma si concentra su un livello di dettaglio specifico. 
     * Più entriamo nei dettagli, più spesso abbiamo bisogno di aggiornare quel diagramma. 
     * Ecco perché per alcuni di loro, la chiave è generarli invece di crearli manualmente.
     * 
     * Immaginiamo di essere allo sviluppo di un sistema di fatturazione per la nostra azienda. 
     * Gli utenti principali saranno contabili e dobbiamo integrarlo con la nostra gestione 
     * interna degli utenti, i sistemi di gestione dei progetti e Slack.
     * 
     * Il primo diagramma che dobbiamo creare è un diagramma di contesto.
     * 
     * -- Diagramma di contesto
     * 
     * Si tratta del quadro generale. Non abbiamo bisogno di conoscere i dettagli tecnici qui. 
     * Ciò che è importante per noi sono le persone (attori, che utilizzano il sistema) e i 
     * sistemi software che fanno parte di una soluzione aziendale.
     * 
     * Il contesto è un diagramma che mostreremmo a un uomo d'affari che spiega che tipo di 
     * sistema stiamo costruendo, quali integrazioni ha e chi lo utilizzerà.
     * 
     * Con il nostro sistema di fatturazione, potrebbe assomigliare a questo.
     * 
     *      // Figura 1 : documenting-c4-picture-1-768x460.png 
     *      
     * -- Diagramma contenitore
     * 
     * Il secondo livello è un diagramma contenitore. Questa volta, il nostro obiettivo è quello 
     * di eseguire / distribuire separatamente parti del nostro sistema. 
     * Potremmo confrontarlo con i contenitori Docker.
     * 
     * Ogni servizio/database/storage separato è un contenitore separato in quel diagramma.
     * 
     * Torniamo al nostro Sistema di Fatturazione. Avrà un'API, un Frontend, un database, ma 
     * anche un livello di memorizzazione nella cache e l'archiviazione dei file. 
     * Ognuno di questi sarà un contenitore separato.
     * 
     * Ciò che è importante a questo livello è descrivere la tecnologia utilizzata per quel 
     * contenitore specifico e anche come appare la comunicazione tra i contenitori. 
     * È HTTP? O forse è gRPC? Tali informazioni devono essere su un diagramma.
     * 
     *      // Figura 2 : documenting-c4-picture-2.png
     *      
     * Come puoi vedere, abbiamo molte più informazioni tecniche su questo diagramma. 
     * Possiamo vedere quale tipo di storage utilizziamo, quali tecnologie vengono utilizzate 
     * per lo sviluppo di software frontend e backend e come questi due sistemi comunicano 
     * tra loro. 
     * 
     * Possiamo anche vedere tutte le integrazioni tra contenitori specifici e servizi esterni, 
     * ma non sappiamo nulla dell'effettiva implementazione.
     * 
     * -- Diagramma dei componenti
     * 
     * A questo livello, ci concentreremo sui principali elementi costitutivi di un contenitore 
     * specifico. È qui che saranno visibili molti più dettagli di implementazione.
     * 
     * Servizi, repository, gestori di comandi, client di servizi esterni e persino 
     * endpoint: tutti questi elementi potrebbero essere inseriti in questo diagramma. 
     * Ciò che è importante è mostrare non solo gli elementi costitutivi, ma anche la 
     * comunicazione/relazione tra questi.
     * 
     * Quindi, come sarebbe nel nostro sistema di fatturazione? Concentriamoci sull'API di 
     * fatturazione. I componenti principali saranno diversi gestori di comandi, client per 
     * comunicare con Keycloak e Slack. 
     * 
     * Avremmo anche bisogno di alcuni repository, un servizio di gestione PDF e l'integrazione 
     * webhook per JIRA.
     * 
     *      // Figura 3 : documenting-c4-picture-3.png
     *      
     * Come puoi vedere, un diagramma dei componenti non consiste nel mettere ogni dettaglio di 
     * implementazione, ma nel concentrarsi su quelli che sono i più importanti. 
     * 
     * Alla Software House, seguiamo anche alcune regole:
     * 
     *      - ogni freccia deve avere una descrizione
     *      - ogni nome che usiamo deve essere un nome che viene utilizzato 
     *        in un codice (fondamentalmente quando chiamiamo qualcosa un generatore 
     *        di PDF, ci deve essere una cosa nel codice per quello),
     *      - ogni endpoint/modo di comunicazione deve essere visibile,
     *      - ogni colore ha un significato (di solito aggiungiamo anche una legenda per quello).
     * 
     * Queste semplici regole rendono più facile seguire e progettare nuovi diagrammi poiché 
     * tutti in un'azienda lo fanno allo stesso modo.
     * 
     * -- Diagramma del codice
     * 
     * L'ultimo diagramma che fa parte di una famiglia C4 è un diagramma di codice. 
     * Questo è quello che verrà aggiornato più spesso, ma è anche quello opzionale. 
     * In TSH, non usiamo davvero questo diagramma. 
     * 
     * Tuttavia, la chiave qui è generarlo dal codice stesso.
     * 
     * Non è così difficile come potrebbe sembrare. In realtà, potremmo usare alcune proprietà 
     * di annotazioni/documentazione per generare un diagramma di classe UML abbastanza buono 
     * che descriverà il nostro codice e come tutto è collegato tra loro.
     * 
     *      // Figura 4 : class-diagram-example-hasp-licensing-domain-800x580.png
     * 
     * Entra così tanto nei dettagli che con ogni richiesta pull apparirà diverso, ecco perché 
     * non c'è modo di crearli manualmente.
     * 
     * Cosa c'è dopo?
     * 
     * C4 è un modello relativamente semplice per descrivere la tua architettura. 
     * C'è un buon vantaggio nell'utilizzare un tale approccio: 
     * 
     *      - l'intero sistema è più facile da capire, rendendo l'onboarding di nuovi 
     *        sviluppatori più veloce e più facile.
     *        
     *      - Usando C4 tutti possono capire il sistema al livello che è utile per loro. 
     *        Non tutti hanno bisogno di conoscere i dettagli di implementazione, tuttavia, 
     *        tutti dovrebbero essere in grado di comprendere gli obiettivi di alto livello 
     *        di un sistema e quali servizi vengono utilizzati per farlo funzionare.
     *        
     * Ovviamente, C4 non è la risposta a tutto. In un sistema più complesso, avremmo bisogno 
     * di ulteriori tipi di diagrammi: ERD, sequenza, diagrammi di flusso, ecc. Tuttavia, C4 è 
     * un ottimo punto di partenza.
     * 
     * Una spiegazione più completa del modello C4 è visionabile sul sito.:
     * 
     * https://c4model.com/
     * 
     *  Tools
     * 
     *      Sul sito https://online.visual-paradigm.com/diagrams/features/c4-model-tool/
     *      è possibile realizzare in modo visuale dei modelli c4 e sono presenti template.
     *      
     *      Sul sito https://app.diagrams.net/?src=about
     *      è possibile realizzare in modo visuale dei modelli generici e sono presenti template.
     *      
     *      PlantUML script per realizzare con codice diagrammi completi
     *      PlantUML Usando come Extension in VS Code
     *      
     *      Semplice ma carina javascript library su https://bramp.github.io/js-sequence-diagrams/
     *      che permette in codice di creare diagrammi di sequenza.
     *      
     *      etc. (Esistono svariati tool in giro per creare documentazione e disegni) ognuno
     *      può adattare strategicamente i tool che più preferisce per rendere anche processi
     *      noisioi di documentazioni automatizzandoli con processi interni legati ad esempio
     *      al codice.
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
