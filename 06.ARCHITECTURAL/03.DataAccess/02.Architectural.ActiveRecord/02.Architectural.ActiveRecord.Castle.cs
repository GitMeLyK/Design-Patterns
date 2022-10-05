using System;

namespace DotNetDesignPatternDemos.Architectural.ActiveRecord.Castle
{
    /*
     * Come abbiamo già accennato nella prima parte il progetto Castle anche 
     * se non più seguito dalla community rimane sempre un buon pretesto per
     * chi vuole usare il modello di progettazione per l'accesso ai dati tramite
     * questo pattern Active Record.
     * 
     * Castle porta con sè molto degli aspetti affrontati da Ruby on Rails ed
     * è fortemente legato a quest'ultimo, quindi di buon auspicio da chi già
     * viene da questo mondo e vuole approdare per affinità su .Net.
     * 
     * Questo progetto in sè contempla diversi sotto progetti che sono interessanti:
     * 
     *  ActiveRecord
     *  
     *      Castle ActiveRecord è il modello di mappatura dei dati aziendali implementato 
     *      utilizzando NHibernate.
     *      
     *  MonoRail
     *      
     *      Castle MonoRail è un framework web MVC che consente di creare rapidamente 
     *      applicazioni testabili e gestibili.
     *      
     *  Windsor
     *      Castle Windsor è il miglior contenitore di inversione del controllo IOC 
     *      (o come alcuni amano chiamarlo - Iniezione di dipendenza). 
     *      Come parte del progetto, ci sono anche una manciata di strutture, 
     *      che estendono Windsor e / o lo integrano con altri strumenti.
     *      
     *  DynamicProxy
     *  
     *      Castle DynamicProxy è un framework di generazione proxy runtime molto veloce 
     *      e leggero. È utilizzato da altri progetti sotto il cofando da Castle, così 
     *      come da numerosi altri progetti, come NHibernate, Moq, Rhino Mocks e molti altri.
     *      
     *  Tools
     *  
     *      I servizi e gli strumenti Castle ti aiutano a avviare facilmente la tua 
     *      infrastruttura.
     * 
     *  Parlando ancora di Active Record viene così definito il progetto di Castle:
     *  
     *  Il progetto Castle ActiveRecord è un'implementazione del modello ActiveRecord per .NET. 
     *  Il modello ActiveRecord è costituito da proprietà di istanza che rappresentano un record 
     *  nel database, metodi di istanza che agiscono su quel record specifico e metodi statici 
     *  che agiscono su tutti i record.
     *  
     *  Castle ActiveRecord è basato su NHibernate, ma il suo mapping basato su attributi libera 
     *  lo sviluppatore della scrittura xml per il mapping da database a oggetto, necessario 
     *  quando si utilizza direttamente NHibernate.
     *  
     *  ⚠️ Avvertimento: Sebbene ActiveRecord renda facile l'utilizzo di NHibernate, non 
     *  nasconde tutti i dettagli del comportamento di NHibernate. 
     *  
     *  È necessario comprendere il comportamento di clean di NHibernate e come lavorare con 
     *  le colonne di testo/ntesto.
     *  
     *  Tutti i dettagli e la documentazione di questo framework sono disponibili al seguente indirizzo:
     *  
     *      https://github.com/castleproject-deprecated/ActiveRecord/blob/master/docs/README.md
     *      
     *  Il codice all'ultima versione rilasciata è in allegato a questa documentazione.
     *  
     *      File : Activerecord-master.zip
     */

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
