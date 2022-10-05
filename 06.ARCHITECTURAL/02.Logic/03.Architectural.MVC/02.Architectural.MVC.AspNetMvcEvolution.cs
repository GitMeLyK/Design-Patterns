using System;

namespace DotNetDesignPatternDemos.Architectural.ModelViewController.AspNetMvcEvolution
{
    /*
     * ASP.NET MVC Framework 5 in poi
     * 
     * Si era parlato poco fa delle librerie per modelli che ASP.NET mette a disposizione dei 
     * suoi utilizzatori. Tra i vari modelli è possibile trovare MVC Frameworks, 
     * un Model View Controller nato in casa Microsoft nel 2009 e divenuto una vera e propria 
     * aggiunta ad ASP.NET.
     * 
     * MVC Frameworks offre una valida alternativa al modello ASP.NET Web Forms, utilizzato per 
     * la creazione di applicazioni web e API.
     * 
     * Ma per che cosa sta MVC?
     * 
     *  Il Model è il modello dei dati e le relazioni che si relaizzano tra le diverse entità
     *  La View è la visualizzazione, in pratica si tratta del codice HTML che crea l’interfaccia utente
     *  Il Controller è il codice di controllo contenente la logica applicativa del programma
     *  
     *  La principale funzione di questo software è, in parole povere, quella di permettere una 
     *  più semplice e ordinata separazione delle competenze che danno vita ad un sito web.
     *  
     *  Il software è supportato dai canonici sistemi operativi, Windows, Linux e macOS e, 
     *  come ASP.NET, è completamente open source e leggero, ottimizzato proprio per 
     *  l’utilizzazione insieme al primo.
     *  
     *  Ma come funziona più precisamente ASP.NET MVC Frameworks?
     *  
     *  In parole povere, grazie a questo software le richiese dell’utente vengono indirizzate 
     *  ad un controller che dovrà a sua volta interagire con il modello per eseguire le richieste 
     *  dell’utente. 
     *  
     *  Spetta poi al controller scegliere la visualizzazione e i dati del modello necessari da 
     *  fornire all’utente. Tutto ciò rende l’applicazione più semplice per quanto riguarda il 
     *  fronte aggiornamenti, debug e test da eseguire su di essa.
     *  
     *  Qua di seguito sono elencate le funzionalità principali di ASP.NET MVC 5 Framework
     *  
     *          Routing
     *          Associazione di modelli
     *          Convalida modello
     *          Inserimento di dipendenze
     *          Filtri
     *          Aree
     *          API Web
     *          Testabilità
     *          Motore di visualizzazione Razor
     *          Visualizzazione molto tipizzate
     *          Helper Tag
     *          Componenti di visualizzazione
     * 
     * L'evoluzione da Model 2 al nuovo framework è anche incluso .net core ha portato
     * a fare passi da gigante nello sfruttare questo metodo di progettazione.
     * 
     * Con queste nuove versioni di framework evitiamo quindi di parlare ormai di Model 2 basato su
     * structs, ma di una e vera rivoluzione del modello verso uno standard preconfezionato nel 
     * framework attestato e pulito quindi MVC vero e proprio.
     * 
     * Alcune evolutive di questo pattern che sono state predefinite in questo modello di .net come
     * Razor per la visualizzazione le DI per le dipendenze e altro sono insiemi di supporto alla
     * configurazione ed al servizio ma non fanno parte essenziale del modello MVC nativo cioè non
     * si discostano mai dal progetto originario del modello, piuttosto sono contesti di helper e 
     * facility per le parti in cui servono, ad esempio Razor è le viste di mvc come le DI cono per
     * la configurazione dei controller etc.
     * 
     * L'uso è sempre lo stesso per lo stato, è sempre in pari con lo stateless del protocollo http.
     * 
     * Una parte fondamentale rispetto al .net framework fino al 4.6 ed al nuovo framework .net è
     * che nell'evoluzione di .net core MVC si fà un intenso uso del modello di progettazione IOC
     * per la configurazione del modlello MVC dove è possibile definire comportamenti e metodi nel
     * routing principale.
     * 
     * Ogni progetto nuovo MVC andrebbe approfondito nella storia delle evoluzioni in ambtio .net
     * e richiede degli studi a parte, che non fanno parte di questo contesto per questo lab che
     * si occupa semplicemente di dare una panormaica dei design pattern e dei modelli di progettazione.
     * 
     * Giusto per vedere la differenza tra MVC come è stato pensato nel Framework fino al 4.6 etichettandolo
     * MVC 5 e come è stato pensato in .Net core etichettandolo MVC 6.
     * 
     *      Uno degli aspetti che più sorprende (e che ha sorpreso anche me!) al momento della 
     *      scrittura di una nuova applicazione basata su .NET Core è sicuramente la mancanza 
     *      di alcuni elementi strutturali a cui ci siamo nel tempo abituati, e la presenza 
     *      di nuovi.
     *      
     *      Sicuramente la differenza principale è la possibilità di eseguire le applicazioni 
     *      scritte in .NET Core in ambienti multipiattaforma, e soprattutto anche senza dover 
     *      utilizzare necessariamente Visual Studio. 
     *      
     *      Il nuovo framework, infatti, integra al suo interno una serie di operazioni 
     *      “a linea di comando” che consentono la creazione di applicazioni senza dover 
     *      utilizzare l’Ide di casa Microsoft (anche se rimane consigliato).
     *      
     *      Vediamo quindi quelle che sono le differenze rispetto ad MVC5:
     *      
     *          - cartella App_start: nella struttura di un’applicazione basata su MVC5 conteneva 
     *                                la definizione dei processi, della configurazione e del routing. 
     *                                In .NET Core non è più necessaria, tutta la configurazione 
     *                                avviene all’interno del file startup.cs tramite IOC
     *                                
     *         - file Web.config:     non è più necessario! Abbandonato il modello di file basato 
     *                                su Xml a favore del json, le configurazioni sono memorizzate 
     *                                all’interno dle file appsettings.json.
     *                                
     *         - cartella App_data:   tipicamente utilizzata per dati locali (database, log ecc..) 
     *                                non è fornita nel template standard. Può comunque essere aggiunta 
     *                                manualmente.
     *                                
     *         - cartella Scripts:    anche questa cartella non è più presente. Tutti i file statici 
     *                                risiedono all’interno della cartella wwwroot/js
     *                               
     *         - file Global.asax:   non è più necessario! Le configurazioni devono essere inserite 
     *                               all’interno del file startup.cs tramite IOC anche questo.
     *                               
     *   Inoltre anche la struttura delle cartelle riporta una serie di modifiche sostanziali:
     *   
     *          - appsettings.json: è il successore del file Web.config.
     *          
     *          - Cartella src:     è la cartella root dell’intera applicazione, chiamata appunto 
     *                              cartella dei sorgenti
     *                              
     *          - file project.json: è il file di configurazione dell’intera soluzione. 
     *                              A differenza della precedente versione è possibile aggiungere 
     *                              e rimuovere dipendenze semplicemente modificando questo file.
     *                              
     *          - Cartella wwwroot: utilizzata per la memorizzazione dei contenuti statici. 
     *                              All’interno di questa cartella possono essere memorizzati 
     *                              (all’interno di opportune sottocartelle) i file relativi ai 
     *                              css, js e le librerie di terze parti.
     *                              
     *          - Cartella Dependencies: contiene le dipendenze del nostro progetto. Ricordiamo 
     *                              il pieno supporto al package manager npm e bower.
     *                              
     *  Questa era una panoramica delle evolutive e si è approfondito meglio per quanto riguarda
     *  il passaggio al nuovo framework .net core. 
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
