'use client';

import {
    Box,
    Container,
    Typography,
    CircularProgress,
    Alert,
    Pagination,
    useMediaQuery,
    useTheme,
    Paper
} from '@mui/material';
import {useState, useEffect, useCallback} from 'react';
import clientApi from "@/lib/clientApi";
import useRoles from "@/app/components/hooks/useRoles";
import ProductCard from '@/app/components/gallery/ProductCard';
import CategorySidebar from '@/app/components/gallery/CategorySidebar';
import SearchFilters from '@/app/components/gallery/SearchFilters';
import MobileFilterDrawer from '@/app/components/gallery/MobileFilterDrawer';
import { GalleryProduct, GalleryCategory, Filters, ProductsResponse, ApiParams } from '@/types';

export default function GalleryPage() {
    const theme = useTheme();
    const isMobile = useMediaQuery(theme.breakpoints.down('md'));
    const { isAdmin } = useRoles();

    const heightOffset = isAdmin ? 192 : 128;

    const [products, setProducts] = useState<GalleryProduct[]>([]);
    const [loading, setLoading] = useState<boolean>(true);
    const [error, setError] = useState<string | null>(null);
    const [totalPages, setTotalPages] = useState<number>(0);
    const [totalCount, setTotalCount] = useState<number>(0);

    const [categories, setCategories] = useState<GalleryCategory[]>([]);
    const [categoriesLoading, setCategoriesLoading] = useState<boolean>(true);
    const [expandedCategories, setExpandedCategories] = useState<Set<string>>(new Set());

    const [filters, setFilters] = useState<Filters>({
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

    const [mobileDrawerOpen, setMobileDrawerOpen] = useState<boolean>(false);
    const [priceRange, setPriceRange] = useState<number[]>([0, 1000]);

    useEffect(() => {
        const fetchCategories = async (): Promise<void> => {
            try {
                const response = await clientApi.get<GalleryCategory[]>('/products/categories');
                setCategories(response.data);
            } catch (err) {
                console.error('Error fetching categories:', err);
            } finally {
                setCategoriesLoading(false);
            }
        };

        fetchCategories().then(r => console.log(r));
    }, []);

    const fetchProducts = useCallback(async (): Promise<void> => {
        setLoading(true);
        try {
            const params: ApiParams = {
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

            const response = await clientApi.get<ProductsResponse>('/products/', { params });

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

    const handleFilterChange = <K extends keyof Filters>(key: K, value: Filters[K]): void => {
        setFilters(prev => ({
            ...prev,
            [key]: value,
            pageNumber: key !== 'pageNumber' ? 1 : (value as number)
        }));
    };

    const handlePriceRangeChange = (event: Event, newValue: number | number[]): void => {
        setPriceRange(newValue as number[]);
    };

    const handlePriceRangeCommitted = (event: Event | React.SyntheticEvent, newValue: number | number[]): void => {
        const range = newValue as number[];
        handleFilterChange('minPrice', range[0]);
        handleFilterChange('maxPrice', range[1]);
    };

    const toggleCategoryExpansion = (categoryId: string): void => {
        const newExpanded = new Set(expandedCategories);
        if (newExpanded.has(categoryId)) {
            newExpanded.delete(categoryId);
        } else {
            newExpanded.add(categoryId);
        }
        setExpandedCategories(newExpanded);
    };

    return (
        <Container maxWidth="xl" sx={{ 
            py: { xs: 0.75, sm: 1.5, md: 3 },
            px: { xs: 1, sm: 2 }
        }}>
            <Box sx={{
                display: 'flex',
                gap: { xs: 0, md: 2 },
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
                                <Box sx={{ p: 2, textAlign: 'center' }}>
                                    <CircularProgress size={32} />
                                </Box>
                            ) : (
                                <CategorySidebar
                                    categories={categories}
                                    filters={filters}
                                    expandedCategories={expandedCategories}
                                    priceRange={priceRange}
                                    onFilterChange={handleFilterChange}
                                    onToggleCategoryExpansion={toggleCategoryExpansion}
                                    onPriceRangeChange={handlePriceRangeChange}
                                    onPriceRangeCommitted={handlePriceRangeCommitted}
                                />
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
                        pr: { xs: 0, md: 0.5 }
                    }}>
                        <Box sx={{ my: { xs: 1.5, sm: 2, md: 3 } }}>
                            <Typography variant="h3" gutterBottom sx={{
                                fontWeight: 700,
                                fontSize: { xs: '1.75rem', sm: '2rem', md: '2.5rem' },
                                lineHeight: 1.2,
                                mb: { xs: 0.5, sm: 0.75 }
                            }}>
                                Product Gallery
                            </Typography>
                            <Typography variant="h6" color="text.secondary" sx={{
                                fontSize: { xs: '0.9rem', sm: '1rem', md: '1.1rem' },
                                lineHeight: 1.3
                            }}>
                                Explore our complete collection of customizable products
                            </Typography>
                        </Box>

                        <SearchFilters
                            filters={filters}
                            totalCount={totalCount}
                            productsCount={products.length}
                            onFilterChange={handleFilterChange}
                            onOpenMobileDrawer={() => setMobileDrawerOpen(true)}
                        />

                        {loading && (
                            <Box sx={{ display: 'flex', justifyContent: 'center', py: { xs: 3, md: 6 } }}>
                                <CircularProgress size={isMobile ? 32 : 48} />
                            </Box>
                        )}

                        {error && (
                            <Alert severity="error" sx={{ 
                                mb: { xs: 1.5, md: 3 },
                                fontSize: { xs: '0.75rem', sm: '0.875rem' }
                            }}>
                                {error}
                            </Alert>
                        )}

                        {!loading && !error && products.length === 0 && (
                            <Paper sx={{ p: { xs: 2, md: 4 }, textAlign: 'center' }}>
                                <Typography variant="h6" color="text.secondary" gutterBottom sx={{
                                    fontSize: { xs: '0.9rem', sm: '1rem' }
                                }}>
                                    No products found
                                </Typography>
                                <Typography variant="body2" color="text.secondary" sx={{
                                    fontSize: { xs: '0.75rem', sm: '0.85rem' }
                                }}>
                                    Try adjusting your search criteria or filters
                                </Typography>
                            </Paper>
                        )}

                        {!loading && !error && products.length > 0 && (
                            <Box sx={{
                                display: 'flex',
                                flexWrap: 'wrap',
                                gap: { xs: 0.75, sm: 1, md: 1.5 },
                                mb: { xs: 1.5, md: 3 }
                            }}>
                                {products.map((product) => (
                                    <ProductCard key={product.id} product={product} />
                                ))}
                            </Box>
                        )}

                        {!loading && !error && totalPages > 1 && (
                            <Box sx={{
                                display: 'flex',
                                justifyContent: 'center',
                                pb: { xs: 1.5, md: 3 }
                            }}>
                                <Pagination
                                    count={totalPages}
                                    page={filters.pageNumber}
                                    onChange={(e, page) => handleFilterChange('pageNumber', page)}
                                    color="primary"
                                    size={isMobile ? 'small' : 'medium'}
                                    showFirstButton={!isMobile}
                                    showLastButton={!isMobile}
                                />
                            </Box>
                        )}
                    </Box>
                </Box>
            </Box>

            <MobileFilterDrawer
                open={mobileDrawerOpen}
                categories={categories}
                categoriesLoading={categoriesLoading}
                filters={filters}
                expandedCategories={expandedCategories}
                priceRange={priceRange}
                onClose={() => setMobileDrawerOpen(false)}
                onFilterChange={handleFilterChange}
                onToggleCategoryExpansion={toggleCategoryExpansion}
                onPriceRangeChange={handlePriceRangeChange}
                onPriceRangeCommitted={handlePriceRangeCommitted}
            />
        </Container>
    );
}