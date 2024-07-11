using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

public class Movie
{
    public string Title { get; set; }
    public string Genre { get; set; }
    public string MainActor { get; set; }
    public string Director { get; set; }
}

public class Program
{
    static List<Movie> moviesFromXml = new List<Movie>();
    static List<Movie> moviesFromText = new List<Movie>();

    static void Main()
    {
        LoadMoviesFromXml("C:\\Users\\romai\\source\\repos\\DataSources\\XML\\films.xml");
        LoadMoviesFromText("C:\\Users\\romai\\source\\repos\\DataSources\\Text\\films.txt");

        while (true)
        {
            Console.WriteLine("Que voulez-vous faire ?");
            Console.WriteLine("1. Transformer une source de données en une autre");
            Console.WriteLine("2. Recherche sur plusieurs sources de données");
            Console.WriteLine("3. Trier les résultats");
            Console.WriteLine("4. Condition de recherche (filtrage)");
            Console.WriteLine("0. Quitter");

            int choix;
            if (!int.TryParse(Console.ReadLine(), out choix))
            {
                Console.WriteLine("Choix invalide. Veuillez entrer un nombre.");
                continue;
            }

            switch (choix)
            {
                case 1:
                    TransformData();
                    break;
                case 2:
                    SearchData();
                    break;
                case 3:
                    FilterData();
                    break;
                case 4:
                    SortData();
                    break;
                case 0:
                    Console.WriteLine("Au revoir !");
                    return;
                default:
                    Console.WriteLine("Choix invalide. Veuillez choisir une option valide.");
                    break;
            }
        }
    }

    static void LoadMoviesFromXml(string filename)
    {
        XDocument doc = XDocument.Load(filename);
        moviesFromXml = doc.Descendants("movie")
                           .Select(m => new Movie
                           {
                               Title = m.Element("title").Value,
                               Genre = m.Element("genre").Value,
                               MainActor = m.Element("main_actor").Value,
                               Director = m.Element("director").Value
                           })
                           .ToList();
    }

    static void LoadMoviesFromText(string filename)
    {
        var lines = File.ReadAllLines(filename);
        moviesFromText = lines.Select(line =>
        {
            var parts = line.Split(';');
            return new Movie
            {
                Title = parts[0].Trim(),
                Genre = parts[1].Trim(),
                MainActor = parts[2].Trim(),
                Director = parts[3].Trim()
            };
        }).ToList();
    }

    static void TransformData()
    {
        // Exemple de transformation (de XML à texte)
        var transformedData = moviesFromXml.Select(m => $"{m.Title}; {m.Genre}; {m.MainActor}; {m.Director}");
        foreach (var item in transformedData)
        {
            Console.WriteLine(item);
        }
    }

    static void SearchData()
    {
        // Exemple de recherche sur plusieurs sources de données (par titre)
        Console.WriteLine("Quelle est votre recherche ?");
        string searchTerm = Console.ReadLine();
        var searchResults = moviesFromXml.Concat(moviesFromText)
                                        .Where(m => m.Title.Contains(searchTerm));
        foreach (var movie in searchResults)
        {
            Console.WriteLine($"{movie.Title} - {movie.Genre} - {movie.MainActor} - {movie.Director}");
        }
    }

    static void SortData()
    {
        Console.WriteLine("Choisissez un sens : ");
        Console.WriteLine("1 - Croissant ");
        Console.WriteLine("2 - Décroissant");

        int sens = int.Parse(Console.ReadLine());

        Console.WriteLine("Choisissez un critère : ");
        Console.WriteLine("1 - Titre");
        Console.WriteLine("2 - Genre");
        Console.WriteLine("3 - Acteur principal");
        Console.WriteLine("4 - Réalisateur");

        int choice = int.Parse(Console.ReadLine());

        IEnumerable<Movie> filteredData = Enumerable.Empty<Movie>();

        switch (choice)
        {
            case 1:
                switch(sens)
                {
                    case 1:
                        filteredData = moviesFromXml.OrderBy(m => m.Title);
                        break;
                    case 2:
                        filteredData = moviesFromXml.OrderByDescending(m => m.Title);
                        break;
                }
                break;
            case 2:
                switch (sens)
                {
                    case 1:
                        filteredData = moviesFromXml.OrderBy(m => m.Genre);
                        break;
                    case 2:
                        filteredData = moviesFromXml.OrderByDescending(m => m.Genre);
                        break;
                }
                break;
            case 3:
                switch (sens)
                {
                    case 1:
                        filteredData = moviesFromXml.OrderBy(m => m.MainActor);
                        break;
                    case 2:
                        filteredData = moviesFromXml.OrderByDescending(m => m.MainActor);
                        break;
                }
                break;
            case 4:
                switch (sens)
                {
                    case 1:
                        filteredData = moviesFromXml.OrderBy(m => m.Director);
                        break;
                    case 2:
                        filteredData = moviesFromXml.OrderByDescending(m => m.Director);
                        break;
                }
                break;
            default:
                Console.WriteLine("Choix invalide.");
                return;
        }

        foreach (var movie in filteredData)
        {
            Console.WriteLine($"{movie.Title} - {movie.Genre} - {movie.MainActor} - {movie.Director}");
        }
    }

    static void FilterData()
    {
        Console.WriteLine("Choisissez un critère de recherche : ");
        Console.WriteLine("1 - Titre");
        Console.WriteLine("2 - Genre");
        Console.WriteLine("3 - Acteur principal");
        Console.WriteLine("4 - Réalisateur");

        int choice = int.Parse(Console.ReadLine());

        Console.WriteLine("Entrez la valeur de recherche : ");
        string searchValue = Console.ReadLine();

        IEnumerable<Movie> filteredData = Enumerable.Empty<Movie>();

        switch (choice)
        {
            case 1:
                filteredData = moviesFromXml.Concat(moviesFromText)
                                            .Where(m => m.Title.Contains(searchValue, StringComparison.InvariantCultureIgnoreCase));
                break;
            case 2:
                filteredData = moviesFromXml.Concat(moviesFromText)
                                            .Where(m => m.Genre.Contains(searchValue, StringComparison.InvariantCultureIgnoreCase));
                break;
            case 3:
                filteredData = moviesFromXml.Concat(moviesFromText)
                                            .Where(m => m.MainActor.Contains(searchValue, StringComparison.InvariantCultureIgnoreCase));
                break;
            case 4:
                filteredData = moviesFromXml.Concat(moviesFromText)
                                            .Where(m => m.Director.Contains(searchValue, StringComparison.InvariantCultureIgnoreCase));
                break;
            default:
                Console.WriteLine("Choix invalide.");
                return;
        }

        foreach (var movie in filteredData)
        {
            Console.WriteLine($"{movie.Title} - {movie.Genre} - {movie.MainActor} - {movie.Director}");
        }
    }
}
