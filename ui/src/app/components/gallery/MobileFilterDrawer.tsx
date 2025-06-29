import {
    Drawer,
    Box,
    Typography,
    IconButton,
    CircularProgress
} from '@mui/material';
import { Close as CloseIcon } from '@mui/icons-material';
import CategorySidebar from './CategorySidebar';
import { GalleryCategory, Filters } from '@/types';

interface MobileFilterDrawerProps {
    open: boolean;
    categories: GalleryCategory[];
    categoriesLoading: boolean;
    filters: Filters;
    expandedCategories: Set<string>;
    priceRange: number[];
    onClose: () => void;
    onFilterChange: <K extends keyof Filters>(key: K, value: Filters[K]) => void;
    onToggleCategoryExpansion: (categoryId: string) => void;
    onPriceRangeChange: (event: Event, newValue: number | number[]) => void;
    onPriceRangeCommitted: (event: Event | React.SyntheticEvent, newValue: number | number[]) => void;
}

export default function MobileFilterDrawer({
    open,
    categories,
    categoriesLoading,
    filters,
    expandedCategories,
    priceRange,
    onClose,
    onFilterChange,
    onToggleCategoryExpansion,
    onPriceRangeChange,
    onPriceRangeCommitted
}: MobileFilterDrawerProps) {
    return (
        <Drawer
            anchor="left"
            open={open}
            onClose={onClose}
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
                <IconButton onClick={onClose}>
                    <CloseIcon />
                </IconButton>
            </Box>
            {categoriesLoading ? (
                <Box sx={{ p: 3, textAlign: 'center' }}>
                    <CircularProgress size={40} />
                </Box>
            ) : (
                <CategorySidebar
                    categories={categories}
                    filters={filters}
                    expandedCategories={expandedCategories}
                    priceRange={priceRange}
                    onFilterChange={onFilterChange}
                    onToggleCategoryExpansion={onToggleCategoryExpansion}
                    onPriceRangeChange={onPriceRangeChange}
                    onPriceRangeCommitted={onPriceRangeCommitted}
                />
            )}
        </Drawer>
    );
}