import { cookies } from 'next/headers';
import { NextResponse } from 'next/server';
import {auth0} from "@/lib/auth0";

export async function GET() {
    try {
        const session = await auth0.getSession();
        const accessToken = session.tokenSet.accessToken;
        if (!accessToken) {
            return NextResponse.json({ error: 'No access token found' }, { status: 401 });
        }

        const response = NextResponse.json({ message: 'Access token set in cookie' });

        const cookieDomain = process.env.COOKIE_DOMAIN || undefined;

        const cookieStore = await cookies();
        cookieStore.set({
            name: 'access_token',
            value: accessToken,
            httpOnly: true,
            secure: true,
            sameSite: 'strict',
            path: '/',
            domain: cookieDomain,
            maxAge: 3600,
        });

        return response;
    } catch (error) {
        console.error('Error in /api/set-token:', error);
        return NextResponse.json({ error: 'Internal Server Error' }, { status: 500 });
    }
}