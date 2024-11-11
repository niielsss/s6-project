import movieAPI from '../../lib/api/movie'
import auth from '../../lib/api/auth'
import { useState } from 'react';

function CreateMovie() {

    const [error, setError] = useState(null);
    const [movie, setMovie] = useState({
        title: '',
        plotSummary: '',
        releaseDate: '',
        genre: '',
        duration: '',
        productionCompany: ''
    });

    const handleChange = (e) => {

        setMovie({
            ...movie,
            [e.target.name]: e.target.value
        });

    }

    const handleSubmit = (e) => {

        e.preventDefault();
        setError(null);

        movieAPI.postMovie({
            title: movie.title,
            plotSummary: movie.plotSummary,
            releaseDate: movie.releaseDate,
            genre: movie.genre,
            duration: movie.duration,
            productionCompany: movie.productionCompany
        }).then((response) => {
            console.log(response.data);
        }).catch((error) => {
            console.log(error);
            //setError(error);
        });
    }

    // logged in
    // admin
    if (!localStorage.getItem('user')) {
        return;
    }

    const checkAdmin = async () => {
        let token = JSON.parse(localStorage.getItem('user'));
        let base64Url = token.split('.')[1];
        let base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
        let jsonPayload = decodeURIComponent(atob(base64).split('').map(function (c) {
            return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
        }).join(''));

        let user = JSON.parse(jsonPayload);
        return user.Role === 'Admin';
    }

    if (!checkAdmin()) {
        return;
    }

    return (
        <div>
            <h1>Create Movie</h1>
            <form onSubmit={handleSubmit}>
                <label>
                    Title:
                    <input type="text" name="title" onChange={handleChange} />
                </label>
                <label>
                    Plot Summary:
                    <input type="text" name="plotSummary" onChange={handleChange} />
                </label>
                <label>
                    Release Date:
                    <input type="date" name="releaseDate" onChange={handleChange} />
                </label>
                <label>
                    Genre:
                    <input type="text" name="genre" onChange={handleChange} />
                </label>
                <label>
                    Duration:
                    <input type="text" name="duration" onChange={handleChange} />
                </label>
                <label>
                    Production Company:
                    <input type="text" name="productionCompany" onChange={handleChange} />
                </label>
                <button type="submit">Create</button>
            </form>
            {error && <p>{error}</p>}
        </div>
    );
}

export default CreateMovie;