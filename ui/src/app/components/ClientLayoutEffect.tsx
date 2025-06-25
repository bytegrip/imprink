'use client';

import { useEffect } from 'react';
import axios, { AxiosResponse } from 'axios';

export default function ClientLayoutEffect(): null {
    useEffect(() => {
        async function fetchData(): Promise<void> {
            try {
                const res: AxiosResponse = await axios.get('/token');
                console.log('Token response:', res.data);
            } catch (error) {
                console.error('Token fetch error:', error);
            }
        }

        fetchData().then(() => console.log('Ok'));
    }, []);

    return null;
}
