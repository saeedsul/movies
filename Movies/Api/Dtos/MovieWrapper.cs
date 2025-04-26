namespace Api.Dtos
{
    public class MovieWrapper
    {
        public IEnumerable<MovieDto>? Movies { get; set; }
        public int TotalRecords { get; set; }
        public object? SearchParameters { get; set; }

    }

}
