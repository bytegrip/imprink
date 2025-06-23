'use client'

import {AppBar, Button, Toolbar, Typography, Avatar, Box} from "@mui/material";
import { useUser } from "@auth0/nextjs-auth0";
import ThemeToggleButton from "@/app/components/theme/ThemeToggleButton";

export default function ImprinkAppBar() {
    const { user, error, isLoading } = useUser();

    return (
        <AppBar position="static">
            <Toolbar>
                <Typography variant="h6" component="div" sx={{ flexGrow: 1 }}>
                    Modern App
                </Typography>
                <ThemeToggleButton />

                {isLoading ? (
                    <Box sx={{ ml: 2 }}>
                        <Typography variant="body2">Loading...</Typography>
                    </Box>
                ) : user ? (
                    <Box sx={{ display: 'flex', alignItems: 'center', ml: 2 }}>
                        <Avatar
                            src={user.picture}
                            alt={user.name}
                            sx={{ width: 32, height: 32, mr: 1 }}
                        />
                        <Typography variant="body2" sx={{ mr: 2, color: 'inherit' }}>
                            {user.name}
                        </Typography>
                        <Button
                            color="inherit"
                            href="/auth/logout"
                        >
                            Logout
                        </Button>
                    </Box>
                ) : (
                    <Box sx={{ ml: 2 }}>
                        <Button
                            color="inherit"
                            href="/auth/login"
                            sx={{ mr: 1 }}
                        >
                            Login
                        </Button>
                        <Button
                            variant="contained"
                            href="/auth/login"
                            sx={{
                                bgcolor: 'primary.main',
                                color: 'primary.contrastText',
                                '&:hover': {
                                    bgcolor: 'primary.dark'
                                }
                            }}
                        >
                            Sign Up
                        </Button>
                    </Box>
                )}
            </Toolbar>
        </AppBar>
    )
}