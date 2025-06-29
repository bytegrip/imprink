'use client';

import {
    AppBar,
    Avatar,
    Box,
    Button,
    Divider,
    IconButton,
    Menu,
    MenuItem,
    Paper,
    Toolbar,
    Typography,
    useMediaQuery,
    useTheme as useMuiTheme,
} from '@mui/material';
import {
    Menu as MenuIcon,
    Home,
    PhotoLibrary,
    ShoppingBag,
    Store,
    Dashboard,
    AdminPanelSettings,
    Api,
    BugReport,
} from '@mui/icons-material';
import { useUser } from '@auth0/nextjs-auth0';
import {useState, MouseEvent, JSX} from 'react';
import ThemeToggleButton from '@/app/components/theme/ThemeToggleButton';
import useRoles from '@/app/components/hooks/useRoles';
import { useTheme } from '@/app/components/theme/ThemeContext';

interface NavLink {
    label: string;
    href: string;
    icon: JSX.Element;
    show: boolean;
}

export default function ImprinkAppBar() {
    const { user, isLoading } = useUser();
    const { isMerchant, isAdmin } = useRoles();
    const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);
    const theme = useMuiTheme();
    const { isDarkMode, toggleTheme } = useTheme();
    const isMobile = useMediaQuery(theme.breakpoints.down('md'));

    const handleMenuOpen = (event: MouseEvent<HTMLElement>) => {
        setAnchorEl(event.currentTarget);
    };

    const handleMenuClose = () => {
        setAnchorEl(null);
    };

    const navigationLinks: NavLink[] = [
        { label: 'Home', href: '/', icon: <Home />, show: true },
        { label: 'Gallery', href: '/gallery', icon: <PhotoLibrary />, show: true },
        { label: 'Orders', href: '/orders', icon: <ShoppingBag />, show: !!user },
        { label: 'Merchant', href: '/merchant', icon: <Store />, show: isMerchant },
    ];

    const adminLinks: NavLink[] = [
        { label: 'Dashboard', href: '/dashboard', icon: <Dashboard />, show: isMerchant },
        { label: 'Admin', href: '/admin', icon: <AdminPanelSettings />, show: isAdmin },
        { label: 'SEQ', href: '/seq', icon: <BugReport />, show: isAdmin },
    ];

    const visibleLinks = navigationLinks.filter(link => link.show);
    const visibleAdminLinks = adminLinks.filter(link => link.show);

    const renderDesktopNavigation = () => (
        <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
            {visibleLinks.map(link => (
                <Button
                    key={link.label}
                    color="inherit"
                    href={link.href}
                    startIcon={link.icon}
                    sx={{ minWidth: 'auto', px: 2, '&:hover': { backgroundColor: 'rgba(255, 255, 255, 0.1)' } }}
                >
                    {link.label}
                </Button>
            ))}
        </Box>
    );

    const renderAdminBar = () => {
        if (!visibleAdminLinks.length || isMobile) return null;

        return (
            <Paper
                elevation={1}
                sx={{
                    backgroundColor: theme.palette.mode === 'dark' ? 'rgba(255,255,255,0.05)' : 'rgba(0,0,0,0.02)',
                    borderTop: `1px solid ${theme.palette.divider}`,
                    borderRadius: 0,
                    py: 1.5,
                    px: 2,
                }}
            >
                <Box sx={{ display: 'flex', alignItems: 'center', gap: 3, justifyContent: 'center', maxWidth: '1200px', mx: 'auto' }}>
                    <Typography variant="caption" sx={{ mr: 2, color: 'text.secondary', fontWeight: 500, textTransform: 'uppercase', letterSpacing: 0.5 }}>
                        Admin Tools
                    </Typography>
                    {visibleAdminLinks.map(link => (
                        <Button
                            key={link.label}
                            href={link.href}
                            startIcon={link.icon}
                            variant="text"
                            size="small"
                            sx={{ minWidth: 'auto', px: 2, py: 0.5, color: 'text.primary', '&:hover': { backgroundColor: 'action.hover' } }}
                        >
                            {link.label}
                        </Button>
                    ))}
                </Box>
            </Paper>
        );
    };

    const renderMobileMenu = () => (
        <>
            <IconButton size="large" edge="start" color="inherit" aria-label="menu" onClick={handleMenuOpen} sx={{ mr: 2 }}>
                <MenuIcon />
            </IconButton>
            <Menu
                anchorEl={anchorEl}
                open={Boolean(anchorEl)}
                onClose={handleMenuClose}
                PaperProps={{
                    sx: {
                        mt: 1,
                        minWidth: 200,
                        '& .MuiMenuItem-root': { px: 2, py: 1.5, gap: 2 },
                    },
                }}
                transformOrigin={{ horizontal: 'left', vertical: 'top' }}
                anchorOrigin={{ horizontal: 'left', vertical: 'bottom' }}
            >
                <Divider />
                {[...visibleLinks, ...visibleAdminLinks].map(link => (
                    <MenuItem
                        key={link.label}
                        onClick={handleMenuClose}
                        component="a"
                        href={link.href}
                        sx={{ color: 'inherit', textDecoration: 'none', '&:hover': { backgroundColor: 'action.hover' } }}
                    >
                        {link.icon}
                        <Typography variant="body2">{link.label}</Typography>
                    </MenuItem>
                ))}
                {!isLoading && (
                    <>
                        <Divider />
                        {user ? (
                            <>
                                <MenuItem sx={{ opacity: 1, '&:hover': { backgroundColor: 'transparent' }, cursor: 'default' }} onClick={e => e.preventDefault()}>
                                    <Avatar src={user.picture ?? undefined} alt={user.name ?? 'User'} sx={{ width: 24, height: 24 }} />
                                    <Typography variant="body2" noWrap>{user.name}</Typography>
                                </MenuItem>
                                <MenuItem component="a" href="/auth/logout" onClick={handleMenuClose} sx={{ color: 'error.main' }}>
                                    <Typography variant="body2">Logout</Typography>
                                </MenuItem>
                            </>
                        ) : (
                            <>
                                <MenuItem component="a" href="/auth/login" onClick={handleMenuClose}>
                                    <Typography variant="body2">Login</Typography>
                                </MenuItem>
                                <MenuItem component="a" href="/auth/login" onClick={handleMenuClose} sx={{ color: 'primary.main', fontWeight: 'bold' }}>
                                    <Typography variant="body2">Sign Up</Typography>
                                </MenuItem>
                            </>
                        )}
                    </>
                )}
            </Menu>
        </>
    );

    return (
        <>
            <AppBar position="static">
                <Toolbar>
                    {isMobile && renderMobileMenu()}
                    <Typography variant="h6" component="div" sx={{ flexGrow: isMobile ? 1 : 0, mr: isMobile ? 0 : 4 }}>
                        Imprink
                    </Typography>
                    {!isMobile && (
                        <>
                            <Box sx={{ flexGrow: 1 }}>{renderDesktopNavigation()}</Box>
                            <ThemeToggleButton />
                            {isLoading ? (
                                <Box sx={{ ml: 2 }}>
                                    <Typography variant="body2">Loading...</Typography>
                                </Box>
                            ) : user ? (
                                <Box sx={{ display: 'flex', alignItems: 'center', ml: 2 }}>
                                    <Avatar src={user.picture ?? undefined} alt={user.name ?? 'User'} sx={{ width: 32, height: 32, mr: 1 }} />
                                    <Typography variant="body2" sx={{ mr: 2, color: 'inherit' }}>{user.name}</Typography>
                                    <Button color="inherit" href="/auth/logout">Logout</Button>
                                </Box>
                            ) : (
                                <Box sx={{ ml: 2 }}>
                                    <Button color="inherit" href="/auth/login" sx={{ mr: 1 }}>Login</Button>
                                    <Button variant="contained" href="/auth/login" sx={{ bgcolor: 'primary.main', color: 'primary.contrastText', '&:hover': { bgcolor: 'primary.dark' } }}>
                                        Sign Up
                                    </Button>
                                </Box>
                            )}
                        </>
                    )}
                    {isMobile && (
                        <Box sx={{ ml: 'auto' }}>
                            <ThemeToggleButton />
                        </Box>
                    )}
                </Toolbar>
            </AppBar>
            {renderAdminBar()}
        </>
    );
}