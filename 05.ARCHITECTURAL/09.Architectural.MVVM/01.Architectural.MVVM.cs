using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;

namespace DotNetDesignPatternDemos.Architectural.ModelViewViewModel
{
    /*
     * MVVM è una variante del modello di progettazione del modello di presentazione 
     * di Martin Fowler. [2][3] 
     * È stato inventato dagli architetti Microsoft Ken Cooper e Ted Peters appositamente 
     * per semplificare la programmazione basata su eventi delle interfacce utente. 
     * Il modello è stato incorporato in Windows Presentation Foundation (WPF) 
     * (il sistema grafico .NET di Microsoft) e Silverlight (derivato dall'applicazione 
     * Internet di WPF). John Gossman, uno degli architetti WPF e Silverlight di Microsoft, 
     * ha annunciato MVVM sul suo blog nel 2005. 
     * 
     * Model–view–viewmodel viene anche definito model–view–binder, soprattutto nelle 
     * implementazioni che non coinvolgono la piattaforma .NET. ZK 
     * (un framework di applicazioni web scritto in Java) e KnockoutJS (una libreria JavaScript) 
     * usano model-view-binder
     *
     * MVVM è stato progettato per rimuovere praticamente tutto il codice GUI ("code-behind") 
     * dal livello di visualizzazione, utilizzando le funzioni di associazione dati in WPF 
     * (Windows Presentation Foundation) per facilitare meglio la separazione dello sviluppo 
     * del livello di visualizzazione dal resto del modello. 
     * Invece di richiedere agli sviluppatori di esperienza utente (UX) di scrivere codice GUI, 
     * possono utilizzare il linguaggio di markup del framework (ad esempio XAML) e creare 
     * associazioni dati al modello di visualizzazione, che viene scritto e gestito dagli 
     * sviluppatori di applicazioni. La separazione dei ruoli consente ai progettisti 
     * interattivi di concentrarsi sulle esigenze di UX piuttosto che sulla programmazione 
     * della logica di business. I livelli di un'applicazione possono quindi essere sviluppati 
     * in più flussi di lavoro per una maggiore produttività. Anche quando un singolo sviluppatore 
     * lavora sull'intera base di codice, una corretta separazione della visualizzazione dal 
     * modello è più produttiva, poiché l'interfaccia utente in genere cambia frequentemente e 
     * in ritardo nel ciclo di sviluppo in base al feedback dell'utente finale. 
     * 
     * Il modello MVVM tenta di ottenere entrambi i vantaggi della separazione dello sviluppo 
     * funzionale fornito da MVC, sfruttando al contempo i vantaggi delle associazioni dati e 
     * del framework associando i dati il più vicino possibile al modello di applicazione pura. 
     * Utilizza il raccoglitore, il modello di visualizzazione e le funzionalità di controllo 
     * dei dati di qualsiasi livello aziendale per convalidare i dati in ingresso. Il risultato 
     * è che il modello e il framework guidano il più possibile le operazioni, eliminando o 
     * riducendo al minimo la logica dell'applicazione che manipola direttamente la vista 
     * (ad esempio, code-behind).
     * 
     */

    /*
     * Componenti
     * 
     * Modello
     *      Il modello si riferisce a un modello di dominio, che rappresenta il contenuto dello stato 
     *      reale (un approccio orientato agli oggetti), o al livello di accesso ai dati, che 
     *      rappresenta il contenuto (un approccio incentrato sui dati). 
     * Vista
     *      Come nei modelli model-view-controller (MVC) e model-view-presenter (MVP), 
     *      la visualizzazione è la struttura, il layout e l'aspetto di ciò che un utente vede 
     *      sullo schermo. Visualizza una rappresentazione del modello e riceve l'interazione 
     *      dell'utente con la vista (clic del mouse, input da tastiera, gesti di tocco dello schermo, 
     *      ecc.) e inoltra la gestione di questi al modello di visualizzazione tramite l'associazione 
     *      dati (proprietà, callback di eventi, ecc.) definita per collegare la vista e il modello 
     *      di visualizzazione.
     * Visualizza modello
     *      Il modello di visualizzazione è un'astrazione della vista che espone proprietà e comandi 
     *      pubblici. Invece del controller del modello MVC o del presentatore del modello MVP, MVVM 
     *      dispone di un raccoglitore che automatizza la comunicazione tra la visualizzazione e le 
     *      relative proprietà associate nel modello di visualizzazione. Il modello di visualizzazione 
     *      è stato descritto come uno stato dei dati nel modello. 
     *      La differenza principale tra il modello di visualizzazione e il relatore nel modello MVP 
     *      è che il relatore ha un riferimento a una visualizzazione, mentre il modello di 
     *      visualizzazione non lo fa. Al contrario, una visualizzazione viene associata direttamente 
     *      alle proprietà del modello di visualizzazione per inviare e ricevere aggiornamenti. 
     *      Per funzionare in modo efficiente, ciò richiede una tecnologia di associazione o la 
     *      generazione di codice boilerplate per eseguire l'associazione. 
     * Raccoglitore
     *      I dati dichiarativi e l'associazione dei comandi sono impliciti nel modello MVVM. 
     *      Nello stack di soluzioni Microsoft, il Raccoglitore è un linguaggio di markup denominato 
     *      XAML. Il raccoglitore libera lo sviluppatore dall'obbligo di scrivere la logica 
     *      boiler-plate per sincronizzare il modello di visualizzazione e la vista. 
     *      Se implementato al di fuori dello stack Microsoft, la presenza di una tecnologia di 
     *      associazione dati dichiarativa è ciò che rende possibile questo modello, e senza un 
     *      raccoglitore, in genere si userebbe MVP o MVC invece e si dovrebbe scrivere più 
     *      boilerplate (o generarlo con qualche altro strumento).
     */

    // Class Model
    public class User : INotifyPropertyChanged
    {
        private int userId;
        private string firstName;
        private string lastName;
        private string city;
        private string state;
        private string country;
        public int UserId
        {
            get
            {
                return userId;
            }
            set
            {
                userId = value;
                OnPropertyChanged("UserId");
            }
        }
        public string FirstName
        {
            get
            {
                return firstName;
            }
            set
            {
                firstName = value;
                OnPropertyChanged("FirstName");
            }
        }
        public string LastName
        {
            get
            {
                return lastName;
            }
            set
            {
                lastName = value;
                OnPropertyChanged("LastName");
            }
        }
        public string City
        {
            get
            {
                return city;
            }
            set
            {
                city = value;
                OnPropertyChanged("City");
            }
        }
        public string State
        {
            get
            {
                return state;
            }
            set
            {
                state = value;
                OnPropertyChanged("State");
            }
        }
        public string Country
        {
            get
            {
                return country;
            }
            set
            {
                country = value;
                OnPropertyChanged("Country");
            }
        }

        #region INotifyPropertyChanged Members  

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

    }


    // Claas View
    class UserViewModel
    {
        private IList<User> _UsersList;

        public UserViewModel()
        {
            _UsersList = new List<User>
            {
                new User{UserId = 1,FirstName="Raj",LastName="Beniwal",City="Delhi",State="DEL",Country="INDIA"},
                new User{UserId=2,FirstName="Mark",LastName="henry",City="New York", State="NY", Country="USA"},
                new User{UserId=3,FirstName="Mahesh",LastName="Chand",City="Philadelphia", State="PHL", Country="USA"},
                new User{UserId=4,FirstName="Vikash",LastName="Nanda",City="Noida", State="UP", Country="INDIA"},
                new User{UserId=5,FirstName="Harsh",LastName="Kumar",City="Ghaziabad", State="UP", Country="INDIA"},
                new User{UserId=6,FirstName="Reetesh",LastName="Tomar",City="Mumbai", State="MP", Country="INDIA"},
                new User{UserId=7,FirstName="Deven",LastName="Verma",City="Palwal", State="HP", Country="INDIA"},
                new User{UserId=8,FirstName="Ravi",LastName="Taneja",City="Delhi", State="DEL", Country="INDIA"}
            };
        }

        public IList<User> Users
        {
            get { return _UsersList; }
            set { _UsersList = value; }
        }

        private ICommand mUpdater;
        public ICommand UpdateCommand
        {
            get
            {
                if (mUpdater == null)
                    mUpdater = new Updater();
                return mUpdater;
            }
            set
            {
                mUpdater = value;
            }
        }

        private class Updater : ICommand
        {
            #region ICommand Members  

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public event EventHandler CanExecuteChanged;

            public void Execute(object parameter)
            {

            }

            #endregion
        }
    }

    // ViewXaml.xaml XAML da impostare per l'esempio
    /*
    <Window x:Class="WpfMVVMSample.MainWindow"  
            xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"  
            xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"  
            Title="MainWindow" Height="485" Width="525">  
        <Grid Margin="0,0,0,20">  
            <Grid.RowDefinitions>  
                <RowDefinition Height="Auto"/>  
                <RowDefinition Height="*"/>  
                <RowDefinition Height="Auto"/>  
            </Grid.RowDefinitions>  
            <ListView Name="UserGrid" Grid.Row="1" Margin="4,178,12,13"  ItemsSource="{Binding Users}"  >  
                <ListView.View>  
                    <GridView x:Name="grdTest">  
                        <GridViewColumn Header="UserId" DisplayMemberBinding="{Binding UserId}"  Width="50"/>  
                        <GridViewColumn Header="First Name" DisplayMemberBinding="{Binding FirstName}"  Width="80" />  
                        <GridViewColumn Header="Last Name" DisplayMemberBinding="{Binding LastName}" Width="100" />  
                        <GridViewColumn Header="City" DisplayMemberBinding="{Binding City}" Width="80" />  
                        <GridViewColumn Header="State" DisplayMemberBinding="{Binding State}" Width="80" />  
                        <GridViewColumn Header="Country" DisplayMemberBinding="{Binding Country}" Width="100" />  
                    </GridView>  
                </ListView.View>  
            </ListView>  
            <TextBox Grid.Row="1" Height="23" HorizontalAlignment="Left" Margin="80,7,0,0" Name="txtUserId" VerticalAlignment="Top" Width="178" Text="{Binding ElementName=UserGrid,Path=SelectedItem.UserId}" />  
            <TextBox Grid.Row="1" Height="23" HorizontalAlignment="Left" Margin="80,35,0,0" Name="txtFirstName" VerticalAlignment="Top" Width="178" Text="{Binding ElementName=UserGrid,Path=SelectedItem.FirstName}" />  
            <TextBox Grid.Row="1" Height="23" HorizontalAlignment="Left" Margin="80,62,0,0" Name="txtLastName" VerticalAlignment="Top" Width="178" Text="{Binding ElementName=UserGrid,Path=SelectedItem.LastName}" />  
            <Label Content="UserId" Grid.Row="1" HorizontalAlignment="Left" Margin="12,12,0,274" Name="label1" />  
            <Label Content="Last Name" Grid.Row="1" Height="28" HorizontalAlignment="Left" Margin="12,60,0,0" Name="label2" VerticalAlignment="Top" />  
            <Label Content="First Name" Grid.Row="1" Height="28" HorizontalAlignment="Left" Margin="12,35,0,0" Name="label3" VerticalAlignment="Top" />  
            <Button Content="Update" Grid.Row="1" Height="23" HorizontalAlignment="Left" Margin="310,40,0,0" Name="btnUpdate"   
                    VerticalAlignment="Top" Width="141"  
                    Command="{Binding Path=UpdateCommad}"  />  
            <TextBox Grid.Row="1" Height="23" HorizontalAlignment="Left" Margin="80,143,0,0" x:Name="txtCity" VerticalAlignment="Top" Width="178" Text="{Binding SelectedItem.City, ElementName=UserGrid}" />  
            <Label Content="Country" Grid.Row="1" Height="28" HorizontalAlignment="Left" Margin="12,141,0,0" x:Name="label2_Copy" VerticalAlignment="Top" />  
            <TextBox Grid.Row="1" Height="23" HorizontalAlignment="Left" Margin="80,88,0,0" x:Name="txtCountry" VerticalAlignment="Top" Width="178" Text="{Binding SelectedItem.Country, ElementName=UserGrid}" />  
            <Label Content="City" Grid.Row="1" Height="28" HorizontalAlignment="Left" Margin="12,86,0,0" x:Name="label2_Copy1" VerticalAlignment="Top" />  
            <TextBox Grid.Row="1" Height="23" HorizontalAlignment="Left" Margin="80,115,0,0" x:Name="txtSTate" VerticalAlignment="Top" Width="178" Text="{Binding SelectedItem.State, ElementName=UserGrid}" />  
            <Label Content="State" Grid.Row="1" Height="28" HorizontalAlignment="Left" Margin="12,113,0,0" x:Name="label2_Copy2" VerticalAlignment="Top" />  
        </Grid>  
    </Window>  
    */

    // Bind di esempio per la ViewXaml.xaml.cs
    /*
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        WpfMVVMSample.MainWindow window = new MainWindow();
        UserViewModel VM = new UserViewModel();
        window.DataContext = VM;
        window.Show();
    }
    */

    class Program
    {
        // Il risultato se compilato con lo xaml separto etc.
        // l'anterpima del risultato visibile nell'immagine in allegato.

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
