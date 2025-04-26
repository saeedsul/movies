using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    public class Movie
    {
        [Key]
        public Guid Id { get; set; }
         
        public string? Title { get; set; } = string.Empty;
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
}
