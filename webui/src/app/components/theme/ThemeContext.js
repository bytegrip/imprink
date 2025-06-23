'use client';

import { createContext, useContext, useState, useEffect } from 'react';

const ThemeContext = createContext({ isDarkMode: true });

function getInitialTheme() {
    if (typeof window === 'undefined') {
        return null;
    }

    const savedTheme = localStorage.getItem('theme-preference');
    if (savedTheme) {
        return savedTheme === 'dark';
    }

    return window.matchMedia('(prefers-color-scheme: dark)').matches;
}

export function ThemeContextProvider({ children }) {
    const [isDarkMode, setIsDarkMode] = useState(true);
    const [isInitialized, setIsInitialized] = useState(false);

    useEffect(() => {
        const initialTheme = getInitialTheme();
        if (initialTheme !== null) {
            setIsDarkMode(initialTheme);
        }
        setIsInitialized(true);
    }, []);

    const toggleTheme = () => {
        const newTheme = !isDarkMode;
        setIsDarkMode(newTheme);

        if (typeof window !== 'undefined') {
            localStorage.setItem('theme-preference', newTheme ? 'dark' : 'light');
        }
    };

    if (!isInitialized) {
        return (
            <div style={{
                visibility: 'hidden',
                position: 'absolute',
                top: 0,
                left: 0,
                width: '100%',
                height: '100%'
            }}>
                {children}
            </div>
        );
    }

    return (
        <ThemeContext.Provider value={{ isDarkMode, toggleTheme }}>
            {children}
        </ThemeContext.Provider>
    );
}

export function useTheme() {
    const context = useContext(ThemeContext);
    if (!context) {
        throw new Error('useTheme must be used within a ThemeContextProvider');
    }
    return context;
}