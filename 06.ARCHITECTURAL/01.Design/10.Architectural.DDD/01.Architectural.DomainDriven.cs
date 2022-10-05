using System;

namespace DotNetDesignPatternDemos.Architectural.DomainDriven
{
    /*
     * Il DDD è un lavoro di analisi che DEVE, quindi, coinvolgere un gruppo altamente 
     * eterogeneo di collaboratori: tecnici, analisti, potenziali utenti finali, persone 
     * del business,.. così che possano provenire contributi da tutti i livelli di competenza 
     * e l’analisi non sia così contaminata da una visione troppo specifica 
     * (troppo business o troppo tecnica)
     * 
     * Definizioni fondamentali
     * 
     *  - Dominio
     *      Un contesto di conoscenza (ontologia), influenza, o attività. L'area in cui 
     *      l'utente lavora con il software è il dominio dello stesso.
     *      
     *  - Modello
     *      Un sistema di astrazione che descrive specifici aspetti del dominio e che 
     *      può essere usato per risolvere problemi relativi alla definizione del dominio.
     *      
     *  - Ubiquitous Language
     *      Un set di termini creato attorno al modello di dominio utilizzato dal team 
     *      per indirizzare e contestualizzare i problemi da risolvere durante l'implementazione.
     *      
     *  - Contesto
     *      L'ambiente in cui un determinato termine assume un determinato significato.
     * 
     *  Strategie nel DDD
     *  
     *   Idealmente, è preferibile avere un singolo modello unificato. Anche se questo è un 
     *   nobile proposito, nella realtà ci dobbiamo tipicamente scontrare con più modelli 
     *   che coesistono. È utile integrare questa assunzione e lavorare con essa.
     *   
     *   Le strategie nel design sono una serie di principi atti a mantenere l'integrità 
     *   del modello, e che portano a sviluppare il modello di dominio e di interazione 
     *   tra i vari modelli.
     *   
     *  Il contesto come delimitato e definito
     *  
     *   In progetti di grandi dimensioni molti modelli devono interagire tra loro. 
     *   Spesso quando il codice che opera su diversi modelli deve essere integrato il lavoro 
     *   diventa difficile, facendo emergere bug e rendendo il progetto difficile da comprendere 
     *   e mantenere. La comunicazione tra i membri del team diventa confusa; diventa quindi poco 
     *   chiaro a quale contesto un modello è applicato.
     *   
     *   La miglior soluzione è definire in maniera esplicita il contesto al quale un modello è 
     *   applicato; inoltre, è utile esplicitare i confini a livello di organizzazione del team, 
     *   per chiarire in quali punti dell'applicazione si usa un modello e soprattutto riversare 
     *   queste assunzioni nell'implementazione. 
     *   
     *   Più di tutto, è fondamentale mantenere il modello coerente all'interno dei limiti posti, 
     *   non facendosi influenzare e confondere dall'ecosistema che sta intorno.
     *   
     * Continuous integration
     * 
     *   Quando più persone lavorano allo stesso contesto definito, c'è una forte tendenza a 
     *   frammentare il modello. Più grosso è il team, più grosso è il problema, anche un team 
     *   di tre o quattro persone può incontrare problemi. Inoltre frammentare in maniera 
     *   eccessiva il contesto porta a problemi nell'integrazione e nella coerenza delle 
     *   informazioni.
     *   
     *   La miglior soluzione è istituire un processo frequente di merging del codice con 
     *   uno step di testing. In questo modo si esercita una costante condivisione della 
     *   concezione del modello che può evolvere liberamente con i contributi di ogni sviluppatore.
     *   
     * Context map
     * 
     *    Un singolo contesto delimitato porta ad una serie di problemi in assenza di una visione 
     *    globale. Il contesto di altri modelli potrebbe essere ancora vago e non completo.
     *    Gli sviluppatori degli altri team potrebbero non essere consapevoli del dominio in 
     *    cui il contesto opera, quindi la modifica ad un contesto si rende complicata a fronte 
     *    di confini non ben definiti. Quando interconnessioni tra contesti si rendono necessarie, 
     *    accade talvolta che i limiti si fondano.
     *    
     *    La soluzione è definire ogni modello che esiste nel progetto e definirne i limiti. 
     *    Questo include modelli impliciti di sotto progetti non object-oriented. 
     *    Trovare il nome di ogni contesto delimitato, e renderlo parte del linguaggio specifico 
     *    del dominio. Descrivere i punti di contatto tra i modelli, esplicitando le traduzioni 
     *    che occorrono nella comunicazione ed evidenziando le condivisioni di informazioni.
     * 
     * Costruire le basi del DDD
     * 
     *      I concetti chiave e pratiche, come il significato ubiquitous language cioè un 
     *      linguaggio unificato creato dagli esperti del dominio che viene usato per descrivere 
     *      i requisiti di sistema, che inoltre si adatta all'utilizzo da parte di vari attori 
     *      quali utenti, produttori e sviluppatori. Descrivere lo strato di dominio in un 
     *      sistema orientato agli oggetti con un'architettura multistrato. 
     *      Nel DDD, esistono artefatti per esprimere, creare e recuperare modelli di dominio:
     *      
     *      Entity
     *      Un oggetto che non è definito dai suoi attributi, ma piuttosto dalla sua identità. 
     *      Per esempio, la maggior parte delle compagnie aeree distinguono in modo univoco ogni 
     *      sedile su ogni volo. Ogni sedile è un'entità in questo contesto. Tuttavia, Southwest 
     *      Airlines, EasyJet e Ryanair non distinguono tra tutti i sedili: tutti i sedili sono 
     *      tra loro indistinguibili. In questo contesto il sedile è in realtà un value object.
     *      
     *      Value Object
     *      Un oggetto che ha una serie di attributi ma non ha identità concettuale. I value 
     *      object devono essere trattati come oggetti immutabili. Per esempio quando le persone 
     *      si scambiano banconote da un dollaro, essi sono solo interessati al valore nominale 
     *      della banconota, non alla singola identità delle banconote scambiate. 
     *      In questo contesto, le banconote da un dollaro sono rappresentate da value object. 
     *      Tuttavia, la Federal Reserve potrebbe essere interessata alla singola banconota 
     *      identificata dal suo numero seriale: in tale contesto ogni banconota è più propriamente 
     *      rappresentata da una entità.
     *      
     *      Aggregate
     *      Una collezione di oggetti che sono legati insieme da un'entità padre, altrimenti nota 
     *      come una radice di aggregato. La radice di aggregazione garantisce la consistenza dei 
     *      cambiamenti compiuti all'interno dell'aggregato proibendo ad oggetti esterni di 
     *      detenere i riferimenti ai suoi membri interni. Per esempio, quando si guida una 
     *      macchina, non ci si deve preoccupare di muovere le ruote in avanti, tenere il motore 
     *      in combustione attraverso l'accensione e del combustibile, ecc.; si sta semplicemente 
     *      guidando l'auto. In questo contesto, la macchina è un aggregato di diversi altri 
     *      oggetti e serve come radice aggregata per tutti gli altri sistemi.
     *      
     *      Domain Event
     *      Un oggetto di dominio che definisce un evento (qualcosa che accade). Un evento di 
     *      dominio è un evento di assoluta importanza per gli esperti di dominio.
     *      
     *      Service
     *      Quando un'operazione non appartiene logicamente a nessun oggetto. Quindi è possibile 
     *      implementare queste operazioni nel settore dei servizi.
     *      
     *      Repository
     *      Metodi per il recupero di oggetti di dominio delegati da un oggetto repository 
     *      specializzato tale che l'implementazione di storage alternativi sia facile.
     *      
     *      Factory
     *      Metodi per la creazione di oggetti di dominio che delegano ad un oggetto la loro 
     *      creazione così da facilitare le implementazioni alternative.
     *      
     * Svantaggi
     *  Al fine di mantenere il modello come un costrutto del linguaggio puro e utile, il team 
     *  deve tipicamente implementare l'isolamento con grande attenzione e prestare attenzione 
     *  all'incapsulamento all'interno del modello di dominio. Di conseguenza, un sistema 
     *  costruito attraverso il Domain Driven Design può essere oneroso in termini di costo di 
     *  sviluppo.
     *  
     *  Quindi un sistema basato sul Domain Driven Design ha un costo relativamente alto. 
     *  Il Domain Driven Design offre certamente numerosi vantaggi tecnici, ad esempio la 
     *  manutenibilità.
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
