import { useEffect, useState, useContext } from 'react';
import { useNavigate } from 'react-router-dom';

import Orders from '../../components/Orders/Orders';
import useServices from '../../services/useServices';
import MyBackdrop from '../../components/MyBackdrop';
import { toasterUtil as toaster } from '../../utils/toasterUtil';
import UserContext from '../../context/UserContext';

const CustomerPendingOrders = () => {
  const {
    data,
    error,
    statusCode,
    isLoading,
    getCustomerPendingOrdersRequest,
    deleteCustomerOrderRequest,
    clearRequest,
  } = useServices();
  const [orders, setOrders] = useState([]);
  const [deletingOrder, setDeletingOrder] = useState(false);
  const { role } = useContext(UserContext);
  const navigate = useNavigate();

  useEffect(() => {
    getCustomerPendingOrdersRequest();
  }, [getCustomerPendingOrdersRequest]);

  useEffect(() => {
    if (isLoading) {
      return;
    } else if (statusCode === 200 && !error && data) {
      setOrders(data.orders);
      clearRequest();
    } else if (statusCode === 200 && !error && deletingOrder) {
      toaster.handleSuccess('Order canceled!');
      setDeletingOrder(false);
      getCustomerPendingOrdersRequest();
      clearRequest();
    } else if (statusCode !== 200 && error) {
      toaster.handleError(statusCode, error);
    }
  }, [
    isLoading,
    statusCode,
    error,
    data,
    clearRequest,
    getCustomerPendingOrdersRequest,
    deletingOrder,
  ]);

  const handleButton = (id) => {
    navigate('/pending-orders/' + id);
  };

  const handleOrderCancel = (orderId) => {
    setDeletingOrder(true);
    deleteCustomerOrderRequest(orderId);
  };

  return (
    <>
      {!isLoading && (
        <Orders
          data={orders}
          role={role}
          hasButton={true}
          buttonCallback={handleButton}
          buttonText='Details'
          cancelOrderCallback={handleOrderCancel}
        ></Orders>
      )}
      <MyBackdrop open={isLoading}></MyBackdrop>
    </>
  );
};

export default CustomerPendingOrders;
