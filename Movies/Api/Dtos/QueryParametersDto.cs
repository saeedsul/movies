namespace Api.Dtos
{
    public class QueryParametersDto
    {
        public string? Title { get; set; }
        public string? Genre { get; set; }
        public int? ReleaseYear { get; set; }
        public int Limit { get; set; } = 10;
        public int Page { get; set; } = 1;
        public string SortBy { get; set; } = "Title";
        public string SortOrder { get; set; } = "asc";
    }

}
