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
    Alert,
    TextField,
    InputAdornment,
    Pagination,
    FormControl,
    Select,
    MenuItem,
    InputLabel,
    Drawer,
    List,
    ListItem,
    ListItemButton,
    ListItemText,
    Collapse,
    IconButton,
    useMediaQuery,
    useTheme,
    Paper,
    Slider,
    Switch,
    FormControlLabel
} from '@mui/material';
import { useState, useEffect, useCallback } from 'react';
import {
    Search,
    ExpandLess,
    ExpandMore,
    Close as CloseIcon,
    Tune as TuneIcon
} from '@mui/icons-material';
import clientApi from "@/lib/clientApi";
import useRoles from "@/app/components/hooks/useRoles";

const SORT_OPTIONS = [
    { value: 'Name-ASC', label: 'Name (A-Z)' },
    { value: 'Name-DESC', label: 'Name (Z-A)' },
    { value: 'Price-ASC', label: 'Price (Low to High)' },
    { value: 'Price-DESC', label: 'Price (High to Low)' },
    { value: 'CreatedDate-DESC', label: 'Newest First' },
    { value: 'CreatedDate-ASC', label: 'Oldest First' }
];

const PAGE_SIZE_OPTIONS = [12, 24, 48, 96];

export default function GalleryPage() {
    const theme = useTheme();
    const isMobile = useMediaQuery(theme.breakpoints.down('md'));
    const { isAdmin } = useRoles();

    const heightOffset = isAdmin ? 192 : 128;

    const [products, setProducts] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const [totalPages, setTotalPages] = useState(0);
    const [totalCount, setTotalCount] = useState(0);

    const [categories, setCategories] = useState([]);
    const [categoriesLoading, setCategoriesLoading] = useState(true);
    const [expandedCategories, setExpandedCategories] = useState(new Set());

    const [filters, setFilters] = useState({
        pageNumber: 1,
        pageSize: 24,
        searchTerm: '',
        categoryId: '',
        minPrice: 0,
        maxPrice: 1000,
        isActive: true,
        isCustomizable: null,
        sortBy: 'Name',
        sortDirection: 'ASC'
    });

    const [mobileDrawerOpen, setMobileDrawerOpen] = useState(false);
    const [priceRange, setPriceRange] = useState([0, 1000]);
    const [searchInput, setSearchInput] = useState('');

    useEffect(() => {
        const fetchCategories = async () => {
            try {
                const response = await clientApi.get('/products/categories');
                setCategories(response.data);
            } catch (err) {
                console.error('Error fetching categories:', err);
            } finally {
                setCategoriesLoading(false);
            }
        };

        fetchCategories();
    }, []);

    const fetchProducts = useCallback(async () => {
        setLoading(true);
        try {
            const params = {
                PageNumber: filters.pageNumber,
                PageSize: filters.pageSize,
                IsActive: filters.isActive,
                SortBy: filters.sortBy,
                SortDirection: filters.sortDirection
            };

            if (filters.searchTerm) params.SearchTerm = filters.searchTerm;
            if (filters.categoryId) params.CategoryId = filters.categoryId;
            if (filters.minPrice > 0) params.MinPrice = filters.minPrice;
            if (filters.maxPrice < 1000) params.MaxPrice = filters.maxPrice;
            if (filters.isCustomizable !== null) params.IsCustomizable = filters.isCustomizable;

            const response = await clientApi.get('/products/', { params });

            setProducts(response.data.items);
            setTotalPages(response.data.totalPages);
            setTotalCount(response.data.totalCount);
        } catch (err) {
            setError('Failed to load products');
            console.error('Error fetching products:', err);
        } finally {
            setLoading(false);
        }
    }, [filters]);

    useEffect(() => {
        fetchProducts();
    }, [fetchProducts]);

    const handleFilterChange = (key, value) => {
        setFilters(prev => ({
            ...prev,
            [key]: value,
            pageNumber: key !== 'pageNumber' ? 1 : value
        }));
    };

    const handleSearch = () => {
        handleFilterChange('searchTerm', searchInput);
    };

    const handlePriceRangeChange = (event, newValue) => {
        setPriceRange(newValue);
    };

    const handlePriceRangeCommitted = (event, newValue) => {
        handleFilterChange('minPrice', newValue[0]);
        handleFilterChange('maxPrice', newValue[1]);
    };

    const handleSortChange = (value) => {
        const [sortBy, sortDirection] = value.split('-');
        handleFilterChange('sortBy', sortBy);
        handleFilterChange('sortDirection', sortDirection);
    };

    const toggleCategoryExpansion = (categoryId) => {
        const newExpanded = new Set(expandedCategories);
        if (newExpanded.has(categoryId)) {
            newExpanded.delete(categoryId);
        } else {
            newExpanded.add(categoryId);
        }
        setExpandedCategories(newExpanded);
    };

    const getChildCategories = (parentId) => {
        return categories.filter(cat => cat.parentCategoryId === parentId);
    };

    const getParentCategories = () => {
        return categories.filter(cat => !cat.parentCategoryId);
    };

    const CategorySidebar = () => (
        <Box sx={{
            width: 300,
            height: '100%',
            display: 'flex',
            flexDirection: 'column'
        }}>
            <Typography variant="h6" gutterBottom sx={{
                fontWeight: 'bold',
                mb: 2,
                p: 2,
                pb: 1,
                flexShrink: 0,
                borderBottom: 1,
                borderColor: 'divider'
            }}>
                Categories
            </Typography>

            <Box sx={{
                flex: 1,
                overflowY: 'auto',
                px: 2,
                minHeight: 0
            }}>
                <List dense>
                    <ListItem disablePadding>
                        <ListItemButton
                            selected={!filters.categoryId}
                            onClick={() => handleFilterChange('categoryId', '')}
                            sx={{ borderRadius: 1, mb: 0.5 }}
                        >
                            <ListItemText primary="All Products" />
                        </ListItemButton>
                    </ListItem>

                    {getParentCategories().map((category) => {
                        const childCategories = getChildCategories(category.id);
                        const hasChildren = childCategories.length > 0;
                        const isExpanded = expandedCategories.has(category.id);

                        return (
                            <Box key={category.id}>
                                <ListItem disablePadding>
                                    <ListItemButton
                                        selected={filters.categoryId === category.id}
                                        onClick={() => handleFilterChange('categoryId', category.id)}
                                        sx={{ borderRadius: 1, mb: 0.5 }}
                                    >
                                        <ListItemText primary={category.name} />
                                        {hasChildren && (
                                            <IconButton
                                                size="small"
                                                onClick={(e) => {
                                                    e.stopPropagation();
                                                    toggleCategoryExpansion(category.id);
                                                }}
                                            >
                                                {isExpanded ? <ExpandLess /> : <ExpandMore />}
                                            </IconButton>
                                        )}
                                    </ListItemButton>
                                </ListItem>

                                {hasChildren && (
                                    <Collapse in={isExpanded} timeout="auto" unmountOnExit>
                                        <List component="div" disablePadding>
                                            {childCategories.map((childCategory) => (
                                                <ListItem key={childCategory.id} disablePadding sx={{ pl: 3 }}>
                                                    <ListItemButton
                                                        selected={filters.categoryId === childCategory.id}
                                                        onClick={() => handleFilterChange('categoryId', childCategory.id)}
                                                        sx={{ borderRadius: 1, mb: 0.5 }}
                                                    >
                                                        <ListItemText
                                                            primary={childCategory.name}
                                                            sx={{ '& .MuiListItemText-primary': { fontSize: '0.9rem' } }}
                                                        />
                                                    </ListItemButton>
                                                </ListItem>
                                            ))}
                                        </List>
                                    </Collapse>
                                )}
                            </Box>
                        );
                    })}
                </List>
            </Box>

            <Box sx={{
                p: 2,
                borderTop: 1,
                borderColor: 'divider',
                flexShrink: 0,
                backgroundColor: 'background.paper'
            }}>
                <Typography variant="subtitle2" gutterBottom sx={{ fontWeight: 'bold' }}>
                    Price Range
                </Typography>
                <Box sx={{ px: 1, mb: 2 }}>
                    <Slider
                        value={priceRange}
                        onChange={handlePriceRangeChange}
                        onChangeCommitted={handlePriceRangeCommitted}
                        valueLabelDisplay="auto"
                        min={0}
                        max={1000}
                        step={5}
                        valueLabelFormat={(value) => `$${value}`}
                    />
                    <Box sx={{ display: 'flex', justifyContent: 'space-between', mt: 1 }}>
                        <Typography variant="caption">${priceRange[0]}</Typography>
                        <Typography variant="caption">${priceRange[1]}</Typography>
                    </Box>
                </Box>

                <FormControlLabel
                    control={
                        <Switch
                            checked={filters.isCustomizable === true}
                            onChange={(e) => handleFilterChange('isCustomizable', e.target.checked ? true : null)}
                        />
                    }
                    label="Customizable Only"
                    sx={{ mb: 0 }}
                />
            </Box>
        </Box>
    );

    return (
        <Container maxWidth="xl" sx={{ py: { xs: 1, sm: 2, md: 4 } }}>
            <Box sx={{
                display: 'flex',
                gap: { xs: 0, md: 3 },
                minHeight: { xs: 'auto', md: `calc(100vh - ${heightOffset}px)` }
            }}>
                {!isMobile && (
                    <Box sx={{ flexShrink: 0 }}>
                        <Paper
                            elevation={1}
                            sx={{
                                height: `calc(100vh - ${heightOffset}px)`,
                                overflow: 'hidden',
                                display: 'flex',
                                flexDirection: 'column'
                            }}
                        >
                            {categoriesLoading ? (
                                <Box sx={{ p: 3, textAlign: 'center' }}>
                                    <CircularProgress size={40} />
                                </Box>
                            ) : (
                                <CategorySidebar />
                            )}
                        </Paper>
                    </Box>
                )}

                <Box sx={{
                    flexGrow: 1,
                    minWidth: 0,
                    display: 'flex',
                    flexDirection: 'column',
                    maxHeight: { xs: 'none', md: `calc(100vh - ${heightOffset}px)` }
                }}>
                    <Box sx={{
                        flex: 1,
                        overflowY: { xs: 'visible', md: 'auto' },
                        minHeight: 0,
                        pr: { xs: 0, md: 1 }
                    }}>
                        <Box sx={{ mb: { xs: 2, sm: 2, md: 3 } }}>
                            <Typography variant="h3" gutterBottom sx={{
                                fontWeight: 'bold',
                                fontSize: { xs: '2rem', sm: '2rem', md: '2rem' }
                            }}>
                                Product Gallery
                            </Typography>
                            <Typography variant="h6" color="text.secondary" sx={{
                                fontSize: { xs: '1rem', sm: '1rem' }
                            }}>
                                Explore our complete collection of customizable products
                            </Typography>
                        </Box>

                        <Paper elevation={1} sx={{
                            p: { xs: 1, sm: 1, md: 1.5 },
                            mb: { xs: 1, sm: 2 }
                        }}>
                            <Grid container spacing={{ xs: 1, sm: 2 }} alignItems="center">
                                {isMobile && (
                                    <Grid item>
                                        <IconButton
                                            onClick={() => setMobileDrawerOpen(true)}
                                            sx={{ mr: 1 }}
                                            size="small"
                                        >
                                            <TuneIcon />
                                        </IconButton>
                                    </Grid>
                                )}

                                <Grid item xs={12} sm={6} md={4}>
                                    <TextField
                                        fullWidth
                                        placeholder="Search products..."
                                        value={searchInput}
                                        onChange={(e) => setSearchInput(e.target.value)}
                                        onKeyPress={(e) => e.key === 'Enter' && handleSearch()}
                                        size={isMobile ? "small" : "medium"}
                                        InputProps={{
                                            startAdornment: (
                                                <InputAdornment position="start">
                                                    <Search fontSize={isMobile ? "small" : "medium"} />
                                                </InputAdornment>
                                            ),
                                            endAdornment: searchInput && (
                                                <InputAdornment position="end">
                                                    <IconButton
                                                        size="small"
                                                        onClick={handleSearch}
                                                        edge="end"
                                                    >
                                                        <Search fontSize="small" />
                                                    </IconButton>
                                                </InputAdornment>
                                            )
                                        }}
                                    />
                                </Grid>

                                <Grid item xs={6} sm={3} md={3}>
                                    <FormControl fullWidth size={isMobile ? "small" : "medium"}>
                                        <InputLabel>Sort By</InputLabel>
                                        <Select
                                            value={`${filters.sortBy}-${filters.sortDirection}`}
                                            label="Sort By"
                                            onChange={(e) => handleSortChange(e.target.value)}
                                         >
                                            {SORT_OPTIONS.map((option) => (
                                                <MenuItem key={option.value} value={option.value}>
                                                    {option.label}
                                                </MenuItem>
                                            ))}
                                        </Select>
                                    </FormControl>
                                </Grid>

                                <Grid item xs={6} sm={3} md={2}>
                                    <FormControl fullWidth size={isMobile ? "small" : "medium"}>
                                        <InputLabel>Per Page</InputLabel>
                                        <Select
                                            value={filters.pageSize}
                                            label="Per Page"
                                            onChange={(e) => handleFilterChange('pageSize', e.target.value)}
                                        >
                                            {PAGE_SIZE_OPTIONS.map((size) => (
                                                <MenuItem key={size} value={size}>
                                                    {size}
                                                </MenuItem>
                                            ))}
                                        </Select>
                                    </FormControl>
                                </Grid>

                                <Grid item xs={12} md={3} sx={{ textAlign: { xs: 'center', md: 'right' } }}>
                                    <Typography variant="body2" color="text.secondary" sx={{
                                        fontSize: { xs: '0.75rem', sm: '0.875rem' }
                                    }}>
                                        Showing {products.length} of {totalCount} products
                                    </Typography>
                                </Grid>
                            </Grid>
                        </Paper>

                        {loading && (
                            <Box sx={{ display: 'flex', justifyContent: 'center', py: { xs: 4, md: 8 } }}>
                                <CircularProgress size={isMobile ? 40 : 60} />
                            </Box>
                        )}

                        {error && (
                            <Alert severity="error" sx={{ mb: { xs: 2, md: 4 } }}>
                                {error}
                            </Alert>
                        )}

                        {!loading && !error && products.length === 0 && (
                            <Paper sx={{ p: { xs: 3, md: 6 }, textAlign: 'center' }}>
                                <Typography variant="h6" color="text.secondary" gutterBottom>
                                    No products found
                                </Typography>
                                <Typography variant="body2" color="text.secondary">
                                    Try adjusting your search criteria or filters
                                </Typography>
                            </Paper>
                        )}

                        {!loading && !error && products.length > 0 && (
                            <Box sx={{
                                display: 'flex',
                                flexWrap: 'wrap',
                                gap: { xs: 1, sm: 1.5, md: 2 },
                                mb: { xs: 2, md: 4 }
                            }}>
                                {products.map((product) => (
                                    <Card
                                        key={product.id}
                                        sx={{
                                            width: {
                                                xs: 'calc(50% - 4px)',
                                                sm: 'calc(50% - 12px)',
                                                lg: 'calc(33.333% - 16px)'
                                            },
                                            maxWidth: { xs: 'none', sm: 350, lg: 370 },
                                            height: { xs: 300, sm: 380, lg: 420 },
                                            display: 'flex',
                                            flexDirection: 'column',
                                            transition: 'transform 0.3s ease-in-out, box-shadow 0.3s ease-in-out',
                                            '&:hover': {
                                                transform: { xs: 'none', sm: 'translateY(-4px)', md: 'translateY(-8px)' },
                                                boxShadow: { xs: 2, sm: 4, md: 6 }
                                            }
                                        }}
                                    >
                                        <CardMedia
                                            component="img"
                                            image={product.imageUrl || '/placeholder-product.jpg'}
                                            alt={product.name}
                                            sx={{
                                                objectFit: 'cover',
                                                height: { xs: 120, sm: 160, lg: 180 }
                                            }}
                                        />
                                        <CardContent sx={{
                                            flexGrow: 1,
                                            display: 'flex',
                                            flexDirection: 'column',
                                            p: { xs: 1, sm: 1.5, lg: 2 },
                                            '&:last-child': { pb: { xs: 1, sm: 1.5, lg: 2 } }
                                        }}>
                                            <Typography variant="h6" gutterBottom sx={{
                                                fontWeight: 'bold',
                                                overflow: 'hidden',
                                                textOverflow: 'ellipsis',
                                                whiteSpace: 'nowrap',
                                                fontSize: { xs: '0.9rem', sm: '1rem', md: '1.1rem' },
                                                mb: { xs: 0.5, sm: 1 }
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
                                                        mb: { xs: 0.5, sm: 1 },
                                                        fontSize: { xs: '0.65rem', sm: '0.75rem' },
                                                        height: { xs: 20, sm: 24 }
                                                    }}
                                                />
                                            )}

                                            <Typography variant="body2" color="text.secondary" sx={{
                                                flexGrow: 1,
                                                mb: { xs: 1, sm: 1.5 },
                                                overflow: 'hidden',
                                                display: '-webkit-box',
                                                WebkitLineClamp: { xs: 2, sm: 2, md: 3 },
                                                WebkitBoxOrient: 'vertical',
                                                minHeight: { xs: 28, sm: 32, md: 48 },
                                                fontSize: { xs: '0.75rem', sm: '0.8rem', md: '0.875rem' }
                                            }}>
                                                {product.description}
                                            </Typography>

                                            <Box sx={{
                                                display: 'flex',
                                                alignItems: 'center',
                                                justifyContent: 'space-between',
                                                mb: { xs: 1, sm: 1.5 }
                                            }}>
                                                <Typography variant="subtitle1" color="primary" sx={{
                                                    fontWeight: 'bold',
                                                    fontSize: { xs: '0.85rem', sm: '0.95rem', md: '1rem' }
                                                }}>
                                                    From ${product.basePrice?.toFixed(2)}
                                                </Typography>
                                                {product.isCustomizable && (
                                                    <Chip
                                                        label="Custom"
                                                        color="primary"
                                                        size="small"
                                                        sx={{
                                                            fontSize: { xs: '0.65rem', sm: '0.75rem' },
                                                            height: { xs: 20, sm: 24 }
                                                        }}
                                                    />
                                                )}
                                            </Box>

                                            <Button
                                                variant="contained"
                                                fullWidth
                                                size={isMobile ? "small" : "medium"}
                                                sx={{
                                                    fontSize: { xs: '0.75rem', sm: '0.875rem', md: '1rem' },
                                                    py: { xs: 0.5, sm: 1, md: 1.5 }
                                                }}
                                            >
                                                Customize
                                            </Button>
                                        </CardContent>
                                    </Card>
                                ))}
                            </Box>
                        )}

                        {!loading && !error && totalPages > 1 && (
                            <Box sx={{
                                display: 'flex',
                                justifyContent: 'center',
                                pb: { xs: 2, md: 4 }
                            }}>
                                <Pagination
                                    count={totalPages}
                                    page={filters.pageNumber}
                                    onChange={(e, page) => handleFilterChange('pageNumber', page)}
                                    color="primary"
                                    size={isMobile ? 'small' : 'large'}
                                    showFirstButton={!isMobile}
                                    showLastButton={!isMobile}
                                />
                            </Box>
                        )}
                    </Box>
                </Box>
            </Box>

            <Drawer
                anchor="left"
                open={mobileDrawerOpen}
                onClose={() => setMobileDrawerOpen(false)}
                ModalProps={{ keepMounted: true }}
                PaperProps={{
                    sx: {
                        width: 280,
                        display: 'flex',
                        flexDirection: 'column'
                    }
                }}
            >
                <Box sx={{ display: 'flex', alignItems: 'center', p: 2, borderBottom: 1, borderColor: 'divider', flexShrink: 0 }}>
                    <Typography variant="h6" sx={{ flexGrow: 1, fontWeight: 'bold' }}>
                        Filters
                    </Typography>
                    <IconButton onClick={() => setMobileDrawerOpen(false)}>
                        <CloseIcon />
                    </IconButton>
                </Box>
                {categoriesLoading ? (
                    <Box sx={{ p: 3, textAlign: 'center' }}>
                        <CircularProgress size={40} />
                    </Box>
                ) : (
                    <CategorySidebar />
                )}
            </Drawer>
        </Container>
    );
}