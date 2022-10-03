using System;

namespace DotNetDesignPatternDemos.Architectural.Onion
{
    /*
     *  Il nome "Onion Architecture" è stato concepito da Jeffrey Palermo, nel tentativo di 
     *  creare un altro modello di architettura denominata tra l'industria fornendo ai 
     *  professionisti un termine comune da utilizzare quando si comunica un approccio 
     *  "diverso" nei modelli. 
     *  
     *  Molti esperti del settore sono venuti prima con modelli architettonici come 
     *  l'architettura esagonale di Alistair Cockburn e Screaming Architecture di Robert 
     *  Martin (Zio Bob) che riuniscono il principio dell'architettura esagonale, 
     *  dell'architettura Clean e di altri modelli. Ward Cunningham e Martin Fowler 
     *  sono stati anche determinanti nei modelli architettonici. 
     *  
     *  Questo è stato creato per essere utilizzato come un approccio non mainstream, 
     *  non una svolta nella nuova tecnica.
     *  
     *  La premessa principale di Principal
     *  
     *  Onion Architecture è che controlla l'accoppiamento. La regola fondamentale è che tutto 
     *  il codice può dipendere da livelli più centrali, ma il codice non può dipendere da 
     *  livelli più lontani dal nucleo. In altre parole, tutto l'accoppiamento è verso il centro. 
     *  
     *  Questa architettura è spudoratamente orientata verso la programmazione orientata 
     *  agli oggetti e mette gli oggetti prima di tutti gli altri.
     *  
     *  Principi chiave di Onion Architecture:
     *  
     *   - L'applicazione è costruita attorno a un modello a oggetti indipendente
     *   - I livelli interni definiscono le interfacce. I livelli esterni implementano le interfacce
     *   - La direzione di accoppiamento è verso il centro
     *   - Tutto il codice di base dell'applicazione può essere compilato ed eseguito separatamente 
     *     dall'infrastruttura
     *  
     * Il database non è il centro. È esterno. Con Onion Architecture, non ci sono 
     * applicazioni di database. Esistono applicazioni che potrebbero utilizzare un database 
     * come servizio di archiviazione, ma solo attraverso un codice di infrastruttura 
     * esterno che implementa un'interfaccia che ha senso per il core dell'applicazione. 
     * Il disaccoppiamento dell'applicazione dal database, dal file system, ecc., 
     * Riduce il costo di manutenzione per tutta la durata dell'applicazione.
     * 
     * Onion Architecture si basa fortemente sul principio di inversione delle dipendenze. 
     * Il core ha bisogno dell'implementazione delle interfacce core. 
     * Se queste classi risiedono ai margini dell'applicazione, un meccanismo per iniettare il 
     * codice in fase di esecuzione in modo che l'applicazione possa fare qualcosa di utile.
     * 
     * Funziona bene per le applicazioni in ambienti DevOps professionali e il modello 
     * dimostra come le risorse DevOps sono organizzate in relazione al resto del codice.
     * 
     * Al centro vediamo il Modello di Dominio, che rappresenta la combinazione di stato e 
     * comportamento che modella la verità per l'organizzazione. Intorno al modello di dominio 
     * ci sono altri livelli con più comportamento. 
     * 
     * I layer esterni al core dell'applicazione sono le classi ConferenceRepository e UserSession.
     * Queste due classi implementano ciascuna un'interfaccia più vicina al centro di se stessa. 
     * In fase di esecuzione, il contenitore Inversion of Control esaminerà il registro e 
     * costruirà le classi appropriate per soddisfare le dipendenze del costruttore di 
     * SpeakerController, che è il seguente:
     * 
     *   IUserSession userSession, IClock clock) : base(userSession)
     *   {
     *      conferenceRepository = conferenceRepository;
     *      _clock = clock;
     *      _userSession = userSession;
     *   }
     *   
     * Martin Fowler, nel suo articolo Inversion of Control Containers and the Dependency Injection 
     * Pattern, aiuta a capire come funziona il pattern.[6] 
     * 
     * In fase di esecuzione, il contenitore IoC risolverà le classi che implementano le interfacce 
     * e le passerà nel costruttore SpeakerController. A questo punto, l'AltoparlanteController può 
     * fare il suo lavoro.
     * 
     * In base alle regole dell'architettura Onion, SpeakerController può utilizzare userSession 
     * direttamente poiché si trova nello stesso livello, ma non può utilizzare direttamente 
     * ConferenceRepository. 
     * 
     * Deve basarsi su qualcosa di esterno che passa in un'istanza di IConferenceRepository. 
     * Questo modello viene utilizzato in tutto e il contenitore IoC rende questo processo senza 
     * soluzione di continuità.
     * 
     * Una panoramica approfondita di questo modello è possibile vederla presso il sito dello stesso
     * che l'o ha inventato Jeffrey Palermo al seguente indirizzo.:
     * 
     * https://jeffreypalermo.com/2008/07/the-onion-architecture-part-1/?ref=hackernoon.com
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
