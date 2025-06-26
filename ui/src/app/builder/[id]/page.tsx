'use client';

import React, { useState, useEffect } from 'react';
import { useRouter, useParams } from 'next/navigation';
import clientApi from '@/lib/clientApi';
import {
  Box,
  Button,
  Card,
  CardContent,
  CardMedia,
  Typography,
  Stepper,
  Step,
  StepLabel,
  Grid,
  TextField,
  IconButton,
  Radio,
  RadioGroup,
  FormControlLabel,
  FormControl,
  FormLabel,
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  Chip,
  Container,
  Paper,
  Fade,
  CircularProgress
} from '@mui/material';
import {
  Add as AddIcon,
  Remove as RemoveIcon,
  LocationOn as LocationIcon,
  AddLocation as AddLocationIcon
} from '@mui/icons-material';

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

interface Variant {
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

interface Address {
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

interface NewAddress {
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

const steps = ['Product Details', 'Select Variant', 'Choose Quantity', 'Delivery Address', 'Review & Order'];

export default function OrderBuilder() {
  const router = useRouter();
  const params = useParams();
  const productId = params.id as string;

  const [activeStep, setActiveStep] = useState(0);
  const [loading, setLoading] = useState(false);
  const [product, setProduct] = useState<Product | null>(null);
  const [variants, setVariants] = useState<Variant[]>([]);
  const [addresses, setAddresses] = useState<Address[]>([]);
  const [selectedVariant, setSelectedVariant] = useState<Variant | null>(null);
  const [quantity, setQuantity] = useState(1);
  const [selectedAddress, setSelectedAddress] = useState<Address | null>(null);
  const [showAddressDialog, setShowAddressDialog] = useState(false);
  const [newAddress, setNewAddress] = useState<NewAddress>({
    addressType: 'Home',
    firstName: '',
    lastName: '',
    company: '',
    addressLine1: '',
    addressLine2: '',
    apartmentNumber: '',
    buildingNumber: '',
    floor: '',
    city: '',
    state: '',
    postalCode: '',
    country: '',
    phoneNumber: '',
    instructions: '',
    isDefault: false,
    isActive: true
  });

  useEffect(() => {
    if (productId) {
      loadProduct();
    }
  }, [productId]);

  const loadProduct = async () => {
    setLoading(true);
    try {
      const productData = await clientApi.get(`/products/${productId}`);
      setProduct(productData.data);
    } catch (error) {
      console.error('Failed to load product:', error);
    } finally {
      setLoading(false);
    }
  };

  const loadVariants = async () => {
    setLoading(true);
    try {
      const variantsData = await clientApi.get(`/products/variants/${productId}`);
      setVariants(variantsData.data);
    } catch (error) {
      console.error('Failed to load variants:', error);
    } finally {
      setLoading(false);
    }
  };

  const loadAddresses = async () => {
    setLoading(true);
    try {
      const addressesData = await clientApi.get('/addresses/me');
      setAddresses(addressesData.data);
      if (addressesData.data.length > 0) {
        const defaultAddress = addressesData.data.find((addr: Address) => addr.isDefault) || addressesData.data[0];
        setSelectedAddress(defaultAddress);
      }
    } catch (error) {
      console.error('Failed to load addresses:', error);
    } finally {
      setLoading(false);
    }
  };

  const handleNext = () => {
    if (activeStep === 0 && product) {
      loadVariants();
    } else if (activeStep === 2) {
      loadAddresses();
    }
    setActiveStep((prevActiveStep) => prevActiveStep + 1);
  };

  const handleBack = () => {
    setActiveStep((prevActiveStep) => prevActiveStep - 1);
  };

  const handleQuantityChange = (delta: number) => {
    const newQuantity = quantity + delta;
    if (newQuantity >= 1) {
      setQuantity(newQuantity);
    }
  };

  const handleAddAddress = async () => {
    try {
      const addedAddress = await clientApi.post('/addresses', newAddress);
      setAddresses([...addresses, addedAddress.data]);
      setSelectedAddress(addedAddress.data);
      setShowAddressDialog(false);
      setNewAddress({
        addressType: 'shipping',
        firstName: '',
        lastName: '',
        company: '',
        addressLine1: '',
        addressLine2: '',
        apartmentNumber: '',
        buildingNumber: '',
        floor: '',
        city: '',
        state: '',
        postalCode: '',
        country: '',
        phoneNumber: '',
        instructions: '',
        isDefault: false,
        isActive: true
      });
    } catch (error) {
      console.error('Failed to add address:', error);
    }
  };

  const handlePlaceOrder = async () => {
    if (!selectedVariant || !selectedAddress) return;

    const orderData = {
      productId: product!.id,
      productVariantId: selectedVariant.id,
      quantity: quantity,
      addressId: selectedAddress.id,
      totalPrice: selectedVariant.price * quantity
    };

    try {
      setLoading(true);
      await clientApi.post('/orders', orderData);
      router.push('/orders/success');
    } catch (error) {
      console.error('Failed to place order:', error);
    } finally {
      setLoading(false);
    }
  };

  const getTotalPrice = () => {
    if (!selectedVariant) return 0;
    return selectedVariant.price * quantity;
  };

  const canProceed = () => {
    switch (activeStep) {
      case 0: return product !== null;
      case 1: return selectedVariant !== null;
      case 2: return quantity > 0;
      case 3: return selectedAddress !== null;
      default: return true;
    }
  };

  const renderStepContent = () => {
    switch (activeStep) {
      case 0:
        return (
          <Fade in={true}>
            <Box>
              {product && (
                <Card>
                  <CardMedia
                    component="img"
                    height="400"
                    image={product.imageUrl}
                    alt={product.name}
                  />
                  <CardContent>
                    <Typography variant="h4" gutterBottom>
                      {product.name}
                    </Typography>
                    <Typography variant="body1" color="text.secondary" paragraph>
                      {product.description}
                    </Typography>
                    <Typography variant="h5" color="primary">
                      ${product.basePrice.toFixed(2)}
                    </Typography>
                    <Box mt={2}>
                      <Chip label={product.category.name} color="primary" variant="outlined" />
                      {product.isCustomizable && <Chip label="Customizable" color="secondary" sx={{ ml: 1 }} />}
                    </Box>
                  </CardContent>
                </Card>
              )}
            </Box>
          </Fade>
        );

      case 1:
        return (
          <Fade in={true}>
            <Box>
              <Typography variant="h5" gutterBottom>
                Select Variant
              </Typography>
              <Grid container spacing={3}>
                {variants.map((variant) => (
                  <Grid size={{ xs:12, sm:6, md:4 }} key={variant.id}>
                    <Card 
                      sx={{ 
                        cursor: 'pointer',
                        border: selectedVariant?.id === variant.id ? 2 : 1,
                        borderColor: selectedVariant?.id === variant.id ? 'primary.main' : 'grey.300'
                      }}
                      onClick={() => setSelectedVariant(variant)}
                    >
                      <CardMedia
                        component="img"
                        height="200"
                        image={variant.imageUrl}
                        alt={`${variant.size} - ${variant.color}`}
                      />
                      <CardContent>
                        <Typography variant="h6">
                          {variant.size} - {variant.color}
                        </Typography>
                        <Typography variant="body2" color="text.secondary">
                          SKU: {variant.sku}
                        </Typography>
                        <Typography variant="h6" color="primary">
                          ${variant.price.toFixed(2)}
                        </Typography>
                        <Typography variant="body2" color={variant.stockQuantity > 0 ? 'success.main' : 'error.main'}>
                          {variant.stockQuantity > 0 ? `${variant.stockQuantity} in stock` : 'Out of stock'}
                        </Typography>
                      </CardContent>
                    </Card>
                  </Grid>
                ))}
              </Grid>
            </Box>
          </Fade>
        );

      case 2:
        return (
          <Fade in={true}>
            <Box>
              <Typography variant="h5" gutterBottom>
                Choose Quantity
              </Typography>
              {selectedVariant && (
                <Card>
                  <CardContent>
                    <Grid container spacing={3} alignItems="center">
                      <Grid size={{ xs:12, md:6 }}>
                        <Box display="flex" alignItems="center" gap={2}>
                          <img 
                            src={selectedVariant.imageUrl} 
                            alt={selectedVariant.size}
                            style={{ width: 80, height: 80, objectFit: 'cover', borderRadius: 8 }}
                          />
                          <Box>
                            <Typography variant="h6">
                              {selectedVariant.product.name}
                            </Typography>
                            <Typography variant="body2" color="text.secondary">
                              {selectedVariant.size} - {selectedVariant.color}
                            </Typography>
                            <Typography variant="h6" color="primary">
                              ${selectedVariant.price.toFixed(2)} each
                            </Typography>
                          </Box>
                        </Box>
                      </Grid>
                      <Grid size={{ xs:12, md:6 }}>
                        <Box display="flex" alignItems="center" justifyContent="center" gap={2}>
                          <IconButton 
                            onClick={() => handleQuantityChange(-1)}
                            disabled={quantity <= 1}
                          >
                            <RemoveIcon />
                          </IconButton>
                          <TextField
                            value={quantity}
                            onChange={(e) => {
                              const val = parseInt(e.target.value) || 1;
                              if (val >= 1) setQuantity(val);
                            }}
                            inputProps={{ 
                              style: { textAlign: 'center', fontSize: '1.2rem' },
                              min: 1
                            }}
                            sx={{ width: 80 }}
                          />
                          <IconButton onClick={() => handleQuantityChange(1)}>
                            <AddIcon />
                          </IconButton>
                        </Box>
                      </Grid>
                    </Grid>
                    <Box mt={3} textAlign="center">
                      <Typography variant="h4" color="primary">
                        Total: ${getTotalPrice().toFixed(2)}
                      </Typography>
                    </Box>
                  </CardContent>
                </Card>
              )}
            </Box>
          </Fade>
        );

      case 3:
        return (
          <Fade in={true}>
            <Box>
              <Typography variant="h5" gutterBottom>
                Select Delivery Address
              </Typography>
              <FormControl component="fieldset" sx={{ width: '100%' }}>
                <RadioGroup
                  value={selectedAddress?.id || ''}
                  onChange={(e) => {
                    const addr = addresses.find(a => a.id === e.target.value);
                    setSelectedAddress(addr || null);
                  }}
                >
                  <Grid container spacing={2}>
                    {addresses.map((address) => (
                      <Grid size={{ xs:12, md:6 }} key={address.id}>
                        <Card sx={{ position: 'relative' }}>
                          <CardContent>
                            <FormControlLabel
                              value={address.id}
                              control={<Radio />}
                              label=""
                              sx={{ position: 'absolute', top: 8, right: 8 }}
                            />
                            <Box display="flex" alignItems="flex-start" gap={1} mb={1}>
                              <LocationIcon color="primary" />
                              <Typography variant="h6">
                                {address.firstName} {address.lastName}
                              </Typography>
                              {address.isDefault && <Chip label="Default" size="small" color="primary" />}
                            </Box>
                            {address.company && (
                              <Typography variant="body2" color="text.secondary">
                                {address.company}
                              </Typography>
                            )}
                            <Typography variant="body2">
                              {address.addressLine1}
                            </Typography>
                            {address.addressLine2 && (
                              <Typography variant="body2">
                                {address.addressLine2}
                              </Typography>
                            )}
                            <Typography variant="body2">
                              {address.city}, {address.state} {address.postalCode}
                            </Typography>
                            <Typography variant="body2">
                              {address.country}
                            </Typography>
                            {address.phoneNumber && (
                              <Typography variant="body2" color="text.secondary">
                                Phone: {address.phoneNumber}
                              </Typography>
                            )}
                          </CardContent>
                        </Card>
                      </Grid>
                    ))}
                    <Grid size={{ xs:12, md:6 }}>
                      <Card 
                        sx={{ 
                          cursor: 'pointer',
                          border: '2px dashed',
                          borderColor: 'primary.main',
                          display: 'flex',
                          alignItems: 'center',
                          justifyContent: 'center',
                          minHeight: 200
                        }}
                        onClick={() => setShowAddressDialog(true)}
                      >
                        <CardContent>
                          <Box textAlign="center">
                            <AddLocationIcon sx={{ fontSize: 48, color: 'primary.main', mb: 1 }} />
                            <Typography variant="h6" color="primary">
                              Add New Address
                            </Typography>
                          </Box>
                        </CardContent>
                      </Card>
                    </Grid>
                  </Grid>
                </RadioGroup>
              </FormControl>
            </Box>
          </Fade>
        );

      case 4:
        return (
          <Fade in={true}>
            <Box>
              <Typography variant="h5" gutterBottom>
                Review Your Order
              </Typography>
              <Grid container spacing={3}>
                <Grid size={{ xs:12, md:8 }}>
                  <Card>
                    <CardContent>
                      <Typography variant="h6" gutterBottom>
                        Order Summary
                      </Typography>
                      {selectedVariant && (
                        <Box display="flex" alignItems="center" gap={2} mb={2}>
                          <img 
                            src={selectedVariant.imageUrl} 
                            alt={selectedVariant.size}
                            style={{ width: 60, height: 60, objectFit: 'cover', borderRadius: 8 }}
                          />
                          <Box flex={1}>
                            <Typography variant="subtitle1">
                              {selectedVariant.product.name}
                            </Typography>
                            <Typography variant="body2" color="text.secondary">
                              {selectedVariant.size} - {selectedVariant.color}
                            </Typography>
                            <Typography variant="body2">
                              Quantity: {quantity}
                            </Typography>
                          </Box>
                          <Typography variant="h6">
                            ${getTotalPrice().toFixed(2)}
                          </Typography>
                        </Box>
                      )}
                    </CardContent>
                  </Card>
                </Grid>
                <Grid size={{ xs:12, md:4 }}>
                  <Card>
                    <CardContent>
                      <Typography variant="h6" gutterBottom>
                        Delivery Address
                      </Typography>
                      {selectedAddress && (
                        <Box>
                          <Typography variant="subtitle2">
                            {selectedAddress.firstName} {selectedAddress.lastName}
                          </Typography>
                          {selectedAddress.company && (
                            <Typography variant="body2" color="text.secondary">
                              {selectedAddress.company}
                            </Typography>
                          )}
                          <Typography variant="body2">
                            {selectedAddress.addressLine1}
                          </Typography>
                          {selectedAddress.addressLine2 && (
                            <Typography variant="body2">
                              {selectedAddress.addressLine2}
                            </Typography>
                          )}
                          <Typography variant="body2">
                            {selectedAddress.city}, {selectedAddress.state} {selectedAddress.postalCode}
                          </Typography>
                          <Typography variant="body2">
                            {selectedAddress.country}
                          </Typography>
                        </Box>
                      )}
                    </CardContent>
                  </Card>
                </Grid>
              </Grid>
            </Box>
          </Fade>
        );

      default:
        return null;
    }
  };

  if (loading && !product) {
    return (
      <Container maxWidth="lg" sx={{ mt: 4, display: 'flex', justifyContent: 'center' }}>
        <CircularProgress />
      </Container>
    );
  }

  return (
    <Container maxWidth="lg" sx={{ mt: 4, mb: 4 }}>
      <Paper sx={{ p: 3 }}>
        <Stepper activeStep={activeStep} sx={{ mb: 4 }}>
          {steps.map((label) => (
            <Step key={label}>
              <StepLabel>{label}</StepLabel>
            </Step>
          ))}
        </Stepper>

        <Box sx={{ mb: 4 }}>
          {renderStepContent()}
        </Box>

        <Box sx={{ display: 'flex', justifyContent: 'space-between' }}>
          <Button
            color="inherit"
            disabled={activeStep === 0}
            onClick={handleBack}
            sx={{ mr: 1 }}
          >
            Back
          </Button>
          <Button
            variant="contained"
            onClick={activeStep === steps.length - 1 ? handlePlaceOrder : handleNext}
            disabled={!canProceed() || loading}
          >
            {loading ? (
              <CircularProgress size={24} />
            ) : activeStep === steps.length - 1 ? (
              'Place Order'
            ) : (
              'Next'
            )}
          </Button>
        </Box>
      </Paper>

      <Dialog open={showAddressDialog} onClose={() => setShowAddressDialog(false)} maxWidth="md" fullWidth>
        <DialogTitle>Add New Address</DialogTitle>
        <DialogContent>
          <Grid container spacing={2} sx={{ mt: 1 }}>
            <Grid size={{ xs:12, sm:6 }}>
              <TextField
                fullWidth
                label="First Name"
                value={newAddress.firstName}
                onChange={(e) => setNewAddress({...newAddress, firstName: e.target.value})}
              />
            </Grid>
            <Grid size={{ xs:12, sm:6 }}>
              <TextField
                fullWidth
                label="Last Name"
                value={newAddress.lastName}
                onChange={(e) => setNewAddress({...newAddress, lastName: e.target.value})}
              />
            </Grid>
            <Grid size={{ xs:12, sm:6 }}>
              <TextField
                fullWidth
                label="Company (Optional)"
                value={newAddress.company}
                onChange={(e) => setNewAddress({...newAddress, company: e.target.value})}
              />
            </Grid>
            <Grid size={{ xs:12, sm:6 }}>
              <TextField
                fullWidth
                label="Address Line 1"
                value={newAddress.addressLine1}
                onChange={(e) => setNewAddress({...newAddress, addressLine1: e.target.value})}
              />
            </Grid>
            <Grid size={{ xs:12, sm:6 }}>
              <TextField
                fullWidth
                label="Address Line 2 (Optional)"
                value={newAddress.addressLine2}
                onChange={(e) => setNewAddress({...newAddress, addressLine2: e.target.value})}
              />
            </Grid>
            <Grid size={{ xs:12, sm:6 }}>
              <TextField
                fullWidth
                label="Apartment #"
                value={newAddress.apartmentNumber}
                onChange={(e) => setNewAddress({...newAddress, apartmentNumber: e.target.value})}
              />
            </Grid>
            <Grid size={{ xs:12, sm:6 }}>
              <TextField
                fullWidth
                label="Building #"
                value={newAddress.buildingNumber}
                onChange={(e) => setNewAddress({...newAddress, buildingNumber: e.target.value})}
              />
            </Grid>
            <Grid size={{ xs:12, sm:6 }}>
              <TextField
                fullWidth
                label="Floor"
                value={newAddress.floor}
                onChange={(e) => setNewAddress({...newAddress, floor: e.target.value})}
              />
            </Grid>
            <Grid size={{ xs:12, sm:6 }}>
              <TextField
                fullWidth
                label="City"
                value={newAddress.city}
                onChange={(e) => setNewAddress({...newAddress, city: e.target.value})}
              />
            </Grid>
            <Grid size={{ xs:12, sm:6 }}>
              <TextField
                fullWidth
                label="State"
                value={newAddress.state}
                onChange={(e) => setNewAddress({...newAddress, state: e.target.value})}
              />
            </Grid>
            <Grid size={{ xs:12, sm:6 }}>
              <TextField
                fullWidth
                label="Postal Code"
                value={newAddress.postalCode}
                onChange={(e) => setNewAddress({...newAddress, postalCode: e.target.value})}
              />
            </Grid>
            <Grid size={{ xs:12, sm:6 }}>
              <TextField
                fullWidth
                label="Country"
                value={newAddress.country}
                onChange={(e) => setNewAddress({...newAddress, country: e.target.value})}
              />
            </Grid>
            <Grid size={{ xs:12 }}>
              <TextField
                fullWidth
                label="Phone Number"
                value={newAddress.phoneNumber}
                onChange={(e) => setNewAddress({...newAddress, phoneNumber: e.target.value})}
              />
            </Grid>
            <Grid size={{ xs:12 }}>
              <TextField
                fullWidth
                multiline
                rows={3}
                label="Delivery Instructions (Optional)"
                value={newAddress.instructions}
                onChange={(e) => setNewAddress({...newAddress, instructions: e.target.value})}
              />
            </Grid>
          </Grid>
        </DialogContent>
        <DialogActions>
          <Button onClick={() => setShowAddressDialog(false)}>Cancel</Button>
          <Button 
            onClick={handleAddAddress} 
            variant="contained"
            disabled={!newAddress.firstName || !newAddress.lastName || !newAddress.addressLine1 || !newAddress.city}
          >
            Add Address
          </Button>
        </DialogActions>
      </Dialog>
    </Container>
  );
}