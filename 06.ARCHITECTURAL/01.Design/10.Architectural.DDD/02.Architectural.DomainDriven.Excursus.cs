using System;

namespace DotNetDesignPatternDemos.Architectural.DomainDriven.Excursus
{
    /*
     * il DDD è un lavoro di analisi che DEVE, quindi, coinvolgere un gruppo altamente eterogeneo 
     * di collaboratori: tecnici, analisti, potenziali utenti finali, persone del business,.. 
     * così che possano provenire contributi da tutti i livelli di competenza e l’analisi non 
     * sia così contaminata da una visione troppo specifica (troppo business o troppo tecnica)
     * 
     * Strategic Design e Tactical Design
     * 
     *  Il domain-driven design si implementa applicando i concetti definiti in due set di tools: 
     *  strategic design tools e tactical design tools. Strategic design e tactical design 
     *  si applicano in sequenza l’uno all’altro (mantenendo così l’approccio top-down)
     *  
     *  Lo strategic design si concentra nella rappresentazione generale del dominio in senso 
     *  stretto. Il tactical design si occupa invece di definire il cosiddetto domain-model 
     *  (model-driven design) che, contrariamente a quanto può dire il nome, non è la 
     *  rappresentazione del dominio (in quanto coperta in fase di strategic design) ma è la 
     *  rappresentazione strutturale di ogni specifico contesto individuato all'interno del dominio.
     *  
     *  Da qui in poi si utilizzerà il più classico degli esempi per far capire meglio i concetti: 
     *  la realizzazione di un software per l’e-commerce, l’esempio più utilizzato (e, forse, 
     *  più calzante) per questo genere di soluzione.
     *  
     *  I principali e più importanti strategic design tools, su cui è importante concentrare la 
     *  maggior parte dei propri sforzi, sono:
     *  
     *      Subdomains: è la suddivisione del proprio dominio in ambiti di competenza, 
     *                  spesso rappresentati dalle aree di business della propria organizzazione. 
     *                  Non è infrequente che l’approccio DDD sia applicato in grosse realtà dove 
     *                  le units aziendali sono ben definite. Esempio: SalesOrder; Supply Chain 
     *                  (gestione merci); Finance (Fatturazione, pagamenti,…); Customers 
     *                  (Anagrafica dei clienti);….
     *                  
     *     Bounded contexts: contesti di business ben delineati all’interno dei subdomain, al cui 
     *                  interno si definiscono delle ben precise operazioni e si tratta in maniera 
     *                  esclusiva una ben definita tipologia di dati. Sono contesti che possono 
     *                  quindi operare in sostanzialmente autonomia. Di solito il subdomain è 
     *                  quello che si definisce nel problem space, ovvero "quello che si desidera". 
     *                  Il bounded context è la risposta al subdomain, quindi il solution space, 
     *                  ovvero "come si realizza il desiderato". Esempio: dei bounded context possono 
     *                  essere SalesOrder, ItemCatalog,… per il subdomain SalesOrder; GoodsReceipt 
     *                  (ricezione merci), Delivery (Spedizione merci),… per il subdomain SupplyChain; 
     *                  Invoice, Payments,… per il subdomain Finance;…
     *      
     *      Ubiquitous Language: è la definizione di una nomenclatura parlante e che assume un significato 
     *                  specifico all’interno del bounded contexts, senza fraintendimenti o incomprensioni.
     *                  
     *      Context map: è la mappa che definisce le relazioni che intercorrono tra diversi bounded 
     *                  context: alla fine l’insieme dei bounded contexts correlati tra loro compongono un 
     *                  unica soluzione software finale che è quella sotto analisi. Esempio: la relazione 
     *                  tra gli ordini (SalesOrder) e il magazzino (Delivery), o tra il magazzino 
     *                  (GoodsReceipt) con le merci disponibili e il catalogo prodotti (ItemCatalog),…
     *                              
     *      //  Figura 1 : Fig1.png
     *      
     *  I principali e più importanti tactical design tools, su cui è importante concentrare la maggior 
     *  parte dei propri sforzi (soprattutto se l'obiettivo è la realizzazione di un'architettura a 
     *  microservizi) sono:
     *  
     *      Entities/Aggregates: gli oggetti di rilevanza per il business. Rappresentano 
     *                  l’informazione che porta il valore aggiunto all’interno del proprio bounded 
     *                  context. Esempio: carrello e-commerce, ordine di vendita, ordine di acquisto 
     *                  merci, fattura, nota di credito, il profilo del cliente,…
     *                  
     *      Value Objects: sono oggetti che esprimono e descrivono il bounded contexts e le 
     *                  entities/aggregates. Esempio: i prezzi di vendita, gli indirizzi,…
     *                  
     *      Services: sono le funzionalità che operano attorno alle entities/aggregates, ovvero 
     *                  che eseguono le operazioni previste dal software per modellare questi 
     *                  oggetti secondo le esigenze di business. Esempio: inserimento articolo a 
     *                  carrello, checkout dell’ordine, invio pagamento, generazione fattura, 
     *                  dettaglio dell’ordine, storico degli ordini di un cliente, aggiornamento 
     *                  ordine, tracking spedizione,….
     *                  
     *      //  Figura 2 : Fig2.png
     *
     * Entities vs Value Objects
     * 
     * Sostanzialmente la Entity è un oggetto che assume una propria specifica identità e che quindi,
     * al variare dei propri attributi, non cambia di significato ma solo di stato (ha un proprio 
     * ciclo di vita e, quindi, è mutabile). 
     * 
     * E’ rappresentato da un identificativo univoco e, cambiando di stato ma mantenendo la propria 
     * identità, possiede un proprio storico. Esempio: l'oggetto Cliente può essere una entity. 
     * Viene identificato da un preciso ID, posso cambiare liberamente uno dei suoi attributi (anche 
     * il nome o il cognome) senza che l’oggetto perda la sua identità, può cambiare di stato (cliente 
     * attivo, cliente disattivo,…) e, di conseguenza, possiede un proprio storico.
     * 
     * Un Value Objects invece è identificato dall’insieme dei propri attributi, non può avere 
     * un’identità e non può cambiare di stato (è quindi immutabile), in quanto identificato dai 
     * propri attributi. Esempio: l'oggetto Indirizzo può essere un Value Object, in quanto cambiando 
     * il nome della via, quell’elemento assume tutta un’altra identità. 
     * Essendo immutabile, non ha quindi un proprio storico o un proprio ciclo di vita.
     * 
     * Attenzione: proprio in virtù del concetto di ubiquitous language e bounded contexts, 
     * quello che con lo stesso nome è una entity in un contesto, può essere un value object 
     * in un altro contesto!
     * 
     * Entities vs Aggregates
     * 
     * Questa è una differenza abbastanza sottile nel mondo della programmazione a servizi, mentre 
     * è più evidente se si applica la programmazione ad oggetti a seguito del DDD. 
     * Un aggregato è un insieme di entità e si definisce quando le entità che lo compongono hanno 
     * senso di esistere solo venendo gestite atomicamente in un concetto di business completo, 
     * che è l'aggregato: in sostanza si necessita di questa tipologia di oggetti quando ci si 
     * preoccupa della consistenza tra le entità collegate tra loro. Esempio: l’oggetto ordine in 
     * un software e-commerce è un aggregato che prevede, tra le altre cose, gli articoli dell’ordine, 
     * che sono delle entities. Un ordine senza articoli non ha senso di esistere e le righe di 
     * articolo senza la testata dell’ordine sono praticamente prive di significato. 
     * Se si vuole modificare un articolo di un ordine, si deve agire sull’aggregato ordine 
     * che garantisce la consistenza sulle righe dell’ordine: per quello un aggregato è identificato 
     * da quella che si dice entity root.
     * 
     * Event Storming
     * 
     * I processi di strategic design e tactical design, ad oggi, vengono operati avvalendosi della 
     * metodologia event storming, una sorta di brain storming, avente però delle regole ben 
     * precise e degli strumenti specifici, che si focalizza sull'analisi di tutti (e ripeto tutti) 
     * i processi previsti nel proprio sistema rappresentati in una sequenza temporale (la 
     * cosiddetta timeline)
     * 
     * Si comincia prevalentemente individuando tutti gli eventi che accadono nel proprio dominio 
     * (la cosiddetta fase di chaotic exploration), a cui segue l’individuazione delle azioni che 
     * devono essere svolte a seguito di quegli eventi e, in ultimo, dei dati impattati da queste 
     * azioni.
     * 
     * Il risultato dell’event storming è poi una big picture sempre ad alto livello del proprio 
     * sistema, grazie alla quale è possibile poi direttamente applicare i tactical design tools, 
     * ovvero:
     * 
     *  si individuano i cosiddetti pivotal events che rappresentano il confine dei propri 
     *  subdomains: sono eventi chiave del dominio che determinano il coinvolgimento di 
     *  operazioni e informazioni di un diverso bounded context
     *  
     *  Per ogni subdomains si individuano poi i bounded contexts: un subdomains può essere 
     *  supportato da uno o più bounded contexts (può dipendere dalla complessità del subdomain)
     *  
     *  in pieno approccio top-down si identificano a questo punto i dati e gli oggetti soggetti 
     *  alle trasformazioni comandate dai processi (eventi, comandi/azioni) individuati nei punti 
     *  precedenti: questi diventano i cosiddetti aggregates o entities. Il disegno di ogni bounded 
     *  contexts nelle sue componenti rappresentative definisce il domain-model, ovvero la 
     *  rappresentazione strutturale del bounded context
     *  
     *      // Figura 4: Fig4.png
     *      
     *  La visione un po’ object-oriented della DDD viene a supporto per la separazione degli ambiti 
     *  di competenza delle componenti del proprio dominio: il concetto di single responsibility 
     *  principle è infatti associabile al concetto di closure of operations.
     *  
     *  Come già accennato in precedenza è assolutamente fondamentale e imprescindibile che nella 
     *  definizione delle componenti e dei processi di un bounded context si debba utilizzare un 
     *  linguaggio chiaro e comune, che assuma un significato specifico in base a quel contesto 
     *  dove opera: questo è il concetto di ubiquitous language, che serve per evitare il più 
     *  possibile l’utilizzo di linguaggio altamente tecnico che risulterebbe non comprensibile 
     *  da altri componenti meno tecnici del team di analisi con i quali si sta svolgendo il DDD. 
     *  
     *  Esempio: identificare il cliente come PurchaserPersonObj è una terminologia troppo tecnica 
     *  e di difficile comprensione da un business analyst o da uno SME. Customer invece è un termine 
     *  di facile comprensione da tutti i ruoli coinvolti nella DDD prima e nello sviluppo e 
     *  mantenimento del software poi. Tra l'altro il termine Customer può rappresentare due 
     *  oggetti ospitati in due contexts differenti e può rappresentare due concetti completamente 
     *  differenti, essendo rappresentato con chiavi e attributi differenti (Customer nel SalesOrder 
     *  è il compratore e possiede informazioni come indirizzo di spedizione, email di login,... 
     *  nel mondo Finance invece Customer è un soggetto fiscale intestatario di una fattura che 
     *  quindi possiede come attributi l'indirizzo di fatturazione, la partita IVA o il codice 
     *  fiscale e così via)
     *  
     *  Da DDD ai Microservizi (MSA)
     *  
     *  Come si collega un approccio DDD ai microservizi?
     *  
     *  Come detto all’inizio il DDD non è un’architettura. E’ un design del proprio software. 
     *  Un’applicazione, paradossalmente, può anche essere realizzata secondo un’architettura 
     *  monolitica dopo essere stata disegnata secondo il DDD. 
     *  La MSA (MicroService Architecture) è invece un’architettura e nella realtà è solitamente 
     *  la scelta architetturale più sensata per implementare all'atto pratico un software 
     *  analizzato e pensato secondo il DDD.
     *  
     *  Nello specifico l’approccio DDD è sfruttato come metodologia per la scomposizione 
     *  (decomposition) del proprio landscape progettuale. 
     *  Per realizzare una MSA i metodi di decomposition più utilizzati sono infatti:
     *  
     *      - Business Capabilities Decomposition
     *      - Domain Driven Design
     *      
     *  La fase di decomposition, specialmente mediante l’approccio DDD, è applicabile sia in 
     *  fase di definizione di un software ex-novo (quindi una scomposizione concettuale) sia 
     *  per trasformare e migrare un proprio sistema monolitico in un sistema a microservizi.
     *  
     *  Dimensionamento Microservizi
     *  
     *  Una volta terminata la fase di DDD, si entra tipicamente nella fase più architetturale 
     *  e, avendo fatto bene tutta il DDD del proprio software, uno dei passi cruciali rimanenti 
     *  è il dimensionamento dei propri microservizi: quanto devono essere grossi i microservizi? 
     *  Quante APIs e quante funzionalità si devono gestire in una singola componente?
     *  
     *  Non esiste una regola fissa, le risposte a queste domande dipendono da tante variabili, 
     *  ma tenendo in considerazione le indicazioni del DDD e il suo risultato e ricordando la 
     *  definizione generica di microservizio (ovvero un set di funzionalità che agisce su una 
     *  propria esclusiva base dati, rendendolo autoconsistente e indipendente nella propria 
     *  gestione e manutenzione), un microservizio viene di solito dimensionato in base a:
     *  
     *      - il team che ci deve lavorare: per garantire l’autonomia sul mantenimento e la 
     *        gestione della componente stessa
     *        
     *      - il contesto: per garantire che la componente (il microservizio) rispetti i criteri 
     *        di indipendenza dalle altre e autonconsistenza
     *        
     * Per questo un bounded context è poi spesso (ma, non sempre!) rappresentato nell’architettura 
     * con uno specifico microservizio
     * 
     * Quando si dimensiona un microservizio non bisogna mai e poi mai farsi ingannare dalla 
     * parola micro-
     * 
     * Un microservizio non è una funzione (non stiamo parlando di FaaS)! quindi "micro" non vuol 
     * dire: fare la più piccola cosa possibile.
     * 
     * Cosa serve in una MSA
     * 
     * La scelta strategica di definire un’architettura a microservizi per la propria applicazione 
     * di solito è determinata da diverse motiviazioni, le cui principali sono:
     * 
     *      - l'esigenza di limitare le integrazioni point-to-point e di abolire il più possibile 
     *        le repliche di dati
     *        
     *      - la volontà di realizzare un'applicazione cloud-native, che quindi possa operare in 
     *        quelli che si definiscono sistemi distribuiti e decentralizzati.
     *        
     * Per essere cloud-native bisogna pensare ad applicazioni e processi di integrazione api-centric,
     * ovvero che interagiscono esclusivamente mediante APIs
     *
     * Avere un’applicazione a microservizi, e quindi api-centric, prevede la presenza di 
     * componenti software fondamentali nel disegno della propria architettura.
     * 
     * API Manager
     * 
     * Una soluzione di API management è fondamentale per un’architettura a microservizi e 
     * cloud-native orientata alle API per gestire il cosiddetto traffico north-south 
     * (l’utilizzatore finale, esterno al sistema, che deve consumare le API per eseguire 
     * servizi e funzionalità definite all'interno del sistema), in quanto garantisce la 
     * gestione del traffico in ingresso al cluster, la sicurezza dall’esterno verso l’interno 
     * e regolamenta l’utilizzo delle API per le applicazioni e gli utilizzatori finali.
     *  
     * Sicurezza Zero Trust
     * 
     * il zero trust è un modello di sicurezza necessario ove il classico approccio cosiddetto 
     * perimetrale non è applicabile (questo è un tema che verrà affrontato in maniera decisamente 
     * più approfondita in un successivo articolo). Zero Trust vuol dire porsi nella mentalità di 
     * non fidarsi di nessuna persona, sia essa all’interno o all’esterno della rete del proprio 
     * sistema. Un modello Zero Trust verifica sempre "chi è" che vuole eseguire o sta eseguendo 
     * qualcosa prima di stabilire la fiducia e garantisce solo l’accesso minimo indispensabile 
     * per completare una specifica funzione (vedi: autorizzazione al consumo di API). 
     * 
     * Le infrastrutture public cloud, i SaaS, i dispositivi personali e, ovviamente, le architetture 
     * a microservizi hanno modificato ed ampliato il margine di rischio rispetto alle precedenti 
     * applicazioni blindate e blackbox, quindi si è reso necessario offrire tale modello di sicurezza.
     *
     * Service Mesh e Microgateway
     * 
     * In un’architettura a microservizi, dove le componenti (microservizi) sono distribuite, 
     * la stabilità nella comunicazione tra queste componenti loosely-coupled (il cosiddetto 
     * traffico east-west) è fondamentale. Per evitare il più possibile di dover dare in carico 
     * allo sviluppatore di un microservizio la gestione della stabilità e dell’affidabilità 
     * della comunicazione con gli altri microservizi (permettendogli invece di concentrarsi 
     * sulla qualità dello sviluppo della core logic), si agganciano ai microservizi dei 
     * piccoli moduli (da qui il termine "sidecar") che erogano la cosiddetta service mesh: 
     * Istio è la più famosa soluzione per questa esigenza.
     * 
     * Event Mesh (EDA e event/message broker)
     * 
     * L’idea dei microservizi è che siano il più possibile disaccoppiati (loosely-coupled) 
     * tra di loro. A parte dove è assolutamente necessario che un microservizio debba 
     * interrogare in maniera sincrona un altro microservizio (vedi service mesh), si cerca 
     * in tutti gli altri casi di implementare una comunicazione event-driven di natura quindi 
     * asincrona. In questo modo si riesce a essere molto più conformi al disegno domain-driven 
     * che prevede, se vi ricordate, l’individuazione dei famosi eventi di dominio come concetti 
     * chiave.
     * 
     * Ovviamente più si sfruttano gli eventi per cercare di disaccoppiare il più possibile i 
     * microservizi tra loro, più aumentano in volumi sia delle componenti coinvolte (i 
     * microservizi) sia, di conseguenza, degli eventi stessi che transitano nel dominio: è quindi 
     * necessario appoggiarsi a sistemi di event mesh o event broker, supportati da soluzioni 
     * software che supportino tale carico.
     * 
     * Integrazione servizi
     * 
     * per implementare e esporre processi complessi. L’approccio DDD, come ormai chiaro, 
     * prevede la definizione di entità che rappresentano oggetti di rilevanza per il business, 
     * ben delineati e lavorabili mediante i microservizi o, più correttamente, mediante le API 
     * esposte dai microservizi. Dato che ogni microservizio agisce all'interno di un bel 
     * delineato contesto (bounded context) e su uno specifico modello dati (il microservizio 
     * ha una propria esclusiva persistenza), spesso si vuole però offrire all’utente finale 
     * una funzione complessa che agisca su più oggetti dello stesso microservizio (più API 
     * che agiscono su due entity differenti dello stesso database) o su diversi oggetti di 
     * diversi microservizi, togliendo all’utente finale l’onere di dover effettuare di proprio 
     * pugno azioni in sequenza o, peggio, in parallelo.
     * 
     * SAGA
     * 
     * un’architettura a microservizi (o MSA) disegnata seguendo il DDD ha molti vantaggi ma 
     * anche parecchie variabili da gestire. Uno dei principali problemi che si porta dietro 
     * un’architettura distribuita, è la gestione della transazione: se in un monolite le 
     * proprietà ACID erano facilmente gestibili, con i microservizi invece, dove le basi dati 
     * non sono centralizzate e condivisibili da tutte le componenti, è necessario prevedere 
     * dei pattern a supporto della transazione distribuita. Se in passato ci si appoggiava 
     * spesso alla two-phase commit, ad oggi nelle MSA si predilige prevalentemente l’applicazione 
     * dei SAGA patterns che prevedono l’implementazione di cosiddette transazioni compensative per 
     * eseguire le opportune azioni di rollback in caso di fallimento dell’intera transazione 
     * distribuita su più microservizi.
     * 
     * Vi sono due diversi SAGA pattern:
     *      - choreography
     *      - orchestration
     *      
     * E finiamo qui avendo anche introdotto l'utilizzo di questo design architetturale per 
     * comprendere in fin dei conti come è possibile infrastrutturare uno sviluppo orientato
     * al dominio applicativo in unteam, ed avendo introdotto come i microservizi ne fanno 
     * uso di questo modello e su questo modello di design hanno definito tutta la sere di 
     * pattern che girano intorno ai microservizi distribuiti o in cloud.
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
