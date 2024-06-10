import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';
import { createHtmlPlugin } from 'vite-plugin-html';
import copy from 'rollup-plugin-copy';
import { resolve } from 'path';

const appName = process.env.NODE_APP.trim();
const outDir = resolve(__dirname, `../wwwroot/${appName}`);
const nerCoreUrl = 'http://localhost:5000';
const netFwUrl = 'http://localhost:64193';
const webSocket = 'ws://localhost:5000';

export default defineConfig({
  base: process.env.NODE_ENV === 'production' ? `/${appName}` : '/',
  server: {
    open: true,
    host: true,
    proxy: {
      '/otherSportNewDataUploadNotifyHub': {
        target: webSocket,
        ws: true,
        secure: false
      },
      '/changedInfoNotifyHub': {
        target: webSocket,
        ws: true,
        secure: false
      },
      '/ticketCenterNotifyHub': {
        target: webSocket,
        ws: true,
        secure: false
      },
      '/preMatchNotifyHub': {
        target: webSocket,
        ws: true,
        secure: false
      },
      '/api-netcore': {
        target: nerCoreUrl,
        secure: false,
        changeOrigin: true,
        rewrite: path => path.replace(/^\/api-netcore/, 'api')
      },
      '/Tickets': {
        target: nerCoreUrl,
        secure: false,
        changeOrigin: true
      },
      '/net-core-state-api': {
        target: nerCoreUrl,
        secure: false,
        changeOrigin: true,
        rewrite: path => path.replace(/^\/net-core-state-api/, 'state-api')
      },
      '/api': {
        target: netFwUrl,
        secure: false,
        changeOrigin: true
      },
      '/signalr': {
        target: netFwUrl,
        secure: false,
        changeOrigin: true
      },
      '/Scripts': {
        target: netFwUrl,
        secure: false,
        changeOrigin: true
      },
      '/Content': {
        target: netFwUrl,
        secure: false,
        changeOrigin: true
      },
      '/fonts': {
        target: netFwUrl,
        secure: false,
        changeOrigin: true
      },
      '/state-api': {
        target: netFwUrl,
        secure: false,
        changeOrigin: true
      },
      '/TaskScheduling': {
        target: netFwUrl,
        secure: false,
        changeOrigin: true
      },
      '/wwwroot': {
        target: netFwUrl,
        secure: false,
        changeOrigin: true
      }
    },
    port: 3000
  },
  plugins: [
    react(),
    createHtmlPlugin({
      minify: false,
      entry: `src/apps/${appName}.jsx`
    }),
    copy({
      targets: [
        {
          src: `../wwwroot/${appName}/server.html`,
          dest: outDir,
          rename: 'server.cshtml'
        }
      ],
      hook: 'closeBundle'
    })
  ],
  define: {
    global: 'window'
  },
  build: {
    outDir,
    sourcemap: false,
    emptyOutDir: true,
    rollupOptions: {
      input: {
        main: 'server.html'
      },
      output: {
        assetFileNames: 'assets/ext/[name]-[hash][extname]',
        chunkFileNames: 'assets/js/[name]-[hash].js',
        entryFileNames: 'assets/js/[name]-[hash].js',
        name: 'MyBundle'
      }
    }
  },
  experimental: {
    renderBuiltUrl(filename, { hostType }) {
      if (hostType === 'js') {
        return `/${appName}/` + filename;
      }
      if (hostType === 'css') {
        const file = filename.replace('assets/ext', '');
        return '.' + file;
      }
    }
  },
  resolve: {
    alias: {
      '~react-bootstrap-multiselect': resolve(
        __dirname,
        'node_modules/react-bootstrap-multiselect'
      ),
      '~sweetalert2': resolve(__dirname, 'node_modules/sweetalert2'),
      '~react-toastify': resolve(__dirname, 'node_modules/react-toastify'),
      '~react-table': resolve(__dirname, 'node_modules/react-table'),
      '~react-datepicker': resolve(__dirname, 'node_modules/react-datepicker'),
      '~fixed-data-table-2': resolve(
        __dirname,
        'node_modules/fixed-data-table-2'
      )
    }
  }
});
