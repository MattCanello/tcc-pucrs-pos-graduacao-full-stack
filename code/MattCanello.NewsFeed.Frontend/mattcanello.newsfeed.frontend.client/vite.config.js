import { fileURLToPath, URL } from 'node:url';

import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';

// https://vitejs.dev/config/
export default defineConfig({
    plugins: [react()],
    base: "/",
    resolve: {
        alias: {
            '@': fileURLToPath(new URL('./src', import.meta.url))
        }
    },
    server: {
        proxy: {
            '^/channels': {
                target: 'http://frontend-server:80/',
                secure: false
            },
            '^/articles': {
                target: 'http://frontend-server:80/',
                secure: false
            }
        },
        port: 5173,
        strictPort: true,
        host: true,
        origin: "http://0.0.0.0:5173"
    }
})
