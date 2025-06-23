import axios from "axios";

const api = axios.create({
    baseURL: process.env.NEXT_PUBLIC_API_URL,
    withCredentials: true,
});

api.interceptors.request.use((config) => {
    if (typeof window !== 'undefined') {
        const token = localStorage.getItem('access_token');
        if (token) {
            config.headers.Authorization = `Bearer ${token}`;
        } else {
            console.log("Token not found in localStorage, please auth!");
        }
    }
    return config;
}, (error) => {
    return Promise.reject(error);
});

export default api;