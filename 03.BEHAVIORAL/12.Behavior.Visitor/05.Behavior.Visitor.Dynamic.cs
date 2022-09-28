using System;
using System.Collections.Generic;
using System.Text;
using static System.Console;

namespace DotNetDesignPatternDemos.Behavioral.Visitor.Dynamic
{
    /*
     * Adesso che abbiamo visto un modello di progettazione classico per il Visitor
     * ed abbiamo visto come usarlo anche con la possibilità di farci restiuire per
     * ogni nodo visitato nella gerarchia sempre che sia omogenea un risultato tipitizzato.
     * Ci chiederemo perchè non rendere il codice più facile da aggregare durante l'attraversamento
     * dei nodi nella gerarchia adottando un interfaccia che li preveda come l'ITrasnformer visto
     * in precedenza e l'IVisitor o IExpressionVisitor nell'altro esempio, come ad esempio usare
     * l'override dei metodi necessari per ogni classe?
     * Bene con questo esempio ci sbarazziamo del costrutto per visitare la gerarchia ad alto livello
     * e di quello che implica per implementarla nelle sottoclassi.
     * L'esempio riporta l'uso incondizionato della keywork dynamica che ci solleva dal fatto di stabilire
     * a design time il tipo previsto in entrata e in uscita, ma è un operazione che è fatta preventivamente
     * dal programmatore che fà un cast nell'operazione di output che visita i nodi e questo cast deve
     * essere di quello previsto, se ci fosse qualche errore lo sapremmo solo a runtime.
     * Il vanataggio di questo approccio tramite questo trucco è buono perchè ti risparmia codice e 
     * valutazione del codice con letture un pò criptiche da valutare, ma per via del tipo dynamic l'accesso
     * ai membri in fase di runtime ed il cast da fare per ogni volta che si attravers è più lento rispetto alla
     * soluzione precedente. Non c'è modo di vedere se ci sono errori come nell'esempio precedente che implemneta
     * a code time l'intera infrastruttura tipitizzata per valutare e visitare i nodi delle espressisioni nella
     * gerarchia, ma eventuali errori lo si potranno vedere solo a runtime.
     * 
     */

    // Class Base 
    public abstract class Expression
    {
    }

    // Inherit Class
    public class DoubleExpression : Expression
    {
        public double Value;

        public DoubleExpression(double value)
        {
            Value = value;
        }
    }

    // Inherit Class
    public class AdditionExpression : Expression
    {
        public Expression Left;
        public Expression Right;

        public AdditionExpression(Expression left, Expression right)
        {
            Left = left ?? throw new ArgumentNullException(paramName: nameof(left));
            Right = right ?? throw new ArgumentNullException(paramName: nameof(right));
        }
    }

    // Class Visitor 
    public class ExpressionPrinter
    {
        public void Print(AdditionExpression ae, StringBuilder sb)
        {
            sb.Append("(");
            // In Modo dinamico tramite il DLR assume il vlore di left
            // nel nodo in valutazione e lo printa
            Print((dynamic)ae.Left, sb);
            sb.Append("+");
            // In Modo dinamico tramite il DLR assume il vlore di right
            // nel nodo in valutazione e lo printa
            Print((dynamic)ae.Right, sb);
            sb.Append(")");
        }

        // Il Metodo per ottenere un determinato output dall'uso
        // omogeneo tramite DLR di attraversare l'oggetto visitato
        // e otteneere un risultato
        public void Print(DoubleExpression de, StringBuilder sb)
        {
            sb.Append(de.Value);
        }
    }

    public class Demo
    {
        public static void Main706()
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

            // Il nostro Visitatore
            var ep = new ExpressionPrinter();
            var sb = new StringBuilder();

            // Il visitaroe che usa il comando per attraversare
            // l'albero delle espressioni e fare un output.
            // In questo caso con il DLR e la keyword dynamic
            // Attraversa l'intera gerarchia per il nodo di root
            // verso l'insieme gerarchico di vlutazione di ogni tipo.
            ep.Print((dynamic)e, sb);

            WriteLine(sb);

            // disadvantages:

            // 1) Performance penalty
            // 2) Runtime error on missing visitor
            // 3) Problematic w.r.t. inheritance
        }
    }
}