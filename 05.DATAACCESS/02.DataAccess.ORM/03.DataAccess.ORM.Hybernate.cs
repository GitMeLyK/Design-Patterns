using System;

namespace DotNetDesignPatternDemos.DataAccess.ORM.Hybernate
{
    /*
     * Hibernate ORM (o semplicemente Hibernate) è uno strumento di mappatura relazionale a oggetti 
     * per il linguaggio di programmazione Java. 
     * 
     * Fornisce un framework per il mapping di un modello di dominio orientato agli oggetti a un 
     * database relazionale. 
     * 
     * L'ibernazione gestisce i problemi di mancata corrispondenza dell'impedenza relazionale 
     * oggetto sostituendo gli accessi diretti e persistenti al database con funzioni di gestione 
     * degli oggetti di alto livello.
     * 
     * Hibernate è un software libero distribuito sotto la GNU Lesser General Public License 2.1.
     * 
     * La funzionalità principale di Hibernate è il mapping dalle classi Java alle tabelle di 
     * database e il mapping dai tipi di dati Java ai tipi di dati SQL. 
     * 
     * Hibernate fornisce anche funzionalità di query e recupero dei dati. 
     * 
     * Genera chiamate SQL e solleva lo sviluppatore dalla gestione manuale e dalla conversione 
     * degli oggetti del set di risultati.
     * 
     * Mappatura
     * 
     * Il mapping delle classi Java alle tabelle di database viene implementato mediante la 
     * configurazione di un file XML o utilizzando le annotazioni Java. 
     * 
     * Quando si utilizza un file XML, Hibernate può generare codice sorgente di ossatura 
     * per le classi di persistenza. 
     * 
     * Questo è ausiliario quando vengono utilizzate le annotazioni. 
     * 
     * Hibernate può utilizzare il file XML o le annotazioni Java per mantenere lo schema 
     * del database.
     * 
     * Ci sono strutture fornite per organizzare relazioni uno-a-molti e molti-a-molti tra 
     * le classi. Oltre a gestire le associazioni tra oggetti, Hibernate può anche gestire 
     * associazioni riflessive in cui un oggetto ha una relazione uno-a-molti con altre 
     * istanze del tipo di classe.
     * 
     * Hibernate supporta il mapping di tipi di valore personalizzati. Ciò rende possibili 
     * i seguenti scenari:
     * 
     *  - Sostituzione del tipo SQL predefinito durante il mapping di una colonna a una proprietà.
     *  - Mapping di Java Enums alle colonne come se fossero proprietà regolari.
     *  - Mapping di una singola proprietà a più colonne.
     *
     * Gli oggetti in un'applicazione orientata agli oggetti seguono i principi OOP, mentre gli 
     * oggetti nel back-end seguono i principi di normalizzazione del database, con conseguenti 
     * requisiti di rappresentazione diversi. 
     * 
     * Questo problema è chiamato "disallineamento dell'impedenza relazionale oggetto- relazione". 
     * La mappatura è un modo per risolvere il problema di mancata corrispondenza dell'impedenza 
     * relazionale oggetto.
     * 
     * La mappatura informa lo strumento ORM dell'oggetto di classe Java da archiviare in quale 
     * tabella di database.
     * 
     * Hibernate Query Language (HQL)
     * 
     *      Hibernate fornisce un linguaggio ispirato a SQL chiamato Hibernate Query Language 
     *      (HQL) per la scrittura di query simili a SQL sugli oggetti dati di Hibernate. 
     *      
     *      Criteri Le query vengono fornite come alternativa orientata agli oggetti all'HQL. 
     *      Criteri Query viene utilizzato per modificare gli oggetti e fornire la restrizione 
     *      per gli oggetti. HQL (Hibernate Query Language) è la versione orientata agli 
     *      oggetti di SQL. 
     *      
     *      Genera query indipendenti dal database in modo che non sia necessario scrivere 
     *      query specifiche del database. 
     *      
     *      Senza questa funzionalità, la modifica del database richiederebbe la modifica 
     *      anche di singole query SQL, con conseguenti problemi di manutenzione.
     * 
     * Persistenza
     * 
     *      L'ibernazione fornisce una persistenza trasparente per i POJO (Plain Old Java Objects). 
     *      L'unico requisito rigoroso per una classe persistente è un costruttore senza argomenti, 
     *      anche se non necessariamente. 
     *      
     *      Il comportamento corretto in alcune applicazioni richiede inoltre un'attenzione particolare 
     *      ai metodi e nelle classi di oggetti. 
     *      
     *      Hibernate consiglia di fornire un attributo identificatore, e questo è previsto per essere 
     *      un requisito obbligatorio in una versione futura. public equals() hashCode()
     *      
     *      Le raccolte di oggetti dati vengono in genere archiviate in classi di raccolta Java, 
     *      ad esempio implementazioni di interfacce e interfacce. 
     *      
     *      Sono supportati anche i generici Java, introdotti in Java 5. 
     *      
     *      L'ibernazione può essere configurata per caricare pigramente le raccolte associate. 
     *      
     *      Il caricamento pigro è l'impostazione predefinita a partire da Hibernate 3. 
     *      SetList
     *      
     *      Gli oggetti correlati possono essere configurati per operazioni a cascata da un 
     *      oggetto all'altro.
     *      
     *      Ad esempio, un oggetto classe padre può essere configurato per collegare in cascata 
     *      le proprie operazioni agli oggetti classe figlio. Album save delete Track
     *      
     * Integrazione
     * 
     *      L'ibernazione può essere utilizzata sia nelle applicazioni Java autonome che nelle 
     *      applicazioni Java EE utilizzando servlet, fagioli di sessione EJB e componenti del 
     *      servizio JBI. 
     *      
     *      Può anche essere incluso come funzionalità in altri linguaggi di programmazione. 
     *      
     *      Ad esempio, Adobe ha integrato Hibernate nella versione 9 di ColdFusion (che viene 
     *      eseguita su server app J2EE) con un livello di astrazione di nuove funzioni e sintassi 
     *      aggiunte in CFML.
     *      
     * Entità e componenti
     * 
     *      Nel gergo hibernate, un'entità è un oggetto autonomo nel meccanismo persistente di 
     *      Hibernate che può essere manipolato indipendentemente da altri oggetti. 
     *      
     *      Al contrario, un componente è subordinato a un'entità e può essere manipolato solo 
     *      rispetto a tale entità. 
     *      
     *      Ad esempio, un oggetto Album può rappresentare un'entità; ma l'oggetto Tracks 
     *      associato agli oggetti Album rappresenterebbe un componente dell'entità Album, 
     *      se si presume che le tracce possano essere salvate o recuperate dal database solo 
     *      tramite l'oggetto Album. 
     *      
     *      A differenza di J2EE, Hibernate può cambiare database.
     *      
     * Versioni
     * 
     * Hibernate è stato avviato nel 2001 da Gavin King con i colleghi di Cirrus Technologies 
     * come alternativa all'utilizzo di fagioli di entità in stile EJB2. 
     * 
     * L'obiettivo originale era quello di offrire migliori capacità di persistenza rispetto 
     * a quelle offerte da EJB2; semplificando le complessità e integrando alcune funzionalità 
     * mancanti.
     * 
     * All'inizio del 2003, il team di sviluppo di Hibernate ha iniziato le versioni di 
     * Hibernate2, che hanno offerto molti miglioramenti significativi rispetto alla prima 
     * versione.
     * 
     * JBoss, Inc. (ora parte di Red Hat) in seguito assunse i principali sviluppatori di 
     * Hibernate per promuoverne lo sviluppo.
     * 
     * Nel 2005 è stata rilasciata la versione 3.0 di Hibernate. 
     * 
     * Le caratteristiche principali includevano una nuova architettura Interceptor/Callback, 
     * filtri definiti dall'utente e annotazioni JDK 5.0 (funzionalità di metadati di Java). 
     * 
     * A partire dal 2010, Hibernate 3 (versione 3.5.0 e successive) era un'implementazione 
     * certificata della specifica Java Persistence API 2.0 tramite un wrapper per il modulo 
     * Core che fornisce la conformità allo standard JSR 317.
     * 
     * Nel dicembre 2011 è stato rilasciato Hibernate Core 4.0.0 Final. 
     * Ciò include nuove funzionalità come il supporto multi-tenancy, l'introduzione di 
     * ServiceRegistry (un importante cambiamento nel modo in cui Hibernate crea e gestisce 
     * i "servizi"), una migliore apertura delle sessioni da SessionFactory, 
     * una migliore integrazione tramite org.hibernate.integrator.spi.Integrator e 
     * l'individuazione automatica, il supporto per l'internazionalizzazione, i codici dei 
     * messaggi nella registrazione e una maggiore distinzione tra API, SPI o classi di implementazione. 
     * 
     * Nel dicembre 2012 è stato rilasciato Hibernate ORM 4.1.9 Final.
     * Nel marzo 2013 è stato rilasciato Hibernate ORM 4.2 Final.
     * Nel dicembre 2013 è stato rilasciato Hibernate ORM 4.3.0 Final.  
     *  È dotato di Java Persistence API 2.1.
     * Nel settembre 2015 è stato rilasciato Hibernate ORM 5.0.2 Final. 
     *  Ha migliorato il bootstrap, l'ibernazione-java8, l'ibernazione-spaziale, il supporto Karaf.
     * Nel novembre 2018 è stato rilasciato Hibernate ORM 5.1.17 Final. 
     *   Questa è la versione finale della serie 5.1.
     * Nell'ottobre 2018 è stato rilasciato Hibernate ORM 5.3 Final. 
     *   Presentava java persistence API 2.2 inheritance caching.
     * Nel dicembre 2018 è stato rilasciato Hibernate ORM 5.4.0 Final
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
