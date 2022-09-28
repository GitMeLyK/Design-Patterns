using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using static System.Console;

namespace DotNetDesignPatternDemos.Structural.Composite.GeometricShapes
{
    /*
     * In questo eesempio vediamo che un ipotetica rete neurale foramta
     * da due insiemi diversi e ogni insieme è quindi un aggregato composito 
     * da gestire per via di alcuni metodi che vogliamo inserire dove un insieme
     * deve comunicare con l'altro insieme come proprio in una rete neurale dove
     * un neurone chiama l'altro. In questo caso gli insieme In e Out usano appunto
     * nella classe composita un metodo chiemato ConnectTo per fare qualcosa.
     * Questo metodo ConnectTo è usato nel contesto come estensione per non essere
     * ereditato dalla classe base ma contestualizzato al singolo insieme di neuroni
     * mentre abbiamo un altro strato NeuronLayer che a sua volta Collection di Neuroni
     * e anche questa diventi soggetta alle connessioni di base.
     * Quindi fondamentalmente mascherando anche il singolo elemento come fosse un aggregato, 
     * ed evitando assolutamente l'ereditarietà per non avere un sacco di metodi per connettere
     * sistemi diversi possiamo dire che il pattern composito tratterà i valori scalari come
     * valori compositi o con contenuto singolare permettendoci di scrivere algoritmi molto
     * generalizzati su valori sia singolari che compositi.
     * */


    // we cannot use a base class 
    // Ecco quindi che per connettersi tra moltitudini di aggegati di 
    // natura diversa viene in soccorso piuttosto che ereditare da Neuron (che sarebbe un errore)
    // un singolo metodo di estensione che prenda qualsiai aggregato di tipo enumerable come List o Collection
    // e faccia il suo lavoro trattando tutto come fosse un insieme e non un singolo elemento e quindi
    // scorre (laddove fosse un Neuron singolo, che abbiamo opportunamente modificato ad implementare
    // comunque un interfaccia IEnumerable) o un LayerNueron che è una Collaection o altro che sia di
    // di tipo enumerable il Modello Composito ne ridefinisce il comportamento per quello che li accoumenrà
    // tutti e cioè l'enumerable e sfrutterà questo per avere le connessioni instrinseche.
    public static class ExtensionMethods
    {
        public static void ConnectTo(this IEnumerable<Neuron> self, IEnumerable<Neuron> other)
        {
            if (ReferenceEquals(self, other)) return;

            foreach (var from in self)
                foreach (var to in other)
                {
                    from.Out.Add(to);
                    to.In.Add(from);
                }
        }
    }

    // La classe implementerà perchè anche essa si comporti come una collection
    // l'interfaccia di tipo IEnumerable<Neuron> ma solo per poter sfruttare il singolo
    // metodo ConnectTo esteso, infatti il metodo di default di interfaccia non farà
    // altro che ritornare se stesso come collection per poter essere usatea dal metodo
    // cconnectto come per le altre classi.
    public class Neuron : IEnumerable<Neuron>
    {
        public float Value;
        public List<Neuron> In, Out;

        // Questo è il metodo con cu si connette un singolo neurone
        // quindi nel singolo elemento della stessa natura questo diventa
        // legittimo ma.. nel momento in cui volessimo aggiungere un altro tipo
        // in questo una NeuroLayer usando anche lì uno stesso metodo, la cosa
        // diventa più comlicata su due aggregati dinisieme diversi, e i metodi devono diventare
        // due per questa classe che si aspetta anche una collection di NeuronLayer
        // e ripetuti anche nella classe NeuronLayer, ecco quindi che separiamo in un estensione
        // questo metodo e ne possono usufruire tutte e due le classi, in questo modo
        // stiamo orgnaizzando in modo composito un collegamente tra aggregati di natura 
        // diversi e che rispettino quello che hanno in comune, in questo caso List o Collection.
        /*
        public void ConnectTo(Neuron other)
        {
            Out.Add(other);
            other.In.Add(this);
        }
        */

        public IEnumerator<Neuron> GetEnumerator()
        {
            yield return this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            yield return this;
        }
    }

    public class NeuronLayer : Collection<Neuron>
    {
    }

    // Quindi come vediamo diversi tipologie di aggregati
    // comportano un maggior numero di metodi per interconnettersi
    // tra di loro
    public class NeuronRing : Collection<Neuron>
    {
    }


    public class Demo2
    {
        static void Main(string[] args)
        {
            var neuron1 = new Neuron();
            var neuron2 = new Neuron();
            var layer1 = new NeuronLayer();
            var layer2 = new NeuronLayer();

            neuron1.ConnectTo(neuron2);
            neuron1.ConnectTo(layer1);
            layer1.ConnectTo(layer2);
        }
    }
}
