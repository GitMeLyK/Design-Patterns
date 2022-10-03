using System;

namespace DotNetDesignPatternDemos.Architectural.Hexagonal
{

    /*
     * Si tratta di un modello di architettura introdotto da Alistair Cockburn nel 2005, 
     * che risolverà i problemi di manutenzione delle applicazioni nell'architettura tradizionale, 
     * che abbiamo usato per implementare dall'architettura database-centrica.
     * 
     * L'architettura esagonale, o per chiamarla correttamente, "Ports and Adapters pattern", 
     * è guidata dall'idea che l'applicazione sia centrale per il tuo sistema. 
     * Tutti gli ingressi e le uscite raggiungono o lasciano il nucleo dell'applicazione 
     * attraverso una porta che isola l'applicazione da tecnologie, strumenti e meccanismi 
     * di consegna esterni.
     * 
     *      // Figura Hexagonal Architecture In ASP.NET Core.png
     * 
     *  Wiki -------------------------------------------------------------------------------------
     *  L'architettura esagonale divide un sistema in diversi componenti intercambiabili 
     *  ad accoppiamento debole, come il core dell'applicazione, il database, l'interfaccia 
     *  utente, gli script di test e le interfacce con altri sistemi. 
     *  
     *  Questo approccio è un'alternativa alla tradizionale architettura a strati.
     *  
     *  Ogni componente è collegato agli altri attraverso una serie di "porte" esposte. 
     *  La comunicazione attraverso queste porte segue un determinato protocollo a seconda 
     *  del loro scopo. 
     *  
     *  Le porte e i protocolli definiscono un'API astratta che può essere implementata 
     *  con qualsiasi mezzo tecnico adatto (ad esempio, chiamata di metodo in un linguaggio 
     *  orientato agli oggetti, chiamate di procedure remote o servizi Web).
     *  
     *  La granularità delle porte e il loro numero non sono vincolati:
     *  
     *      Un solo punto potrebbe in alcuni casi essere sufficiente (ad esempio nel caso 
     *      di un semplice consumatore di servizi);
     *      
     *      In genere, ci sono porte per le origini degli eventi (interfaccia utente, 
     *      alimentazione automatica), notifiche (notifiche in uscita), database 
     *      (al fine di interfacciare il componente con qualsiasi DBMS adatto) e 
     *      amministrazione (per il controllo del componente);
     *      
     *      In un caso estremo, potrebbe esserci una porta diversa per ogni caso d'uso, 
     *      se necessario.
     *      
     *  Gli adattatori sono il collante tra i componenti e il mondo esterno. Adattano gli 
     *  scambi tra il mondo esterno e le porte che rappresentano i requisiti dell'interno 
     *  del componente applicativo. 
     *  
     *  Possono essere presenti diversi adattatori per una porta, ad esempio, i dati possono 
     *  essere forniti da un utente tramite una GUI o un'interfaccia a riga di comando, 
     *  da un'origine dati automatizzata o da script di test.
     *  
     *  Critica
     *  
     *  Il termine "esagonale" implica che ci sono 6 parti del concetto, mentre ci sono 
     *  solo 4 aree chiave. L'uso del termine deriva dalle convenzioni grafiche che mostrano 
     *  il componente dell'applicazione come una cella esagonale. 
     *  
     *  Lo scopo non era quello di suggerire che ci sarebbero stati sei confini/porte, ma 
     *  di lasciare abbastanza spazio per rappresentare le diverse interfacce necessarie tra 
     *  il componente e il mondo esterno. 
     *  
     *  Secondo Martin Fowler, l'architettura esagonale ha il vantaggio di utilizzare somiglianze 
     *  tra livello di presentazione e livello di origine dati per creare componenti simmetrici 
     *  costituiti da un nucleo circondato da interfacce, ma con lo svantaggio di nascondere 
     *  l'asimmetria intrinseca tra un fornitore di servizi e un consumatore di servizi che 
     *  sarebbe meglio rappresentato come livelli.
     * 
     *  Nota.: Secondo alcuni autori, l'architettura esagonale è all'origine dell'architettura dei microservizi.
     *  
     *  L'architettura Onion proposta da Jeffrey Palermo nel 2008 è simile all'architettura esagonale: 
     *  esternalizza anche l'infrastruttura con interfacce adeguate per garantire un accoppiamento 
     *  libero tra l'applicazione e il database. [6] 
     *  Scompone ulteriormente il nucleo dell'applicazione in diversi anelli concentrici usando 
     *  l'inversione del controllo. 
     *  
     *  L'architettura pulita proposta da Robert C. Martin nel 2012 combina i principi 
     *  dell'architettura esagonale, l'architettura Clean e diverse altre varianti; 
     *  Fornisce ulteriori livelli di dettaglio del componente, che vengono presentati come 
     *  anelli concentrici. Isola adattatori e interfacce (interfaccia utente, database, 
     *  sistemi esterni, dispositivi) negli anelli esterni dell'architettura e lascia gli 
     *  anelli interni per casi d'uso ed entità. L'architettura pulita utilizza il principio 
     *  dell'inversione della dipendenza con la regola rigorosa che le dipendenze esistono 
     *  solo tra un anello esterno a un anello interno e mai il contrario.
     *  
     *  -------------------------------------------------------------------------------------
     *  
     *  -- Vantaggi dell'architettura esagonale
     *  
     *  Plug & play
     *  
     *      Possiamo aggiungere o rimuovere l'adattatore in base al nostro sviluppo, 
     *      come possiamo sostituire l'adattatore Rest con l'adattatore GraphQL o gRPC. 
     *      Il resto della logica rimarrà lo stesso
     *      
     *  Testabilità
     *  
     *      Poiché disaccoppiava tutti i livelli, è facile scrivere un test case per ogni 
     *      componente
     *      
     *  Adattabilità/Miglioramento
     *  
     *      Aggiungere un nuovo modo di interagire con le applicazioni è molto semplice
     *      
     *  Sostenibilità
     *  
     *      Possiamo mantenere tutte le librerie di terze parti a livello di infrastruttura 
     *      e quindi la manutenzione sarà facile
     *  
     *  Indipendente dal database
     *  
     *      Poiché il database è separato dall'accesso ai dati, è abbastanza facile cambiare 
     *      provider di database
     *      
     *  Codice pulito
     *  
     *      Poiché la logica di business è lontana dal livello di presentazione, è facile 
     *      implementare l'interfaccia utente (come React, Angular o Blazor)
     *      
     *  Ben organizzato
     *  
     *      Il progetto è ben organizzato per una migliore comprensione e per l'onboarding 
     *      per il nuovo joinee al progetto
     *      
     * -- Svantaggi dell'architettura esagonale
     * 
     *  Il livello di dominio sarà pesante
     *  
     *  Molta logica verrà implementata nel livello di dominio (a volte chiamato livello Core)
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
