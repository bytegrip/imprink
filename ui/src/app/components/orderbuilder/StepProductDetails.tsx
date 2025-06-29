'use client';

import { 
  Box, 
  Typography, 
  Chip, 
  Fade,
  Grid,
  Stack
} from '@mui/material';
import { Product } from '@/types';

export default function StepProductDetails({ product }: { product: Product | null }) {
  if (!product) return null;

  return (
    <Fade in>
      <Box sx={{ p: { xs: 2, sm: 3, md: 4 } }}>
        <Grid container spacing={{ xs: 3, md: 4 }} alignItems="flex-start">
          <Grid size={{ xs: 12, md: 5 }}>
            <Box
              sx={{
                display: 'flex',
                justifyContent: 'center',
                alignItems: 'center'
              }}
            >
              <Box
                component="img"
                src={product.imageUrl}
                alt={product.name}
                sx={{
                  maxWidth: '100%',
                  maxHeight: { xs: 250, sm: 300, md: 350 },
                  objectFit: 'contain',
                  borderRadius: 2,
                  boxShadow: 2,
                  transition: 'transform 0.3s ease-in-out, box-shadow 0.3s ease-in-out',
                  '&:hover': {
                    boxShadow: 4
                  }
                }}
              />
            </Box>
          </Grid>

          <Grid size={{ xs: 12, md: 7 }}>
            <Stack spacing={{ xs: 2, md: 3 }}>
              <Typography 
                variant="h3" 
                component="h1"
                sx={{ 
                  fontWeight: 600,
                  lineHeight: 1.2,
                  color: 'text.primary',
                  fontSize: { xs: '1.75rem', sm: '2.125rem', md: '2.5rem' }
                }}
              >
                {product.name}
              </Typography>

              <Stack direction="row" spacing={1} flexWrap="wrap" useFlexGap>
                <Chip 
                  label={product.category.name} 
                  color="primary" 
                  variant="filled"
                  size="medium"
                  sx={{ 
                    fontWeight: 500,
                    fontSize: { xs: '0.8rem', md: '0.875rem' }
                  }}
                />
                {product.isCustomizable && (
                  <Chip 
                    label="Customizable" 
                    color="primary" 
                    variant="filled"
                    size="medium"
                    sx={{ 
                      fontWeight: 500,
                      fontSize: { xs: '0.8rem', md: '0.875rem' },
                    }}
                  />
                )}
              </Stack>

              <Typography 
                variant="h2" 
                color="primary"
                sx={{ 
                  fontWeight: 700,
                  fontSize: { xs: '2.5rem', sm: '3rem', md: '3.5rem' },
                  lineHeight: 1.1
                }}
              >
                ${product.basePrice.toFixed(2)}
              </Typography>

              <Typography 
                variant="body1" 
                color="text.secondary"
                sx={{ 
                  lineHeight: 1.6,
                  fontSize: { xs: '1rem', md: '1.125rem' },
                  maxWidth: { md: '90%' }
                }}
              >
                {product.description}
              </Typography>
            </Stack>
          </Grid>
        </Grid>
      </Box>
    </Fade>
  );
}