using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;

namespace DotNetDesignPatternDemos.Architectural.ModelViewViewModel.WPF
{
    /*
     * In questo esempio vengono descritti l'utilizzo e le funzionalità di base del 
     * modello MVVM in WPF.
     * 
     * Model ViewModel (MVVM) è un modello architetturale utilizzato nell'ingegneria 
     * del software che ha avuto origine da Microsoft, specializzata nel modello di 
     * progettazione del modello di presentazione. Si basa sul modello MVC 
     * (Model-view-controller pattern) ed è destinato alle moderne piattaforme di 
     * sviluppo dell'interfaccia utente (WPF e Silverlight) in cui esiste uno sviluppatore 
     * UX che ha requisiti diversi rispetto a uno sviluppatore più "tradizionale". 
     * 
     * MVVM è un modo per creare applicazioni client che sfrutta le funzionalità principali 
     * della piattaforma WPF, consente semplici unit test delle funzionalità delle applicazioni 
     * e aiuta sviluppatori e progettisti a collaborare con meno difficoltà tecniche.
     * 
     * VIEW: una visualizzazione è definita in XAML e non deve avere alcuna logica nel code-behind. 
     *       Si associa al modello di visualizzazione utilizzando solo l'associazione dati.
     *       
     * MODELLO: un modello è responsabile dell'esposizione dei dati in un modo facilmente 
     *          utilizzabile da WPF. Deve implementare INotifyPropertyChanged e/o 
     *          INotifyCollectionChanged a seconda dei casi.
     *          
     * VIEWMODEL: Un ViewModel è un modello per una vista nell'applicazione o possiamo dire 
     *            come astrazione della vista. Espone i dati rilevanti per la vista ed espone 
     *            i comportamenti per le viste, in genere con i comandi.
     *            
     *  Come utilizzare MVVM C #?
     *  L'utilizzo di Model-View-ViewModel (MVVM) è il modello di progettazione del software 
     *  architetturale che separa i controlli dell'interfaccia utente e la logica dei programmi. 
     *  
     *  È anche chiamato Model-View-Binder ed è sviluppato da architetti Microsoft.
     *  
     *      //Figrua 2
     *      
     *  I tre componenti agiscono come una squadra facendo riferimento l'un l'altro nel 
     *  seguente modello come segue:
     *      
     *      View sottolinea il ViewModel
     *      ViewModel indica il modello
     *      
     *  La cosa essenziale è che ViewModel e View sono in grado di comunicare in due metodi 
     *  chiamati Associazioni dati. 
     *  
     *  Il componente principale per la comunicazione è l'interfaccia chiamata 
     *  INotifyPropertyChanged.
     *  
     *  Per utilizzare questo metodo, view deve modificare le informazioni nel ViewModel 
     *  attraverso l'input del client e ViewModel deve aggiornare la view con le informazioni 
     *  che sono state aggiornate attraverso i processi nel modello o alle informazioni 
     *  aggiornate dal repository. 
     *  
     *  L'architettura MVVM (Model View ViewModel) attribuisce grande importanza alla 
     *  separazione delle preoccupazioni per ogni singolo livello. 
     *  
     *  Separando gli strati, ci sono alcuni altri vantaggi. Vediamo le seguenti cose.
     *  
     *      Modularità:             La modularità supporta che è stato alterato o scambiato durante 
     *                              l'implementazione interna dei livelli senza disturbare gli altri.
     *      Maggiore testabilità:   In questo, ogni singolo componente deve essere testato da uno unit 
     *                              test con informazioni false ed è impossibile se il programma ViewModel 
     *                              è scritto in Code-Behind of View.
     *      
     *  MVVM C# ViewModel in Esempi
     *  
     *      Vediamo le responsabilità di ViewModel come segue:
     *      
     *          ViewModel è la cosa essenziale nell'applicazione MVVM (Model-View-ViewModel). 
     *                    Il compito più importante di ViewModel è quello di presentare le informazioni 
     *                    alla vista in modo che la vista inserisca i dati richiesti sullo schermo.
     *          ViewModel consente all'utente di collaborare con le informazioni e modificare i dati.
     *          ViewModel incapsula la logica di relazione per la visualizzazione, ma non significa che 
     *                    la logica dell'applicazione sia necessaria per entrare in ViewModel.
     *          ViewModel gestisce la serie di chiamate adatta per costruire l'elemento accurato da 
     *                    verificare in base al client e alle eventuali modifiche sulla vista.
     *          ViewModel gestisce la logica di navigazione, come scegliere quando è il momento di 
     *                    navigare in varie viste.
     *          
     * Per creare il nuovo progetto di applicazione WPF per una migliore comprensione di ViewModel.
     * 
     *  Quindi crea tre cartelle per Model, View e ViewModel e rimuovi il file ManiWindow.xaml 
     *  esistente, nient'altro che iniziare da capo.
     *  
     *      // Figura MVVM3.jpg
     *      
     *  Quindi, crea nuovi elementi e ciascuno dei corrispondenti ai componenti separati.
     *  
     *      Inizialmente a destra, fare clic sulla cartella del modello per includere l'elemento 
     *      Class e denominarlo come HelloWorldModel.cs.
     *      
     *      Quindi, fare clic con il pulsante destro del mouse sulla cartella ViewModel, 
     *      includere gli elementi Class e denominarla helloWorldViewModel.cs.
     *      
     *      Quindi, fai clic con il pulsante destro del mouse sulla cartella Visualizza, 
     *      includi l'elemento WPF (Window) e assegnagli il nome HellowWorldView.xaml.
     *  
     *     // Figura MVVM4.jpg
     *     
     *  Nel file di visualizzazione modifica app.xaml in modo che punti alla nuova visualizzazione 
     *  come indicato di seguito.
     *  
     *      // Figura MVVM5.jpg
     *      
     *  ViewModel:
     *  
     *      In ViewModel, inizialmente iniziare con la compilazione di ViewModel e la classe 
     *      deve includere l'interfaccia denominata INotifyPropertyChanged per indicare che 
     *      l'evento PropertyChangedEventHandler e per compilare il metodo per generare l'evento. 
     *      
     *      Quindi, dichiarare il campo e la proprietà correlata e assicurarsi di chiamare 
     *      il metodo OnPropertyChanged () nelle proprietà come accesso impostato. 
     *      Vediamo l'esempio a livello di codice in cui il costruttore viene utilizzato 
     *      per visualizzare il modello e fornisce i dati al ViewModel.
     *      
     *      // Code 1
     *      
     *  Modello:
     *  
     *      Quindi vieni alla creazione del modello, offre i dati per ViewModel trascinandolo 
     *      dal repository e inoltre tornerà al repository per scopi di archiviazione. 
     *      
     *      Qui il programma è spiegato con il metodo GetInfo () dove restituisce il semplice 
     *      elenco <string> e la logica di business applicata anche qui, e ci sarà 
     *      ConcatenateData() Metodo. Questo viene utilizzato per creare il messaggio 
     *      "Hello World" dall'elenco <string> restituito dal repository.
     *      
     *      // Code 2
     *      // MVVM6.jpg
     *  
     *  Vista:
     *  
     *      Infine, dobbiamo creare una vista; non è altro che deve includere alcuni codici 
     *      nel codice XAML; la finestra richiede i riferimenti allo spazio dei nomi ViewModel. 
     *      Viene quindi mappato a XAML. La cosa principale è assicurarsi di associare la 
     *      proprietà di ViewModel, che visualizza il contenuto dell'etichetta.
     *  
     *      // MVVM7.jpg
     *      
     *  
    */

    /*
     * Code 1
            using System;
            using System.Collections.Generic;
            using System.ComponentModel;
            using System.Linq;
            using System.Runtime.CompilerServices;
            using System.Text;
            using System.Threading.Tasks;
            using MyMVVMProject.Model;
            namespace MyMVVMProject.ViewModel
             {
             // the interface INotifyPropertyChanged implements for supporting the binding purpose
             public class HelloWorldViewModel : INotifyPropertyChanged
              {
              private string _helloString;
              public event PropertyChangedEventHandler PropertyChanged;
              public string HelloString
              {
               get
               {
                return _helloString;
               }
              set
              {
               helloString = value;
               OnPropertyChanged();
             }
            }
            /// <summary>
            /// when the Property modifies it Raises OnPropertychangedEvent
            /// </summary>
            /// <param name="name">Property name represented by String</param>
            protected void OnPropertyChanged([CallerMemberName] string name = null)
            {
             PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            }
            public HelloWorldViewModel()
              {
               HelloWorldModel hwModel1 = new HelloWorldModel();
               _helloString = hwModel1.ImportantInfo;
              }
             }
            }
     */

    /*
    * Code 2
        private List<string> GetInfo ()
         {
          repositoryData = new List<string>()
          {
           "Hello",
           "world"
         };
         return repositoryData;
        }
    */

    /*
    * Code 3

    */

    /*
     * Code 4
    
     */



    class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
