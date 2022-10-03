using System;

namespace DotNetDesignPatternDemos.Architectural.ModelViewPresentation.Example
{
    /*
     * Model View Presenter (MVP) è un modello di progettazione particolarmente utile 
     * per l'implementazione di interfacce utente in modo tale da disaccoppiare il 
     * software in problemi separati, come quelli destinati all'elaborazione e 
     * all'archiviazione dei dati (modello), alla logica di business, al routing 
     * dei comandi utente e così via, rendendo così disponibile più codice per unit test.
     * 
     * Il modello di progettazione MVP separa le seguenti preoccupazioni:
     * 
     *      La modalità         Memorizza i dati da visualizzare o su cui agire nell'interfaccia utente.
     *      
     *      La vista.           Interfaccia utente passiva che visualizza i dati del modello e instrada gli 
     *                          eventi avviati dall'utente, ad esempio i comandi del clic del mouse, al relatore 
     *                          per agire su tali dati.
     *      
     *      Il presentatore.    Agisce sul modello e sulla vista. Recupera i dati dal modello e li 
     *                          visualizza nella vista.
     *                          
     *      // Figura MVP3.jpg
     *  
     *  In generale, ci sono due tipi di MVP: il relatore supervisore e la vista passiva. 
     *  
     *      - Il relatore supervisore consente l'accoppiamento tra la vista e il modello, mentre la 
     *        vista passiva lo vieta. 
     *  
     *      - La vista passiva è da preferire se si desidera massimizzare l'unità di test, 
     *        incoraggiando al contempo viste sottili che non contengono logica. 
     *  
     *  La vista passiva è l'approccio descritto in questo esempio.
     *  
     *  Quando l'utente invia una richiesta, ad esempio un clic del mouse su un controllo pulsante, 
     *  la vista (che avrà creato i suoi oggetti Presenter e Model) accetta la richiesta e delega 
     *  la richiesta all'oggetto Presenter, che richiamerà un metodo scelto a sé stante.
     *  
     *  Il relatore, che è in grado di ottenere sia lo stato della vista corrente 
     *  (testo della casella di testo, numero intero di selezione della casella di riepilogo, ecc.) 
     *  sia i dati del modello corrente, eseguirà tutti i calcoli o la logica di business 
     *  richiesta e aggiornerà la vista con i risultati e opererà sul modello come appropriato. 
     *  
     *  La vista crea un'istanza dell'oggetto Presenter nel relativo costruttore, fornendo così 
     *  un riferimento a se stesso.
     *  
     *  Esaminiamo l'operazione MVP utilizzando un semplice WinForm come sequenza di passaggi:
     *  
     *      1. L'Utente invia una richiesta come la pressione del pulsante 'Imposta' di controllo:
     *      
     *      // Figura MVP1.jpg
     *      
     *      2. La vista accede direttamente al relatore. Accetta la richiesta e delega l'input 
     *         dell'utente al relatore. La vista può anche rispondere a qualsiasi evento avviato 
     *         dal modello sottoscrivendoli.
     *      
     *          // Code 1
     *      
     *          Visualizza l'interfaccia come segue. Contiene le proprietà per impostare e 
     *          recuperare il contenuto dei controlli di visualizzazione, ad esempio le caselle 
     *          di testo. Inoltre può contenere eventi per notificare le interazioni dell'utente 
     *          come clic sui pulsanti, eventi del mouse ecc.
     *         
     *      3. Il Presentatore agisce come una sorta di "intermediario" tra le interfacce Model e View. 
     *         Ottiene lo stato corrente dall'interfaccia di visualizzazione (testo della casella di 
     *         testo, elemento di visualizzazione elenco selezionato, ecc.) e richiama un metodo 
     *         proprio scelto, ad esempio l'esecuzione di alcuni calcoli o logica di business. 
     *         Il relatore comanda inoltre le modifiche dello stato del modello in base alle esigenze:
     *         
     *         // Code 2
     *      
     *      4. Il modello aggiorna e memorizza le modifiche apportate al suo stato. 
     *         Come risultato delle modifiche apportate al suo stato, il modello può anche generare 
     *         eventi propri, per notificare ai propri clienti le modifiche in modo che possano agire 
     *         di conseguenza, ad esempio modificando la visualizzazione dell'interfaccia utente.
     *         
     *          // Code 3
     *          
     *          Come risultato delle modifiche apportate al suo stato, il modello può anche generare 
     *          eventi propri, per notificare ai propri clienti le modifiche in modo che possano agire 
     *          di conseguenza, ad esempio modificando la visualizzazione dell'interfaccia utente:
     *          
     *          // MVP2.jpg
     *          
     *          E facendo clic sul pulsante REVERSE vedere che l'etichetta di testo è aggiornata come mostrato:
     *          
     *          // MVP3.jpg
     *          
     *   Una caratteristica importante dell'MVP è che il modello può consentire a più viste di 
     *   osservare i propri dati. 
     *   
     *   Un'altra distinzione importante è che il modello non è a conoscenza né della vista né 
     *   del presentatore.
     *   
     *   In allegato in formato compresso l'intero codice per questo esempio.
     *          
     */


    /*
     * Code 1

    using MVP.Presenter;
    using MVP.Views;
    using System.Runtime.CompilerServices;

    namespace MVP
    {
        public partial class Form1 : Form, IView
        {
            public event EventHandler<string>? SetText;
            public event EventHandler<string>? ReverseText;
            public event EventHandler? ClearText;

            readonly TextPresenter? presenter = null;

            public Form1()
            {
                InitializeComponent();
                presenter = new(this);
            }

            private void ButtonSet_Click(object sender, EventArgs e)
            {
                presenter?.SetTextDisplay(InputText);
                SetText?.Invoke(this, InputText);
            }

            private void ButtonReverse_Click(object sender, EventArgs e)
            {
                var currentText = LabelText;
                presenter?.ReverseTextDisplay();
                var newText = InputText;
                ReverseText?.Invoke(this, currentText);
            }

            private void buttonClear_Click(object sender, EventArgs e)
            {
                presenter?.ClearTextDisplay();
                ClearText?.Invoke(this, EventArgs.Empty);
            }

            public string LabelText
            {
                get { return labelTxt.Text; }
                set { labelTxt.Text = value; }
            }

            public string InputText
            {
                get { return inputTxt.Text; }
                set { inputTxt.Text = value; }
            }
        }
    }
    */

    /*
     * Code 2
        using MVP.Models;
        using MVP.Views;
        using System.Windows.Forms;
 
        namespace MVP.Presenter
        {
            public  class TextPresenter
            {
                private readonly IView _textDisplay;
                public TextPresenter(IView textDisplay)
                {
                    _textDisplay = textDisplay;
                    textDisplay.SetText += (o, e) => {
                        MessageBox.Show("Text label set to: " + e.ToString());
                    };
 
                    textDisplay.ReverseText += (o, e) => {
                        MessageBox.Show("Text label reversed from " + e.ToString() + " to: " + _textDisplay.LabelText);
                    };
 
                    textDisplay.ClearText += (o, e) => {
                        MessageBox.Show("Text label cleared");
                    };
                }
 
                public void ReverseTextDisplay()
                {
                    TextDisplay textDisplay = new();
                    string currentText = _textDisplay.LabelText;
                    var reverseText = textDisplay.Reverse(currentText);
                    _textDisplay.LabelText = reverseText;
                }
 
                public void SetTextDisplay(string text)
                {
                    _textDisplay.LabelText = text;
                }
 
                internal void ClearTextDisplay()
                {
                    _textDisplay.LabelText = "";
                    _textDisplay.InputText = "";
                }
            }
        }
    */

    /*
     * Code 3
     namespace MVP.Models
     {
        public class TextDisplay : ITextDisplay
        {
            public string Reverse(string text)
            {
                char[] charArray = text.ToCharArray();
                Array.Reverse(charArray);
                return new string(charArray); 
            }
        }
     }

    // Astrazione del modello come segue:
    namespace MVP.Models
    {
        public interface ITextDisplay
        {
            string Reverse(string text);
        }   
    }

    */

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
