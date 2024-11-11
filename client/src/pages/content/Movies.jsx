import React, { useEffect, useState } from 'react';
import movie from '../../lib/api/movie';
import { Link } from 'react-router-dom';

function Movies() {

    const [data, setData] = useState();

    useEffect(() => {

        movie.getMovies().then((response) => {
            setData(response.data);
        });
    }, []);

    console.log(data);

    return (
        <div>
            <h1>Movies</h1>
            <div>
                {
                    data?.result.map((movie) => {
                        return (
                            <div key={movie.id}>
                                <Link to={`../movie/${movie.id}`}>
                                    {movie.id}. {movie.title}
                                </Link>
                            </div>
                        )
                    })
                }
            </div>
        </div>
    );
}

export default Movies;