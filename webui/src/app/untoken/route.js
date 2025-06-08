import {cookies} from 'next/headers';
import {NextResponse} from 'next/server';

export async function GET() {
    try {
        (await cookies()).set({
            name: 'access_token',
            value: '',
            httpOnly: true,
            secure: true,
            sameSite: 'strict',
            path: '/',
            domain: process.env.COOKIE_DOMAIN,
            maxAge: -1,
        });

        return NextResponse.json({message: 'Deleted access token'});
    } catch (error) {
        console.error('Error in /api/untoken:', error);
        return NextResponse.json({ error: 'Internal Server Error' }, { status: 500 });
    }
}