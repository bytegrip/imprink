'use client';

import {
  Dialog, DialogTitle, DialogContent, DialogActions,
  Button, Grid, TextField
} from '@mui/material';

import { NewAddress } from '@/types'; // Or inline if needed

export default function AddAddressDialog({
  open,
  onClose,
  onAdd,
  newAddress,
  setNewAddress,
}: {
  open: boolean,
  onClose: () => void,
  onAdd: () => void,
  newAddress: NewAddress,
  setNewAddress: (address: NewAddress) => void,
}) {
  return (
    <Dialog open={open} onClose={onClose} maxWidth="md" fullWidth>
      <DialogTitle>Add New Address</DialogTitle>
      <DialogContent>
        <Grid container spacing={2} sx={{ mt: 1 }}>
          {[
            { label: 'First Name', field: 'firstName' },
            { label: 'Last Name', field: 'lastName' },
            { label: 'Company (Optional)', field: 'company' },
            { label: 'Address Line 1', field: 'addressLine1' },
            { label: 'Address Line 2 (Optional)', field: 'addressLine2' },
            { label: 'Apartment #', field: 'apartmentNumber' },
            { label: 'Building #', field: 'buildingNumber' },
            { label: 'Floor', field: 'floor' },
            { label: 'City', field: 'city' },
            { label: 'State', field: 'state' },
            { label: 'Postal Code', field: 'postalCode' },
            { label: 'Country', field: 'country' },
            { label: 'Phone Number', field: 'phoneNumber' },
          ].map(({ label, field }, index) => (
            <Grid size={{ xs:12 }} key={index}>
              <TextField
                fullWidth
                label={label}
                value={(newAddress as any)[field]}
                onChange={(e) => setNewAddress({ ...newAddress, [field]: e.target.value })}
              />
            </Grid>
          ))}
          <Grid size={{ xs:12 }}>
            <TextField
              fullWidth
              multiline
              rows={3}
              label="Delivery Instructions (Optional)"
              value={newAddress.instructions}
              onChange={(e) => setNewAddress({ ...newAddress, instructions: e.target.value })}
            />
          </Grid>
        </Grid>
      </DialogContent>
      <DialogActions>
        <Button onClick={onClose}>Cancel</Button>
        <Button
          onClick={onAdd}
          variant="contained"
          disabled={
            !newAddress.firstName ||
            !newAddress.lastName ||
            !newAddress.addressLine1 ||
            !newAddress.city
          }
        >
          Add Address
        </Button>
      </DialogActions>
    </Dialog>
  );
}
