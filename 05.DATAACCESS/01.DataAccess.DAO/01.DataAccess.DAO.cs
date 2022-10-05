using System;

namespace DotNetDesignPatternDemos.DataAccess.DAO
{

    /*
     *  Sebbene questo modello di progettazione sia ugualmente applicabile alla maggior parte 
     *  dei linguaggi di programmazione, alla maggior parte dei tipi di software con esigenze 
     *  di persistenza e alla maggior parte dei tipi di database, è tradizionalmente associato 
     *  alle applicazioni Java EE e ai database relazionali (accessibili tramite l'API JDBC 
     *  a causa della sua origine nelle linee guida sulle migliori pratiche di Sun Microsystems
     *  "Core J2EE Patterns" per quella piattaforma).
     *  
     *  Quindi facciamo una distinzione per Java e una per .Net
     *  
     *  Il vantaggio principale dell'utilizzo di oggetti di accesso ai dati è la separazione 
     *  relativamente semplice e rigorosa tra 2 parti importanti di un'applicazione che possono 
     *  ma non dovrebbero sapere nulla l'una dell'altra e che ci si può aspettare che si evolvano 
     *  frequentemente e in modo indipendente. 
     *  
     *  La modifica della logica di business può basarsi sulla stessa interfaccia DAO, mentre le 
     *  modifiche alla logica di persistenza non influiscono sui client DAO finché l'interfaccia 
     *  rimane implementata correttamente.
     *  
     *  Tutti i dettagli dell'archiviazione sono nascosti al resto dell'applicazione. 
     *  
     *  Pertanto, le possibili modifiche al meccanismo di persistenza possono essere implementate 
     *  semplicemente modificando un'implementazione DAO mentre il resto dell'applicazione non è 
     *  interessato. 
     *  
     *  I DAO fungono da intermediari tra l'applicazione e il database. 
     *  
     *  Spostano i dati avanti e indietro tra oggetti e record di database. 
     *  
     *  Lo unit test del codice è facilitato sostituendo il DAO con un doppio test nel test, 
     *  rendendo così i test indipendenti dal livello di persistenza.
     *  
     *  Svantaggi
     *  
     *  I potenziali svantaggi dell'utilizzo di DAO includono l'astrazione che perde, la 
     *  duplicazione del codice e l'inversione dell'astrazione. In particolare, l'astrazione 
     *  del DAO come un normale oggetto Java può nascondere l'elevato costo di ogni accesso 
     *  al database e può anche costringere gli sviluppatori a attivare più query di database 
     *  per recuperare informazioni che potrebbero altrimenti essere restituite in una singola 
     *  operazione utilizzando le operazioni del set SQL. 
     *  
     *  Se un'applicazione richiede più DAO, ci si potrebbe trovare a ripetere essenzialmente 
     *  lo stesso codice di creazione, lettura, aggiornamento ed eliminazione per ogni DAO. 
     *  
     *  Questo codice boiler-plate può essere evitato, tuttavia, implementando un DAO generico 
     *  che gestisce queste operazioni comuni.
     *  
     *  In Java 
     *  
     *  Nel contesto generale del linguaggio di programmazione Java, Data Access Objects come 
     *  concetto di progettazione può essere implementato in diversi modi. 
     *  Questo può variare da un'interfaccia abbastanza semplice che separa le parti di accesso 
     *  ai dati dalla logica dell'applicazione, ai framework e ai prodotti commerciali. 
     *  I paradigmi di codifica DAO possono richiedere una certa abilità. 
     *  Tecnologie come Java Persistence API ed Enterprise JavaBeans sono integrate nei server 
     *  applicazioni e possono essere utilizzate in applicazioni che utilizzano un server 
     *  applicazioni JavaEE.
     *  
     *  In .Net o C++ o vb
     *  
     *  Nel contesto invece del framework .Net il Data Access Objects come implementazione di 
     *  questa metodologia di progettazione ha subito nel tempo delle variazioni.
     *  In primis il DAO come componente essenziale in ambiente microsoft era offerto come 
     *  libreria indipendente e utilizzabile per lo più per l'accesso a database basati su ISAM
     *  come Access, dopodichè ha subito dei cambiamenti ed è stato istiuito come componente
     *  incorporato nel framework .net con svariati nomi e soluzioni, alla fine per .net core
     *  il tutto è presente nella parte fondamentale per l'accesso ai dati sotto il namespace
     *  system.data.
     *  Viene presentato in questa sezione appunto l'intero processo di evoluzione che avuto 
     *  questo oggetto e come è oggi usato. vedere il riferimento successito a questa documentaizione.
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
