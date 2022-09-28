using System;
using System.Collections.Generic;
using System.Text;
using static System.Console;

namespace DotNetDesignPatternDemos.Behavioral.Visitor.Acylic
{
    /*
     * Rispetto a un modello di progettazione classico per il Visitor
     * presentato nell'esempio precedente trasformiamo il modo di implementarlo
     * tramite un iterfaccia comune che si usa come geric per un tipo 
     * passato alla definizione della classe che eredita e usa l'interfaccia Visitor.
     * Questo perchè potrebbe in determinate situazioni risultare difficile modificare
     * questo comportamento se si dovessero aggiungere elementi alla gerarchia di classi
     * già esistenti. Per esempio si dispone di un progetto che ha la sua classe base
     * con le classi ereditate e vediamo con il modello classico che siamo legati a
     * ad aggiungere molto codice ripetitivo nelle sotto classi per farlo funzionare
     * e quindi si rende necessario per ogni classe della gerarchia o di classi da usare
     * in seguito ad un altra implementazione aggiungere tutto il codice per il metodo
     * Accept() peer usare l'handle in modo corretto che attraversa tutto l'albero delle
     * espressioni nella gerarchia e in Aggiunta hai bisogno del metodo Visit() per 
     * ogni singolo elemento in override nell'interfaccia prevista per questo modello, e tutti
     * gli accesori che usano il visit devnon implementare per tutti i tipi anche quelli non previsti.
     * Quindi per fare un esempio se aggiungi una classe nuova navigabile come sotto classe di 
     * quella base per potrla usare nel odello visit ti tocca aggiungere il metodo all'interfaccia
     * anche per questo tipo e riprendere tutte le classi Visit() oltre che per la classe aggiungere
     * il metodo Accept() completamente implementato.
     * Ecco perchè in questo esempio vediamo un Visitatore Ciclico e come si applica, e il trucco
     * di come usare un interfaccia marker per segnaposto dei tipi previsti per la classe accessorio
     * che vuole usare il Visit() solo per le tipologie realmente coinvolte.
     * L'interfaccia marker non ha metodi e serve solamente per indicare che un certo tipo di classe
     * derivata è un Visitatore.
     * A questo punto la classe base avrà un unico metodo già implementato chee non è abstract per il metodo
     * Accept ma virtual e che con l'argomento Marker stabilisce che il tipo è di tipo marcato come IVisitor e 
     * quindi l'espressione può essere risolta durante la navigazione nella gerarchia dalla classe che ne fa uso.
     * Nel caso di Double Dispatcher come nell'esempio precedente vediamo come generalizzando ad alto
     * livello e definendo per il tipo un marker che ne attribuisce che le derivate sono Visitor abbiamo
     * scritto meno codice del necessario rispetto al precedente, e abbiamo fatto in modo che per eventuali
     * altre nuove implementazioni di classi dderivate non dobbiamo implementarle obbligatoriamente per non 
     * fargli fare niente perchè sarà il metodo astratto della classe base a sopperire a queste eccezioni durante
     * il ciclo che fa il visitatore nella gerarchia delle espressioni sottostanti.
     * In questo modo ci libera dal dover per forza specificare quale tipo particolare è il visitante e per
     * ognuno applicare una convenzione ma semplicemente prendere il visitatore e applicare.
     * Nel Main per fare un ricapitolo di test di questo modo di scrivere il visitatore, non implementiamo una
     * qualunque interfaccia per quel tipo visitor, al momento dell'output non avremo errori ma semplicemente non
     * avremo il risultato di quei tipi di espressione nella gerarchia.
     * Questa è un implementazione alternativa del visitatore che può essere rilevante in alcuni scenari di sviluppo
     * dove è più efficente anche in termini di prestazioni.
     */

    // Interface for Model Visitor questa volta senza
    // identificare tutti i tipi previsti in Override
    // per le classi che prevedono questa Metodologia.
    // L'unica cosa che devono avere le classi è il tipo
    // da passare in modo Generic a questa implmentazione.
    public interface IVisitor<TVisitable>
    {
        void Visit(TVisitable obj);
    }

    public interface IVisitor { } // marker interface

    // Class Base Con Accept per tutti già implementato tramite il marker
    public abstract class Expression
    {
        // Funzione accessoria implementata per il modello Visitor corrente
        // Il metodo corrente è astratto nel senso che non fa interrompere la normale
        // operazione ciclica che effettua il visitatore sulla gerarchia e così non
        // dobbiamo preoccuparci se una delle classi derivate non lo implementa, semlicemente
        // non farà nulla per quella sezione.
        public virtual void Accept(IVisitor visitor)
        {
            // Il Marker a questo punto è identificabile per il tipo Visitor
            // che usa il metodo perchè è stato identificato come tale.
            if (visitor is IVisitor<Expression> typed)
                typed.Visit(this);
        }
    }

    // Inherit Class
    public class DoubleExpression : Expression
    {
        public double Value;

        public DoubleExpression(double value)
        {
            Value = value;
        }

        // Riscrittura dell'operzione da un quanlunque visitatore a patto
        // che il visitatore sia di questo tipo di classe
        // *** se si toglie il commento e quindi si riscrive la funzione
        //     otteniamo la visita in questo tipo di classe altrimenti non viene valutata.
        /*
        public override void Accept(IVisitor visitor)
        {
            // Ma controllando che il tipo sia omogeneo per questa espressione
            if (visitor is IVisitor<DoubleExpression> typed)
                typed.Visit(this);
        }
        */
    }

    // Inherit Class
    public class AdditionExpression : Expression
    {
        public Expression Left;
        public Expression Right;

        public AdditionExpression(Expression left, Expression right)
        {
            Left = left;
            Right = right;
        }

        // Riscrittura dell'operzione da un quanlunque visitatore a patto
        // che il visitatore sia di questo tipo di classe
        // Ogni espressione è formulata da due espressioni ulteriori innestate.
        public override void Accept(IVisitor visitor)
        {
            // Ma controllando che il tipo sia omogeneo per questa espressione
            if (visitor is IVisitor<AdditionExpression> typed)
                typed.Visit(this);
        }
    }

    // Class Visitor pr accessory Print, promiscua
    // per i tipi previsti di espressione usati nella
    // visita ai nodi della gerarchia delle classi
    // sottostanti.
    public class ExpressionPrinter : IVisitor,
      IVisitor<Expression>,
      IVisitor<DoubleExpression>,
      IVisitor<AdditionExpression>
    {
        StringBuilder sb = new StringBuilder();

        public void Visit(DoubleExpression de)
        {
            sb.Append(de.Value);
        }

        public void Visit(AdditionExpression ae)
        {
            sb.Append("(");
            ae.Left.Accept(this);
            sb.Append("+");
            ae.Right.Accept(this);
            sb.Append(")");
        }

        public void Visit(Expression obj)
        {
            // default handler?
        }

        public override string ToString() => sb.ToString();
    }

    public class Demo
    {
        public static void Main()
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

            // La classe che Visiterà la gerarchia delle
            // classi per ottenere e trasformare i risultati
            // in una stringa presentata a video
            var ep = new ExpressionPrinter();

            // Avvia l'hadle della visitazione delle espressioni
            // previste er tutti i nodi della gerarchia delle classi
            // di espressione usate per comporre una Espressione.
            ep.Visit(e);

            // Il valore è catturato e presentato sottoforma di stringa a video.
            WriteLine(ep.ToString());
        }
    }
}