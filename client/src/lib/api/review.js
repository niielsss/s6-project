import base from './base';

const postReview = async ({
    userId,
    movieId,
    rating,
    comment
}) => {
    return await base.post('/review', {
        "userId": userId,
        "movieId": movieId,
        "rating": rating,
        "comment": comment
    });
}

export default {
    postReview
}