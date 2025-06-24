import axios from "axios";

const clientApi = axios.create({
    baseURL: process.env.NEXT_PUBLIC_API_URL,
    withCredentials: true,
});

clientApi.interceptors.request.use(async (config) => {
    if (typeof window === 'undefined') return config;

    try {
        const res = await fetch('/auth/access-token');
        if (!res.ok)
            throw new Error('Failed to fetch token');
        const data = await res.json();

        if (data.token) {
            config.headers.Authorization = `Bearer ${data.token}`;
        } else {
            console.warn('No token received from /auth/access-token');
        }
    } catch (err) {
        console.error('Error fetching token:', err);
    }

    return config;
}, error => {
    return Promise.reject(error);
});

export default clientApi;