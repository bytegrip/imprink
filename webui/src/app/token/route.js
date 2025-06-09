import {cookies, headers} from 'next/headers';
import { NextResponse } from 'next/server';
import {auth0} from "@/lib/auth0";
import api from "@/lib/api";

export async function GET() {
    try {
        const token = (await auth0.getSession()).tokenSet.accessToken;

        if (!token) { return NextResponse.json({ error: 'No access token found' }, { status: 401 }); }

        (await cookies()).set('access_token', token, {
            httpOnly: true,
            secure: true,
            sameSite: 'strict',
            path: '/',
            domain: process.env.COOKIE_DOMAIN,
            maxAge: 3600,
        });

        await api.post('/users/sync', {}, {
            headers: { Cookie: `access_token=${token}` }
        });

        return NextResponse.json({ message: 'Access token set in cookie' });
    } catch (error) {
        console.error('Error in /api/token:', error);
        return NextResponse.json({ error: 'Internal Server Error' }, { status: 500 });
    }
}