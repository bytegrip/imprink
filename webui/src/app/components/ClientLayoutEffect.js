'use client';

import { useEffect } from 'react';
import axios from 'axios';

export default function ClientLayoutEffect() {
    useEffect(() => {
        async function fetchData() {
            try {
                const res = await axios.get('/token');
                console.log('Token response:', res.data);
            } catch (error) {
                console.error('Token fetch error:', error);
            }
        }

        fetchData().then(r => console.log("Ok"));
    }, []);

    return null;
}