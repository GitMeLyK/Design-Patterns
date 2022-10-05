using System;

namespace DotNetDesignPatternDemos.Architectural.ModelViewController.ExampleMvc6
{
    /*
     * Riportiamo giusto un esempio dii una prima applicazione sviluppata con
     * il nuovo framework .net core 6 e i primi passi per utilizzare il modello
     * di progettazione MVC portato in questo framewrok ed etichettato brevemente
     * MVC 6.
     * 
     * Con viusal studio partire da un progetto template per la creazione di questo
     * progetto di esempio selezionando il modello chiamato ASP.NET Core Web App (Model-View-Controller) 
     * e quindi fare clic sul pulsante "Avanti".
     * 
     * Cosa dovrei imparare prima di imparare ASP.NET Core
     * 
     * Prima di iniziare a imparare ASP.NET Core, è necessario avere una conoscenza di base di 
     * HTML CSS e C #. HTML e CSS vengono utilizzati nelle viste che costituiscono il componente 
     * dell'interfaccia utente di ASP.NET Core MVC. C# è un linguaggio di programmazione per creare 
     * logiche di codice come la comunicazione con il database, la ricerca di un valore nei dati 
     * e così via.
     * 
     * Successivamente configurerai il tuo nuovo progetto. Quindi aggiungi il nome del progetto 
     * come FirstApp e seleziona la posizione sul tuo disco in cui desideri creare questa 
     * applicazione.
     * 
     * Fare clic sul pulsante Avanti per continuare. Si raggiungerà una pagina Di informazioni 
     * aggiuntive in cui è necessario selezionare il framework e l'autenticazione.
     * 
     *      // Figura 1 : additional-information-visual-studio-2022.webp
     *  
     *  Verificare che .NET 6.0 Framework sia selezionato e che l'autenticazione sia impostata 
     *  su Nessuno. Anche l'opzione di Docker dovrebbe essere deselezionata.
     *  
     *  Infine fai clic sul pulsante Crea per creare la tua applicazione.
     *  
     *  La tua app verrà creata in pochi secondi ed è pronta per essere eseguita. 
     *  Selezionare Avvia debug dal menu Debug (se viene richiesto di abilitare il debug, 
     *  fare clic sul pulsante OK). È inoltre possibile avviare il debug utilizzando un 
     *  collegamento che è il tasto F5.
     *  
     *  Visual Studio compilerà l'applicazione e la aprirà nel browser predefinito. 
     *  Avrà un aspetto simile a quello mostrato nell'immagine sottostante.
     *  
     *      // Figura 2 : first-application.webp
     *      
     *  Qui è stato scelto il modello MVC (Model-View-Controller) in modo che le cartelle e i file MVC 
     *  necessari vengano creati automaticamente da Visual Studio. È possibile aprire il menu Esplora 
     *  soluzioni da Visualizza ➤ Esplora soluzioni, verrà aggiunto nell'angolo destro di Visual 
     *  Studio e mostrerà tutti questi file / cartelle nell'app appena creata. 
     *  
     *  Se hai scelto l'altra opzione, che è il modello "Vuoto", devi creare questi file e cartelle 
     *  MVC uno per uno dal menu file.
     *  
     *      // Figura 3 : solution-explorer.webp
     *      
     *  Cerchiamo di comprendere questi file e cartelle nella tua applicazione.
     *  
     *      - wwwroot – contiene i file statici come immagini, script, framework esterni e 
     *                  librerie come Bootstrap, jQuery.
     *                  
     *      - Controller: questa cartella contiene i file del controller.
     *      
     *      - Modelli: questa cartella contiene i file del modello.
     *      
     *      - Visualizzazioni: questa cartella contiene i file di visualizzazione.
     *      
     *      - appsettings.json: questo file contiene le impostazioni di configurazione 
     *                          dell'applicazione. È possibile utilizzarlo per archiviare 
     *                          stringhe di connessione al database, valori delle variabili 
     *                          dell'applicazione e altre informazioni.
     *                          
     *      - program.cs:       È il punto di ingresso dell'applicazione che inizia quando si 
     *                          esegue l'applicazione. Qui crei il tuo host applicativo, scegli 
     *                          il server web, aggiungi servizi, autorizzazioni e autenticazioni
     *                          il tutto tramite il pattern IoC per inserire le dipendenze per 
     *                          controller e servizi necessari, oltre anche a definire questi
     *                          puoi implementare contesti per middleware e swagger e tanto altro.
     *                          
     *     - startup.cs:        – Nota – DOT NET 6.0 non dispone di startup.cs. Se usi solo 
     *                          versioni precedenti di .NET, devi avere questo file nell'app. 
     *                          Il .cs di avvio viene chiamato dal file .cs programma. 
     *                          Qui si aggiungono servizi e si configura la pipeline HTTP. 
     *                          È inoltre possibile scrivere route URL in questo file.
     *                          il tutto tramite il pattern IoC per inserire le dipendenze per 
     *                          controller e servizi necessari, oltre anche a definire questi
     *                          puoi implementare contesti per middleware e swagger e tanto altro.
     * 
     * Quanto sopra sono solo una breve descrizione di questi file e cartelle MVC. 
     * 
     * - Aggiunta di un controller
     * 
     *  Come spiegato in precedenza, un ASP.NET Core Controller è una classe C#, VB o F#, il cui 
     *  compito è gestire tutte le richieste HTTP in arrivo. Un controller può disporre di uno o 
     *  più metodi di azione. Ogni metodo di azione può essere richiamato dal Web da alcuni URL. 
     *  Per esempio: Se un controller denominato 'Home' ha un metodo di azione denominato 
     *  'Calculate', questo metodo di azione può essere richiamato aprendo 
     *  l'URL – /Home/Calculate sul browser.
     *  
     *      // Figur 3 : add-new-controller-solution-explorer.webp
     *      
     *  Per impostazione predefinita, Visual Studio aggiunge il controller "Home". 
     *  Ora aggiungerai un nuovo controller, quindi fai clic con il pulsante destro del 
     *  mouse sulla cartella Controller in Esplora soluzioni. 
     *  
     *  Quindi selezionare Aggiungi ➤ Controller nel menu.
     *      
     *  Otterrai una nuova finestra di dialogo, qui seleziona la prima opzione che dice - 
     *  Controller MVC - Svuota e fai clic sul pulsante "Aggiungi".
     *  
     *      // Figura 4 : MVC-Controller-Empty-Visual-Studio
     *      
     *  Successivamente, viene visualizzata un'altra finestra di dialogo in cui è possibile 
     *  assegnare il nome al controller. Assegna un nome a questo controller - FirstController 
     *  e fai clic sul pulsante "Aggiungi".
     *  
     *      // Figura 5 : adding-controller-aspnet-core-mvc.webp
     *      
     *  Il nuovo controller viene aggiunto all'applicazione e VS lo apre per la modifica. 
     *  Per impostazione predefinita avrà il seguente codice indicato di seguito:
     *  
     *      using Microsoft.AspNetCore.Mvc;
     *      namespace FirstApp.Controllers
     *      {
     *          public class FirstController : Controller
     *          {
     *              public IActionResult Index()
     *              {
     *                  return View();
     *              }
     *          }
     *      }
     *  
     *  DOT NET 6 utilizza la versione C# 10. In C# 10 non è necessario scrivere la parentesi 
     *  dello spazio dei nomi. Si presume solo che - tutto ciò che è all'interno dello spazio 
     *  dei nomi, definito nel file, sia in realtà all'interno dello spazio dei nomi. 
     *  Questo riduce il "rumore" extra nel nostro codice e rende C# più leggibile.
     *  
     * - Metodo Action che restituisce String
     * 
     *  Il controller, che ho creato in precedenza, ha un metodo di azione che si chiama Index. 
     *  Ho intenzione di cambiare questo metodo in modo che restituisca una stringa.
     *  
     *  I controller e i metodi di azione sono parti fondamentali di ASP.NET Core. 
     *  Leggi l'articolo chiamato Metodi di azione in ASP.NET Core per capire come funziona 
     *  ciascuno di essi e come puoi estrarre il loro pieno potenziale.
     *  
     *  Quindi, cambia questo metodo di azione in:
     *  
     *      using Microsoft.AspNetCore.Mvc;
     *      namespace FirstApp.Controllers;
     *
     *      public class FirstController : Controller
     *      {
     *          public string Index()
     *          {
     *              return "Hello World";
     *          }
     *      }
     *      
     *  Ora il metodo action restituisce una stringa – 'Hello World'.
     *  
     *  Invochiamo questa azione sul browser. Eseguire l'applicazione facendo clic su 
     *  Debug ➤ Avvia debug o premere il tasto F5. Visual Studio aprirà il browser mostrando 
     *  la home page.
     *  
     *  Modificare l'URL del browser in http://localhost:59009/First/Index e premere Invio. 
     *  Vedrai - 'Hello World' sul browser come mostrato di seguito.
     *  
     *      // Figura 8 : invoking-action-browser.webp
     *      
     *  Qui 59009 è il port della mia applicazione, nel tuo caso sarà diverso. 
     *  Quindi assicurati di mantenere la porta dell'applicazione nell'URL del browser. 
     *  Il metodo di azione 'Index' è il metodo di azione predefinito impostato sul 
     *  file Program.cs (o Startup.cs se si utilizza Dot Net 5 o versioni precedenti), quindi non è 
     *  necessario specificarlo esplicitamente nell'URL del browser. 
     *  
     *  L'azione di cui sopra può essere semplicemente invocata da http://localhost:59009/First.
     *  
     *  Questo metodo di azione restituisce una stringa, pertanto non è necessario creare una vista. 
     *  In generale, la maggior parte delle volte avrai bisogno di metodi di azione che restituiscano 
     *  una "Vista". Pertanto, per queste azioni, il tipo restituito deve essere specificato come 
     *  IActionResult
     *  
     * - Metodo di azione che restituisce View
     * 
     *  Aggiungi un nuovo metodo di azione al controller e assegnagli il nome "Ciao". 
     *  Nota qui avremo il tipo di ritorno di IActionResult mentre restituisce una vista.
     *  
     *      using Microsoft.AspNetCore.Mvc;
     *      namespace FirstApp.Controllers;
     *      public class FirstController : Controller
     *      {
     *          public string Index()
     *          {
     *              return "Hello World";
     *          }
     *
     *          public IActionResult Hello()
     *          {
     *              return View();
     *          }
     *      }
     *      
     * Esegui l'applicazione (tasto F5 di scelta rapida) e quindi vai all'URL del metodo di 
     * azione http://localhost:59009/First/Hello.
     * 
     * Vedrai che l'applicazione sta cercando di trovare la vista, come mostrato nel messaggio 
     * di errore visualizzato di seguito.
     * 
     *      // Figura 9 : search-location-of-view.webp
     *      
     * L'errore indica:
     *      InvalidOperationException: The view 'Hello' was not found. The following locations were searched:
     *      /Views/First/Hello.cshtml
     *      /Views/Shared/Hello.cshtml
     *      
     * Questo è un messaggio utile che spiega l'errore si è verificato a causa dell'assenza del 
     * file View nella nostra applicazione.
     * 
     * ASP.NET viste MVC principali vengono archiviate nella cartella Viste e organizzate in 
     * sottocartelle. Le viste associate al controller denominato 'FirstController.cs' sono 
     * memorizzate nella cartella Views/First. Le viste, non specifiche di un singolo controller, 
     * vengono memorizzate all'interno della cartella denominata Views/Shared. 
     * 
     * Quindi crei sbarazzati di questo errore creando la vista "Ciao" nella cartella 
     * "Visualizzazioni / Prima" o "Visualizzazioni / Condivisa".
     * 
     * Ma c'è un problema, se metti 'Hello' View all'interno della cartella 'Views/Shared' e un 
     * altro Controller, supponiamo che AnotherController.cs abbia anche un metodo di azione con 
     * lo stesso nome - 'Hello', e questa azione non ha la sua vista specifica (cioè all'interno 
     * di Views/Another folder). Quindi, ogni volta che viene richiamata l'azione "Ciao" di 
     * qualsiasi controller ("Primo" o "Altro"), Visualizzazioni ➤ Condiviso ➤ Ciao verrà 
     * chiamato dal motore di runtime ASP.NET Core.
     * 
     * In breve, entrambi gli URL sottostanti richiameranno view – Views/Shared/Hello, 
     * a condizione che non abbiamo le visualizzazioni specifiche per le azioni:
     *      /Primo/Ciao
     *      /Altro/Ciao
     *      
     * Ora creiamo una vista specifica per il controller "Hello". Inizia facendo clic con il 
     * pulsante destro del mouse sulla cartella Visualizzazioni e seleziona Aggiungi ➤ Nuova cartella. 
     * Assegna prima un nome a questa nuova cartella.
     * 
     * Quindi, fai clic con il pulsante destro del mouse su questa cartella "Prima" e seleziona 
     * Aggiungi ➤ Nuovo elemento. Quindi, nella finestra di dialogo visualizzata, selezionare 
     * Razor View - Vuoto e fare clic sul pulsante Aggiungi.
     * 
     *      // Figura 11 : razor-view-empty.webp
     *      
     * Assegna un nome alla tua vista come "Ciao" e infine fai clic sul pulsante "Aggiungi" per 
     * creare questa vista. VS lo creerà e lo aprirà per la modifica. Aggiungere il codice 
     * seguente a questo file di visualizzazione appena creato:
     * 
     *  @{
     *      ViewData["Title"] = "Hello";
     *   }
     *  <h2>Hello World</h2>
     *
     * Ho fatto 2 cose:
     * 
     *      Aggiunta la variabile ViewData "Title" con un valore stringa "Hello". 
     *      Questo verrà impostato come titolo della pagina.
     *      
     *      Un tag H2 con Hello World che verrà mostrato sulla pagina.
     *      
     *      Ora esegui la tua applicazione e vai all'URL http://localhost:59009/First/Hello.
     *      
     *      Vedrai la vista renderizzata sul browser.
     *      
     * Nota 2 cose, il titolo della pagina sul browser è impostato come "Ciao" e la pagina mostra 
     * "Hello World" nel tag h2. La prima visualizzazione è stata creata correttamente e l'ha 
     * richiamata dal browser. 
     * 
     * Successivamente vedrai come aggiungere dati dinamici alla vista.
     * 
     * - Aggiunta di Dynamic Data alla visualizzazione
     * 
     * Il passaggio dei dati dai controller alle viste avviene in diversi modi. 
     * Ti spiegherò tutti questi modi più tardi. Per qui, farò uso di ViewBag che è un oggetto 
     * dinamico per assegnare qualsiasi tipo di valore.
     * 
     * Ora cambia il metodo di azione "Hello" per includere il codice "ViewBag" che assegna 
     * una variabile a una stringa "Hello World". Ho chiamato la variabile come 'Message' 
     * (può essere qualsiasi nome per la variabile).
     * 
     *      public IActionResult Hello()
     *      {
     *          ViewBag.Message = "Hello World";
     *          return View();
     *      }
     *      
     * L'azione memorizza la stringa sulla variabile 'ViewBag' chiamata Message. 
     * Ora devi visualizzare questa stringa memorizzata sulla vista. Quindi cambia il codice 
     * Hello.cshtml in questo:
     * 
     *      @{
     *          ViewData["Title"] = "Hello";
     *      }
     *      <h2>@ViewBag.Message</h2>
     *      
     * La modifica è all'interno del tag h2 che ora contiene @ViewBag.Message. 
     * Visualizza semplicemente il valore del "Messaggio di ViewBag" passato dal Controller.
     * 
     * Esegui nuovamente l'applicazione e vai a /First/Hello sul browser. 
     * Otterrai lo stesso risultato di prima, ma qui lo hai reso dinamico.
     * 
     * È possibile assegnare qualsiasi valore a una variabile ViewBag come string, int, class object, 
     * xml, json, ecc. La variabile ViewBag viene automaticamente distrutta da .NET Runtime una volta 
     * che il rendering della visualizzazione viene eseguito nel browser.
     * 
     * - Aggiunta del modello e trasferimento dei dati nella vista
     * 
     * In questa sezione aggiungerò un modello all'applicazione. Il modello, come tutti sappiamo, 
     * è l'elemento costitutivo di un'applicazione MVC. Riempirò questo modello dal controller e 
     * poi lo passerò alla vista, dove verrà visualizzato sul browser. Si noti che i modelli non 
     * sono altro che classi C#.
     * 
     * Aggiungerò una classe semplice alla cartella Modelli. Quindi, fai clic con il pulsante 
     * destro del mouse sulla cartella Modelli e seleziona Aggiungi ➤ Classe. 
     * Assegna alla classe il nome "Person.cs". 
     * 
     * Aggiungi le seguenti proprietà:
     * 
     *  using System;
     *      namespace FirstApp.Models;
     *
     *      public class Person
     *      {
     *          public string Name { get; set; } = String.Empty;
     *          public int Age { get; set; }
     *          public string Location { get; set; } = String.Empty;
     *      }
     *      
     * È una classe semplice che contiene 3 proprietà: Nam, Age e Location. 
     * Ora riempirò queste 3 proprietà con i valori nel controller.
     * 
     * Vai al controller "First" e aggiungi un nuovo metodo di azione con il nome "Info". 
     * Il codice del controller è simile al seguente:
     * 
     *      using FirstApp.Models;
     *      using Microsoft.AspNetCore.Mvc;
     *      namespace FirstApp.Controllers;
     *
     *      public class FirstController : Controller
     *      {
     *          // Index Action
     *
     *          // Hello Action
     *
     *          public IActionResult Info()
     *          {
     *              Person person = new Person();
     *              person.Name = "John";
     *              person.Age = 18;
     *              person.Location = "United States";
     *              return View(person);
     *          }
     *      }
     *
     * Nel metodo Info Action ho creato un nuovo oggetto per la classe Model 'Person'. 
     * Quindi assegnare la proprietà Name come 'John', la proprietà Age come 18 e la 
     * proprietà Location come 'United States' a questo oggetto. 
     * 
     * Infine, alla fine, l'oggetto 'person' viene restituito alla View (dal codice View(person)).
     * 
     * Per utilizzare la classe 'Person' nel controller 'First' è necessario importare lo 
     * spazio dei nomi Models sul controller in alto – usando FirstApp.Models
     * 
     * Ora il modello è compilato nel controller e dobbiamo mostrare i suoi dati sulla vista. 
     * Quindi crea il file di visualizzazione "Info" all'interno della cartella Views ➤ First.
     * 
     * La visualizzazione Info inizialmente avrà il codice seguente:
     *
     *      @{
     *          ViewData["Title"] = "Info";
     *      }
     *      <h2>Info</h2>
     *
     * Aggiorna la vista definendo il tipo di modello (che è la classe Person) che ottiene 
     * dal controller, in alto in questo modo:
     * 
     *      @model Person
     *      @{
     *          ViewData["Title"] = "Info";
     *      }
     *      <h2>Info</h2>
     *
     * Nota: riceverà il modello di tipo "Persona", quindi ho aggiunto "Persona" lì.
     * 
     * Infine, per visualizzare i dati del modello, aggiungere il codice seguente alla vista:
     * 
     *      @model Person
     *      @{
     *          ViewData["Title"] = "Info";
     *      }
     *      <h2>Info</h2>
     *
     *      <p>Name: @Model.Name</p>
     *      <p>Name: @Model.Age</p>
     *      <p>Name: @Model.Location</p>
     *
     * Eseguire l'applicazione e passare a /First/Info sul browser. I dati del modello verranno 
     * visualizzati come mostrato nell'immagine qui sotto.
     * 
     *      // Figura 12 : view-model-data.webp
     *      
     * È necessario notare che quando si definisce il tipo di modello nella vista, si utilizza 
     * la "m" piccola come @model Persona. Quando devi mostrare il valore delle proprietà del 
     * modello, usa la "M" maiuscola come @Model.Name.
     * 
     * 
     * Il codice sorgente completo del progetto di esempio è in ellegato a questa soluzione.:
     *      // File :  FirstAppMvc6 
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
