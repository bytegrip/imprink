'use client';

import React, { useState, useEffect } from 'react';
import { useRouter, useParams } from 'next/navigation';
import { loadStripe } from '@stripe/stripe-js';
import { Elements } from '@stripe/react-stripe-js';
import clientApi from '@/lib/clientApi';
import {
  Container,
  Paper,
  Box,
  Button,
  CircularProgress,
} from '@mui/material';
import { Product, Variant, Address, NewAddress } from '@/types';
import StepperHeader from '@/app/components/orderbuilder/StepperHeader';
import AddAddressDialog from '@/app/components/orderbuilder/AddAddressDialog';
import StepProductDetails from '@/app/components/orderbuilder/StepProductDetails';
import StepSelectVariant from '@/app/components/orderbuilder/StepSelectVariant';
import StepCustomization from '@/app/components/orderbuilder/StepCustomization';
import StepChooseQuantity from '@/app/components/orderbuilder/StepChooseQuantity';
import StepDeliveryAddress from '@/app/components/orderbuilder/StepDeliveryAddress';
import StepReviewOrder from '@/app/components/orderbuilder/StepReviewOrder';
import StepPayment from '@/app/components/orderbuilder/StepPayment';

interface CustomizationImage {
  id: string;
  url: string;
  file: File;
}

const stripePromise = loadStripe(process.env.NEXT_PUBLIC_STRIPE_PUBLISHABLE_KEY!);

const steps = [
  'Product Details',
  'Select Variant',
  'Customization',
  'Choose Quantity',
  'Delivery Address',
  'Review & Order',
  'Payment'
];

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
  const [customizationImages, setCustomizationImages] = useState<CustomizationImage[]>([]);
  const [finalImageUrl, setFinalImageUrl] = useState('');
  const [customizationDescription, setCustomizationDescription] = useState('');
  const [orderId, setOrderId] = useState<string>('');
  const [clientSecret, setClientSecret] = useState<string>('');
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
    isActive: true,
  });

  useEffect(() => {
    if (productId) loadProduct();
  }, [productId]);

  const loadProduct = async () => {
    setLoading(true);
    try {
      const { data } = await clientApi.get(`/products/${productId}`);
      setProduct(data);
    } finally {
      setLoading(false);
    }
  };

  const loadVariants = async () => {
    setLoading(true);
    try {
      const { data } = await clientApi.get(`/products/variants/${productId}`);
      setVariants(data);
    } finally {
      setLoading(false);
    }
  };

  const loadAddresses = async () => {
    setLoading(true);
    try {
      console.log('Loading addresses...');
      const response = await clientApi.get('/addresses/me');
      console.log('API Response:', response);
      console.log('API Data:', response.data);
      
      const data = response.data;
      setAddresses(data);
      
      if (data.length > 0) {
        const defaultAddr = data.find((addr: Address) => addr.isDefault) || data[0];
        setSelectedAddress(defaultAddr);
        console.log('Selected address:', defaultAddr);
      } else {
        console.log('No addresses found');
      }
    } catch (error) {
      console.error('Error loading addresses:', error);
    } finally {
      setLoading(false);
    }
  };

  const handleNext = async () => {
  if (activeStep === 0 && product) {
    loadVariants();
  } else if (activeStep === 2 && product?.isCustomizable && customizationImages.length > 0 && !finalImageUrl) {
    return;
  } else if (activeStep === 3) {
    await loadAddresses();
  }
  setActiveStep((prev) => prev + 1);
};

  const handleBack = () => setActiveStep((prev) => prev - 1);

  const handleQuantityChange = (delta: number) => {
    setQuantity((prev) => Math.max(1, prev + delta));
  };

  const handleAddAddress = async () => {
    const added = await clientApi.post('/addresses', newAddress);
    setAddresses([...addresses, added.data]);
    setSelectedAddress(added.data);
    setShowAddressDialog(false);
    setNewAddress({
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
      isActive: true,
    });
  };

  const handlePlaceOrder = async () => {
    if (!selectedVariant || !selectedAddress) return;
    
    const orderData = {
      quantity,
      productId: product!.id,
      productVariantId: selectedVariant.id,
      originalImageUrls: customizationImages.map(img => img.url),
      customizationImageUrl: finalImageUrl,
      customizationDescription,
      addressId: selectedAddress.id,
    };
    
    setLoading(true);
    try {
      const orderResponse = await clientApi.post('/orders', orderData);
      const newOrderId = orderResponse.data.id;
      setOrderId(newOrderId);
      
      const paymentResponse = await clientApi.post('/stripe/create-payment-intent', {
        orderId: newOrderId
      });
      
      setClientSecret(paymentResponse.data.clientSecret);
      setActiveStep((prev) => prev + 1); 
    } catch (error) {
      console.error('Error creating order or payment intent:', error);
      alert('Failed to create order. Please try again.');
    } finally {
      setLoading(false);
    }
  };

  const handlePaymentSuccess = () => {
    router.push(`/orders/success?orderId=${orderId}`);
  };

  const getTotalPrice = () => (selectedVariant ? selectedVariant.price * quantity : 0);

  const canProceed = () => {
    switch (activeStep) {
      case 0: return product !== null;
      case 1: return selectedVariant !== null;
      case 2: 
        if (!product?.isCustomizable) return true;
        return customizationImages.length === 0 || finalImageUrl !== '';
      case 3: return quantity > 0;
      case 4: return selectedAddress !== null;
      case 5: return true; 
      case 6: return false; 
      default: return true;
    }
  };

  const shouldShowCustomization = () => {
    return product?.isCustomizable;
  };

  const renderStepContent = () => {
    switch (activeStep) {
      case 0:
        return <StepProductDetails product={product} />;
      case 1:
        return (
          <StepSelectVariant
            variants={variants}
            selectedVariant={selectedVariant}
            setSelectedVariant={setSelectedVariant}
          />
        );
      case 2:
        if (!shouldShowCustomization()) {
          setActiveStep(3);
          return null;
        }
        return (
          <StepCustomization
            images={customizationImages}
            setImages={setCustomizationImages}
            finalImageUrl={finalImageUrl}
            setFinalImageUrl={setFinalImageUrl}
            customizationDescription={customizationDescription}
            setCustomizationDescription={setCustomizationDescription}
            loading={loading}
            setLoading={setLoading}
          />
        );
      case 3:
        return (
          <StepChooseQuantity
            selectedVariant={selectedVariant}
            quantity={quantity}
            handleQuantityChange={handleQuantityChange}
            getTotalPrice={getTotalPrice}
            setQuantity={setQuantity}
          />
        );
      case 4:
        return (
          <StepDeliveryAddress
            addresses={addresses}
            selectedAddress={selectedAddress}
            setSelectedAddress={setSelectedAddress}
            setShowAddressDialog={setShowAddressDialog}
          />
        );
      case 5:
        return (
          <StepReviewOrder
            selectedVariant={selectedVariant}
            quantity={quantity}
            getTotalPrice={getTotalPrice}
            selectedAddress={selectedAddress}
            customizationImages={customizationImages}
            finalImageUrl={finalImageUrl}
            customizationDescription={customizationDescription}
          />
        );
      case 6:
        if (clientSecret) {
          return (
            <Elements 
              stripe={stripePromise} 
              options={{ 
                clientSecret,
                appearance: {
                  theme: 'stripe',
                  variables: {
                    colorPrimary: '#1976d2',
                    colorBackground: '#ffffff',
                    colorText: '#30313d',
                    colorDanger: '#df1b41',
                    fontFamily: 'Roboto, system-ui, sans-serif',
                    spacingUnit: '4px',
                    borderRadius: '8px',
                  },
                }
              }}
            >
              <StepPayment
                orderId={orderId}
                selectedVariant={selectedVariant}
                quantity={quantity}
                getTotalPrice={getTotalPrice}
                selectedAddress={selectedAddress}
                customizationImages={customizationImages}
                finalImageUrl={finalImageUrl}
                onSuccess={handlePaymentSuccess}
              />
            </Elements>
          );
        }
        return (
          <Box display="flex" justifyContent="center" alignItems="center" minHeight={200}>
            <CircularProgress />
          </Box>
        );
      default:
        return null;
    }
  };

  const getStepButtonText = () => {
    if (activeStep === steps.length - 2) return 'Place Order'; 
    if (activeStep === steps.length - 1) return 'Payment'; 
    return 'Next';
  };

  const handleStepAction = () => {
    if (activeStep === steps.length - 2) {
      handlePlaceOrder(); 
    } else if (activeStep === steps.length - 1) {
      return;
    } else {
      handleNext();
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
        <StepperHeader activeStep={activeStep} steps={steps} />
        <Box sx={{ mb: 4 }}>{renderStepContent()}</Box>
        
        {activeStep < steps.length - 1 && (
          <Box sx={{ display: 'flex', justifyContent: 'space-between' }}>
            <Button 
              color="inherit" 
              disabled={activeStep === 0} 
              onClick={handleBack}
            >
              Back
            </Button>
            <Button
              variant="contained"
              onClick={handleStepAction}
              disabled={!canProceed() || loading}
            >
              {loading ? <CircularProgress size={24} /> : getStepButtonText()}
            </Button>
          </Box>
        )}
      </Paper>

      <AddAddressDialog
        open={showAddressDialog}
        onClose={() => setShowAddressDialog(false)}
        onAdd={handleAddAddress}
        newAddress={newAddress}
        setNewAddress={setNewAddress}
      />
    </Container>
  );
}