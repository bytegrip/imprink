const express = require('express');
const multer = require('multer');
const path = require('path');
const fs = require('fs');
const { v4: uuidv4 } = require('uuid');

const app = express();
const PORT = process.env.PORT || 3000;
const BASE_URL = process.env.BASE_URL || 'http://localhost:3000';
const UPLOAD_DIR = process.env.UPLOAD_DIR || './uploads';
const HASH_FILENAME = process.env.HASH_FILENAME === 'true';

if (!fs.existsSync(UPLOAD_DIR)) {
  fs.mkdirSync(UPLOAD_DIR, { recursive: true });
}

const storage = multer.diskStorage({
  destination: (req, file, cb) => {
    cb(null, UPLOAD_DIR);
  },
  filename: (req, file, cb) => {
    if (HASH_FILENAME) {
      const ext = path.extname(file.originalname);
      const filename = uuidv4() + ext;
      cb(null, filename);
    } else {
      cb(null, file.originalname);
    }
  }
});

const upload = multer({ storage });

app.post('/upload', upload.single('file'), (req, res) => {
  if (!req.file) {
    return res.status(400).json({ error: 'No file uploaded' });
  }
  
  const fileUrl = `${BASE_URL}/files/${req.file.filename}`;
  res.json({ url: fileUrl });
});

app.put('/files/:path(*)', upload.single('file'), (req, res) => {
  const filePath = req.params.path;
  
  if (!req.file) {
    return res.status(400).json({ error: 'No file uploaded' });
  }
  
  const oldPath = req.file.path;
  const newPath = path.join(UPLOAD_DIR, filePath);
  
  const dir = path.dirname(newPath);
  if (!fs.existsSync(dir)) {
    fs.mkdirSync(dir, { recursive: true });
  }
  
  fs.renameSync(oldPath, newPath);
  
  const fileUrl = `${BASE_URL}/files/${filePath}`;
  res.json({ url: fileUrl });
});

app.get('/files/:path(*)', (req, res) => {
  const filePath = path.join(UPLOAD_DIR, req.params.path);
  
  if (!fs.existsSync(filePath)) {
    return res.status(404).json({ error: 'File not found' });
  }
  
  res.sendFile(path.resolve(filePath));
});

app.listen(PORT, () => {
  console.log(`Server running on port ${PORT}`);
});