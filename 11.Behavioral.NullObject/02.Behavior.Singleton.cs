using System;
using System.Dynamic;
using Autofac;
using ImpromptuInterface;
using JetBrains.Annotations;
using static System.Console;

namespace DotNetDesignPatternDemos.Behavioral.Singleton
{
    /*
     * In questo esempio vediamo un tipo di approccio per farci restuire un NullObject
     * attraverso un istanza Singleton. E vediamo che avere questo NullObject dove server
     * con istanza Singleton da utilizzare nel sistema porta ad avere una classe dedicata
     * e separata di riutilizzare come per l'interfaccia, e questo in termini di incapsulamento
     * porta empre a considerare due unti di un oggetto che in definitiva deve simulare senza
     * fare nulla il coportamento di un oggetto implementato.
     * Tenendo presente le ultime novità di c# dal 9 in poi abbiamo la possibilità di implementare
     * i metodi all'interno delle interfacce, che vengono definiti metodi di default, e questo
     * non ci proibisce di avere anche una Sub Class all'interno che faccia da supporto ad essere
     * usata per questo scopo, quindi invece di avere una class singlton dedicata alle istanze
     * di oggetti null innestiamo la classe nella stessa interfaccia e questa diventa magicamente
     * portabile con essa.
     */

    // L'interfaccia per i metodi del Componente B che deve essere usato da A
    public interface ILog
    {
        public void Warn();

        public static ILog Null => NullLog.Instance;

        // La nostra Sub Class dentro l'interfaccia per ottenere nuove istanze Lazy in questo
        // caso degli ObjectNull da usare nei punti dove ci serve del programma.
        // Questo rende l'utilizzo più un Oggetto che non serve in altri ambiti più consono allo scopo
        // nascondendo l'implementazione dal resto del programma, e accedendo nello stesso punto in cui
        // si dovrebbe importare l'interfaccia corrente.
        private sealed class NullLog : ILog
        {
            private NullLog() { }

            private static Lazy<NullLog> instance =
              new Lazy<NullLog>(() => new NullLog());

            public static ILog Instance => instance.Value;

            public void Warn()
            {

            }
        }
    }

    // L'elemento A che può opzionalmente usare il componente B o il null di questo oggetto
    public class BankAccount
    {
        public BankAccount(ILog log )
        {
            if (log is null) log = ILog.Null;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Ottenere l'istanza di un oggetto Nullo preso direttamente dall'interfaccia.
            ILog log = ILog.Null;
        }
    }
}
