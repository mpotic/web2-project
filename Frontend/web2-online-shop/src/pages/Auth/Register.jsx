import { useState, useEffect, useRef } from 'react';
import { Link as RouterLink } from 'react-router-dom';
import { useNavigate } from 'react-router-dom';

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

import styles from '../../style/centerFormStyles';

import Calendar from '../../components/Calendar';
import UploadButtons from '../../components/UploadButton';
import { formatDateTime } from '../../utils/dateTimeUtils';
import MyBackdrop from '../../components/MyBackdrop';
import useServices from '../../services/useServices';
import { toasterUtil as toaster } from '../../utils/toasterUtil';

const Register = () => {
  const user = useRef(userInit);
  const [isAddedImage, setIsAddedImage] = useState(false);
  const [isSelected, setIsSelected] = useState(false);
  const [validity, setValidity] = useState(fieldValidity);
  const { registerRequest, isLoading, error, statusCode } = useServices();
  const navigate = useNavigate();

  const handleSubmit = (event) => {
    event.preventDefault();

    setValidity(validateFields(user.current));

    for (const field in validity) {
      if (validity[field].error) {
        return;
      }
    }

    registerRequest(user.current);
  };

  useEffect(() => {
    if (isLoading) {
      return;
    } else if (statusCode === 200 && !error) {
      toaster.handleSuccess('Successfully registered!');
      navigate('/login');
    } else if (statusCode !== 200 && error) {
      toaster.handleError(statusCode, error);
    }
  }, [isLoading, statusCode, error, navigate]);

  return (
    <>
      <Container sx={{ ...styles.container }}>
        <Paper component='form' sx={{ ...styles.paper }} elevation={4}>
          <Box sx={styles.rowBox}>
            <TextField
              placeholder='Username'
              id='username'
              error={validity.username.error}
              helperText={validity.username.helper}
              sx={styles.textField}
              onChange={(e) => {
                user.current.username = e.target.value;
              }}
              required
            />
            <TextField
              placeholder='Email'
              id='email'
              error={validity.email.error}
              helperText={validity.email.helper}
              sx={styles.textField}
              type='email'
              onChange={(e) => {
                user.current.email = e.target.value;
              }}
              required
            />
          </Box>
          <Box sx={styles.rowBox}>
            <TextField
              placeholder='Password'
              id='password'
              type='password'
              error={validity.password.error}
              helperText={validity.password.helper}
              sx={styles.textField}
              onChange={(e) => {
                user.current.password = e.target.value;
              }}
              required
            />
            <TextField
              placeholder='Repeat password'
              id='repeat-password'
              type='password'
              error={validity.repeatPassword.error}
              helperText={validity.repeatPassword.helper}
              sx={styles.textField}
              onChange={(e) => {
                user.current.repeatPassword = e.target.value;
              }}
              required
            />
          </Box>
          <Box sx={styles.rowBox}>
            <TextField
              placeholder='Firstname'
              id='firstname'
              error={validity.firstname.error}
              helperText={validity.firstname.helper}
              sx={styles.textField}
              onChange={(e) => {
                user.current.firstname = e.target.value;
              }}
              required
            />
            <TextField
              placeholder='Lastname'
              id='lastname'
              error={validity.lastname.error}
              helperText={validity.lastname.helper}
              sx={styles.textField}
              onChange={(e) => {
                user.current.lastname = e.target.value;
              }}
              required
            />
          </Box>
          <Box sx={styles.rowBox}>
            <TextField
              placeholder='Address'
              id='address'
              error={validity.address.error}
              helperText={validity.address.helper}
              sx={styles.textField}
              onChange={(e) => {
                user.current.address = e.target.value;
              }}
              required
            />
            <Calendar
              label='Birthdate'
              id='birthdate'
              sx={styles.textField}
              error={validity.birthdate.error}
              helperText={validity.birthdate.helper}
              disableFuture={true}
              callback={(date) => {
                date = date == null ? null : formatDateTime(date);
                user.current.birthdate = date;
              }}
            />
          </Box>
          <Box sx={{ ...styles.rowBox }}>
            <FormControl
              sx={{
                width: isAddedImage ? '30%' : '50%',
              }}
            >
              <InputLabel id='select-role-label' error={validity.role.error}>
                Role
              </InputLabel>
              <Select
                labelId='select-role-label'
                label='Role'
                id='role'
                value={user.current.role}
                error={validity.role.error}
                helpertext={validity.role.helper}
                onChange={(e) => {
                  user.current.role = e.target.value;
                  setIsSelected(!isSelected);
                }}
              >
                <MenuItem value='Customer'>Customer</MenuItem>
                <MenuItem value='Seller'>Seller</MenuItem>
              </Select>
            </FormControl>
            <UploadButtons
              width={isAddedImage ? '70%' : '50%'}
              maxHeightPerc='55%'
              maxWidthPerc='55%'
              id='profileImage'
              uploadCallback={(file) => {
                user.current.profileImage = file;
                setIsAddedImage(!!file);
              }}
              doubleClickCallback={(file) => {
                user.current.profileImage = file;
                setIsAddedImage(!!file);
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
      <MyBackdrop open={isLoading} />
    </>
  );
};

var userInit = {
  username: '',
  password: '',
  repeatPassword: '',
  email: '',
  firstname: '',
  lastname: '',
  address: '',
  birthdate: '',
  role: '',
  profileImage: null,
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

      // Check email format
      if (field === 'email') {
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        if (!emailRegex.test(user.email)) {
          updatedFieldValidity.email.error = true;
          updatedFieldValidity.email.helper = 'Invalid email format';
        }
      }

      // Check password and repeatPassword
      if (user.password !== user.repeatPassword) {
        updatedFieldValidity.password.error = true;
        updatedFieldValidity.password.helper = 'Passwords do not match';
        updatedFieldValidity.repeatPassword.error = true;
        updatedFieldValidity.repeatPassword.helper = 'Passwords do not match';
      }

      // Check if birthdate is in the future
      if (field === 'birthdate') {
        const today = new Date();
        const birthdate = new Date(user.birthdate);
        if (birthdate > today) {
          updatedFieldValidity.birthdate.error = true;
          updatedFieldValidity.birthdate.helper =
            'Birthdate must be in the past';
        }
      }
    }
  });

  return updatedFieldValidity;
};

export default Register;
