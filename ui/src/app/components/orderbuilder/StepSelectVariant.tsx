'use client';

import { Box, Typography, Grid, Card, Fade, Stack, Chip } from '@mui/material';
import { Variant } from '@/types';

export default function StepSelectVariant({
  variants,
  selectedVariant,
  setSelectedVariant,
}: {
  variants: Variant[],
  selectedVariant: Variant | null,
  setSelectedVariant: (variant: Variant) => void
}) {
  return (
    <Fade in>
      <Box>
        <Typography variant="h5" gutterBottom sx={{ mb: 3, fontWeight: 600 }}>
          Select Variant
        </Typography>
        <Grid container spacing={2}>
          {variants.map((variant) => (
            <Grid key={variant.id} size={{ xs: 12, sm: 6, lg: 4 }}>
              <Card
                sx={{
                  cursor: 'pointer',
                  border: '2px solid',
                  borderColor: selectedVariant?.id === variant.id ? 'primary.main' : 'transparent',
                  bgcolor: selectedVariant?.id === variant.id ? 'primary.50' : 'background.paper',
                  borderRadius: 2,
                  transition: 'all 0.2s ease-in-out',
                  '&:hover': {
                    borderColor: selectedVariant?.id === variant.id ? 'primary.main' : 'primary.light',
                    bgcolor: selectedVariant?.id === variant.id ? 'primary.50' : 'primary.25',
                    transform: 'translateY(-2px)',
                    boxShadow: 3
                  },
                  p: 2
                }}
                onClick={() => setSelectedVariant(variant)}
              >
                <Stack direction="row" spacing={2} alignItems="center">
                  <Box
                    component="img"
                    src={variant.imageUrl}
                    alt={`${variant.size} - ${variant.color}`}
                    sx={{
                      width: 80,
                      height: 80,
                      objectFit: 'cover',
                      borderRadius: 1.5,
                      flexShrink: 0,
                      border: '1px solid',
                      borderColor: 'grey.200'
                    }}
                  />
                  
                  <Stack spacing={1} sx={{ minWidth: 0, flex: 1 }}>
                    <Typography 
                      variant="h6" 
                      sx={{ 
                        fontSize: '1.1rem',
                        fontWeight: 600,
                        color: selectedVariant?.id === variant.id ? 'primary.main' : 'text.primary'
                      }}
                    >
                      {variant.size} - {variant.color}
                    </Typography>
                    
                    <Typography 
                      variant="body2" 
                      color="text.secondary"
                      sx={{ fontSize: '0.85rem' }}
                    >
                      SKU: {variant.sku}
                    </Typography>
                    
                    <Stack direction="row" spacing={1} alignItems="center" flexWrap="wrap">
                      <Typography 
                        variant="h6" 
                        color="primary"
                        sx={{ 
                          fontSize: '1.2rem',
                          fontWeight: 700
                        }}
                      >
                        ${variant.price.toFixed(2)}
                      </Typography>
                      
                      <Chip
                        label={variant.stockQuantity > 0 ? `${variant.stockQuantity} in stock` : 'Out of stock'}
                        size="small"
                        color="primary" 
                        variant="filled"
                        sx={{ 
                          fontSize: '0.75rem',
                          height: 24
                        }}
                      />
                    </Stack>
                  </Stack>
                </Stack>
              </Card>
            </Grid>
          ))}
        </Grid>
      </Box>
    </Fade>
  );
}