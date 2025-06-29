'use client';

import { Stepper, Step, StepLabel } from '@mui/material';

export default function StepperHeader({ activeStep, steps }: { activeStep: number, steps: string[] }) {
  return (
    <Stepper activeStep={activeStep} sx={{ mb: 4 }}>
      {steps.map((label) => (
        <Step key={label}>
          <StepLabel>{label}</StepLabel>
        </Step>
      ))}
    </Stepper>
  );
}
