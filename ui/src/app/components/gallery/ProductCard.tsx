import {
    Card,
    CardContent,
    CardMedia,
    Typography,
    Button,
    Chip,
    Box,
    useMediaQuery,
    useTheme
} from '@mui/material';
import { useUser } from '@auth0/nextjs-auth0';
import { useRouter } from 'next/navigation';
import { GalleryProduct } from '@/types';

interface ProductCardProps {
    product: GalleryProduct;
}

export default function ProductCard({ product }: ProductCardProps) {
    const theme = useTheme();
    const isMobile = useMediaQuery(theme.breakpoints.down('md'));
    const { user, isLoading } = useUser();
    const router = useRouter();

    const handleViewProduct = () => {
        router.push(`/products/${product.id}`);
    };

    const handleLoginRedirect = () => {
        router.push('/auth/login');
    };

    const handleBuild = () => {
        router.push(`/builder/${product.id}`);
    };

    return (
        <Card
            sx={{
                width: {
                    xs: 'calc(50% - 6px)',
                    sm: 'calc(50% - 8px)',
                    lg: 'calc(33.333% - 12px)'
                },
                maxWidth: { xs: 'none', sm: 280, lg: 320 },
                height: { xs: 240, sm: 300, lg: 340 },
                display: 'flex',
                flexDirection: 'column',
                transition: 'transform 0.3s ease-in-out, box-shadow 0.3s ease-in-out',
                '&:hover': {
                    transform: { xs: 'none', sm: 'translateY(-2px)', md: 'translateY(-4px)' },
                    boxShadow: { xs: 1, sm: 3, md: 4 }
                }
            }}
        >
            <CardMedia
                component="img"
                image={product.imageUrl || '/placeholder-product.jpg'}
                alt={product.name}
                sx={{
                    objectFit: 'cover',
                    height: { xs: 80, sm: 120, lg: 140 }
                }}
            />
            <CardContent sx={{
                flexGrow: 1,
                display: 'flex',
                flexDirection: 'column',
                p: { xs: 0.75, sm: 1, lg: 1.25 },
                '&:last-child': { pb: { xs: 0.75, sm: 1, lg: 1.25 } }
            }}>
                <Typography variant="h6" gutterBottom sx={{
                    fontWeight: 600,
                    overflow: 'hidden',
                    textOverflow: 'ellipsis',
                    whiteSpace: 'nowrap',
                    fontSize: { xs: '0.75rem', sm: '0.85rem', md: '0.9rem' },
                    mb: { xs: 0.25, sm: 0.5 },
                    lineHeight: 1.2
                }}>
                    {product.name}
                </Typography>

                {product.category && (
                    <Chip
                        label={product.category.name}
                        size="small"
                        color="secondary"
                        sx={{
                            alignSelf: 'flex-start',
                            mb: { xs: 0.25, sm: 0.5 },
                            fontSize: { xs: '0.55rem', sm: '0.65rem' },
                            height: { xs: 16, sm: 20 },
                            '& .MuiChip-label': {
                                px: { xs: 0.5, sm: 0.75 }
                            }
                        }}
                    />
                )}

                <Typography variant="body2" color="text.secondary" sx={{
                    flexGrow: 1,
                    mb: { xs: 0.5, sm: 0.75 },
                    overflow: 'hidden',
                    display: '-webkit-box',
                    WebkitLineClamp: { xs: 1, sm: 2, md: 2 },
                    WebkitBoxOrient: 'vertical',
                    minHeight: { xs: 16, sm: 24, md: 32 },
                    fontSize: { xs: '0.65rem', sm: '0.7rem', md: '0.75rem' },
                    lineHeight: 1.3
                }}>
                    {product.description}
                </Typography>

                <Box sx={{
                    display: 'flex',
                    alignItems: 'center',
                    justifyContent: 'space-between',
                    mb: { xs: 0.5, sm: 0.75 }
                }}>
                    <Typography variant="subtitle1" color="primary" sx={{
                        fontWeight: 600,
                        fontSize: { xs: '0.7rem', sm: '0.8rem', md: '0.85rem' }
                    }}>
                        From ${product.basePrice?.toFixed(2)}
                    </Typography>
                    {product.isCustomizable && (
                        <Chip
                            label="Custom"
                            color="primary"
                            size="small"
                            sx={{
                                fontSize: { xs: '0.55rem', sm: '0.65rem' },
                                height: { xs: 16, sm: 20 },
                                '& .MuiChip-label': {
                                    px: { xs: 0.5, sm: 0.75 }
                                }
                            }}
                        />
                    )}
                </Box>

                {!isLoading && (
                    <Box sx={{ 
                        display: 'flex', 
                        gap: { xs: 0.5, sm: 0.75 },
                        flexDirection: { xs: 'column', sm: user ? 'row' : 'column' }
                    }}>
                        {user ? (
                            <>
                                <Button
                                    variant="outlined"
                                    size="small"
                                    onClick={handleViewProduct}
                                    sx={{
                                        fontSize: { xs: '0.65rem', sm: '0.7rem' },
                                        py: { xs: 0.25, sm: 0.5 },
                                        flex: 1,
                                        minHeight: { xs: 24, sm: 32 }
                                    }}
                                >
                                    View
                                </Button>
                                <Button
                                    variant="contained"
                                    size="small"
                                    onClick={handleBuild}
                                    sx={{
                                        fontSize: { xs: '0.65rem', sm: '0.7rem' },
                                        py: { xs: 0.25, sm: 0.5 },
                                        flex: 1,
                                        minHeight: { xs: 24, sm: 32 }
                                    }}
                                >
                                    Build
                                </Button>
                            </>
                        ) : (
                            <>
                                <Button
                                    variant="contained"
                                    size="small"
                                    onClick={handleViewProduct}
                                    sx={{
                                        fontSize: { xs: '0.65rem', sm: '0.7rem' },
                                        py: { xs: 0.25, sm: 0.5 },
                                        minHeight: { xs: 24, sm: 32 }
                                    }}
                                >
                                    View Product
                                </Button>
                                <Typography
                                    component="button"
                                    onClick={handleLoginRedirect}
                                    sx={{
                                        fontSize: { xs: '0.6rem', sm: '0.65rem' },
                                        color: 'primary.main',
                                        textDecoration: 'underline',
                                        cursor: 'pointer',
                                        border: 'none',
                                        background: 'none',
                                        p: 0,
                                        textAlign: 'center',
                                        '&:hover': {
                                            color: 'primary.dark'
                                        }
                                    }}
                                >
                                    Login to customize
                                </Typography>
                            </>
                        )}
                    </Box>
                )}
            </CardContent>
        </Card>
    );
}