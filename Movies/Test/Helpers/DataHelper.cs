using Api.Models; 

namespace Test.Helpers
{ 
    public static class DataHelper
    {
        public static List<Movie> GetFakeMovieList()
        {
            return
            [
                        new(){
                            Title = "Beyond the Ultimate Spin: The Making of 'Spider-Man'",
                            Overview = "Documentary on the making of 'Spider-Man.'",
                            ReleaseDate = new DateTime(2002, 4, 20),
                            Genre = "Documentary",
                            PosterUrl = "https://image.tmdb.org/t/p/original/oPS6xwFz5Ej9s7LvkGHtOukJTrC.jpg",
                            VoteAverage = 3.5,
                            VoteCount = 2,
                            OriginalLanguage = "en",
                            Popularity = 15.325
                        },
                        new()
                        {
                            Title = "Jack Black: Spider-Man",
                            Overview = "Peter Parker is no ordinary kid, but what if he was played by Jack Black? That's right, you got yourself some pretty irresponsible hero!",
                            ReleaseDate = new DateTime(2002, 6, 6),
                            Genre = "Comedy",
                            PosterUrl = "https://image.tmdb.org/t/p/original/rhU1aBcLhbqcesDOn6cjmz6sjos.jpg",
                            VoteAverage = 6.7,
                            VoteCount = 21,
                            OriginalLanguage = "en",
                            Popularity = 35.66
                        },
                        new()
                        {
                            Title = "LEGO Marvel Spider-Man: Vexed by Venom",
                            Overview = "Thanks to Green Goblin and Venom, tech theft is now at an all-time high...",
                            ReleaseDate = new DateTime(2019, 8, 3),
                            Genre = "Animation, Action",
                            PosterUrl = "https://image.tmdb.org/t/p/original/gTo2r8nNU3ZYAS6DqdeSp1VEqkq.jpg",
                            VoteAverage = 7.5,
                            VoteCount = 12,
                            OriginalLanguage = "en",
                            Popularity = 20.355
                        },
                        new()
                        {
                            Title = "Spider-Man",
                            Overview = "When an extortionist threatens to force a multi-suicide unless a huge ransom is paid...",
                            ReleaseDate = new DateTime(1977, 9, 14),
                            Genre = "Science Fiction, Action, Crime, TV Movie",
                            PosterUrl = "https://image.tmdb.org/t/p/original/e7pbfh9GWn2oj72aEpUG8IH31dH.jpg",
                            VoteAverage = 5.5,
                            VoteCount = 77,
                            OriginalLanguage = "en",
                            Popularity = 50.753
                        },
                        new()
                        {
                            Title = "Spider-Man",
                            Overview = "After being bitten by a genetically altered spider at Oscorp...",
                            ReleaseDate = new DateTime(2002, 5, 1),
                            Genre = "Fantasy, Action",
                            PosterUrl = "https://image.tmdb.org/t/p/original/gh4cZbhZxyTbgxQPxD0dOudNPTn.jpg",
                            VoteAverage = 7.2,
                            VoteCount = 15336,
                            OriginalLanguage = "en",
                            Popularity = 206.376
                        },
                        new()
                        {
                            Title = "Spider-Man 2",
                            Overview = "Peter Parker is going through a major identity crisis...",
                            ReleaseDate = new DateTime(2004, 6, 25),
                            Genre = "Action, Adventure, Fantasy",
                            PosterUrl = "https://image.tmdb.org/t/p/original/olxpyq9kJAZ2NU1siLshhhXEPR7.jpg",
                            VoteAverage = 7.2,
                            VoteCount = 1231,
                            OriginalLanguage = "en",
                            Popularity = 53.75
                        }
            ];
        }

       


    }
}
