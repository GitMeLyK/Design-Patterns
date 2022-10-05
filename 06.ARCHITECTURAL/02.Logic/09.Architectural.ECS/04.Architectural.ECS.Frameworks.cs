using System;
using System.Collections.Generic;

namespace DotNetDesignPatternDemos.Architectural.EntityComponentSystem.Frameworks
{

    /*
     *  Per una dprogrammazione di giochi si porta un modello per implementare un 
     *  Entity-Component-System (ECS). 
     *  
     *  Un modello ampiamente utilizzato nella progettazione di giochi, un ECS consente 
     *  un modello di gameoggetto più semplice del polimorfismo. 
     *
     *  Diversi sono le implementazioni che usano questo framewrok di fatto basati 
     *  su questo modello di progettazione.
     *  
     *  Ne citiamo alcuni .:
     *  
     *      Svelto.ECS
     *      morpeh
     *      ecslite
     *      KECS
     *      ecs-saber
     *      
     *  In questo documento ci focalizziamo sul primo che a dispetto degli altri sono
     *  di riferimento prettamente all'aspetto integrato con Unity per i video game
     *  Svelto ECS è possibile utilizzarlo anche in autonomia senza Unity e ssfrutta
     *  appunto questa metdologia di progettazione ECS.
     *  
     *  Svelto.ECS è facile da iniziare, ma pieno di trucchi per utenti esperti. 
     *  Il problema più difficile da superare è di solito quello di spostare la 
     *  mentalità dalla programmazione OOP alla programmazione ECS più che 
     *  utilizzare il framework stesso.
     *  
     *  Guardando al Code 1  
     *  
     *  In uno scenario Multi-Paradigm, un'applicazione ECS-centrica può utilizzare oggetti. 
     *  L'utente può decidere, ad esempio, di inserire dipendenze all'interno di motori da 
     *  utilizzare al di fuori dello scenario ECS. 
     *  
     *  L'utente deve essere consapevole però che abusare di questa flessibilità potrebbe 
     *  portare a percorsi sbagliati, quindi al fine di evitare una situazione pericolosa 
     *  in cui viene utilizzato troppo codice OOP nei sistemi del progetto, è auspicabile 
     *  l'approccio di astrazione OOP.
     *  
     *  ---
     *  Pertanto, si presentano in questo link due modi in Svelto.ECS per astrarre oggetti. 
     *  
     *  La strategia del livello di astrazione OOP è importante perché può essere applicata 
     *  con qualsiasi framework ECS indipendentemente dai dettagli di implementazione.
     *  
     *  L'approccio esclusivo del modello EntityViewComponent di Svelto.ECS può essere più 
     *  conveniente in casi specifici, ma in altri potrebbe invece portare a un degrado delle 
     *  prestazioni e al codice boilerplate.
     *  
     *      // https://www.sebaslab.com/oop-abstraction-layer-in-a-ecs-centric-application/
     *  
     * --
     * 
     * -- Perché usare Svelto.ECS con Unity?
     *  
     *  Svelto.ECS non è nato solo dalle esigenze di un grande team, ma anche come risultato 
     *  di anni di ragionamento dietro l'ingegneria del software applicata allo sviluppo di giochi.
     *  
     *  Svelto.ECS non è stato scritto solo per sviluppare codice più veloce, è stato progettato 
     *  per aiutare a sviluppare codice migliore. 
     *  
     *  I miglioramenti delle prestazioni sono solo uno dei vantaggi nell'utilizzo di Svelto.ECS, 
     *  poiché ECS è un ottimo modo per scrivere codice compatibile con la cache. 
     *  
     *  Svelto.ECS è stato sviluppato con l'idea che ECS sia un paradigma e non solo un modello, 
     *  consentendo all'utente di allontanarsi completamente dalla programmazione orientata agli 
     *  oggetti con conseguenti miglioramenti della progettazione del codice e della manutenibilità 
     *  del codice. 
     *  
     *  Svelto.ECS è il risultato di anni di iterazione del paradigma ECS applicato allo sviluppo 
     *  di giochi reali con l'intento di essere il più infallibile possibile. 
     *  
     *  Svelto.ECS è stato progettato per essere utilizzato da un team di medie/grandi dimensioni 
     *  che lavora su progetti a lungo termine in cui il costo della manutenibilità è rilevante.
     *  
     *  Svelto.ECS è snello, non è stato progettato per spostare un intero motore da OOP a ECS, 
     *  quindi non soffre di un overhead di complessità ingiustificabile per cercare di risolvere 
     *  problemi che spesso non sono legati allo sviluppo del gameplay. 
     *  
     *  Svelto.ECS è fondamentalmente funzionalità complete a questo punto della scrittura e 
     *  le nuove funzionalità nelle nuove versioni sono più belle da avere che fondamentali.
     *  
     * -- Compatibilità con Unity
     *  
     *  Svelto.ECS è parzialmente compatibile con il ciclo Unity 2019.3.x purché non venga 
     *  utilizzato con alcun pacchetto DOTS (incluse le raccolte). 
     *  È compatibile con tutte le versioni di Unity dal 2020 in poi.
     *  
     *  Svelto.ECS è completamente compatibile con DOTS Burst e Jobs.
     *  
     *  Svelto.ECS è progettato per sfruttare appieno i moduli DOTS e per utilizzare specificamente 
     *  DOTS ECS come libreria di motori, attraverso il wrapper (opzionale) Svelto.OnDOTS.
     *  
     * -- Perché usare Svelto.ECS senza Unity?
     * 
     * La domanda è solo per divertimento! Ci sono così tanti motori di gioco c# là fuori 
     * (Stride, Flax, Monogame, FlatRedBall, Evergine, UnrealCLR, UniEngine solo per citarne alcuni) 
     * e Svelto.ECS è compatibile con tutti loro!
     * 
     * -- Considerazioni sulle prestazioni
     * 
     * Oltre a ridimensionare il database in modo assoluto quando necessario, tutte le operazioni 
     * di Svelto sono prive di allocazione della memoria. 
     * 
     * Alcuni contenitori potrebbero dover essere preallocati (e quindi smaltiti), ma si tratta 
     * già di scenari avanzati. 
     * 
     * Quando si utilizza ECS puro (senza EntityViewComponents) i componenti vengono archiviati 
     * in raccolte native su tutte le piattaforme, il che significa ottenere alcune prestazioni 
     * dalla perdita dei controlli della memoria gestita. 
     * 
     * Con ECS puro, l'iterazione dei componenti è automaticamente compatibile con la cache.
     * 
     * Nota: Svelto.ECS ha un sacco di allocazione dei controlli di runtime nel debug, quindi 
     *       se si desidera profilare è necessario profilare una versione di rilascio o utilizzare 
     *       e definire il PROFILE_SVELTO.
     *       
     * -- Se si decide di utilizzare Svelto.ECS
     * 
     * Svelto.ECS è un progetto Open Source fornito così com'è, nessun supporto è garantito se 
     * non l'aiuto dato sul canale Svelto Discord. I problemi verranno risolti quando possibile. 
     * 
     * Se decidi di adottare Svelto.ECS, si presume che tu sia disposto a partecipare allo 
     * sviluppo del prodotto se necessario.
     * 
     *      // Git :  https://github.com/sebas77/Svelto.ECS
     *      
     */


    // Code 1

    // Classe di contesto
    public class SimpleContext
    {
        //the group where the entity will be built in
        public static ExclusiveGroup group0 = new ExclusiveGroup();

        public SimpleContext()
        {
            var simpleSubmissionEntityViewScheduler = new SimpleEntitiesSubmissionScheduler();
            //Build Svelto Entities and Engines container, called EnginesRoot
            _enginesRoot = new EnginesRoot(simpleSubmissionEntityViewScheduler);

            var entityFactory = _enginesRoot.GenerateEntityFactory();

            //Add an Engine to the enginesRoot to manage the SimpleEntities
            var behaviourForEntityClassEngine = new BehaviourForEntityClassEngine();
            _enginesRoot.AddEngine(behaviourForEntityClassEngine);

            //build a new Entity with ID 0 in group0
            entityFactory.BuildEntity<SimpleEntityDescriptor>(new EGID(0, ExclusiveGroups.group0));

            //submit the previously built entities to the Svelto database
            simpleSubmissionEntityViewScheduler.SubmitEntities();

            //as Svelto doesn't provide an engine ticking system, it's the user's responsibility to
            //update engines 
            behaviourForEntityClassEngine.Update();
        }

        readonly EnginesRoot _enginesRoot;
    }

    // descrittore di entità
    public struct EntityComponent : IEntityComponent
    {
        public int counter;
    }

    // motore che esegue i comportamenti delle entità
    public class BehaviourForEntityClassEngine : IQueryingEntitiesEngine
    {
        public EntitiesDB entitiesDB { get; set; }

        public void Ready() { }

        public void Update()
        {
            var (components, count) = entitiesDB.QueryEntities<EntityComponent>(ExclusiveGroups.group0);

            for (var i = 0; i < count; i++)
                components[i].counter++;
        }
    }

    class SimpleEntityDescriptor : GenericEntityDescriptor<EntityComponent>
    { }

    public class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}