using System;
using System.Collections.Generic;
using System.Text;
using static System.Console;


namespace DotNetDesignPatternDemos.Behavioral.Visitor.DoubleDispatch
{
    /*
     *  Qui iniziamo con un esempio di doppio messaggio chiamato appunto DoubleDispatch
     *  dove viene implementato concretamente il modello di progettazione Visitor.
     *  In questo caso senza usare una tabella dei metodi da trattare in reflection adottiamo
     *  il pattern vero e prorio e il metodo cardine della progettazione è in questo caso
     *  l'Accept che prende come argomento una possibile classe che implementa in sè l'inteerfaccia
     *  IExpressionVisitor che nell'esempio è ExpressionPrinter per attraversare la gerarchia delle
     *  strutture gerarchiche istanziate per la costruzione di una espressione per l'esempio corrente.
     *  E come vediamo anche una classe successiva come ExpressionCalculator che eredita anche lei i
     *  metodi dell'interfaccia IExpressionVisitor possa effettiavamente fare le stesse mosse come 
     *  fa il printer per attraversare le strutture gerarchiche e ottenere un risultato, in questo caso 
     *  un calcolo e non una print.
     *  Una differenza sostanziale rispetto all'emepio precedetne con le lambda inserite per ogni tipo
     *  nel dictionary apporpriato, qui non dobbiamo ogni volta controllare il tipo e prelevare la lamda
     *  necessaria per l'operazione da eseguire. 
     *  Grazie a questo modo di ottenere generalisticamente un attraversamento delle strutture dati è ipoteticamente
     *  possibile farlo in qualsiasi parte del programma per qualsiasi tipo che usa queste strutture senza
     *  preoccuparsi di dover costruire esternamente nuovamente il modo di attraversarle.
     *  Quindi la classe base Expression deve prevedere una sola volta che questa classe è soggetta alla
     *  possibile iterazione di oggetti esterni che vogliono interrogare l'intera sua struttura gerarchica
     *  di classi derivate, e lo fa con una firma in cui accetta come segnaposto iniziale un tipo di 
     *  interfaccia che detiene l'elenco di tutti i derivati in questo caso IExpressionVisitor che mantiene
     *  un override della stessa funzione Visit(...) per ogni tipo previsto che vuole usare questa struttura
     *  gerarchica. Il vantaggio è anche che tutto il modello è disegnato in fase di compilazione e non
     *  a runtime, dove possiamo preventivamente vedere se manca qualcosa o da sistemare.
     *  Questo modello è preventivamente un pò intrusivo, nel senso che c'è da apportare in tutte le classi
     *  derivate la funzione di visita dell'istanza, ma è da fare una sola volta e non devi aggiungere altro.
     */

    // Interfaccia per modello Visitor
    public interface IExpressionVisitor
    {
        // In Override su tutti i tipi di dati strutturati
        // e gerarchici come argomento di Accept per disporre
        // di una doppia comunicazione.
        // Sulle classi che esternamente vogliono attraversare
        // queste strutture dati diverse tra di loro ne imlementano
        // i metodi adottandola ome nell'esempio del Printer e del Calculator.

        void Visit(DoubleExpression de);
        void Visit(AdditionExpression ae);
    }


    // Class base con il pattern Visitor
    public abstract class Expression
    {
        public abstract void Accept(IExpressionVisitor visitor);
    }

    // Inherit Class
    public class DoubleExpression : Expression
    {
        public double Value;

        public DoubleExpression(double value)
        {
            Value = value;
        }

        public override void Accept(IExpressionVisitor visitor)
        {
            // double dispatch
            visitor.Visit(this);
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

        public override void Accept(IExpressionVisitor visitor)
        {
            // double dispatch
            visitor.Visit(this);
        }
    }


    // Esempio di componente esterno per attraversare la struttura
    // dati gerarchica e ricavare una stringa da printare a video.
    public class ExpressionPrinter : IExpressionVisitor
    {
        StringBuilder sb = new StringBuilder();

        /***************** IExpressionVisitor *************/

        public void Visit(DoubleExpression de)
        {
            sb.Append(de.Value);
        }

        public void Visit(AdditionExpression ae)
        {
            sb.Append("(");
            // Nel caso di più valori visiti l'istanza
            // che ne setta i valori successivi sulla StringBuilder
            ae.Left.Accept(this);
            sb.Append("+");
            // E rifai la stessa operazione per la proietà successiva.
            ae.Right.Accept(this);
            sb.Append(")");
        }

        /***************** IExpressionVisitor *************/

        public override string ToString() => sb.ToString();
    }

    // Esempio di componente esterno per attraversare la struttura
    // dati gerarchica e ricavare un risultato
    public class ExpressionCalculator : IExpressionVisitor
    {
        public double Result;

        // what you really want is int Visit(...)

        /***************** IExpressionVisitor *************/

        public void Visit(DoubleExpression de)
        {
            // Nel caso del singolo valore puoi visitare
            // l'oggetto e prendere il campo che ti interessa
            Result = de.Value;
        }

        public void Visit(AdditionExpression ae)
        {
            // Nel caso di più valori devi
            // attraversare l'istanza e prendere il risultato
            ae.Left.Accept(this);
            var a = Result;
            // E ancora rifai la stessa operazione per l'altro valore
            ae.Right.Accept(this);
            var b = Result;
            // catturati i dati interessati esegui l'operazione appropriata
            Result = a + b;
        }

        /***************** IExpressionVisitor *************/

    }

    public class Demo
    {
        public static void Main703()
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
            
            // Usiamo il componente esterno per 
            // ottenere una stringa
            var ep = new ExpressionPrinter();
            
            // Attraversa l'insieme gerarchico componendo
            // la StringBuilder
            ep.Visit(e);
            WriteLine(ep.ToString());

            // Usiamo il componente esterno per 
            // ottenere un risultato
            var calc = new ExpressionCalculator();

            // Attraversa l'insieme gerarchico componendo
            // la StringBuilder
            calc.Visit(e);
            
            WriteLine($"{ep} = {calc.Result}");
        }
    }
}