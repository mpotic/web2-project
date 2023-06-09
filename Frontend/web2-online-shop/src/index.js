import React from 'react';
import ReactDOM from 'react-dom/client';
import App from './App';
import { BrowserRouter } from 'react-router-dom';
import { ThemeProvider } from '@emotion/react';
import myTheme from './style/myTheme';
import { CssBaseline } from '@mui/material';
import ContextProvider from './context/ContextProvider';

const root = ReactDOM.createRoot(document.getElementById('root'));
root.render(
  <ThemeProvider theme={myTheme}>
    <React.StrictMode>
      <BrowserRouter>
        <CssBaseline />
        <ContextProvider>
          <App />
        </ContextProvider>
      </BrowserRouter>
    </React.StrictMode>
  </ThemeProvider>
);
