import { Link as RouterLink } from 'react-router-dom';
import { useContext } from 'react';

import { Typography, Container, Paper, Link } from '@mui/material';

import UserContext from '../context/UserContext';

const Home = () => {
  const { isLoggedin, username } = useContext(UserContext);

  return (
    <Container sx={{ display: 'flex', justifyContent: 'center' }}>
      <Paper
        sx={{
          width: '60%',
          display: 'flex',
          flexDirection: 'column',
          textAlign: 'center',
          marginTop: '12vh',
          padding: '20px',
        }}
        elevation={4}
      >
        {!isLoggedin && (
          <>
            <Typography
              variant='h3'
              color='primary'
              sx={{ marginBottom: '20px' }}
            >
              Welcome!
            </Typography>
            <Typography variant='h4' color='primary'>
              <Link
                component={RouterLink}
                to='/register'
                underline='hover'
                color='secondary'
              >
                Sign up
              </Link>
              &nbsp;to get started.
              <br />
              If you already have an account,&nbsp;
              <Link
                component={RouterLink}
                to='/login'
                underline='hover'
                color='secondary'
              >
                sign in
              </Link>
              .
            </Typography>
          </>
        )}
        {isLoggedin && (
          <Typography variant='h3' color='primary'>
            Welcome {username}!
          </Typography>
        )}
      </Paper>
    </Container>
  );
};

export default Home;
