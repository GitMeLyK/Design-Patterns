using System;

namespace DotNetDesignPatternDemos.Creational.Factories
{

    /*
     * 
     * Usando il principio della singola responsabilità qui ottenicamo
     * un esempio più completo della classe factory separata per usare più
     * metodi di costruzione dell'oggetto in una classe dedicata a questo compito.
     * 
     * */

    // Factory separate concerc in class dedicata
    class PointFactory
    {

        public static Point NewPolarPoint(float x, float y)
        {
            return new Point(x, y); // needs to be public
        }

        public static Point NewCartesianPoint(float rho, float theta)
        {
            return new Point(rho * Math.Cos(rho), theta * Math.Sin(theta)); // needs to be public
        }
    }



    public class Point
    {
        private readonly double x, y;

        protected Point(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public Point(double a,
          double b, // names do not communicate intent
          CoordinateSystem cs = CoordinateSystem.Cartesian)
        {
            switch (cs)
            {
                case CoordinateSystem.Polar:
                    x = a * Math.Cos(b);
                    y = a * Math.Sin(b);
                    break;
                default:
                    x = a;
                    y = b;
                    break;
            }

            // steps to add a new system
            // 1. augment CoordinateSystem
            // 2. change ctor
        }

        // factory property

        public static Point Origin => new (0, 0);

        // singleton field
        public static Point Origin2 = new (0, 0);

        // factory method Some class
        public static Point NewCartesianPoint(double x, double y)
        {
            return new Point(x, y);
        }

        public static Point NewPolarPoint(double rho, double theta)
        {
            //...
            return null;
        }

        public enum CoordinateSystem
        {
            Cartesian,
            Polar
        }

        // make it lazy
        public static class Factory
        {

            public static Point NewCartesianPoint(double x, double y)
            {
                return new Point(x, y);
            }
        }
    }

    class Demo
    {
        static void Main12(string[] args)
        {

            // Classic cosnstrunctor for object
            var p1 = new Point(2, 3, Point.CoordinateSystem.Cartesian);
            var origin = Point.Origin;

            // Internal separate of concern
            var p2 = Point.Factory.NewCartesianPoint(1, 2);

            // Separate of concercn with class dedicate externale
            var p3 = PointFactory.NewPolarPoint(10, 30);

        }
    }
}
