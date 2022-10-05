using System;

namespace DotNetDesignPatternDemos.Architectural.IdentityMap
{
    /*
     *  Il pattern IdentityMap definisce un comportamento relativo alla funzione di individuazione
     *  dei duplicati, molto usato in ambito DBMS.
     *  
     *  Assicura che ogni oggetto venga caricato una sola volta mantenendo ogni oggetto caricato 
     *  in una mappa. 
     *  
     *  Cerca gli oggetti utilizzando la mappa quando si fa riferimento a loro.
     *  
     *  Una mappa di identità conserva un record di tutti gli oggetti che sono stati letti dal 
     *  database in una singola transazione aziendale. 
     *  
     *  Ogni volta che vuoi un oggetto, controlla prima la mappa di identità per vedere se lo 
     *  hai già.
     * 
     * - Come funziona
     * 
     *      Esistono 4 tipi di mappa di identità
     * 
     *          Esplicito
     *          Generico
     *          Sessione
     *          Classe
     *  
     *  L'idea di base alla base della mappa di identità è quella di avere una serie di mappe 
     *  contenenti oggetti che sono stati estratti dal database. 
     *  
     *  In un caso semplice, con uno schema isomorfo, avrai una mappa per tabella di database.
     *  
     *  Quando si carica un oggetto dal database, si controlla innanzitutto la mappa. 
     *   - Se è presente un oggetto che corrisponde a quello che stai caricando, lo restituisci. 
     *   - In caso contrario, si accede al database, inserendo gli oggetti sulla mappa per 
     *     riferimento futuro durante il caricamento.
     * 
     * - Scelta delle chiavi
     * 
     *  La prima cosa da considerare è la chiave per la mappa. 
     *  La scelta ovvia è la chiave primaria della tabella di database corrispondente.
     *  
     *  Esplicito o generico
     *  
     *  Devi scegliere se rendere esplicita o generica la mappa di identità. 
     *  È possibile accedere a una mappa di identità esplicita con metodi distinti per ogni tipo 
     *  di oggetto necessario: 
     *  
     *      ad esempio findPerson(1). 
     *      Una mappa generica utilizza un unico metodo per tutti i tipi di oggetti, con forse 
     *      un parametro per indicare quale tipo di oggetto è necessario, 
     *      
     *      ad esempio find("Person", 1). 
     *      L'ovvio vantaggio è che è possibile supportare una mappa generica con un oggetto 
     *      generico e riutilizzabile.
     *      
     * - Quanti
     * 
     *  Qui la decisione varia tra una mappa per classe e una mappa per l'intera sessione. 
     *  
     *  Una singola mappa per la sessione funziona solo se si dispone di chiavi univoche del 
     *  database. 
     *  
     *  Una volta che hai una mappa di identità, il vantaggio è che hai solo un posto dove 
     *  andare e nessuna decisione imbarazzante sull'eredità.
     *  
     * - Dove metterli
     * 
     *  Le mappe di identità devono essere da qualche parte dove sono facili da trovare. 
     *  Sono anche legati al contesto del processo in cui stai lavorando.
     *  
     * - Quando usarlo
     * 
     *  L'idea alla base del modello Identity Map è che ogni volta che leggiamo un record 
     *  dal database, controlliamo prima la Identity Map per vedere se il record è già stato 
     *  recuperato. 
     *  
     *  Questo ci consente di restituire semplicemente un nuovo riferimento al record in 
     *  memoria piuttosto che creare un nuovo oggetto, mantenendo l'integrità referenziale.
     *  
     *  Un vantaggio secondario della mappa di identità è che, poiché funge da cache, riduce 
     *  il numero di chiamate al database necessarie per recuperare gli oggetti, il che produce 
     *  un miglioramento delle prestazioni.
     *  
     * - Codice di esempio
     * 
     *  Creiamo il diagramma di sequenza per demonstarte IdentityMap Pattern.
     *  
     *  Con riferimento al diagramma di sequenza sopra la sequenza di passaggi è
     *  
     *      1. La richiesta arriva prima al metodo di ricerca con la chiave primaria.
     *      
     *      2. Il Finder controlla se PersonMap (IdentityMap) contiene l'oggetto persona 
     *         per una determinata chiave primaria.
     *         
     *      3. Se l'oggetto persona viene trovato in PersonMap (IdentityMap), restituire 
     *         l'oggetto persona da PersonMap e non colpire il database.
     *         
     *      4. Se l'oggetto persona non viene trovato in PersonMap (IdentityMap), genera 
     *         l'SQL nel database e ottiene il record dal database.
     * 
     * Puoi vedere qui, riduce il numero di chiamate al database necessarie per recuperare gli 
     * oggetti, il che produce un miglioramento delle prestazioni.
     * 
     *      // Figura 1 : identitymap-sequence-diagram.png
     *      
     *      // Code 1
     * 
     */

    /*
     * Code 1
     * 

        public class Person
        {
            private int key;
            private String firstName;
            private String lastName;
            private String noOfDependents;

            // getter and setter methods
        }

    
        public class PersonDatabase
        {

            public Person finder(int key)
            {

                // Check for person object in IdentityMap
                Person person = IdentityMapUtility.getPerson(key);
                if (person == null)
                {
                    // get person object from database

                    // add person object to IdentityMap
                    addPerson(person);
                }
                return person;

            }
        }
        public class IdentityMapUtility
        {

            // personMap as IdentityMap
            private Map personMap = new HashMap();

            // Add person object to IdentityMap
            public static void addPerson(Person arg)
            {
                personMap.put(arg.getID(), arg);
            }

            // Retrieve person object from personMap
            public static Person getPerson(Long key)
            {
                return (Person)personMap.get(key);
            }

            public static Person getPerson(long key)
            {
                return getPerson(new Long(key));
            }
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
