'use client';

import React, { useState, useRef, useEffect } from 'react';
import {
  Box,
  Typography,
  Paper,
  IconButton,
  Grid,
  Button,
  CircularProgress,
  Fade,
  Alert,
  Divider
} from '@mui/material';
import {
  CloudUpload as UploadIcon,
  Delete as DeleteIcon,
  Refresh as RefreshIcon,
  Save as SaveIcon
} from '@mui/icons-material';

interface CustomizationImage {
  id: string;
  url: string;
  file: File;
}

declare global {
  interface Window {
    fabric: any;
  }
}

export default function StepCustomization({
  images,
  setImages,
  finalImageUrl,
  setFinalImageUrl,
  customizationDescription,
  setCustomizationDescription,
  loading,
  setLoading
}: {
  images: CustomizationImage[];
  setImages: (images: CustomizationImage[]) => void;
  finalImageUrl: string;
  setFinalImageUrl: (url: string) => void;
  customizationDescription: string;
  setCustomizationDescription: (desc: string) => void;
  loading: boolean;
  setLoading: (loading: boolean) => void;
}) {
  const [uploading, setUploading] = useState(false);
  const [generating, setGenerating] = useState(false);
  const [fabricLoaded, setFabricLoaded] = useState(false);
  const [canvasInitialized, setCanvasInitialized] = useState(false);
  const canvasRef = useRef<HTMLCanvasElement>(null);
  const fabricCanvasRef = useRef<any>(null);
  const fileInputRef = useRef<HTMLInputElement>(null);

  useEffect(() => {
    const loadFabric = async () => {
      if (typeof window !== 'undefined' && !window.fabric) {
        const script = document.createElement('script');
        script.src = 'https://cdnjs.cloudflare.com/ajax/libs/fabric.js/5.3.0/fabric.min.js';
        script.onload = () => {
          console.log('Fabric.js loaded');
          setFabricLoaded(true);
        };
        script.onerror = () => {
          console.error('Failed to load Fabric.js');
        };
        document.head.appendChild(script);
      } else if (window.fabric) {
        console.log('Fabric.js already available');
        setFabricLoaded(true);
      }
    };

    loadFabric();

    return () => {
      if (fabricCanvasRef.current) {
        fabricCanvasRef.current.dispose();
      }
    };
  }, []);

  // Initialize canvas when Fabric is loaded AND canvas ref is available
  useEffect(() => {
    if (fabricLoaded && canvasRef.current && !canvasInitialized) {
      initCanvas();
    }
  }, [fabricLoaded, canvasInitialized]);

  const initCanvas = () => {
    if (canvasRef.current && window.fabric && !fabricCanvasRef.current) {
      console.log('Initializing canvas');
      try {
        fabricCanvasRef.current = new window.fabric.Canvas(canvasRef.current, {
          width: 800,
          height: 600,
          backgroundColor: 'white'
        });
        
        // Add some visual feedback that canvas is working
        fabricCanvasRef.current.renderAll();
        setCanvasInitialized(true);
        console.log('Canvas initialized successfully');
      } catch (error) {
        console.error('Failed to initialize canvas:', error);
      }
    }
  };

  const uploadImage = async (file: File): Promise<string> => {
    const formData = new FormData();
    formData.append('file', file);
    
    const response = await fetch('https://impr.ink/upload', {
      method: 'POST',
      body: formData,
    });
    
    if (!response.ok) {
      throw new Error('Upload failed');
    }
    
    const data = await response.json();
    return data.url;
  };

  const handleFileSelect = async (files: FileList) => {
    if (images.length + files.length > 10) {
      alert('Maximum 10 images allowed');
      return;
    }

    setUploading(true);
    try {
      const newImages: CustomizationImage[] = [];
      
      for (let i = 0; i < files.length; i++) {
        const file = files[i];
        if (file.type.startsWith('image/')) {
          const url = await uploadImage(file);
          newImages.push({
            id: Math.random().toString(36).substr(2, 9),
            url,
            file
          });
        }
      }
      
      setImages([...images, ...newImages]);
    } catch (error) {
      console.error('Upload failed:', error);
      alert('Upload failed. Please try again.');
    } finally {
      setUploading(false);
    }
  };

  const addImageToCanvas = (imageUrl: string) => {
    console.log('Adding image to canvas:', imageUrl);
    console.log('Canvas available:', !!fabricCanvasRef.current);
    console.log('Fabric available:', !!window.fabric);
    
    if (!fabricCanvasRef.current || !window.fabric) {
      console.error('Canvas or Fabric not available');
      return;
    }

    window.fabric.Image.fromURL(imageUrl, (img: any) => {
      if (img) {
        console.log('Image loaded successfully');
        
        // Scale the image to fit reasonably on canvas
        const maxWidth = 200;
        const maxHeight = 200;
        const scaleX = maxWidth / img.width;
        const scaleY = maxHeight / img.height;
        const scale = Math.min(scaleX, scaleY);
        
        img.set({
          left: Math.random() * (800 - maxWidth),
          top: Math.random() * (600 - maxHeight),
          scaleX: scale,
          scaleY: scale
        });
        
        fabricCanvasRef.current.add(img);
        fabricCanvasRef.current.setActiveObject(img);
        fabricCanvasRef.current.renderAll();
        console.log('Image added to canvas');
      } else {
        console.error('Failed to load image');
      }
    }, { 
      crossOrigin: 'anonymous'
    });
  };

  const removeImage = (id: string) => {
    setImages(images.filter(img => img.id !== id));
  };

  const clearCanvas = () => {
    if (fabricCanvasRef.current) {
      fabricCanvasRef.current.clear();
      fabricCanvasRef.current.backgroundColor = 'white';
      fabricCanvasRef.current.renderAll();
    }
  };

  const generateFinalImage = async () => {
    if (!fabricCanvasRef.current) return;

    setGenerating(true);
    try {
      const dataURL = fabricCanvasRef.current.toDataURL({
        format: 'png',
        quality: 0.9
      });

      const response = await fetch(dataURL);
      const blob = await response.blob();
      const file = new File([blob], 'customization.png', { type: 'image/png' });
      const url = await uploadImage(file);
      setFinalImageUrl(url);
    } catch (error) {
      console.error('Failed to generate final image:', error);
      alert('Failed to generate final image. Please try again.');
    } finally {
      setGenerating(false);
    }
  };

  const handleDrop = (e: React.DragEvent) => {
    e.preventDefault();
    const files = e.dataTransfer.files;
    if (files.length > 0) {
      handleFileSelect(files);
    }
  };

  const handleDragOver = (e: React.DragEvent) => {
    e.preventDefault();
  };

  if (!fabricLoaded) {
    return (
      <Box display="flex" justifyContent="center" alignItems="center" minHeight={400}>
        <CircularProgress />
        <Typography ml={2}>Loading canvas editor...</Typography>
      </Box>
    );
  }

  return (
    <Fade in>
      <Box>
        <Typography variant="h5" gutterBottom>Customize Your Product</Typography>
        
        <Grid container spacing={3}>
          <Grid size={{ xs:12, md:8 }}>
            <Paper sx={{ p: 2 }}>
              <Box display="flex" justifyContent="space-between" alignItems="center" mb={2}>
                <Typography variant="h6">Design Canvas</Typography>
                <Box>
                  <Button
                    size="small"
                    startIcon={<RefreshIcon />}
                    onClick={clearCanvas}
                    disabled={!canvasInitialized}
                    sx={{ mr: 1 }}
                  >
                    Clear
                  </Button>
                  <Button
                    variant="contained"
                    size="small"
                    startIcon={generating ? <CircularProgress size={16} /> : <SaveIcon />}
                    onClick={generateFinalImage}
                    disabled={generating || !canvasInitialized}
                  >
                    {generating ? 'Saving...' : 'Save Design'}
                  </Button>
                </Box>
              </Box>
              
              <Box 
                sx={{ 
                  border: '2px solid #ddd',
                  borderRadius: 1,
                  overflow: 'hidden',
                  position: 'relative',
                  backgroundColor: canvasInitialized ? 'transparent' : '#f5f5f5'
                }}
              >
                <canvas 
                  ref={canvasRef}
                  style={{ 
                    display: 'block',
                    maxWidth: '100%'
                  }}
                />
                {!canvasInitialized && (
                  <Box
                    position="absolute"
                    top={0}
                    left={0}
                    right={0}
                    bottom={0}
                    display="flex"
                    alignItems="center"
                    justifyContent="center"
                    bgcolor="rgba(0,0,0,0.1)"
                  >
                    <Typography color="text.secondary">
                      Initializing canvas...
                    </Typography>
                  </Box>
                )}
              </Box>
              
              <Typography variant="caption" color="text.secondary" mt={1} display="block">
                Drag, resize, and rotate images on the canvas. Use the controls around selected images to manipulate them.
              </Typography>
            </Paper>
          </Grid>

          <Grid size={{ xs:12, md:4 }}>
            <Paper sx={{ p: 2 }}>
              <Typography variant="h6" gutterBottom>Image Library</Typography>
              
              <Button
                fullWidth
                variant="outlined"
                startIcon={<UploadIcon />}
                onClick={() => fileInputRef.current?.click()}
                disabled={images.length >= 10 || uploading}
                sx={{ mb: 2 }}
              >
                Add Images ({images.length}/10)
              </Button>

              {images.length === 0 ? (
                <Paper
                  sx={{
                    p: 4,
                    textAlign: 'center',
                    border: '2px dashed',
                    borderColor: 'grey.300',
                    bgcolor: 'grey.50',
                    cursor: 'pointer'
                  }}
                  onDrop={handleDrop}
                  onDragOver={handleDragOver}
                  onClick={() => fileInputRef.current?.click()}
                >
                  <UploadIcon sx={{ fontSize: 40, color: 'grey.500', mb: 1 }} />
                  <Typography variant="body2" color="text.secondary">
                    Drop images here or click to browse
                  </Typography>
                </Paper>
              ) : (
                <Box>
                  {images.map((image, index) => (
                    <Box key={image.id} mb={1}>
                      <Paper
                        sx={{
                          p: 1,
                          display: 'flex',
                          alignItems: 'center',
                          gap: 1,
                          cursor: canvasInitialized ? 'pointer' : 'not-allowed',
                          opacity: canvasInitialized ? 1 : 0.6,
                          '&:hover': canvasInitialized ? { bgcolor: 'grey.50' } : {}
                        }}
                        onClick={() => canvasInitialized && addImageToCanvas(image.url)}
                      >
                        <img
                          src={image.url}
                          alt={`Image ${index + 1}`}
                          style={{
                            width: 40,
                            height: 40,
                            objectFit: 'cover',
                            borderRadius: 4
                          }}
                        />
                        <Typography variant="body2" flex={1}>
                          Image {index + 1}
                        </Typography>
                        <IconButton
                          size="small"
                          onClick={(e) => {
                            e.stopPropagation();
                            removeImage(image.id);
                          }}
                        >
                          <DeleteIcon fontSize="small" />
                        </IconButton>
                      </Paper>
                    </Box>
                  ))}
                  
                  <Divider sx={{ my: 2 }} />
                  
                  <Typography variant="caption" color="text.secondary">
                    {canvasInitialized 
                      ? 'Click on any image to add it to the canvas'
                      : 'Canvas is initializing...'
                    }
                  </Typography>
                </Box>
              )}
            </Paper>

            {finalImageUrl && (
              <Paper sx={{ p: 2, mt: 2 }}>
                <Typography variant="h6" gutterBottom>Saved Design</Typography>
                <img
                  src={finalImageUrl}
                  alt="Final design"
                  style={{
                    width: '100%',
                    height: 'auto',
                    objectFit: 'contain',
                    borderRadius: 8,
                    border: '1px solid #ddd'
                  }}
                />
              </Paper>
            )}
          </Grid>
        </Grid>

        {uploading && (
          <Alert severity="info" sx={{ mt: 2 }}>
            <Box display="flex" alignItems="center" gap={1}>
              <CircularProgress size={16} />
              Uploading images...
            </Box>
          </Alert>
        )}

        <input
          ref={fileInputRef}
          type="file"
          multiple
          accept="image/*"
          style={{ display: 'none' }}
          onChange={(e) => {
            if (e.target.files) {
              handleFileSelect(e.target.files);
            }
          }}
        />
      </Box>
    </Fade>
  );
}