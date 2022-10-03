using System;

namespace DotNetDesignPatternDemos.Architectural.ClientServer
{
    /*
     * Questa architettura viene utilizzata quando un server e i client si connettono tramite 
     * Internet. 
     * 
     *  Qui, Server è il fornitore di servizi. 
     *  E il cliente è il consumatore del servizio. 
     *  
     *  Normalmente il server si trova in una rete locale o in Internet. 
     *  Se il server si trova in una rete locale, gli estranei non possono accedere al 
     *  server ma gli addetti ai lavori possono.
     *  
     *  Esistono alcuni metodi per distribuire un server come segue,
     *  
     *      On-premise : server distribuiti all'interno dell'organizzazione.
     *      
     *      Cloud : utilizzato principalmente in questi giorni. Agire come IAAS- 
     *      Infrastructure As A Service. Es: AWS (Amazon web services), Microsoft Azure, 
     *      Google Cloud.
     *      
     *  L'uso di un server è le richieste di invio del client e il server risponde a questo.
     *  
     *  es: l'utente richiede un sito web YouTube (cerca YouTube in un browser web). 
     *  quindi il server invia una risposta "qui il tuo sito web" (reindirizza a YouTube).
     *  
     *  Cos'è un server in breve
     *  
     *      Un server è un'altra macchina con un'enorme quantità di potenza di elaborazione.
     *      
     *      Dopo la distribuzione del server, gli utenti saranno continuamente aumentati, 
     *      ma il problema è che il server ha solo risorse limitate (RAM, CPU,.. ecc.) 
     *      Ciò significa che nella situazione in cui più persone sono connesse 
     *      contemporaneamente a un server, quindi il server potrebbe essere bloccato a 
     *      causa del posizionamento di risorse limitate secondo necessità per pochi utenti. 
     *      Quindi il numero di server dovrebbe essere aumentato. 
     *      Questo si chiama scaling up. Ora è fatto automaticamente perché non sappiamo 
     *      quanti server hanno bisogno in quelle situazioni. Ma ai vecchi tempi veniva 
     *      fatto da un essere umano. E un'altra cosa è che se non c'è bisogno di alcuni 
     *      server, separa automaticamente i server che sono considerati come risorse 
     *      aggiuntive, che si chiama ridimensionamento.
     *
     *      Tale automazione viene eseguita da un altro server. Questo è il server LB . 
     *      ( Server di bilanciamento del carico ) che è anche un altro server. 
     *      Gli utenti si connettono ai server (come web server, mail server, file server .. ) 
     *      attraverso questo server LB. Quel server sa come scalare verso l'alto e 
     *      ridimensionare in ora di punta e non di punta. A causa di avere LB , non 
     *      importa se un server si spegne, si connette automaticamente a un altro server 
     *      funzionante. 
     *      Questo è un vantaggio di avere LB. Pensi se il server LB si interrompe? 
     *      ecco perché avere stand by LB server.
     *      
     *          // Figura 2 ServerBianciamentoCarico.jpg
     *          
     *      Quando un utente accede al server, alloca un processore per un utente, come 
     *      quello aumentando l'accesso dell'utente, il server dovrebbe avere risorse 
     *      come le esigenze dell'utente. Questo è un problema. Come soluzione a questo 
     *      problema, il server utilizzava i thread per la gestione delle funzioni utente. 
     *      ( Sai che i thread e i processori sono molto diversi nelle azioni proprie ). 
     *      I thread possono condividere risorse con altri thread, il che rappresenta un 
     *      vantaggio per questa situazione. E anche se un processore può avere 4 thread e 
     *      un server ha 4 processori, più utenti possono accedere al server contemporaneamente.
     *      
     *      Supponiamo che un server abbia 4 processori, un processore può avere un utente. 
     *      Quindi gli utenti massimi sembrerebbe che possano essere solo quattro.
     *      
     *          // Figura 3 4Processori.jpeg
     *          
     *       Ma ora un processore può avere 4 thread e il server ha 4 processori. 
     *       Ora il numero massimo di utenti è 16 per quel particolare server.
     *       
     *          // Figura 4 16Processi.jpeg
     *          
     *  Ecco come funziona il server.
     *  
     *      Ora supponiamo un'applicazione web. Front-end utilizzando React, Angular, .. 
     *      o qualche framework di sviluppo front-end, come Back-end NodeJs, SpringBoot, 
     *      PHP .... o qualcosa del genere, con database MongoDB, SQL.... o qualcosa del genere.
     *      
     *      Quindi front-end e back-end sono separati (separazione delle preoccupazioni). 
     *      È necessario collegare front-end e back-end in qualche modo per archiviare i 
     *      dati nel DB. Qui abbiamo bisogno di un'API REST per inviare richieste e ottenere risposte.
     *          
     *          // Figura 5 GetPost.jpeg
     *          
     *  Questa è quanto riguarda del modello di progettazione Client Server, è applicato in
     *  tutti i casi dello sviluppo del software e questa introduzione ti fa dare un idea degli
     *  elementi hardware coinvolti e dei moduli software come si distribuiscono per fare i 
     *  compiti del server BackEnd e del client se deve accedere con software dedicati o via browser,
     *  insomma è ormai consietudine parlare di queste componenti fondamentali.
     * 
     *  Questo modello è la versione più semplice di un modello a strati. C'è il client front-end 
     *  e il server back-end. A differenza dell'approccio a più livelli, il modello client-server 
     *  è più focalizzato sulle risorse. Questo approccio abbraccia l'idea di un server centrale 
     *  a cui i client si connettono per la condivisione dei dati. Fornisce inoltre un punto 
     *  centrale (il server) per ridimensionare l'applicazione. 
     *  Pertanto, è possibile pensare a questo come un'architettura hub e spoke con il server 
     *  come hub.
     *  
     *  Integrazione ridotta
     *  Qualsiasi sviluppatore che abbia dedicato del tempo alla creazione di applicazioni ti 
     *  dirà che una minore integrazione porta a soluzioni più semplici. 
     *  Sono disponibili scorciatoie e approcci semplificati quando si dispone di un singolo 
     *  livello. L'applicazione può fare le cose senza fare affidamento su come gli altri 
     *  fanno qualcosa o su ciò che si aspettano. 
     *  Il modello client-server ci offre il meglio di entrambi i mondi. 
     *  Possiamo lavorare quasi in un silo sul client e scaricare solo ciò che dobbiamo (i dati) 
     *  per la nostra soluzione. Anche le applicazioni per utente singolo a volte utilizzano 
     *  questo modello. Consente l'elaborazione e l'archiviazione distribuite, il che è 
     *  particolarmente utile man mano che le dimensioni dei dati aumentano.
     *  
     *  Prova
     *  Ci sono migliaia di applicazioni client-server che sono state scritte negli ultimi 
     *  cinquanta anni circa. Ciò significa che c'è molta conoscenza condivisa su come 
     *  adottare al meglio questo modello. Inoltre, abbiamo libri e siti che possono fornire 
     *  una conoscenza approfondita delle avvertenze e delle buche che si verificano. 
     *  Questo ci dà fiducia in un'attuazione di successo.
     *  
     *  Sfide
     *  Abbiamo detto all'inizio che questo è un modello che fatica a fornire soluzioni moderne. 
     *  Sì, è possibile per applicazioni mobili e web. Tuttavia, il modello di livello funziona 
     *  molto meglio a causa del numero di connessioni spesso richieste al datastore. 
     *  Una connessione a un server Web è molto più semplice ed economica da supportare rispetto 
     *  a una connessione di database. 
     *  Questo modello rende anche difficile memorizzare nella cache i dati e consegnarli 
     *  correttamente. Detto questo, questo è un modello che può soddisfare la maggior parte 
     *  delle tue esigenze anche se non è la soluzione più scalabile.
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
