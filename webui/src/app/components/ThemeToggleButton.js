'use client';

import { IconButton } from '@mui/material';
import { useTheme } from './theme/ThemeContext';

export default function ThemeToggleButton() {
    const { isDarkMode, toggleTheme } = useTheme();

    return (
        <IconButton
            onClick={toggleTheme}
            color="inherit"
            sx={{
                width: 40,
                height: 40,
                borderRadius: '8px',
                '&:hover': {
                    backgroundColor: 'rgba(99, 102, 241, 0.1)',
                },
            }}
        >
            {isDarkMode ? '🌙' : '☀️'}
        </IconButton>
    );
}