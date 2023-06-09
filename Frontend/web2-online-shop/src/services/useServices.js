import useHttp from '../hooks/useHttp';

const useServices = () => {
  const http = useHttp();
  const baseUrl = process.env.REACT_APP_API_BASE_URL;
  const registerUrl = baseUrl + process.env.REACT_APP_API_REGISTER_URL;
  const loginUrl = baseUrl + process.env.REACT_APP_API_LOGIN_URL;
  const getUserUrl = baseUrl + process.env.REACT_APP_API_GET_USER_URL;

  const registerRequest = (user) => {
    http.postRequestFormData(registerUrl, user);
  };

  const loginRequest = (credentials) => {
    http.postRequest(loginUrl, credentials);
  };

  const getUserProfileRequest = () => {
    http.getRequest(getUserUrl);
  };

  return {
    data: http.data,
    isLoading: http.isLoading,
    error: http.error,
    statusCode: http.statusCode,
    registerRequest,
    loginRequest,
    getUserProfileRequest,
  };
};

export default useServices;
