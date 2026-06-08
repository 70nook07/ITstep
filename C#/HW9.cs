namespace CSharp
{
    // TASK 1: IOutput & Array Class
    public interface IOutput
    {
        void Show();
        void Show(string info);
    }

    // TASK 2: IMath
    public interface IMath
    {
        int Max();
        int Min();
        float Avg();
        bool Search(int valueToSeek);
    }

    // TASK 3: ISort
    public interface ISort
    {
        void SortAsc();
        void SortDesc();
        void SortByParam(bool isAsc);
    }

    // Array with every interface
    public class Array : IOutput, IMath, ISort
    {
        private int[] _elems;

        public Array(int[] initialElements)
        {
            _elems = initialElements ?? [];
        }

        //IOutput
        public void Show()
        {
            Console.WriteLine(string.Join(", ", _elems));
        }

        public void Show(string info)
        {
            Console.WriteLine($"{info}: {string.Join(", ", _elems)}");
        }

        //IMath
        public int Max()
        {
            if (_elems.Length == 0) throw new InvalidOperationException("Array is empty.");
            int max = _elems[0];
            foreach (int num in _elems)
            {
                if (num > max) max = num;
            }

            return max;
        }

        public int Min()
        {
            if (_elems.Length == 0) throw new InvalidOperationException("Array is empty.");
            int min = _elems[0];
            foreach (int num in _elems)
            {
                if (num < min) min = num;
            }

            return min;
        }

        public float Avg()
        {
            if (_elems.Length == 0) return 0f;
            float sum = 0;
            foreach (int num in _elems)
            {
                sum += num;
            }

            return sum / _elems.Length;
        }

        public bool Search(int valueToSearch)
        {
            foreach (int num in _elems)
            {
                if (num == valueToSearch) return true;
            }

            return false;
        }

        // ISort
        public void SortAsc()
        {
            System.Array.Sort(_elems);
        }

        public void SortDesc()
        {
            System.Array.Sort(_elems);
            System.Array.Reverse(_elems);
        }

        public void SortByParam(bool isAsc)
        {
            if (isAsc)
                SortAsc();
            else
                SortDesc();
        }
    }

    class HW9
    {
        static void Main(string[] args)
        {
            int[] data = [24, -5, 7, 12, 0, 88, -14, 3];
            Array customArray = new Array(data);

            // Task 1
            Console.WriteLine("\tTASK 1: IOutput");
            Console.Write("Default Show(): ");
            customArray.Show();
            customArray.Show("Show with info message (Initial Array)");
            Console.WriteLine();

            // Task 2
            Console.WriteLine("\tTASK 2: IMath");
            Console.WriteLine($"Maximum element: {customArray.Max()}");
            Console.WriteLine($"Minimum element: {customArray.Min()}");
            Console.WriteLine($"Average value: {customArray.Avg():F2}");
            int target1 = 12;
            int target2 = 99;
            Console.WriteLine($"Searching for {target1}: {customArray.Search(target1)}");
            Console.WriteLine($"Searching for {target2}: {customArray.Search(target2)}");
            Console.WriteLine();

            // Task 3
            Console.WriteLine("\tTASK 3: ISort");
            customArray.SortByParam(true);
            customArray.Show("Sorted by parameter (isAsc = true)");
            customArray.SortByParam(false);
            customArray.Show("Sorted by parameter (isAsc = false)");
            Console.WriteLine("\nTesting completed successfully.");
        }
    }
}
