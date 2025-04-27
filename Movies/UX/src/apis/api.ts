import axios from '../utils/axiosInstance';

export interface Movie {
    id: string;
    title: string;
    overview: string;
    releaseDate: string;
    genre: string;
    posterUrl: string;
    voteAverage: number;
    voteCount: number;
    originalLanguage: string;
    popularity: number;
}

export interface Filters {
    title?: string;
    genre?: string | null;
    releaseYear?: number | null;
    limit?: number;
    page?: number;
    sortBy?: string;
    sortOrder?: 'asc' | 'desc';
}

interface MovieResponse {
    success: boolean,
    message?: string | null;
    data: {
        movies: Movie[];
        totalRecords: number
    }; 
}

export const getAll = async (params: Filters): Promise<MovieResponse | null> => {
    try {
        const response = await axios.get<MovieResponse>("/api/movie/search", { params });
        return response.data;
    } catch (error) {
        console.error("Failed to fetch movies:", error);
        return null;
    }
};
