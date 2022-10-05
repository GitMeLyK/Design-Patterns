using System;

namespace DotNetDesignPatternDemos.Architectural.Documental.Inline
{
    /*
     * Abbiamo definito per sommi capi come documentare in modo architetturale e 
     * istrutire in modo pragmatico il team ad adottare modelli per la documentazione
     * dell'infrastruttura fino al codice.
     * 
     * Nelle parti del codice però esiste a secondo del linguaggio di sviluppo utilizzato
     * una serie di regole che ne documentano fino anche al singolo metodo.
     * 
     * Per esempio in Java documentare un metodo è prassi usare il codice in questo modo.:
     * 
     * 
        /**
        * Returns an Image object that can then be painted on the screen. 
        * The url argument must specify an absolute <a href="#{@link}">{@link URL}</a>. The name
        * argument is a specifier that is relative to the url argument. 
        * <p>
        * This method always returns immediately, whether or not the 
        * image exists. When this applet attempts to draw the image on
        * the screen, the data will be loaded. The graphics primitives 
        * that draw the image will incrementally paint on the screen. 
        *
        * @param  url  an absolute URL giving the base location of the image
        * @param  name the location of the image, relative to the url argument
        * @return      the image at the specified URL
        * @see         Image
        * /
     * 
     *  tutte le specifiche si trovano al seguente indirizzo.:
     *  https://www.oracle.com/technical-resources/articles/java/javadoc-tool.html
     * 
     *  In .net invece 
     *  
     *  /// <summary>Class <c>Point</c> models a point in a two-dimensional
     *  /// plane.
     *  /// </summary>
     *  /// <example>For example:
     *  ///     <code>
     *  ///         Point p = new Point(3,5);
     *  ///         p.Translate(-1,3);
     *  ///         </code>
     *  ///         results in <c>p</c>'s having the value (2,8).
     *  /// </example>
     *  /// </summary>
     *  /// <param name="xor">the new x-coordinate.</param>
     *  /// <param name="yor">the new y-coordinate.</param>
     *  
     *  tutte le specifiche si trovano al seguente indirizzo.:
     *  https://learn.microsoft.com/it-it/dotnet/csharp/language-reference/language-specification/documentation-comments
     * 
     *  E' importante notare che ogni framework di linguaggio adottato usa una
     *  sintassi per definnire i contesti dell'elemento documentato per clssificare esempi
     *  di codice attributi parametri e descrizione.
     *  
     *  Con questo è possibile quindi usare strumenti esterni per raccogliere questo volume
     *  di informazioni per realizzare una documntazione tecinca completa del software sviluppato
     *  o creare delle regole per cui alcune parti possono documentare l'utilizzatore finale
     *  e altre più approfondite un eventale sdk da rilasciare.
     *  
     *  In .Net ma anche in altri linguaggi è possibile documentare in file esterni per non
     *  creare caos nel codice che deve essere chiaro adottando alcune accortezze.
     *  In .net per esempio è possibile documentare un attributo ma anche metodi eventi ed
     *  eccezioni in questo modo.:
     *  
     *  /// <include file="docs.xml" path='extradoc/class[@name="IntList"]/*' />
     *  public class IntList { ... }
     *  
     *  che punterà per la parte da completare come suggerimenti inline su visual studio o come
     *  processo documntale da redarre a un file esterno completo come questo ad esempio.:
     *  
     *  <?xml version="1.0"?>
     *  <extradoc>
     *    <class name="IntList">
     *       <summary>
     *          Contains a list of integers.
     *       </summary>
     *    </class>
     *    <class name="StringList">
     *       <summary>
     *          Contains a list of integers.
     *       </summary>
     *    </class>
     *  </extradoc>
     *
     * In questo modo i file di documentazione vero e proprio saranno accatastati in una 
     * unica posizione e non ingombrano il codice vero e prorpio che altrimenti porterebbe
     * ad essere fastidioso per chi sviluppa o fa analisi sul codice.
     * 
     *  Tools
     *  
     *      Questo tool permette appunto di scrivere i file di documentazione xml compatibili con .net
     *      in modo visuale ed esterno.
     *      https://www.innovasys.com/product/dx/features_dotnet?gclid=CjwKCAjws--ZBhAXEiwAv-RNLxkU9gzshDCyE8CFbHQ0A8JgrLwodp8VunW3ZrUZWB15UX8mCOIi1xoCyBsQAvD_BwE
     * 
     *      VSdocman Free
     *      
     *      DocFx Free
     *      
     *      Esistono poi tool per creare anche documntazione in formato ormai standard 
     *      markdown più adatte per il web e quindi fonte di documentazione non tecnica 
     *      da usare per utilizzatori finali e non contestuali a singoli processi del
     *      software.
     *      
     *      Un interessante progetto piuttosto che in markdown in asciidoc che è un formato
     *      valido alla pari se non superiore secondo me a markdown è usare asciidoctor.
     * 
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
