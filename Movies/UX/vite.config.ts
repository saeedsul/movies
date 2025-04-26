import { defineConfig } from 'vite';
import plugin from '@vitejs/plugin-react';

// https://vitejs.dev/config/
export default defineConfig({
    plugins: [plugin()],
    server: {
        port: 54316,
        proxy: {
            '/api': {
                target: 'http://localhost:5118',
                changeOrigin: true
            }
        }
    }
})
