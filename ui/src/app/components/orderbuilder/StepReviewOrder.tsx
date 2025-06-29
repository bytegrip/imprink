'use client';

import { Box, Typography, Grid, Card, CardContent, Fade, Stack, Divider } from '@mui/material';
import { Variant, Address } from '@/types';

interface CustomizationImage {
  id: string;
  url: string;
  file: File;
}

export default function StepReviewOrder({
  selectedVariant,
  quantity,
  getTotalPrice,
  selectedAddress,
  customizationImages = [],
  finalImageUrl = '',
  customizationDescription = ''
}: {
  selectedVariant: Variant | null,
  quantity: number,
  getTotalPrice: () => number,
  selectedAddress: Address | null,
  customizationImages?: CustomizationImage[],
  finalImageUrl?: string,
  customizationDescription?: string
}) {
  if (!selectedVariant || !selectedAddress) return null;

  return (
    <Fade in>
      <Box sx={{ p: { xs: 2, md: 3 } }}>
        <Typography variant="h5" gutterBottom sx={{ mb: 4, fontWeight: 600 }}>
          Review Your Order
        </Typography>
        
        <Grid container spacing={4}>
          <Grid size={{ xs: 12, md: 8 }}>
            <Stack spacing={4}>
              <Box>
                <Typography variant="h6" gutterBottom sx={{ fontWeight: 600, mb: 3 }}>
                  Order Summary
                </Typography>
                
                <Stack direction="row" spacing={2} alignItems="center" sx={{ mb: 3 }}>
                  <Box
                    component="img"
                    src={selectedVariant.imageUrl}
                    alt={selectedVariant.size}
                    sx={{
                      width: 80,
                      height: 80,
                      objectFit: 'cover',
                      borderRadius: 2,
                      border: '1px solid',
                      borderColor: 'grey.200'
                    }}
                  />
                  <Stack sx={{ flex: 1 }}>
                    <Typography variant="h6" sx={{ fontWeight: 600 }}>
                      {selectedVariant.product.name}
                    </Typography>
                    <Typography variant="body2" color="text.secondary">
                      {selectedVariant.size} - {selectedVariant.color}
                    </Typography>
                    <Typography variant="body2" color="text.secondary">
                      Quantity: {quantity}
                    </Typography>
                  </Stack>
                  <Typography variant="h5" color="primary" sx={{ fontWeight: 700 }}>
                    ${getTotalPrice().toFixed(2)}
                  </Typography>
                </Stack>

                {customizationImages.length > 0 && (
                  <>
                    <Divider sx={{ my: 3 }} />
                    <Box>
                      <Typography variant="h6" gutterBottom sx={{ fontWeight: 600 }}>
                        Customization Details
                      </Typography>
                      <Typography variant="body2" color="text.secondary" sx={{ mb: 2 }}>
                        {customizationImages.length} original image{customizationImages.length > 1 ? 's' : ''} uploaded
                      </Typography>
                      {customizationDescription && (
                        <Typography variant="body1" sx={{ mb: 2, lineHeight: 1.6 }}>
                          {customizationDescription}
                        </Typography>
                      )}
                    </Box>
                  </>
                )}
              </Box>
            </Stack>
          </Grid>

          <Grid size={{ xs: 12, md: 4 }}>
            <Stack spacing={3}>
              {finalImageUrl && (
                <Card sx={{ borderRadius: 2, overflow: 'hidden' }}>
                  <CardContent sx={{ p: 2 }}>
                    <Typography variant="h6" gutterBottom sx={{ fontWeight: 600, mb: 2 }}>
                      Preview
                    </Typography>
                    <Box
                      component="img"
                      src={finalImageUrl}
                      alt="Final customization"
                      sx={{
                        width: '100%',
                        height: 'auto',
                        objectFit: 'contain',
                        borderRadius: 1,
                        border: '1px solid',
                        borderColor: 'grey.200'
                      }}
                    />
                  </CardContent>
                </Card>
              )}

              <Card sx={{ borderRadius: 2 }}>
                <CardContent sx={{ p: 3 }}>
                  <Typography variant="h6" gutterBottom sx={{ fontWeight: 600, mb: 2 }}>
                    Delivery Address
                  </Typography>
                  <Stack spacing={0.5}>
                    <Typography variant="subtitle1" sx={{ fontWeight: 600 }}>
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
                  </Stack>
                </CardContent>
              </Card>
            </Stack>
          </Grid>
        </Grid>
      </Box>
    </Fade>
  );
}