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
import { ShoppingCart, Palette, ImageOutlined, CreditCard, LocalShipping, CheckCircle } from '@mui/icons-material';
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
                        PageSize: 3,
                        PageNumber: 1,
                        IsActive: true,
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
            number: 1,
            label: 'Pick an Item',
            description: 'Browse our extensive collection of customizable products and select the perfect base for your design. From premium t-shirts and hoodies to mugs, phone cases, and more - we have everything you need to bring your vision to life.',
            icon: <ShoppingCart sx={{ fontSize: 80 }} />,
            details: 'Explore hundreds of high-quality products across multiple categories. Filter by material, size, color, and price to find exactly what you\'re looking for.'
        },
        {
            number: 2,
            label: 'Choose Variant',
            description: 'Select from available sizes, colors, and material options that match your preferences and needs. Each product comes with detailed specifications and sizing guides.',
            icon: <Palette sx={{ fontSize: 80 }} />,
            details: 'View real-time previews of different variants. Check material quality, durability ratings, and care instructions for each option.'
        },
        {
            number: 3,
            label: 'Customize with Images',
            description: 'Upload your own designs, add custom text, or use our intuitive design tools to create something truly unique. Our editor supports various file formats and offers professional design features.',
            icon: <ImageOutlined sx={{ fontSize: 80 }} />,
            details: 'Drag and drop images, adjust positioning, add filters, create text overlays, and preview your design in real-time on the selected product.'
        },
        {
            number: 4,
            label: 'Pay',
            description: 'Complete your order with our secure checkout process. We accept multiple payment methods and provide instant order confirmation with detailed receipts.',
            icon: <CreditCard sx={{ fontSize: 80 }} />,
            details: 'Review your design, confirm quantities, apply discount codes, and choose from various secure payment options including cards, PayPal, and more.'
        },
        {
            number: 5,
            label: 'Wait for Order',
            description: 'Sit back and relax while we handle the rest. Our professional printing team will carefully produce your custom item and ship it directly to your door.',
            icon: <LocalShipping sx={{ fontSize: 80 }} />,
            details: 'Track your order status in real-time, from printing to packaging to shipping. Receive updates via email and SMS throughout the process.'
        }
    ];

    return (
        <Container maxWidth="lg" sx={{ py: 4 }}>
            <Grid container spacing={6} alignItems="center" sx={{ mb: 6, minHeight: '70vh' }}>
                <Grid item xs={12} md={7}>
                    <Box sx={{ textAlign: { xs: 'center', md: 'left' } }}>
                        <Typography
                            variant="h1"
                            gutterBottom
                            sx={{
                                fontWeight: 'bold',
                                mb: 3,
                                fontSize: { xs: '2.5rem', md: '3.5rem', lg: '4rem' },
                                lineHeight: 1.2
                            }}
                        >
                            Custom Printing<br />
                            <Box component="span" sx={{ color: 'primary.main' }}>Made Simple</Box>
                        </Typography>
                        <Typography
                            variant="h5"
                            color="text.secondary"
                            sx={{
                                mb: 4,
                                lineHeight: 1.6,
                                fontSize: { xs: '1.2rem', md: '1.4rem' }
                            }}
                        >
                            Transform your ideas into reality with our premium custom printing services.
                            From t-shirts to mugs, we bring your designs to life with professional quality
                            and lightning-fast turnaround times.
                        </Typography>

                        <Box sx={{ mb: 4 }}>
                            <Grid container spacing={3}>
                                <Grid item xs={12} sm={6}>
                                    <Box sx={{ display: 'flex', alignItems: 'center', mb: 2 }}>
                                        <CheckCircle sx={{ color: 'primary.main', mr: 2 }} />
                                        <Typography variant="body1" fontWeight="medium">
                                            Professional Quality Guaranteed
                                        </Typography>
                                    </Box>
                                </Grid>
                                <Grid item xs={12} sm={6}>
                                    <Box sx={{ display: 'flex', alignItems: 'center', mb: 2 }}>
                                        <CheckCircle sx={{ color: 'primary.main', mr: 2 }} />
                                        <Typography variant="body1" fontWeight="medium">
                                            Fast 24-48 Hour Turnaround
                                        </Typography>
                                    </Box>
                                </Grid>
                                <Grid item xs={12} sm={6}>
                                    <Box sx={{ display: 'flex', alignItems: 'center', mb: 2 }}>
                                        <CheckCircle sx={{ color: 'primary.main', mr: 2 }} />
                                        <Typography variant="body1" fontWeight="medium">
                                            Free Design Support
                                        </Typography>
                                    </Box>
                                </Grid>
                                <Grid item xs={12} sm={6}>
                                    <Box sx={{ display: 'flex', alignItems: 'center', mb: 2 }}>
                                        <CheckCircle sx={{ color: 'primary.main', mr: 2 }} />
                                        <Typography variant="body1" fontWeight="medium">
                                            100% Satisfaction Promise
                                        </Typography>
                                    </Box>
                                </Grid>
                            </Grid>
                        </Box>

                        <Box sx={{ display: 'flex', gap: 2, justifyContent: { xs: 'center', md: 'flex-start' }, flexWrap: 'wrap', mb: 4 }}>
                            <Button
                                variant="contained"
                                size="large"
                                href="/gallery"
                                sx={{ px: 5, py: 2, fontSize: '1.2rem' }}
                            >
                                View Gallery
                            </Button>
                        </Box>

                        <Typography variant="body2" color="text.secondary">
                            ⭐⭐⭐⭐⭐ Trusted by 10,000+ customers • 4.9/5 rating
                        </Typography>
                    </Box>
                </Grid>
            </Grid>

            <Box sx={{ mb: 12 }}>
                <Box sx={{ mb: 6, textAlign: { xs: 'center', md: 'left' } }}>
                    <Typography variant="h2" gutterBottom sx={{ fontWeight: 'bold', mb: 2 }}>
                        Featured Products
                    </Typography>
                    <Typography variant="h6" color="text.secondary" sx={{ maxWidth: 600 }}>
                        Discover our most popular customizable products. Each item is carefully selected
                        for quality and perfect for personalization.
                    </Typography>
                </Box>

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
                    <Box sx={{
                        display: 'flex',
                        flexWrap: 'wrap',
                        gap: 3,
                        justifyContent: { xs: 'center', lg: 'flex-start' }
                    }}>
                        {products.map((product) => (
                            <Card
                                key={product.id}
                                sx={{
                                    width: { xs: '100%', sm: 'calc(50% - 12px)', lg: 'calc(33.333% - 16px)' },
                                    maxWidth: { xs: 400, sm: 350, lg: 370 },
                                    height: { xs: 450, sm: 480, lg: 500 },
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
                                    height="200"
                                    image={product.imageUrl || '/placeholder-product.jpg'}
                                    alt={product.name}
                                    sx={{
                                        objectFit: 'cover',
                                        height: { xs: 180, sm: 200, lg: 220 }
                                    }}
                                />
                                <CardContent sx={{
                                    flexGrow: 1,
                                    display: 'flex',
                                    flexDirection: 'column',
                                    p: { xs: 2, sm: 2.5, lg: 3 },
                                    '&:last-child': { pb: { xs: 2, sm: 2.5, lg: 3 } }
                                }}>
                                    <Typography variant="h6" gutterBottom sx={{
                                        fontWeight: 'bold',
                                        overflow: 'hidden',
                                        textOverflow: 'ellipsis',
                                        whiteSpace: 'nowrap',
                                        fontSize: { xs: '1.1rem', sm: '1.25rem' }
                                    }}>
                                        {product.name}
                                    </Typography>
                                    <Typography variant="body2" color="text.secondary" sx={{
                                        flexGrow: 1,
                                        mb: 2,
                                        overflow: 'hidden',
                                        display: '-webkit-box',
                                        WebkitLineClamp: { xs: 2, sm: 3 },
                                        WebkitBoxOrient: 'vertical',
                                        minHeight: { xs: 36, sm: 54 },
                                        fontSize: { xs: '0.875rem', sm: '0.875rem' }
                                    }}>
                                        {product.description}
                                    </Typography>
                                    <Box sx={{ display: 'flex', alignItems: 'center', justifyContent: 'space-between', mb: 2 }}>
                                        <Typography variant="subtitle1" color="primary" sx={{
                                            fontWeight: 'bold',
                                            fontSize: { xs: '1rem', sm: '1.1rem' }
                                        }}>
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
                                        size="medium"
                                        sx={{
                                            fontSize: { xs: '0.875rem', sm: '1rem' },
                                            py: { xs: 1, sm: 1.5 }
                                        }}
                                    >
                                        Customize
                                    </Button>
                                </CardContent>
                            </Card>
                        ))}
                    </Box>
                )}
            </Box>

            <Box sx={{ mb: 12, py: 8 }}>
                <Container maxWidth="lg">
                    <Box sx={{ mb: 8 }}>
                        <Typography variant="h2" gutterBottom sx={{ fontWeight: 'bold', mb: 2 }}>
                            How It Works
                        </Typography>
                        <Typography variant="h5" color="text.secondary" sx={{ maxWidth: 600 }}>
                            Our streamlined process makes custom printing simple and stress-free.
                            Follow these five easy steps to get your perfect custom products.
                        </Typography>
                    </Box>

                    <Grid container spacing={6}>
                        {steps.map((step, index) => (
                            <Grid item xs={12} key={index}>
                                <Card
                                    sx={{
                                        p: 4,
                                        boxShadow: 2,
                                        borderRadius: 3,
                                        transition: 'all 0.3s ease',
                                        '&:hover': {
                                            boxShadow: 8,
                                            transform: 'translateY(-4px)'
                                        }
                                    }}
                                >
                                    <Grid container spacing={4} alignItems="center">
                                        <Grid item xs={12} md={2}>
                                            <Box
                                                sx={{
                                                    display: 'flex',
                                                    alignItems: 'center',
                                                    justifyContent: { xs: 'center', md: 'flex-start' },
                                                    mb: { xs: 2, md: 0 }
                                                }}
                                            >
                                                <Box
                                                    sx={{
                                                        width: 100,
                                                        height: 100,
                                                        borderRadius: 2,
                                                        bgcolor: 'primary.main',
                                                        display: 'flex',
                                                        alignItems: 'center',
                                                        justifyContent: 'center',
                                                        color: 'white',
                                                        mr: 2
                                                    }}
                                                >
                                                    {step.icon}
                                                </Box>
                                                <Box
                                                    sx={{
                                                        width: 60,
                                                        height: 60,
                                                        borderRadius: '50%',
                                                        bgcolor: 'primary.main',
                                                        display: 'flex',
                                                        alignItems: 'center',
                                                        justifyContent: 'center',
                                                        color: 'white',
                                                        fontSize: '1.5rem',
                                                        fontWeight: 'bold'
                                                    }}
                                                >
                                                    {step.number}
                                                </Box>
                                            </Box>
                                        </Grid>

                                        <Grid item xs={12} md={10}>
                                            <Box sx={{ textAlign: { xs: 'center', md: 'left' } }}>
                                                <Typography
                                                    variant="h4"
                                                    gutterBottom
                                                    sx={{
                                                        fontWeight: 'bold',
                                                        color: 'primary.main',
                                                        mb: 2
                                                    }}
                                                >
                                                    {step.label}
                                                </Typography>
                                                <Typography
                                                    variant="h6"
                                                    paragraph
                                                    sx={{
                                                        mb: 3,
                                                        lineHeight: 1.6,
                                                        color: 'text.primary',
                                                        fontWeight: 400
                                                    }}
                                                >
                                                    {step.description}
                                                </Typography>
                                                <Typography
                                                    variant="body1"
                                                    color="text.secondary"
                                                    sx={{
                                                        lineHeight: 1.7,
                                                        fontSize: '1.1rem'
                                                    }}
                                                >
                                                    {step.details}
                                                </Typography>
                                            </Box>
                                        </Grid>
                                    </Grid>
                                </Card>
                            </Grid>
                        ))}
                    </Grid>
                </Container>
            </Box>

            <Box sx={{ textAlign: 'center', py: 8, bgcolor: 'background.paper', borderRadius: 2, boxShadow: 1 }}>
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