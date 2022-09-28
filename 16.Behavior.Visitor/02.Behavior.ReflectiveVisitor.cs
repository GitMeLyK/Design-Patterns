using System;
using System.Collections.Generic;
using System.Text;
using static System.Console;


namespace DotNetDesignPatternDemos.Behavioral.Visitor.Reflective
{
    /*
     *  Per rispondere quindi alla domanda precedente che richiedeva appunto
     *  l'uso di un modello di progettazione Visitor che possa fare in modo
     *  di creare il modello applicando anche in assenza delle funzioni accessorie
     *  un punto di ingresso valido per usare la navigazione tra strutture gerarchihe
     *  anche di diversa tipologia.
     *  In questo esempio come si vede rispetto all'esempio precedente abbiamo la 
     *  classe base Expression senza la funzione accesoria importante per il Print()
     *  e come si vede anche le classi concrete grarchiche per la formulazione dell'espressione 
     *  sono sprovvisti di questa funzione accessoria per il Print().
     *  In questo esempio affronteremo il problema completamente dal'esterno adottando
     *  le un Componente esterno che simuli la funzione mancate per sfruttare la composizione 
     *  arbitraria di codice inserito a runtimee rendere possibile l'uso della funzione Accessoria 
     *  per lo scopo Print().
     *  Il Print2() è quello proposto all'esterno che userà il Dictionary per attraversare tutte 
     *  le istanze gerarchiche di un terminato oggetto e usare le funzioni per printare i valori.
     *  E' importante notare che in questa tecnica la gerarchia tratta da un componente esterno
     *  viene ad essere non instrusiva nelle classi in quanto opera solo con le caratteristiche
     *  pubbliche delle classi e in questo esempio che per forza di cose che la proprietà Value
     *  deve essere rispetto all'esempio precedente di tipo Pubblica come anche le proprietà
     *  Left e Right della classe AdditionExpression.
     *  Quindi anche questo esempio ci riporta all'idea concreta che non stiamo usando un vero 
     *  e proprio pattern di Visitor che ci obbliga a vincoli di switch su classi che vogliamo
     *  attraversare, e quindi anche questo tentativo non rende propriamente pulito il modello
     *  pattern Visitor previsto, anche se migliore rispetto all'esempio precedente ma obbligandoci
     *  a rimodificare per gli attributi il loro comportamento conservativo in questo case, e anhe
     *  perchè non avrremmo potuto usare funzioni interne alle classi concrete perchè non raggiungibili.
     */

    // Tabella per i tipi di espressioni Dictionary con il tipo e la funzione accessoria
    using DictType = Dictionary<Type, Action<Expression, StringBuilder>>;

    // Class base senza la funzione accesoria Print()
    public abstract class Expression{}

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

        // Ogni espressione è formulata da due espressioni ulteriori innestate.
        public AdditionExpression(Expression left, Expression right)
        {
            Left = left ?? throw new ArgumentNullException(paramName: nameof(left));
            Right = right ?? throw new ArgumentNullException(paramName: nameof(right));
        }
    }


    // Class external per l'ausilio della funzione print mancante 
    public static class ExpressionPrinter
    {
        private static DictType actions = new DictType
        {
            // Aggiunge all'insieme un tipo DoubleExpression con questa espressione mancante Print()
            [typeof(DoubleExpression)] = (e, sb) =>
            {
                var de = (DoubleExpression)e;
                sb.Append(de.Value);
            },
            // Aggiunge all'insieme un tipo AdditionExpression con questa espressione mancante Print()
            [typeof(AdditionExpression)] = (e, sb) =>
            {
                var ae = (AdditionExpression)e;
                sb.Append("(");
                Print(ae.Left, sb);
                sb.Append("+");
                Print(ae.Right, sb);
                sb.Append(")");
            }
        };

        public static void Print2(Expression e, StringBuilder sb)
        {
            // Richiama la collection attraversandola per tutti i tipi
            // e ottenere il metodo ricostruito con i Print() per eseguire
            // le operazioni.
            actions[e.GetType()](e, sb);
        }

        // Per capire meglio come si comporta il Print2, se richiami questo
        // metodo su una singola espressione noterai che esistono due casi
        // per ogni elemento nel dichionary etichettate tramite la chiave del Tipo 
        // di classe concreta a cui applicare il metodo per eseguire il print().
        public static void Print(Expression e, StringBuilder sb)
        {
            if (e is DoubleExpression de)
            {
                sb.Append(de.Value);
            }
            else
            if (e is AdditionExpression ae)
            {
                sb.Append("(");
                Print(ae.Left, sb);
                sb.Append("+");
                Print(ae.Right, sb);
                sb.Append(")");
            }
            // breaks open-closed principle
            // will work incorrectly on missing case
        }
    }

    public class Demo
    {
        public static void Main702()
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

            // Ma questa volta tramite il componente esterno
            // che attraversa tutte le strutture è in grado 
            // di emulare la funzione Print() mancante.
            ExpressionPrinter.Print2(e, sb);
            
            WriteLine(sb);
        }
    }
}