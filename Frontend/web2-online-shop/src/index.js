import React from 'react';
import ReactDOM from 'react-dom/client';
import App from './App';
import { BrowserRouter } from 'react-router-dom';
import { ThemeProvider } from '@emotion/react';
import myTheme from './style/myTheme';
import { CssBaseline } from '@mui/material';
import ContextProvider from './context/ContextProvider';
import { GoogleOAuthProvider } from '@react-oauth/google';

const root = ReactDOM.createRoot(document.getElementById('root'));
root.render(
  <GoogleOAuthProvider clientId='19815919039-igkkmbt256ijmj205mgp6o5uch8esbp2.apps.googleusercontent.com'>
    <ThemeProvider theme={myTheme}>
      <BrowserRouter>
        <CssBaseline />
        <ContextProvider>
          <App />
        </ContextProvider>
      </BrowserRouter>
    </ThemeProvider>
  </GoogleOAuthProvider>
);
