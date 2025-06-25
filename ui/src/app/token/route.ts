import { NextResponse } from 'next/server';
import { auth0 } from '@/lib/auth0';
import serverApi from '@/lib/serverApi';

export async function GET(): Promise<Response> {
    try {
        const session = await auth0.getSession();
        const token = session?.tokenSet?.accessToken;

        if (!token) {
            return NextResponse.json({ error: 'No access token found' }, { status: 401 });
        }

        await serverApi.post('/users/me/sync', null, {
            headers: { Authorization: `Bearer ${token}` }
        });

        return NextResponse.json("Ok");
    } catch (error) {
        console.error('Error in /serverApi/token:', error);
        return NextResponse.json({ error: 'Internal Server Error' }, { status: 500 });
    }
}
