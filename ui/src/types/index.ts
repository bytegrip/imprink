import {JSX} from 'react';

export interface Category {
  id: string;
  name: string;
  description: string;
  imageUrl: string;
  sortOrder: number;
  isActive: boolean;
  parentCategoryId: string;
  createdAt: string;
  modifiedAt: string;
}

export interface Product {
  id: string;
  name: string;
  description: string;
  basePrice: number;
  isCustomizable: boolean;
  isActive: boolean;
  imageUrl: string;
  categoryId: string;
  category: Category;
  createdAt: string;
  modifiedAt: string;
}

export interface Variant {
  id: string;
  productId: string;
  size: string;
  color: string;
  price: number;
  imageUrl: string;
  sku: string;
  stockQuantity: number;
  isActive: boolean;
  product: Product;
  createdAt: string;
  modifiedAt: string;
}

export interface Address {
  id: string;
  userId: string;
  addressType: string;
  firstName: string;
  lastName: string;
  company: string;
  addressLine1: string;
  addressLine2: string;
  apartmentNumber: string;
  buildingNumber: string;
  floor: string;
  city: string;
  state: string;
  postalCode: string;
  country: string;
  phoneNumber: string;
  instructions: string;
  isDefault: boolean;
  isActive: boolean;
}

export interface NewAddress {
  addressType: string;
  firstName: string;
  lastName: string;
  company: string;
  addressLine1: string;
  addressLine2: string;
  apartmentNumber: string;
  buildingNumber: string;
  floor: string;
  city: string;
  state: string;
  postalCode: string;
  country: string;
  phoneNumber: string;
  instructions: string;
  isDefault: boolean;
  isActive: boolean;
}

export interface SortOption {
    value: string;
    label: string;
}

export interface GalleryCategory {
    id: string;
    name: string;
    parentCategoryId?: string;
}

export interface GalleryProduct {
    id: string;
    name: string;
    description: string;
    basePrice: number;
    imageUrl?: string;
    isCustomizable: boolean;
    category?: Category;
}

export interface ProductsResponse {
    items: Product[];
    totalPages: number;
    totalCount: number;
}

export interface Filters {
    pageNumber: number;
    pageSize: number;
    searchTerm: string;
    categoryId: string;
    minPrice: number;
    maxPrice: number;
    isActive: boolean;
    isCustomizable: boolean | null;
    sortBy: string;
    sortDirection: string;
}

export interface ApiParams {
    PageNumber: number;
    PageSize: number;
    IsActive: boolean;
    SortBy: string;
    SortDirection: string;
    SearchTerm?: string;
    CategoryId?: string;
    MinPrice?: number;
    MaxPrice?: number;
    IsCustomizable?: boolean;
}