using System;

namespace DotNetDesignPatternDemos.Architectural.Broker
{
    /*
     * Il modello di broker può sembrare molto simile ad altri che abbiamo esaminato. 
     * Tuttavia, si tratta più di sistemi distribuiti che di bilanciamento del traffico. 
     * I modelli pipe-filter e master-slave forniscono un'elaborazione a percorso singolo. 
     * Questo modello invia le richieste lungo percorsi diversi in base alla richiesta stessa. 
     * 
     * Il modello di broker definito
     * Un modello di broker è una sorta di struttura ad albero. 
     * Iniziamo con un cliente che fa una richiesta al broker. 
     * Il broker valuta tale richiesta e quindi la invia a un server in grado di gestire 
     * la richiesta. 
     * Si noti che il server selezionato non è un candidato ai fini del caricamento 
     * tanto quanto i suoi servizi. 
     * Il broker interroga i server (o il report del server quando si registrano) per 
     * i servizi forniti. Questo modello ci consente di avere un sistema flessibile per 
     * le richieste.
     * 
     * Espansione graduale dei servizi
     * Il punto di forza di un broker è che può accettare qualsiasi richiesta. 
     * Tuttavia, ciò non significa che elaborerà ogni richiesta. Se non esiste un server 
     * in grado di gestire la richiesta, viene inviata una risposta di errore o di servizio 
     * non disponibile. 
     * Ciò consente ai server di andare giù e al broker di accettare ancora le richieste 
     * in qualche modo, oppure possiamo aggiungere servizi per espandere ciò che 
     * il broker può fare.
     * Possiamo anche creare una versione dei nostri servizi per espanderci individualmente 
     * pur continuando a supportare le richieste meno recenti. 
     * Ad esempio, la richiesta di ottenere un record cliente può essere inviata a server 
     * diversi a seconda che si tratti di un record cliente "1.0" o di un record "2.0". 
     * I client saranno sempre in grado di effettuare richieste e potrebbero persino riprovarli 
     * ad attendere che un server diventi disponibile.
     *
     * Sfide
     * Il modello del broker è un po 'come uno yes-man. Prenderà qualsiasi richiesta e 
     * la trasmetterà o dirà che il servizio non è definito. 
     * Non ti dirà molto di più sulla richiesta. Una richiesta non valida potrebbe non 
     * essere mai supportata mentre non è contrassegnata come tale. 
     * Questo modello può creare un enorme arretrato di miglioramenti perché spesso 
     * possono essere visti come facilmente affrontabili. 
     * È necessario regnare nell'ambito delle nuove richieste o almeno pianificare la 
     * tabella di marcia per esse. In caso contrario, il broker potrebbe essere inondato 
     * da richieste di servizi inesistenti che influiscono sulle prestazioni di quelli 
     * che dovrebbe servire.
     */

    /*
     * Il modello broker è un modello architetturale che può essere utilizzato 
     * per strutturare sistemi software distribuiti con componenti disaccoppiati 
     * che interagiscono tramite chiamate di procedura remote. Un componente broker 
     * è responsabile del coordinamento della comunicazione, come l'inoltro delle richieste, 
     * nonché della trasmissione di risultati ed eccezioni.
     * 
     * Nei pattern Creazionali vediamo ad esempio come un MessageBroker è usato per
     * il modello di progettazione basato su Singleton dove abbiamo riportato un esempio
     * di Sungleton<EventBroker>
     * 
     * Nei pattern Strucural abbiamo anche un modello di cui abbiamo parlato che implementa
     * l'interffaccia IMessageBroker con una classe di esempio MessageBroker molto importante
     * per l'aspetto che tratta lo sviluppo dei moderni sistemi enterprise per lo sviluppo
     * di applicazioni distribuite chiamate microservizi.
     * 
     * Nei pattern Behavioral abbiamo ancora un modello di progettazione che è ilChainOfResponsability 
     * che implementa anche questo modello di progettazione per la gestione distribuita tramite Broker.
     * 
     * Contesto
     * 
     *      E' un sistema costituito da più oggetti remoti che interagiscono in modo 
     *      sincrono o asincrono.
     *      
     *      Ambiente eterogeneo
     * 
     * Motivazione
     * 
     *      Di solito, è necessario avere una grande flessibilità, manutenibilità 
     *      e mutevolezza durante lo sviluppo di applicazioni.
     *      La scalabilità è ridotta.
     *      Complessità di rete intrinseche come problemi di sicurezza, guasti parziali, ecc.
     *      Diversità di rete in protocolli, sistemi operativi, hardware.
     *      
     * Soluzione
     * 
     *      Separare la funzionalità di comunicazione del sistema dalla funzionalità principale 
     *      dell'applicazione fornendo un broker che isola i problemi relativi alla comunicazione.
     *      
     *  Gli elementi essenziali sono:
     *  
     *      client (PROXY)
     *      broker
     *      oggetto remoto
     *      
     *  Come in un normale sistema client server il proxy interroga il broker per ricevere 
     *  un dato servizio. Il broker però, non fornisce direttamente il servizio ma mette in 
     *  comunicazione il client con l'oggetto remoto, chiamandone i metodi.
     *  
     *  Questa tipologia di pattern architetturale permette di sviluppare separatamente i 
     *  vari componenti spesso riuscendo a rendere i vari oggetti remoti utilizzabili anche 
     *  da altri sistemi e dunque permette l'aggiornamento dei vari oggetti senza dover 
     *  interrompere il funzionamento dell'intero sistema dato che i proxy possono comunque 
     *  continuare ad accedere agli altri oggetti. Inoltre, talora ne nascesse il bisogno, 
     *  si possono scrivere client per nuove piattaforme continuando ad accedere agli 
     *  stessi broker e oggetti remoti. Tale progettazione rende possibile anche il 
     *  riutilizzo di componenti progettati in passato o per altri sistemi.
     *  
     *  Infrastrtuttura per la Comunicazione
     *  
     *  Lo sviluppo di sistemi software distribuiti è sostenuto da opportuni strumenti 
     *  di middleware:
     *  
     *      - i componenti di un sistema software distribuito possono essere focalizzati 
     *      sulla logica di business dell’applicazione 
     *      
     *      - il collegamento tra questi componenti avviene mediante connettori – 
     *      che comunicano sulla base di stili di comunicazione distribuita implementata 
     *      dal middleware
     *      
     *      -il middleware è un’infrastruttura di comunicazione che protegge i componenti 
     *      delle applicazioni dalle numerose complessità tipiche dei sistemi distribuiti
     *  
     *  In allegato è presente uno studio completo sul Broker e il contesto completo con
     *  gli altri pattern che abbiamo citato cioè il Message e il Publisher-Subscriber 
     *  per realizzare al meglio una soluzione infrastrutturale completa con queste 
     *  metodologie.
     *  
     *  L'insieme di queste metodologie per lo sviluppo di software distribuiti viene usato
     *  nel contesto di questi pattern con sigla POSA.
     *  
     *      File BrokerPOSA.pdf
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
