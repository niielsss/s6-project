import React, { useState } from 'react';
import auth from '../../lib/api/auth';
import { useAuth } from '../../lib/hooks/useAuth';
import { Navigate } from 'react-router-dom';

function Login() {

    const { login } = useAuth();

    const [credentials, setCredentials] = useState({
        username: '',
        password: ''
    });

    const [error, setError] = useState(null);

    const handleChange = (e) => {
        setCredentials({
            ...credentials,
            [e.target.name]: e.target.value
        });
    }

    const handleLogin = async (e) => {
        e.preventDefault();
        setError(null);
        //console.log(credentials)
        try {

            await auth.login({ username: credentials.username, password: credentials.password }).then((response) => {
                console.log(response);
                login(response.data.token);
                window.location.reload();
            });

        } catch (error) {
            setError(error);
        }
    }

    if (localStorage.getItem('user')) {
        return <Navigate to="/" />
    }

    return (
        <div>
            <h1>Login</h1>
            <form onSubmit={handleLogin}>
                <input type="text" placeholder="Username" onChange={handleChange} name='username' value={credentials.username} />
                <input type="password" placeholder="Password" onChange={handleChange} name='password' value={credentials.password} />
                <button type="submit">Login</button>
            </form>
            {error && <p>{error.message}</p>}
        </div>
    )
}

export default Login;