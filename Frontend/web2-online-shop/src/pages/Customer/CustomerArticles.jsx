import { useEffect, useState, useContext } from 'react';

import useServices from '../../services/useServices';
import MyBackdrop from '../../components/MyBackdrop';
import { toasterUtil as toaster } from '../../utils/toasterUtil';
import UserContext from '../../context/UserContext';
import Articles from '../../components/Articles/Articles';
import OrderContext from '../../context/OrderContext';

const SellersArticles = () => {
  const {
    data,
    error,
    statusCode,
    isLoading,
    getCustomerArticlesRequest,
    clearRequest,
  } = useServices();
  const [articles, setArticles] = useState([]);
  const { role } = useContext(UserContext);
  const orderContext = useContext(OrderContext);

  useEffect(() => {
    getCustomerArticlesRequest();
  }, [getCustomerArticlesRequest]);

  useEffect(() => {
    if (isLoading) {
      return;
    } else if (statusCode === 200 && !error && data) {
      data?.articles.forEach((item) => {
        item.productImage = 'data:image/*;base64,' + item.productImage;
      });
      setArticles(data.articles);
      clearRequest();
    } else if (statusCode !== 200 && error) {
      toaster.handleError(statusCode, error);
    }
  }, [isLoading, statusCode, error, data, clearRequest]);

  const handleButton = (article) => {
    orderContext.addItemToOrder(article, 1);
  };

  return (
    <>
      {!isLoading && (
        <Articles
          data={articles}
          role={role}
          hasButton={true}
          buttonCallback={handleButton}
          buttonText='Add to order'
        ></Articles>
      )}
      <MyBackdrop open={isLoading}></MyBackdrop>
    </>
  );
};

export default SellersArticles;
