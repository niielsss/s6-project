import './App.css';
import Navbar from './components/navbar/Navbar';
import { Route, Routes, BrowserRouter as Router, Navigate } from "react-router-dom";
import { AuthContext } from './lib/context/AuthContext';
import { useAuth } from './lib/hooks/useAuth';
import Login from './pages/auth/Login';
import Logout from './pages/auth/Logout';
import Movies from './pages/content/Movies';
import Register from './pages/auth/Register';
import Movie from './pages/content/Movie';
import TermsAndConditions from './pages/misc/TermsAndConditions';
import PrivacyPolicy from './pages/misc/PrivacyPolicy';
import Profile from './pages/profile/Profile';
import CreateMovie from './pages/content/CreateMovie';

function App() {

  const { user, login, logout, setUser } = useAuth();

  return (
    <AuthContext.Provider value={{user, setUser}}>
      <div className="App">
        <Router>
          <Navbar />
          <Routes>
            <Route path="/" />
            <Route path="/movies-shows" element={<Movies />} />
            <Route path="/movie/:id" element={<Movie />} />
            <Route path="/list" />
            <Route path="/login" element={<Login />} />
            <Route path="/logout" element={<Logout />}/>
            <Route path="/register"  element={<Register />}/>
            <Route path="/profile" element={<Profile />}/>
            <Route path="/admin" element={<CreateMovie />} />
            <Route path="/terms-and-conditions" element={<TermsAndConditions />} />
            <Route path="/privacy-policy" element={<PrivacyPolicy />} />
          </Routes>
        </Router>
      </div>
    </AuthContext.Provider>
  );
}

export default App;
