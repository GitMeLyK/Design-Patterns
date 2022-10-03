using System;

namespace DotNetDesignPatternDemos.Architectural.Model2
{
    /*
     *  JSP Model 2 è un modello di progettazione complesso utilizzato nella 
     *  progettazione di applicazioni Web Java che separa la visualizzazione 
     *  del contenuto dalla logica utilizzata per ottenere e manipolare il contenuto. 
     *  
     *  Poiché il modello 2 determina una separazione tra logica e visualizzazione, 
     *  di solito è associato al paradigma modello-vista-controller (MVC). 
     *  
     *  Mentre la forma esatta del "Modello" MVC non è mai stata specificata dal 
     *  progetto del Modello 2, un certo numero di pubblicazioni consiglia un livello 
     *  formalizzato per contenere il codice del Modello MVC. I BluePrint Java, ad esempio, 
     *  originariamente consigliavano di utilizzare EJB per incapsulare il modello MVC.
     *  
     *  In un'applicazione Model 2, le richieste provenienti dal browser client vengono 
     *  passate al controller. Il controller esegue qualsiasi logica necessaria per ottenere 
     *  il contenuto corretto per la visualizzazione. 
     *  Quindi inserisce il contenuto nella richiesta (comunemente sotto forma di JavaBean o POJO) 
     *  e decide a quale vista passerà la richiesta. 
     *  La vista esegue quindi il rendering del contenuto passato dal controller.
     *  
     *  Storia
     *  
     *  Nel 1998, Sun Microsystems pubblicò una pre-release della specifica JavaServer Pages, 
     *  versione 0.92. In questa specifica, Sun ha stabilito due metodi con cui le pagine JSP 
     *  potevano essere utilizzate. Il primo modello (indicato come "modello 1" a causa 
     *  del suo ordinamento nel documento) era un modello semplicistico in cui le pagine JSP 
     *  erano entità autonome e disgiunte. 
     *  
     *  La logica poteva essere contenuta all'interno della pagina stessa e la navigazione 
     *  tra le pagine era in genere ottenuta tramite collegamenti ipertestuali. 
     *  
     *  Questo si adatta all'uso allora comune della tecnologia dei modelli.
     *  
     *  ColdFusion e Active Server Pages sono esempi di tecnologie contemporanee che hanno 
     *  implementato anche questo modello.
     *  
     *  Il secondo modello a cui fa riferimento il documento ("modello 2" nell'ordinamento) 
     *  era un metodo migliorato che combinava la tecnologia servlet con la tecnologia JSP. 
     *  La differenza specifica elencata era che un servlet intercettava la richiesta, 
     *  inseriva il contenuto da renderizzare in un attributo di richiesta (in genere 
     *  rappresentato da un JavaBean), quindi chiamava un JSP per eseguire il rendering 
     *  del contenuto nel formato di output desiderato. Questo modello differiva dal 
     *  modello precedente nel fatto che la tecnologia JSP veniva utilizzata come puro 
     *  motore di modelli. Tutta la logica è stata separata in un servlet, lasciando al 
     *  JSP la sola responsabilità di rendere l'output per il contenuto fornito.
     *  
     *  Nel dicembre 1999, JavaWorld pubblicò un articolo di Govind Seshadri intitolato 
     *  Understanding JavaServer Pages Model 2 architecture. 
     *  
     *  In questo articolo, Govind ha raggiunto due importanti pietre miliari nell'uso 
     *  del termine "Modello 2". La prima pietra miliare è stata quella di formalizzare 
     *  il termine "Modello 2" come modello architettonico piuttosto che una delle due 
     *  opzioni possibili. La seconda pietra miliare è stata l'affermazione che Model 2 
     *  ha fornito un'architettura MVC per il software basato sul web.
     *  
     *  Govind credeva che, poiché l'architettura "Model 2" separava la logica dal JSP e 
     *  la collocava in un servlet, i due pezzi potevano essere visti come "View" e 
     *  "Controller" (rispettivamente) in un'architettura MVC. 
     *  
     *  La parte "Model" dell'architettura MVC è stata lasciata aperta da Govind, con il 
     *  suggerimento che quasi tutte le strutture di dati potrebbero soddisfare i requisiti. 
     *  
     *  L'esempio specifico utilizzato nell'articolo era un elenco Vector memorizzato 
     *  nella sessione dell'utente.
     *  
     *  Un malinteso comune è che sia necessario un modello MVC formalizzato per ottenere 
     *  un'implementazione del Modello 2. 
     *  
     *  Tuttavia, i BluePrint Java mettono in guardia specificamente contro 
     *  questa interpretazione:
     *  
     *      La letteratura sulla tecnologia a livello Web nella piattaforma J2EE utilizza 
     *      spesso i termini "Modello 1" e "Modello 2" senza spiegazione. 
     *      Questa terminologia deriva dalle prime bozze della specifica JSP, 
     *      che descriveva due modelli di utilizzo di base per le pagine JSP. 
     *      Mentre i termini sono scomparsi dal documento di specifica, rimangono di uso comune. 
     *      Model 1 e Model 2 si riferiscono semplicemente all'assenza o alla presenza 
     *      (rispettivamente) di un servlet controller che invia richieste dal livello 
     *      client e seleziona le viste.
     *      
     *      Inoltre, il termine "MVC2" ha portato molti a credere erroneamente che il Modello 2 
     *      rappresenti un modello MVC di prossima generazione. In realtà, MVC2 è semplicemente 
     *      un accorciamento del termine "MVC Model 2". 
     *      
     *      La confusione sul termine "MVC2" ha portato a un'ulteriore confusione sul codice 
     *      Model 1, con conseguente uso comune del termine inesistente "MVC1".
     *      
     *      Nel marzo 2000 è stato rilasciato il progetto Apache Struts. 
     *      Questo progetto ha formalizzato la divisione tra View e Controller e ha rivendicato 
     *      l'implementazione del modello "Model 2". [3] 
     *      Ancora una volta, l'implementazione del "Modello" è stata lasciata indefinita con 
     *      l'aspettativa che gli sviluppatori di software avrebbero compilato una soluzione 
     *      appropriata. L'interazione del database tramite JDBC ed EJB è stata suggerita 
     *      sulla homepage di Struts. Più recentemente, Hibernate, iBatis e Object Relational 
     *      Bridge sono stati elencati come opzioni più moderne che potrebbero essere utilizzate 
     *      per un modello. 
     *      
     *      Dal rilascio di Struts, sono apparsi numerosi framework concorrenti. 
     *      Molti di questi framework affermano anche di implementare "Model 2" e "MVC". 
     *      Di conseguenza, i due termini sono diventati sinonimi nella mente degli sviluppatori. 
     *      Ciò ha portato all'uso del termine "MVC Model 2" o "MVC2" in breve.
     */

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
