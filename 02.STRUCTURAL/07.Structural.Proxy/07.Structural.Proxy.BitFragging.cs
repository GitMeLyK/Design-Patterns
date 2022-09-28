using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;


namespace DotNetDesignPatternDemos.Structural.Proxy.BitFragging
{
    /*
     * Con questo esempio articolato ma da citare in casi in cui si ha che 
     * fare con valori di Bit su problemi matematici di un certo rilievo, dove
     * il risparmio di memoria in termini di prestazioni diventa essenziale.
     * Quando operaiamo con i bool per avere un flag sappiamo che non stiamo
     * operando in c# con un singolo bit ma con un costrutto mascherato che
     * definisce uno stato true o false. Quindi per operazioni dove è richiesto
     * l'uso di flag a singolo bit con motlitudine di flag questo tipo diventa
     * inproponibile occupando un sacco di risorse e degradando in prestazioni.
     * In questo esempio è posto un problema matematico dove si vuole che
     * un insieme di interi in sequenza sottoposti ad operazioni binarie qualifichi
     * come risultato 0 come problema risolto.
     * quindi avremo una sequesnza del tipo 1 2 3 4 ... 10 e voglio che sia 
     * sottoposto a usare operazioni di + - * / fino ad ottenere come risultato 0
     * quindi 1-3-5+7 diventa risolto con 0.
     * In questo esempio viene appunto risolto questo problema identificando una
     * classe OpImpl che presuppone implicitamente quali sono le operazioni binarie
     * prese da un enum ottenendo così internamente un elenco descritto degli 
     * operatori trattati e operando con un call a richiamare l'operazione tramite
     * il suo nome descrittivo tra il valore sinistro e il valore destro.
     * Usando poi una classe Problem che in sè contiene l'intera lista di interi 
     * passati come sequenza da sottoporre ai calcoli per ottenere il risultato desiderato.
     * Questo processare il problema avverrà quindi nella funzione cardine di eval che
     * detiene in modo composito la lista degli interi e la lista degli operatori e iterando
     * nella sequenza numerica si occupa di combinare con gli operatori fino ad ottenere 0 1 2 ...
     * Il punto dove il proxy quindi interviene e in modo composito tiene un numero intero
     * che diviso a 2 bit alla vota per un byte che identifica la n compinazione degli operatori
     * e la lista dei numeri interi da sottoporre per confrontare il risultato è nella classe
     * problem che sottopone per ogni eval un puntamento alla n operazione nel numero intero che 
     * poi è un insieme di bit a mò di lista ecco il nome quindi a questo particolare pattern
     * BitFragment.
     */


    public enum Op : byte
    {
        [Description("*")]
        Mul = 0,
        [Description("/")]
        Div = 1,
        [Description("+")]
        Add = 2,
        [Description("-")]
        Sub = 3
    }

    // La classe per le operazioni implicite sui valori
    public static class OpImpl
    {
        static OpImpl()
        {
            var type = typeof(Op);
            foreach (Op op in Enum.GetValues(type))
            {
                MemberInfo[] memInfo = type.GetMember(op.ToString());
                if (memInfo.Length > 0)
                {
                    var attrs = memInfo[0].GetCustomAttributes(
                      typeof(DescriptionAttribute), false);

                    if (attrs.Length > 0)
                    {
                        opNames[op] = ((DescriptionAttribute)attrs[0]).Description[0];
                    }
                }
            }
        }

        private static readonly Dictionary<Op, char> opNames
          = new ();

        // notice the data types!
        private static readonly Dictionary<Op, Func<double, double, double>> opImpl =
          new ()
          {
              [Op.Mul] = ((x, y) => x * y),
              [Op.Div] = ((x, y) => x / y),
              [Op.Add] = ((x, y) => x + y),
              [Op.Sub] = ((x, y) => x - y),
          };

        public static double Call(this Op op, int x, int y)
        {
            return opImpl[op](x, y);
        }

        public static char Name(this Op op)
        {
            return opNames[op];
        }
    }

    // La classe del problema sulla matrice di operazioni
    // da effettuare per insiemi di interi sottoposti a
    // calcoli binari al fine di ootenere nell'eval un
    // combinatorio tale da tornare la combinazione tale
    // da ottenere 0 evitando i frazionati
    public class Problem
    {
        private readonly List<int> numbers;
        private readonly List<Op> ops;

        public Problem(IEnumerable<int> numbers, IEnumerable<Op> ops)
        {
            this.numbers = new List<int>(numbers);
            this.ops = new List<Op>(ops);
        }

        public int Eval()
        {
            var opGroups = new[]
            {
        new[] {Op.Mul, Op.Div},
        new[] {Op.Add, Op.Sub}
      };
        startAgain:
            foreach (var group in opGroups)
            {
                for (var idx = 0; idx < ops.Count; ++idx)
                {
                    if (group.Contains(ops[idx]))
                    {
                        // evaluate value
                        var op = ops[idx];
                        var result = op.Call(numbers[idx], numbers[idx + 1]);

                        // assume all fractional results are wrong
                        if (result != (int)result)
                            return int.MinValue; // calculation won't work

                        numbers[idx] = (int)result;
                        numbers.RemoveAt(idx + 1);
                        ops.RemoveAt(idx);
                        if (numbers.Count == 1) return numbers[0];
                        goto startAgain; // :)
                    }
                }
            }

            return numbers[0];
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            int i = 0;

            for (; i < ops.Count; ++i)
            {
                sb.Append(numbers[i]);
                sb.Append(ops[i].Name());
            }

            sb.Append(numbers[i]);
            return sb.ToString();
        }
    }

    // Una classe del trattamento di 2 bit composti come se fossero
    // singoli elementi nell'insieme di un numero a 64 bit, quindi 
    // 32 valori.
    public class TwoBitSet
    {
        // 64 bits --> 32 values
        private readonly ulong data;

        public TwoBitSet(ulong data)
        {
            this.data = data;
        }

        // Indicizzato questo valore e prendendo 
        // il riferimento come l'indice di un elenco 
        // composto da una sequenza due a due otteniamo 
        // il byte corrispondente.
        public byte this[int index]
        {
            get
            {
                var shift = index << 1;
                ulong mask = (0b11U << shift);
                return (byte)((data & mask) >> shift);
            }
        }
    }

    class Program
    {
        static void Main()
        {
            var numbers = new[] { 1, 3, 5, 7 };
            int numberOfOps = numbers.Length - 1;

            for (int result = 0; result <= 10; ++result)
            {
                // Itera per le 64 possibilità
                for (var key = 0UL; key < (1UL << 2 * numberOfOps); ++key)
                {
                    var tbs = new TwoBitSet(key);
                    var ops = Enumerable.Range(0, numberOfOps)
                      .Select(i => tbs[i]).Cast<Op>().ToArray();
                    var problem = new Problem(numbers, ops);
                    if (problem.Eval() == result)
                    {
                        Console.WriteLine($"{new Problem(numbers, ops)} = {result}");
                        break;
                    }
                }
            }

            Console.ReadKey();
        }
    }
}