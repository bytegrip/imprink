'use client';

import React, { useState } from 'react';
import { lightTheme } from '../theme/lightTheme';
import {
  Box,
  Typography,
  Paper,
  Button,
  CircularProgress,
  Alert,
  Fade,
  Stack,
  Divider,
  Card,
  CardContent,
  useTheme,
  ThemeProvider,
  createTheme
} from '@mui/material';
import {
  CheckCircle as CheckCircleIcon,
  Payment as PaymentIcon,
  Receipt as ReceiptIcon
} from '@mui/icons-material';
import {
  useStripe,
  useElements,
  PaymentElement,
  AddressElement,
} from '@stripe/react-stripe-js';
import { Variant, Address } from '@/types';

interface CustomizationImage {
  id: string;
  url: string;
  file: File;
}

export default function StepPayment({
  orderId,
  selectedVariant,
  quantity,
  getTotalPrice,
  selectedAddress,
  customizationImages = [],
  finalImageUrl = '',
  onSuccess
}: {
  orderId: string;
  selectedVariant: Variant | null;
  quantity: number;
  getTotalPrice: () => number;
  selectedAddress: Address | null;
  customizationImages?: CustomizationImage[];
  finalImageUrl?: string;
  onSuccess: () => void;
}) {
  const stripe = useStripe();
  const elements = useElements();
  const theme = useTheme();
  const [isLoading, setIsLoading] = useState(false);
  const [message, setMessage] = useState('');
  const [isSuccess, setIsSuccess] = useState(false);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    
    if (!stripe || !elements) {
      return;
    }

    setIsLoading(true);
    setMessage('');

    const { error } = await stripe.confirmPayment({
      elements,
      confirmParams: {
        return_url: `${window.location.origin}/payment-success`,
      },
      redirect: 'if_required',
    });

    if (error) {
      if (error.type === 'card_error' || error.type === 'validation_error') {
        setMessage(error.message || 'An error occurred');
      } else {
        setMessage('An unexpected error occurred.');
      }
      setIsLoading(false);
    } else {
      setMessage('Payment successful! 🎉');
      setIsSuccess(true);
      setTimeout(() => {
        onSuccess();
      }, 2000);
    }
  };

  // Create Stripe appearance object with light theme forced for payment sections
  const stripeAppearance = {
    theme: 'stripe' as const, // Always use light theme
    variables: {
      colorPrimary: '#1976d2', // Use a consistent primary color
      colorBackground: '#ffffff', // Force white background
      colorText: '#212121', // Force dark text
      colorDanger: '#d32f2f', // Red for errors
      fontFamily: '"Roboto","Helvetica","Arial",sans-serif',
      spacingUnit: '4px',
      borderRadius: '4px',
    },
    rules: {
      '.Input': {
        backgroundColor: '#ffffff',
        border: '1px solid #e0e0e0',
        borderRadius: '4px',
        color: '#212121',
        fontSize: '14px',
        padding: '12px',
      },
      '.Input:focus': {
        borderColor: '#1976d2',
        boxShadow: '0 0 0 1px #1976d2',
      },
      '.Label': {
        color: '#424242',
        fontSize: '12px',
        fontWeight: '500',
      },
      '.Tab': {
        backgroundColor: '#ffffff',
        border: '1px solid #e0e0e0',
        color: '#757575',
      },
      '.Tab:hover': {
        backgroundColor: '#f5f5f5',
      },
      '.Tab--selected': {
        backgroundColor: '#1976d2',
        color: '#ffffff',
        borderColor: '#1976d2',
      },
    }
  };

  const paymentElementOptions = {
    layout: 'tabs' as const,
    appearance: stripeAppearance,
  };

  const addressElementOptions = {
    mode: 'billing' as const,
    // Remove allowedCountries to accept all countries
    fields: {
      phone: 'always' as const,
    },
    validation: {
      phone: {
        required: 'never' as const,
      },
    },
    appearance: stripeAppearance,
  };

  if (isSuccess) {
    return (
      <Fade in>
        <Box sx={{ p: { xs: 2, md: 3 }, textAlign: 'center' }}>
          <Stack spacing={4} alignItems="center" sx={{ maxWidth: 600, mx: 'auto' }}>
            <CheckCircleIcon 
              sx={{ 
                fontSize: 80, 
                color: 'success.main',
                filter: 'drop-shadow(0 4px 8px rgba(76, 175, 80, 0.3))'
              }} 
            />
            
            <Typography 
              variant="h4" 
              sx={{ 
                fontWeight: 600, 
                color: 'success.main',
                fontSize: { xs: '1.75rem', md: '2.125rem' }
              }}
            >
              Payment Successful!
            </Typography>
            
            <Typography 
              variant="h6" 
              color="text.secondary"
              sx={{ fontSize: { xs: '1rem', md: '1.25rem' } }}
            >
              Thank you for your purchase!
            </Typography>
            
            <Card sx={{ width: '100%', bgcolor: 'success.50', border: '1px solid', borderColor: 'success.200' }}>
              <CardContent sx={{ p: 3 }}>
                <Stack spacing={2}>
                  <Stack direction="row" alignItems="center" spacing={1}>
                    <ReceiptIcon color="success" />
                    <Typography variant="h6" sx={{ fontWeight: 600 }}>
                      Order Details
                    </Typography>
                  </Stack>
                  <Typography variant="body1">
                    <strong>Order ID:</strong> {orderId}
                  </Typography>
                  <Typography variant="body2" color="text.secondary">
                    You will receive a confirmation email shortly with your order details and tracking information.
                  </Typography>
                </Stack>
              </CardContent>
            </Card>
          </Stack>
        </Box>
      </Fade>
    );
  }

  if (!selectedVariant || !selectedAddress) return null;

  return (
    <Fade in>
      <Box sx={{ p: { xs: 2, md: 3 } }}>
        <Typography variant="h5" gutterBottom sx={{ mb: 4, fontWeight: 600 }}>
          Complete Your Payment
        </Typography>

        <Box component="form" onSubmit={handleSubmit}>
          <Stack spacing={4}>
            <Card sx={{ borderRadius: 2 }}>
              <CardContent sx={{ p: 3 }}>
                <Typography variant="h6" gutterBottom sx={{ fontWeight: 600, mb: 2 }}>
                  Order Summary
                </Typography>
                
                <Stack direction="row" spacing={2} alignItems="center" sx={{ mb: 2 }}>
                  <Box
                    component="img"
                    src={selectedVariant.imageUrl}
                    alt={selectedVariant.size}
                    sx={{
                      width: 60,
                      height: 60,
                      objectFit: 'cover',
                      borderRadius: 1.5,
                      border: '1px solid',
                      borderColor: 'grey.200'
                    }}
                  />
                  <Stack sx={{ flex: 1 }}>
                    <Typography variant="subtitle1" sx={{ fontWeight: 600 }}>
                      {selectedVariant.product.name}
                    </Typography>
                    <Typography variant="body2" color="text.secondary">
                      {selectedVariant.size} - {selectedVariant.color} × {quantity}
                    </Typography>
                  </Stack>
                  <Typography variant="h6" color="primary" sx={{ fontWeight: 700 }}>
                    ${getTotalPrice().toFixed(2)}
                  </Typography>
                </Stack>

                <Divider />
                
                <Stack direction="row" justifyContent="space-between" alignItems="center" sx={{ mt: 2 }}>
                  <Typography variant="h6" sx={{ fontWeight: 600 }}>
                    Total Amount:
                  </Typography>
                  <Typography variant="h5" color="primary" sx={{ fontWeight: 700 }}>
                    ${getTotalPrice().toFixed(2)}
                  </Typography>
                </Stack>
              </CardContent>
            </Card>

            {/* Billing Address - Always Light Theme */}
            <ThemeProvider theme={lightTheme}>
              <Paper sx={{ p: 3, borderRadius: 2 }}>
                <Stack direction="row" alignItems="center" spacing={1} sx={{ mb: 3 }}>
                  <PaymentIcon color="primary" />
                  <Typography variant="h6" sx={{ fontWeight: 600 }}>
                    Billing Information
                  </Typography>
                </Stack>
                
                <Box>
                  <AddressElement options={addressElementOptions} />
                </Box>
              </Paper>
            </ThemeProvider>

            {/* Payment Information - Always Light Theme */}
            <ThemeProvider theme={lightTheme}>
              <Paper sx={{ p: 3, borderRadius: 2 }}>
                <Typography variant="h6" gutterBottom sx={{ fontWeight: 600, mb: 3 }}>
                  Payment Information
                </Typography>
                
                <Box>
                  <PaymentElement options={paymentElementOptions} />
                </Box>
              </Paper>
            </ThemeProvider>

            <ThemeProvider theme={lightTheme}>
              <Box sx={{ display: 'flex', justifyContent: 'center', pt: 2 }}>
                <Button
                  type="submit"
                  variant="contained"
                  size="large"
                  disabled={!stripe || !elements || isLoading}
                  sx={{
                    minWidth: 200,
                    height: 48,
                    fontSize: '1.1rem',
                    fontWeight: 600,
                    borderRadius: 2
                  }}
                >
                  {isLoading ? (
                    <Stack 
                        direction="row" 
                        alignItems="center" 
                        spacing={1}
                        sx={{ color: 'white' }}
                    >
                        <CircularProgress size={20} color="inherit" />
                        <span>Processing...</span>
                    </Stack>
                    ) : (
                    `Pay ${getTotalPrice().toFixed(2)}$`
                    )}
                </Button>
              </Box>
            </ThemeProvider>

            {message && (
              <Alert 
                severity={isSuccess ? "success" : "error"} 
                sx={{ borderRadius: 2 }}
              >
                {message}
              </Alert>
            )}
          </Stack>
        </Box>
      </Box>
    </Fade>
  );
}