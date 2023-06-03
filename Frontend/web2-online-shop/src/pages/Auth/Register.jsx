import { useState } from 'react';
import { Link as RouterLink } from 'react-router-dom';

import {
  Button,
  Container,
  Link,
  MenuItem,
  Paper,
  Select,
  TextField,
  Typography,
  InputLabel,
  FormControl,
  Box,
} from '@mui/material';

import styles from './styles';

import Calendar from '../../components/Calendar';
import UploadButtons from '../../components/UploadButton';
import { formatDateTime } from '../../utils/dateTimeUtils';
import useHttp from '../../hooks/useHttp';

var userData = {
  username: '',
  password: '',
  repeatPassword: '',
  email: '',
  firstname: '',
  lastname: '',
  address: '',
  birthdate: '',
  role: '',
  imageUrl: null,
};

const Register = () => {
  const [user, setUser] = useState(userData);
  const [validity, setValidity] = useState(fieldValidity);
  const http = useHttp();

  const handleSubmit = (event) => {
    event.preventDefault();

    setValidity(validateFields(user));

    for (const field in validity) {
      if (validity[field].error) {
        return;
      }
    }

    http.postRequestFormData('localhost', user);
  };

  return (
    <Container sx={{ ...styles.container }}>
      <Paper component='form' sx={{ ...styles.paper }} elevation={4}>
        <Box sx={styles.rowBox}>
          <TextField
            placeholder='Username'
            id='username'
            value={user.username}
            sx={styles.textField}
            onChange={(e) => {
              setUser((currentUser) => {
                return { ...currentUser, username: e.target.value };
              });
            }}
            required
          />
          <TextField
            placeholder='Email'
            id='email'
            value={user.email}
            sx={styles.textField}
            type='email'
            onChange={(e) => {
              setUser((currentUser) => {
                return { ...currentUser, email: e.target.value };
              });
            }}
            required
          />
        </Box>
        <Box sx={styles.rowBox}>
          <TextField
            placeholder='Password'
            id='password'
            value={user.password}
            sx={styles.textField}
            onChange={(e) => {
              setUser((currentUser) => {
                return { ...currentUser, password: e.target.value };
              });
            }}
            required
          />
          <TextField
            placeholder='Repeat password'
            id='repeat-password'
            value={user.repeatPassword}
            sx={styles.textField}
            onChange={(e) => {
              setUser((currentUser) => {
                return { ...currentUser, repeatPassword: e.target.value };
              });
            }}
            required
          />
        </Box>
        <Box sx={styles.rowBox}>
          <TextField
            placeholder='Firstname'
            id='firstname'
            value={user.firstname}
            sx={styles.textField}
            onChange={(e) => {
              setUser((currentUser) => {
                return { ...currentUser, firstname: e.target.value };
              });
            }}
            required
          />
          <TextField
            placeholder='Lastname'
            id='lastname'
            value={user.lastname}
            sx={styles.textField}
            onChange={(e) => {
              setUser((currentUser) => {
                return { ...currentUser, lastname: e.target.value };
              });
            }}
            required
          />
        </Box>
        <Box sx={styles.rowBox}>
          <TextField
            placeholder='Address'
            id='address'
            value={user.address}
            sx={styles.textField}
            onChange={(e) => {
              setUser((currentUser) => {
                return { ...currentUser, address: e.target.value };
              });
            }}
            required
          />
          <Calendar
            label='Birthdate'
            id='birthdate'
            sx={styles.textField}
            callback={(date) => {
              date = formatDateTime(date);
              setUser((currentUser) => {
                return { ...currentUser, birthdate: date };
              });
            }}
          />
        </Box>
        <Box sx={{ ...styles.rowBox }}>
          <FormControl
            sx={{
              width: user.imageUrl == null ? '50%' : '30%',
            }}
          >
            <InputLabel id='select-role-label'>Role</InputLabel>
            <Select
              value={user.role}
              labelId='select-role-label'
              label='Role'
              id='role'
              onChange={(e) => {
                setUser((currentUser) => {
                  return { ...currentUser, role: e.target.value };
                });
              }}
            >
              <MenuItem value='Buyer'>Buyer</MenuItem>
              <MenuItem value='Seller'>Seller</MenuItem>
            </Select>
          </FormControl>
          <UploadButtons
            width={user.imageUrl == null ? '50%' : '70%'}
            maxHeightPerc='55%'
            maxWidthPerc='55%'
            id='imageUrl'
            callback={(url) => {
              setUser((currentUser) => {
                return { ...currentUser, imageUrl: url };
              });
            }}
          />
        </Box>
        <Button
          sx={{ width: '30%', marginTop: '15px', alignSelf: 'center' }}
          variant='contained'
          type='submit'
          onClick={(event) => {
            handleSubmit(event);
          }}
        >
          Register
        </Button>
        <Typography sx={{ marginTop: '10px', alignSelf: 'center' }}>
          Already have an account? Sign in
          <Link component={RouterLink} to='/login' underline='hover'>
            here
          </Link>
          !
        </Typography>
      </Paper>
    </Container>
  );
};

const fieldValidity = {
  username: {
    error: false,
    helper: '',
  },
  email: {
    error: false,
    helper: '',
  },
  password: {
    error: false,
    helper: '',
  },
  repeatPassword: {
    error: false,
    helper: '',
  },
  firstname: {
    error: false,
    helper: '',
  },
  lastname: {
    error: false,
    helper: '',
  },
  address: {
    error: false,
    helper: '',
  },
  birthdate: {
    error: false,
    helper: '',
  },
  role: {
    error: false,
    helper: '',
  },
  imageUrl: {
    error: false,
    helper: '',
  },
};

const validateFields = (user) => {
  const updatedFieldValidity = { ...fieldValidity };

  const requiredFields = [
    'username',
    'email',
    'password',
    'repeatPassword',
    'firstname',
    'lastname',
    'address',
    'birthdate',
    'role',
    'imageUrl',
  ];

  requiredFields.forEach((field) => {
    if (!user[field]) {
      updatedFieldValidity[field].error = true;
      updatedFieldValidity[field].helper = 'Field is required';
    } else {
      updatedFieldValidity[field].error = false;
      updatedFieldValidity[field].helper = '';

      // Check username length
      if (field === 'username' && user.username.length < 5) {
        updatedFieldValidity.username.error = true;
        updatedFieldValidity.username.helper = 'Too short';
      }

      // Check email format
      if (field === 'email') {
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        if (!emailRegex.test(user.email)) {
          updatedFieldValidity.email.error = true;
          updatedFieldValidity.email.helper = 'Invalid email format';
        }
      }

      // Check password and repeatPassword
      if (field === 'password' || field === 'repeatPassword') {
        if (!user.password || !user.repeatPassword) {
          updatedFieldValidity.password.error = true;
          updatedFieldValidity.password.helper = 'Password is required';
          updatedFieldValidity.repeatPassword.error = true;
          updatedFieldValidity.repeatPassword.helper = 'Password is required';
        } else if (user.password !== user.repeatPassword) {
          updatedFieldValidity.password.error = true;
          updatedFieldValidity.password.helper = 'Passwords do not match';
          updatedFieldValidity.repeatPassword.error = true;
          updatedFieldValidity.repeatPassword.helper = 'Passwords do not match';
        }
      }

      // Check birthdate is in the future
      if (field === 'birthdate') {
        const today = new Date();
        const birthdate = new Date(user.birthdate);
        if (birthdate > today) {
          updatedFieldValidity.birthdate.error = true;
          updatedFieldValidity.birthdate.helper =
            'Birthdate must be in the past';
        }
      }

      // Additional field-specific validations can be added here
    }
  });

  return updatedFieldValidity;
};

export default Register;
