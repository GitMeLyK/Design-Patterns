using System;

namespace DotNetDesignPatternDemos.Architectural.EntityComponentSystem
{
    /*
     *  L'Entity Component System è un modello architettonico spesso utilizzato nello sviluppo 
     *  di giochi video. 
     *  
     *  Facilita la riusabilità del codice separando i dati dal comportamento. 
     *  
     *  Inoltre, ECS obbedisce al "principio di composizione rispetto all'ereditarietà", 
     *  fornendo una maggiore flessibilità e aiutando gli sviluppatori a identificare 
     *  le entità nella scena di un gioco in cui tutti gli oggetti sono classificati come 
     *  entità.
     *  
     *  Per come è il modello architettonico è stato come abbiamo detto spesso utilizzato
     *  di videogame e soprattutto con Unity il framework offerto da microsoft, ma rimane
     *  un modello di progettazione architettonico che in modo logico suddivide i due
     *  aspetti Entity ComponentSystem ed è quindi un modello applicabile anche in contesti
     *  diversi dai videogiochi come vedremo.
     *  
     *  I framework spesso abilitano Entity Component Systems e il termine "ECS" 
     *  viene spesso utilizzato per illustrare l'implementazione di uno specifico 
     *  modello di progettazione.
     *  
     *  Un ECS è costituito dai seguenti elementi:
     *  
     *      - Ha identificatori univoci noti come entità.
     *      
     *      - Contiene tipi di dati semplici senza comportamento noto come componenti.
     *      
     *      - Ha sistemi, definiti come funzioni che sono abbinate a entità che hanno 
     *        un particolare insieme di componenti.
     *        
     *      - Le entità possono contenere zero o più componenti.
     *      
     *      - Le entità possono modificare dinamicamente i componenti.
     *      
     * Quindi, un ECS è un'architettura che si concentra sui dati e separa dati/componenti, 
     * identità/entità e comportamento/sistemi. 
     * 
     * Queste caratteristiche lo rendono una scelta naturale per il design dei videogiochi.
     * 
     * Che cosa è definito come un'entità?
     * 
     * Un'entità rappresenta una "cosa" in un videogioco, un oggetto distinto che rappresenta 
     * un attore in uno spazio simulato, tipicamente espresso come un valore intero univoco. 
     * 
     * Ad esempio, se stai giocando a Skyrim, tutte le "cose" tangibili e visibili nell'universo 
     * del gioco sono entità. 
     * Non contengono dati o comportamenti effettivi.
     * 
     * Ora, suddividiamo il termine in parti separate e identifichiamole meglio.
     * 
     * Qual è la definizione di componente?
     * 
     * I componenti sono tipi di dati costituiti da un comportamento univoco assegnato 
     * a un'entità. 
     * 
     * Sono moduli riutilizzabili che i programmatori collegano alle entità, fornendo 
     * comportamento, funzionalità e aspetto, formando un'entità.
     * 
     * Ad esempio, un programmatore di giochi di spada e stregoneria potrebbe costruire 
     * un'entità spada magica raccogliendo questi componenti:
     * 
     *  - Un componente materiale, come la "lucentezza", influenza l'aspetto della spada
     *  - Un componente di peso misura "libbre" per determinare il peso complessivo della spada
     *  - Una componente di danno che influenza la praticità di un'arma della spada
     *  
     * Qual è considerato il sistema?
     * 
     * I sistemi iterano i componenti per eseguire funzioni di basso livello come l'esecuzione 
     * di calcoli fisici o il rendering di grafici. 
     * 
     * I sistemi forniscono ambito, servizi e gestione globali per le classi di componenti. 
     * 
     * È fondamentalmente la logica che opera sui componenti.
     * 
     * Ad esempio, un sistema di telecamere gestisce ogni entità con un componente della 
     * telecamera, controllando quale telecamera è attiva.
     * 
     * Cos'è una composizione?
     * Le composizioni consentono agli sviluppatori di collegare più componenti per aggiungere 
     * funzionalità, comportamento o aspetto aggiuntivi.
     * 
     * I vantaggi di ECS
     * 
     * Ecco perché gli Entity Component Systems sono un vantaggio per i programmatori:
     * 
     *  - I programmatori possono utilizzare ECS per creare codice più breve e meno complicato
     *  - Offre un design pulito che impiega metodi di disaccoppiamento, incapsulamento, 
     *    modularizzazione e riutilizzabilità
     *  - Consente ai programmatori di combinare parti riutilizzabili, offrendo una migliore 
     *    flessibilità nella definizione degli oggetti
     *  - Presenta un comportamento emergente molto flessibile
     *  - Offre un'architettura sia per lo sviluppo 3D che VR, consentendo di costruire 
     *    quest'ultima applicazione in termini di complessità
     *  - Consente ai non tecnici di scrivere script in base al comportamento
     *  - È una scelta facile per unit test e simulazioni
     *  - È possibile cambiare componente con componenti simulati in fase di esecuzione
     *  - Ti aiuta ad aggiungere o rafforzare nuove funzionalità
     *  - È un metodo intuitivo per il multi-threading e l'elaborazione parallela
     *  
     *  Aiuta i programmatori a separare i dati dalle funzioni che possono agire su di essi.
     * 
     *  Gli svantaggi dell'utilizzo di ECS
     *  
     *  Naturalmente, ogni strumento ha il suo lato negativo. 
     *  Ecco alcune cose su ECS che non sono così grandi:
     *  
     *      - ECS non è molto conosciuto. La maggior parte delle persone non ne ha nemmeno 
     *        sentito parlare. Ciò può porre problemi per la collaborazione.
     *        
     *      - Non è definito in modo concreto come altri modelli, come Model-View-Controller (MVC).
     *      
     *      - È difficile da applicare correttamente ed è facile da abusare. Di conseguenza, 
     *        i programmatori devono pensare di più a come progettare buoni componenti.
     *        
     *      - ECS richiede ai programmatori di scrivere molti piccoli sistemi che possono 
     *        potenzialmente essere utilizzati in un gran numero di entità. 
     *        Questo metodo comporta il rischio di scrivere codice molto inefficiente.
     *        
     * Esempio di entity component system
     * 
     * Questa illustrazione, fornita da Docs.unity3d, mostra un esempio di architettura ECS e 
     * come le parti funzionano tutte insieme.
     * 
     *      // Figura 1 : Fig1.png
     * 
     * -- Flusso di dati in ECS
     * 
     *      Possiamo suddividere il flusso di dati ECS nei seguenti passaggi:
     *      
     *       - Sistema: il sistema ascolta gli eventi esterni e pubblica gli aggiornamenti ai componenti.
     *       - Componente: i componenti ascoltano gli eventi di sistema, quindi aggiornano il loro stato.
     *       - Entità: l'entità acquisisce il comportamento attraverso le modifiche negli stati dei componenti.
     *       
     *      Quindi, un giocatore preme il tasto "freccia destra" mentre si avventura in un mondo 
     *      fantastico. 
     *      
     *      Il sistema di input del giocatore rileva la pressione dei tasti del giocatore e aggiorna 
     *      il componente di movimento. 
     *      
     *      Il sistema di movimento si attiva e "vede" che il moto dell'entità è a destra, quindi applica 
     *      la forza fisica di conseguenza. 
     *      
     *      Quindi, il sistema di rendering prende il sopravvento e legge la posizione corrente 
     *      dell'entità, disegnandola secondo la sua nuova definizione spaziale.
     * 
     * -- Perché viene utilizzato ECS?
     * 
     *      Abbiamo già accennato ai vantaggi di ECS, ma ci sono quattro motivi per cui agli 
     *      sviluppatori di giochi piace usarlo:
     *          - Innanzitutto, può supportare molti oggetti di gioco
     *          - In secondo luogo, il suo codice è più riutilizzabile
     *          - In terzo luogo, consente uno stile di codifica più dinamico
     *          - In quarto luogo, consente agli sviluppatori di estendere/aggiungere nuove funzionalità
     * 
     * -- In che modo ECS è diverso da OOP?
     * 
     *      Alcune persone considerano ECS un'alternativa alla programmazione orientata agli 
     *      oggetti, altrimenti nota come OOP. 
     *      
     *      Sebbene i due condividano alcune somiglianze sovrapposte, ci sono quattro 
     *      differenze chiave tra loro:
     *      
     *              -- OOP incoraggia l'incapsulamento dei dati mentre ECS promuove gli 
     *                 oggetti POD (Plain Old Data) esposti
     *                 
     *              -- OOP considera l'ereditarietà un cittadino di prima classe, mentre 
     *                 ECS considera la composizione una prima classe
     *                 
     *              -- OOP memorizza i dati in base al comportamento, ma ECS separa i dati 
     *                 dal comportamento
     *                 
     *              -- Le istanze di OOP Object sono statiche singole e le entità possono 
     *                 modificare dinamicamente più componenti
     *                 
     * -- In che modo ECS è diverso dai framework Entity-Component?
     * 
     *      Sebbene condividano due terzi dello stesso nome, ECS e Entity-Component 
     *      Framework non sono la stessa cosa. 
     *      
     *      I framework EC, come quelli che si trovano di solito nei motori di gioco, 
     *      sono come ECS in quanto consentono la creazione di entità e la composizione 
     *      dei componenti.
     *      
     *      Ma in EC, i componenti sono classi che hanno sia dati che comportamento. 
     *      Inoltre, il comportamento viene eseguito direttamente sul componente.
     *      
     *      class IComponent {
     *
     *          pubblic virtual void update() = 0;
     *      };
     *
     *      class Entity {
     *
     *          IComponent vettoriali<IComponent*>;
     *
     *          public void addComponent(IComponent *componente);
     *
     *          public void removeComponent(IComponent *componente);
     *
     *          public void updateComponents();
     *
     *      }; 
     *
     * -- ECS è un livello inferiore di astrazione?
     * 
     *  Non proprio. Sebbene diversi progetti ECS possano sfruttare ottimizzazioni di macchine 
     *  di basso livello, il codice scritto di un ECS non deve essere inferiore o superiore 
     *  rispetto ad altri approcci.
     *  
     * -- ECS richiede la scrittura di più codice?
     * 
     *  La risposta dipende davvero dal framework ECS e dal tipo di motore che stai utilizzando. 
     *  Quando si integra un framework ECS con un motore, è possibile ottenere un codice piuttosto 
     *  compatto e conciso che a volte è più breve delle alternative non ECS.
     *  
     *  Ma quando il tuo ECS non è integrato con un motore, avrai bisogno di un codice di colla 
     *  aggiuntivo per creare un ponte tra i tipi di motore nativi e l'ECS. 
     *  
     *  Questa procedura può causare la scrittura di più codice da parte di un'applicazione.
     *  
     *  In ultima analisi, tuttavia, qualsiasi tempo speso a scrivere codice ECS è in genere 
     *  compensato dal risparmio di tempo che si ottiene dall'avere una base di codice più 
     *  gestibile.
     *  
     * -- ECS è veloce?
     * 
     *  Di norma, sì, anche se dipende da ciò che viene misurato e dall'implementazione ECS 
     *  stessa perché implementazioni diverse comportano compromessi diversi. 
     *  
     *  Quindi, ad esempio, un'operazione lenta in un framework potrebbe essere estremamente 
     *  veloce in un altro.
     *  
     *  In termini di velocità, le implementazioni ECS sono in genere utili per modificare 
     *  dinamicamente i componenti in fase di esecuzione e per interrogare e iterare 
     *  linearmente i set di entità. D'altra parte, le implementazioni ECS non hanno 
     *  velocità nell'esecuzione di query o operazioni che richiedono strutture di dati 
     *  altamente specializzate come strutture spaziali e alberi binari.
     *  
     *  Ma puoi ottenere il massimo dal tuo ECS se familiarizzi con i compromessi 
     *  dell'implementazione e sfrutti il suo design.
     *  
     * -- Il codice ECS è più riutilizzabile?
     *  
     *  Sì, perché i comportamenti di un ECS sono abbinati a un insieme di componenti, 
     *  non strettamente accoppiati con una classe come in OOP. 
     *  
     *  Poiché i comportamenti non sono legati a una classe, possono essere utilizzati in 
     *  diverse classi di entità.
     *  
     *  Inoltre, i programmatori possono introdurre nuovi sistemi in qualsiasi fase dello sviluppo. 
     *  
     *  I sistemi verranno automaticamente abbinati a qualsiasi entità nuova o esistente che 
     *  contenga i componenti giusti.
     *  
     * -- ECS è buono per il multi-threading?
     * 
     *  Di solito sì, perché la separazione di dati e comportamento significa che è più facile 
     *  identificare i singoli sistemi, le loro dipendenze e il modo in cui lo sviluppatore 
     *  dovrebbe pianificarli.
     *  
     * -- ECS può essere utilizzato al di fuori del gioco?
     * 
     *  Sì, può ed è stato utilizzato in progetti non di gioco.
     *  
     * -- Come si crea una gerarchia in ECS?
     * 
     *  Ecco un approccio che puoi usare per creare una gerarchia in un Entity Component System:
     *  
     *  / Store the parent entity on child entities
     *  
     *      struct Parent {
     *
     *          entity parent;
     *
     *      }; 
     *
     *      // Store all children of a parent in a component with a vector
     *
     *      struct Children {
     *
     *          vector<entity> children;
     *
     *      }; 
     *
     *      // Store children in linked list
     *
     *      struct ChildList {
     *
     *          entity first_child; // First child of entity
     *
     *          entity prev_sibling; // Previous sibling
     *
     *          entity next_sibling; // Next sibling
     *
     *      }; 
     *  
     * -- Come si memorizzano i dati spaziali in ECS?
     * 
     *  Le strutture dei dati spaziali utilizzano layout che non corrispondono bene al tipico 
     *  layout ECS. 
     *  
     *  Tuttavia, è possibile creare una query che itera le voci pertinenti, memorizzandole 
     *  in strutture spaziali all'inizio o alla fine di ogni fotogramma.
     *  
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
