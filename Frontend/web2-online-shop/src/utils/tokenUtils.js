import jwtDecode from 'jwt-decode';
import { error } from './toasterUtil';

// Returns decoded token
export const getToken = () => {
  const token = window.localStorage.getItem('token');

  if (!token) {
    return null;
  }

  try {
    const decoded = jwtDecode(token);

    return decoded;
  } catch (exception) {
    error(exception.message);
  }
};

export const isLoggedin = () => {
  const token = window.localStorage.getItem('token');

  if (!token) {
    return false;
  }

  const decoded = getToken();

  if (!decoded) {
    return false;
  }

  return true;
};

// Gets token JSON object
export const getRawToken = () => {
  const token = window.localStorage.getItem('token');
  const tokenJson = token && JSON.parse(token);

  return tokenJson;
};

export const getRole = () => {
  const token = getToken();

  return token?.role;
};

export const getId = () => {
  const token = getToken();

  return token?.id;
};

export const getUsername = () => {
  const token = getToken();

  return token?.username;
};

export const getUser = () => {
  const user = {
    username: getUsername(),
    role: getRole(),
    id: getId(),
    rawToken: getRawToken(),
  };

  return user;
};

// Takes token as Object and saves it as stringified JSON
export const saveToken = (tokenObj) => {
  if (!tokenObj) {
    return;
  }

  const token = JSON.stringify(tokenObj);
  window.localStorage.setItem('token', token);
};

export const removeToken = () => {
  window.localStorage.removeItem('token');
};

const tokenUtils = {
  isLoggedin,
  getToken,
  getRawToken,
  getRole,
  getId,
  getUsername,
  getUser,
  saveToken,
  removeToken,
};

export default tokenUtils;
