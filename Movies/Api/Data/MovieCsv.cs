using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;

namespace Api.Data
{
    public class MovieCsv
    {
        [Name("Release_Date")]
        public string? ReleaseDate { get; set; }

        [Name("Title")]
        public string? Title { get; set; }

        [Name("Overview")]
        public string? Overview { get; set; }
        [Name("Popularity")]
        public double? Popularity { get; set; }

        [Name("Vote_Count")]
        public int? VoteCount { get; set; }

        [Name("Vote_Average")]
        public double? VoteAverage { get; set; }
        [Name("Original_Language")]
        public string? OriginalLanguage { get; set; }

        [Name("Genre")]
        public string? Genre { get; set; }        

        [Name("Poster_Url")]
        public string? PosterUrl { get; set; }
    } 
}
