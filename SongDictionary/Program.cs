//***************************************************************************************************
//Program Title: Song Dictionary
//Programmer: Jackson Meyn (22558031)
//Version: 0.1
//Decription: Create, manage and filter a dictionary of songs 
//through use of a SongList class to create a dictionary, and Song class for individual songs.
//***************************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SongDictionary
{
    class Program
    {
        static void Main(string[] args)
        {

            SongList songs = new SongList("Jackson's Favourites");

            //Adding two songs with same title to test case-insensitive searching of member functions
            songs.Add("Hello", new Song("Hello", "Lionel Richie", "Lionel Richie", 1984, 4));
            songs.Add("hello", new Song("Hello", "Adele", "Adele Adkins, Greg Kurstin", 2015, 4));

            //More songs...
            songs.Add("Gangnam Style", new Song("Gangnam Style", "PSY", "PSY", 2012, 3));
            songs.Add("Boys of Summer", new Song("Boys of Summer", "Don Henley", "Don Henley, Mike Campbell", 1984, 4));
            songs.Add("Pompeii", new Song("Pompeii", "Bastille", "Dan Smith", 2013, 3));
            songs.Add("Pumped Up Kicks", new Song("Pumped Up Kicks", "Foster The People", "Mark Foster", 2010, 4));
            songs.Add("Goodbye Horses", new Song("Goodbye Horses", "Q Lazzarus", "William Garvey", 1988, 3));
            songs.Add("The Great Beyond", new Song("The Great Beyond", "R.E.M.", "Peter Buck, Mike Mills, Michael Stipe", 1999, 5));
            songs.Add("The Partisan", new Song("The Partisan", "Leonard Cohen", "Leonard Cohen, Emmanuel d'Astier de la Vigerie, Anna Marly", 1969, 4));
            songs.Add("Eight Days A Week", new Song("Eight Days A Week", "The Beatles", "John Lennon, Paul McCartney", 1964, 2));
            songs.Add("Hound Dog", new Song("Hound Dog", "Elvis Presley", "Jerry Lieber, Mike Stoller", 1956, 2));

            //Test all member functions
            songs.Print();
            songs.PrintByTitle("hello");
            songs.PrintByDuration(3, 4);
            songs.PrintByYear(1984);
            songs.PrintAllByYearDesc();

            Console.WriteLine("Deleting all songs with key 'hello' (case insensitive)...");
            songs.DeleteByTitle("hello");
            songs.PrintAllByYearDesc(); //Print songs again to confirm deletion

            Console.ReadLine();

        }
    }

    class Song
    {
        private string title;
        private string singer;
        private string songwriter;
        private int year;
        private int duration;
        //in minutes
        public Song(string _title, string _singer, string _songwriter, int _year, int _duration)
        {
            title = _title;
            singer = _singer;
            songwriter = _songwriter;
            year = _year;
            duration = _duration;
        }
        public string Title
        // Title is immutable
        {
            get
            {
                return title;
            }
        }
        public string Singer
        // Singer is immutable
        {
            get
            {
                return singer;
            }
        }
        public string SongWriter
        // Song writer is immutable
        {
            get
            {
                return songwriter;
            }
        }
        public int Year
        {
            get
            {
                return year;
            }
            set
            {
                year = value;
            }
        }
        public int Duration
        {
            get
            {
                return duration;
            }
            set
            {
                duration = value;
            }
        }
        public override string ToString()
        {
            return string.Format("{0} ({1}). Written by {2}. Released in \"{3}\" {4}mins.", title, singer, songwriter, year, duration);
        }

        public void Print()
        {
            Console.WriteLine(this.ToString());
        }

    }

    class SongList
    {
        //Properties
        private string name;
        private Dictionary<string, Song> songs = new Dictionary<string, Song>();

        internal Dictionary<string, Song> Songs { get => songs; set => songs = value; }
        public string Name { get => name; set => name = value; }

        //End properties

        //Constructors
        public SongList()
        {
            string input = "";
            while (input == "")
            {
                Console.WriteLine("Enter a name for this song list. It cannot be left blank");
                input = Console.ReadLine();
            }
            Name = input;
        }

        public SongList(string _name)
        {
            Name = _name;
        }
        //End constructors

        public void Add(string name, Song song)
        {
            Songs.Add(name, song);
        }

        public void Print()
        {
            Console.WriteLine("SONGS IN COLLECTION {0}", Name);
            Console.WriteLine("===================================");
            if (Songs.Count != 0)
            {
                foreach (var kvp in Songs)
                {
                    Console.WriteLine(kvp.Value.ToString());
                }
                
            } else
            {
                Console.WriteLine("No songs found to display.");
            }
            Console.WriteLine("===================================");
        }

        public int DeleteByTitle(string _title)
        {
            var result = GetSongByTitle(_title);
            if (result != null)
            {
                int deleteCount = result.Count();
                foreach (var kvp in result.ToList())
                {
                    Songs.Remove(kvp.Key);
                }

                Console.WriteLine("Delete successful. {0} songs removed from collection", deleteCount);
                return deleteCount;

            } else
            {
                Console.WriteLine("No songs of title '{0}' found to delete", _title);
                return 0;
            }
            
        }

        public IEnumerable<KeyValuePair<string, Song>> GetAllSongs()
        {
            var query = from song in Songs
                        select song;
            if (query.Any())
            {
                return query;
            }
            else
            {
                return null;
            }

        }

        public IEnumerable<KeyValuePair<string, Song>> GetSongByTitle(string _title)
        {

            var query = from song in Songs
                        where song.Key.Equals(_title,StringComparison.CurrentCultureIgnoreCase) //Case insensitive comparison
                        select song;
            if (query.Any())
            {
                return query;
            } else
            {
                return null;
            }
            
        }

        public IEnumerable<KeyValuePair<string, Song>> GetSongByDuration(int shortest, int longest)
        {
            var query = from song in Songs
                        where song.Value.Duration >= shortest && song.Value.Duration <= longest
                        select song;
            if (query.Any())
            {
                return query;
            }
            else
            {
                return null;
            }

        }

        public IEnumerable<KeyValuePair<string, Song>> GetByYear(int year)
        {
            var query = from song in Songs
                        where song.Value.Year == year
                        select song;
            if (query.Any())
            {
                return query;
            }
            else
            {
                return null;
            }

        }

        public void PrintByDuration(int shortest, int longest)
        {
            var result = GetSongByDuration(shortest, longest);

            Console.WriteLine("PRINT SONGS IN COLLECTION {0} WITH DURATION BETWEEN " + shortest + " AND " + longest + " MINUTES", Name);
            Console.WriteLine("========================================");

            if (result != null)
            {

                foreach (var kvp in result.OrderBy(key => key.Value.Duration))
                {
                    Console.WriteLine(kvp.Value.ToString());
                }
            }
            else
            {
                Console.WriteLine("No results found with duration between " + shortest + " and " + longest + " minutes");
            }

            Console.WriteLine("========================================");
        }

        public void PrintByTitle(string title)
        {
            var result = GetSongByTitle(title);

            Console.WriteLine("PRINT SONGS IN COLLECTION {0} BY TITLE " + title, Name);
            Console.WriteLine("========================================");

            if (result != null)
            {
                
                foreach (var kvp in result.OrderBy(key => key.Key))
                {
                    Console.WriteLine(kvp.Value.ToString());
                }
            } else
            {
                Console.WriteLine("No results found with title " + title);
            }

            Console.WriteLine("========================================");
        }

        public void PrintByYear(int year)
        {
            var result = GetByYear(year);

            Console.WriteLine("PRINT SONGS IN COLLECTION {0} WITH RELEASE YEAR " + year, Name);
            Console.WriteLine("========================================");

            if (result != null)
            {

                foreach (var kvp in result.OrderBy(key => key.Value.Year))
                {
                    Console.WriteLine(kvp.Value.ToString());
                }
            }
            else
            {
                Console.WriteLine("No results found with release year " + year);
            }

            Console.WriteLine("========================================");
        }

        public void PrintAllByYearDesc()
        {
            var result = GetAllSongs();

            Console.WriteLine("PRINT SONGS IN COLLECTION {0} BY DECENDING RELEASE YEAR", Name);
            Console.WriteLine("========================================");

            if (result != null)
            {

                foreach (var kvp in result.OrderByDescending(key => key.Value.Year))
                {
                    Console.WriteLine(kvp.Value.ToString());
                }
            }
            else
            {
                Console.WriteLine("No results found.");
            }

            Console.WriteLine("========================================");
        }
    }

}
