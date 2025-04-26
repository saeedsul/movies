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
        movies: [];
        totalRecords: number
    }; 
}

export const getAll = async (params: Filters): Promise<MovieResponse> => {
    const response = await axios.get<MovieResponse>("/movie/search", { params });
    return response.data;
};
  