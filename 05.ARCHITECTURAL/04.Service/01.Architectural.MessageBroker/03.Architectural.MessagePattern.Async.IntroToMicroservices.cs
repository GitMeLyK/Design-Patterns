using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static System.Console;

namespace DotNetDesignPatternDemos.Structural.Async.IntroToMicroservices
{
    /*
     * Guardando l'esempio precedente semplificato per la gestione di un broker
     * di messaggi nel contesto di un sitema architetturale già esistente distribuito
     * sulla rete attraverso programmi come Kafka RabbitMQ e altri abbiamo introdotto
     * quello che viene definito un modello enteprise per la gestione di messaggi in 
     * coda distrubuiti tra componenti software presenti sulla rete a interagire rispondere
     * e inviare messaggi tramite questo adozione.
     * 
     * Il termine nuovo quindi che vediamo in questo esempio e in queste illustrazioni 
     * ci permette sommariamente di capire questo modello ampio di messaggi distribuiti.
     * 
     * Questo assume il nome di Microservizi.
     *
     *  ** Questo esempio porta una panoramica completa di un sistema di Microservizi.
     *  
     *  In questo verrà illustrata la comunicazione asincrona basata su messaggi dei microservizi 
     *  tra microservizi interni back-end. Come sapete, abbiamo imparato pratiche e modelli sui 
     *  modelli di progettazione delle comunicazioni dei microservizi e li abbiamo aggiunti alla 
     *  nostra cassetta degli attrezzi di progettazione. E useremo questi modelli e queste 
     *  procedure durante la progettazione dell'architettura dei microservizi.
     *  
     *  MS1.png
     *  
     *  Alla fine dell'esempio, verrà illustrato come comunicare i microservizi in modo asincrono 
     *  tra i microservizi interni back-end con l'applicazione di modelli e principi di 
     *  progettazione delle comunicazioni di microservizi. 
     *  Applicheremo il principio DIP e Pubblicheremo il modello di progettazione della 
     *  sottoscrizione.
     *  
     *  Impareremo come progettare l'architettura dei microservizi utilizzando modelli di 
     *  progettazione, principi e procedure consigliate. 
     *  Inizieremo con la progettazione di microservizi da monolitici a guidati da eventi 
     *  passo dopo passo e insieme utilizzando i giusti modelli e tecniche di progettazione 
     *  dell'architettura.
     *  
     *  Microservizi Comunicazione asincrona basata su messaggi
     *  
     *  La comunicazione sincrona è buona se la comunicazione avviene solo tra pochi microservizi. 
     *  Ma quando si tratta di diversi microservizi devono chiamarsi a vicenda e attendere alcune 
     *  lunghe operazioni fino al termine, quindi dovremmo usare la comunicazione asincrona. 
     *  In caso contrario, la dipendenza e l'accoppiamento dei microservizi creeranno colli di 
     *  bottiglia e creeranno seri problemi all'architettura.
     *  
     *  MS2.png
     *  
     *  Quindi dovremmo capire che l'isolamento è importante tra i microservizi, dovremmo isolare 
     *  i servizi il più possibile. Poiché i microservizi sono sistemi distribuiti in esecuzione 
     *  su più processi, i servizi devono interagire tra loro utilizzando protocolli di 
     *  comunicazione interprocesso come i protocolli HTTP di sincronizzazione, gRPC o AMQP 
     *  asincroni.
     *  
     *  Se abbiamo qualche interazione con i microservizi di query, dovremmo usare la 
     *  richiesta/risposta HTTP con le API delle risorse. 
     *  Ma quando si tratta di interazioni impegnate nella comunicazione tra più microservizi, 
     *  allora dovremmo usare piattaforme di messaggistica asincrone come i sistemi di broker 
     *  di messaggi che abbiamo visto un tipo nell'esempio 01.Structural.MessagePattern.Broker.
     *  
     *  MS3.png
     *  
     *  Vediamo l'immagine, si può vedere nella comunicazione sincrona sta diventando catena di 
     *  richieste e altamente accoppiata dipendente tra microservizi. 
     *  Quindi questo è il punto dolente dell'architettura e possiamo dire che questo è 
     *  anti-pattern e ha bisogno di riprogettare il sistema.
     *  
     *  Con la comunicazione asincrona, questo problema può essere risolto. I microservizi 
     *  comunicano tra loro tramite il sistema del broker di messaggi in modo asincrono. 
     *  Per lo più è bene usare il modello di pubblicazione/sottoscrizione con i sistemi di 
     *  broker di messaggi che vedremo nelle prossime sezioni.
     *  
     *  Abbiamo detto protocolli asincroni, che è il protocollo AMQP per l'esecuzione di 
     *  trasmissioni di messaggi asincroni. In questo protocollo AMQP, il produttore invia 
     *  un messaggio e non attende una risposta. Invia solo messaggi e si aspetta che consumerà 
     *  i servizi degli abbonati tramite i sistemi di broker di messaggi.
     *  
     *  Comunicazione asincrona basata su messaggi nell'architettura dei microservizi
     *  
     *  Se si dispone di più microservizi necessari per interagire tra loro e se si desidera 
     *  interagire senza alcuna dipendenza o fare accoppiamenti in modo approssimativo, è 
     *  consigliabile utilizzare la comunicazione asincrona basata su messaggi nell'architettura 
     *  dei microservizi. Perché la comunicazione asincrona basata su messaggi fornisce opere con 
     *  eventi. In questo modo gli eventi possono posizionare la comunicazione tra i microservizi. 
     *  Abbiamo chiamato questa comunicazione è una comunicazione guidata dagli eventi.
     *  
     *  Ciò significa che se si verificano modifiche nei domini dei microservizi, si propagano 
     *  le modifiche tra più microservizi come evento successivo a quello di questi eventi 
     *  consumati dai microservizi del sottoscrittore. Questa comunicazione guidata dall'evento (Eevet-Driven)
     *  e la messaggistica asincrona ci portano "eventuale coerenza".
     *  
     *  Quindi, se riassumiamo la comunicazione asincrona, il microservizio client invia un 
     *  messaggio o un evento ai sistemi del broker di messaggi e non è necessario attendere 
     *  la risposta. Perché è consapevole di questo è la comunicazione basata su messaggi e 
     *  non risponderà immediatamente. Un messaggio o un evento può includere alcuni dati. 
     *  E questi messaggi vengono inviati attraverso protocolli asincroni come AMQP sui sistemi 
     *  di broker di messaggi come Kafka e Rabbitmq.
     *  
     *  Tipo di comunicazione di messaggistica asincrona
     *    Esistono due tipi di comunicazione di messaggistica asincrona:
     *          * Single receiver message-based communication one-to-one (queue) model
     *          * Multi receiver message-based communication one-to-many (topic) model or
     *            publish/subscribe model
     *   
     *  MS4.png
     *  
     *  Comunicazione basata su messaggi a ricevitore singolo che possiamo dire modello uno-a-uno, 
     *  Comunicazione basata su messaggi di ricevitori multipli che abbiamo anche detto modello 
     *  di pubblicazione/sottoscrizione.
     *  
     *  Comunicazione basata su messaggi a ricevitore singolo
     *  
     *  Questa comunicazione è fondamentalmente per eseguire comunicazioni one-to-one o 
     *  point-to-point. Se invieremo 1 richiesta al consumatore specifico e questa operazione 
     *  richiederà molto tempo, allora è bene utilizzare questa comunicazione one-to-one 
     *  asincrona a ricevitore singolo.
     *  
     *  MS5.png
     *  
     *  Se guardiamo l'immagine, puoi vedere un caso d'uso di esempio per questa comunicazione 
     *  che è la richiesta CreateOrder inviata dai microservizi del carrello e la creazione 
     *  dell'ordine può richiedere più tempo, quindi non restituirà la risposta immediata ai 
     *  microservizi del carrello.
     *  
     *  Comunicazione basata su messaggi con più ricevitori
     *  
     *  Questa comunicazione è fondamentalmente per eseguire meccanismi di pubblicazione/sottoscrizione 
     *  che hanno più ricevitori. Quindi, in questa comunicazione, il servizio consumer pubblica un 
     *  messaggio e consuma da diversi microservizi che stanno sottoscrivendo questo messaggio sul 
     *  sistema del broker di messaggi. Queste operazioni di pubblicazione/sottoscrizione devono 
     *  richiedere un'interfaccia del bus eventi per pubblicare eventi su qualsiasi sottoscrittore.
     *  
     *  Con questa comunicazione l'editore non ha bisogno di alcun abbonato, il che significa che 
     *  non c'è alcuna dipendenza con le parti della comunicazione.
     *  
     *  Per lo più questa comunicazione asincrona viene utilizzata in architetture basate su eventi.
     *  Nella comunicazione asincrona basata su eventi, il microservizio pubblica un evento quando 
     *  accade qualcosa. Il caso d'uso di esempio può essere simile a una variazione di prezzo 
     *  in un microservizio del prodotto. L'evento Price Changed può essere sottoscritto dal 
     *  microservizio Carrello per aggiornare il prezzo del carrello in modo asincrono.
     *  
     *  MS6.png
     *  
     *  Questi modelli di pubblicazione/sottoscrizione vengono implementati utilizzando il bus eventi.
     *  E il bus degli eventi può anche avere implementazioni con sistemi di broker di messaggistica 
     *  che supportano la comunicazione asincrona e un modello di pubblicazione/sottoscrizione 
     *  come Kafka e Rabbitmq.
     *  
     *  Controlliamo l'immagine sopra qui, Questo è un esempio di messaggistica di 
     *  pubblicazione/sottoscrizione basata sulla comunicazione basata su eventi, i microservizi 
     *  Order Order pubblicano gli eventi OrderSubmitted su un bus di eventi e i microservizi 
     *  Shipment e Inventory possono sottoscrivere questo evento e intraprendere le loro azioni 
     *  interne in base ai dettagli dell'evento.
     *  
     *  Quindi abbiamo visto le comunicazioni asincrone basate su messaggi. 
     *  Ma quando espandiamo queste comunicazioni asincrone con architetture basate su eventi, 
     *  affronteremo il modello CQRS, l'archiviazione degli eventi, gli eventuali principi di 
     *  coerenza presenti in questo progetto di studio.
     *  
     *  Ma prima dobbiamo imparare quali sono i modelli e le pratiche alla base quando 
     *  progettiamo comunicazioni asincrone, in particolare architetture basate su eventi.
     *  
     *  Principi di progettazione — Principi di inversione delle dipendenze (DIP)
     *  
     *  In realtà, questo design principi per lo sviluppo del software e fornisce di rompere 
     *  la dipendenza delle classi invertendo le dipendenze e iniettare classi dipendenti tramite 
     *  il costruttore o la proprietà della classe principale 8Argomento trattato in deviersi punti di questo progetto lab).
     *  
     *  Allora perché dobbiamo imparare questo in questa fase?
     *  
     *  MS7.png
     *  
     *  Che cos'è Dependency Inversion Principles (DIP)
     *  
     *  Possiamo spiegare che una classe dipende da un'altra classe, il che significa che 
     *  una classe ha bisogno di un'altra classe per funzionare. Come vediamo nell'immagine, 
     *  il livello di livello superiore utilizza il livello di livello inferiore 
     *  (classi, interfacce, ecc.).
     *  Possiamo dire che il livello dipende dal livello. Per quanto riguarda il termine 
     *  riusabilità; In primo luogo, pensiamo che una classe possa essere scritta e 
     *  utilizzata in varie parti del progetto, ma possiamo definire un codice che usiamo 
     *  per il riutilizzo e una libreria di livello superiore può essere utilizzata in 
     *  altri progetti senza toccare il codice. Non possiamo farlo a causa dell'interdipendenza 
     *  delle classi in software ben progettati.
     *  A questo punto, entra in gioco il principio di inversione delle dipendenze (DIP), 
     *  che è un importante principio software che deve essere implementato per sviluppare 
     *  moduli più flessibili e riutilizzabili.
     *  Spiegare brevemente questo metodo; I moduli o le classi di livello superiore e le 
     *  classi di livello inferiore non devono dipendere dai moduli. 
     *  I moduli di livello inferiore devono dipendere da moduli di livello superiore 
     *  (interfacce di moduli). In breve, la chiamiamo Inversione delle dipendenze.
     *  Vedi di nuovo l'immagine sopra, Sul lato sinistro abbiamo trovato un'applicazione 
     *  a più livelli in cui la logica di business dipende dall'implementazione di SqlDatabase. 
     *  È un modo accoppiato per scrivere codice.
     *  Sul lato destro, aggiungendo un IRepository e applicando DIP, l'SqlDatabase ha la 
     *  sua dipendenza rivolta verso l'interno. Quindi fondamentalmente ha rotto la dipendenza 
     *  con i livelli e li ha iniettati usando il principio DIP.
     *  
     *  Comunicazioni da servizio a servizio — Query a catena
     *  
     *  MS8.png
     *  
     *  Vedi l'immagine sopra e pensa che abbiamo un caso d'uso che è effettuare l'ordine. 
     *  Se il cliente effettua il checkout del carrello, questo avvierà una serie di operazioni. 
     *  Quindi, se proviamo a eseguire questo caso d'uso dell'ordine di posto con il modello 
     *  di richiesta/risposta chiamando molte chiamate HTTP a più microservizi, il progetto 
     *  non può essere gestito.
     *  Quindi dovremmo rompere le dipendenze con i microservizi per renderli liberamente 
     *  accoppiati e una migliore gestibilità. Quando si tratta di affrontare le dipendenze, 
     *  seguiremo il modello DIP anche qui. Ma questa volta per seguire DIP cambieremo le 
     *  comunicazioni dei microservizi in modo asincrono con i sistemi di broker di messaggi.
     *  Pertanto, se sono necessari più microservizi per interagire tra loro e se si desidera 
     *  interagire tra loro senza alcuna dipendenza o fare accoppiamenti in modo approssimativo, 
     *  è consigliabile utilizzare la comunicazione asincrona basata su eventi con il modello 
     *  di pubblicazione/sottoscrizione nell'architettura dei microservizi.
     *  
     *  Modello di progettazione Pubblica/Sottoscrizione
     *  
     *  In realtà abbiamo già visto come il modello Publish-subscribe funziona con la comunicazione
     *  asincrona, ma lasciatemi spiegare anche qui, Publish-subscribe è un modello di messaggistica,
     *  che ha il mittente di messaggi che sono chiamati editori e ha destinatari specifici che sono
     *  chiamati abbonati.
     *  
     *  MS9.png
     *  
     *  Quindi gli editori non inviano i messaggi direttamente agli abbonati. 
     *  Classifica invece i messaggi pubblicati e inviali in sistemi di broker di messaggi senza 
     *  sapere quali abbonati ci sono. Allo stesso modo, gli abbonati esprimono interesse e 
     *  ricevono solo messaggi di interesse, senza sapere quali editori inviano loro.
     *  In questo modo, gli editori e gli abbonati comunicano tra loro senza accoppiamento o 
     *  alcuna dipendenza l'uno dall'altro. Disaccoppia le comunicazioni dei microservizi, 
     *  in modo che i microservizi possano essere gestiti in modo indipendente e scalati 
     *  in modo indipendente senza preoccuparsi delle comunicazioni. 
     *  Quindi aumenta la scalabilità e migliora la reattività del sistema.
     *  
     *  Questo esempio e quello precedente sono introduzioni per l'aspetto tramite MessagePattern
     *  di individuare come a livello enterprise nella progettazione dei modelli in questo caso per l'uso
     *  di una struttura pensata a microservizi facciano parte anche questi pattern per usarli nei contesti
     *  di sviluppo.
  
     */


    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
