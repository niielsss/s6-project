import base from './base';

const getMovies = async ({
    id = '',
    currentPage = 1,
    pageSize = 10,
    orderBy = 'id',
    sortBy = 'asc'
} = {}) => {
    const query = new URLSearchParams({
        currentPage,
        pageSize,
        orderBy,
        sortBy
    }).toString();
    return base.get(`/movie?${query}`);
}


const getMovie = async (id) => {
    return await base.get(`/movie/${id}`);
}

const postMovie = async ({
    title,
    plotSummary,
    releaseDate,
    genre,
    duration,
    productionCompany
}) => {
    return await base.post('/movie', {
        "title": title,
        "plotSummary": plotSummary,
        "releaseDate": new Date(),
        "genre": parseInt(genre),
        "duration": parseInt(duration),
        "productionCompany": productionCompany
    });
}

const putMovie = async (id, {
    title,
    plotSummary,
    releaseDate,
    genre,
    duration,
    productionCompany
}) => {
    return await base.put(`/movie/${id}`, {
        "id": id,
        "title": title,
        "plotSummary": plotSummary,
        "releaseDate": releaseDate,
        "genre": genre,
        "duration": duration,
        "productionCompany": productionCompany
    });
}

const deleteMovie = async (id) => {
    return await base.delete(`/movie/${id}`);
}

export default {
    getMovies,
    getMovie,
    postMovie,
    putMovie,
    deleteMovie
}