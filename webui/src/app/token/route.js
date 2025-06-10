import { NextResponse } from 'next/server';
import {auth0} from "@/lib/auth0";
import api from "@/lib/api";

export async function GET() {
    try {
        const token = (await auth0.getSession()).tokenSet.accessToken;

        if (!token) { return NextResponse.json({ error: 'No access token found' }, { status: 401 }); }

        await api.post('/users/sync', {}, {
            headers: { Cookie: `access_token=${token}` }
        });

        return NextResponse.json({ access_token: token });
    } catch (error) {
        console.error('Error in /api/token:', error);
        return NextResponse.json({ error: 'Internal Server Error' }, { status: 500 });
    }
}