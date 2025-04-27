using Microsoft.Extensions.Configuration;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.ComponentModel.DataAnnotations;
using CsvHelper.Configuration.Attributes;
using Microsoft.EntityFrameworkCore; 

class Program
{
    private static ApplicationDbContext _context;

    static async Task Main(string[] args)
    {
        var configuration = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appsettings.json")
                            .Build();

        string connectionString = configuration.GetConnectionString("dbConnectionString");

        // Initialize DbContext
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseSqlServer(connectionString);
        _context = new ApplicationDbContext(optionsBuilder.Options);

        try
        {
            // Create database if it doesn't exist
            await _context.Database.EnsureCreatedAsync();

            // Check if Movies table is empty
            if (!await _context.Movies.AnyAsync())
            {
                var movies = LoadMoviesFromCsv();
                await _context.Movies.AddRangeAsync(movies);
                await _context.SaveChangesAsync();
                Console.WriteLine($"Successfully imported {movies.Count()} movies.");
            }
            else
            {
                Console.WriteLine("Database already contains movies. Skipping import.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
            }
        }
        finally
        {
            await _context.DisposeAsync();
        }
    }

    private static IEnumerable<Movie> LoadMoviesFromCsv()
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "mymoviedb.csv");

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"The CSV file was not found: {filePath}");
        }

        var movies = new List<Movie>();

        using (var reader = new StreamReader(filePath))
        using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
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
        }))
        {
            csv.Context.RegisterClassMap<MovieCsvMap>();

            try
            {
                var records = csv.GetRecords<MovieCsv>();
                foreach (var record in records)
                {
                    try
                    {
                        var movie = new Movie
                        {
                            Id = Guid.NewGuid(),
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
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error processing record at row {csv.Context.Parser.Row}: {ex.Message}");
                        continue;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading CSV file: {ex.Message}");
            }
        }

        return movies;
    }

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Movie> Movies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Movie>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Genre).IsRequired().HasMaxLength(250);
            });
        }
    }

    public class Movie
    {
        [Key]
        public Guid Id { get; set; }

        public string? Title { get; set; }
        public string? Overview { get; set; }
        public DateTime? ReleaseDate { get; set; }

        [Required]
        [MaxLength(250)]
        public string? Genre { get; set; }
        public double? Popularity { get; set; }
        public int? VoteCount { get; set; }
        public double? VoteAverage { get; set; }
        public string? OriginalLanguage { get; set; }
        public string? PosterUrl { get; set; }
    }

    public class MovieCsv
    {
        [Name("Release_Date")]
        public string? ReleaseDate { get; set; }

        [Name("Title")]
        public string? Title { get; set; }

        [Name("Overview")]
        public string? Overview { get; set; }

        [Name("Popularity")]
        public double Popularity { get; set; }

        [Name("Vote_Count")]
        public int VoteCount { get; set; }

        [Name("Vote_Average")]
        public double VoteAverage { get; set; }

        [Name("Original_Language")]
        public string? OriginalLanguage { get; set; }

        [Name("Genre")]
        public string? Genre { get; set; }

        [Name("Poster_Url")]
        public string? PosterUrl { get; set; }
    }

    public sealed class MovieCsvMap : ClassMap<MovieCsv>
    {
        public MovieCsvMap()
        {
            Map(m => m.ReleaseDate).Name("Release_Date");
            Map(m => m.Title).Name("Title");
            Map(m => m.Overview).Name("Overview");
            Map(m => m.Popularity).Name("Popularity");
            Map(m => m.VoteCount).Name("Vote_Count");
            Map(m => m.VoteAverage).Name("Vote_Average");
            Map(m => m.OriginalLanguage).Name("Original_Language");
            Map(m => m.Genre).Name("Genre");
            Map(m => m.PosterUrl).Name("Poster_Url");
        }
    }
}