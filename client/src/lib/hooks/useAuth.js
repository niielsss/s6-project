import { useUser } from "./useUser";

export const useAuth = () => {
    
    const { user, addUser, removeUser, setUser } = useUser();
  
    const login = (user) => {
      addUser(user);
    };
  
    const logout = () => {
      removeUser();
    };
  
    return { user, login, logout, setUser };
  };