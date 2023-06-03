export function isLoggedin() {
  return window.localStorage.getItem('token') == null ? false : true;
}

export function getToken() {
  return window.localStorage.getItem('token');
}

const userHelper = {
  isLoggedin,
  getToken,
};

export default userHelper;
