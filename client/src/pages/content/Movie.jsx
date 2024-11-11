import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import movie from '../../lib/api/movie'
import review from '../../lib/api/review'

function Movie() {

    const { id } = useParams();
    const [data, setData] = useState();

    useEffect(() => {

        movie.getMovie(id).then((response) => {
            console.log(response.data);
            setData(response.data);
        });
    }, [])

    const loggedIn = () => localStorage.getItem('user') ? true : false;

    const [error, setError] = useState(null);

    const getUserId = () => {

        let token = JSON.parse(localStorage.getItem('user'));
        console.log(token);
        // Parse jwt token
        let base64Url = token.split('.')[1];
        let base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
        let jsonPayload = decodeURIComponent(atob(base64).split('').map(function (c) {
            return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
        }).join(''));

        let user = JSON.parse(jsonPayload);
        console.log(user);

        return user.nameid;
    }

    const handleSubmit = (e) => {
        e.preventDefault();
        const rating = e.target.rating.value;
        const comment = e.target.comment.value;
        review.postReview({ userId: getUserId(), movieId: id, rating: rating, comment: comment }).then((response) => {
            console.log(response.data);
        }).catch((error) => {
            setError(error);
        });

        movie.getMovie(id).then((response) => {
            console.log(response.data);
            setData(response.data);
        });
    }

    return (
        <div>
            <h1>Movie</h1>
            <p>{id}</p>
            <p>{data?.title}</p>
            <p>{data?.plotSummary}</p>
            <p>{data?.productionCompany}</p>
            <p>{Date(data?.releaseDate)}</p>
            <p>{data?.duration}</p>
            <p>{data?.genre}</p>
            <h2>Reviews</h2>
            <div>
                {
                    data?.reviews?.result.map((review) => {
                        return (
                            <div key={review.id}>
                                <p>user: {review.userId}</p>
                                <p>rating: {review.rating}</p>
                                <p>comment: {review.comment}</p>
                            </div>
                        )
                    })
                }
                <p>count: {data?.reviews?.count}</p>
                <p>current page: {data?.reviews?.currentPage}</p>
            </div>
            {
                loggedIn() ?
                    <div>
                        <h2>Leave a review</h2>
                        <form onSubmit={handleSubmit}>
                            <label>
                                Rating:
                                <input type="number" name="rating" min={1} max={5} />
                            </label>
                            <label>
                                Comment:
                                <input type="text" name="comment" />
                            </label>
                            <button type="submit">Submit</button>
                        </form>
                    </div>
                    :
                    <p>Log in to leave a review</p>
            }
        </div>
    )
}

export default Movie;