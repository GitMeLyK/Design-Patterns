using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using static System.Console;
//using RxSamples.Annotations;
using JetBrains.Annotations;

namespace DotNetDesignPatternDemos.Behavioral.Observer.Collections
{
    /*
     * Come abbiamo visto negli esempi precedenti il Modello Observer torna
     * utile in tutte le parti del sistema che hanno bisogno di ricevere notifiche
     * da queste classi che sono Osservabili. E come abbiamo visto esiste già il
     * modo Built-in di usare gli eventi per metodi delegati che si sottoscrivono
     * a questa speciale proprietà offerta dal framework come anche implementando
     * le estensioni reattive tramite la dll a corredo RX si può operare in modo
     * più strutturale nella dichiarazione di oggetti Osservabili e oggetti che Osservano
     * definendo e delegando il gestore degli eventi a notificare tutti i sottoscrittori.
     * Ma parliamo adesso degli eventi intesi come insieme cioè collection che tornano
     * utili in molti aspetti della programmazione dove si ha che fare con liste
     * di oggetti trattati e di cui si vuole tenere traccia dei momenti in cui la lista
     * subisce modifiche in aggiunta o in elimnazione di elementi, e questo grazie al
     * fatto che chi si sottoscrive all'Observer per monitoreare queste liste abbia un
     * tipo di associazione particolare chiamata Binding.
     * Quindi come possiamo vedere se si tratta di un singolo valore da trattare questo
     * sarebbe stato un gestore di evento con la keyword event che tramite i metodi delegati
     * che si sottoscrivono o con l'aiuto dell'implementazione dell interfaccia  INotifyPropertyChanged
     * e l'ausilio delle property annotations che supportano l'hanlder come decorator si
     * presuppone che nella prima classe di esempio Market:INotifyPropertyChanged abbiamo
     * il classico uso di event come gestore responsabile del pattern Observer.
     * Nel secondo caso invece si tiene in considerazione che per ogni cambiamento delp prezzo
     * sia osservabile non il singolo valore a una collecion o un qualsiasi tipo enumerable di 
     * insieme di valori. Per dire in breve Se cambia il numero di elementi che sono nella lista
     * perchè si è appena eseguita un aggiunta di un nuovo prezzo questa viene notificata alle
     * classi Observing che terranno in considerazione l'interffaccia Bindings<list..> implementata
     * e andranno tramite le enum ItemAdded ItemDeleted etc. a fare qualcosa a tal proposito.
     */


    // L'Observer con un solo parametro  il volatility da notificare
    // come valore da passare a tutti i sottoscrittori ogni volta che cambia
    // Implmentato con le estensioni Rx annotations per il gestore dell'event
    /*
    public class Market : INotifyPropertyChanged
    {

        private float volatility;

        public float Volatility
        {
            get => volatility;
            set
            {
                if (value.Equals(volatility)) return;
                volatility = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    */

    // La classe Observer tramite
    // il supporto a un insieme per
    // la gestione come evento del suo stato.
    public class Market
    {

        //    public List<float> Prices = new List<float>();
        //
        public void AddPrice(float price)
        {
            Prices.Add(price);
            //PriceAdded?.Invoke(this, new PriceAddedEventArgs{ Price = price});
        }
        //
        //    public event EventHandler<PriceAddedEventArgs> PriceAdded;
        public BindingList<float> Prices = new BindingList<float>();
    }

    // L'evento
    public class PriceAddedEventArgs
    {
        public float Price;
    }

    // Il Pattern relativo alla gestione d'insime 
    public class ObserverPattern
    {
        static void MainOP(string[] args)
        {
            Market market = new Market();
            
            // Nel primo caso secondo un singolo valore di stato
            //      market.PriceAdded += (sender, eventArgs) =>
            //      {
            //        Console.WriteLine($"Added price {eventArgs.Price}");
            //      };
            //      market.AddPrice(123);
            
            // Nel secondo caso con l'observer implementato per monitorare
            // l'insieme dei valori di stato aggiunti che grazie all'uso
            // del Bindings del framework associa per le Liste un evento di stato
            market.Prices.ListChanged += (sender, eventArgs) => // Subscribe
            {
                if (eventArgs.ListChangedType == ListChangedType.ItemAdded)
                {
                    Console.WriteLine($"Added price {((BindingList<float>)sender)[eventArgs.NewIndex]}");
                }
            };
            market.AddPrice(123);
            // 1) How do we know when a new item becomes available?

            // 2) how do we know when the market is done supplying items?
            // maybe you are trading a futures contract that expired and there will be no more prices

            // 3) What happens if the market feed is broken?

        }
    }
}