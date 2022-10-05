using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static System.Console;

namespace DotNetDesignPatternDemos.Structural.Sync.IntroToMicroservices
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
     *  In questo verrà illustrata la comunicazione sincrona basata su messaggi dei microservizi 
     *  tra microservizi interni back-end. 
     *  
     *  MS10.png
     *  
     *  Microservizi Comunicazione sincrona tra microservizi interni back-end
     *  
     *  La comunicazione sincrona è buona se la comunicazione avviene solo tra pochi microservizi. 
     *  Ma quando si tratta di diversi microservizi devono chiamarsi a vicenda e attendere alcune 
     *  lunghe operazioni fino al termine, quindi dovremmo usare la comunicazione asincrona. 
     *  In caso contrario, la dipendenza e l'accoppiamento dei microservizi creeranno colli di 
     *  bottiglia e creeranno seri problemi all'architettura.
     *  
     *  MS2.png
     *  
     *  quindi una cosa è usare una comunicazione asincrona descritta nell'esempio successivo
     *  e una cosa è fare un forte accoppiamento tra servizi solo se necessario. In questo 
     *  esempio illustriamo questa tecnica molte volte necessaria per eseigenze particolari
     *  o richieste dal cliente per strategie di sviluppo, ma da tenere sempre con le pinze
     *  e laddove è necessaria.
     *  
     *  Abbiamo creato GATEWAY API nella nostra architettura di microservizi. E ha detto che 
     *  tutte queste richieste di sincronizzazione provengono dai client e vanno ai microservizi 
     *  interni tramite i gateway API.
     *  
     *  MS11.png
     *  
     *  Ma cosa succede se le richieste del client sono necessarie per visitare più di un 
     *  microservizio interno? Come possiamo gestire le comunicazioni interne ai microservizi?
     *  Quando si progettano applicazioni di microservizi, è necessario prestare attenzione 
     *  al modo in cui i microservizi interni back-end comunicano tra loro. 
     *  La migliore pratica consiste nel ridurre il più possibile la comunicazione tra servizi. 
     *  Tuttavia, in alcuni casi, non possiamo ridurre queste comunicazioni interne a causa 
     *  delle esigenze del cliente o della necessità operativa richiesta di visitare diversi 
     *  servizi interni.
     *  Per lo più questo accade quando il client invia una richiesta di query ai microservizi 
     *  interni, al fine di accumulare alcuni dati nella pagina dello schermo dell'applicazione 
     *  frontend.
     *  
     *  MS12.png
     *  
     *  Ad esempio, guarda l'immagine sopra e pensa al caso d'uso come;
     *    L'utente vuole vedere il carrello con i dettagli del prodotto e le informazioni 
     *    sui prezzi.
     *    
     * Quindi, come possiamo attuare questa richiesta? 
     *   Esistono molti approcci per implementare questo tipo di operazioni di query.
     *   Ma per ora ci stiamo concentrando su; sincronizzare la comunicazione tramite i 
     *   gateway API, che è il messaggio di richiesta/risposta.
     *   
     *   Quindi, per il nostro caso, fondamentalmente il client invia una richiesta 
     *   http ai microservizi per interrogare le informazioni del carrello.
     *   
     *   Se seguiamo la richiesta,
     *      1- Il client invia la richiesta ad API Gateway (Pattern per microservizi)
     *      2- La richiesta di invio di API Gateway a SC
     *      3- SC deve estendere le informazioni inviando la richiesta di 
     *         sincronizzazione ai microservizi Prodotto e Prezzi per ottenere i 
     *         dettagli del prodotto e del prezzo.
     *         
     *  Quindi queste chiamate interne rendono l'accoppiamento di ciascun microservizi, 
     *  nel nostro caso Carrello della spesa - I microservizi Prodotto e Prezzi sono 
     *  dipendenti e si accoppiano tra loro. E se uno dei microservizi è inattivo, non 
     *  può restituire i dati al client, quindi non è alcuna tolleranza di errore. 
     *  Se la dipendenza e l'accoppiamento dei microservizi aumentano, si creano molti 
     *  problemi e si perde la potenza dell'architettura dei microservizi.
     *         
     *  In questo caso ci sono solo 2 chiamate interne quindi può essere gestibile e 
     *  potrebbe essere accettabile per alcuni sistemi. 
     *  Tuttavia, se le chiamate di servizio richieste sono molte, alcune chiamate HTTP a 
     *  più microservizi, il progetto non può essere gestito.
     *  
     *  MS13.png
     *  
     *  E pensa che abbiamo un caso d'uso che è l'ordine. 
     *  Se il cliente effettua il checkout del carrello, questo avvierà una serie di operazioni. 
     *  Quindi, se proviamo a eseguire questo caso d'uso dell'ordine di posto con il modello di 
     *  messaggistica di sincronizzazione richiesta/risposta, allora sembrerà come questa immagine.
     *  
     *  Come puoi vedere che c'è 6 richiesta http di sincronizzazione per una richiesta http 
     *  client. Quindi è ovvio che aumentare la latenza e influire negativamente sulle prestazioni, 
     *  la scalabilità e la disponibilità del nostro sistema.
     *  
     *  Se abbiamo questo caso d'uso, cosa succede se il passaggio 5 o 6 non è riuscito, o cosa 
     *  succede se alcuni servizi intermedi sono inattivi? Anche se non c'è down, potrebbe essere 
     *  occupato da alcuni servizi che non possono rispondere in modo tempestivo e che hanno 
     *  causato latenze elevate non accettabili.
     *  
     *  Quale potrebbe essere la soluzione a questo tipo di requisiti?
     *  
     *  Possiamo applicare 2 modi per risolvere questi problemi,
     *      
     *      1- Cambia le comunicazioni dei microservizi in modo asincrono con i sistemi di 
     *      broker di messaggi.
     *      2- utilizzo di Service Aggregator Pattern per aggregare alcune operazioni di 
     *      query in 1 API Gateway.
     *      
     *  Quindi dovremmo evolvere la nostra architettura con l'applicazione della comunicazione 
     *  asincrona o del modello di aggregatore di servizi nei modelli di microservizi al fine di 
     *  adattarsi agli adattamenti aziendali più rapidamente time-to-market e gestire richieste 
     *  più grandi.
     *  
     *  Questo primo esempio e quello successivo sono introduzioni per l'aspetto tramite MessagePattern
     *  di individuare come a livello enterprise nella progettazione dei modelli in questo caso per l'uso
     *  di una struttura pensata a microservizi facciano parte anche questi pattern per usarli nei contesti
     *  di sviluppo.
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
