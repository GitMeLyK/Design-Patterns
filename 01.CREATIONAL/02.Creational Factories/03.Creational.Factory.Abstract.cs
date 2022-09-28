using System;
using System.Collections.Generic;

namespace DotNetDesignPatternDemos.Creational.AbstractFactory
{
    public interface IHotDrink
    {
        void Consume();
    }

    internal class Tea : IHotDrink
    {
        public void Consume()
        {
            Console.WriteLine("This tea is nice but I'd prefer it with milk.");
        }
    }

    internal class Coffee : IHotDrink
    {
        public void Consume()
        {
            Console.WriteLine("This coffee is delicious!");
        }
    }

    public interface IHotDrinkFactory
    {
        IHotDrink Prepare(int amount);
    }

    internal class TeaFactory : IHotDrinkFactory
    {
        public IHotDrink Prepare(int amount)
        {
            Console.WriteLine($"Put in tea bag, boil water, pour {amount} ml, add lemon, enjoy!");
            return new Tea();
        }
    }

    internal class CoffeeFactory : IHotDrinkFactory
    {
        public IHotDrink Prepare(int amount)
        {
            Console.WriteLine($"Grind some beans, boil water, pour {amount} ml, add cream and sugar, enjoy!");
            return new Coffee();
        }
    }

    public class HotDrinkMachine
    {
        public enum AvailableDrink // violates open-closed
        {
            Coffee, Tea
        }

        private readonly Dictionary<AvailableDrink, IHotDrinkFactory> factories =
          new ();


        public HotDrinkMachine()
        {            
            foreach (AvailableDrink drink in Enum.GetValues(typeof(AvailableDrink)))
            {
              var factory = (IHotDrinkFactory) Activator.CreateInstance(
                Type.GetType("DotNetDesignPatternDemos.Creational.AbstractFactory." + Enum.GetName(typeof(AvailableDrink), drink) + "Factory"));
              factories.Add(drink, factory);
            }
        }

        public IHotDrink MakeDrink(AvailableDrink drink, int amount)
        {
          return factories[drink].Prepare(amount);
        }
    }

    class Program
    {
        static void Main32(string[] args)
        {
            var machine = new HotDrinkMachine();
   
            IHotDrink drink = machine.MakeDrink( HotDrinkMachine.AvailableDrink.Tea, 100);
            drink.Consume();
        }
    }
}
