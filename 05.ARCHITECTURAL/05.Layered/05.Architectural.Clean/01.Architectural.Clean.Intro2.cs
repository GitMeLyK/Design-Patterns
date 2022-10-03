using System;

namespace DotNetDesignPatternDemos.Architectural.Clean
{
    /*
     * La Clean Architecture è la linea guida sull'architettura di sistema proposta 
     * da Robert C. Martin (Uncle Bob) derivata da molte linee guida architettoniche 
     * come Hexagonal Architecture, Onion Architecture, ecc... nel corso degli anni.
     * 
     * Questa è una delle linee guida rispettate dagli ingegneri del software per 
     * creare software scalabile, testabile e manutenibile.
     * 
     * Perché abbiamo bisogno di fare l'architetto?
     * 
     * "L'obiettivo dell'architettura software è ridurre al minimo le risorse umane 
     * necessarie per costruire e mantenere il sistema richiesto." ― Robert C. Martin, 
     * Architettura pulita"
     * 
     * Vantaggi di una corretta architettura
     * 
     *      Testabile
     *      Mantenibile
     *      Variabile
     *      Facile da sviluppare
     *      Facile da implementare
     *      Indipendente
     * 
     *      // Figura 1 : Clean ArchitectureScheme.png
     *      
     * Possiamo vedere che ci sono quattro livelli nel diagramma. 
     * Livello blu, livello verde, livello rosso e livello giallo.
     * 
     * Ogni cerchio rappresenta diverse aree del software. 
     * Lo strato più esterno è il livello più basso del software e man mano che ci spostiamo 
     * più in profondità, il livello sarà più alto. In generale, man mano che ci muoviamo più 
     * in profondità, lo strato è meno incline al cambiamento.
     * 
     * Regola di dipendenza
     * 
     * La regola di dipendenza indica che le dipendenze del codice sorgente possono puntare solo 
     * verso l'interno.
     * 
     * Ciò significa che nulla in un cerchio interno può sapere nulla di qualcosa in un cerchio 
     * esterno. cioè il cerchio interno non dovrebbe dipendere da nulla nel cerchio esterno. 
     * Le frecce nere rappresentate nel diagramma mostrano la regola di dipendenza.
     * 
     * Questa è la regola importante che fa funzionare questa architettura. 
     * Inoltre, questo è difficile da capire. 
     * Quindi ho intenzione di infrangere questa regola all'inizio per farti capire quali problemi 
     * porta e poi spiegare e vediamo come tenere il passo con questa regola. 
     * 
     * Prima di tutto, questa rappresentazione circolare potrebbe essere fonte di confusione per 
     * molti. Proviamo quindi a rappresentarlo verticalmente.
     * 
     *  // Figura 2 : Fig2.png
     *  
     *  I colori qui rappresentati sono gli stessi dei colori rappresentati nel diagramma 
     *  dell'architettura pulita.
     *  
     *  Ricorda, la freccia dovrebbe essere letta come "dipende da". cioè dovrebbe dipendere da , 
     *  che dipendono da quali dipendono da .Frameworks and DriversInterface 
     *  AdaptersApplication Business RulesEnterprise Business Rules
     *  
     *  Nulla nello strato inferiore dovrebbe dipendere dallo strato superiore.
     *  
     *  Framework e driver
     *  
     *  Le aree software che risiedono all'interno di questo livello sono
     *  
     *      Interfaccia utente
     *      Banca dati
     *      Interfacce esterne (es: API della piattaforma nativa)
     *      Web (es: Richiesta di rete)
     *      Dispositivi (ad esempio: stampanti e scanner)
     *      Adattatori di interfaccia
     *      
     *      Questo livello tiene
     *          
     *          - Presenters (Logica dell'interfaccia utente, Stati)
     *          - Controllers (Interfaccia che contiene i metodi necessari per l'applicazione 
     *            implementata da Web, dispositivi o interfacce esterne)
     *          - Gateways (Interfaccia che contiene ogni operazione CRUD eseguita 
     *            dall'applicazione, implementata da DB)
     *            
     *  Regole di business dell'applicazione
     *  
     *  Le regole che non sono regole di core-business, ma essenziali per questa particolare 
     *  applicazione rientrano in questo. Questo livello contiene . Come suggerisce il nome, 
     *  dovrebbe fornire ogni caso d'uso dell'applicazione. vale a dire che contiene tutte le 
     *  funzionalità fornite dall'applicazione.
     *  Use Cases
     *  
     *  Inoltre, questo è il livello che determina quale / essere chiamato per il particolare 
     *  caso d'uso. A volte abbiamo bisogno di controller di moduli diversi.
     *  ControllerGateway
     *  
     *  È qui che vengono coordinati diversi moduli. Ad esempio, vogliamo applicare uno sconto 
     *  per l'utente che ha acquistato per x importo entro un mese.
     *  
     *  Qui abbiamo bisogno di ottenere l'importo che l'utente ha speso per questo mese dal e 
     *  quindi con il risultato dobbiamo applicare lo sconto per l'utente nel file . 
     *  Qui chiama il controller del modulo di acquisto per i dati e quindi applica lo sconto 
     *  nel modulo di checkout.
     *  purchase module checkout module applyDiscountUseCase
     *  
     *  Regole aziendali aziendali
     *  
     *  Questo è il livello che contiene le regole di core business o le regole di business 
     *  specifiche del dominio. Inoltre, questo livello è il meno incline al cambiamento.
     *  
     *  La modifica di qualsiasi livello esterno non influisce su questo livello. 
     *  Poiché non cambierà spesso, il cambiamento in questo livello è molto raro. 
     *  Questo livello contiene Entità.
     *  Business Rules
     *  
     *  Un'entità può essere una struttura di dati di base necessaria per le regole di business 
     *  o un oggetto con metodi che contengono la logica di business.
     *  
     *  Ad esempio: il modulo di calcolo nell'applicazione bancaria è la logica di business 
     *  principale che dovrebbe essere all'interno di questo livello.
     *  Interest
     *  
     *  Diamo un'occhiata a un semplice esempio per capirlo bene.
     *  
     *  Nell'esempio viene illustrata un'applicazione semplice con una sola richiesta di rete.
     *  
     *  Come possiamo progettare un'app che traduce la frase data dall'utente utilizzando 
     *  un'API di traduzione? 
     *  
     *      // Fig3.png
     *      
     *  Ogni livello fa una cosa specifica. Sembra buono giusto? Controlliamo il flusso di 
     *  dipendenze per questa architettura sopra per sapere se qualcosa non va.
     *  
     *  Ricordi la regola di dipendenza? "La regola di dipendenza afferma che le dipendenze 
     *  del codice sorgente possono puntare solo verso l'interno".
     *  
     *      // Fig4.png
     *      
     *  Interfaccia utente → Presenter (✅ non in violazione)
     *  Presentatore → tradurre il caso d'uso (✅ non viola)
     *  Translate Usecase → Translate Controller ( ❌ Violazione)
     *  Tradurre Controller → Web ( ❌ Violazione)
     *  
     *  Ma sembra corretto, giusto?
     *  
     *  UI richiede dati da cui richiede dati da cui dovrebbe richiedere dati da cui 
     *  dovrebbe richiedere dati.
     *  Presenter Use Case Controller Web
     *  
     *  "Dopotutto, come possiamo aspettarci di gettare alcuni dati senza che dipenda da esso? 
     *  Inoltre, come possiamo aspettarci di ottenere i dati corretti dall'esterno a seconda di esso?
     *  web Controller Controller Use Case Controller
     *  
     *  Ma la regola di dipendenza dice rigorosamente che le dipendenze possono puntare solo verso 
     *  l'interno. Si somma dicendo che questa è la regola che fa funzionare l'architettura.
     *  
     *  Per passare questa regola, dobbiamo invertire la freccia nella direzione opposta. 
     *  E' possibile? Ecco che arriva il polimorfismo. 
     *  Quando includiamo qui un po' di polimorfismo, succede qualcosa di magico.
     *  
     *  Semplicemente avendo un tra questi 2 strati, potremmo invertire la dipendenza. 
     *  Questo è noto come principio di inversione delle dipendenze.
     *  Interface
     *  
     *  Implementiamo il principio di inversione delle dipendenze nei casi in cui la regola 
     *  di dipendenza viene violata.
     *  
     *      // Fig5.png
     *      
     *      //Fig6.png
     *  
     *  Così il flusso diventa
     *      
     *      // Fig7.png
     *      
     *  Controlliamo ora il flusso di dipendenza per sapere se qualcosa lo viola.
     *  
     *      // Fig8.png
     *      
     *  Ora possiamo vedere che nessuno strato interno dipende da alcuno strato esterno. 
     *  Piuttosto, lo strato esterno dipende dallo strato interno.
     *  
     *  Allora perché lo strato esterno dovrebbe dipendere dallo strato interno ma non 
     *  il contrario?
     *  
     *  Immagina di essere in un hotel. Vogliamo che l'hotel ci serva quello che vogliamo, 
     *  ma non quello che offrono giusto?. La stessa cosa sta accadendo qui, vogliamo che 
     *  il DB fornisca i dati di cui l'applicazione ha bisogno ma non i dati che ha.
     *  
     *  L'applicazione ordina quali dati desidera e non si preoccupa di come DB o API prepara 
     *  i dati. In questo modo, l'applicazione non dipende dal DB o dall'API. 
     *  Se abbiamo bisogno / vogliamo cambiare lo schema DB o API in futuro, 
     *  possiamo semplicemente cambiarlo. Nella misura in cui fornisce ciò che l'applicazione 
     *  richiede, l'applicazione non conosce nemmeno il cambiamento nel DB o nell'API.
     *  
     *  Inoltre, la regola di dipendenza unidirezionale salva l'applicazione dallo stato 
     *  di deadlock. cioè immagina in un'architettura a 2 livelli, il primo livello dipende 
     *  dal secondo livello e il secondo livello dipende dal primo livello. 
     *  In tal caso, se abbiamo bisogno di cambiare qualcosa nel primo strato, rompe il secondo 
     *  strato. Se abbiamo bisogno di cambiare qualcosa nel secondo strato, si rompe il primo 
     *  strato. 
     *  Questo può essere rifiutato seguendo lo stato di deadlock.
     *  
     *  Questa è l'architettura pulita descritta dallo zio Bob.
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
