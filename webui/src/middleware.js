import { auth0 } from "./lib/auth0";

export async function middleware(request) {
    return await auth0.middleware(request);
}

export const config = {
    matcher: [
        "/((?!_next/static|_next/image|favicon.ico|sitemap.xml|robots.txt).*)",
    ],
};