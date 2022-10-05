using System;
using System.Collections.Generic;

namespace DotNetDesignPatternDemos.Architectural.IdentityMap.Example
{

    /*
     *  Per questo pattern comportamentale illustriamo un esempio pratico.
     *  
     *  Supponiamo di dover tenere traccia degli oggetti che sono stati istanziati 
     *  nella nostra applicazione. 
     *  
     *  Il modello "Identity Map" fa proprio questo e possiamo implementarlo con un 
     *  semplice Dictionary<TKey, TValue>. 
     *  
     *  Tuttavia, abbiamo bisogno di un dizionario diverso per ogni tipo di oggetto che 
     *  può diventare ingombrante di cui tenere traccia. 
     *  
     *  Diamo un'occhiata a una tecnica con cui possiamo avere tutti i nostri pool in 
     *  un oggetto pool aggregato.
     *  
     *  Il trucco è mantenere la sicurezza del tipo. 
     *  
     *  Al centro si connette con il pool aggregato non conoscendo necessariamente i tipi, 
     *  ma tutto sull'esterno fruibile che è pubblicamente disponibile per il consumo di 
     *  codice lo sarà. 
     *  
     *  Diciamo che tutti i nostri oggetti hanno una proprietà chiave che è un Int32. 
     *  
     *  Se questo è il caso, possiamo semplicemente costruire un wrapper type-safe per 
     *  un Dictionary<Type, Dictionary<Int32, Object>> dove l'oggetto interno è del tipo 
     *  della chiave del dizionario esterno.
     *  
     *      // Immagine 1 : image002.jpg
     *      
     *  Per mantenere le cose al sicuro, dovremo rendere privata l'istanza del pool di 
     *  dizionari nidificati. 
     *  
     *  Quindi, per aggiungere in modo sicuro un elemento al pool, dobbiamo prima verificare 
     *  se esiste un dizionario "interno" prima di eseguire il metodo add.
     *  
     *      // Code 1
     *      
     *  Quando rimuoviamo un oggetto dal pool, dovremo nuovamente verificare che esista 
     *  il dizionario "interno":
     *  
     *      // Code 2
     *      
     *  Se questi due metodi sono l'unico modo per aggiungere e rimuovere elementi dal pool, 
     *  possiamo essere sicuri del tipo di oggetto nel Dictionary<Int32 interno, Object>. 
     *  
     *  Questo ci consente di avere un oggetto mappa di identità per tutti i nostri oggetti caricati. 
     *  
     *  Quindi possiamo avere un metodo per recuperare qualsiasi oggetto nel pool:
     *  
     *      // Code 3
     *      
     *  La cosa a cui prestare attenzione è che se si dispone di lunghe catene di ereditarietà nel 
     *  modello a oggetti, è possibile immettere un oggetto nel pool con molte chiavi "tipo" 
     *  diverse e non sapere come recuperare gli oggetti inseriti nel dizionario. 
     *  
     *  Essendo consapevoli di questo, è necessario essere sicuri di utilizzare tipi coerenti 
     *  con ogni chiamata al metodo.
     *  
     *  Ecco come utilizzeremmo il nostro pool aggregato:
     *  
     *      // Code 4
     *      
     *      In allegato a questo esempio è presente un file compresso con tutto il codice
     *      riportato.
     *      
     *      //  File    :   PoolSample.zip
     * 
     */

    /*
     * Code 1
     * 
    
    private Dictionary<Type, Dictionary<Int32, Object>> m_pool;

    public void AddItem<T>(Int32 pID, T value)
    {
        Type myType = typeof(T);

        if (!m_pool.ContainsKey(myType))
        {
            m_pool.Add(myType, new Dictionary<int, object>());
            m_pool[myType].Add(pID, value);
            return;
        }

        if (!m_pool[myType].ContainsKey(pID))
        {
            m_pool[myType].Add(pID, value);
            return;
        }

        m_pool[myType][pID] = value;
    }
    */

    /*
     * Code 2
     * 
        public bool RemoveItem<T>(Int32 pID)
        {
            Type myType = typeof(T);

            if (!m_pool.ContainsKey(myType))
                return false;

            if (!m_pool[myType].ContainsKey(pID))
                return false;

            return m_pool[myType].Remove(pID);
        }

     */

    /*
     * Code 3

            public T GetItem<T>(Int32 pID)
            {
                // will throw KeyNotFoundException if either of the dictionaries
                // does not hold the required key
                return (T) m_pool[myType][pID];
            }      

     */


    class Program
    {
        //public static ObjectPool pool = new ObjectPool();

        static void Main(string[] args)
        {

            /*
             * Code 4
             * 
            
            Animal dog = new Animal() { Name = "Fido", ID = 1 };
            Vegetable carrot = new Vegetable { Color = "Orange", Identifier = 1, IsTasty = true };
            Mineral carbon = new Mineral() { UniqueID = 2, IsPoisonousToAnimal = false };

            pool.AddItem<Animal>(dog.ID, dog);
            pool.AddItem<Vegetable>(carrot.Identifier, carrot);
            pool.AddItem<Mineral>(carbon.UniqueID, carbon);

            Console.WriteLine("Dog is in the pool -- this statement is " + pool.ContainsKey<Animal>(dog.ID));
            Console.ReadLine();

            */
        }
    }
}
