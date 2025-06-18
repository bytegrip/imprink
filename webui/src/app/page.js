'use client'

import {useEffect, useState} from 'react';
import axios from 'axios';
import {
    Alert,
    AppBar,
    Badge,
    Box,
    Button,
    Card,
    CardActions,
    CardContent,
    CardMedia,
    Chip,
    Container,
    Fab,
    Grid,
    IconButton,
    Skeleton,
    Toolbar,
    Typography
} from '@mui/material';
import {createTheme, ThemeProvider} from '@mui/material/styles';
import CssBaseline from '@mui/material/CssBaseline';
import {Menu, Palette, Search, ShoppingCart} from 'lucide-react';

const theme = createTheme({
    palette: {
        mode: 'dark',
        primary: {
            main: '#D0BCFF',
            light: '#EADDFF',
            dark: '#9A82DB',
            contrastText: '#21005D'
        },
        secondary: {
            main: '#CCC2DC',
            light: '#E8DEF8',
            dark: '#A8A2BA',
            contrastText: '#332D41'
        },
        background: {
            default: '#101418',
            paper: '#1D1B20'
        },
        surface: {
            main: '#1D1B20',
            variant: '#49454F'
        },
        error: {
            main: '#F2B8B5',
            light: '#FFDAD6',
            dark: '#BA1A1A',
            contrastText: '#410002'
        },
        success: {
            main: '#A6D4A3',
            light: '#C4F0B8',
            dark: '#52B788'
        },
        text: {
            primary: '#E6E0E9',
            secondary: '#CAC4D0'
        }
    },
    shape: {
        borderRadius: 16
    },
    typography: {
        fontFamily: '"Google Sans", "Roboto", "Helvetica", "Arial", sans-serif',
        h4: {
            fontWeight: 400,
            fontSize: '2rem'
        },
        h6: {
            fontWeight: 500,
            fontSize: '1.25rem'
        },
        body1: {
            fontSize: '1rem',
            lineHeight: 1.5
        },
        body2: {
            fontSize: '0.875rem',
            lineHeight: 1.43
        }
    },
    components: {
        MuiCard: {
            styleOverrides: {
                root: {
                    backgroundImage: 'none',
                    backgroundColor: '#1D1B20',
                    border: '1px solid #49454F',
                    transition: 'all 0.3s ease',
                    '&:hover': {
                        transform: 'translateY(-4px)',
                        borderColor: '#D0BCFF',
                        boxShadow: '0 8px 32px rgba(208, 188, 255, 0.1)'
                    }
                }
            }
        },
        MuiButton: {
            styleOverrides: {
                root: {
                    textTransform: 'none',
                    fontWeight: 500,
                    borderRadius: 20,
                    paddingLeft: 24,
                    paddingRight: 24,
                    height: 40
                }
            }
        },
        MuiTextField: {
            styleOverrides: {
                root: {
                    '& .MuiOutlinedInput-root': {
                        borderRadius: 12,
                        backgroundColor: '#1D1B20',
                        '& fieldset': {
                            borderColor: '#49454F'
                        },
                        '&:hover fieldset': {
                            borderColor: '#CAC4D0'
                        }
                    }
                }
            }
        },
        MuiChip: {
            styleOverrides: {
                root: {
                    borderRadius: 8
                }
            }
        }
    }
});

const ProductCard = ({ product }) => {
    const [imageLoaded, setImageLoaded] = useState(false);

    return (
        <Card sx={{ height: '100%', display: 'flex', flexDirection: 'column' }}>
            <Box sx={{ position: 'relative', overflow: 'hidden' }}>
                {!imageLoaded && (
                    <Skeleton variant="rectangular" height={200} />
                )}
                <CardMedia
                    component="img"
                    height="200"
                    image={product.imageUrl}
                    alt={product.name}
                    onLoad={() => setImageLoaded(true)}
                    sx={{
                        display: imageLoaded ? 'block' : 'none',
                        objectFit: 'cover',
                        transition: 'transform 0.3s ease',
                        '&:hover': {
                            transform: 'scale(1.05)'
                        }
                    }}
                />
                <Box sx={{
                    position: 'absolute',
                    top: 12,
                    right: 12,
                    display: 'flex',
                    flexDirection: 'column',
                    gap: 1
                }}>
                    {product.isCustomizable && (
                        <Chip
                            icon={<Palette size={16} />}
                            label="Customizable"
                            size="small"
                            color="primary"
                            sx={{ fontSize: '0.75rem' }}
                        />
                    )}
                    <Chip
                        label={product.category.name}
                        size="small"
                        variant="outlined"
                        sx={{ fontSize: '0.75rem' }}
                    />
                </Box>
            </Box>

            <CardContent sx={{ flexGrow: 1, pb: 1 }}>
                <Typography variant="h6" component="h3" gutterBottom>
                    {product.name}
                </Typography>
                <Typography variant="body2" color="text.secondary" sx={{ mb: 2 }}>
                    {product.description}
                </Typography>
                <Typography variant="h5" color="primary.main" fontWeight="bold">
                    ${product.basePrice.toFixed(2)}
                </Typography>
            </CardContent>

            <CardActions sx={{ p: 2, pt: 0 }}>
                <Button
                    variant="contained"
                    fullWidth
                    startIcon={<ShoppingCart size={18} />}
                    sx={{
                        background: 'linear-gradient(45deg, #D0BCFF 30%, #EADDFF 90%)',
                        color: '#21005D'
                    }}
                >
                    Add to Cart
                </Button>
            </CardActions>
        </Card>
    );
};

const LoadingSkeleton = () => (
    <Grid container spacing={3}>
        {[...Array(8)].map((_, index) => (
            <Grid item xs={12} sm={6} md={4} lg={3} key={index}>
                <Card>
                    <Skeleton variant="rectangular" height={200} />
                    <CardContent>
                        <Skeleton variant="text" height={32} />
                        <Skeleton variant="text" height={20} />
                        <Skeleton variant="text" height={20} width="60%" />
                        <Skeleton variant="text" height={28} width="40%" />
                    </CardContent>
                </Card>
            </Grid>
        ))}
    </Grid>
);

const ImprintLanding = () => {
    const [products, setProducts] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const [cartCount, setCartCount] = useState(0);

    useEffect(() => {
        const fetchProducts = async () => {
            try {
                setLoading(true);
                const response = await axios.get('https://impr.ink/api/products', {
                    params: {
                        PageNumber: 1,
                        PageSize: 20
                    }
                });
                setProducts(response.data.items);
            } catch (err) {
                setError('Failed to load products. Please try again later.');
                console.error('Error fetching products:', err);
            } finally {
                setLoading(false);
            }
        };

        fetchProducts().then(r => console.log(r));
    }, []);

    const featuredProducts = products.slice(0, 4);
    return (
        <ThemeProvider theme={theme}>
            <CssBaseline />
            <Box sx={{ flexGrow: 1 }}>
                {/* App Bar */}
                <AppBar position="sticky" sx={{ backgroundColor: 'background.paper', borderBottom: '1px solid #49454F' }}>
                    <Toolbar>
                        <IconButton edge="start" color="inherit" sx={{ mr: 2 }}>
                            <Menu />
                        </IconButton>
                        <Typography variant="h6" component="div" sx={{ flexGrow: 1, color: 'primary.main', fontWeight: 'bold' }}>
                            impr.ink
                        </Typography>
                        <IconButton color="inherit" sx={{ mr: 1 }}>
                            <Search />
                        </IconButton>
                        <IconButton color="inherit">
                            <Badge badgeContent={cartCount} color="primary">
                                <ShoppingCart />
                            </Badge>
                        </IconButton>
                    </Toolbar>
                </AppBar>

                {/* Hero Section */}
                <Box sx={{
                    background: 'linear-gradient(135deg, #1D1B20 0%, #2D1B69 50%, #1D1B20 100%)',
                    py: 8,
                    position: 'relative',
                    overflow: 'hidden'
                }}>
                    <Container maxWidth="lg">
                        <Box sx={{ textAlign: 'center', position: 'relative', zIndex: 1 }}>
                            <Typography variant="h2" component="h1" gutterBottom sx={{
                                fontWeight: 300,
                                background: 'linear-gradient(45deg, #D0BCFF, #EADDFF)',
                                backgroundClip: 'text',
                                WebkitBackgroundClip: 'text',
                                WebkitTextFillColor: 'transparent',
                                mb: 2
                            }}>
                                Custom Printing Made Beautiful
                            </Typography>
                            <Typography variant="h5" color="text.secondary" sx={{ mb: 4, maxWidth: 600, mx: 'auto' }}>
                                High-quality custom printing solutions for caps, flyers, coasters, and more.
                                Premium materials with excellent print adhesion and longevity.
                            </Typography>
                            <Button
                                variant="contained"
                                size="large"
                                sx={{
                                    background: 'linear-gradient(45deg, #D0BCFF 30%, #EADDFF 90%)',
                                    color: '#21005D',
                                    px: 4,
                                    py: 1.5,
                                    fontSize: '1.1rem'
                                }}
                            >
                                Explore Products
                            </Button>
                        </Box>
                    </Container>
                </Box>

                <Container maxWidth="lg" sx={{ py: 6 }}>
                    {error && (
                        <Alert severity="error" sx={{ mb: 4 }}>
                            {error}
                        </Alert>
                    )}

                    {/* Featured Products */}
                    <Box sx={{ mb: 6 }}>
                        <Typography variant="h4" component="h2" gutterBottom sx={{ mb: 4, textAlign: 'center' }}>
                            Featured Products
                        </Typography>
                        {loading ? (
                            <Grid container spacing={3}>
                                {[...Array(4)].map((_, index) => (
                                    <Grid item xs={12} sm={6} md={3} key={index}>
                                        <Card>
                                            <Skeleton variant="rectangular" height={200} />
                                            <CardContent>
                                                <Skeleton variant="text" height={32} />
                                                <Skeleton variant="text" height={20} />
                                                <Skeleton variant="text" height={28} width="40%" />
                                            </CardContent>
                                        </Card>
                                    </Grid>
                                ))}
                            </Grid>
                        ) : (
                            <Grid container spacing={3}>
                                {featuredProducts.map((product) => (
                                    <Grid item xs={12} sm={6} md={3} key={product.id}>
                                        <ProductCard product={product} />
                                    </Grid>
                                ))}
                            </Grid>
                        )}
                    </Box>

                    {/* All Products */}
                    <Box>
                        <Typography variant="h4" component="h2" gutterBottom sx={{ mb: 4, textAlign: 'center' }}>
                            All Products
                        </Typography>
                        {loading ? (
                            <LoadingSkeleton />
                        ) : (
                            <Grid container spacing={3}>
                                {products.map((product) => (
                                    <Grid item xs={12} sm={6} md={4} lg={3} key={product.id}>
                                        <ProductCard product={product} />
                                    </Grid>
                                ))}
                            </Grid>
                        )}
                    </Box>

                    {/* Load More Button */}
                    {!loading && products.length > 0 && (
                        <Box sx={{ textAlign: 'center', mt: 6 }}>
                            <Button
                                variant="outlined"
                                size="large"
                                sx={{ px: 4 }}
                            >
                                Load More Products
                            </Button>
                        </Box>
                    )}
                </Container>

                {/* Floating Action Button */}
                <Fab
                    color="primary"
                    sx={{
                        position: 'fixed',
                        bottom: 16,
                        right: 16,
                        background: 'linear-gradient(45deg, #D0BCFF 30%, #EADDFF 90%)',
                        color: '#21005D'
                    }}
                >
                    <Badge badgeContent={cartCount} color="error">
                        <ShoppingCart />
                    </Badge>
                </Fab>
            </Box>
        </ThemeProvider>
    );
};

export default ImprintLanding;