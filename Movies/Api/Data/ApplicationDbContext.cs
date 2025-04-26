using Api.Models;
using CsvHelper.Configuration;
using CsvHelper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System.Globalization;

namespace Api.Data
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    { 
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            RelationalDatabaseCreator? dbCreater = Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator;
            if (dbCreater != null)
            {
                // Create Database 
                if (!dbCreater.CanConnect())
                {
                    dbCreater.Create();
                }

                // Create Tables
                if (!dbCreater.HasTables())
                {
                    dbCreater.CreateTables();
                }
            }
            SeedDatabase();
        }

        public DbSet<Movie> Movies { get; set ; }

        private void SeedDatabase()
        { 
            if (!Movies.Any())
            {
                var movies = LoadMoviesFromCsv();  
                Movies.AddRange(movies);
                SaveChanges(); 
            }
        }

        private IEnumerable<Movie> LoadMoviesFromCsv()
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "mymoviedb.csv");

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"The CSV file was not found: {filePath}");
            }

            var movies = new List<Movie>();

            using (var reader = new StreamReader(filePath))
            {
                var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = true,
                    Delimiter = ",",
                    MissingFieldFound = null,
                    BadDataFound = null,  
                    ReadingExceptionOccurred = (ex) =>
                    { 
                        Console.WriteLine($"Error reading record: {ex.Exception.Message}");
                        return false;  
                    }
                });

                csv.Read();
                csv.ReadHeader();
                 
                while (csv.Read())
                {
                    try
                    {
                        var record = csv.GetRecord<MovieCsv>();
                        if (record != null)
                        {
                            var movie = new Movie
                            {
                                Title = record.Title,
                                Overview = record.Overview,
                                ReleaseDate = DateTime.TryParse(record.ReleaseDate, out var releaseDate)
                                    ? releaseDate
                                    : (DateTime?)null,
                                Genre = record.Genre,
                                Popularity = record.Popularity,
                                VoteCount = record.VoteCount,
                                VoteAverage = record.VoteAverage,
                                OriginalLanguage = record.OriginalLanguage,
                                PosterUrl = record.PosterUrl
                            };
                            movies.Add(movie);
                        }
                    }
                    catch (Exception ex)
                    { 
                        Console.WriteLine($"Error processing record at row {csv.Context.Parser.Row}: {ex.Message}");
                        continue;
                    }
                }
            }

            return movies;
        }
    }
}
