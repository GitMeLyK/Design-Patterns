using System;
using static System.Console;

namespace DotNetDesignPatternDemos.Creational.Factories.Method
{
    public class Point
    {

        // factory method

        public static Point NewCartesianPoint(double x, double y)
        {
            return new Point(x, y);
        }

        public static Point NewPolarPoint(double rho, double theta)
        {
            //...
            return new Point(rho * Math.Cos(theta), rho * Math.Sin(theta));
        }


        private readonly double x, y;

        private Point(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public override string ToString()
        {
            return $"{nameof(x)} : {x} , {nameof(y)} : {y}";
        }

    }

    class Demo
    {
        static void Main7(string[] args)
        {
            var p1 = Point.NewPolarPoint(1.0, Math.PI / 2);
            WriteLine(p1);

            var p2 = Point.NewCartesianPoint(1, 2);
        }
    }
}
