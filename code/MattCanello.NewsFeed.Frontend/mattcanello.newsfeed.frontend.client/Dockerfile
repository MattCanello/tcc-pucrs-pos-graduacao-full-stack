FROM node:alpine

WORKDIR /srv/app

COPY package*.json ./

RUN npm install

COPY . .

ENV CHOKIDAR_USEPOLLING=true

ENTRYPOINT ["/bin/sh", "-c", "npm run dev"]