import { useContext } from 'react';
import { Link as RouterLink } from 'react-router-dom';

import AppBar from '@mui/material/AppBar';
import Toolbar from '@mui/material/Toolbar';
import IconButton from '@mui/material/IconButton';
import HomeIcon from '@mui/icons-material/Home';
import { Link, Box } from '@mui/material';

import UserContext from '../context/UserContext';

const Navbar = () => {
  const userContext = useContext(UserContext);

  return (
    <Box>
      <AppBar position='static' sx={{ padding: '4px' }}>
        <Toolbar>
          <Link component={RouterLink} to='/' sx={{ flexGrow: 1 }}>
            <IconButton color='primary'>
              <HomeIcon sx={{ fontSize: '40px' }} />
            </IconButton>
          </Link>
          {!userContext.isLoggedin && (
            <Link
              component={RouterLink}
              to='/login'
              underline='hover'
              sx={{
                fontSize: '22px',
                marginRight: 2,
                fontWeight: 'bold',
              }}
            >
              Log in
            </Link>
          )}
          {!userContext.isLoggedin && (
            <Link
              component={RouterLink}
              to='/register'
              underline='hover'
              sx={{
                fontSize: '22px',
                marginRight: 1,
                fontWeight: 'bold',
              }}
            >
              Sign up
            </Link>
          )}
          {userContext.isLoggedin && (
            <Link
              component={RouterLink}
              to='/profile'
              underline='hover'
              sx={{
                fontSize: '22px',
                marginRight: 1,
                fontWeight: 'bold',
              }}
            >
              Profile
            </Link>
          )}
          {userContext.isLoggedin && (
            <Link
              underline='hover'
              component={RouterLink}
              to='/login'
              sx={{
                fontSize: '22px',
                marginRight: 1,
                fontWeight: 'bold',
              }}
              onClick={(e) => {
                userContext.handleLogout();
              }}
            >
              Log out
            </Link>
          )}
        </Toolbar>
      </AppBar>
    </Box>
  );
};

export default Navbar;
