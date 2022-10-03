using System;

namespace DotNetDesignPatternDemos.Architectural.ModelViewController.Example
{
    /*
     *  I vantaggi dell'utilizzo del modello MVC (Model-View-Control) nei nostri progetti 
     *  di sviluppo sono che possiamo disaccoppiare completamente i nostri livelli di 
     *  applicazione aziendale e di presentazione. 
     *  
     *  Inoltre, avremo un oggetto completamente indipendente per controllare il livello 
     *  di presentazione. 
     *  
     *  L'indipendenza tra gli oggetti / livelli nel nostro progetto che l'MVC fornisce 
     *  renderà la manutenzione un po 'più facile e il riutilizzo del codice molto facile'.
     *  
     *  Come pratica generale sappiamo di voler ridurre al minimo le dipendenze degli 
     *  oggetti nei nostri progetti, in modo che le modifiche siano facili e possiamo 
     *  riutilizzare il codice su cui abbiamo lavorato così duramente. 
     *  
     *  Per raggiungere questo obiettivo seguiremo un principio generale di 
     *  "programmazione all'interfaccia, non alla classe" usando il modello MVC.
     *  
     *  Si sono create le tre parti dell'MVC: il modello, il controllo e la vista. 
     *  Nel nostro sistema, il modello sarebbe la nostra auto, il View sarebbe 
     *  l'interfaccia utente e il control è ciò che lega i due insieme.
     *  
     *      //MVC1.jpg
     *      
     *  Per apportare modifiche al modello (la nostra auto sportiva ACME 2000), utilizzeremo 
     *  il nostro controllo. Il nostro controllo farà la richiesta al modello 
     *  (la nostra auto sportiva ACME 2000) e aggiornerà la nostra vista, che è la nostra 
     *  interfaccia utente (UI).
     *  
     *  Sembra abbastanza semplice, ma ecco il primo problema che dobbiamo risolvere: 
     *  cosa succede quando l'utente finale vuole apportare una modifica alla nostra auto 
     *  sportiva ACME 2000, come andare più veloce o girare? 
     *  Dovranno farlo richiedendo una modifica utilizzando il controllo, attraverso la 
     *  vista (il nostro modulo windows).
     *  
     *      //MVC2.jpg
     *  
     *  Ora ci rimane un ultimo problema da risolvere. Cosa succede se la vista non dispone 
     *  delle informazioni necessarie per visualizzare lo stato corrente del modello? 
     *  Dovremo aggiungere un'altra freccia al nostro diagramma: 
     *      la vista sarà in grado di richiedere lo stato del modello per ottenere ciò di 
     *      cui ha bisogno per visualizzare le informazioni sullo stato del modello.
     *  
     *      //MVC3.jpg
     *  
     *  Infine, il nostro utente finale (il nostro autista) interagirà con il nostro intero 
     *  sistema di controllo del veicolo ACME attraverso la vista. Se si desidera richiedere 
     *  una modifica al sistema, ad esempio aggiungendo un po ' di accelerazione, la richiesta 
     *  verrà avviata dalla vista e gestita dal controllo.
     *  
     *  L'ontrol C chiederà quindi al modello di modificare e apportare le modifiche necessarie 
     *  alla vista. Ad esempio, se l'auto sportiva ACME 2000 ha una richiesta "floor it" 
     *  da parte di un guidatore indisciplinato e ora sta viaggiando velocemente per fare 
     *  una svolta, il controllo saprà disabilitare la possibilità di girare nella vista, 
     *  prevenendo così un accumulo catastrofico nel mezzo dell'ora di punta (whew!).
     *  
     *  Il modello (l'auto sportiva ACME 2000) notificherà alla vista che la sua velocità è 
     *  aumentata e la vista verrà aggiornata se appropriato.
     *  
     *  Quindi, dopo tutto questo, ecco la panoramica di ciò che costruiremo:
     *  
     *      //MVC4.gif
     *  
     *  Essendo sviluppatori che pensano sempre al futuro, vogliamo essere sicuri che il nostro 
     *  sistema avrà una vita lunga e prospera. Ciò significa essere preparati per il maggior 
     *  numero possibile di cambiamenti in ACME. Per fare questo sappiamo di seguire 
     *  due regole d'oro... 
     *      "mantieni le tue classi liberamente accoppiate" e, per raggiungere questo obiettivo... 
     *      "programma all'interfaccia".
     *      
     * Quindi faremo tre interfacce (come avrai intuito, una per il modello, una per la vista e 
     * una per il controllo). Dopo molte ricerche e laboriose interviste con la gente di ACME, 
     * scopriamo di più sulle specifiche del sistema. 
     * 
     * Vogliamo essere sicuri di poter impostare le velocità massime per viaggiare avanti, 
     * indietro e girare. Dobbiamo anche essere in grado di accelerare, rallentare e girare 
     * a sinistra ea destra. La nostra vista "dashboard" deve mostrare la velocità e la 
     * direzione correnti.
     * 
     * È un compito arduo implementare tutti questi requisiti, ma siamo sicuri di poterlo 
     * gestire ...
     * 
     * Per prima cosa, occupiamoci di alcuni elementi preliminari. 
     * Avremo bisogno di qualcosa per rappresentare la direzione e girare le richieste. 
     * Creeremo due enumerabili, AbsoluteDirection e RelativeDirection.
     * 
     * code 1
     * 
     * Successivamente, affrontiamo l'interfaccia di controllo. 
     * Sappiamo che il controllo deve passare le richieste al modello, in particolare: 
     * accelerare, decelerare e girare. Creeremo un'interfaccia IVehicleControl con i 
     * metodi appropriati.
     * 
     * code 2
     * 
     * Ora metteremo insieme l'interfaccia del modello. 
     * Abbiamo bisogno di conoscere il nome del veicolo, la velocità, la velocità massima, 
     * la velocità massima di retromarcia, la velocità massima di svolta e la direzione. 
     * Abbiamo anche bisogno di metodi per accelerare, decelerare e girare.
     * 
     * code 3
     * 
     * E infine, metteremo insieme l'interfaccia Visualizza. 
     * Sappiamo che la vista dovrebbe esporre alcune funzionalità al controllo, come 
     * l'abilitazione e la disabilitazione delle richieste di accelerazione, decelerazione 
     * e rotazione.
     * 
     * code 4
     * 
     * Ora dobbiamo apportare alcune modifiche alle nostre interfacce per consentire loro 
     * di interagire. Prima di tutto, qualsiasi controllo dovrebbe essere a conoscenza della 
     * sua vista e modello, quindi aggiungeremo i metodi "SetModel" e "SetView" alla nostra 
     * interfaccia IvehicleControl:
     * 
     * code 5
     * 
     * La parte successiva è un po 'complicata. 
     * Vogliamo che la vista sia a conoscenza delle modifiche apportate al modello. 
     * Per fare questo useremo un modello di progettazione GOF "Observer".
     * Per implementare il modello Observer, è necessario aggiungere i seguenti metodi al 
     * modello (che verranno "osservati" dalla vista): 
     * AddObserver, RemoveObserver e NotifyObservers.
     * 
     * code 6
     * 
     * ... e aggiungere il seguente metodo alla Vista (che "osserverà" il Modello). 
     * Ciò che accadrà è che il modello avrà un riferimento alla vista. 
     * Quando il modello cambia, chiamerà il metodo NotifyObservers() e passerà un 
     * riferimento a se stesso e notificherà alla vista una modifica chiamando il 
     * metodo Update() della vista. (Diventerà chiaro come fango quando cableremo tutto più tardi).
     * 
     * code 7
     * 
     * Quindi ora abbiamo le nostre interfacce messe insieme. Useremo solo riferimenti a queste 
     * interfacce nel resto del nostro codice per assicurarci di avere un accoppiamento allentato 
     * (che sappiamo essere una buona cosa). Qualsiasi interfaccia utente che mostri lo stato 
     * di un veicolo implementerà IVehicleView, tutte le nostre automobili ACME implementeranno 
     * IVehicleModel e faremo controlli per le nostre automobili ACME con controlli del veicolo 
     * ACME che implementeranno IVehicleControl.
     * 
     * Prossimo... Quali cose avranno cose in comune?
     * 
     * Sappiamo che tutti i nostri veicoli dovrebbero agire allo stesso modo, quindi creeremo 
     * uno "scheletro" di base di codice comune per gestire il loro funzionamento. 
     * Questa sarà una classe astratta perché non vogliamo che nessuno guidi intorno a uno 
     * scheletro (non puoi fare un'istanza di una classe astratta). 
     * La chiameremo Automobile. Useremo un ArrayList (da System.Collections) per tenere traccia 
     * di tutte le viste interessate (ricordate il modello Observer?). 
     * Avremmo potuto usare una semplice vecchia serie di riferimenti iVehicleView, ma a 
     * questo punto siamo tutti stanchi e vogliamo superare questo articolo. 
     * Se sei interessato, dai un'occhiata all'implementazione dei metodi 
     * AddObserver, RemoveObserver e NotifyObservers per avere un'idea di come funziona 
     * il modello Observer aiutando il nostro IVehicleModel a interagire con IVehicleView. 
     * 
     * Ogni volta che c'è un cambiamento di velocità o direzione, l'automobile avvisa tutte 
     * le IVehicleViews
     * 
     * code 8
     * 
     * Ultimo ma non meno importante...
     * 
     * Ora che il nostro "ACME Framework" è in atto, non ci resta che impostare le nostre 
     * classi concrete e la nostra interfaccia. Occupiamoci prima delle ultime due classi 
     * che saranno il nostro Controllo e il nostro Modello...
     * 
     * Ecco il nostro concreto AutomobileControl che implementa l'interfaccia IVehicleControl. 
     * Il nostro AutomobileControl imposterà anche la vista in base allo stato del modello 
     * (controlla il metodo SetView che viene chiamato ogni volta che viene passata una 
     * richiesta al modello).
     * 
     * Nota, abbiamo solo riferimenti a IVehicleModel (non alla classe astratta Automobile) 
     * per mantenere le cose sciolte e IVehicleView (non una vista specifica).
     * 
     * code 9
     * 
     * Ecco la nostra classe ACME200SportsCar (che estende la classe astratta Automobile 
     * che implementa l'interfaccia IVehicleModel):
     * 
     * code 10
     * 
     * E ora per la nostra vista...
     * 
     * Ora dobbiamo creare l'ultimo dei nostri tre componenti ACME MVC... la vista!
     * Creeremo un controllo utente AutoView e lo faremo implementare l'interfaccia 
     * IVehicleView. 
     * Il componente AutoView avrà riferimenti alle interfacce di controllo e di modello:
     * 
     * code 11
     * 
     * Abbiamo anche bisogno di cablare tutto nel costruttore per UserControl.
     * 
     * code 12
     * 
     * Successivamente, aggiungeremo i nostri pulsanti, un'etichetta per visualizzare 
     * lo stato dell'auto sportiva ACME2000 e una barra di stato solo per i calci e 
     * compileremo il codice per tutti i pulsanti.
     * 
     *  //MVC5.jpg
     *  
     * code 12
     * 
     * Aggiungi un metodo per aggiornare l'interfaccia...
     * 
     * code 13
     * 
     * Infine, collegheremo i metodi dell'interfaccia IVehicleView ...
     * 
     * code 14
     * 
     * Ora possiamo fare un test drive nell'auto sportiva ACME2000. 
     * Tutto sta andando come previsto e poi ci imbattiamo in un dirigente ACME che vuole 
     * guidare un pick-up invece di un'auto sportiva.
     * 
     * Meno male che abbiamo usato l'MVC! Tutto quello che dobbiamo fare è creare una 
     * nuova classe ACMETruck, collegarla e siamo in attività!
     * 
     * code 15
     * 
     * in AutoView, non ci resta che costruire il camion e cablarlo!
     * 
     * code 16
     * 
     * Se volessimo un nuovo controllo che ci permettesse solo di aumentare o diminuire la 
     * velocità di un massimo di 5 mph, è un gioco da ragazzi! Crea uno SlowPokeControl 
     * (come il nostro AutoControl, ma con limiti su quanto verrà richiesto di accelerare 
     * un Modello)
     * 
     * code 17
     * 
     * Se vogliamo rendere il nostro camion ACME2000 uno SlowPoke, 
     * lo colleghiamo semplicemente in AutoView!
     * 
     * code 18
     *
     * code 19
     * E infine, se volessimo un'interfaccia abilitata per il web, tutto ciò che dobbiamo fare è creare 
     * un progetto web e su UserControl implementare l'interfaccia IVehicleView!
     * 
     * 
     *  Per vedere l'esempio completo scomprimere il codice zip in allegato a questa cartella.
     * 
     */

    /*
    // Code 1
    public enum AbsoluteDirection
    {
        North = 0, East, South, West
    }
    public enum RelativeDirection
    {
        Right,
        Left,
        Back
    }

    // Code 2
    public interface IVehicleControl
    {
        void Accelerate(int paramAmount);
        void Decelerate(int paramAmount);
        void Turn(RelativeDirection paramDirection);
    }

    // Code 3
    public interface IVehicleModel
    {
        string Name { get; set; }
        int Speed { get; set; }
        int MaxSpeed { get; }
        int MaxTurnSpeed { get; }
        int MaxReverseSpeed { get; }
        AbsoluteDirection Direction { get; set; }
        void Turn(RelativeDirection paramDirection);
        void Accelerate(int paramAmount);
        void Decelerate(int paramAmount);
    }

    // Code 4
    public class IVehicleView
    {
        void DisableAcceleration();
        void EnableAcceleration();
        void DisableDeceleration();
        void EnableDeceleration();
        void DisableTurning();
        void EnableTurning();
    }

    // Code 5
    public interface IVehicleControl
    {
        void RequestAccelerate(int paramAmount);
        void RequestDecelerate(int paramAmount);
        void RequestTurn(RelativeDirection paramDirection);
        void SetModel(IVehicleModel paramAuto);
        void SetView(IVehicleView paramView);
    }

    // Code 6
    public interface IVehicleModel
    {
        string Name { get; set; }
        int Speed { get; set; }
        int MaxSpeed { get; }
        int MaxTurnSpeed { get; }
        int MaxReverseSpeed { get; }
        AbsoluteDirection Direction { get; set; }
        void Turn(RelativeDirection paramDirection);
        void Accelerate(int paramAmount);
        void Decelerate(int paramAmount);
        void AddObserver(IVehicleView paramView);
        void RemoveObserver(IVehicleView paramView);
        void NotifyObservers();
    }

    // Code 7
    public class IVehicleView
    {
        void DisableAcceleration();
        void EnableAcceleration();
        void DisableDeceleration();
        void EnableDeceleration();
        void DisableTurning();
        void EnableTurning();
        void Update(IVehicleModel paramModel);
    }

    // Code 8
    public abstract class Automobile : IVehicleModel
    {
        #region "Declarations "  
        private ArrayList aList = new ArrayList();
        private int mintSpeed = 0;
        private int mintMaxSpeed = 0;
        private int mintMaxTurnSpeed = 0;
        private int mintMaxReverseSpeed = 0;
        private AbsoluteDirection mDirection = AbsoluteDirection.North;
        private string mstrName = "";
        #endregion
        #region "Constructor"  
        public Automobile(int paramMaxSpeed, int paramMaxTurnSpeed, int paramMaxReverseSpeed, string paramName)
        {
            this.mintMaxSpeed = paramMaxSpeed;
            this.mintMaxTurnSpeed = paramMaxTurnSpeed;
            this.mintMaxReverseSpeed = paramMaxReverseSpeed;
            this.mstrName = paramName;
        }
        #endregion
        #region "IVehicleModel Members"  
        public void AddObserver(IVehicleView paramView)
        {
            aList.Add(paramView);
        }
        public void RemoveObserver(IVehicleView paramView)
        {
            aList.Remove(paramView);
        }
        public void NotifyObservers()
        {
            foreach (IVehicleView view in aList)
            {
                view.Update(this);
            }
        }
        public string Name
        {
            get
            {
                return this.mstrName;
            }
            set
            {
                this.mstrName = value;
            }
        }
        public int Speed
        {
            get
            {
                return this.mintSpeed;
            }
        }
        public int MaxSpeed
        {
            get
            {
                return this.mintMaxSpeed;
            }
        }
        public int MaxTurnSpeed
        {
            get
            {
                return this.mintMaxTurnSpeed;
            }
        }
        public int MaxReverseSpeed
        {
            get
            {
                return this.mintMaxReverseSpeed;
            }
        }
        public AbsoluteDirection Direction
        {
            get
            {
                return this.mDirection;
            }
        }
        public void Turn(RelativeDirection paramDirection)
        {
            AbsoluteDirection newDirection;
            switch (paramDirection)
            {
                case RelativeDirection.Right:
                    newDirection = (AbsoluteDirection)((int)(this.mDirection + 1) % 4);
                    break;
                case RelativeDirection.Left:
                    newDirection = (AbsoluteDirection)((int)(this.mDirection + 3) % 4);
                    break;
                case RelativeDirection.Back:
                    newDirection = (AbsoluteDirection)((int)(this.mDirection + 2) % 4);
                    break;
                default:
                    newDirection = AbsoluteDirection.North;
                    break;
            }
            this.mDirection = newDirection;
            this.NotifyObservers();
        }
        public void Accelerate(int paramAmount)
        {
            this.mintSpeed += paramAmount;
            if (mintSpeed >= this.mintMaxSpeed) mintSpeed = mintMaxSpeed;
            this.NotifyObservers();
        }
        public void Decelerate(int paramAmount)
        {
            this.mintSpeed -= paramAmount;
            if (mintSpeed <= this.mintMaxReverseSpeed) mintSpeed = mintMaxReverseSpeed;
            this.NotifyObservers();
        }
        #endregion
    }

    // Code 9
    public class AutomobileControl : IVehicleControl
    {
        private IVehicleModel Model;
        private IVehicleView View;
        public AutomobileControl(IVehicleModel paramModel, IVehicleView paramView)
        {
            this.Model = paramModel;
            this.View = paramView;
        }
        public AutomobileControl()
        {
        }
        #region IVehicleControl Members  
        public void SetModel(IVehicleModel paramModel)
        {
            this.Model = paramModel;
        }
        public void SetView(IVehicleView paramView)
        {
            this.View = paramView;
        }
        public void RequestAccelerate(int paramAmount)
        {
            if (Model != null)
            {
                Model.Accelerate(paramAmount);
                if (View != null) SetView();
            }
        }
        public void RequestDecelerate(int paramAmount)
        {
            if (Model != null)
            {
                Model.Decelerate(paramAmount);
                if (View != null) SetView();
            }
        }
        public void RequestTurn(RelativeDirection paramDirection)
        {
            if (Model != null)
            {
                Model.Turn(paramDirection);
                if (View != null) SetView();
            }
        }
        #endregion
        public void SetView()
        {
            if (Model.Speed >= Model.MaxSpeed)
            {
                View.DisableAcceleration();
                View.EnableDeceleration();
            }
            else if (Model.Speed <= Model.MaxReverseSpeed)
            {
                View.DisableDeceleration();
                View.EnableAcceleration();
            }
            else
            {
                View.EnableAcceleration();
                View.EnableDeceleration();
            }
            if (Model.Speed >= Model.MaxTurnSpeed)
            {
                View.DisableTurning();
            }
            else
            {
                View.EnableTurning();
            }
        }
    }

    // Code 10
    public class ACME2000SportsCar : Automobile
    {
        public ACME2000SportsCar(string paramName) : base(250, 40, -20, paramName) { }
        public ACME2000SportsCar(string paramName, int paramMaxSpeed, int paramMaxTurnSpeed, int paramMaxReverseSpeed) :
        base(paramMaxSpeed, paramMaxTurnSpeed, paramMaxReverseSpeed, paramName)
        { }
    }

    // Code 11
    public class AutoView : System.Windows.Forms.UserControl, IVehicleView
    {
        private IVehicleControl Control = new ACME.AutomobileControl();
        private IVehicleModel Model = new ACME.ACME2000SportsCar("Speedy");
    }

    // Code 12
    public AutoView()
    {
        // This call is required by the Windows.Forms Form Designer.  
        InitializeComponent();
        WireUp(Control, Model);
    }
    public void WireUp(IVehicleControl paramControl, IVehicleModel paramModel)
    {
        // If we're switching Models, don't keep watching  
        // the old one!   
        if (Model != null)
        {
            Model.RemoveObserver(this);
        }
        Model = paramModel;
        Control = paramControl;
        Control.SetModel(Model);
        Control.SetView(this);
        Model.AddObserver(this);
    }

    // Code 13
    private void btnAccelerate_Click(object sender, System.EventArgs e)
    {
        Control.RequestAccelerate(int.Parse(this.txtAmount.Text));
    }
    private void btnDecelerate_Click(object sender, System.EventArgs e)
    {
        Control.RequestDecelerate(int.Parse(this.txtAmount.Text));
    }
    private void btnLeft_Click(object sender, System.EventArgs e)
    {
        Control.RequestTurn(RelativeDirection.Left);
    }
    private void btnRight_Click(object sender, System.EventArgs e)
    {
        Control.RequestTurn(RelativeDirection.Right);
    }

    // Code 14
    public void UpdateInterface(IVehicleModel auto)
    {
        this.label1.Text = auto.Name + " heading " + auto.Direction.ToString() + " at speed: " + auto.Speed.ToString();
        this.pBar.Value = (auto.Speed > 0) ? auto.Speed * 100 / auto.MaxSpeed : auto.Speed * 100 / auto.MaxReverseSpeed;
    }

    // Code 15
    public void DisableAcceleration()
    {
        this.btnAccelerate.Enabled = false;
    }
    public void EnableAcceleration()
    {
        this.btnAccelerate.Enabled = true;
    }
    public void DisableDeceleration()
    {
        this.btnDecelerate.Enabled = false;
    }
    public void EnableDeceleration()
    {
        this.btnDecelerate.Enabled = true;
    }
    public void DisableTurning()
    {
        this.btnRight.Enabled = this.btnLeft.Enabled = false;
    }
    public void EnableTurning()
    {
        this.btnRight.Enabled = this.btnLeft.Enabled = true;
    }
    public void Update(IVehicleModel paramModel)
    {
        this.UpdateInterface(paramModel);
    }

    // Code 16
    public class ACME2000Truck : Automobile
    {
        public ACME2000Truck(string paramName) : base(80, 25, -12, paramName) { }
        public ACME2000Truck(string paramName, int paramMaxSpeed, int paramMaxTurnSpeed, int paramMaxReverseSpeed) :
        base(paramMaxSpeed, paramMaxTurnSpeed, paramMaxReverseSpeed, paramName)
        { }
    }

    // Code 17
    private void btnBuildNew_Click(object sender, System.EventArgs e)
    {
        this.autoView1.WireUp(new ACME.AutomobileControl(), new ACME.ACME2000Truck(this.txtName.Text));
    }

    // Code 18
    public void RequestAccelerate(int paramAmount)
    {
        if (Model != null)
        {
            int amount = paramAmount;
            if (amount > 5) amount = 5;
            Model.Accelerate(amount);
            if (View != null) SetView();
        }
    }
    public void RequestDecelerate(int paramAmount)
    {
        if (Model != null)
        {
            int amount = paramAmount;
            if (amount > 5) amount = 5;
            Model.Accelerate(amount);
            Model.Decelerate(amount);
            if (View != null) SetView();
        }
    }

    // Code 19
    private void btnBuildNew_Click(object sender, System.EventArgs e)
    {
        this.autoView1.WireUp(new ACME.SlowPokeControl(), new ACME.ACME2000Truck(this.txtName.Text));
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
