using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static System.Console;

namespace DotNetDesignPatternDemos.Structural.MessageBroker
{
    /*
     * In questo esempio viene illustrata ampiamente l'implementazione del codice C# 
     * per il modello di broker di messaggi che si trova in genere come soluzione 
     * all'intermediazione/routing dei messaggi nei prodotti software aziendali.
     * In molti prodotti software aziendali, è necessario instradare i messaggi tra 
     * i componenti del prodotto, con le condizioni che
     *       * Il routing dei messaggi non deve essere in grado di riconoscere il tipo, 
     *        significa che il componente che instrada effettivamente i messaggi non deve 
     *        preoccuparsi del tipo di messaggio e sia l'editore del messaggio che il destinatario 
     *        del messaggio nel canale di routing devono essere disaccoppiati, 
     *        il che significa che l'editore non deve essere consapevole di chi sono gli 
     *        abbonati al messaggio. 
     *       * Allo stesso modo, i sottoscrittori non devono essere a conoscenza dell'autore 
     *        del messaggio publisher può anche essere un abbonato per il tipo di messaggio 
     *        che intende ricevere.
     * 
     * Il broker/scambio di messaggi è illustrato nel diagramma in allegato a questo esempio, 
     * in cui la direzione della freccia dal componente verso il messaggio (A, B, ecc.) 
     * rappresenta la pubblicazione, mentre la direzione della freccia dal messaggio al 
     * componente rappresenta la sottoscrizione. 
     * Inoltre, gli editori sono completamente trasparenti dal meccanismo di pubblicazione/consumo 
     * e dai consumatori effettivi.
     * 
     * ** Si noti che il modello di broker di messaggi descritto in questo esempio è per la 
     *    soluzione all'interno del contesto del processo e non descrive l'intermediazione/routing 
     *    dei messaggi tra i sistemi distribuiti. Per tale scalabilità di sistemi, abbiamo già 
     *    broker di messaggi aziendali, come Kafka, coda del bus di servizio di Azure e così via.
     * 
     * Si consideri la seguente interfaccia di livello core, che definisce un contratto per un 
     * broker di messaggi. Come indicato in esso, il metodo è un metodo di pubblicazione generico 
     * di qualsiasi payload di tipo. 
     * In genere, l'originatore chiamerà questo metodo per pubblicare un messaggio di tipo. 
     * Il metodo viene chiamato dal client per sottoscrivere un messaggio di tipo. 
     * 
     * Si noti che il sottoscrittore aggancia il metodo di azione del gestore per ricevere il 
     * payload del messaggio ed eseguire l'azione su di esso di conseguenza.
     *   Publish<T>() Subscribe<T>() Unsubscribe<T>()
     * 
     * Il tipo è un tipo generico, che porta il messaggio originale. 
     * Le proprietà - , e sono le proprietà che descrivono rispettivamente l'origine, 
     * il contenuto e l'ora di pubblicazione. 
     * La classe è descritta di seguito: MessagePayload TWhoWhatWhen
     * 
     * L'implementazione dell'interfaccia di cui sopra è descritta nel codice seguente, 
     * in cui il broker è implementato come istanza singleton. 
     * Si prega di notare che il broker deve essere un singleton per garantire che tutti 
     * i messaggi siano instradati solo attraverso quell'istanza.
     * 
     * Nei prossimi due esempi che introducono un modello di infrastrtuttura enterprise
     * per lo svilupppo di microservizi vedremo come questo pattern è una parte fondamentale.
     * 
     * Ma anche gli altri pattern per l'implmenetazione di microservizi particolari basati
     * sull'accesso e la comunicazione sono indispensabili come il facade per L API Gateway
     * che fà da reverse proxy verso le chiamate tra clien e microservizi il Proxy l'adapter
     * etc.
     * 
     */

    // Interfaccia per il gestore
    public interface IMessageBroker : IDisposable
    {
        void Publish<T>(object source, T message);
        void Subscribe<T>(Action<MessagePayload<T>> subscription);
        void Unsubscribe<T>(Action<MessagePayload<T>> subscription);
    }

    // Tipo di Classe generic per gli oggetti PayLoad
    public class MessagePayload<T>
    {
        public object Who { get; private set; }
        public T What { get; private set; }
        public DateTime When { get; private set; }
        public MessagePayload(T payload, object source)
        {
            Who = source; What = payload; When = DateTime.UtcNow;
        }

    }

    // Implementazione interfaccia Broker singleton
    // -- 
    //  L'implementazione dell'interfaccia del broker di messaggi mantiene un dizionario
    //  centralizzato di tipo di messaggio rispetto al suo elenco di sottoscrittori.
    //  Ogni chiamata popolerà questo dizionario con il tipo come chiave.
    //  Considerando che, la chiamata a garantirà che la chiave venga rimossa dal dizionario
    //  o che il metodo di azione di sottoscrizione venga rimosso dall'elenco che rappresenta
    //  i sottoscrittori per il tipo. 
    public class MessageBrokerImpl : IMessageBroker
    {
        private static MessageBrokerImpl _instance;
        private readonly Dictionary<Type, List<Delegate>> _subscribers;
        public static MessageBrokerImpl Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new MessageBrokerImpl();
                return _instance;
            }
        }

        private MessageBrokerImpl()
        {
            _subscribers = new Dictionary<Type, List<Delegate>>();
        }

        public void Publish<T>(object source, T message)
        {
            if (message == null || source == null)
                return;
            if (!_subscribers.ContainsKey(typeof(T)))
            {
                return;
            }
            var delegates = _subscribers[typeof(T)];
            if (delegates == null || delegates.Count == 0) return;
            var payload = new MessagePayload<T>(message, source);
            foreach (var handler in delegates.Select
            (item => item as Action<MessagePayload<T>>))
            {
                Task.Factory.StartNew(() => handler?.Invoke(payload));
            }
        }

        public void Subscribe<T>(Action<MessagePayload<T>> subscription)
        {
            var delegates = _subscribers.ContainsKey(typeof(T)) ?
                            _subscribers[typeof(T)] : new List<Delegate>();
            if (!delegates.Contains(subscription))
            {
                delegates.Add(subscription);
            }
            _subscribers[typeof(T)] = delegates;
        }

        public void Unsubscribe<T>(Action<MessagePayload<T>> subscription)
        {
            if (!_subscribers.ContainsKey(typeof(T))) return;
            var delegates = _subscribers[typeof(T)];
            if (delegates.Contains(subscription))
                delegates.Remove(subscription);
            if (delegates.Count == 0)
                _subscribers.Remove(typeof(T));
        }

        public void Dispose()
        {
            _subscribers?.Clear();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
