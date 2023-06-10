import { useCallback } from 'react';
import useHttp from '../hooks/useHttp';

const baseUrl = process.env.REACT_APP_API_BASE_URL;
const registerUrl = baseUrl + process.env.REACT_APP_API_REGISTER_URL;
const loginUrl = baseUrl + process.env.REACT_APP_API_LOGIN_URL;
const getUserUrl = baseUrl + process.env.REACT_APP_API_GET_USER_URL;
const updateUserUrl = baseUrl + process.env.REACT_APP_API_UPDATE_USER_URL;
const changePasswordUrl =
  baseUrl + process.env.REACT_APP_API_CHANGE_PASSWORD_URL;
const getProfileImageUrl =
  baseUrl + process.env.REACT_APP_API_GET_PROFILE_IMAGE_URL;
const changeProfileImageUrl =
  baseUrl + process.env.REACT_APP_API_CHANGE_PROFILE_IMAGE_URL;
const getAllSellersUrl =
  baseUrl + process.env.REACT_APP_API_ADMIN_ALL_SELLERS_URL;
const updateSellerStatusUrl =
  baseUrl + process.env.REACT_APP_API_UPDATE_APPROVAL_STATUS_URL;
const allOrdersUrl = baseUrl + process.env.REACT_APP_API_ALL_ORDERS_URL;

const useServices = () => {
  const {
    data,
    isLoading,
    error,
    statusCode,
    getRequest,
    postRequest,
    postRequestFormData,
    putRequest,
    putRequestFormData,
    resetHttp,
  } = useHttp();

  const registerRequest = useCallback(
    (user) => {
      postRequestFormData(registerUrl, user);
    },
    [postRequestFormData]
  );

  const loginRequest = useCallback(
    (credentials) => {
      postRequest(loginUrl, credentials);
    },
    [postRequest]
  );

  const getUserProfileRequest = useCallback(() => {
    getRequest(getUserUrl);
  }, [getRequest]);

  const updateUserRequest = useCallback(
    (user) => {
      putRequest(updateUserUrl, user);
    },
    [putRequest]
  );

  const changePasswordRequest = useCallback(
    (data) => {
      putRequest(changePasswordUrl, data);
    },
    [putRequest]
  );

  const getProfileImageRequest = useCallback(() => {
    getRequest(getProfileImageUrl);
  }, [getRequest]);

  const updateProfileImageRequest = useCallback(
    (data) => {
      putRequestFormData(changeProfileImageUrl, data);
    },
    [putRequestFormData]
  );

  const getAllSellersRequest = useCallback(() => {
    getRequest(getAllSellersUrl);
  }, [getRequest]);

  const updateSellerStatusRequest = useCallback(
    (data) => {
      putRequest(updateSellerStatusUrl, data);
    },
    [putRequest]
  );

  const getAllOrdersRequest = useCallback(() => {
    getRequest(allOrdersUrl);
  }, [getRequest]);

  return {
    data,
    isLoading,
    error,
    statusCode,
    clearRequest: resetHttp,
    registerRequest,
    loginRequest,
    getUserProfileRequest,
    updateUserRequest,
    changePasswordRequest,
    getProfileImageRequest,
    updateProfileImageRequest,
    getAllSellersRequest,
    updateSellerStatusRequest,
    getAllOrdersRequest,
  };
};

export default useServices;
