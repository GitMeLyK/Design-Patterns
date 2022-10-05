using System;

namespace DotNetDesignPatternDemos.Architectural.NakedObjects.FW
{
    /*
     * Implementazione del 'naked objects pattern' sulla piattaforma .NET. 
     *      
     *      Trasforma un modello di dominio POCO (che segue alcune semplici convenzioni) 
     *      in un'applicazione completa.
     * 
     * Questo repository ospita ora due framework di sviluppo di applicazioni .NET: 
     * 
     *  Naked Objects e Naked Functions. 
     *  
     * Con Naked Objects scrivi il codice della tua applicazione in puro stile orientato agli oggetti; 
     * con Naked Functions lo scrivi in puro stile di programmazione funzionale. 
     * 
     * Ma è ciò che hanno in comune che li distingue da altri framework di sviluppo di applicazioni:
     * 
     *  Si scrivono solo tipi di dominio e logica. Per Oggetti naked si intendono le classi che 
     *  rappresentano entità di dominio persistenti e modelli di visualizzazione, con tutta la 
     *  logica di dominio incapsulata come metodi. 
     *  
     *  Per Naked Functions significa scrivere record C# (o classi immutabili) e funzioni 
     *  indipendenti (statiche) che sono al 100% prive di effetti collaterali.
     *  
     *  La persistenza viene gestita tramite Entity Framework Core o Entity Framework 6
     *  
     *  Utilizzando l'introspezione (durante l'avvio) i framework generano un'API RESTful 
     *  completa per il codice di dominio.
     *  
     *  Un client generico utilizza questa API RESTful per fornire un'interfaccia utente avanzata. 
     *  
     *  Il client, che è comune sia agli oggetti nudi che alle funzioni naked, è scritto 
     *  in Angular 12 e viene eseguito come un'applicazione a pagina singola (SPA).
     *  
     *  Il client generico può essere personalizzato per l'aspetto e la sensazione utilizzando 
     *  modelli angolari standard e la bellezza del design è che questa personalizzazione può 
     *  essere intrapresa in modo completamente indipendente dallo sviluppo dell'applicazione 
     *  di dominio. 
     *  
     *  Molti utenti hanno scoperto che non è necessario personalizzarlo affatto: 
     *  il client generico è abbastanza buono per la distribuzione. 
     *  All'altro estremo, poiché il Client adotta un'architettura a più livelli ben 
     *  strutturata (ogni livello è un pacchetto NPM separato) puoi scegliere di costruire 
     *  la tua SPA da zero, utilizzando solo i livelli inferiori dell'architettura Client 
     *  generica come helper per interagire con l'API RESTful.
     *  
     *  I framework possono essere utilizzati già confezionati (come pacchetti NuGet per 
     *  il lato server e come pacchetti NPM per il client) - non è necessario scaricare il 
     *  codice sorgente da questo repository.
     *  
     *  Entrambi i framework offrono quindi i seguenti vantaggi:
     *  
     *      Ciclo di sviluppo rapido.
     *      
     *      Manutenzione semplificata: 
     *      la modifica del modello di dominio non richiede alcuna modifica al codice 
     *      dell'interfaccia utente (o al livello di persistenza se si adottano le procedure 
     *      consigliate di Entity Framework).
     *      
     *      Aspetto dell'interfaccia utente coerente al 100%, su modelli di dominio grandi 
     *      e complessi o applicazioni multiple.
     *      
     *      Funzionamento server senza stato con tutti i vantaggi di distribuzione derivanti 
     *      dall'utilizzo di un'API RESTful pura.
     *      
     *      Migliore comunicazione tra utenti e sviluppatori durante lo sviluppo/manutenzione 
     *      perché l'interfaccia utente corrisponde direttamente al modello di dominio sottostante.
     *      
     * -- Oggetti naked
     * 
     * Naked Objects è un framework maturo, in continuo sviluppo da 20 anni 
     * (gli ultimi 7 su GitHub) e ora alla versione 12 (con il lavoro sulla v13 iniziato).
     * 
     * La documentazione completa su come utilizzare il framework (in genere a partire dai 
     * progetti Template) è contenuta nel Naked Objects - Developer Manual. 
     * Non è necessario scaricare e compilare il codice sorgente, poiché il modo consigliato 
     * per utilizzare il framework è tramite i pacchetti NuGet e NPM pubblicati. 
     * (Tuttavia ci sono dettagli nel manuale su come costruire il sorgente per coloro che 
     * lo vogliono davvero.)
     * 
     * Differenze tra v12 e v11: Ora funziona con Entity Framework Core o Entity Framework 6
     * 
     * Consente alle proprietà (incluse le proprietà della raccolta) di essere "contrributed" 
     * a un oggetto (utilizzando il nuovo attributo DisplayAsProperty) in modo simile al 
     * concetto esistente di "azioni con contributo".
     * 
     * Consente di annotare le azioni il cui scopo è modificare una o più proprietà su un 
     * oggetto permanente con il nuovo attributo Modifica e quindi consentire di richiamare 
     * l'azione utilizzando la nuova icona "modifica" accanto a uno di questi campi e di 
     * modificare i valori di proprietà in situ anziché tramite una finestra di dialogo separata.
     * 
     * -- Funzioni naked
     * Naked Functions è un framework nuovo di zecca, attualmente alla release 1.0. 
     * Dipende da .NET 6 ed Entity Framework Core.
     * 
     * La documentazione completa su come utilizzare il framework (in genere a partire 
     * dai progetti Template) è contenuta nel Naked Functions - Developer Manual. 
     * 
     * Non è necessario scaricare e compilare il codice sorgente, poiché il modo consigliato 
     * per utilizzare il framework è tramite i pacchetti NuGet e NPM pubblicati.
     * 
     * 
     * In allegato al presente documento è disponibile il progetto Framework completo, altriemnti
     * come consigliato per usare il modello di progettazione naked usare Npm o NuGet.
     * 
     *  // File :   NakedObjectsFramework-master
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
