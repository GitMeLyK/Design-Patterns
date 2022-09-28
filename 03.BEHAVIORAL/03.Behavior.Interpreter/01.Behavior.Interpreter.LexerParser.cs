using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static System.Console;

namespace DotNetDesignPatternDemos.Behavioral.Interpreter.Handmade
{

    /*
     * Come ci siamo detti l'iterprete fondamentalmente quello più comune
     * è formato da due parti il lexing e il parsing, il primo si occupa di 
     * tokenizzare la stringa o l'intero testo in entrata e il secondo prendere
     * l'insieme dei token costrutiti dal lexer per essere trattai converiti elaborati
     * e inseriti nel contesto di un applicazione orientata agli oggetti per 
     * fornire un accesso programmatico al contenuto nelle sue parti concrete.
     * In questo esemip viene trattata una stringa come ad esempio "(13+4)-(12+1)"
     * dove vengono riconosciute le operazioni ognuna raggruppata tra parentesi
     * e convertita succesivamente in numeri interi per essere calcolati in una funzione
     * eval ottenendo il risultato del calcolo matematico.
     * Il lexer in questo caso leggendo carattere per carattere riprenderà quelli che sono
     * i caratteri previsti come () pr il raggruppamento + - per il tipo di operazione
     * e i valori numerici per la parte destra e sinistra del calcolo scartando tutto il resto.
     * Il parser instanzierà un oggetto di tipo BinayOperation valorizzandolo con quello che
     * è la sequenza dei Token ricevuti dal lexer e nel caso di () raggruppando operazione per operazione.
     * 
     */

    public interface IElement
    {
        int Value { get; }
    }

    public class Integer : IElement
    {
        public Integer(int value)
        {
            Value = value;
        }

        public int Value { get; }
    }

    // La classe usata per attribuire ad oggetto
    // una operazione matematica disposta da un valore
    // intero nella part sinistra un tipo di operazione al centro
    // ed un altro valore a destra per eseguire l'espressione.
    public class BinaryOperation : IElement
    {
        public enum Type
        {
            Addition,
            Subtraction
        }

        public Type MyType;
        public IElement Left, Right;

        public int Value
        {
            get
            {
                switch (MyType)
                {
                    case Type.Addition:
                        return Left.Value + Right.Value;
                    case Type.Subtraction:
                        return Left.Value - Right.Value;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }

    // La classe usata dal lexer prima e poi dal parser
    // per identificare gli elementi letterali che vengono
    // catturati nella stringa del calcolo matematico da eseguire
    public class Token
    {
        public enum Type
        {
            Integer, Plus, Minus, Lparen, Rparen
        }

        public Type MyType;
        public string Text;

        public Token(Type type, string text)
        {
            MyType = type;
            Text = text ?? throw new ArgumentNullException(paramName: nameof(text));
        }

        public override string ToString()
        {
            return $"`{Text}`";
        }
    }

    public class Demo
    {
        // L'operazione di lettura della sringa carattere per 
        // carattere e catturare quelli che sono caratteri numerici
        // da caratteri di tipo di operazione da quelli che sono i 
        // caratteri di raggruppamento.
        static List<Token> Lex(string input)
        {
            var result = new List<Token>();

            for (int i = 0; i < input.Length; i++)
            {
                switch (input[i])
                {
                    case '+':
                        result.Add(new Token(Token.Type.Plus, "+"));
                        break;
                    case '-':
                        result.Add(new Token(Token.Type.Minus, "-"));
                        break;
                    case '(':
                        result.Add(new Token(Token.Type.Lparen, "("));
                        break;
                    case ')':
                        result.Add(new Token(Token.Type.Rparen, ")"));
                        break;
                    default:
                        var sb = new StringBuilder(input[i].ToString());
                        for (int j = i + 1; j < input.Length; ++j)
                        {
                            if (char.IsDigit(input[j]))
                            {
                                sb.Append(input[j]);
                                ++i;
                            }
                            else
                            {
                                result.Add(new Token(Token.Type.Integer, sb.ToString()));
                                break;
                            }
                        }
                        break;
                }
            }

            return result;
        }

        // L'operazione di Parsing dei token catturati per contestualizzare
        // la funzione matematica che era stata espressa e ottenere un oggetto
        // BinaryOperation da usare per il calcolo finale.
        static IElement Parse(IReadOnlyList<Token> tokens)
        {
            var result = new BinaryOperation();
            bool haveLHS = false;
            for (int i = 0; i < tokens.Count; i++)
            {
                var token = tokens[i];

                // look at the type of token
                switch (token.MyType)
                {
                    case Token.Type.Integer:
                        var integer = new Integer(int.Parse(token.Text));
                        if (!haveLHS)
                        {
                            result.Left = integer;
                            haveLHS = true;
                        }
                        else
                        {
                            result.Right = integer;
                        }
                        break;
                    case Token.Type.Plus:
                        result.MyType = BinaryOperation.Type.Addition;
                        break;
                    case Token.Type.Minus:
                        result.MyType = BinaryOperation.Type.Subtraction;
                        break;
                    case Token.Type.Lparen:
                        int j = i;
                        for (; j < tokens.Count; ++j)
                            if (tokens[j].MyType == Token.Type.Rparen)
                                break; // found it!
                                       // process subexpression w/o opening (
                        var subexpression = tokens.Skip(i + 1).Take(j - i - 1).ToList();
                        var element = Parse(subexpression);
                        if (!haveLHS)
                        {
                            result.Left = element;
                            haveLHS = true;
                        }
                        else result.Right = element;
                        i = j; // advance
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            return result;
        }

        public static void Main()
        {
            var input = "(13+4)-(12+1)";
            var tokens = Lex(input);
            WriteLine(string.Join("\t", tokens));

            var parsed = Parse(tokens);
            WriteLine($"{input} = {parsed.Value}");
        }
    }
}