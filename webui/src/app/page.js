'use client';

import {
    Box,
    Container,
    Typography,
    Button,
    Card,
    CardContent,
    CardMedia,
    Grid,
    Chip,
    CircularProgress,
    Alert
} from '@mui/material';
import { useState, useEffect } from 'react';
import { ShoppingCart, Palette, CreditCard, LocalShipping, CheckCircle } from '@mui/icons-material';
import clientApi from "@/lib/clientApi";

export default function HomePage() {
    const [products, setProducts] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    useEffect(() => {
        const fetchProducts = async () => {
            try {
                const response = await clientApi.get('/products/', {
                    params: {
                        PageSize: 6,
                        PageNumber: 1,
                        IsActive: true,
                        IsCustomizable: true,
                        SortBy: 'Price',
                        SortDirection: 'DESC'
                    }
                });
                setProducts(response.data.items);
            } catch (err) {
                setError('Failed to load products');
                console.error('Error fetching products:', err);
            } finally {
                setLoading(false);
            }
        };

        fetchProducts();
    }, []);

    const steps = [
        {
            label: 'Pick an Item',
            description: 'Browse our collection of customizable products and select the perfect base for your design.',
            icon: <ShoppingCart sx={{ fontSize: 40 }} />,
            color: '#1976d2'
        },
        {
            label: 'Choose Variant',
            description: 'Select size, color, and material options that match your preferences and needs.',
            icon: <Palette sx={{ fontSize: 40 }} />,
            color: '#9c27b0'
        },
        {
            label: 'Customize with Images',
            description: 'Upload your designs, add text, or use our design tools to create something unique.',
            icon: <Palette sx={{ fontSize: 40 }} />,
            color: '#f57c00'
        },
        {
            label: 'Pay',
            description: 'Secure checkout with multiple payment options. Review your order before finalizing.',
            icon: <CreditCard sx={{ fontSize: 40 }} />,
            color: '#388e3c'
        },
        {
            label: 'Wait for Order',
            description: 'We\'ll print and ship your custom item. Track your order status in real-time.',
            icon: <LocalShipping sx={{ fontSize: 40 }} />,
            color: '#d32f2f'
        }
    ];

    return (
        <Container maxWidth="lg" sx={{ py: 4 }}>
            <Box sx={{ mb: 8, textAlign: 'center' }}>
                <Typography variant="h1" gutterBottom sx={{ fontWeight: 'bold', mb: 3 }}>
                    Custom Printing Made Simple
                </Typography>
                <Typography variant="h5" color="text.secondary" sx={{ mb: 4, maxWidth: 800, mx: 'auto' }}>
                    Transform your ideas into reality with our premium custom printing services.
                    From t-shirts to mugs, we bring your designs to life with professional quality.
                </Typography>
                <Box sx={{ display: 'flex', gap: 2, justifyContent: 'center', flexWrap: 'wrap' }}>
                    <Button variant="contained" size="large" sx={{ px: 4, py: 1.5 }}>
                        Start Customizing
                    </Button>
                    <Button variant="outlined" size="large" sx={{ px: 4, py: 1.5 }}>
                        View Gallery
                    </Button>
                </Box>
            </Box>

            <Box sx={{ mb: 8 }}>
                <Typography variant="h2" gutterBottom sx={{ textAlign: 'center', mb: 6 }}>
                    Featured Products
                </Typography>

                {loading && (
                    <Box sx={{ display: 'flex', justifyContent: 'center', py: 8 }}>
                        <CircularProgress size={60} />
                    </Box>
                )}

                {error && (
                    <Alert severity="error" sx={{ mb: 4 }}>
                        {error}
                    </Alert>
                )}

                {!loading && !error && (
                    <Grid container spacing={3} sx={{ justifyContent: 'center' }}>
                        {products.map((product) => (
                            <Grid item key={product.id}>
                                <Card
                                    sx={{
                                        height: 380,
                                        width: 300,
                                        display: 'flex',
                                        flexDirection: 'column',
                                        transition: 'transform 0.3s ease-in-out, box-shadow 0.3s ease-in-out',
                                        '&:hover': {
                                            transform: 'translateY(-8px)',
                                            boxShadow: 6
                                        }
                                    }}
                                >
                                    <CardMedia
                                        component="img"
                                        height="180"
                                        image={product.imageUrl || '/placeholder-product.jpg'}
                                        alt={product.name}
                                        sx={{ objectFit: 'cover' }}
                                    />
                                    <CardContent sx={{
                                        flexGrow: 1,
                                        display: 'flex',
                                        flexDirection: 'column',
                                        p: 2,
                                        '&:last-child': { pb: 2 }
                                    }}>
                                        <Typography variant="h6" gutterBottom sx={{
                                            fontWeight: 'bold',
                                            overflow: 'hidden',
                                            textOverflow: 'ellipsis',
                                            whiteSpace: 'nowrap'
                                        }}>
                                            {product.name}
                                        </Typography>
                                        <Typography variant="body2" color="text.secondary" sx={{
                                            flexGrow: 1,
                                            mb: 2,
                                            overflow: 'hidden',
                                            display: '-webkit-box',
                                            WebkitLineClamp: 2,
                                            WebkitBoxOrient: 'vertical',
                                            minHeight: 40
                                        }}>
                                            {product.description}
                                        </Typography>
                                        <Box sx={{ display: 'flex', alignItems: 'center', justifyContent: 'space-between', mb: 2 }}>
                                            <Typography variant="subtitle1" color="primary" sx={{ fontWeight: 'bold' }}>
                                                From ${product.basePrice?.toFixed(2)}
                                            </Typography>
                                            {product.isCustomizable && (
                                                <Chip
                                                    label="Custom"
                                                    color="primary"
                                                    size="small"
                                                />
                                            )}
                                        </Box>
                                        <Button
                                            variant="contained"
                                            fullWidth
                                            size="small"
                                        >
                                            Customize
                                        </Button>
                                    </CardContent>
                                </Card>
                            </Grid>
                        ))}
                    </Grid>
                )}
            </Box>

            <Box sx={{ mb: 8 }}>
                <Typography variant="h2" gutterBottom sx={{ textAlign: 'center', mb: 6 }}>
                    How It Works
                </Typography>

                <Box sx={{ maxWidth: 800, mx: 'auto' }}>
                    {steps.map((step, index) => (
                        <Box key={index} sx={{ display: 'flex', mb: 4, position: 'relative' }}>
                            {index < steps.length - 1 && (
                                <Box
                                    sx={{
                                        position: 'absolute',
                                        left: 30,
                                        top: 80,
                                        width: 2,
                                        height: 60,
                                        bgcolor: 'divider',
                                        zIndex: 0
                                    }}
                                />
                            )}

                            <Box
                                sx={{
                                    width: 60,
                                    height: 60,
                                    borderRadius: '50%',
                                    bgcolor: step.color,
                                    display: 'flex',
                                    alignItems: 'center',
                                    justifyContent: 'center',
                                    mr: 3,
                                    position: 'relative',
                                    zIndex: 1,
                                    color: 'white'
                                }}
                            >
                                {step.icon}
                            </Box>

                            <Box sx={{ flex: 1, pt: 1 }}>
                                <Typography variant="h4" gutterBottom sx={{ fontWeight: 'bold', color: step.color }}>
                                    {index + 1}. {step.label}
                                </Typography>
                                <Typography variant="body1" color="text.secondary" sx={{ fontSize: '1.1rem' }}>
                                    {step.description}
                                </Typography>
                            </Box>
                        </Box>
                    ))}
                </Box>
            </Box>

            <Box sx={{ textAlign: 'center', py: 6, bgcolor: 'background.paper', borderRadius: 2 }}>
                <Typography variant="h3" gutterBottom sx={{ fontWeight: 'bold', mb: 3 }}>
                    Ready to Get Started?
                </Typography>
                <Typography variant="h6" color="text.secondary" sx={{ mb: 4, maxWidth: 600, mx: 'auto' }}>
                    Join thousands of satisfied customers who trust us with their custom printing needs.
                    Quality guaranteed, fast turnaround, and competitive prices.
                </Typography>
                <Button
                    variant="contained"
                    size="large"
                    sx={{ px: 6, py: 2, fontSize: '1.2rem' }}
                    startIcon={<CheckCircle />}
                >
                    Start Your Order Today
                </Button>
            </Box>
        </Container>
    );
}