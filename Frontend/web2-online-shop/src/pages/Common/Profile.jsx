import { Link as RouterLink } from 'react-router-dom';
import { useState, useEffect } from 'react';

import {
  Button,
  Container,
  Link,
  Paper,
  TextField,
  Typography,
} from '@mui/material';

import styles from '../../style/authStyles';
import { toasterUtil as toaster } from '../../utils/toasterUtil';
import useHttp from '../../hooks/useHttp';

const Profile = () => {
  const [user, setUser] = useState(userData);
  const [validity, setValidity] = useState(fieldValidity);
  const { getRequest, isLoading, error, statusCode, data } = useHttp();

  const handleSubmit = (event) => {
    event.preventDefault();

    setValidity(validateFields(user));

    for (const field in validity) {
      if (validity[field].error) {
        return;
      }
    }

    getRequest('https://localhost:44301/api/user/user');
  };

  useEffect(() => {
    if (data) {
    }
    if (isLoading) {
      return;
    } else if (statusCode === 200 && !error) {
      toaster.handleSuccess('Successfuly logged in!');
    } else if (statusCode !== 200 && error) {
      toaster.handleError(statusCode, error);
    }
  }, [isLoading, statusCode, error, data]);

  return (
    <Container sx={{ ...styles.container, marginTop: '100px' }}>
      <Paper
        component='form'
        sx={{ ...styles.paper, width: '30%' }}
        elevation={4}
      >
        PRofile
        <TextField
          placeholder='Username'
          value={user.username}
          sx={{ width: '100%' }}
          error={validity.username.error}
          helperText={validity.username.helper}
          onChange={(e) => {
            setUser((currentUser) => {
              return { ...currentUser, username: e.target.value };
            });
          }}
        />
        <TextField
          placeholder='Password'
          value={user.password}
          sx={{ width: '100%' }}
          error={validity.password.error}
          helperText={validity.password.helper}
          onChange={(e) => {
            setUser((currentUser) => {
              return { ...currentUser, password: e.target.value };
            });
          }}
        />
        <Button
          sx={{ marginTop: '20px', width: '50%' }}
          variant='contained'
          onClick={(event) => {
            handleSubmit(event);
          }}
        >
          Sign in
        </Button>
        <Typography sx={{ marginTop: '15px' }}>
          Don't have an account? Sign up
          <Link component={RouterLink} to='/register' underline='hover'>
            here
          </Link>
          !
        </Typography>
      </Paper>
    </Container>
  );
};

var userData = {
  username: '',
  password: '',
};

const fieldValidity = {
  username: {
    error: false,
    helper: '',
  },
  password: {
    error: false,
    helper: '',
  },
};

const validateFields = (user) => {
  const updatedFieldValidity = { ...fieldValidity };

  const requiredFields = Object.keys(fieldValidity);

  requiredFields.forEach((field) => {
    if (!user[field]) {
      updatedFieldValidity[field].error = true;
      updatedFieldValidity[field].helper = 'Field is required';
    } else if (user[field].length < 3) {
      updatedFieldValidity[field].error = true;
      updatedFieldValidity[field].helper = 'Too short';
    } else {
      updatedFieldValidity[field].error = false;
      updatedFieldValidity[field].helper = '';
    }
  });

  return updatedFieldValidity;
};

export default Login;
