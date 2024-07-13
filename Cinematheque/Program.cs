using System.Xml.Linq;
using Cinematheque;
using System;
using System.IO;


public class Program
{
    static List<Movie> moviesFromXml = new List<Movie>();
    static List<Movie> moviesFromText = new List<Movie>();

    static void Main()
    {
        string currentDirectory = Directory.GetCurrentDirectory();
        string solutionDirectory = Directory.GetParent(currentDirectory).Parent.Parent.Parent.FullName;
        string xmlFilePath = Path.Combine(solutionDirectory, "DataSources", "XML", "films.xml");
        string textFilePath = Path.Combine(solutionDirectory, "DataSources", "Text", "films.txt");
        LoadMoviesFromXml(xmlFilePath);
        LoadMoviesFromText(textFilePath);

        while (true)
        {

            DisplayMainMenu();

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
                    SortData();
                    break;
                case 4:
                    FilterData();
                    break;
                case 0:
                    Console.Clear();
                    Console.WriteLine("╔════════════════════════════════════════════════════════════════╗");
                    Console.WriteLine("║                           AU REVOIR !                          ║");
                    Console.WriteLine("╚════════════════════════════════════════════════════════════════╝\n");
                    return;
                default:
                    Console.WriteLine("Choix invalide. Veuillez choisir une option valide.");
                    break;
            }
        }
    }

    static void DisplayMainMenu()
    {
        Console.Clear();
        Console.WriteLine("╔════════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║                        MENU PRINCIPAL                          ║");
        Console.WriteLine("╠════════════════════════════════════════════════════════════════╣");
        Console.WriteLine("║  1. Transformer une source de données en une autre             ║");
        Console.WriteLine("║  2. Recherche sur plusieurs sources de données                 ║");
        Console.WriteLine("║  3. Trier les résultats                                        ║");
        Console.WriteLine("║  4. Condition de recherche (filtrage)                          ║");
        Console.WriteLine("║  0. Quitter                                                    ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════════════╝\n");
        Console.Write("Votre choix : ");
    }

    static void DisplayReturnToMenu()
    {
        Console.WriteLine("\nAppuyez sur une touche pour revenir au menu principal...");
        Console.ReadKey();
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
        Console.Clear();   
        Console.WriteLine("╔════════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║                       TRANSFORMATION                           ║");
        Console.WriteLine("╠════════════════════════════════════════════════════════════════╣");
        Console.WriteLine("║ Transformation du fichier films.xml en texte                   ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════════════╝\n");

        // Transformation (de XML à texte)
        var transformedData = moviesFromXml.Select(m => $"{m.Title}; {m.Genre}; {m.MainActor}; {m.Director}");
        foreach (var item in transformedData)
        {
            Console.WriteLine(item);
        }

        DisplayReturnToMenu();
    }

    static void SearchData()
    {
        // Recherche sur plusieurs sources de données (par titre)
        Console.Clear();
        Console.WriteLine("╔════════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║                         RECHERCHE                              ║");
        Console.WriteLine("╠════════════════════════════════════════════════════════════════╣");
        Console.WriteLine("║ Quel est le titre que vous recherchez ?                        ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════════════╝\n");
        Console.Write("Votre choix : ");
        string searchTerm = Console.ReadLine();
        var searchResultsXml = moviesFromXml.Where(m => m.Title.Contains(searchTerm, StringComparison.InvariantCultureIgnoreCase));
        var searchResultsTxt = moviesFromText.Where(m => m.Title.Contains(searchTerm, StringComparison.InvariantCultureIgnoreCase));

        Console.WriteLine("\n╔════════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║                     Resultat du XML :                          ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════════════╝\n");
        foreach (var movie in searchResultsXml)
        {
            Console.WriteLine($"{movie.Title} - {movie.Genre} - {movie.MainActor} - {movie.Director}");
        }


        Console.WriteLine("\n╔════════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║                    Resultat du Texte :                         ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════════════╝\n");
        foreach (var movie in searchResultsTxt)
        {
            Console.WriteLine($"{movie.Title} - {movie.Genre} - {movie.MainActor} - {movie.Director}");
        }

        DisplayReturnToMenu();

    }

    static void SortData()
    {
        Console.Clear();
        Console.WriteLine("╔════════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║                      TRI DES DONNÉES                           ║");
        Console.WriteLine("╠════════════════════════════════════════════════════════════════╣");
        Console.WriteLine("║ Choisissez un sens :                                           ║");
        Console.WriteLine("║ 1. Croissant                                                   ║");
        Console.WriteLine("║ 2. Décroissant                                                 ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════════════╝\n");
        Console.Write("Votre choix : ");

        int sens = int.Parse(Console.ReadLine());

        Console.Clear();
        Console.WriteLine("╔════════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║                      TRI DES DONNÉES                           ║");
        Console.WriteLine("╠════════════════════════════════════════════════════════════════╣");
        Console.WriteLine("║ Choisissez un critère :                                        ║");
        Console.WriteLine("║ 1. Titre                                                       ║");
        Console.WriteLine("║ 2. Genre                                                       ║");
        Console.WriteLine("║ 3. Acteur principal                                            ║");
        Console.WriteLine("║ 4. Réalisateur                                                 ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════════════╝\n");
        Console.Write("Votre choix : ");

        int choice = int.Parse(Console.ReadLine());

        IEnumerable<Movie> filteredData = Enumerable.Empty<Movie>();

        switch (choice)
        {
            case 1:
                switch (sens)
                {
                    case 1:
                        filteredData = moviesFromXml.OrderBy(m => m.Title);
                        Console.WriteLine("Affichage : Titre - Genre - Acteur principal - Réalisateur \n");
                        foreach (var movie in filteredData)
                        {
                            Console.WriteLine($"{movie.Title} - {movie.Genre} - {movie.MainActor} - {movie.Director}");
                        }
                        break;
                    case 2:
                        filteredData = moviesFromXml.OrderByDescending(m => m.Title);
                        Console.WriteLine("Affichage : Titre - Genre - Acteur principal - Réalisateur \n");
                        foreach (var movie in filteredData)
                        {
                            Console.WriteLine($"{movie.Title} - {movie.Genre} - {movie.MainActor} - {movie.Director}");
                        }
                        break;
                }
                break;
            case 2:
                switch (sens)
                {
                    case 1:
                        filteredData = moviesFromXml.OrderBy(m => m.Genre);
                        Console.WriteLine("Affichage : Genre - Titre - Acteur principal - Réalisateur \n");
                        foreach (var movie in filteredData)
                        {
                            Console.WriteLine($"{movie.Genre} - {movie.Title} - {movie.MainActor} - {movie.Director}");
                        }
                        break;
                    case 2:
                        filteredData = moviesFromXml.OrderByDescending(m => m.Genre);
                        Console.WriteLine("Affichage : Genre - Titre - Acteur principal - Réalisateur \n");
                        foreach (var movie in filteredData)
                        {
                            Console.WriteLine($"{movie.Genre} - {movie.Title} - {movie.MainActor} - {movie.Director}");
                        }
                        break;
                }
                break;
            case 3:
                switch (sens)
                {
                    case 1:
                        filteredData = moviesFromXml.OrderBy(m => m.MainActor);
                        Console.WriteLine("Affichage : Acteur principal - Titre - Genre - Réalisateur \n");
                        foreach (var movie in filteredData)
                        {
                            Console.WriteLine($"{movie.MainActor} - {movie.Title} - {movie.Genre} - {movie.Director}");
                        }
                        break;
                    case 2:
                        filteredData = moviesFromXml.OrderByDescending(m => m.MainActor);
                        Console.WriteLine("Affichage : Acteur principal - Titre - Genre - Réalisateur \n");
                        foreach (var movie in filteredData)
                        {
                            Console.WriteLine($"{movie.MainActor} - {movie.Title} - {movie.Genre} - {movie.Director}");
                        }
                        break;
                }
                break;
            case 4:
                switch (sens)
                {
                    case 1:
                        filteredData = moviesFromXml.OrderBy(m => m.Director);
                        Console.WriteLine("Affichage : Réalisateur - Titre - Genre - Acteur principal \n");
                        foreach (var movie in filteredData)
                        {
                            Console.WriteLine($"{movie.Director} - {movie.Title} - {movie.Genre} - {movie.MainActor}");
                        }
                        break;
                    case 2:
                        filteredData = moviesFromXml.OrderByDescending(m => m.Director);
                        Console.WriteLine("Affichage : Réalisateur - Titre - Genre - Acteur principal \n");
                        foreach (var movie in filteredData)
                        {
                            Console.WriteLine($"{movie.Director} - {movie.Title} - {movie.Genre} - {movie.MainActor}");
                        }
                        break;
                }
                break;
            default:
                Console.WriteLine("Choix invalide.");
                return;
        }

        DisplayReturnToMenu();

    }

    static void FilterData()
    {

        Console.Clear();
        Console.WriteLine("╔════════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║                    FILTRAGE DES DONNÉES                        ║");
        Console.WriteLine("╠════════════════════════════════════════════════════════════════╣");
        Console.WriteLine("║ Choisissez un critère de recherche:                            ║");
        Console.WriteLine("║ 1. Titre                                                       ║");
        Console.WriteLine("║ 2. Genre                                                       ║");
        Console.WriteLine("║ 3. Acteur principal                                            ║");
        Console.WriteLine("║ 4. Réalisateur                                                 ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════════════╝\n");
        Console.Write("Votre choix : ");

        int choice = int.Parse(Console.ReadLine());

        Console.WriteLine("\nEntrez la valeur de recherche : ");
        string searchValue = Console.ReadLine();

        IEnumerable<Movie> filteredData = Enumerable.Empty<Movie>();

        switch (choice)
        {
            case 1:
                filteredData = moviesFromXml.Where(m => m.Title.Contains(searchValue, StringComparison.InvariantCultureIgnoreCase));
                Console.WriteLine("Affichage : Titre - Genre - Acteur principal - Réalisateur \n");
                foreach (var movie in filteredData)
                {
                    Console.WriteLine($"{movie.Title} - {movie.Genre} - {movie.MainActor} - {movie.Director}");
                }
                break;
            case 2:
                filteredData = moviesFromXml.Where(m => m.Genre.Contains(searchValue, StringComparison.InvariantCultureIgnoreCase));
                Console.WriteLine("Affichage : Genre - Titre - Acteur principal - Réalisateur \n");
                foreach (var movie in filteredData)
                {
                    Console.WriteLine($"{movie.Genre} - {movie.Title} - {movie.MainActor} - {movie.Director}");
                }
                break;
            case 3:
                filteredData = moviesFromXml.Where(m => m.MainActor.Contains(searchValue, StringComparison.InvariantCultureIgnoreCase));
                Console.WriteLine("Affichage : Acteur principal - Titre - Genre - Réalisateur \n");
                foreach (var movie in filteredData)
                {
                    Console.WriteLine($"{movie.MainActor} - {movie.Title} - {movie.Genre} - {movie.Director}");
                }
                break;
            case 4:
                filteredData = moviesFromXml.Where(m => m.Director.Contains(searchValue, StringComparison.InvariantCultureIgnoreCase));
                Console.WriteLine("Affichage : Réalisateur - Titre - Genre - Acteur principal \n");
                foreach (var movie in filteredData)
                {
                    Console.WriteLine($"{movie.Director} - {movie.Title} - {movie.Genre} - {movie.MainActor}");
                }
                break;
            default:
                Console.WriteLine("Choix invalide.");
                return;
        }

        DisplayReturnToMenu();

    }
}
