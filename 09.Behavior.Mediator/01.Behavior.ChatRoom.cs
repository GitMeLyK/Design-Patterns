using System.Collections.Generic;
using System.Linq;
using static System.Console;

namespace DotNetDesignPatternDemos.Behavioral.ObjectMediator.ChatRoom
{

    /*
     * Un esempio classico di Mediator lo possiamo incontrare in tipologie
     * di software come chat. Dove la classe componente che fa da corpo centrale
     * per la comunicazione tra componenti è appunto la ChatRoom.
     * Ogni istanza di un componente Person in questo caso si sottoscrive a questa
     * ChatRoom con il metodo Join che è un api del componente mediatore, e così
     * per altre istanze dello stesso componente Person. 
     * La classe centrale del Mediator in questo caso detiene all'interno una lista
     * di tutti i partecipanti per le sottoscrizioni ed espone due metodi per la
     * comunicazione tra queste istanze uno è il Broadcast per impostare per ogni
     * sottoscrittore un messaggio in entrata e l'altro Message per impostare un
     * messaggio in entrata sul destinatario da un mittente specifico.
     * I metodi esposti dal componente Person per ricevere i messaggi che arrivano
     * dal mediator sono metodi che ne il mittente e ne il destinatario useranno
     * per cominucarsi direttamente quindi non avrepmo un tizio.Receive(...) chiamato
     * da caio anche perchè in modo semplicistico in questo codice ci si attiene al
     * pattern e queste chiamate dirette senza il set della room in cui la comunicazione
     * deve avvenire è fondamentale per identificare il mediator come centrale a queste
     * comunicazioni.
     */

    // Il Componente
    public class Person
    {
        public string Name;
        // Il riferimento alla ChatRoom mediatrice
        public ChatRoom Room;
        // I messaggi ricevuti dal mediator da parte di altri componenti
        private List<string> chatLog = new List<string>();

        public Person(string name)
        {
            Name = name;
        }

        public void Receive(string sender, string message)
        {
            string s = $"{sender}: '{message}'";
            WriteLine($"[{Name}'s chat session] {s}");
            chatLog.Add(s);
        }

        // Comunicazione al Mediator ChatRoom
        public void Say(string message)
        {
            Room.Broadcast(Name, message);
        }

        // Comunicazione al Mediator privato verso altro compnente identificato dal nome
        public void PrivateMessage(string who, string message)
        {
            Room.Message(Name, who, message);
        }
    }

    // Il Mediator
    public class ChatRoom
    {
        private List<Person> people = new List<Person>();

        // I messaggi da inviare a tutti i compinenti 
        public void Broadcast(string source, string message)
        {
            foreach (var p in people)
                if (p.Name != source)
                    p.Receive(source, message);
        }

        // La regitrazione a questo mediator
        public void Join(Person p)
        {
            string joinMsg = $"{p.Name} joins the chat";
            Broadcast("room", joinMsg);

            p.Room = this;
            people.Add(p);
        }

        // Il messaggio da in inviare a uno specifico componente
        public void Message(string source, string destination, string message)
        {
            people.FirstOrDefault(p => p.Name == destination)?.Receive(source, message);
        }
    }

    public class Demo
    {
        static void MainB(string[] args)
        {
            var room = new ChatRoom();

            var john = new Person("John");
            var jane = new Person("Jane");

            room.Join(john);
            room.Join(jane);

            john.Say("hi room");
            jane.Say("oh, hey john");

            var simon = new Person("Simon");
            room.Join(simon);
            simon.Say("hi everyone!");

            jane.PrivateMessage("Simon", "glad you could join us!");
        }
    }
}