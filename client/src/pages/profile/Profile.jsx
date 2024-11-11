import auth from '../../lib/api/auth';
import { Navigate } from 'react-router-dom';

function Profile() {

    const handleDelete = async () => {

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

        try {
            await auth.deleteUser({ id: user.nameid });
            localStorage.removeItem('user');
            window.location.reload();

            console.log('User deleted');
        } catch (error) {
            console.error(error);
        }

    }

    if (!localStorage.getItem('user')) {
        return <Navigate to={{ pathname: '/login' }} state={{ reload: true }} />
    }

    return (
        <div>
            <h1>Profile</h1>
            <button type="button" className="btn btn-primary" data-bs-toggle="modal" data-bs-target="#myModal">
                Delete your Account
            </button>
            <div className="modal" id="myModal">
                <div className="modal-dialog">
                    <div className="modal-content">

                        <div className="modal-header">
                            <h4 className="modal-title">Delete your account?</h4>
                            <button type="button" className="btn-close" data-bs-dismiss="modal"></button>
                        </div>

                        <div className="modal-body">
                            <p>Are you sure you want to delete your account?</p>
                            <p>This action is unreversible.</p>
                        </div>

                        <div className="modal-footer">
                            <button type="button" class="btn btn-danger" onClick={handleDelete}>Yes</button>
                            <button type="button" class="btn btn-danger" data-bs-dismiss="modal">Close</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
}

export default Profile;