import { useState, useEffect, useContext } from 'react';

import {
  Button,
  Container,
  Paper,
  TextField,
  Typography,
  Box,
  FormControl,
} from '@mui/material';

import Calendar from '../../components/Calendar';
import { getIsoString } from '../../utils/dateTimeUtils';
import MyBackdrop from '../../components/MyBackdrop';
import useServices from '../../services/useServices';

import styles from '../../style/centerFormStyles';
import { toasterUtil as toaster } from '../../utils/toasterUtil';
import UploadButton from '../../components/UploadButton';
import UserContext from '../../context/UserContext';

const Profile = () => {
  const { role, status } = useContext(UserContext);
  const [basicUserInfo, setBasicUserInfo] = useState(basicUserInit);
  const [passwordInfo, setPasswordInfo] = useState(passwordInfoInit);
  const [profileImage, setProfileImage] = useState(null);

  const [validity, setValidity] = useState(fieldValidityInit);
  const [passwordValidity, setPasswordValidity] = useState(
    passwordFieldValidityInitInit
  );

  const [fetchingProfile, setFetchingProfile] = useState(true);
  const [updatingProfile, setUpdatingProfile] = useState(false);
  const [fetchingProfileImage, setFetchingProfileImage] = useState(false);
  const [updatingProfileImage, setUpdatingProfileImage] = useState(false);
  const [changingPassword, setChangingPassword] = useState(false);

  const {
    getUserProfileRequest,
    getProfileImageRequest,
    updateUserRequest,
    clearRequest,
    changePasswordRequest,
    updateProfileImageRequest,
    isLoading,
    data,
    error,
    statusCode,
  } = useServices();

  const handleProfileUpdate = (event) => {
    event.preventDefault();

    setValidity(validateFields(basicUserInfo));

    for (const field in validity) {
      if (validity[field].error) {
        return;
      }
    }

    setUpdatingProfile(true);
    updateUserRequest(basicUserInfo);
  };

  const handlePasswordUpdate = (event) => {
    event.preventDefault();

    setPasswordValidity(validatePassword(passwordInfo));

    for (const field in passwordValidity) {
      if (passwordValidity[field].error) {
        return;
      }
    }

    setChangingPassword(true);
    changePasswordRequest(passwordInfo);
  };

  useEffect(() => {
    getUserProfileRequest();
  }, [getUserProfileRequest]);

  useEffect(() => {
    if (isLoading) {
      return;
    }

    if (statusCode === 200 && !error && data && fetchingProfile) {
      setFetchingProfile(false);
      setBasicUserInfo(data);
      clearRequest();
      setFetchingProfileImage(true);
      getProfileImageRequest();
    }

    if (statusCode === 200 && !error && data && fetchingProfileImage) {
      setFetchingProfileImage(false);
      setProfileImage('data:image/*;base64,' + data.profileImage);
      clearRequest();
    }

    if (statusCode === 200 && !error && updatingProfile) {
      setUpdatingProfile(false);
      setFetchingProfile(true);
      getUserProfileRequest();
      toaster.handleSuccess('Successfully updated profile!');
      clearRequest();
    } else if (statusCode === 200 && !error && changingPassword) {
      setChangingPassword(false);
      toaster.handleSuccess('Successfully changed password!');
      clearRequest();
    } else if (statusCode === 200 && !error && updatingProfileImage) {
      setUpdatingProfileImage(false);
      toaster.handleSuccess('Successfully updated profile image!');
      clearRequest();
    } else if (statusCode !== 200 && error && updatingProfileImage) {
      setUpdatingProfileImage(false);
      setFetchingProfileImage(true);
      getProfileImageRequest();
      toaster.handleError(statusCode, error);
      clearRequest();
    } else if (statusCode !== 200 && error) {
      toaster.handleError(statusCode, error);
      clearRequest();
    }
  }, [
    isLoading,
    statusCode,
    error,
    data,
    fetchingProfile,
    clearRequest,
    getUserProfileRequest,
    updatingProfile,
    getProfileImageRequest,
    changingPassword,
    fetchingProfileImage,
    updatingProfileImage,
  ]);

  return (
    <>
      <Container
        sx={{
          ...styles.container,
          flexDirection: 'column',
          gap: '30px',
          alignItems: 'center',
        }}
      >
        <Box sx={{ ...styles.rowBox, justifyContent: 'center', gap: '40px' }}>
          <Box
            sx={{
              display: 'flex',
              flexDirection: 'column',
              gap: '25px',
              justifyItems: 'center',
              alignItems: 'center',
              width: '30%',
              borderRadius: '3px',
              padding: '10px',
              boxShadow:
                '0px 2px 4px -1px rgba(0,0,0,0.2), 0px 4px 5px 0px rgba(0,0,0,0.14), 0px 1px 10px 0px rgba(0,0,0,0.12)',
              margin: '0px',
            }}
          >
            <Typography sx={{ fontSize: '24px', padding: '0px' }}>
              Profile image
            </Typography>
            <UploadButton
              width='100%'
              maxHeightPerc='250px'
              maxWidthPerc='100%'
              image={profileImage}
              buttonText='Reupload'
              direction='column'
              alternativeToNoImage='No profile image...'
              doubleClickCallback={() => {
                setProfileImage(null);
                setUpdatingProfileImage(true);
                updateProfileImageRequest({
                  profileImage: null,
                });
              }}
              uploadCallback={(file) => {
                setProfileImage(file);
                setUpdatingProfileImage(true);
                updateProfileImageRequest({
                  profileImage: file,
                });
              }}
            />
          </Box>
          <Paper
            component='form'
            sx={{ ...styles.paper, width: '50%' }}
            elevation={4}
          >
            <Box
              sx={{ ...styles.rowBox, justifyContent: 'center', gap: '0px' }}
            >
              <Typography variant='h5' paddingBottom='15px'>
                {role.toUpperCase()}
              </Typography>
              {role.toLowerCase() === 'seller' &&
                status?.toLowerCase() !== 'approved' && (
                  <Typography
                    variant='h5'
                    color='secondary'
                    paddingBottom='15px'
                  >
                    (Not approved!)
                  </Typography>
                )}
              {role.toLowerCase() === 'seller' &&
                status?.toLowerCase() === 'approved' && (
                  <Typography variant='h5' color='primary' paddingBottom='15px'>
                    (Approved)
                  </Typography>
                )}
            </Box>
            <Box sx={styles.rowBox}>
              <TextField
                placeholder='Username'
                id='username'
                label='Username'
                value={basicUserInfo.username}
                error={validity.username.error}
                helperText={validity.username.helper}
                sx={styles.textField}
                onChange={(e) => {
                  setBasicUserInfo((old) => {
                    return { ...old, username: e.target.value };
                  });
                }}
                required
              />
              <FormControl sx={{ width: '50%', margin: '0px' }}>
                <TextField
                  placeholder='Email'
                  id='email'
                  label='Email'
                  disabled
                  value={basicUserInfo.email}
                  sx={{
                    ...styles.textField,
                    width: '100%',
                    // '.MuiInputBase-input.Mui-disabled': {
                    //   WebkitTextFillColor: 'white',
                    // },
                  }}
                  type='email'
                  aria-readonly='true'
                  variant='outlined'
                />
              </FormControl>
            </Box>
            <Box sx={styles.rowBox}>
              <TextField
                placeholder='Firstname'
                id='firstname'
                label='Firstname'
                value={basicUserInfo.firstname}
                error={validity.firstname.error}
                helperText={validity.firstname.helper}
                sx={styles.textField}
                onChange={(e) => {
                  setBasicUserInfo((old) => {
                    return { ...old, firstname: e.target.value };
                  });
                }}
                required
              />
              <TextField
                placeholder='Lastname'
                id='lastname'
                label='Lastname'
                value={basicUserInfo.lastname}
                error={validity.lastname.error}
                helperText={validity.lastname.helper}
                sx={styles.textField}
                onChange={(e) => {
                  setBasicUserInfo((old) => {
                    return { ...old, lastname: e.target.value };
                  });
                }}
                required
              />
            </Box>
            <Box sx={styles.rowBox}>
              <TextField
                placeholder='Address'
                id='address'
                label='Address'
                value={basicUserInfo.address}
                error={validity.address.error}
                helperText={validity.address.helper}
                sx={styles.textField}
                onChange={(e) => {
                  setBasicUserInfo((old) => {
                    return { ...old, address: e.target.value };
                  });
                }}
                required
              />
              <Calendar
                label='Birthdate'
                id='birthdate'
                sx={styles.textField}
                value={basicUserInfo.birthdate}
                error={validity.birthdate.error}
                helperText={validity.birthdate.helper}
                disableFuture={true}
                callback={(date) => {
                  date = date == null ? null : getIsoString(date);
                  setBasicUserInfo((old) => {
                    return { ...old, birthdate: date };
                  });
                }}
              />
            </Box>
            <Button
              sx={{ width: '30%', marginTop: '15px', alignSelf: 'center' }}
              variant='contained'
              type='submit'
              onClick={(event) => {
                handleProfileUpdate(event);
              }}
            >
              Update
            </Button>
          </Paper>
        </Box>
        <Paper
          component='form'
          sx={{ marginTop: '10px', ...styles.paper, width: '30%' }}
          elevation={4}
        >
          <TextField
            placeholder='Old password'
            id='OldPassword'
            type='password'
            sx={{ width: '100%' }}
            value={passwordInfo.oldPassword}
            error={passwordValidity.oldPassword.error}
            helperText={passwordValidity.oldPassword.helper}
            onChange={(e) => {
              setPasswordInfo((old) => {
                return { ...old, oldPassword: e.target.value };
              });
            }}
            required
          />
          <TextField
            placeholder='New password'
            id='newPassword'
            type='password'
            sx={{ width: '100%' }}
            value={passwordInfo.newPassword}
            error={passwordValidity.newPassword.error}
            helperText={passwordValidity.newPassword.helper}
            onChange={(e) => {
              setPasswordInfo((old) => {
                return { ...old, newPassword: e.target.value };
              });
            }}
            required
          />
          <TextField
            placeholder='Repeat new password'
            id='repeatNewPassword'
            type='password'
            sx={{ width: '100%' }}
            value={passwordInfo.repeatNewPassword}
            error={passwordValidity.repeatNewPassword.error}
            helperText={passwordValidity.repeatNewPassword.helper}
            onChange={(e) => {
              setPasswordInfo((old) => {
                return { ...old, repeatNewPassword: e.target.value };
              });
            }}
            required
          />
          <Button
            sx={{ width: '70%', marginTop: '15px', alignSelf: 'center' }}
            variant='contained'
            type='submit'
            onClick={(event) => {
              handlePasswordUpdate(event);
            }}
          >
            Change password
          </Button>
        </Paper>
      </Container>
      <MyBackdrop open={isLoading} />
    </>
  );
};

var basicUserInit = {
  username: '',
  email: '',
  firstname: '',
  lastname: '',
  address: '',
  birthdate: '',
};

const passwordInfoInit = {
  oldPassword: '',
  newPassword: '',
  repeatNewPassword: '',
};

const fieldValidityInit = {
  username: {
    error: false,
    helper: '',
  },
  email: {
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
};

const passwordFieldValidityInitInit = {
  oldPassword: {
    error: false,
    helper: '',
  },
  newPassword: {
    error: false,
    helper: '',
  },
  repeatNewPassword: {
    error: false,
    helper: '',
  },
};

const validateFields = (user) => {
  const updatedFieldValidityInit = { ...fieldValidityInit };

  const requiredFields = Object.keys(fieldValidityInit);

  requiredFields.forEach((field) => {
    if (!user[field]) {
      updatedFieldValidityInit[field].error = true;
      updatedFieldValidityInit[field].helper = 'Field is required';
    } else if (user[field].length < 3) {
      updatedFieldValidityInit[field].error = true;
      updatedFieldValidityInit[field].helper = 'Too short';
    } else {
      updatedFieldValidityInit[field].error = false;
      updatedFieldValidityInit[field].helper = '';

      // Check email format
      if (field === 'email') {
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        if (!emailRegex.test(user.email)) {
          updatedFieldValidityInit.email.error = true;
          updatedFieldValidityInit.email.helper = 'Invalid email format';
        }
      }

      // Check birthdate is in the future
      if (field === 'birthdate') {
        const today = new Date();
        const birthdate = new Date(user.birthdate);
        if (birthdate > today) {
          updatedFieldValidityInit.birthdate.error = true;
          updatedFieldValidityInit.birthdate.helper =
            'Birthdate must be in the past';
        }
      }
    }
  });

  return updatedFieldValidityInit;
};

const validatePassword = (user) => {
  const updatePasswordFieldValidityInit = { ...passwordFieldValidityInitInit };

  const requiredFields = Object.keys(passwordFieldValidityInitInit);

  requiredFields.forEach((field) => {
    if (!user[field]) {
      updatePasswordFieldValidityInit[field].error = true;
      updatePasswordFieldValidityInit[field].helper = 'Field is required';
    } else if (user[field].length < 3) {
      updatePasswordFieldValidityInit[field].error = true;
      updatePasswordFieldValidityInit[field].helper = 'Too short';
    } else {
      updatePasswordFieldValidityInit[field].error = false;
      updatePasswordFieldValidityInit[field].helper = '';

      // Check password and repeatPassword
      if (user.newPassword !== user.repeatNewPassword) {
        updatePasswordFieldValidityInit.newPassword.error = true;
        updatePasswordFieldValidityInit.newPassword.helper =
          'Passwords do not match';
        updatePasswordFieldValidityInit.repeatNewPassword.error = true;
        updatePasswordFieldValidityInit.repeatNewPassword.helper =
          'Passwords do not match';
      }
    }
  });

  return updatePasswordFieldValidityInit;
};

export default Profile;
