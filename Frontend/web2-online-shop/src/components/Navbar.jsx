import * as React from 'react';
import AppBar from '@mui/material/AppBar';
import Toolbar from '@mui/material/Toolbar';
import IconButton from '@mui/material/IconButton';
import HomeIcon from '@mui/icons-material/Home';
import userHelper from '../utils/userUtils';
import { Link, Box } from '@mui/material';
import { Link as RouterLink } from 'react-router-dom';

function Navbar() {
  return (
    <Box>
      <AppBar position='static' sx={{ padding: '4px' }}>
        <Toolbar>
          <Link component={RouterLink} to='/' sx={{ flexGrow: 1 }}>
            <IconButton color='primary'>
              <HomeIcon sx={{ fontSize: '40px' }} />
            </IconButton>
          </Link>
          {!userHelper.isLoggedin() && (
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
          {!userHelper.isLoggedin() && (
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
        </Toolbar>
      </AppBar>
    </Box>
  );
}

export default Navbar;
