using System;
using System.ComponentModel;
using System.Transactions;

namespace DotNetDesignPatternDemos.Behavioral.Visitor.ReductionTransform
{
    /*
     *  Una domanda rispetto all'esempio precedente è se potresti avere sul
     *  modello del visitatore è come mai quei metodi di accettazione e visita
     *  non restituiscono mai un valore?
     *  Bene c'è una diversa modalità di costruzione del modello di progettazione
     *  con un Visitator che fà in modo simile ad un operazione di riduzione, fondamentale
     *  per piegare una struttura ad albero ad un singolo elemento e assumendo che l'elemento
     *  sia omogeneo che significa che è dello stesso tipo dappertutto.
     *  Anche se le aterminoliga cambia in questo caso invece di IExpressionVisitor abbiamo ITransformer<T>
     *  ma che fondamentalmente non differisce dove raccoglie tutti i tipi che possono essere
     *  visitati nell'insieme gerarchico e i metodi invece di accettare un Visititor perleremo
     *  di ridurre e trasformare le operazioni come prima.
     *  Per farla breve il Visitator questa volta restituisce qualcosa di un determinato tipo T
     *  per ogni volta che si esegue il metodo nell'espressione Reduce, questo perchè è il pattern 
     *  Transformer che fa in modo che venga restutito il tipo T dopo aver effettuato una trasromazione
     *  sul valore come ad esemio T di tipo double resituie un double magari dopo calcolato, ma torna
     *  utile se deve restituire un tipo ExpressionAddtitional ulteriore trasformandolo con un nuovo set
     *  di valori per il tipo T revisto etc, e questo sempre con il modello di progettazione corrente
     *  di visitare l'intera gerarchia sempre tramite il metodo predefinito per attraversare questo
     *  struttura dati pur diversa tra di loro.
     */

    // Interfaccia per modello Visitor ma con il pattern Transformer
    public interface ITransformer<T>
    {
        T Transform(DoubleExpression de);
        T Transform(AdditionExpression ae, T left, T right);
    }


    // Class base con il pattern Visitor 
    public abstract class Expression
    {
        // Questa volta invece di avere un metodo Accept avremo un metodo di Reduce
        // e oltretutto il metodo è un metodo generico
        public abstract T Reduce<T>(ITransformer<T> transformer);
    }

    // Inherit Class
    public class DoubleExpression : Expression
    {
        public readonly double Value;

        public DoubleExpression(double value)
        {
            Value = value;
        }

        // Il metodo di riduzione per un determinato tipo
        public override T Reduce<T>(ITransformer<T> transformer)
        {
            return transformer.Transform(this);
        }
    }

    // Inherit Class
    public class AdditionExpression : Expression
    {
        public readonly Expression Left, Right;

        public AdditionExpression(Expression left, Expression right)
        {
            Left = left;
            Right = right;
        }

        // Il metodo di riduzione per un determinato tipo
        public override T Reduce<T>(ITransformer<T> transformer)
        {
            var left = Left.Reduce(transformer);
            var right = Right.Reduce(transformer);
            // Per utilizzare un transformer è possibile trasportando tutti i valori
            // nello stack quindi argomentati per Restutire un oggetto T valorizzato
            // appunto dopo essere stato trasformato in un oggetto con valori.
            return transformer.Transform(this, left, right);
        }
    }


    // Esempio di componente esterno per attraversare la struttura
    // dati gerarchica e ricavare una trasformazione per la valutazione.
    public class EvaluationTransformer : ITransformer<double>
    {
        public double Transform(DoubleExpression de) => de.Value;

        // prevede lo stack completo negli argometi perchè chiamato 
        // per questo calcolo dall'annidamento successivo nelle gerarchia
        // in uso nel metodo Reduce di AdditionExpression
        public double Transform(AdditionExpression ae, double left, double right)
        {
            // La trasfomazione stavolta per restituire un valore double al visitatore
            return left + right;
        }
    }

    // Esempio di componente esterno per attraversare la struttura
    // dati gerarchica e ricavare una trasformazione per la print trasformata.
    public class PrintTransformer : ITransformer<string>
    {
        public string Transform(DoubleExpression de)
        {
            return de.Value.ToString();
        }

        // prevede lo stack completo negli argometi perchè chiamato 
        // per questa stringa dall'annidamento successivo nelle gerarchia
        // in uso nel metodo Reduce di AdditionExpression
        public string Transform(AdditionExpression ae, string left, string right)
        {
            // La trasformazione stavolta per restituirre una stringa al visitatore
            return $"({left} + {right})";
        }
    }

    // Esempio di componente esterno per attraversare la struttura
    // dati gerarchica e ricavare una trasformazione per la valutazione particolare.
    public class SquareTransformer : ITransformer<Expression>
    {
        public Expression Transform(DoubleExpression de)
        {
            return new DoubleExpression(de.Value * de.Value);
        }

        // prevede lo stack completo negli argometi perchè chiamato 
        // per questo nuovo annidamento dall'annidamento successivo nelle gerarchia
        // in uso nel metodo Reduce di AdditionExpression
        public Expression Transform(AdditionExpression ae, Expression left, Expression right)
        {
            // La trasformazione stavolta per restituirre una nuovo oggetto trasformato di tipi espressione al visitatore
            return new AdditionExpression(left, right);
        }
    }

    public class Program
    {
        static void Main705()
        {
            var expr = new AdditionExpression(
              new DoubleExpression(1), new DoubleExpression(2));
            
            // Riduce e trasforma ad un Double che è il risultato
            var et = new EvaluationTransformer(); // 3
            var result = expr.Reduce(et);

            // Riduce e trasforma ad una Stinga (1 + 2)
            var pt = new PrintTransformer();
            var text = expr.Reduce(pt);
            Console.WriteLine($"{text} = {result}"); // (1 + 2) = 3

            // Riduce e trasforma ad una nuova Espressione da attraversare
            var st = new SquareTransformer();
            var newExpr = expr.Reduce(st);

            // Riduce e trasforma per la valutazione rispetto al risultato
            // precedete dell'espressione annidata nuova
            var et2 = new EvaluationTransformer(); // 5
            var result2 = newExpr.Reduce(et);

            // Riduce e trasforma l'epressione del print
            // per la nuova espressione restiuita precedente 
            text = newExpr.Reduce(pt);

            Console.WriteLine($"{text} = {result2}"); // (1 + 4) = 5

        }
    }
}
