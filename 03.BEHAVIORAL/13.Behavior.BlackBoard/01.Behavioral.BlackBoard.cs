using System;

namespace DotNetDesignPatternDemos.Behavioral.Blackboard
{
    /*
     * Il modello di progettazione Blackboard definisce tre componenti principali:
     *      - Blackboard : una memoria globale strutturata contenente oggetti dallo spazio 
     *        della soluzione
     *      - Knowledge sources - moduli specializzati con una propria rappresentazione
     *      - Control Component : seleziona, configura ed esegue i moduli.
     * 
     * Implementazione
     * 
     *      Il primo passo è progettare lo spazio della soluzione (cioè le potenziali soluzioni) 
     *      che porta alla struttura della lavagna. 
     *      Quindi, vengono identificate le fonti di conoscenza. 
     *      Queste due attività sono strettamente correlate.
     *      
     *      Il passaggio successivo consiste nello specificare il componente di controllo; 
     *      generalmente assume la forma di uno scheduler complesso che utilizza un 
     *      insieme di euristiche specifiche del dominio per valutare la rilevanza delle 
     *      fonti di conoscenza eseguibili.
     *      
     *  Applicazioni
     *          I domini di utilizzo includono:
     *                  riconoscimento vocale
     *                  identificazione e tracciamento del veicolo
     *                  identificazione della struttura proteica
     *                  interpretazione dei segnali sonar.
     *                  
     * Il modello Blackboard fornisce soluzioni efficaci per la progettazione e 
     * l'implementazione di sistemi complessi in cui moduli eterogenei devono essere 
     * combinati dinamicamente per risolvere un problema. 
     * Ciò fornisce proprietà non funzionali quali:
     *      - riusabilità
     *      - variabilità
     *      - robustezza. 
     *      
     * Il modello Blackboard consente a più processi di lavorare più vicini su thread separati, 
     * eseguendo il polling e reagendo quando necessario.
     * 
     * Blackboard System
     * 
     * Un sistema blackboard è un approccio di intelligenza artificiale basato sul modello 
     * architettonico della blackboard, in cui una base di conoscenza comune, la "blackboard", 
     * viene aggiornata iterativamente da un gruppo eterogeneo di fonti di conoscenza specialistiche, 
     * a partire da una specifica del problema e termina con una soluzione. 
     * Ogni fonte di informazioni aggiorna la lavagna con una soluzione parziale quando i suoi 
     * vincoli interni corrispondono allo stato della blackboard. 
     * In questo modo, gli specialisti lavorano insieme per risolvere il problema. 
     * Il modello blackboard è stato originariamente progettato come un modo per gestire problemi 
     * complessi e mal definiti, in cui la soluzione è la somma delle sue parti.
     * 
     * Scenario
     * 
     * Lo scenario seguente fornisce una semplice metafora che fornisce alcune informazioni su 
     * come funziona una blackboard:
     *      -   Un gruppo di specialisti è seduto in una stanza con una grande lavagna. 
     *      -   Lavorano come una squadra per fare brainstorming su una soluzione a un problema, 
     *          usando la lavagna come luogo di lavoro per sviluppare in modo cooperativo 
     *          la soluzione.
     *      -   La sessione inizia quando le specifiche del problema vengono scritte sulla lavagna. 
     *          Tutti gli specialisti guardano la lavagna, alla ricerca di un'opportunità per 
     *          applicare la loro esperienza alla soluzione di sviluppo. 
     *          Quando qualcuno scrive qualcosa sulla lavagna che consente a un altro specialista 
     *          di applicare la propria esperienza, il secondo specialista registra il proprio 
     *          contributo sulla lavagna, sperando che consenta ad altri specialisti di applicare 
     *          la propria esperienza. 
     *          Questo processo di aggiunta di contributi alla lavagna continua fino a quando 
     *          il problema non è stato risolto.
     *          
     *          
     *  Un'applicazione di sistema blackboard è costituita da tre componenti principali
     *  
     *      -   I moduli specialistici del software, che sono chiamati fonti di conoscenza (KSs). 
     *          Come gli esperti umani di una lavagna, ogni fonte di conoscenza fornisce competenze 
     *          specifiche necessarie per l'applicazione.
     *          
     *      -   La lavagna, un archivio condiviso di problemi, soluzioni parziali, suggerimenti 
     *          e informazioni fornite. La lavagna può essere pensata come una "libreria" dinamica 
     *          di contributi al problema attuale che sono stati recentemente "pubblicati" da altre 
     *          fonti di conoscenza.
     *          
     *      -   Il guscio di controllo, che controlla il flusso dell'attività di risoluzione dei 
     *          problemi nel sistema. Proprio come gli specialisti umani desiderosi hanno bisogno 
     *          di un moderatore per impedire loro di calpestarsi a vicenda in una folle corsa 
     *          per afferrare il gesso, i KS hanno bisogno di un meccanismo per organizzare il 
     *          loro uso nel modo più efficace e coerente. 
     *          In un sistema blackboard, questo è fornito dal guscio di controllo.
     * 
     * Learnable Task Modeling Language
     *  Un sistema blackboard è lo spazio centrale in un sistema multi-agente. 
     *  Viene utilizzato per descrivere il mondo come una piattaforma di comunicazione per gli 
     *  agenti. Per realizzare una blackboard in un programma per computer, è necessaria una 
     *  notazione leggibile dalla macchina in cui i fatti possono essere memorizzati. 
     *  Un tentativo in questo senso è un database SQL, un'altra opzione è il Learnable Task 
     *  Modeling Language (LTML). La sintassi del linguaggio di pianificazione LTML è simile a PDDL, 
     *  ma aggiunge funzionalità extra come strutture di controllo e modelli OWL-S. 
     *  LTML è stato sviluppato nel 2007 come parte di un progetto molto più ampio chiamato POIROT 
     *  (Plan Order Induction by Reasoning from One Trial), che è un framework Learning from 
     *  demonstrations per il process mining. In POIROT, le tracce e le ipotesi di Plan sono 
     *  memorizzate nella sintassi LTML per la creazione di servizi Web semantici. 
     *  
     *  Ecco un piccolo esempio: un utente umano sta eseguendo un flusso di lavoro in un gioco 
     *  per computer. L'utente preme alcuni pulsanti e interagisce con il motore di gioco. 
     *  Mentre l'utente interagisce con il gioco, viene creata una traccia del piano. 
     *  Ciò significa che le azioni dell'utente sono memorizzate in un file di registro. 
     *  Il file di registro viene trasformato in una notazione leggibile dalla macchina 
     *  che è arricchita da attributi semantici. 
     *  Il risultato è un file di testo nella sintassi LTML che viene messo sulla lavagna. 
     *  Gli agenti (programmi software nel sistema blackboard) sono in grado di analizzare 
     *  la sintassi LTML.
     *  
     *  I sistemi blackboard erano popolari prima dell'inverno dell'IA e, insieme alla maggior 
     *  parte dei modelli di intelligenza artificiale simbolici, caddero di moda durante quel 
     *  periodo. Insieme ad altri modelli ci si rese conto che i successi iniziali sui problemi 
     *  dei giocattoli non si adattavano bene ai problemi reali sui computer disponibili 
     *  dell'epoca. La maggior parte dei problemi che utilizzano le lavagne sono intrinsecamente 
     *  NP-hard, quindi resistono alla soluzione trattabile da qualsiasi algoritmo nel limite 
     *  delle grandi dimensioni. 
     *  Durante lo stesso periodo, il riconoscimento dei modelli statistici è diventato dominante, 
     *  in particolare attraverso semplici modelli di Markov nascosti che hanno superato 
     *  approcci simbolici come Hearsay-II nel dominio del riconoscimento vocale.
     *  
     *  Sviluppi recenti
     *  I sistemi simili a lavagne sono stati costruiti all'interno delle moderne impostazioni 
     *  di apprendimento automatico bayesiano, utilizzando agenti per aggiungere e rimuovere nodi 
     *  di rete bayesiani. In questi sistemi di "lavagna bayesiana", l'euristica può acquisire 
     *  significati probabilistici più rigorosi come proposta e accettazione nel campionamento 
     *  di Metropolis Hastings attraverso lo spazio di possibili strutture. [12][13][14] 
     *  Al contrario, utilizzando queste mappature, i campionatori Metropolis-Hastings esistenti 
     *  su spazi strutturali possono ora essere visti come forme di sistemi di lavagne anche 
     *  quando non nominati come tali dagli autori. 
     *  Tali campionatori si trovano comunemente negli algoritmi di trascrizione musicale, ad 
     *  esempio. 
     *  I sistemi blackboard sono stati utilizzati anche per costruire sistemi intelligenti 
     *  su larga scala per l'annotazione di contenuti multimediali, automatizzando parti 
     *  della ricerca tradizionale sulle scienze sociali. In questo dominio, il problema 
     *  dell'integrazione di vari algoritmi di intelligenza artificiale in un unico sistema 
     *  intelligente sorge spontaneamente, con le blackboard che forniscono un modo per 
     *  una raccolta di algoritmi di elaborazione del linguaggio naturale distribuiti e 
     *  modulari per annotare i dati in uno spazio centrale, senza dover coordinare il 
     *  loro comportamento.
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
