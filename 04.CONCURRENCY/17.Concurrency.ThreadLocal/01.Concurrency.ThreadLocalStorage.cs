using System;

namespace DotNetDesignPatternDemos.Concurrency.ThreadLocalStorage
{

    /*
     *  In Window
     *  La funzione API (Application Programming Interface) può 
     *  essere utilizzata per ottenere un indice di slot TLS inutilizzato; l'indice di slot TLS 
     *  sarà quindi considerato "usato". TlsAlloc
     *  Le funzioni and vengono quindi utilizzate per leggere e scrivere un indirizzo di memoria 
     *  in una variabile thread-local identificata dall'indice dello slot TLS. influisce solo 
     *  sulla variabile per il thread corrente. 
     *  La funzione può essere chiamata per rilasciare l'indice dello slot TLS. 
     *  TlsGetValueTlsSetValueTlsSetValueTlsFree
     *  
     *  È disponibile un blocco di informazioni sul thread Win32 per ogni thread. 
     *  Una delle voci in questo blocco è la tabella di archiviazione thread-local 
     *  per quel thread. TlsAlloc restituisce un indice a questa tabella, univoco per spazio 
     *  di indirizzi, per ogni chiamata. 
     *  Ogni thread ha la propria copia della tabella di archiviazione locale del thread. 
     *  Pertanto, ogni thread può utilizzare in modo indipendente TlsSetValue(index) 
     *  e ottenere il valore specificato tramite TlsGetValue(index), perché questi impostano 
     *  e cercano una voce nella tabella del thread.
     *  
     *  Oltre alla famiglia di funzioni TlsXxx, gli eseguibili di Windows possono definire una 
     *  sezione mappata a una pagina diversa per ogni thread del processo in esecuzione. 
     *  A differenza dei valori TlsXxx, queste pagine possono contenere indirizzi arbitrari e 
     *  validi. Questi indirizzi, tuttavia, sono diversi per ogni thread in esecuzione e 
     *  pertanto non devono essere passati a funzioni asincrone (che possono essere eseguite 
     *  in un thread diverso) o altrimenti passati al codice che presuppone che un indirizzo 
     *  virtuale sia univoco all'interno dell'intero processo. 
     *  Le sezioni TLS sono gestite utilizzando il paging della memoria e la sua dimensione 
     *  è quantizzata in una dimensione di pagina (4 kB su macchine x86). 
     *  Tali sezioni possono essere definite solo all'interno di un eseguibile principale 
     *  di un programma - le DLL non devono contenere tali sezioni, perché non vengono 
     *  inizializzate correttamente durante il caricamento con LoadLibrary. 
     * 
     * Implementazione di Pthreads
     * Nell'API Pthreads, la memoria locale di un thread è designata con il termine dati 
     * specifici del thread.
     * 
     * Le funzioni e vengono utilizzate rispettivamente per creare ed eliminare una chiave 
     * per i dati specifici del thread. 
     * Il tipo di chiave viene esplicitamente lasciato opaco e viene definito. 
     * Questa chiave può essere vista da tutti i thread. 
     * 
     * In ogni thread, la chiave può essere associata a dati specifici del thread tramite. 
     * I dati possono essere successivamente recuperati utilizzando. 
     * pthread_key_createpthread_key_deletepthread_key_tpthread_setspecificpthread_getspecific
     * 
     * Inoltre, facoltativamente, è possibile accettare una funzione distruttrice che 
     * verrà chiamata automaticamente all'uscita del thread, se i dati specifici del 
     * thread non sono NULL. Il distruttore riceve il valore associato alla chiave come 
     * parametro in modo da poter eseguire azioni di pulizia (connessioni chiuse, memoria 
     * libera, ecc.). Anche quando viene specificato un distruttore, il programma deve 
     * comunque chiamare per liberare i dati specifici del thread a livello di processo 
     * (il distruttore libera solo i dati locali nel thread). 
     * pthread_key_createpthread_key_delete
     * 
     */

    /* C#
     * Linguaggi .NET: C# e altri
     * 
     * Nei linguaggi .NET Framework, ad esempio C#, i campi statici possono essere 
     * contrassegnati con l'attributo ThreadStatic:

        class FooBar
        {
            [ThreadStatic]
            private static int _foo;
        }

    * In .NET Framework 4.0 la classe System.Threading.ThreadLocal<T> 
    * è disponibile per l'allocazione e il caricamento pigro di variabili thread-local.

        class FooBar
        {
            private static System.Threading.ThreadLocal<int> _foo;
        }

    * È inoltre disponibile un'API per l'allocazione dinamica di variabili thread-locali.

    */

    /*
     * C++

     * In C11, la parola chiave viene utilizzata per definire le variabili thread-local. L'intestazione , se supportata, definisce come sinonimo di quella parola chiave. Esempio di utilizzo: _Thread_local<threads.h>thread_local

        #include <threads.h>
        thread_local int foo = 0;
    
    * C++ 11 introduce la parola chiave che può essere utilizzata nei seguenti casi thread_local

    * Variabili a livello di spazio dei nomi (globale)
    * Variabili statiche dei file
    * Variabili statiche di funzione
    * Variabili membro statiche

    * A parte questo, varie implementazioni del compilatore forniscono modi specifici 
    * per dichiarare le variabili thread-local:

    *   - Monolocale Solaris C/C++, IBM XL C/C++,[3] GNU C,[4] Clang[5] e Intel C++ Compiler (sistemi Linux)[6] usano la sintassi:
        __thread int number;

    *   - Visual C++,[7] Intel C/C++ (sistemi Windows),[8] C++Builder e Digital Mars C++ utilizzano la sintassi:
        __declspec(thread) int number;

    *   - C++Builder supporta anche la sintassi:
        int __thread number;

    * Nelle versioni di Windows precedenti a Vista e Server 2008, funziona nelle DLL solo 
    * quando tali DLL sono associate all'eseguibile e non funzionerà per quelle caricate 
    * con LoadLibrary() (potrebbe verificarsi un errore di protezione o un danneggiamento dei dati). 
        __declspec(thread)
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
