namespace Api.Dtos
{
    public class MovieDto
    {
        public string? Title { get; set; } = string.Empty;
        public string? Overview { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public string? Genre { get; set; }
        public string? PosterUrl { get; set; }
        public double? VoteAverage { get; set; }
        public int? VoteCount { get; set; }
        public string? OriginalLanguage { get; set; }
        public double? Popularity { get; set; }
    }

}
