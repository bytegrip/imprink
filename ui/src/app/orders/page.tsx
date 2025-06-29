'use client';

import React, { useState, useEffect } from 'react';
import {
  Box,
  Container,
  Typography,
  Card,
  CardContent,
  Grid,
  Chip,
  Avatar,
  Skeleton,
  Alert,
  Paper,
  Divider,
  Dialog,
  DialogContent,
  IconButton,
  Fade,
  Collapse,
  Button,
} from '@mui/material';
import {
  ShoppingBag,
  LocalShipping,
  Close,
  ZoomIn,
  ExpandMore,
  ExpandLess,
} from '@mui/icons-material';
import clientApi from '@/lib/clientApi';

interface OrderStatus {
  id: number;
  name: string;
  description: string;
}

interface ShippingStatus {
  id: number;
  name: string;
  description: string;
}

interface Category {
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

interface Product {
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

interface ProductVariant {
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

interface OrderAddress {
  id: string;
  orderId: string;
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
  createdAt: string;
  updatedAt: string;
}

interface Order {
  id: string;
  userId: string;
  orderDate: string;
  amount: number;
  quantity: number;
  productId: string;
  productVariantId: string;
  orderStatusId: number;
  shippingStatusId: number;
  notes: string;
  merchantId: string;
  customizationImageUrl: string;
  originalImageUrls: string[];
  customizationDescription: string;
  createdAt: string;
  updatedAt: string;
  orderStatus: OrderStatus;
  shippingStatus: ShippingStatus;
  orderAddress: OrderAddress;
  product: Product;
  productVariant: ProductVariant;
}

const getStatusColor = (statusName: string) => {
  const normalizedStatus = statusName.toLowerCase();
  if (normalizedStatus.includes('pending') || normalizedStatus.includes('processing')) {
    return 'warning';
  }
  if (normalizedStatus.includes('completed') || normalizedStatus.includes('delivered')) {
    return 'success';
  }
  if (normalizedStatus.includes('cancelled') || normalizedStatus.includes('failed')) {
    return 'error';
  }
  return 'default';
};

const formatCurrency = (amount: number) => {
  return new Intl.NumberFormat('en-US', {
    style: 'currency',
    currency: 'USD',
  }).format(amount);
};

const formatDate = (dateString: string) => {
  return new Date(dateString).toLocaleDateString('en-US', {
    year: 'numeric',
    month: 'long',
    day: 'numeric',
    hour: '2-digit',
    minute: '2-digit',
  });
};

export default function OrdersPage() {
  const [orders, setOrders] = useState<Order[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [selectedImage, setSelectedImage] = useState<string | null>(null);
  const [expandedOrders, setExpandedOrders] = useState<Set<string>>(new Set());

  useEffect(() => {
    const fetchOrders = async () => {
      try {
        setLoading(true);
        const response = await clientApi.get('/orders/user/me?includeDetails=true');
        console.log("Data", response.data);
        setOrders(response.data);
      } catch (err: any) {
        setError(err.response?.data?.message || err.message || 'Failed to fetch orders');
      } finally {
        setLoading(false);
      }
    };

    fetchOrders();
  }, []);

  const handleImageClick = (imageUrl: string) => {
    setSelectedImage(imageUrl);
  };

  const handleCloseImage = () => {
    setSelectedImage(null);
  };

  const toggleOrderExpansion = (orderId: string) => {
    const newExpanded = new Set(expandedOrders);
    if (newExpanded.has(orderId)) {
      newExpanded.delete(orderId);
    } else {
      newExpanded.add(orderId);
    }
    setExpandedOrders(newExpanded);
  };

  if (loading) {
    return (
      <Container maxWidth="lg" sx={{ py: 4 }}>
        <Typography variant="h4" component="h1" gutterBottom>
          My Orders
        </Typography>
        <Grid container spacing={3}>
          {[...Array(3)].map((_, index) => (
            <Grid size={{ xs:12 }} key={index}>
              <Card>
                <CardContent>
                  <Grid container spacing={2}>
                    <Grid size={{ xs:12, md:3 }}>
                      <Skeleton variant="rectangular" height={200} />
                    </Grid>
                    <Grid size={{ xs:12, md:9 }}>
                      <Skeleton variant="text" height={32} />
                      <Skeleton variant="text" height={24} />
                      <Skeleton variant="text" height={24} />
                      <Box sx={{ mt: 2 }}>
                        <Skeleton variant="rectangular" height={32} width={100} />
                      </Box>
                    </Grid>
                  </Grid>
                </CardContent>
              </Card>
            </Grid>
          ))}
        </Grid>
      </Container>
    );
  }

  if (error) {
    return (
      <Container maxWidth="lg" sx={{ py: 4 }}>
        <Alert severity="error" sx={{ mb: 3 }}>
          {error}
        </Alert>
      </Container>
    );
  }

  if (orders.length === 0) {
    return (
      <Container maxWidth="lg" sx={{ py: 4 }}>
        <Typography variant="h4" component="h1" gutterBottom>
          My Orders
        </Typography>
        <Paper sx={{ p: 4, textAlign: 'center' }}>
          <ShoppingBag sx={{ fontSize: 64, color: 'text.secondary', mb: 2 }} />
          <Typography variant="h6" color="text.secondary">
            No orders found
          </Typography>
          <Typography color="text.secondary">
            Your orders will appear here once you make a purchase.
          </Typography>
        </Paper>
      </Container>
    );
  }

  return (
    <Container maxWidth="lg" sx={{ py: 4 }}>
      <Typography variant="h4" component="h1" gutterBottom sx={{ mb: 4 }}>
        My Orders
      </Typography>

      <Grid container spacing={3}>
        {orders.map((order) => {
          const isExpanded = expandedOrders.has(order.id);
          
          return (
            <Grid size={{ xs:12 }} key={order.id}>
              <Card elevation={2}>
                <CardContent 
                  sx={{ p: 2, cursor: 'pointer' }}
                  onClick={() => toggleOrderExpansion(order.id)}
                >
                  <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
                    <Box sx={{ display: 'flex', alignItems: 'center', gap: 2, flex: 1 }}>
                      <Box>
                        <Typography variant="subtitle1" component="h3" sx={{ fontWeight: 600 }}>
                          Order #{order.id.slice(-8).toUpperCase()}
                        </Typography>
                        <Typography variant="body2" color="text.secondary">
                          {formatDate(order.orderDate)}
                        </Typography>
                      </Box>
                      
                      <Box sx={{ display: 'flex', gap: 1 }}>
                        <Chip
                          icon={<ShoppingBag />}
                          label={order.orderStatus.name}
                          color={getStatusColor(order.orderStatus.name) as any}
                          size="small"
                        />
                        <Chip
                          icon={<LocalShipping />}
                          label={order.shippingStatus.name}
                          color={getStatusColor(order.shippingStatus.name) as any}
                          size="small"
                          variant="outlined"
                        />
                      </Box>
                    </Box>

                    <Box sx={{ display: 'flex', alignItems: 'center', gap: 2 }}>
                      <Box sx={{ textAlign: 'right' }}>
                        <Typography variant="h6" color="primary">
                          {formatCurrency(order.amount)}
                        </Typography>
                        <Typography variant="body2" color="text.secondary">
                          Qty: {order.quantity}
                        </Typography>
                      </Box>
                      
                      <IconButton size="small">
                        {isExpanded ? <ExpandLess /> : <ExpandMore />}
                      </IconButton>
                    </Box>
                  </Box>

                  <Collapse in={isExpanded}>
                    <Box sx={{ px: 4, pb: 4, mt:2 }} >
                      <Divider sx={{ mb: 3 }} />
                    
                      <Grid container spacing={3}>
                      <Grid size={{ xs:12, md:8 }}>
                        <Box sx={{ display: 'flex', gap: 2 }}>
                          <Avatar
                            src={order.product.imageUrl}
                            alt={order.product.name}
                            sx={{ width: 80, height: 80 }}
                            variant="rounded"
                          />
                          <Box sx={{ flex: 1 }}>
                            <Typography variant="h6">{order.product.name}</Typography>
                            <Typography variant="body2" color="text.secondary" sx={{ mb: 1 }}>
                              {order.product.description}
                            </Typography>
                            <Typography variant="body2">
                              <strong>Variant:</strong> {order.productVariant.size} - {order.productVariant.color}
                            </Typography>
                            <Typography variant="body2">
                              <strong>SKU:</strong> {order.productVariant.sku}
                            </Typography>
                            {order.customizationDescription && (
                              <Typography variant="body2" sx={{ mt: 1 }}>
                                <strong>Customization:</strong> {order.customizationDescription}
                              </Typography>
                            )}
                          </Box>
                        </Box>
                      </Grid>

                      <Grid size={{ xs:12, md:4 }}>
                        <Typography variant="subtitle2" gutterBottom>
                          Shipping Address
                        </Typography>
                        <Typography variant="body2" color="text.secondary">
                          {order.orderAddress.firstName} {order.orderAddress.lastName}
                          <br />
                          {order.orderAddress.company && (
                            <>
                              {order.orderAddress.company}
                              <br />
                            </>
                          )}
                          {order.orderAddress.addressLine1}
                          <br />
                          {order.orderAddress.addressLine2 && (
                            <>
                              {order.orderAddress.addressLine2}
                              <br />
                            </>
                          )}
                          {order.orderAddress.city}, {order.orderAddress.state} {order.orderAddress.postalCode}
                          <br />
                          {order.orderAddress.country}
                        </Typography>
                      </Grid>

                      {(order.productVariant.imageUrl || order.customizationImageUrl || order.originalImageUrls.length > 0) && (
                        <Grid size={{ xs:12 }}>
                          <Divider sx={{ my: 2 }} />
                          <Typography variant="subtitle2" gutterBottom>
                            Images
                          </Typography>
                          
                          <Box sx={{ display: 'flex', gap: 2, flexWrap: 'wrap' }}>
                            {order.productVariant.imageUrl && (
                              <Box>
                                <Typography variant="caption" display="block" gutterBottom>
                                  Product Variant
                                </Typography>
                                <Paper
                                  elevation={1}
                                  sx={{
                                    p: 1,
                                    cursor: 'pointer',
                                    position: 'relative',
                                  }}
                                  onClick={() => handleImageClick(order.productVariant.imageUrl)}
                                >
                                  <Box
                                    component="img"
                                    src={order.productVariant.imageUrl}
                                    alt="Product variant"
                                    sx={{
                                      width: 100,
                                      height: 100,
                                      objectFit: 'cover',
                                      borderRadius: 1,
                                    }}
                                  />
                                  <IconButton
                                    size="small"
                                    sx={{
                                      position: 'absolute',
                                      top: 4,
                                      right: 4,
                                      bgcolor: 'rgba(0,0,0,0.5)',
                                      color: 'white',
                                      '&:hover': { bgcolor: 'rgba(0,0,0,0.7)' },
                                    }}
                                  >
                                    <ZoomIn fontSize="small" />
                                  </IconButton>
                                </Paper>
                              </Box>
                            )}

                            {order.customizationImageUrl && (
                              <Box>
                                <Typography variant="caption" display="block" gutterBottom>
                                  Customized
                                </Typography>
                                <Paper
                                  elevation={1}
                                  sx={{
                                    p: 1,
                                    cursor: 'pointer',
                                    position: 'relative',
                                  }}
                                  onClick={() => handleImageClick(order.customizationImageUrl)}
                                >
                                  <Box
                                    component="img"
                                    src={order.customizationImageUrl}
                                    alt="Customization"
                                    sx={{
                                      width: 100,
                                      height: 100,
                                      objectFit: 'cover',
                                      borderRadius: 1,
                                    }}
                                  />
                                  <IconButton
                                    size="small"
                                    sx={{
                                      position: 'absolute',
                                      top: 4,
                                      right: 4,
                                      bgcolor: 'rgba(0,0,0,0.5)',
                                      color: 'white',
                                      '&:hover': { bgcolor: 'rgba(0,0,0,0.7)' },
                                    }}
                                  >
                                    <ZoomIn fontSize="small" />
                                  </IconButton>
                                </Paper>
                              </Box>
                            )}

                            {order.originalImageUrls.length > 0 && (
                              <Box>
                                <Typography variant="caption" display="block" gutterBottom>
                                  Original Images ({order.originalImageUrls.length})
                                </Typography>
                                <Box sx={{ display: 'flex', gap: 1, flexWrap: 'wrap' }}>
                                  {order.originalImageUrls.slice(0, 3).map((imageUrl, index) => (
                                    <Paper
                                      key={index}
                                      elevation={1}
                                      sx={{
                                        p: 1,
                                        cursor: 'pointer',
                                        position: 'relative',
                                      }}
                                      onClick={() => handleImageClick(imageUrl)}
                                    >
                                      <Box
                                        component="img"
                                        src={imageUrl}
                                        alt={`Original ${index + 1}`}
                                        sx={{
                                          width: 60,
                                          height: 60,
                                          objectFit: 'cover',
                                          borderRadius: 1,
                                        }}
                                      />
                                      {index === 2 && order.originalImageUrls.length > 3 && (
                                        <Box
                                          sx={{
                                            position: 'absolute',
                                            top: 0,
                                            left: 0,
                                            right: 0,
                                            bottom: 0,
                                            bgcolor: 'rgba(0,0,0,0.7)',
                                            color: 'white',
                                            display: 'flex',
                                            alignItems: 'center',
                                            justifyContent: 'center',
                                            borderRadius: 1,
                                          }}
                                        >
                                          +{order.originalImageUrls.length - 3}
                                        </Box>
                                      )}
                                      <IconButton
                                        size="small"
                                        sx={{
                                          position: 'absolute',
                                          top: 2,
                                          right: 2,
                                          bgcolor: 'rgba(0,0,0,0.5)',
                                          color: 'white',
                                          '&:hover': { bgcolor: 'rgba(0,0,0,0.7)' },
                                        }}
                                      >
                                        <ZoomIn sx={{ fontSize: 12 }} />
                                      </IconButton>
                                    </Paper>
                                  ))}
                                </Box>
                              </Box>
                            )}
                          </Box>
                        </Grid>
                      )}

                      {order.notes && (
                        <Grid size={{ xs:12 }}>
                          <Divider sx={{ my: 2 }} />
                          <Typography variant="subtitle2" gutterBottom>
                            Notes
                          </Typography>
                          <Typography variant="body2" color="text.secondary">
                            {order.notes}
                          </Typography>
                        </Grid>
                      )}
                    </Grid>
                  </Box>
                  </Collapse>
                </CardContent>
              </Card>
            </Grid>
          );
        })}
      </Grid>

      <Dialog
        open={!!selectedImage}
        onClose={handleCloseImage}
        maxWidth="md"
        fullWidth
        PaperProps={{
          sx: { bgcolor: 'transparent', boxShadow: 'none' }
        }}
      >
        <DialogContent sx={{ p: 0, position: 'relative' }}>
          <IconButton
            onClick={handleCloseImage}
            sx={{
              position: 'absolute',
              top: 8,
              right: 8,
              bgcolor: 'rgba(0,0,0,0.5)',
              color: 'white',
              zIndex: 1,
              '&:hover': { bgcolor: 'rgba(0,0,0,0.7)' },
            }}
          >
            <Close />
          </IconButton>
          {selectedImage && (
            <Fade in={!!selectedImage}>
              <Box
                component="img"
                src={selectedImage}
                alt="Full size preview"
                sx={{
                  width: '100%',
                  height: 'auto',
                  maxHeight: '90vh',
                  objectFit: 'contain',
                  borderRadius: 1,
                }}
              />
            </Fade>
          )}
        </DialogContent>
      </Dialog>
    </Container>
  );
}