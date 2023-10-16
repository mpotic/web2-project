import { createTheme } from '@mui/material';
import { teal } from '@mui/material/colors';

export const themeOptions = {
  palette: {
    mode: 'dark',
    background: { default: '#398f88' },
    primary: {
      main: teal[300],
    },
    secondary: {
      main: '#f50057', // Razzmatazz
    },
    error: {
      main: '#f44336', // Red
    },
    warning: {
      main: '#ffeb3b', // Yellow
    },
    success: {
      main: '#4caf50', // Green
    },
  },
  components: {
    MuiContainer: {
      styleOverrides: {
        root: {
          padding: '16px',
        },
      },
    },
    MuiPaper: {
      styleOverrides: {
        root: {},
      },
    },
    MuiTypography: {
      styleOverrides: {
        root: {
          padding: '4px',
        },
      },
    },
    MuiButton: {
      styleOverrides: {
        root: {
          '&:hover': {
            backgroundColor: 'inherit',
            color: 'inherit',
          },
        },
      },
    },
  },
};

const myTheme = createTheme(themeOptions);

myTheme.spacing(2);

export default myTheme;
