using System;

namespace DotNetDesignPatternDemos.SOLID.InterfaceSegregationPrinciple
{

    /*
     *  Il Principio di Segregazione o Separazione per Interfacce.
     *  Nell'esempio sintetico vediamo come questo principio di separazione
     *  per interfaccia venga compreso al meglio. Questo modo di pensare
     *  porta in definitiva a un concetto molto semplice e cioè quello 
     *  di separare il più possibile l'uso dei metodi nelle interfacce quando
     *  una di queste diventa troppo grande. Questo principio risolve una
     *  questione nelle classi che implmenetano quella interfaccia, e cioè 
     *  non obbligare le classi che la implementano a dover per forza sovraccaricare
     *  il codice non utilizzato per tutti gli altri metodi dell'interfaccia che
     *  non servono e non sono di rilievo nella classe che la adotta.
     *  Nell'esempio vediamo come la classe document è usata come oggetto per le
     *  classi Printer specializzate. Ogni classe Printer che emula un determinato
     *  tipo di stampante/scanner è definita per fare alcune cose e non altre.
     *  Nel caso della stampante completa MultiFunctionPrinter che adotta un interfaccia
     *  completa i metodi implementabili sono tutti quelli dell'interfaccia IMachine, e
     *  per questo tipo questa interfaccia potrebbe essere adatta allo scopo.
     *  Nel caso invece di una stampante vecchia definita nella Classe
     *  OldFashionedPrinter ti ritroveresti che come si vede nell'esempio
     *  dovresti implementare la stesssa interfaccia completa e quindi
     *  a dover implementare le funzioni Fax e Scan che in realtà non usi.
     *  Quindi come puoi vedere questa interffaccia porta con sè molte
     *  responsabilità che a seconda dei casi non servono.
     *  Secondo questo principio invece intervine separando le singole responsabilità
     *  quindi il meotod o i metodi dall'interfaccia completa di tutto e ne definisci
     *  nuove interfacce singole con il suo set di responsabilità, in questo caso
     *  lìinterfaccia completa ha tre responsabilità diverse e con scopi diversi,
     *  avrai altrettante interfacce per ogni singolo metodo implementativo.
     *  IScanner per la funzione Scan IPrinter per la funzione Print etc.
     *  Le classi quindi per specializzazione di classe adotteranno una o più di 
     *  queste interfacce per lo scopo, e come in questo esempio vedremo che la
     *  classe Printer che si occupa solo di essere un dispositivo di stampa e non 
     *  altro implementerà solo IPrinter, mentre per la classe Photocopier saranno
     *  due interfacce ad essere implementate cioè la IPrinter e la IScan che per
     *  il dispositivo di cui tratta la classe sono per il suo scopo.
     *  E' anche possibile usare combinazioni ereditate su interfacce ad hoc per
     *  il dispositivo di destinazione quindi per la classe nell'esempio MultiFunctionMachine
     *  vediamo che invece di dichiarare ogni singola interfaccia per il suo scopo
     *  ne adotta una singola chiamata IMultiFunctionDevice che è un interfaccia che 
     *  eredita tutte le interfacce adatte allo scopo per una destinazaione particolare.
     */


    public class Document
    {
    }

    // Questa interfaccia emula tutti i metodi
    // per stampare fare fax e scannerizzare.
    public interface IMachine
    {
        void Print(Document d);
        void Fax(Document d);
        void Scan(Document d);
    }

    // ok if you need a multifunction machine
    public class MultiFunctionPrinter : IMachine
    {
        public void Print(Document d)
        {
            //
        }

        public void Fax(Document d)
        {
            //
        }

        public void Scan(Document d)
        {
            //
        }
    }

    public class OldFashionedPrinter : IMachine
    {
        public void Print(Document d)
        {
            // yep
        }

        public void Fax(Document d)
        {
            throw new System.NotImplementedException();
        }

        public void Scan(Document d)
        {
            throw new System.NotImplementedException();
        }
    }

    // Quando invece è megli relegare a più interfacce ogni singola funzionalità
    // e secondo il principio della segregazione aggiungi tante interfacce quante
    // sono le singole repsonsabilità da implementare.

    public interface IPrinter
    {
        void Print(Document d);
    }

    public interface IScanner
    {
        void Scan(Document d);
    }

    public interface IFax
    {
        void Send(Document d);
    }

    // Ecco quindi una stampante semplice
    public class Printer : IPrinter
    {
        public void Print(Document d)
        {

        }
    }

    // e ottenere ciò che serve nelle clssi concrete

    // Un Fotocopiatore che usa IPrinter e IScan
    public class Photocopier : IPrinter, IScanner
    {
        public void Print(Document d)
        {
            throw new System.NotImplementedException();
        }

        public void Scan(Document d)
        {
            throw new System.NotImplementedException();
        }
    }

    // o creare tramite ereditarietà interfacce più consone allo specifico problema
    
    // Uso di un Interfaccia che per ereditarietà adotta 
    // le interfacce per combinare le responsabilità, in
    // questo caso la IMultiFunctionDevice eredita IPrinter e IScan
    public interface IMultiFunctionDevice : IPrinter, IScanner //
    {

    }

    // E la classe o la struct come in questo caso implmentano
    // una singola interfaccia combinata precedentemente per
    // le repsonsabilità coinvolte da questi di oggetti.
    // Ed è possibile anche iniettare le classi che singolarmente
    // hanno l'implementazione delle responsabilità che devono
    // essere wrappate dalla classe e poi essere usate per i metodi
    // responsabilit dell'uso di quella responsabilità.
    public struct MultiFunctionMachine : IMultiFunctionDevice
    {
        // compose this out of several modules
        private readonly IPrinter printer;
        private readonly IScanner scanner;

        public MultiFunctionMachine(IPrinter printer, IScanner scanner)
        {
            if (printer == null)
            {
                throw new ArgumentNullException(paramName: nameof(printer));
            }
            if (scanner == null)
            {
                throw new ArgumentNullException(paramName: nameof(scanner));
            }
            this.printer = printer;
            this.scanner = scanner;
        }

        public void Print(Document d)
        {
            printer.Print(d);
        }

        public void Scan(Document d)
        {
            scanner.Scan(d);
        }
    }

    class Demo
    {
        static void Main100(string[] args)
        {
            Console.WriteLine("");
        }
    }

}
