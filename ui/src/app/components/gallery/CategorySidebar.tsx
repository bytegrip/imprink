import {
    Box,
    Typography,
    List,
    ListItem,
    ListItemButton,
    ListItemText,
    Collapse,
    IconButton,
    Slider,
    Switch,
    FormControlLabel
} from '@mui/material';
import { ExpandLess, ExpandMore } from '@mui/icons-material';
import { ChangeEvent } from 'react';
import { GalleryCategory, Filters } from '@/types';

interface CategorySidebarProps {
    categories: GalleryCategory[];
    filters: Filters;
    expandedCategories: Set<string>;
    priceRange: number[];
    onFilterChange: <K extends keyof Filters>(key: K, value: Filters[K]) => void;
    onToggleCategoryExpansion: (categoryId: string) => void;
    onPriceRangeChange: (event: Event, newValue: number | number[]) => void;
    onPriceRangeCommitted: (event: Event | React.SyntheticEvent, newValue: number | number[]) => void;
}

export default function CategorySidebar({
    categories,
    filters,
    expandedCategories,
    priceRange,
    onFilterChange,
    onToggleCategoryExpansion,
    onPriceRangeChange,
    onPriceRangeCommitted
}: CategorySidebarProps) {
    const getChildCategories = (parentId: string): GalleryCategory[] => {
        return categories.filter(cat => cat.parentCategoryId === parentId);
    };

    const getParentCategories = (): GalleryCategory[] => {
        return categories.filter(cat => !cat.parentCategoryId);
    };

    return (
        <Box sx={{
            height: '100%',
            display: 'flex',
            flexDirection: 'column'
        }}>
            <Typography variant="h6" gutterBottom sx={{
                fontWeight: 'bold',
                mb: 2,
                p: 2,
                pb: 1,
                flexShrink: 0
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
                            onClick={() => onFilterChange('categoryId', '')}
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
                                        onClick={() => onToggleCategoryExpansion(category.id)}
                                        sx={{ borderRadius: 1, mb: 0.5 }}
                                    >
                                        <ListItemText primary={category.name} />
                                        {hasChildren && (
                                            <IconButton
                                                size="small"
                                                onClick={(e) => {
                                                    e.stopPropagation();
                                                    onToggleCategoryExpansion(category.id);
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
                                                        onClick={() => onFilterChange('categoryId', childCategory.id)}
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
                        onChange={onPriceRangeChange}
                        onChangeCommitted={onPriceRangeCommitted}
                        valueLabelDisplay="auto"
                        min={0}
                        max={100}
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
                            onChange={(e: ChangeEvent<HTMLInputElement>) =>
                                onFilterChange('isCustomizable', e.target.checked ? true : null)
                            }
                        />
                    }
                    label="Customizable Only"
                    sx={{ mb: 0 }}
                />
            </Box>
        </Box>
    );
}