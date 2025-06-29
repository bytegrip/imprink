import { SortOption } from '@/types';

export const SORT_OPTIONS: SortOption[] = [
    { value: 'Name-ASC', label: 'Name (A-Z)' },
    { value: 'Name-DESC', label: 'Name (Z-A)' },
    { value: 'Price-ASC', label: 'Price (Low to High)' },
    { value: 'Price-DESC', label: 'Price (High to Low)' },
    { value: 'CreatedDate-DESC', label: 'Newest First' },
    { value: 'CreatedDate-ASC', label: 'Oldest First' }
];

export const PAGE_SIZE_OPTIONS: number[] = [12, 24, 48, 96];