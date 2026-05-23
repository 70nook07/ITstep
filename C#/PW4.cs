namespace CSharp;
public class Book
{
    public string Title { get; set; }
    public string Author { get; set; }

    public Book(string title, string author)
    {
        Title = title;
        Author = author;
    }

    public override string ToString()
    {
        return $"\"{Title}\" by {Author}";
    }
}

public class ReadingList
{
    private List<Book> books;

    public ReadingList()
    {
        books = new List<Book>();
    }

    public void Add(Book book)
    {
        books.Add(book);
        Console.WriteLine($"Added: {book}");
    }

    public bool Remove(string title)
    {
        for (int i = 0; i < books.Count; i++)
        {
            if (books[i].Title.Equals(title, StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine($"Removed: {books[i]}");
                books.RemoveAt(i);
                return true;
            }
        }

        Console.WriteLine($"Book \"{title}\" not found.");
        return false;
    }

    public bool doesExist(string title)
    {
        for (int i = 0; i < books.Count; i++)
        {
            if (books[i].Title.Equals(title, StringComparison.OrdinalIgnoreCase)) return true;
        }

        return false;
    }

    // INDEXERS

    // by int pos
    public Book this[int index]
    {
        get { return books[index]; }
        set { books[index] = value; }
    }

    // by title
    public Book this[string title]
    {
        get
        {
            for (int i = 0; i < books.Count; i++)
            {
                if (books[i].Title.Equals(title, StringComparison.OrdinalIgnoreCase)) return books[i];
            }

            return null;
        }
    }

    // by title and author
    public Book this[string title, string author]
    {
        get
        {
            for (int i = 0; i < books.Count; i++)
            {
                if (books[i].Title.Equals(title, StringComparison.OrdinalIgnoreCase) &&
                    books[i].Author.Equals(author, StringComparison.OrdinalIgnoreCase))
                    return books[i];
            }

            return null;
        }
    }

    public void PrintAll()
    {
        if (books.Count == 0)
        {
            Console.WriteLine("Reading list is empty.");
            return;
        }

        Console.WriteLine("\nReading List");
        for (int i = 0; i < books.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {books[i]}");
        }

        Console.WriteLine();
    }
}

class PW4
{
    static void Main(string[] args)
    {
        ReadingList myList = new ReadingList();

        // Add books
        myList.Add(new Book("1984", "George Orwell"));
        myList.Add(new Book("Brave New World", "Aldous Huxley"));
        myList.Add(new Book("Fahrenheit 451", "Ray Bradbury"));
        myList.Add(new Book("Dune", "Frank Herbert"));
        myList.PrintAll();

        // by index
        Console.WriteLine($"Book at index 2: {myList[2]}");

        // by string
        Book foundBook = myList["1984"];
        Console.WriteLine($"Search by title '1984': {(foundBook != null ? foundBook.ToString() : "Not found")}");

        // by title and author
        foundBook = myList["Dune", "Frank Herbert"];
        Console.WriteLine($"Search by title and author: {(foundBook != null ? foundBook.ToString() : "Not found")}");

        // check if book exists
        Console.WriteLine($"Contains 'Dune'? {myList.doesExist("Dune")}");
        Console.WriteLine($"Contains 'A prayer for Owen Miny'? {myList.doesExist("A prayer for Owen Miny")}");

        // remove a book
        myList.Remove("1984");
        myList.PrintAll();
        foundBook = myList["1984"];
        Console.WriteLine($"Search by title '1984': {(foundBook != null ? foundBook.ToString() : "Not found")}");
    }
}
