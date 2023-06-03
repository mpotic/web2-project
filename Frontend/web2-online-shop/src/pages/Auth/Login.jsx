import { Link as RouterLink } from 'react-router-dom';

import {
  Button,
  Container,
  Link,
  Paper,
  TextField,
  Typography,
} from '@mui/material';

import styles from './styles';
import { useState } from 'react';

const userData = {
  username: '',
  password: '',
};

const Login = () => {
  const [user, setUser] = useState(userData);

  return (
    <Container sx={{ ...styles.container, marginTop: '100px' }}>
      <Paper
        component='form'
        sx={{ ...styles.paper, width: '30%' }}
        elevation={4}
      >
        <TextField
          placeholder='Username'
          sx={{ width: '100%' }}
          onChange={(e) => {
            setUser((currentUser) => {
              return { ...currentUser, username: e.value.target };
            });
          }}
        />
        <TextField
          placeholder='Password'
          sx={{ width: '100%' }}
          onChange={(e) => {
            setUser((currentUser) => {
              return { ...currentUser, password: e.value.target };
            });
          }}
        />
        <Button sx={{ marginTop: '20px', width: '50%' }} variant='contained'>
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

export default Login;
