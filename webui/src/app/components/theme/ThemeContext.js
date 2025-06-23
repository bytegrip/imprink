'use client';

import { createContext, useContext, useState } from 'react';

const ThemeContext = createContext({ isDarkMode: true });

export function ThemeContextProvider({ children }) {
    const [isDarkMode, setIsDarkMode] = useState(true);

    const toggleTheme = () => {
        setIsDarkMode(!isDarkMode);
    };

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