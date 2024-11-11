import { Navigate, NavLink } from 'react-router-dom';
import auth from '../../lib/api/auth';
import { useState } from 'react';

function Register() {

    const [credentials, setCredentials] = useState({
        username: '',
        password: '',
        passwordConfirmation: '',
        email: ''
    });

    const [error, setError] = useState('');

    const [isSignedUp, setIsSignedUp] = useState(false);

    const [terms, setTerms] = useState(false);
    const [privacy, setPrivacy] = useState(false);

    const handleChange = (e) => {
        setCredentials({
            ...credentials,
            [e.target.name]: e.target.value
        });
    }

    const handleTerms = (e) => {
        setTerms(e.target.checked);
    }

    const handlePrivacy = (e) => {
        setPrivacy(e.target.checked);
    }

    const handleRegister = async (e) => {
        e.preventDefault();
        setError(null);

        console.log(terms)
        console.log(privacy)

        if (!passwordConfirmation()) {
            console.log('password confirmation');
            return;
        }

        if (termsAndConditions(e)) {
            console.log('terms and conditions');
            return;
        }

        if (privacyPolicy(e)) {
            console.log('privacy policy');
            return;
        }

        try {
            await auth.register({ username: credentials.username, password: credentials.password, email: credentials.email, passwordConfirmation: credentials.passwordConfirmation }).then((response) => {
                console.log(response);
                setIsSignedUp(true);
            });
        } catch (error) {
            setError(error);
        }
    }

    const passwordConfirmation = (e) => {
        if (credentials.password !== credentials.passwordConfirmation) {
            setError("Passwords do not match. Please try again.");
            return false;
        }
        return true;
    }

    const termsAndConditions = (e) => {
        if (terms) {
            setError("You must agree to the terms and conditions to proceed.");
            return false;
        }
        return true;
    }

    const privacyPolicy = (e) => {
        if (privacy) {
            setError("You must agree to the privacy policy to proceed.");
            return false;
        }
        return true;
    }

    if (isSignedUp) {
        return <Navigate to="/login" />
    }

    if (localStorage.getItem('user')) {
        return <Navigate to="/" />
    }

    return (
        <div>
            <h1>Register</h1>
            <form onSubmit={handleRegister}>
                <input type="text" placeholder="Username" onChange={handleChange} name='username' value={credentials.username} />
                <input type="text" placeholder="Email" onChange={handleChange} name='email' value={credentials.email} />
                <input type="password" placeholder="Password" onChange={handleChange} name='password' value={credentials.password} />
                <input type="password" placeholder="Confirm Password" onChange={handleChange} name='passwordConfirmation' value={credentials.passwordConfirmation} /><br /><br />
                <input type="checkbox" name="terms" onChange={handleTerms} /> I agree to the <NavLink to={"/terms-and-conditions"} >terms and conditions</NavLink><br /><br />
                <input type="checkbox" name="privacy" onChange={handlePrivacy} /> I agree to the <NavLink to={"/privacy-policy"}>privacy policy</NavLink><br /><br />
                <button type="submit">Register</button>
            </form>
            {error && <p>{error.message}</p>}
        </div>
    )
}

export default Register;