import { Typography, Container, Paper } from '@mui/material';

const Home = () => {
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
        <Typography variant='h3' color='primary' sx={{ marginBottom: '15px' }}>
          Welcome!
        </Typography>
        <Typography variant='h4' color='primary'>
          Sign up to get started.
          <br />
          If you already have an account, sign in.
        </Typography>
      </Paper>
    </Container>
  );
};

export default Home;
