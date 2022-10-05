using System;

namespace DotNetDesignPatternDemos.Architectural.ADR.Example
{
    /*
     * Action-domain-responder (ADR) come perfezionamento di Model-view-controller (MVC) 
     * 
     * Il modello ADR è stato creato da Paul M. Jones nel 2014 e l'idea è, come RMR, di adattare 
     * MVC al contesto delle API REST web. 
     * 
     * La spiegazione originale di ADR è abbastanza semplice e chiara, non posso davvero riformularla 
     * molto meglio, quindi mi limiterò a copiare / incollare parti di esso qui e aggiungere solo 
     * qualche altro commento.
     * 
     *  // Figutra 1 : adr-22.png
     *  
     * Action
     * 
     * È la logica per connettere il dominio e il risponditore. Richiama il dominio con gli input 
     * raccolti dalla richiesta HTTP, quindi richiama il risponditore con i dati necessari per 
     * creare una risposta HTTP.
     * 
     * Domain
     * 
     * È un punto di ingresso alla logica del dominio che costituisce il nucleo dell'applicazione, 
     * modificando lo stato e la persistenza in base alle esigenze. 
     * Può trattarsi di uno script di transazione, di un livello di servizio, di un servizio 
     * applicazione o di qualcosa di simile.
     * 
     * Response
     * Logica di presentazione per creare una risposta HTTP dai dati ricevuti dall'azione. 
     * Si occupa di codici di stato, intestazioni e cookie, contenuto, formattazione e 
     * trasformazione, modelli e visualizzazioni e così via.
     * 
     * Come funziona
     * 
     *  1 - Il gestore Web riceve una richiesta HTTP e la invia a un'azione;
     *  
     *  2 - L'azione richiama il dominio, raccogliendo tutti gli input richiesti al dominio 
     *      dalla richiesta HTTP;
     *  
     *  3 - L'azione richiama quindi il risponditore con i dati necessari per creare una 
     *      risposta HTTP (in genere la richiesta HTTP e i risultati del dominio, se presenti);
     *  
     *  4 - Il Risponditore crea una risposta HTTP utilizzando i dati forniti dall'Azione;
     *  
     *  5 - L'azione restituisce la risposta HTTP al gestore Web che invia la risposta HTTP.
     * 
     * La risposta HTTP viene compilata dal risponditore analizzando e comprendendo la risposta 
     * del dominio, che a sua volta dipende dal caso d'uso del metodo di azione. 
     * 
     * Ciò significa che ogni metodo di azione richiede un Risponditore specifico. 
     * 
     * Se inserissimo tutti i metodi delle risorse nello stesso controller, dovremmo creare 
     * un'istanza e iniettare tutti i Risponditori, ma ne useremmo solo uno durante una 
     * richiesta HTTP, il che ovviamente non è ottimale. La soluzione è quindi quella di 
     * avere un solo metodo in ogni controller. 
     * 
     * Questo controller e il suo unico metodo di azione sono ciò che ADR chiama Azione.
     * 
     * Poiché Action ha un solo metodo, il nome del metodo può essere qualcosa di generico 
     * come nel caso di PHP rendendo la classe un richiamabile. 
     * 
     * Tuttavia, poiché l'idea è quella di adattare il modello MVC al contesto di 
     * un'API REST HTTP, i nomi Action (Controller) sono mappati ai metodi di richiesta 
     * HTTP, quindi avremo Azioni con nomi come rendendo esplicito il controller chiamato 
     * per ogni tipo di richiesta HTTP. 
     * 
     * Come modello di organizzazione, tutte le azioni su una risorsa devono essere raggruppate 
     * in una cartella denominata dopo la risorsa.
     * runexecute__invokeGetPostPutDelete
     * 
     * Idea sbagliata dell'ADR
     * 
     * Anthony Ferrara paragona ADR a RMR come "lo stesso modello con alcuni dettagli modificati".
     * 
     * In realtà penso che Anthony Ferrara abbia capito male:
     * 
     * "Resource==Domain"
     *      In RMR la Resource non è il Dominio, è un oggetto dominio, è un'entità di dominio, 
     *      mentre Dominio in ADR si riferisce all'insieme degli oggetti di dominio, a tutte le 
     *      entità e alle loro relazioni nel loro complesso;
     *      
     * "Rappresentazione== Responder"
     *      In RMR la Rappresentazione è la risposta inviata al client, in ADR un Responder è un 
     *      oggetto la cui responsabilità è quella di creare la risposta, dato alcuni contenuti 
     *      e alcuni modelli.
     *      
     * "condivide l'accoppiamento RMR a HTTP che diventa difficile creare un'interfaccia non HTTP"
     *      Poiché ADR vede il dominio nel suo insieme e non come un'entità, l'azione non è 
     *      all'interno di un oggetto dominio, chiede semplicemente agli oggetti dominio di 
     *      eseguire una logica di business. 
     *      Quindi il dominio non è accoppiato all'interfaccia utente, possiamo creare un 
     *      comando CLI che utilizza oggetti di dominio per eseguire alcune attività.
     *      
     * Opinione su questo modello
     * 
     *  Mi sembra che, al momento della stesura di questo documento, ADR sia il miglior 
     *  adattamento di MVC per il paradigma di richiesta/risposta HTTP perché adatta 
     *  chiaramente le richieste e le risposte HTTP alle richieste e alle risposte del 
     *  dominio pur mantenendo il dominio completamente disaccoppiato dal livello di 
     *  presentazione.
     *  
     *  I metodi delle richieste HTTP (azioni desiderate sulla risorsa) sono connessi in 
     *  modo esplicito al codice che riceve la richiesta HTTP perché ogni metodo HTTP viene 
     *  mappato direttamente al nome di un metodo controller. 
     *  
     *  Ciò ha l'ulteriore vantaggio di risultare in un'organizzazione del codice chiara, 
     *  esplicita e prevedibile rispetto ai controller con molte azioni, spesso non correlate, 
     *  spesso mal nominate e quindi imprevedibili, spesso facendo cose estremamente simili. 
     *  
     *  In altre parole, previene uno spaghetto fangoso di controllori e azioni.
     *  
     *  Inoltre, fa anche un ottimo lavoro nel disaccoppiare il codice che riguarda 
     *  l'interazione stessa (richiamando il dominio) dal codice che riguarda la 
     *  comprensione del risultato dell'interazione (la risposta del dominio) e 
     *  la traduzione al client.
     *  
     *  Tuttavia, ci sono alcuni punti da prendere in considerazione:
     *  
     *      Questo modello è stato pensato specificamente per le API REST, quindi in questa forma 
     *      non è abbastanza raffinato da essere utilizzato in applicazioni web con un'interfaccia 
     *      HTML (cioè quale sarebbe il nome dell'azione per mostrare un modulo, prima di creare 
     *      una risorsa?);
     *      
     *      Avere un solo metodo per controller rende questo modello più prolisso, perché, ad 
     *      esempio, invece di avere un controller (classe) con 4 azioni (metodi pubblici) avremo 
     *      quattro controller e quattro azioni;
     *      
     *      La creazione di risponditori per ogni azione aggiunge anche dettaglio al modello. 
     *      Se la logica per tradurre la risposta del dominio in una risposta HTTP è semplice, 
     *      dovremmo considerare se vale la pena utilizzare un Risponditore. 
     *      Non utilizzare i risponditori significherebbe che saremmo in grado di avere diversi 
     *      metodi in ogni controller, ognuno dei quali ancora mappato a un metodo HTTP.
     *      
     * Considerando i punti 2 e 3, Paul M. Jones li riconosce lui stesso e concorda sul fatto che 
     * in alcuni casi può essere accettabile utilizzare una semplificazione del modello che, 
     * sebbene non così elegante, può essere sufficiente per il contesto in questione.
     * 
     * Per quanto riguarda il punto 1, penso che il modello possa essere facilmente esteso in modo che 
     * sia completamente utilizzabile con un'interfaccia HTML: possiamo emulare alcuni metodi HTTP 
     * extra specificamente per gestire le richieste HTML che un'API REST non ha. 
     * 
     * Ad esempio, in un'API REST possiamo usare a o a per creare e / o aggiornare una risorsa e 
     * questo è tutto ciò che serve, tuttavia con un'interfaccia HTML dobbiamo mostrare un modulo 
     * prima di inviare un o una richiesta, ma non ci sono metodi HTTP che specificano che il client 
     * richiede un modulo per creare una risorsa, né un modulo per modificare una risorsa. 
     * 
     * Tuttavia, possiamo emulare questo utilizzando una richiesta con un'intestazione di o che il 
     * front controller può interpretare e inoltrare la richiesta all'azione corrispondente denominata 
     * o che risponderebbe quindi con il modulo HTML corrispondente. 
     * 
     * Avremmo bisogno, tuttavia, di essere molto attenti e minimalisti sui metodi HTTP personalizzati 
     * extra che creiamo ... altrimenti potrebbe portarci a una pletora di metodi HTTP personalizzati 
     * e un pasticcio di metodi HTTP personalizzati legati ad azioni spaghetti!! 
     * 
     * Quindi, procedi con cautela per quanto riguarda quest'ultimo mio suggerimento.
     * PUT POST POST PUT GET createedit CreateEdit
     * 
     * 
     * Per finire è stato ampiamente descritto qui il modello di progettazione come variante MVC per
     * essere usato in contesti di API e non di interfacce come le viste in MVC.
     * 
     * Un esempio di questo modello di progettazione completo è incluso come allegato a questo
     * documento.
     * 
     *      // File : adr-example-master.zip
     *      
     * Il codice è in php, purtroppo non ho trovato delle implementazioni in .net, non perchè non 
     * ce ne siano ma probabilmente non viene identificato il modello ma usato come tale.
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
