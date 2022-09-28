using System;
using System.Text;
using static System.Console;

namespace DotNetDesignPatternDemos.Behavioral.Visitor.Intrusive
{
    /*
     *  In questo esempio non viene proprosto un modello di Visitor reale 
     *  ma è solo un esempio senza Modello Visitor che stampa un espressione
     *  matematica. Quindi diciamo come questo problema si risolverebbe con
     *  codice non modellato intorno alla progettazione di un Visitor.
     *  Quindi immaginiamo di avere espressimi matematiche compostose solo
     *  da numeri e addizioni di altri numeri, e definiamo una sorta di gerarchia
     *  per definire come funzionano queste espressioni. La classe astratta avrà
     *   un singolo metodo public per stampare l'espressione chiamato Print()
     *   che sarà tramite stringbuilder un insieme di stringhe elaborate a mo di 
     *   espressione da presentare a video.
     *  Le classi che eredinato la base Expression avranno il loro Print parziale
     *  fino al print Completo nella AdditionExpression ultima della gerarchia.
     *  Vedremo quindi nella composizione come il modello gerarchico i occupi di formulare
     *  l'espressione da renderizzare innestando uno dentro l'altro le istanze coinvolte.
     *  In qeusto esempio abbiamo potuto operare avendo inserito nella classe base
     *  il metodo Print() di tipo abstract e forzando le altre classi a reimplementarlo
     *  e dobbiamo immaginare quelle situazioni in cui questa opzione non è disponibile
     *  e non hai quini il metodo di Print() e la domandda ' come fare in assenza di questo?
     *  come fare che lo schema del Visitatore sia tutto relativo all'aggiunta di funzionalità
     *  aggiuntive quando le gerarchie sono già importate e non puoi entrare negli stessi
     *  membri e modificarli?
     */

    // Class base
    public abstract class Expression
    {
        // adding a new operation
        public abstract void Print(StringBuilder sb);
    }

    // Inherit Class
    public class DoubleExpression : Expression
    {
        private double value;

        public DoubleExpression(double value)
        {
            // Valore Double da accodare
            this.value = value;
        }

        public override void Print(StringBuilder sb)
        {
            sb.Append(value);
        }
    }

    // Inherit Class
    public class AdditionExpression : Expression
    {
        private Expression left, right;

        // Ogni espressione è formulata da due espressioni ulteriori innestate.
        public AdditionExpression(Expression left, Expression right)
        {
            // Aggiunta valore destro e sinistro per il +
            this.left = left ?? throw new ArgumentNullException(paramName: nameof(left));
            this.right = right ?? throw new ArgumentNullException(paramName: nameof(right));
        }

        public override void Print(StringBuilder sb)
        {
            // Espressione completa
            sb.Append(value: "(");
            left.Print(sb);
            sb.Append(value: "+");
            right.Print(sb);
            sb.Append(value: ")");
        }
    }

    public class Demo
    {
        private static void Main701(string[] args)
        {
            // Vediamo come in modo intrusivo innestiamo
            // nella classe ultima della gerarchia gli 
            // elementi che dovranno in modo gerarchico 
            // innestarsi uno dentro l'altro.
            var e = new AdditionExpression(
              left: new DoubleExpression(1),
              right: new AdditionExpression(
                left: new DoubleExpression(2),
                right: new DoubleExpression(3)));
            var sb = new StringBuilder();

            // Ecco quindi il risultato di tutta la
            // soluzione gerarchica delle classi innestate
            // per ottenere tramite StringBuilder il nostro risultato.
            e.Print(sb);
            WriteLine(sb);

            // what is more likely: new type o rnew operation
        }
    }
}