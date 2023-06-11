import { useEffect, useState, useContext } from 'react';
import { useNavigate } from 'react-router-dom';

import Orders from '../../components/Orders/Orders';
import useServices from '../../services/useServices';
import MyBackdrop from '../../components/MyBackdrop';
import { toasterUtil as toaster } from '../../utils/toasterUtil';
import UserContext from '../../context/UserContext';

const BuyersFinishedOrders = () => {
  const {
    data,
    error,
    statusCode,
    isLoading,
    getSellersFinishedOrders,
    clearRequest,
  } = useServices();
  const [orders, setOrders] = useState([]);
  const { role } = useContext(UserContext);
  const navigate = useNavigate();

  useEffect(() => {
    getSellersFinishedOrders();
  }, [getSellersFinishedOrders]);

  useEffect(() => {
    if (isLoading) {
      return;
    } else if (statusCode === 200 && !error && data) {
      setOrders(data.orders);
      clearRequest();
    } else if (statusCode !== 200 && error) {
      toaster.handleError(statusCode, error);
    }
  }, [isLoading, statusCode, error, data, clearRequest]);

  const handleButton = (id) => {
    navigate('/finished-orders/' + id);
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
        ></Orders>
      )}
      <MyBackdrop open={isLoading}></MyBackdrop>
    </>
  );
};

export default BuyersFinishedOrders;
