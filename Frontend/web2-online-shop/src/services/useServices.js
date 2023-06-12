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
// const adminOrderDetailsUrl =
//   baseUrl + process.env.REACT_APP_API_ADMIN_ORDER_DETAILS_URL;
const getSellersFinishedOrdersUrl =
  baseUrl + process.env.REACT_APP_API_SELLER_FINISHED_ORDERS_URL;
const getSellersPendingOrdersUrl =
  baseUrl + process.env.REACT_APP_API_SELLER_PENDING_ORDERS_URL;
const getSellersOrderDetailsUrl =
  baseUrl + process.env.REACT_APP_API_ORDER_DETAILS_URL;
const getSellerArticleDetailsUrl =
  baseUrl + process.env.REACT_APP_API_SELLER_GET_ARTICLE_DETAILS_URL;
const getSellerArticlesUrl =
  baseUrl + process.env.REACT_APP_API_SELLER_ARTICLES_URL;
const updateArticleUrl =
  baseUrl + process.env.REACT_APP_API_SELLER_UPDATE_ARTICLE_URL;
const updateArticleProductImageUrl =
  baseUrl + process.env.REACT_APP_API_SELLER_UPDATE_ARTICLE_IMAGE_URL;
const deleteArticleUrl =
  baseUrl + process.env.REACT_APP_API_SELLER_DELETE_ARTICLE_URL;
const postArticleUrl =
  baseUrl + process.env.REACT_APP_API_SELLER_POST_ARTICLE_URL;
const customerFinishedOrdersUrl =
  baseUrl + process.env.REACT_APP_API_CUSTOMER_FINISHED_ORDERS_URL;
const customerPendingOrdersUrl =
  baseUrl + process.env.REACT_APP_API_CUSTOMER_PENDING_ORDERS_URL;
const customerArticlesUrl =
  baseUrl + process.env.REACT_APP_API_CUSTOMER_ARTICLES_URL;
const customerOrderUrl = baseUrl + process.env.REACT_APP_API_CUSTOMER_ORDER_URL;
const customerDeleteOrderUrl =
  baseUrl + process.env.REACT_APP_API_CUSTOMER_DELETE_ORDER;
const customerPostOrderUrl =
  baseUrl + process.env.REACT_APP_API_CUSTOMER_POST_ORDER_URL;
const googleLoginUrl = baseUrl + process.env.REACT_APP_API_GOOGLE_LOGIN;

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
    deleteRequest,
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

  const getAdminOrderDetailsRequest = useCallback(
    (id) => {
      getRequest(baseUrl + '/admin/order' + '?id=' + id);
    },
    [getRequest]
  );

  const getSellersFinishedOrders = useCallback(() => {
    getRequest(getSellersFinishedOrdersUrl);
  }, [getRequest]);

  const getSellersPendingOrders = useCallback(() => {
    getRequest(getSellersPendingOrdersUrl);
  }, [getRequest]);

  const getSellersOrderDetailsRequest = useCallback(
    (id) => {
      getRequest(getSellersOrderDetailsUrl + '?id=' + id);
    },
    [getRequest]
  );

  const getSellerArticleDetailsRequest = useCallback(
    (name) => {
      getRequest(getSellerArticleDetailsUrl + '?name=' + name);
    },
    [getRequest]
  );

  const getSellersArticlesRequest = useCallback(() => {
    getRequest(getSellerArticlesUrl);
  }, [getRequest]);

  const updateArticleRequest = useCallback(
    (article) => {
      console.log(article, updateArticleUrl);
      putRequest(updateArticleUrl, article);
    },
    [putRequest]
  );

  const updateArticleProductImageRequest = useCallback(
    (article) => {
      putRequestFormData(updateArticleProductImageUrl, article);
    },
    [putRequestFormData]
  );

  const postArticleRequest = useCallback(
    (article) => {
      postRequestFormData(postArticleUrl, article);
    },
    [postRequestFormData]
  );

  const getCustomerFinishedOrdersRequest = useCallback(() => {
    getRequest(customerFinishedOrdersUrl);
  }, [getRequest]);

  const getCustomerPendingOrdersRequest = useCallback(() => {
    getRequest(customerPendingOrdersUrl);
  }, [getRequest]);

  const getCustomerArticlesRequest = useCallback(() => {
    getRequest(customerArticlesUrl);
  }, [getRequest]);

  const getCustomerOrderDetailsRequest = useCallback(
    (id) => {
      getRequest(customerOrderUrl + '?id=' + id);
    },
    [getRequest]
  );

  const postCustomerOrderRequest = useCallback(
    (order) => {
      postRequest(customerPostOrderUrl, order);
    },
    [postRequest]
  );

  const deleteCustomerOrderRequest = useCallback(
    (orderId) => {
      deleteRequest(customerDeleteOrderUrl + '?orderId=' + orderId);
    },
    [deleteRequest]
  );

  const deleteArticleRequest = useCallback(
    (name) => {
      deleteRequest(deleteArticleUrl + '?name=' + name);
    },
    [deleteRequest]
  );

  const googleLogin = useCallback(() => {
    postRequest(googleLoginUrl);
  }, [postRequest]);

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
    getAdminOrderDetailsRequest,
    getSellersFinishedOrders,
    getSellersPendingOrders,
    getSellersOrderDetailsRequest,
    getSellerArticleDetailsRequest,
    getSellersArticlesRequest,
    updateArticleProductImageRequest,
    postArticleRequest,
    updateArticleRequest,
    getCustomerFinishedOrdersRequest,
    getCustomerPendingOrdersRequest,
    getCustomerArticlesRequest,
    getCustomerOrderDetailsRequest,
    postCustomerOrderRequest,
    deleteCustomerOrderRequest,
    googleLogin,
    deleteArticleRequest,
  };
};

export default useServices;
