import { useEffect, useState, useContext } from 'react';

import Orders from '../Orders/Orders';
import useServices from '../../services/useServices';
import MyBackdrop from '../../components/MyBackdrop';
import { toasterUtil as toaster } from '../../utils/toasterUtil';
import UserContext from '../../context/UserContext';

const AllOrders = () => {
  const {
    data,
    error,
    statusCode,
    isLoading,
    getAllOrdersRequest,
    clearRequest,
  } = useServices();
  const [orders, setOrders] = useState([]);
  const { role } = useContext(UserContext);

  useEffect(() => {
    getAllOrdersRequest();
  }, [getAllOrdersRequest]);

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

  return (
    <>
      {!isLoading && (
        <Orders data={orders} role={role} hasButton={false}></Orders>
      )}
      <MyBackdrop open={isLoading}></MyBackdrop>
    </>
  );
};

export default AllOrders;
