import axios, { InternalAxiosRequestConfig } from 'axios';

const clientApi = axios.create({
    baseURL: process.env.NEXT_PUBLIC_API_URL,
    withCredentials: true,
});

clientApi.interceptors.request.use(
    async (config: InternalAxiosRequestConfig): Promise<InternalAxiosRequestConfig> => {
        if (typeof window === 'undefined') return config;

        try {
            const res = await fetch('/auth/access-token');

            if (!res.ok) {
                console.error('Failed to fetch token');
                return config;
            }

            const data: { token?: string } = await res.json();

            if (data.token) {
                config.headers.set('Authorization', `Bearer ${data.token}`);
            } else {
                console.warn('No token received from /auth/access-token');
            }
        } catch (err) {
            console.error('Error fetching token:', err);
        }

        return config;
    },
    (error) => Promise.reject(error)
);

export default clientApi;
