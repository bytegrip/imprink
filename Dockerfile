FROM node:18-alpine

WORKDIR /app

COPY scripts/package.json ./

RUN npm install

COPY scripts/upload-server.js ./

RUN mkdir -p uploads

EXPOSE 3000

CMD ["npm", "start"]