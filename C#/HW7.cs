namespace CSharp
{
    public class Square
    {
        public double A
        {
            get;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Side length cannot be negative.");
                field = value;
            }
        }

        public Square()
        {
            A = 0;
        }

        public Square(double a)
        {
            A = a;
        }

        public override string ToString()
        {
            return $"Square Side A = {A}";
        }

        // Unary Operators
        public static Square operator ++(Square s)
        {
            return new Square(s.A + 1);
        }

        public static Square operator --(Square s)
        {
            return new Square(s.A - 1);
        }

        // Mathematical Operators
        public static Square operator +(Square s1, Square s2) => new (s1.A + s2.A);
        public static Square operator -(Square s1, Square s2) => new (s1.A - s2.A);
        public static Square operator *(Square s1, Square s2) => new (s1.A * s2.A);
        public static Square operator /(Square s1, Square s2)
        {
            if (s2.A == 0) throw new DivideByZeroException("Cannot divide by a square with side 0.");
            return new Square(s1.A / s2.A);
        }

        // Comparison Operators
        public static bool operator <(Square s1, Square s2) => s1.A < s2.A;
        public static bool operator >(Square s1, Square s2) => s1.A > s2.A;
        public static bool operator <=(Square s1, Square s2) => s1.A <= s2.A;
        public static bool operator >=(Square s1, Square s2) => s1.A >= s2.A;
        public static bool operator ==(Square s1, Square s2)
        {
            if (ReferenceEquals(s1, null) && ReferenceEquals(s2, null)) return true;
            if (ReferenceEquals(s1, null) || ReferenceEquals(s2, null)) return false;
            return s1.Equals(s2);
        }
        public static bool operator !=(Square s1, Square s2) => !(s1 == s2);

        // Equals and GetHashCode Overrides
        public override bool Equals(object obj)
        {
            if (obj is Square other)
            {
                return this.A == other.A;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return A.GetHashCode();
        }

        // True and False Operators
        public static bool operator true(Square s) => s.A != 0;
        public static bool operator false(Square s) => s.A == 0;

        // Implicit to rectangle
        public static implicit operator Rectangle(Square s)
        {
            return new Rectangle(s.A, s.A);
        }

        // To int
        public static implicit operator int(Square s)
        {
            return (int)s.A;
        }
    }

    public class Rectangle
    {
        public double A
        {
            get;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Side length A cannot be negative.");
                field = value;
            }
        }

        public double B
        {
            get;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Side length B cannot be negative.");
                field = value;
            }
        }

        public Rectangle()
        {
            A = 0;
            B = 0;
        }

        public Rectangle(double a, double b)
        {
            A = a;
            B = b;
        }

        public override string ToString()
        {
            return $"Rectangle Side A = {A}, Side B = {B}";
        }

        // Unary Operators
        public static Rectangle operator ++(Rectangle r)
        {
            return new Rectangle(r.A + 1, r.B + 1);
        }

        public static Rectangle operator --(Rectangle r)
        {
            return new Rectangle(r.A - 1, r.B - 1);
        }

        // Mathematical Operators
        public static Rectangle operator +(Rectangle r1, Rectangle r2) => new (r1.A + r2.A, r1.B + r2.B);
        public static Rectangle operator -(Rectangle r1, Rectangle r2) => new (r1.A - r2.A, r1.B - r2.B);
        public static Rectangle operator *(Rectangle r1, Rectangle r2) => new (r1.A * r2.A, r1.B * r2.B);
        public static Rectangle operator /(Rectangle r1, Rectangle r2)
        {
            if (r2.A == 0 || r2.B == 0) throw new DivideByZeroException("Cannot divide by zero side length.");
            return new Rectangle(r1.A / r2.A, r1.B / r2.B);
        }

        // Comparison Operators
        public static bool operator <(Rectangle r1, Rectangle r2) => r1.A < r2.A && r1.B < r2.B;
        public static bool operator >(Rectangle r1, Rectangle r2) => r1.A > r2.A && r1.B > r2.B;
        public static bool operator <=(Rectangle r1, Rectangle r2) => r1.A <= r2.A && r1.B <= r2.B;
        public static bool operator >=(Rectangle r1, Rectangle r2) => r1.A >= r2.A && r1.B >= r2.B;
        public static bool operator ==(Rectangle r1, Rectangle r2)
        {
            if (ReferenceEquals(r1, null) && ReferenceEquals(r2, null)) return true;
            if (ReferenceEquals(r1, null) || ReferenceEquals(r2, null)) return false;
            return r1.Equals(r2);
        }
        public static bool operator !=(Rectangle r1, Rectangle r2) => !(r1 == r2);

        // Equals and GetHashCode Overrides
        public override bool Equals(object obj)
        {
            if (obj is Rectangle other)
            {
                return this.A == other.A && this.B == other.B;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(A, B);
        }

        // True and False Operators
        public static bool operator true(Rectangle r) => r.A != 0 && r.B != 0;
        public static bool operator false(Rectangle r) => r.A == 0 || r.B == 0;

        // Explicit to square (uses side A)
        public static explicit operator Square(Rectangle r)
        {
            return new Square(r.A);
        }

        // Explicit to int (returns side A)
        public static explicit operator int(Rectangle r)
        {
            return (int)r.A;
        }
    }

    class Hw7
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Square:");
            Square s1 = new Square(5);
            Square s2 = new Square(3);
            Console.WriteLine($"s1: {s1}");
            Console.WriteLine($"s2: {s2}");

            s1++;
            Console.WriteLine($"s1 after ++: {s1}");

            Square s3 = s1 - s2;
            Console.WriteLine($"s1 - s2: {s3}");

            Console.WriteLine($"Is s1 > s2? {s1 > s2}");

            Square emptySquare = new Square(0);
            if (emptySquare) Console.WriteLine("emptySquare is true (non-zero)");
            else Console.WriteLine("emptySquare is false (0 side)");

            Console.WriteLine("\nRectangle:");
            Rectangle r1 = new Rectangle(6, 8);
            Rectangle r2 = new Rectangle(2, 4);
            Console.WriteLine($"r1: {r1}");
            Console.WriteLine($"r2: {r2}");

            Rectangle r3 = r1 / r2;
            Console.WriteLine($"r1 / r2: {r3}");

            Console.WriteLine("\nType Conversions:");
            
            // Implicit Square to Rectangle
            Rectangle rFromSquare = s2; 
            Console.WriteLine($"Implicit Square(3) to Rectangle: {rFromSquare}");

            // Explicit Rectangle to Square
            Square sFromRectangle = (Square)r1; 
            Console.WriteLine($"Explicit Rectangle(6,8) to Square: {sFromRectangle}");

            // Implicit Square to int
            int intFromSquare = s2; 
            Console.WriteLine($"Implicit int from Square(3): {intFromSquare}");

            // Explicit Rectangle to int
            int intFromRectangle = (int)r1; 
            Console.WriteLine($"Explicit int from Rectangle(6,8): {intFromRectangle}");

            Console.WriteLine("\nError Test:");
            try
            {
                Square invalidSquare = s2 - s1; // 3 - 6 = -3 (Should throw error)
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }
        }
    }
}
