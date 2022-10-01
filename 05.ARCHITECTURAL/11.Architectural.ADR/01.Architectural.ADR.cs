using System;

namespace DotNetDesignPatternDemos.Architectural.ADR
{
    /*
     * Action-domain-responder (ADR) è un modello architetturale software proposto da 
     * Paul M. Jones[1] come perfezionamento di Model-view-controller (MVC) più adatto 
     * per le applicazioni web. ADR è stato concepito per abbinare il flusso di 
     * richiesta-risposta delle comunicazioni HTTP più da vicino rispetto a MVC, 
     * originariamente progettato per applicazioni software desktop. 
     * Simile a MVC, il modello è diviso in tre parti.
     * 
     * ADR non deve essere confuso con una ridenominazione di MVC; 
     * tuttavia, esistono alcune somiglianze.
     *  
     *  - Il modello MVC è molto simile al dominio ADR. La differenza è nel comportamento: 
     *  in MVC, la vista può inviare o modificare il modello, mentre in ADR, 
     *  il dominio riceve solo informazioni dall'azione, non dal risponditore.
     *  
     *  - In MVC incentrato sul Web, la visualizzazione viene semplicemente utilizzata 
     *  dal controller per generare il contenuto di una risposta, che il controller 
     *  potrebbe quindi manipolare prima di inviarla come output. In ADR, il controllo 
     *  dell'esecuzione passa al risponditore al termine dell'interazione dell'azione 
     *  con il dominio e quindi il risponditore è interamente responsabile della 
     *  generazione di tutto l'output. Il risponditore può quindi utilizzare qualsiasi 
     *  sistema di visualizzazione o modello necessario.
     *  
     *  - I controllerMVC di solito contengono diversi metodi che, se combinati in una 
     *  singola classe, richiedono una logica aggiuntiva per essere gestiti correttamente, 
     *  come gli hook pre e post-azione. Ogni azione ADR, tuttavia, è rappresentata da 
     *  classi o chiusure separate. In termini di comportamento, l'azione interagisce con 
     *  il dominio nello stesso modo in cui il controller MVC interagisce con il modello, 
     *  tranne per il fatto che l'azione non interagisce quindi con un sistema di 
     *  visualizzazione o modello, ma piuttosto passa il controllo al risponditore 
     *  che lo gestisce.
     * 
     */

    /*
     * Componenti
     * 
     *  - L'azione accetta le richieste HTTP (URL e relativi metodi) e utilizza tale 
     *    input per interagire con il dominio, dopodiché passa l'output del dominio 
     *    a un solo e un solo risponditore.
     *  - Il dominio può modificare lo stato, interagendo con l'archiviazione e/o 
     *    manipolando i dati in base alle esigenze. Contiene la logica di business.
     *  - Il risponditore crea l'intera risposta HTTP dall'output del dominio che 
     *    gli viene dato dall'azione.
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
