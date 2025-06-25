'use client';

import { ThemeProvider } from '@mui/material/styles';
import CssBaseline from '@mui/material/CssBaseline';
import { darkTheme } from './darkTheme';
import { lightTheme } from './lightTheme';
import { useTheme } from './ThemeContext';
import { ReactNode } from 'react';

interface MuiThemeProviderProps {
    children: ReactNode;
}

export default function MuiThemeProvider({ children }: MuiThemeProviderProps) {
    const { isDarkMode } = useTheme();

    return (
        <ThemeProvider theme={isDarkMode ? darkTheme : lightTheme}>
            <CssBaseline />
            {children}
        </ThemeProvider>
    );
}
