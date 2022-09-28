using System.Collections.Generic;
using System.Text;
using static System.Console;

namespace DotNetDesignPatternDemos.Structural.Flyweight.TextFormatting
{

    /*
     * In questo esempio viene riportato come operando
     * su una strigna immutabile già in memoria possiamo
     * ottimizzare il contesto sfruttando già quello che
     * preventivamente già da qualche altra parte abbiamo
     * conservato.
     * Per capire meglio nel primo caso del FormattedText
     * indicizziamo nella stringa del costruttore la lunghezza della stringa
     * e nel metodo capitalize definiamo quale nell'indice dei caratteri che la
     * compongono vogliamo che sia in output renderizzato con un carattere maiuscolo.
     * Quindi il ToString() trmiate  lo stringbuildr farà sb.Append(capitalize[i] ? char.ToUpper(c) : c);
     * rispetto a ogni indice della sequenz un controllo se a true e formatterà il testo con quella lettera
     * in maiuscolo, questo per via dei Flag che usiamo per sapere quale lettera o lettere vogliamo
     * che siano maiuscole tramite il valore boolean impoistato occupano memoria. In un tipico editor
     * di testo basti pensare ad una stringa dove ogni singolo carattere può essere bold italico Maiuscolo...
     * e incorriamo in un dispendio di memeoria tanto per quanto lungo è il testo e i caratteri in esso
     * contenuti moltiplicato per il numero dei flag..
     * Nel secondo Caso BetterFormatText, cosa succede fondamentalmente, che piuttosto che
     * aspettarsi che tutti i caratteri abbiano una qualche flag di formattazione ci concentriamo sulle
     * sequenze di parole contigue, e cioè conserviamo il range di ogni formattazione applicata sul testo
     * per meglio dire un bold può partire dal 10 carattere della stringa e finire al 15, quindi scorro
     * nel testo per raggiungere dove applicare il bold a quell'insieme. Il che si traduce in un ottimizzazione
     * vera e propria di un editor di testo.
     * Quindi abbiamo usato lo stesso approccio utilizzando il modello FlyWheight risolutivo con gli intervalli.
     */

    // Non ottimizzato
    public class FormattedText
    {
        private readonly string plainText;

        public FormattedText(string plainText)
        {
            this.plainText = plainText;
            capitalize = new bool[plainText.Length];
        }

        public void Capitalize(int start, int end)
        {
            for (int i = start; i <= end; ++i)
                capitalize[i] = true;
        }

        private readonly bool[] capitalize;

        public override string ToString()
        {
            var sb = new StringBuilder();
            for (var i = 0; i < plainText.Length; i++)
            {
                var c = plainText[i];
                sb.Append(capitalize[i] ? char.ToUpper(c) : c);
            }
            return sb.ToString();
        }
    }

    // Ottimizzato 
    public class BetterFormattedText
    {
        private readonly string plainText;
        private readonly List<TextRange> formatting = new ();

        public BetterFormattedText(string plainText)
        {
            this.plainText = plainText;
        }

        public TextRange GetRange(int start, int end)
        {
            var range = new TextRange { Start = start, End = end };
            formatting.Add(range);
            return range;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            for (var i = 0; i < plainText.Length; i++)
            {
                var c = plainText[i];
                foreach (var range in formatting)
                    if (range.Covers(i) && range.Capitalize)
                        c = char.ToUpperInvariant(c);
                sb.Append(c);
            }

            return sb.ToString();
        }

        // Il range di caratteri nell'intero testo che 
        // contiene il tipo di formattazione/i da applicare.
        public class TextRange
        {
            public int Start, End;
            public bool Capitalize, Bold, Italic;

            public bool Covers(int position)
            {
                return position >= Start && position <= End;
            }
        }
    }

    public class Demo
    {
        static void Main(string[] args)
        {
            // Non ottimizzato
            var ft = new FormattedText("This is a brave new world");
            ft.Capitalize(10, 15);
            WriteLine(ft);

            // ottimizzato
            var bft = new BetterFormattedText("This is a brave new world");
            bft.GetRange(10, 15).Capitalize = true;
            WriteLine(bft);
        }
    }
}