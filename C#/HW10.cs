using System;
using System.Collections;
using System.Collections.Generic;

namespace CinemaTask
{
    public enum Genre
    {
        Comedy,
        Horror,
        Adventure,
        Drama,
        Action,
        SciFi,
        Thriller
    }
    
    public class Director : ICloneable
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public Director(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }

        // ICloneable
        public object Clone()
        {
            return new Director(this.FirstName, this.LastName);
        }

        public override string ToString()
        {
            return $"{FirstName} {LastName}";
        }
    }
    
    public class Movie : ICloneable, IComparable<Movie>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public Director Director { get; set; }
        public string Country { get; set; }
        public Genre Genre { get; set; }
        public int Year { get; set; }
        public short Rating { get; set; }

        // ICloneable
        public object Clone()
        {
            return new Movie
            {
                Title = this.Title,
                Description = this.Description,
                Director = this.Director != null ? (Director)this.Director.Clone() : null,
                Country = this.Country,
                Genre = this.Genre,
                Year = this.Year,
                Rating = this.Rating
            };
        }

        // IComparable
        public int CompareTo(Movie other)
        {
            if (other == null) return 1;
            return string.Compare(this.Title, other.Title, StringComparison.OrdinalIgnoreCase);
        }

        public override string ToString()
        {
            return $"Movie: '{Title}' ({Year}) / Genre: {Genre} / Rating: {Rating} / Director: {Director}";
        }
    }

    // Comparator classes
    public class CompareByRating : IComparer<Movie>
    {
        public int Compare(Movie x, Movie y)
        {
            if (x == null && y == null) return 0;
            if (x == null) return -1;
            if (y == null) return 1;
            
            return x.Rating.CompareTo(y.Rating);
        }
    }

    public class CompareByYear : IComparer<Movie>
    {
        public int Compare(Movie x, Movie y)
        {
            if (x == null && y == null) return 0;
            if (x == null) return -1;
            if (y == null) return 1;
            
            return x.Year.CompareTo(y.Year);
        }
    }
    
    public class Cinema : IEnumerable<Movie>
    {
        private Movie[] _movies;
        private string _address;

        public Cinema(string address, Movie[] movies)
        {
            _address = address;
            _movies = movies ?? Array.Empty<Movie>();
        }
        
        public void Sort()
        {
            Array.Sort(_movies);
        }

        // Custom sort
        public void Sort(IComparer<Movie> comparer)
        {
            Array.Sort(_movies, comparer);
        }

        // IEnumerable
        public IEnumerator<Movie> GetEnumerator()
        {
            for (int i = 0; i < _movies.Length; i++)
            {
                yield return _movies[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void ShowMovies()
        {
            Console.WriteLine($"\nCinema Address: {_address}");
            foreach (var movie in _movies)
            {
                Console.WriteLine(movie.ToString());
            }
        }
    }

    // testing
    class HW010
    {
        static void Main()
        {
            // Створюємо масив фільмів
            Movie[] movies = {
                new Movie { Title = "Interstellar", Year = 2014, Rating = 9, Genre = Genre.SciFi, Director = new Director("Christopher", "Nolan") },
                new Movie { Title = "Matrix", Year = 1999, Rating = 8, Genre = Genre.SciFi, Director = new Director("Laurence", "Wachowski") },
                new Movie { Title = "Lord of the Rings", Year = 2001, Rating = 10, Genre = Genre.Adventure, Director = new Director("Peter", "Jackson") }
            };

            Cinema myCinema = new Cinema("Mark Twain park", movies);

            Console.WriteLine("Starting movie list:");
            myCinema.ShowMovies();

            // 1. ABC sort
            myCinema.Sort();
            Console.WriteLine("\nAfter ABC sort:");
            myCinema.ShowMovies();

            // 2. Sort by year
            myCinema.Sort(new CompareByYear());
            Console.WriteLine("\nSort by year:");
            myCinema.ShowMovies();

            // 3. Sort by rating
            myCinema.Sort(new CompareByRating());
            Console.WriteLine("\nSort by rating:");
            myCinema.ShowMovies();

            // 4. Deep cloning
            Console.WriteLine("\nCloning test");
            Movie originalMovie = movies[0];
            Movie clonedMovie = (Movie)originalMovie.Clone();
            
            // Change a director name for a specific movie
            clonedMovie.Director.FirstName = "CHANGED!";
            
            Console.WriteLine($"OG: {originalMovie.Director}");
            Console.WriteLine($"Clone: {clonedMovie.Director}");
        }
    }
}
