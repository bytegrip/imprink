export const metadata = {
    title: 'Stripe Payment Demo',
    description: 'Stripe payment integration demo with Next.js App Router',
}

export default function RootLayout({ children }) {
    return (
        <html lang="en">
        <body>
        {children}
        </body>
        </html>
    )
}