'use client';

import React from 'react';
import {
    Box,
    Card,
    CardContent,
    TextField,
    Button,
    Typography,
    Grid,
    MenuItem,
    FormControl,
    InputLabel,
    Select,
    FormHelperText,
    Chip,
    Avatar,
    Container,
    SelectChangeEvent,
} from '@mui/material';
import { useFormik, FormikHelpers } from 'formik';
import * as yup from 'yup';
import PersonIcon from '@mui/icons-material/Person';
import EmailIcon from '@mui/icons-material/Email';
import PhoneIcon from '@mui/icons-material/Phone';
import WorkIcon from '@mui/icons-material/Work';
import SendIcon from '@mui/icons-material/Send';

interface FormValues {
    firstName: string;
    lastName: string;
    email: string;
    phone: string;
    age: string | number;
    department: string;
    skills: string[];
    bio: string;
}

const validationSchema = yup.object<FormValues>({
    firstName: yup
        .string()
        .min(2, 'First name should be at least 2 characters')
        .required('First name is required'),
    lastName: yup
        .string()
        .min(2, 'Last name should be at least 2 characters')
        .required('Last name is required'),
    email: yup
        .string()
        .email('Enter a valid email')
        .required('Email is required'),
    phone: yup
        .string()
        .matches(/^[\+]?[1-9][\d]{0,15}$/, 'Enter a valid phone number')
        .required('Phone number is required'),
    age: yup
        .number()
        .min(18, 'Must be at least 18 years old')
        .max(120, 'Must be less than 120 years old')
        .required('Age is required'),
    department: yup
        .string()
        .required('Department is required'),
    skills: yup
        .array()
        .of(yup.string())
        .min(1, 'Select at least one skill')
        .required('Skills are required'),
    bio: yup
        .string()
        .min(10, 'Bio should be at least 10 characters')
        .max(500, 'Bio should not exceed 500 characters')
        .required('Bio is required'),
});


const departments: string[] = [
    'Engineering',
    'Marketing',
    'Sales',
    'HR',
    'Finance',
    'Operations',
    'Design',
];

const skillOptions: string[] = [
    'JavaScript',
    'React',
    'Node.js',
    'Python',
    'UI/UX Design',
    'Project Management',
    'Data Analysis',
    'Marketing',
    'Sales',
    'Communication',
];

const FormPage: React.FC = () => {
    const formik = useFormik<FormValues>({
        initialValues: {
            firstName: '',
            lastName: '',
            email: '',
            phone: '',
            age: '',
            department: '',
            skills: [],
            bio: '',
        },
        validationSchema: validationSchema,
        onSubmit: (values: FormValues, { setSubmitting, resetForm }: FormikHelpers<FormValues>) => {
            setTimeout(() => {
                console.log('Form submitted:', values);
                alert('Form submitted successfully!');
                setSubmitting(false);
                resetForm();
            }, 1000);
        },
    });

    const handleSkillChange = (event: SelectChangeEvent<string[]>): void => {
        const value = event.target.value;
        formik.setFieldValue('skills', typeof value === 'string' ? value.split(',') : value);
    };

    return (
        <Box
            sx={{
                minHeight: '100vh',
                background: (theme) =>
                    theme.palette.mode === 'dark'
                        ? 'linear-gradient(135deg, #0f0f23 0%, #1a1a2e 50%, #16213e 100%)'
                        : 'linear-gradient(135deg, #667eea 0%, #764ba2 100%)',
                py: 4,
            }}
        >
            <Container maxWidth="sm">
                <Card
                    elevation={8}
                    sx={{
                        borderRadius: 3,
                        background: (theme) =>
                            theme.palette.mode === 'dark'
                                ? 'rgba(26, 26, 46, 0.95)'
                                : 'rgba(255, 255, 255, 0.95)',
                        backdropFilter: 'blur(10px)',
                        border: (theme) =>
                            theme.palette.mode === 'dark'
                                ? '1px solid rgba(99, 102, 241, 0.2)'
                                : '1px solid rgba(255, 255, 255, 0.3)',
                    }}
                >
                    <CardContent sx={{ p: 4 }}>
                        <Box
                            display="flex"
                            flexDirection="column"
                            alignItems="center"
                            mb={4}
                        >
                            <Avatar
                                sx={{
                                    bgcolor: 'primary.main',
                                    width: 56,
                                    height: 56,
                                    mb: 2,
                                }}
                            >
                                <PersonIcon fontSize="large" />
                            </Avatar>
                            <Typography
                                variant="h4"
                                component="h1"
                                fontWeight="bold"
                                color="primary"
                                textAlign="center"
                            >
                                User Registration
                            </Typography>
                            <Typography variant="subtitle1" color="text.secondary" textAlign="center">
                                Please fill out all required fields
                            </Typography>
                        </Box>

                        <form onSubmit={formik.handleSubmit}>
                            <Box sx={{ display: 'flex', flexDirection: 'column', gap: 3 }}>
                                <Typography variant="h6" color="primary" sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
                                    <PersonIcon /> Personal Information
                                </Typography>

                                <Grid container spacing={2}>
                                    <Grid size={{ xs: 6 }}>
                                        <TextField
                                            fullWidth
                                            id="firstName"
                                            name="firstName"
                                            label="First Name"
                                            value={formik.values.firstName}
                                            onChange={formik.handleChange}
                                            onBlur={formik.handleBlur}
                                            error={formik.touched.firstName && Boolean(formik.errors.firstName)}
                                            helperText={formik.touched.firstName && formik.errors.firstName}
                                            variant="outlined"
                                        />
                                    </Grid>
                                    <Grid size={{ xs: 6 }}>
                                        <TextField
                                            fullWidth
                                            id="lastName"
                                            name="lastName"
                                            label="Last Name"
                                            value={formik.values.lastName}
                                            onChange={formik.handleChange}
                                            onBlur={formik.handleBlur}
                                            error={formik.touched.lastName && Boolean(formik.errors.lastName)}
                                            helperText={formik.touched.lastName && formik.errors.lastName}
                                            variant="outlined"
                                        />
                                    </Grid>
                                </Grid>

                                <TextField
                                    fullWidth
                                    id="email"
                                    name="email"
                                    label="Email Address"
                                    type="email"
                                    value={formik.values.email}
                                    onChange={formik.handleChange}
                                    onBlur={formik.handleBlur}
                                    error={formik.touched.email && Boolean(formik.errors.email)}
                                    helperText={formik.touched.email && formik.errors.email}
                                    variant="outlined"
                                    InputProps={{
                                        startAdornment: <EmailIcon sx={{ color: 'action.active', mr: 1 }} />,
                                    }}
                                />

                                <TextField
                                    fullWidth
                                    id="phone"
                                    name="phone"
                                    label="Phone Number"
                                    value={formik.values.phone}
                                    onChange={formik.handleChange}
                                    onBlur={formik.handleBlur}
                                    error={formik.touched.phone && Boolean(formik.errors.phone)}
                                    helperText={formik.touched.phone && formik.errors.phone}
                                    variant="outlined"
                                    InputProps={{
                                        startAdornment: <PhoneIcon sx={{ color: 'action.active', mr: 1 }} />,
                                    }}
                                />

                                <TextField
                                    fullWidth
                                    id="age"
                                    name="age"
                                    label="Age"
                                    type="number"
                                    value={formik.values.age}
                                    onChange={formik.handleChange}
                                    onBlur={formik.handleBlur}
                                    error={formik.touched.age && Boolean(formik.errors.age)}
                                    helperText={formik.touched.age && formik.errors.age}
                                    variant="outlined"
                                />

                                <Typography variant="h6" color="primary" sx={{ display: 'flex', alignItems: 'center', gap: 1, mt: 2 }}>
                                    <WorkIcon /> Professional Information
                                </Typography>

                                <FormControl
                                    fullWidth
                                    error={formik.touched.department && Boolean(formik.errors.department)}
                                >
                                    <InputLabel id="department-label">Department</InputLabel>
                                    <Select
                                        labelId="department-label"
                                        id="department"
                                        name="department"
                                        value={formik.values.department}
                                        label="Department"
                                        onChange={formik.handleChange}
                                        onBlur={formik.handleBlur}
                                    >
                                        {departments.map((dept: string) => (
                                            <MenuItem key={dept} value={dept}>
                                                {dept}
                                            </MenuItem>
                                        ))}
                                    </Select>
                                    {formik.touched.department && formik.errors.department && (
                                        <FormHelperText>{formik.errors.department}</FormHelperText>
                                    )}
                                </FormControl>

                                <FormControl
                                    fullWidth
                                    error={formik.touched.skills && Boolean(formik.errors.skills)}
                                >
                                    <InputLabel id="skills-label">Skills</InputLabel>
                                    <Select
                                        labelId="skills-label"
                                        id="skills"
                                        multiple
                                        value={formik.values.skills}
                                        onChange={handleSkillChange}
                                        onBlur={formik.handleBlur}
                                        label="Skills"
                                        renderValue={(selected: string[]) => (
                                            <Box sx={{ display: 'flex', flexWrap: 'wrap', gap: 0.5 }}>
                                                {selected.map((value: string) => (
                                                    <Chip key={value} label={value} size="small" />
                                                ))}
                                            </Box>
                                        )}
                                    >
                                        {skillOptions.map((skill: string) => (
                                            <MenuItem key={skill} value={skill}>
                                                {skill}
                                            </MenuItem>
                                        ))}
                                    </Select>
                                    {formik.touched.skills && formik.errors.skills && (
                                        <FormHelperText>{formik.errors.skills}</FormHelperText>
                                    )}
                                </FormControl>

                                <TextField
                                    fullWidth
                                    id="bio"
                                    name="bio"
                                    label="Bio"
                                    multiline
                                    rows={4}
                                    value={formik.values.bio}
                                    onChange={formik.handleChange}
                                    onBlur={formik.handleBlur}
                                    error={formik.touched.bio && Boolean(formik.errors.bio)}
                                    helperText={
                                        formik.touched.bio && formik.errors.bio
                                            ? formik.errors.bio
                                            : `${formik.values.bio.length}/500 characters`
                                    }
                                    variant="outlined"
                                    placeholder="Tell us about yourself..."
                                />

                                <Button
                                    color="primary"
                                    variant="contained"
                                    fullWidth
                                    size="large"
                                    type="submit"
                                    disabled={formik.isSubmitting}
                                    startIcon={<SendIcon />}
                                    sx={{
                                        mt: 2,
                                        py: 1.5,
                                        borderRadius: 3,
                                        background: (theme) =>
                                            theme.palette.mode === 'dark'
                                                ? 'linear-gradient(45deg, #6366f1 30%, #8b5cf6 90%)'
                                                : 'linear-gradient(45deg, #FE6B8B 30%, #FF8E53 90%)',
                                        boxShadow: (theme) =>
                                            theme.palette.mode === 'dark'
                                                ? '0 3px 5px 2px rgba(99, 102, 241, .3)'
                                                : '0 3px 5px 2px rgba(255, 105, 135, .3)',
                                        '&:hover': {
                                            background: (theme) =>
                                                theme.palette.mode === 'dark'
                                                    ? 'linear-gradient(45deg, #5b21b6 60%, #7c3aed 100%)'
                                                    : 'linear-gradient(45deg, #FE6B8B 60%, #FF8E53 100%)',
                                        },
                                    }}
                                >
                                    {formik.isSubmitting ? 'Submitting...' : 'Submit Registration'}
                                </Button>
                            </Box>
                        </form>
                    </CardContent>
                </Card>
            </Container>
        </Box>
    );
};

export default FormPage;