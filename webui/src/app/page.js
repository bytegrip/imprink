'use client';

import {
    Box,
    Container,
    Typography,
    Button,
    Card,
    CardContent,
    TextField,
    Chip,
    Grid,
} from '@mui/material';

export default function HomePage() {

    return (
            <Container maxWidth="lg" sx={{ py: 4 }}>
                <Box sx={{ mb: 6, textAlign: 'center' }}>
                    <Typography variant="h1" gutterBottom>
                        Welcome to Modern UI
                    </Typography>
                    <Typography variant="h5" color="text.secondary" sx={{ mb: 4 }}>
                        Experience the beauty of modern Material-UI theming
                    </Typography>
                    <Box sx={{ display: 'flex', gap: 2, justifyContent: 'center', flexWrap: 'wrap' }}>
                        <Button variant="contained" size="large">
                            Get Started
                        </Button>
                        <Button variant="outlined" size="large">
                            Learn More
                        </Button>
                    </Box>
                </Box>

                <Grid container spacing={4}>
                    <Grid item xs={12} md={6}>
                        <Card>
                            <CardContent>
                                <Typography variant="h4" gutterBottom>
                                    Beautiful Cards
                                </Typography>
                                <Typography variant="body1" color="text.secondary" paragraph>
                                    Cards with modern gradients, hover effects, and perfect spacing that make your content shine.
                                </Typography>
                                <Box sx={{ display: 'flex', gap: 1, flexWrap: 'wrap', mb: 2 }}>
                                    <Chip label="Modern" />
                                    <Chip label="Responsive" />
                                    <Chip label="Beautiful" />
                                </Box>
                                <Button variant="contained">Explore</Button>
                            </CardContent>
                        </Card>
                    </Grid>

                    <Grid item xs={12} md={6}>
                        <Card>
                            <CardContent>
                                <Typography variant="h4" gutterBottom>
                                    Form Elements
                                </Typography>
                                <Box component="form" sx={{ display: 'flex', flexDirection: 'column', gap: 3 }}>
                                    <TextField
                                        label="Your Name"
                                        variant="outlined"
                                        fullWidth
                                    />
                                    <TextField
                                        label="Email Address"
                                        variant="outlined"
                                        type="email"
                                        fullWidth
                                    />
                                    <TextField
                                        label="Message"
                                        variant="outlined"
                                        multiline
                                        rows={4}
                                        fullWidth
                                    />
                                    <Button variant="contained" size="large">
                                        Send Message
                                    </Button>
                                </Box>
                            </CardContent>
                        </Card>
                    </Grid>
                </Grid>
            </Container>
    );
}