using System;
using System.Collections.Generic;

namespace DotNetDesignPatternDemos.Concurrency.DoubleCheckedLocking
{

    /*
     * 
     * ● Perché utilizzare l'inizializzazione pigra - perché le prestazioni di le prime JVM 
     *   non erano molto buone. 
     *   
     *   Meglio ancora, cos'è l'inizializzazione pigra?
     *      
     *      ● L'inizializzazione pigra ritarda la creazione di un oggetto, il calcolo di 
     *      un valore o qualche altro processo costoso fino alla prima volta che serve.
     *  
     *  Esempio .:
     *  
     *  public class Fruit {
     *      private String typeName;
     *      private static Map<String, Fruit> types = new HashMap<String, Fruit>();
     *      private Fruit(String typeName) {
     *          this.typeName = typeName;
     *      }
     *      
     *  public static Fruit getFruitByTypeName(String type) {
     *      Fruit fruit;
     *      if (!types.containsKey(type)) {
     *          // Lazy initialization
     *          fruit = new Fruit(type); <-- ***
     *          types.put(type, fruit);
     *      } else {
     *          // Okay, it's available currently
     *          fruit = types.get(type);
     *      }
     *      return fruit;
     *  }
     * 
     *  *** Ecco l'inizializzazione pigra dell'oggetto Fruit. Se la stringa tipo parametro 
     *      fornito nella funzione, non è una chiave, quindi a viene creato un nuovo oggetto 
     *      fruit, altrimenti la funzione restituisce un tipo di frutta già esistente.
     *      
     *  Come Lavora questo Modello A doppio controllo del Lock?
     *  
     *  ● Verifica se è necessario o meno inizializzare senza sincronizzare, lazy inizializzazione.
     *  ● Se l'oggetto che sta cercando non è nullo, utilizzare esso.
     *  ● Altrimenti, se è nullo, sincronizzare e controllare di nuovo, per assicurarsi che la 
     *    risorsa non lo sia inizializzato e quindi consentire a un solo thread di farlo inizializzarlo.
     *  
     * Implementazione tipica di un Double-Checked Locking con inizializzazione lazy.:
     * 
     *  public static Fruit getFruitByTypeNameHighConcurrentVersion(String type) {
     *      Fruit fruit;
     *      if (!types.containsKey(type)) {
     *          synchronized (types) {
     *              if (!types.containsKey(type)) { <--- ***
     *                  // Lazy initialization
     *                  types.put(type, new Fruit(type));
     *              }
     *          }
     *     }
     *     fruit = types.get(type);
     *     return fruit;
     *  }
     * 
     *  *** Si assicura che dopo l'acquisizione il blocco sui tipi oggetto che nessun altro thread 
     *  ha creato un oggetto di frutta. In caso contrario, usa inizializzazione lazy per creare un 
     *  nuovo oggetto Fruit .
     *  
     *  Da tenere in considerazione per l'implementazione.:
     *  
     *  Come può fallire il Double-Checked Locking Prima di Java 5.0
     *  
     *      ● Il problema con la chiusura a doppio controllo è che non vi è alcuna garanzia 
     *        funzionerà su macchine a processore singolo o multi-processore.
     *      ● Il problema del fallimento della chiusura a doppio controllo non è dovuto a bug 
     *        di implementazione nelle JVM ma nella memoria della piattaforma Java precedente modello.
     *      ● Il modello di memoria ha consentito il verificarsi di "scritture fuori ordine" ed è il motivo
     *        Double-Check Locking è caduto in disgrazia.
     *  
     *  * Il metodo della GuardedSupsension sembra antiquato rispetto al Modello di
     *   progettazione Scheduler che fornisce una granularità più fine per la notifica del thread.
     *   
     */

    // Classe di esempio con utilizza uno DoubleChecked Lock in modo errato
    public static class ValuateList
    {
        private static readonly object _lock = new object();
        private static volatile IDictionary<string, object> _cache =
            new Dictionary<string, object>();

        public static object Create(string key)
        {
            object val;
            if (!_cache.TryGetValue(key, out val))
            {
                lock (_lock)
                {
                    if (!_cache.TryGetValue(key, out val))
                    {
                        val = new object(); // factory construction based on key here.
                        _cache.Add(key, val);
                    }
                }
            }
            return val;
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
