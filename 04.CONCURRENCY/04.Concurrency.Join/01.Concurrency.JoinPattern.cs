using System;

namespace DotNetDesignPatternDemos.Concurrency.Join
{
    /*
     * Il join-pattern (o un accordo in Cω) è come una super pipeline con sincronizzazione e 
     * corrispondenza. In effetti, questo concetto viene riassunto per corrispondenza e si unisce 
     * a un set di messaggi disponibili da diverse code di messaggi, quindi li gestisce tutti 
     * contemporaneamente con un unico gestore. [1] 
     * Potrebbe essere rappresentato dalle parole chiave per specificare la prima comunicazione 
     * che ci aspettavamo, con il per unire/accoppiare altri canali e l'esecuzione di alcune 
     * attività con i diversi messaggi raccolti. 
     * Un modello di join costruito assume in genere questa forma: when and do 
     *  j.When(a1).And(a2). ... .And(an).Do(d)
     * L'argomento a1 di può essere un canale sincrono o asincrono o una matrice di canali 
     * asincroni. Ogni argomento successivo ai a (per ) deve essere un canale asincrono. 
     * 
     * Più precisamente, quando un messaggio corrisponde a una catena di modelli collegati fa 
     * sì che il suo gestore venga eseguito (in un nuovo thread se si trova in un contesto 
     * asincrono), altrimenti il messaggio viene accodato fino a quando uno dei suoi modelli 
     * non è abilitato; se ci sono più corrispondenze, viene selezionato un modello non 
     * specificato. 
     * A differenza di un gestore eventi, che gestisce uno dei numerosi eventi alternativi 
     * alla volta, in combinazione con tutti gli altri gestori di tale evento, un modello 
     * di join attende una congiunzione di canali e compete per l'esecuzione con qualsiasi 
     * altro modello abilitato.
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
