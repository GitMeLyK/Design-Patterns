using System;

namespace DotNetDesignPatternDemos.Architectural.Clean_1
{
    /*
     * La progettazione e l'architettura del software si riferiscono generalmente alla base, 
     * alla struttura e all'organizzazione della soluzione. 
     * Durante la progettazione di una soluzione sostenibile, dovremmo sempre considerare 
     * la manutenibilità al top. Manutenibilità significa che la soluzione dovrebbe essere 
     * ben progettata con una solida base. La soluzione dovrebbe seguire i migliori principi 
     * e pratiche di progettazione.
     * 
     * In questo articolo, spiegherò cos'è un'architettura pulita e progetterò una 
     * soluzione in .NET. Inoltre, fornirò alcune informazioni sui principi di progettazione 
     * della soluzione.
     * 
     *      Architettura pulita
     *      Principi di base alla base di Clean Architecture
     *      Diagramma architettonico
     *      Livelli di architettura pulita
     *      Progettazione e strutturazione di una soluzione ad architettura pulita
     * 
     * Architettura pulita
     * 
     *      Clean Architecture, nota anche come Domain-Driven Design, si è evoluta con notevoli 
     *      miglioramenti negli ultimi anni. Alcuni nomi di architettura utilizzati per 
     *      l'architettura pulita nel corso degli anni sono riportati di seguito:
     *      
     *      Architettura Hexagonal      *Trattata in questo Lab
     *      Architettura Onion          *Trattata in questo Lab
     *      Domain-Driven Design (DDD) o Domain Centric Architecture
     *      Vertical slice Architettura 
     *      Architettura pulita
     *      
     * Le architetture sopra menzionate hanno principi di progettazione simili che hanno 
     * l'idea principale di mantenere la logica di business principale e il dominio 
     * dell'applicazione al centro della struttura della soluzione. 
     * 
     * In questa architettura, rendiamo la logica di business dell'applicazione di base e 
     * il dominio o le entità indipendenti dal livello di presentazione e dal livello di 
     * accesso ai dati.
     * 
     * Pertanto, l'intera idea di questa architettura è quella di consentire la parte 
     * principale, che consiste in una logica di business completa e entità applicative, 
     * abbastanza adattiva e flessibile da affrontare tecnologie e interfacce in evoluzione. 
     * 
     * Inoltre, l'applicazione principale rimane la stessa e indipendente dai livelli di 
     * presentazione, dalle infrastrutture e dai database.
     * 
     * In queste tecnologie frenetiche, i framework JavaScript, il framework web, il database e 
     * le parti esterne diventano assoluti o aggiornati, utilizzando questa architettura pulita 
     * è possibile sostituire questi elementi con il minimo sforzo. 
     * 
     * Questa architettura consente di risparmiare molto tempo perché la logica di business e 
     * l'entità dell'applicazione principale sono indipendenti da framework, presentazioni, 
     * database e parti esterne. 
     * 
     * Successivamente, l'architettura è sostenibile ed è liberamente accoppiata logica di 
     * business principale ed entità con livello di presentazione o framework.
     * 
     * In queste tecnologie frenetiche, i framework JavaScript, il framework web, il database 
     * e le parti esterne diventano assoluti o aggiornati, utilizzando questa architettura pulita 
     * è possibile sostituire questi elementi con il minimo sforzo. 
     * 
     * Questa architettura consente di risparmiare molto tempo perché la logica di business e 
     * l'entità dell'applicazione principale sono indipendenti da framework, presentazioni, 
     * database e parti esterne. 
     * 
     * Successivamente, l'architettura è sostenibile ed è liberamente accoppiata logica 
     * di business principale ed entità con livello di presentazione o framework.
     * 
     * Principi di base
     * 
     * L'idea principale in Clean Architecture è quella di rendere la soluzione adattiva, 
     * mantenere il core business o i casi d'uso della logica applicativa indipendenti dal 
     * frontend e dai framework esterni. 
     * 
     * In sintesi, possiamo delineare i seguenti risultati dell'architettura pulita,
     * 
     *  Indipendentemente dall'interfaccia utente
     *      Utilizzando un'architettura pulita, dovremmo essere in grado di modificare facilmente 
     *      i livelli dell'interfaccia utente o della presentazione senza modificare il livello 
     *      dell'applicazione e così via. 
     *      L'interfaccia utente può provenire da qualsiasi framework front-end o interfaccia 
     *      utente della console, da qualsiasi Web e può essere sostituita senza modificare gli 
     *      altri livelli o il resto del sistema.
     *      
     * Indipendente dal database 
     *      L'architettura deve essere sufficientemente flessibile da poter scambiare 
     *      il database senza influire sui casi d'uso e sulle entità dell'applicazione. 
     *      La soluzione può cambiare il set di dati in MS SQL, MySQL, Oracle, MongoDB 
     *      o qualcos'altro.
     *      
     * Indipendente da agent/dll/driver esterni
     *      Le regole aziendali dovrebbero essere indipendenti da parti o agent esterni.
     * 
     * Framework Independent
     *      Il core business o le regole applicative dovrebbero essere indipendenti 
     *      dall'esistenza di framework, librerie per il futuro. 
     *      Possiamo includere i framework ma come strumenti, e la soluzione non dovrebbe 
     *      essere completamente invocata su quelli.
     * 
     * Testabile
     *      L'architettura deve essere conforme al test dell'applicazione principale e dei 
     *      business case e regole senza l'interfaccia utente, il database, il server Web 
     *      o qualsiasi componente esterno.
     * 
     * Diagramma dell'architettura pulita
     *      Generalmente, l'architettura pulita è approfondita con l'illustrazione 
     *      circolare primaria presentata da Robot Martin (Zio Bob).
     *      
     *      // Fig1.png
     * 
     * Questo diagramma mostra perfettamente i concetti di alto livello alla base dell'architettura 
     * pulita. Possiamo interpretare questo diagramma in due modi, circolare o emisfero, tuttavia, 
     * è possibile comprendere l'idea di base da entrambi gli approcci.
     * 
     * Dal diagramma, vediamo che le entità sono il nucleo interno, che è generalmente noto come 
     * entità di dominio aziendale. Viene anche chiamato Enterprise Business Rules.
     * 
     * Queste entità aziendali sono coperte da casi d'uso, chiamati anche regole di business 
     * dell'applicazione. Questi casi d'uso vengono chiamati da relatori, controller o gateway, 
     * come illustrato nel diagramma precedente. Ulteriori interfacce esterne, DB, UI o web 
     * che sono comunemente chiamate shell pubblica o superficie o interfaccia pubblica. 
     * Tutti i framework e i driver sono in strati più esterni. 
     * Inoltre, possiamo osservare che il flusso proviene dal guscio pubblico alle entità interne.
     * 
     * Se comprendiamo da un altro lato, le dipendenze si spostano dall'interno all'esterno, cioè 
     * dal nucleo alla superficie esterna o pubblica. 
     * Le entità interne principali e i casi d'uso, chiamati anche livelli aziendali e applicativi, 
     * non hanno dipendenze e hanno meno probabilità di cambiare. 
     * Ogni livello di questo diagramma circolare ha dipendenze dal livello accanto ad esso. 
     * È più probabile che i livelli esterni cambino in base a tecnologie, framework e così via, 
     * di conseguenza, l'architettura della soluzione ha un impatto minore nella logica delle 
     * applicazioni di base.
     * 
     * Livelli di architettura pulita
     * 
     * In questa sezione, chiarirò i livelli di architettura pulita. 
     * Osserveremo di nuovo gli strati dell'illustrazione dello zio Bob.
     * 
     *      // Figura 2 : What is Clean Architecture2.png
     *      
     * Entità
     *      Questo è anche noto come regole aziendali aziendali. Consiste di domini semplici. 
     *      In questo livello, aggiungiamo oggetti (entità o dominio) senza framework e 
     *      annotazioni. Aggiungiamo logiche generali che vengono applicate a ogni dominio 
     *      come convalide, entità di base e così via. 
     *      Sono i meno influenzati da cambiamenti esterni e nessuna dipendenza.
     *      
     * Casi d'uso
     * 
     *      Questo è un livello logico puro in cui scriviamo core business o logica applicativa. 
     *      Questo è anche chiamato Regole di business dell'applicazione. 
     *      Noi, in generale, utilizziamo il termine servizio o gestore per i casi d'uso delle 
     *      applicazioni. Questo livello utilizza un livello di dominio e genera risultati. 
     *      In questo caso d'uso, non sappiamo chi attiva o come verrà presentato il risultato. 
     *      Tuttavia, in base ai servizi, manteniamo la logica di business indipendente 
     *      dall'interfaccia utente o dal database. Potremmo utilizzare alcune librerie, tuttavia, 
     *      solo come strumenti.
     *      
     * Adattatori di interfaccia
     * 
     *      Questo livello funge da comunicatore per convertire i dati nel formato desiderato per 
     *      l'archiviazione in origini esterne come database, file system, 3rd party e convertire 
     *      i dati per casi d'uso o logica di business. Questo livello è anche chiamato come 
     *      adattatori che necessariamente eseguono la conversione dei dati in entrambi i modi. 
     *      Possiamo considerare MVC di GUI o API REST a questo livello che consiste di relatori, 
     *      viste e controller. Implementa anche le interfacce dei casi d'uso richiesti per i 
     *      componenti esterni.
     *      
     * Interfacce esterne (framework e driver)
     * 
     *      Questo è il livello più esterno in questa architettura pulita che cambia 
     *      frequentemente in base alle tecnologie, agli aggiornamenti come database, 
     *      framework Web Front-end. In questo livello, presentiamo i dati all'interfaccia utente 
     *      o al database.
     *      
     * Progettazione di soluzioni
     *      
     *      L'architettura pulita è un insieme di principi organizzativi, che è la prima cosa 
     *      essenziale che dobbiamo capire. Possiamo progettare la soluzione in vari modi in 
     *      base ai requisiti o alle regolazioni personali, tuttavia, i principi fondamentali 
     *      devono essere mantenuti intatti e implementati correttamente.
     *      La struttura di esempio è simile a quella mostrata in figura
     *      
     *              // Figura 3 : image-2.png
     * 
     * Dominio Libreria di classi: 
     * 
     *      nessuna dipendenza, nessun riferimento a progetti o classi, nessuna logica
     * 
     * Applicazione Libreria di classi: 
     * 
     *      solo il dominio viene aggiunto come progetto di riferimento, logica di business o 
     *      servizi pure.
     *      
     * Dominio e Applicazione:
     * 
     *      sono il cuore di questa soluzione che sono indipendenti da Infrastruttura, 
     *      WebUI e librerie esterne.
     *      
     * Infrastruttura Libreria di classi: 
     * 
     *      la classe dell'applicazione viene aggiunta come riferimento. Questa classe è 
     *      responsabile delle comunicazioni dell'infrastruttura esterna come l'archiviazione 
     *      di database, il file system, i sistemi esterni /API/Servizi e così via.     
     *      Possiamo aggiungere più librerie di classi in questa cartella per plugin esterni o SDK 
     *      per organizzare la soluzione in modo migliore.
     * 
     * Interfaccia utente Web: 
     * 
     *      si tratta di un progetto di interfaccia utente Web di presentazione. Questo può 
     *      essere un MVC, framework front-end. Se stiamo progettando una soluzione basata 
     *      su API, possiamo mantenere sia l'API Web che il front-end in questo host di cartelle. 
     *      Aggiungiamo Applicazione e Infrastruttura come riferimento in questo progetto.
     *      
     * Questo è un modo per organizzare e progettare una soluzione di architettura pulita. 
     * Questo è un modo per strutturare la soluzione seguendo i principi dell'architettura pulita. 
     * Tuttavia, possiamo fare l'organizzazione in diversi modi, mantenendo intatti i valori 
     * fondamentali.
     * 
     * In questa illustrazione , abbiamo mantenuto le applicazioni di base indipendenti e altre 
     * infrastrutture e interfaccia utente Web dipendono dalle applicazioni principali.
     * 
     * Conclusione
     * 
     *  In questo articolo, ho descritto l'architettura pulita in modo approfondito con i valori 
     *  fondamentali. L'idea principale alla base di questa architettura pulita è quella di 
     *  progettare in modo tale che dovrebbe essere abbastanza adattiva e flessibile da gestire 
     *  le tecnologie in evoluzione, i componenti esterni e i framework di frontend Web. 
     *  
     *  Per raggiungere questo obiettivo, l'architettura pulita fornisce principi organizzativi 
     *  in cui manteniamo le entità principali e la logica di business indipendenti da livelli 
     *  di presentazione, infrastrutture, database e agenti esterni. 
     *  
     *  Inoltre, ho illustrato un diagramma architettonico di architettura pulita e l'ho approfondito 
     *  con i principi fondamentali. 
     *  
     *  Inoltre, ho spiegato i livelli di architettura pulita e infine ho condiviso il diagramma 
     *  di progettazione della soluzione con un esempio. 
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
