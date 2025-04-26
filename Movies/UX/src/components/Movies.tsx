import { useCallback, useEffect, useState } from 'react';
import { handleRequest } from '../utils/requestWrapper';
import { getAll, Filters, Movie } from "../apis/api";


export default function Movies() {
    const [movies, setMovies] = useState<Movie[]>([]);
    const [totalCount, setTotalCount] = useState(0);
    const [filters, setFilters] = useState<Filters>({
        title: '',
        genre: '',
        releaseYear: null,
        limit: 5,
        page: 1,
        sortBy: 'Title',
        sortOrder: 'asc'
    });
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);

    const totalPages = Math.ceil(totalCount / (filters.limit || 5));
     
    const [searchInputs, setSearchInputs] = useState({
        title: '',
        genre: '',
        releaseYear: '',
        sortBy: 'Title',
        sortOrder: 'asc',
        limit: '5'
    });

    const fetchMovies = useCallback(async () => {
        setIsLoading(true);
        setError(null);

        const [response, err] = await handleRequest({ promise: getAll(filters) });

        if (err) {
            console.error("Error fetching movies", err);
            setMovies([]);
            setTotalCount(0);
            setError('Failed to fetch movies. Please try again later.');
        } else if (response?.success && response.data) {
            setMovies(response.data.movies || []);
            setTotalCount(response.data.totalRecords || 0);
        }

        setIsLoading(false);
    }, [filters]);

    useEffect(() => {
        fetchMovies();
    }, [fetchMovies]);

    const handleInputChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
        const { name, value } = e.target;
        setSearchInputs((prev) => ({
            ...prev,
            [name]: value
        }));
    };

    const handleSearchSubmit = (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        setFilters({
            title: searchInputs.title.trim(),
            genre: searchInputs.genre.trim() || null,
            releaseYear: searchInputs.releaseYear ? parseInt(searchInputs.releaseYear) : null,
            limit: searchInputs.limit ? parseInt(searchInputs.limit) : 5,
            sortBy: searchInputs.sortBy,
            sortOrder: searchInputs.sortOrder as 'asc' | 'desc',
            page: 1  
        });
    };

    const handlePreviousPage = () => {
        setFilters((prev) => ({
            ...prev,
            page: Math.max((prev.page || 1) - 1, 1)
        }));
    };

    const handleNextPage = () => {
        setFilters((prev) => ({
            ...prev,
            page: Math.min((prev.page || 1) + 1, totalPages)
        }));
    };

    const handleReset = () => {
        setSearchInputs({
            title: '',
            genre: '',
            releaseYear: '',
            sortBy: 'Title',
            sortOrder: 'asc',
            limit: '5'
        });

        setFilters({
            title: '',
            genre: '',
            releaseYear: null,
            limit: 5,
            page: 1,
            sortBy: 'Title',
            sortOrder: 'asc'
        });
    };


    return (
        <div style={{ padding: '20px', fontFamily: 'Arial, sans-serif' }}>
            <h1>🎬 Movies List</h1>
             
            <form onSubmit={handleSearchSubmit} style={{ marginBottom: '20px', display: 'grid', gridTemplateColumns: 'repeat(auto-fit, minmax(200px, 1fr))', gap: '10px' }}>
                <input
                    type="text"
                    name="title"
                    placeholder="Search by title..."
                    value={searchInputs.title}
                    onChange={handleInputChange}
                    style={inputStyle}
                />
                <input
                    type="text"
                    name="genre"
                    placeholder="Search by genre..."
                    value={searchInputs.genre}
                    onChange={handleInputChange}
                    style={inputStyle}
                />
                <input
                    type="number"
                    name="releaseYear"
                    placeholder="Search by release year..."
                    value={searchInputs.releaseYear}
                    onChange={handleInputChange}
                    style={inputStyle}
                />
                <select
                    name="sortBy"
                    value={searchInputs.sortBy}
                    onChange={handleInputChange}
                    style={inputStyle}
                >
                    <option value="Title">Sort by Title</option>
                    <option value="ReleaseDate">Sort by Release Year</option>
                </select>
                <select
                    name="sortOrder"
                    value={searchInputs.sortOrder}
                    onChange={handleInputChange}
                    style={inputStyle}
                >
                    <option value="asc">Ascending</option>
                    <option value="desc">Descending</option>
                </select>
                <select
                    name="limit"
                    value={searchInputs.limit}
                    onChange={handleInputChange}
                    style={inputStyle}
                >
                    <option value="5">5 per page</option>
                    <option value="10">10 per page</option>
                    <option value="20">20 per page</option>
                </select>
                <div style={{ display: 'flex', gap: '10px' }}>
                    <button type="submit" style={buttonStyle}>
                        Search
                    </button>
                    <button type="button" style={{ ...buttonStyle, backgroundColor: '#666' }} onClick={handleReset}>
                        Reset
                    </button>
                </div>
            </form>
             
            {isLoading && <p>Loading movies...</p>}
             
            {error && (
                <div style={{ color: 'red', marginBottom: '10px' }}>
                    {error}
                </div>
            )}
             
            {!isLoading && !error && movies.length === 0 && (
                <p>No movies found.</p>
            )}
             
            <div style={{ display: 'grid', gridTemplateColumns: 'repeat(auto-fill, minmax(250px, 1fr))', gap: '20px' }}>
                {movies.map((movie) => (
                    <div
                        key={movie.id}
                        style={{
                            border: '1px solid #ccc',
                            borderRadius: '8px',
                            padding: '10px',
                            textAlign: 'center'
                        }}
                    >
                        <img
                            src={movie.posterUrl}
                            alt={movie.title}
                            style={{ width: '100%', height: '350px', objectFit: 'cover', borderRadius: '4px' }}
                        />
                        <h2 style={{ fontSize: '18px', marginTop: '10px' }}>{movie.title}</h2>
                        <p style={{ fontSize: '14px', color: '#666' }}>{movie.genre}</p>
                        <p style={{ fontSize: '12px', color: '#888' }}>Released: {new Date(movie.releaseDate).toLocaleDateString()}</p>
                        <p style={{ fontSize: '14px', marginTop: '5px' }}>⭐ {movie.voteAverage} ({movie.voteCount} votes)</p>
                    </div>
                ))}
            </div>
             
            {!isLoading && !error && movies.length > 0 && (
                <div style={{ marginTop: '20px', display: 'flex', justifyContent: 'center', alignItems: 'center', gap: '10px' }}>
                    <button
                        disabled={filters.page === 1}
                        onClick={handlePreviousPage}
                        style={buttonStyle}
                    >
                        Previous
                    </button>

                    <span>Page {filters.page} of {totalPages}</span>

                    <button
                        disabled={filters.page === totalPages}
                        onClick={handleNextPage}
                        style={buttonStyle}
                    >
                        Next
                    </button>
                </div>
            )}
        </div>
    );
}

 
const inputStyle = {
    padding: '8px',
    border: '1px solid #ccc',
    borderRadius: '4px'
};

const buttonStyle = {
    padding: '8px 16px',
    backgroundColor: '#0070f3',
    color: 'white',
    border: 'none',
    borderRadius: '4px',
    cursor: 'pointer'
};