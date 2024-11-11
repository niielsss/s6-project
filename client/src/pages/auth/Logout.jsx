import { useEffect, useState } from 'react';
import { Navigate } from 'react-router-dom';

function Logout() {

    const [isLoggedOut, setIsLoggedOut] = useState(false);

    useEffect(() => {

        if (!localStorage.getItem('user')) {
            setIsLoggedOut(true);
            return;
        }
        //auth.logout();
        localStorage.removeItem('user');
        window.location.reload();
    }, []);

    if (isLoggedOut) {
        return <Navigate to={{pathname: '/'}} state={{reload: true}} />
    }

    return (
        <div>
            <h1>Logging out...</h1>
        </div>
    )

}

export default Logout;