import base from './base';

const login = async ({
    username,
    password
}) => {
    console.log(username, password);
    return await base.post('/login', {
        "username": username,
        "password": password
    });
}

const logout = async () => {

    return await base.post('/logout');
}

const register = async ({
    email,
    password,
    passwordConfirmation,
    username
}) => {

    return await base.post('/register', {
        "email": email,
        "password": password,
        "passwordConfirmation": passwordConfirmation,
        "username": username
    });
}
const refresh = async () => {

    return await base.post('/refresh');
}

const revoke = async () => {

    return await base.post('/revoke');
}

const deleteUser = async ({
    id
}) => {

    return await base.delete('/user/' + id);

}

export default {
    login,
    logout,
    register,
    refresh,
    revoke,
    deleteUser
}