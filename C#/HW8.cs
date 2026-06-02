namespace CSharp
{
    public abstract class Shape
    {
        public abstract double GetArea();
        public abstract double GetPerimeter();
    }

    public class Triangle : Shape
    {
        public double SideA { get; set; }
        public double SideB { get; set; }
        public double SideC { get; set; }

        public Triangle(double a, double b, double c)
        {
            SideA = a;
            SideB = b;
            SideC = c;
        }

        public override double GetArea()
        {
            double s = GetPerimeter() / 2;
            return Math.Sqrt(s * (s - SideA) * (s - SideB) * (s - SideC));
        }

        public override double GetPerimeter()
        {
            return SideA + SideB + SideC;
        }
    }

    public class Square : Shape
    {
        public double Side { get; set; }

        public Square(double side)
        {
            Side = side;
        }

        public override double GetArea()
        {
            return Side * Side;
        }

        public override double GetPerimeter()
        {
            return 4 * Side;
        }
    }

    public class Rhombus : Shape
    {
        public double Side { get; set; }
        public double Height { get; set; }

        public Rhombus(double side, double height)
        {
            Side = side;
            Height = height;
        }

        public override double GetArea()
        {
            return Side * Height;
        }

        public override double GetPerimeter()
        {
            return 4 * Side;
        }
    }

    public class Rectangle : Shape
    {
        public double Width { get; set; }
        public double Height { get; set; }

        public Rectangle(double width, double height)
        {
            Width = width;
            Height = height;
        }

        public override double GetArea()
        {
            return Width * Height;
        }

        public override double GetPerimeter()
        {
            return 2 * (Width + Height);
        }
    }

    public class Parallelogram : Shape
    {
        public double BaseSide { get; set; }
        public double SlantSide { get; set; }
        public double Height { get; set; }

        public Parallelogram(double baseSide, double slantSide, double height)
        {
            BaseSide = baseSide;
            SlantSide = slantSide;
            Height = height;
        }

        public override double GetArea()
        {
            return BaseSide * Height;
        }

        public override double GetPerimeter()
        {
            return 2 * (BaseSide + SlantSide);
        }
    }

    public class Trapezoid : Shape
    {
        public double BaseA { get; set; }
        public double BaseB { get; set; }
        public double SideC { get; set; }
        public double SideD { get; set; }
        public double Height { get; set; }

        public Trapezoid(double baseA, double baseB, double sideC, double sideD, double height)
        {
            BaseA = baseA;
            BaseB = baseB;
            SideC = sideC;
            SideD = sideD;
            Height = height;
        }

        public override double GetArea()
        {
            return ((BaseA + BaseB) / 2) * Height;
        }

        public override double GetPerimeter()
        {
            return BaseA + BaseB + SideC + SideD;
        }
    }

    public class Circle : Shape
    {
        public double Radius { get; set; }

        public Circle(double radius)
        {
            Radius = radius;
        }

        public override double GetArea()
        {
            return Math.PI * Math.Pow(Radius, 2);
        }

        public override double GetPerimeter()
        {
            return 2 * Math.PI * Radius;
        }
    }

    public class Ellipse : Shape
    {
        public double Major { get; set; } // a
        public double Minor { get; set; } // b

        public Ellipse(double major, double minor)
        {
            Major = major;
            Minor = minor;
        }

        public override double GetArea()
        {
            return Math.PI * Major * Minor;
        }

        public override double GetPerimeter()
        {
            // Ramanujan's first approximation for ellipse perimeter
            return Math.PI * (3 * (Major + Minor) - Math.Sqrt((3 * Major + Minor) * (Major + 3 * Minor)));
        }
    }

    public class CompositeShape : Shape
    {
        private Shape[] _shapes;

        public CompositeShape(params Shape[] shapes)
        {
            _shapes = shapes ?? new Shape[0];
        }

        public override double GetArea()
        {
            double totalArea = 0;
            foreach (var shape in _shapes)
            {
                totalArea += shape.GetArea();
            }

            return totalArea;
        }

        public override double GetPerimeter()
        {
            double totalPerimeter = 0;
            foreach (var shape in _shapes)
            {
                totalPerimeter += shape.GetPerimeter();
            }

            return totalPerimeter;
        }
    }

    class Hw8
    {
        static void Main(string[] args)
        {
            Shape triangle = new Triangle(3, 4, 5);
            Shape square = new Square(5);
            Shape rhombus = new Rhombus(5, 4);
            Shape rectangle = new Rectangle(4, 6);
            Shape parallelogram = new Parallelogram(6, 5, 4);
            Shape trapezoid = new Trapezoid(6, 10, 5, 5, 4);
            Shape circle = new Circle(7);
            Shape ellipse = new Ellipse(5, 3);
            Shape[] shapesCollection = new Shape[]
            {
                triangle, square, rhombus, rectangle, parallelogram, trapezoid, circle, ellipse
            };
            Console.WriteLine(" ~~~ Individual Shapes ~~~ ");
            foreach (var shape in shapesCollection)
            {
                string shapeName = shape.GetType().Name;
                Console.WriteLine($"Shape: {shapeName}");
                Console.WriteLine($"\tArea:      {shape.GetArea():F2}");
                Console.WriteLine($"\tPerimeter: {shape.GetPerimeter():F2}");
                Console.WriteLine();
            }

            Console.WriteLine("~~~~~~~~~~~~~~\n");
            Console.WriteLine(" ~~~ Composite Shapes ~~~ ");

            // Angular shapes
            CompositeShape angularGroup = new CompositeShape(triangle, square, rectangle, trapezoid);

            // Round shapes
            CompositeShape roundGroup = new CompositeShape(circle, ellipse);

            // Both
            CompositeShape bigGroup = new CompositeShape(angularGroup, roundGroup, rhombus, parallelogram);
            Console.WriteLine(
                $"Angular Shapes Group \tTotal Area: {angularGroup.GetArea():F2} / Total Perimeter: {angularGroup.GetPerimeter():F2}");
            Console.WriteLine(
                $"Round Shapes Group \tTotal Area: {roundGroup.GetArea():F2} / Total Perimeter: {roundGroup.GetPerimeter():F2}");
            Console.WriteLine(
                $"Both Shapes Group \tTotal Area: {bigGroup.GetArea():F2} / Total Perimeter: {bigGroup.GetPerimeter():F2}");
        }
    }
}
