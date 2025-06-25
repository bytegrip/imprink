'use client'

import { createTheme, ThemeOptions } from '@mui/material/styles';

export const lightTheme : ThemeOptions = createTheme({
    palette: {
        mode: 'light',
        primary: {
            main: '#6366f1',
            light: '#818cf8',
            dark: '#4f46e5',
            contrastText: '#ffffff',
        },
        secondary: {
            main: '#f59e0b',
            light: '#fbbf24',
            dark: '#d97706',
            contrastText: '#000000',
        },
        background: {
            default: '#f8fafc',
            paper: '#ffffff',
        },
        text: {
            primary: '#0f172a',
            secondary: '#475569',
        },
        error: {
            main: '#ef4444',
            light: '#f87171',
            dark: '#dc2626',
        },
        warning: {
            main: '#f59e0b',
            light: '#fbbf24',
            dark: '#d97706',
        },
        info: {
            main: '#06b6d4',
            light: '#22d3ee',
            dark: '#0891b2',
        },
        success: {
            main: '#10b981',
            light: '#34d399',
            dark: '#059669',
        },
        divider: '#e2e8f0',
    },
    typography: {
        fontFamily: '"Inter", -apple-system, BlinkMacSystemFont, "Segoe UI", Roboto, sans-serif',
        h1: {
            fontSize: '2.5rem',
            fontWeight: 700,
            lineHeight: 1.2,
            letterSpacing: '-0.025em',
        },
        h2: {
            fontSize: '2rem',
            fontWeight: 600,
            lineHeight: 1.3,
            letterSpacing: '-0.025em',
        },
        h3: {
            fontSize: '1.5rem',
            fontWeight: 600,
            lineHeight: 1.4,
            letterSpacing: '-0.015em',
        },
        h4: {
            fontSize: '1.25rem',
            fontWeight: 600,
            lineHeight: 1.4,
        },
        h5: {
            fontSize: '1.125rem',
            fontWeight: 600,
            lineHeight: 1.5,
        },
        h6: {
            fontSize: '1rem',
            fontWeight: 600,
            lineHeight: 1.5,
        },
        body1: {
            fontSize: '1rem',
            lineHeight: 1.6,
            fontWeight: 400,
        },
        body2: {
            fontSize: '0.875rem',
            lineHeight: 1.6,
            fontWeight: 400,
        },
        button: {
            fontSize: '0.875rem',
            fontWeight: 500,
            textTransform: 'none',
            letterSpacing: '0.025em',
        },
    },
    shape: {
        borderRadius: 12,
    },
    shadows: [
        'none',
        '0 1px 3px 0 rgba(0, 0, 0, 0.1), 0 1px 2px 0 rgba(0, 0, 0, 0.06)',
        '0 4px 6px -1px rgba(0, 0, 0, 0.1), 0 2px 4px -1px rgba(0, 0, 0, 0.06)',
        '0 10px 15px -3px rgba(0, 0, 0, 0.1), 0 4px 6px -2px rgba(0, 0, 0, 0.05)',
        '0 20px 25px -5px rgba(0, 0, 0, 0.1), 0 10px 10px -5px rgba(0, 0, 0, 0.04)',
        '0 25px 50px -12px rgba(0, 0, 0, 0.15)',
        '0 25px 50px -12px rgba(0, 0, 0, 0.15)',
        '0 25px 50px -12px rgba(0, 0, 0, 0.15)',
        '0 25px 50px -12px rgba(0, 0, 0, 0.15)',
        '0 25px 50px -12px rgba(0, 0, 0, 0.15)',
        '0 25px 50px -12px rgba(0, 0, 0, 0.15)',
        '0 25px 50px -12px rgba(0, 0, 0, 0.15)',
        '0 25px 50px -12px rgba(0, 0, 0, 0.15)',
        '0 25px 50px -12px rgba(0, 0, 0, 0.15)',
        '0 25px 50px -12px rgba(0, 0, 0, 0.15)',
        '0 25px 50px -12px rgba(0, 0, 0, 0.15)',
        '0 25px 50px -12px rgba(0, 0, 0, 0.15)',
        '0 25px 50px -12px rgba(0, 0, 0, 0.15)',
        '0 25px 50px -12px rgba(0, 0, 0, 0.15)',
        '0 25px 50px -12px rgba(0, 0, 0, 0.15)',
        '0 25px 50px -12px rgba(0, 0, 0, 0.15)',
        '0 25px 50px -12px rgba(0, 0, 0, 0.15)',
        '0 25px 50px -12px rgba(0, 0, 0, 0.15)',
        '0 25px 50px -12px rgba(0, 0, 0, 0.15)',
        '0 25px 50px -12px rgba(0, 0, 0, 0.15)',
    ],
    components: {
        MuiCssBaseline: {
            styleOverrides: {
                body: {
                    scrollbarWidth: 'thin',
                    scrollbarColor: '#6366f1 #f1f5f9',
                    '&::-webkit-scrollbar': {
                        width: '8px',
                    },
                    '&::-webkit-scrollbar-track': {
                        background: '#f1f5f9',
                    },
                    '&::-webkit-scrollbar-thumb': {
                        background: '#6366f1',
                        borderRadius: '4px',
                    },
                },
            },
        },
        MuiButton: {
            styleOverrides: {
                root: {
                    borderRadius: '8px',
                    padding: '10px 24px',
                    fontSize: '0.875rem',
                    fontWeight: 500,
                    boxShadow: 'none',
                    '&:hover': {
                        boxShadow: '0 4px 12px rgba(99, 102, 241, 0.3)',
                        transform: 'translateY(-1px)',
                    },
                    '&:active': {
                        transform: 'translateY(0)',
                    },
                },
                contained: {
                    background: 'linear-gradient(135deg, #6366f1 0%, #8b5cf6 100%)',
                    '&:hover': {
                        background: 'linear-gradient(135deg, #5b21b6 0%, #7c3aed 100%)',
                    },
                },
                outlined: {
                    borderWidth: '1.5px',
                    '&:hover': {
                        borderWidth: '1.5px',
                        backgroundColor: 'rgba(99, 102, 241, 0.08)',
                    },
                },
            },
        },
        MuiTextField: {
            styleOverrides: {
                root: {
                    '& .MuiOutlinedInput-root': {
                        borderRadius: '8px',
                        backgroundColor: 'rgba(241, 245, 249, 0.5)',
                        '&:hover .MuiOutlinedInput-notchedOutline': {
                            borderColor: '#6366f1',
                        },
                        '&.Mui-focused .MuiOutlinedInput-notchedOutline': {
                            borderColor: '#6366f1',
                            borderWidth: '2px',
                        },
                    },
                },
            },
        },
        MuiCard: {
            styleOverrides: {
                root: {
                    background: 'linear-gradient(145deg, #ffffff 0%, #f8fafc 100%)',
                    border: '1px solid rgba(99, 102, 241, 0.1)',
                    backdropFilter: 'blur(20px)',
                    '&:hover': {
                        border: '1px solid rgba(99, 102, 241, 0.2)',
                        transform: 'translateY(-2px)',
                        boxShadow: '0 20px 40px rgba(99, 102, 241, 0.15)',
                    },
                    transition: 'all 0.3s cubic-bezier(0.4, 0, 0.2, 1)',
                },
            },
        },
        MuiPaper: {
            styleOverrides: {
                root: {
                    backgroundImage: 'none',
                    backgroundColor: '#ffffff',
                    border: '1px solid rgba(99, 102, 241, 0.1)',
                },
            },
        },
        MuiAppBar: {
            styleOverrides: {
                root: {
                    background: 'rgba(255, 255, 255, 0.8)',
                    backdropFilter: 'blur(20px)',
                    borderBottom: '1px solid rgba(99, 102, 241, 0.1)',
                    boxShadow: '0 8px 32px rgba(99, 102, 241, 0.1)',
                    color: '#0f172a',
                },
            },
        },
        MuiDrawer: {
            styleOverrides: {
                paper: {
                    background: 'linear-gradient(180deg, #ffffff 0%, #f8fafc 100%)',
                    border: 'none',
                    borderRight: '1px solid rgba(99, 102, 241, 0.1)',
                },
            },
        },
        MuiChip: {
            styleOverrides: {
                root: {
                    background: 'rgba(99, 102, 241, 0.7)',
                    border: '1px solid rgba(99, 102, 241, 0.2)',
                    '&:hover': {
                        background: 'rgba(99, 102, 241, 0.9)',
                    },
                },
            },
        },
        MuiTab: {
            styleOverrides: {
                root: {
                    textTransform: 'none',
                    fontWeight: 500,
                    fontSize: '0.875rem',
                    '&.Mui-selected': {
                        color: '#6366f1',
                    },
                },
            },
        },
        MuiTabs: {
            styleOverrides: {
                indicator: {
                    background: 'linear-gradient(90deg, #6366f1, #8b5cf6)',
                    height: '3px',
                    borderRadius: '3px',
                },
            },
        },
    },
});