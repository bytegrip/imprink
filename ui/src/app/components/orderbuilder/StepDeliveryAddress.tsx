'use client';

import {
  Box, Typography, FormControl, RadioGroup, FormControlLabel,
  Radio, Grid, Card, CardContent, Chip, Fade, Stack, Divider, Alert,
  Avatar, IconButton, Tooltip
} from '@mui/material';
import { 
  LocationOn as LocationIcon, 
  AddLocation as AddLocationIcon,
  Person as PersonIcon,
  Business as BusinessIcon,
  Phone as PhoneIcon
} from '@mui/icons-material';
import { Address } from '@/types';

export default function StepDeliveryAddress({
  addresses,
  selectedAddress,
  setSelectedAddress,
  setShowAddressDialog,
}: {
  addresses: Address[],
  selectedAddress: Address | null,
  setSelectedAddress: (addr: Address | null) => void,
  setShowAddressDialog: (open: boolean) => void,
}) {

  const formatAddress = (address: Address) => {
    const parts = [
      address.addressLine1,
      address.addressLine2,
      `${address.city}, ${address.state} ${address.postalCode}`,
      address.country
    ].filter(Boolean);
    return parts.join(' • ');
  };

  const getInitials = (firstName: string, lastName: string) => {
    return `${firstName.charAt(0)}${lastName.charAt(0)}`.toUpperCase();
  };

  return (
    <Fade in>
      <Box sx={{ p: { xs: 2, md: 3 } }}>
        <Typography variant="h5" gutterBottom sx={{ mb: 3, fontWeight: 600 }}>
          Select Delivery Address
        </Typography>

        {addresses.length === 0 && (
          <Alert severity="info" sx={{ mb: 3 }}>
            No addresses found. Please add a delivery address to continue.
          </Alert>
        )}

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
                <Grid key={address.id} size={{ xs: 12, sm: 6, lg: 4 }}>
                  <Card
                    sx={{
                      position: 'relative',
                      height: 180,
                      border: '2px solid',
                      borderColor: selectedAddress?.id === address.id ? 'primary.main' : 'grey.200',
                      bgcolor: selectedAddress?.id === address.id ? 'primary.50' : 'background.paper',
                      transition: 'all 0.2s ease-in-out',
                      '&:hover': {
                        borderColor: selectedAddress?.id === address.id ? 'primary.main' : 'primary.light',
                        transform: 'translateY(-2px)',
                        boxShadow: 2
                      },
                      cursor: 'pointer',
                      display: 'flex',
                      flexDirection: 'column'
                    }}
                    onClick={() => setSelectedAddress(address)}
                  >
                    <CardContent sx={{ 
                      p: 2, 
                      height: '100%', 
                      display: 'flex', 
                      flexDirection: 'column',
                      '&:last-child': { pb: 2 }
                    }}>
                      <FormControlLabel
                        value={address.id}
                        control={
                          <Radio 
                            sx={{ 
                              position: 'absolute', 
                              top: 8, 
                              right: 8,
                              p: 0
                            }} 
                          />
                        }
                        label=""
                        sx={{ m: 0 }}
                      />
                      
                      <Stack direction="row" alignItems="center" spacing={1.5} sx={{ mb: 1.5, pr: 4 }}>
                        <Avatar 
                          sx={{ 
                            width: 32, 
                            height: 32, 
                            bgcolor: 'primary.main',
                            fontSize: '0.875rem'
                          }}
                        >
                          {getInitials(address.firstName, address.lastName)}
                        </Avatar>
                        <Box sx={{ flex: 1, minWidth: 0 }}>
                          <Typography 
                            variant="subtitle2" 
                            sx={{ 
                              fontWeight: 600,
                              overflow: 'hidden',
                              textOverflow: 'ellipsis',
                              whiteSpace: 'nowrap'
                            }}
                          >
                            {address.firstName} {address.lastName}
                          </Typography>
                          {address.company && (
                            <Typography 
                              variant="caption" 
                              color="text.secondary"
                              sx={{ 
                                display: 'block',
                                overflow: 'hidden',
                                textOverflow: 'ellipsis',
                                whiteSpace: 'nowrap'
                              }}
                            >
                              {address.company}
                            </Typography>
                          )}
                        </Box>
                        {address.isDefault && (
                          <Chip 
                            label="Default" 
                            size="small" 
                            color="primary" 
                            variant="outlined"
                            sx={{ 
                              height: 20,
                              fontSize: '0.6875rem',
                              '& .MuiChip-label': { px: 1 }
                            }}
                          />
                        )}
                      </Stack>

                      <Stack direction="row" spacing={1} sx={{ mb: 1.5, flex: 1 }}>
                        <LocationIcon sx={{ fontSize: 16, color: 'text.secondary', mt: 0.25, flexShrink: 0 }} />
                        <Typography 
                          variant="body2" 
                          sx={{ 
                            fontSize: '0.8125rem',
                            lineHeight: 1.3,
                            color: 'text.secondary',
                            display: '-webkit-box',
                            WebkitLineClamp: 3,
                            WebkitBoxOrient: 'vertical',
                            overflow: 'hidden'
                          }}
                        >
                          {formatAddress(address)}
                        </Typography>
                      </Stack>

                      <Stack 
                        direction="row" 
                        alignItems="center" 
                        justifyContent="space-between"
                        sx={{ mt: 'auto' }}
                      >
                        {address.phoneNumber ? (
                          <Stack direction="row" alignItems="center" spacing={0.5}>
                            <PhoneIcon sx={{ fontSize: 14, color: 'text.secondary' }} />
                            <Typography variant="caption" color="text.secondary">
                              {address.phoneNumber}
                            </Typography>
                          </Stack>
                        ) : (
                          <Box /> 
                        )}
                      </Stack>
                    </CardContent>
                  </Card>
                </Grid>
              ))}
              
              <Grid size={{ xs: 12, sm: 6, lg: 4 }}>
                <Card
                  sx={{
                    cursor: 'pointer',
                    height: 180, 
                    border: '2px dashed',
                    borderColor: 'primary.main',
                    bgcolor: 'primary.25',
                    display: 'flex',
                    alignItems: 'center',
                    justifyContent: 'center',
                    transition: 'all 0.2s ease-in-out',
                    '&:hover': {
                      bgcolor: 'primary.50',
                      transform: 'translateY(-2px)',
                      boxShadow: 2
                    }
                  }}
                  onClick={() => setShowAddressDialog(true)}
                >
                  <CardContent sx={{ textAlign: 'center' }}>
                    <Stack alignItems="center" spacing={1.5}>
                      <AddLocationIcon sx={{ fontSize: 40, color: 'primary.main' }} />
                      <Typography variant="subtitle1" color="primary" sx={{ fontWeight: 600 }}>
                        Add New Address
                      </Typography>
                      <Typography variant="body2" color="text.secondary" sx={{ fontSize: '0.8125rem' }}>
                        Click to add delivery address
                      </Typography>
                    </Stack>
                  </CardContent>
                </Card>
              </Grid>
            </Grid>
          </RadioGroup>
        </FormControl>
      </Box>
    </Fade>
  );
}