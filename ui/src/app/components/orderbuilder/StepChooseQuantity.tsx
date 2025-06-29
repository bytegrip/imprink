'use client';

import { Box, Typography, IconButton, TextField, Fade, Stack, Divider } from '@mui/material';
import { Add as AddIcon, Remove as RemoveIcon } from '@mui/icons-material';
import { Variant } from '@/types';

export default function StepChooseQuantity({
  selectedVariant,
  quantity,
  handleQuantityChange,
  getTotalPrice,
  setQuantity,
}: {
  selectedVariant: Variant | null,
  quantity: number,
  handleQuantityChange: (delta: number) => void,
  getTotalPrice: () => number,
  setQuantity: (val: number) => void,
}) {
  if (!selectedVariant) return null;

  return (
    <Fade in>
      <Box sx={{ p: { xs: 2, md: 3 } }}>
        <Typography variant="h5" gutterBottom sx={{ mb: 4, fontWeight: 600 }}>
          Choose Quantity
        </Typography>
        
        <Stack spacing={4}>
          <Stack direction={{ xs: 'column', md: 'row' }} spacing={3} alignItems="flex-start">
            <Stack direction="row" spacing={2} alignItems="center" sx={{ flex: 1 }}>
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
              <Stack spacing={0.5}>
                <Typography variant="h6" sx={{ fontWeight: 600 }}>
                  {selectedVariant.product.name}
                </Typography>
                <Typography variant="body2" color="text.secondary">
                  {selectedVariant.size} - {selectedVariant.color}
                </Typography>
                <Typography variant="h6" color="primary" sx={{ fontWeight: 700 }}>
                  ${selectedVariant.price.toFixed(2)} each
                </Typography>
              </Stack>
            </Stack>

            <Stack spacing={2} alignItems="center" sx={{ minWidth: 200 }}>
              <Typography variant="body1" color="text.secondary" sx={{ fontWeight: 500 }}>
                Quantity
              </Typography>
              <Stack direction="row" alignItems="center" spacing={1}>
                <IconButton
                  onClick={() => handleQuantityChange(-1)}
                  disabled={quantity <= 1}
                  sx={{
                    border: '1px solid',
                    borderColor: 'grey.300',
                    '&:hover': { borderColor: 'primary.main' }
                  }}
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
                    style: { textAlign: 'center', fontSize: '1.2rem', fontWeight: 600 }, 
                    min: 1 
                  }}
                  sx={{ 
                    width: 80,
                    '& .MuiOutlinedInput-root': {
                      '& fieldset': {
                        borderColor: 'grey.300',
                      },
                      '&:hover fieldset': {
                        borderColor: 'primary.main',
                      }
                    }
                  }}
                />
                <IconButton
                  onClick={() => handleQuantityChange(1)}
                  sx={{
                    border: '1px solid',
                    borderColor: 'grey.300',
                    '&:hover': { borderColor: 'primary.main' }
                  }}
                >
                  <AddIcon />
                </IconButton>
              </Stack>
            </Stack>
          </Stack>

          <Divider />

          <Stack direction="row" justifyContent="space-between" alignItems="center">
            <Typography variant="h5" sx={{ fontWeight: 600 }}>
              Total Price:
            </Typography>
            <Typography variant="h3" color="primary" sx={{ fontWeight: 700 }}>
              ${getTotalPrice().toFixed(2)}
            </Typography>
          </Stack>
        </Stack>
      </Box>
    </Fade>
  );
}