using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace DotNetDesignPatternDemos.Structural.Proxy.ViewModel
{
    /*
     * A volte è necessario collegare un oggetto alle interfacce utente
     * e potrebbe essere necessario avere una funzionalità aggiuntiva
     * rispetto a ciò che gli oggetti forniscono di suo.
     * Quindi ad esempio si potrebbe avere la necessità di avere notifiche
     * o convalidare proprietà. Quindi questo si traduce nel dire come posso
     * fare questo rispettando la Separaione delle Preoccupazioni sul principio SOLID?.
     * In questo esempio è riportato un tipico oggetto di dominio Person con i propri
     * Membri che normalmente sono associati a una tabella di dati di un tipico DB. 
     * All'ìinizio si potrebbe pensare di implementare direttamente dentro questa classe
     * il gestore di notifiche e gli errori verso l'interfaccia ad esempio con
     * public class Person :INotifyPropertYChanged, IDataError
     * e se qualcosa succede ti ritrovi il modello di dominio che fà tutto questo, ma ti
     * allontani con tutti i vantaggi dal concetto di Separazione e Preoccupazione SOLID.
     * Piuttosto ci concentriamo a separare il gestore di notifiche in una classe apposita
     * così come il gestore di errori. Questi componenti solitamente chiamati ViewModel
     * che fanno appunto parte del mondo MVVM che è un pattern per la gestione delle 
     * interfacce separate dai dati usato nei maggiori framework dispinibili. Questo tipo
     * di approccio rende tre tipologie di classi da gestire il Modello appunto la classe Person
     * la View che si occupa di interagire con l'interfaccia utente UI ed è qui che ci occupiamo
     * di aggiungere notifiche per la pmodifica degli attributi che ha il Model olte al fatto che
     * è possibile aggiungere funzionalità aggiuntive come ad esempio cambiare contemporaneamente
     * da un nome completo gli attributi del model. Nell'esempio è riportato una View completa 
     * di questa funzionalità che prende in Input il nome completo e lo separa nelle parti al
     * momento di aggiornare il Model per Nome e Cognome. Riassumendo questo modo di approcciare
     * alla UI tramite il paradigma di MVVM fa sì che venga riportato un Proxy che è la View che
     * si prende in pancia l'input e lavori verso il model trattando delle operazioni in entrata
     * operando sull'interfaccia e posizionandole in uscita dulla classe del model. In questo 
     * modo potresti avere un gestore di eventi ben ordinato al momeno in cui sulla vista qualcosa
     * cambia e invocando un Handler di evento per la notifica o quant'alltro tramite il OnProprertyChanged per esempio.
     * Alla fine possiamo portare questa particolare classe di proxy in un decorator che farà
     * si di aumentare le funzionalità.
     */

    // Model
    public class Person
    {
        public string FirstName;
        public string LastName;
    }

    // what if you need to update on changes?

    // View
    /// <summary>
    /// A wrapper around a <c>Person</c> that can be
    /// bound to UI controls.
    /// </summary>
    public class PersonViewModel
      : INotifyPropertyChanged
    {
        private readonly Person person;

        public PersonViewModel(Person person)
        {
            this.person = person;
        }

        public string FirstName
        {
            get => person.FirstName;
            set
            {
                if (person.FirstName == value) return;
                person.FirstName = value;
                OnPropertyChanged();
            }
        }

        public string LastName
        {
            get => person.LastName;
            set
            {
                if (person.LastName == value) return;
                person.LastName = value;
                OnPropertyChanged();
            }
        }

        // Decorator functionality (augments original object)
        // Project two properties together into, e.g., an edit box.
        public string FullName
        {
            get => $"{FirstName} {LastName}".Trim();
            set
            {
                if (value == null)
                {
                    FirstName = LastName = null;
                    return;
                }
                // Chiameremo FirstName e LastName piuttosto
                // che assegnare direttamente il valore nel modello
                // così da innescare il propertychanged per gli eventi 
                // nella ui che si sottoscrivono.
                var items = value.Split();
                if (items.Length > 0)
                    FirstName = items[0]; // may cause npc
                if (items.Length > 1)
                    LastName = items[1];
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        // In una UI l'evento si propaga attraverso l'invocazione del nome della proprietà
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(
          [CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this,
              new PropertyChangedEventArgs(propertyName));
        }
    }
}