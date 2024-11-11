import axios from "axios";
import authHeader from "./authHeader";

export default axios.create({
    //baseURL: "http://4.182.168.170:8080/",
    baseURL: "http://localhost:8080/",
    headers: {
        "Content-type": "application/json",
        ...authHeader()
    }
});
