import {
    Paper,
    Grid,
    IconButton,
    TextField,
    InputAdornment,
    FormControl,
    Select,
    MenuItem,
    InputLabel,
    Typography,
    useMediaQuery,
    useTheme,
    SelectChangeEvent,
    Box,
    Button
} from '@mui/material';
import {
    Search,
    Tune as TuneIcon
} from '@mui/icons-material';
import { useState, ChangeEvent, KeyboardEvent } from 'react';
import { Filters } from '@/types';
import { SORT_OPTIONS, PAGE_SIZE_OPTIONS } from '@/constants';

interface SearchFiltersProps {
    filters: Filters;
    totalCount: number;
    productsCount: number;
    onFilterChange: <K extends keyof Filters>(key: K, value: Filters[K]) => void;
    onOpenMobileDrawer: () => void;
}

export default function SearchFilters({
    filters,
    totalCount,
    productsCount,
    onFilterChange,
    onOpenMobileDrawer
}: SearchFiltersProps) {
    const theme = useTheme();
    const isMobile = useMediaQuery(theme.breakpoints.down('md'));
    const isSmall = useMediaQuery(theme.breakpoints.down('sm'));
    const [searchInput, setSearchInput] = useState<string>('');

    const handleSearch = (): void => {
        onFilterChange('searchTerm', searchInput);
    };

    const handleSortChange = (value: string): void => {
        const [sortBy, sortDirection] = value.split('-');
        onFilterChange('sortBy', sortBy);
        onFilterChange('sortDirection', sortDirection);
    };

    if (isMobile) {
        return (
            <Paper elevation={1} sx={{
                p: { xs: 1, sm: 1.25 },
                mb: { xs: 1, sm: 1.5 }
            }}>
                <Box sx={{ mb: 1 }}>
                    <TextField
                        fullWidth
                        placeholder="Search products..."
                        value={searchInput}
                        onChange={(e: ChangeEvent<HTMLInputElement>) => setSearchInput(e.target.value)}
                        onKeyPress={(e: KeyboardEvent) => e.key === 'Enter' && handleSearch()}
                        size="small"
                        sx={{
                            '& .MuiInputBase-input': {
                                fontSize: '0.85rem',
                                py: 1.25
                            }
                        }}
                        InputProps={{
                            startAdornment: (
                                <InputAdornment position="start">
                                    <Search />
                                </InputAdornment>
                            ),
                            endAdornment: searchInput && (
                                <InputAdornment position="end">
                                    <IconButton
                                        size="small"
                                        onClick={handleSearch}
                                        edge="end"
                                    >
                                        <Search />
                                    </IconButton>
                                </InputAdornment>
                            )
                        }}
                    />
                </Box>

                <Box sx={{ 
                    display: 'flex', 
                    alignItems: 'center',
                    gap: 1,
                    mb: 1
                }}>
                    <Button
                        variant="outlined"
                        onClick={onOpenMobileDrawer}
                        startIcon={<TuneIcon />}
                        size="small"
                        sx={{ 
                            fontSize: '0.8rem',
                            px: 1.5,
                            py: 0.75,
                            flexShrink: 0,
                            minWidth: 'fit-content'
                        }}
                    >
                        Filters
                    </Button>

                    <FormControl size="small" sx={{ flex: 1 }}>
                        <InputLabel sx={{ fontSize: '0.8rem' }}>
                            Sort
                        </InputLabel>
                        <Select
                            value={`${filters.sortBy}-${filters.sortDirection}`}
                            label="Sort"
                            onChange={(e: SelectChangeEvent) => handleSortChange(e.target.value)}
                            sx={{
                                '& .MuiSelect-select': {
                                    fontSize: '0.8rem',
                                    py: 0.75
                                }
                            }}
                        >
                            {SORT_OPTIONS.map((option) => (
                                <MenuItem 
                                    key={option.value} 
                                    value={option.value}
                                    sx={{ fontSize: '0.8rem' }}
                                >
                                    {option.label.split(' ')[0]}
                                </MenuItem>
                            ))}
                        </Select>
                    </FormControl>

                    <FormControl size="small" sx={{ flex: 1 }}>
                        <InputLabel sx={{ fontSize: '0.8rem' }}>
                            Per
                        </InputLabel>
                        <Select
                            value={filters.pageSize}
                            label="Per"
                            onChange={(e: SelectChangeEvent<number>) =>
                                onFilterChange('pageSize', e.target.value as number)
                            }
                            sx={{
                                '& .MuiSelect-select': {
                                    fontSize: '0.8rem',
                                    py: 0.75
                                }
                            }}
                        >
                            {PAGE_SIZE_OPTIONS.map((size) => (
                                <MenuItem 
                                    key={size} 
                                    value={size}
                                    sx={{ fontSize: '0.8rem' }}
                                >
                                    {size}
                                </MenuItem>
                            ))}
                        </Select>
                    </FormControl>
                </Box>

                <Box sx={{ textAlign: 'center' }}>
                    <Typography variant="body2" color="text.secondary" sx={{
                        fontSize: '0.75rem'
                    }}>
                        {productsCount} of {totalCount} products
                    </Typography>
                </Box>
            </Paper>
        );
    }

    return (
        <Paper elevation={1} sx={{
            p: { xs: 0.75, sm: 1, md: 1.25 },
            mb: { xs: 1, sm: 1.5 }
        }}>
            <Grid container spacing={{ xs: 0.75, sm: 1, md: 1.5 }} alignItems="center">
                <Grid size={{ xs: 12, sm: 6, md: 5 }}>
                    <TextField
                        fullWidth
                        placeholder="Search products..."
                        value={searchInput}
                        onChange={(e: ChangeEvent<HTMLInputElement>) => setSearchInput(e.target.value)}
                        onKeyPress={(e: KeyboardEvent) => e.key === 'Enter' && handleSearch()}
                        size="small"
                        sx={{
                            '& .MuiInputBase-input': {
                                fontSize: { xs: '0.75rem', sm: '0.85rem' },
                                py: { xs: 0.75, sm: 1 }
                            }
                        }}
                        InputProps={{
                            startAdornment: (
                                <InputAdornment position="start">
                                    <Search fontSize="small" />
                                </InputAdornment>
                            ),
                            endAdornment: searchInput && (
                                <InputAdornment position="end">
                                    <IconButton
                                        size="small"
                                        onClick={handleSearch}
                                        edge="end"
                                        sx={{ p: 0.5 }}
                                    >
                                        <Search fontSize="small" />
                                    </IconButton>
                                </InputAdornment>
                            )
                        }}
                    />
                </Grid>

                <Grid size={{ xs: 6, sm: 3, md: 3 }}>
                    <FormControl fullWidth size="small">
                        <InputLabel sx={{ fontSize: { xs: '0.75rem', sm: '0.85rem' } }}>
                            Sort
                        </InputLabel>
                        <Select
                            value={`${filters.sortBy}-${filters.sortDirection}`}
                            label="Sort"
                            onChange={(e: SelectChangeEvent) => handleSortChange(e.target.value)}
                            sx={{
                                '& .MuiSelect-select': {
                                    fontSize: { xs: '0.75rem', sm: '0.85rem' },
                                    py: { xs: 0.75, sm: 1 }
                                }
                            }}
                        >
                            {SORT_OPTIONS.map((option) => (
                                <MenuItem 
                                    key={option.value} 
                                    value={option.value}
                                    sx={{ fontSize: { xs: '0.75rem', sm: '0.85rem' } }}
                                >
                                    {isSmall ? option.label.split(' ')[0] : option.label}
                                </MenuItem>
                            ))}
                        </Select>
                    </FormControl>
                </Grid>

                <Grid size={{ xs: 6, sm: 3, md: 2 }}>
                    <FormControl fullWidth size="small">
                        <InputLabel sx={{ fontSize: { xs: '0.75rem', sm: '0.85rem' } }}>
                            {isSmall ? 'Per' : 'Per Page'}
                        </InputLabel>
                        <Select
                            value={filters.pageSize}
                            label={isSmall ? 'Per' : 'Per Page'}
                            onChange={(e: SelectChangeEvent<number>) =>
                                onFilterChange('pageSize', e.target.value as number)
                            }
                            sx={{
                                '& .MuiSelect-select': {
                                    fontSize: { xs: '0.75rem', sm: '0.85rem' },
                                    py: { xs: 0.75, sm: 1 }
                                }
                            }}
                        >
                            {PAGE_SIZE_OPTIONS.map((size) => (
                                <MenuItem 
                                    key={size} 
                                    value={size}
                                    sx={{ fontSize: { xs: '0.75rem', sm: '0.85rem' } }}
                                >
                                    {size}
                                </MenuItem>
                            ))}
                        </Select>
                    </FormControl>
                </Grid>

                <Grid size={{ xs: 12, md: 2 }} sx={{ 
                    textAlign: { xs: 'center', md: 'right' },
                    mt: { xs: 0.5, md: 0 }
                }}>
                    <Typography variant="body2" color="text.secondary" sx={{
                        fontSize: { xs: '0.65rem', sm: '0.75rem' },
                        lineHeight: 1.2
                    }}>
                        {isSmall ? (
                            `${productsCount}/${totalCount}`
                        ) : (
                            `Showing ${productsCount} of ${totalCount}`
                        )}
                    </Typography>
                </Grid>
            </Grid>
        </Paper>
    );
}