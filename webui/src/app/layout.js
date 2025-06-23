import { Inter } from 'next/font/google';
import MuiThemeProvider from './components/theme/MuiThemeProvider';
import { ThemeContextProvider } from './components/theme/ThemeContext';
import { AppRouterCacheProvider } from "@mui/material-nextjs/v13-appRouter";

const inter = Inter({ subsets: ['latin'] });

export const metadata = {
    title: 'Imprink',
    description: 'Turn your dreams into colorful realities!',
};

export default function RootLayout({children}) {
    return (
        <html lang="en">
            <body className={inter.className}>
                <AppRouterCacheProvider>
                    <ThemeContextProvider>
                        <MuiThemeProvider>
                            {children}
                        </MuiThemeProvider>
                    </ThemeContextProvider>
                </AppRouterCacheProvider>
            </body>
        </html>
    );
}