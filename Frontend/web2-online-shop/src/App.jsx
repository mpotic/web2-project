import { Routes, Route } from 'react-router-dom';
import { useContext, useEffect } from 'react';

import { Box } from '@mui/material';
import { ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';

import Navbar from './components/Navbar';
import Home from './pages/Home';
import Login from './pages/Auth/Login';
import Register from './pages/Auth/Register';
import NotFound from './pages/NotFound';
import Profile from './pages/Common/Profile';

import toasterStyle from './style/ToasterStyle.module.css';
import UserContext from './context/UserContext';
import AllSellers from './pages/Admin/AllSellers';
import AllOrders from './pages/Admin/AllOrders';
import OrderDetails from './components/Orders/OrderDetails';
import SellersArticles from './pages/Seller/SellersArticles';
import SellersFinishedOrders from './pages/Seller/SellersFinishedOrders';
import SellersPendingOrders from './pages/Seller/SellersPendingOrders';
import ArticleDetails from './components/Articles/ArticleDetails';
import NewArticle from './pages/Seller/NewArticle';
import CustomerPendingOrders from './pages/Customer/CustomerPendingOrders';
import CustomerFinishedOrders from './pages/Customer/CustomerFinishedOrders';
import CustomerArticles from './pages/Customer/CustomerArticles';
import OrderContext from './context/OrderContext';
import Order from './pages/Customer/Order';

function App() {
  const { loadUser, ...userContext } = useContext(UserContext);
  const { removeOrder, ...orderContext } = useContext(OrderContext);

  const isLoggedin = userContext.isLoggedin;
  const role = isLoggedin && userContext.role.toLowerCase();
  const approvedSeller =
    role === 'seller' && userContext.status?.toLowerCase() === 'approved';

  useEffect(() => {
    loadUser();
  }, [loadUser]);

  useEffect(() => {
    if (!isLoggedin || role !== 'customer') {
      removeOrder();
    }
  }, [isLoggedin, removeOrder, role]);

  return (
    <Box sx={{ width: '100%', height: '100%' }}>
      <ToastContainer
        autoClose={3000}
        toastClassName={`${toasterStyle['toaster-customization']}`}
      />
      <Navbar />
      <Routes>
        <Route path='/' element={<Home />} />
        {!isLoggedin && <Route path='/login' element={<Login />} />}
        {isLoggedin && <Route path='/login' element={<Home />} />}
        {!isLoggedin && <Route path='/register' element={<Register />} />}
        {isLoggedin && <Route path='/register' element={<Home />} />}
        {isLoggedin && <Route path='/profile' element={<Profile />} />}
        {role === 'admin' && (
          <Route path='/all-sellers' element={<AllSellers />} />
        )}
        {role === 'admin' && <Route path='/orders' element={<AllOrders />} />}
        {role === 'admin' && (
          <Route path='/orders/:id' element={<OrderDetails />} />
        )}
        {approvedSeller && (
          <Route path='/articles' element={<SellersArticles />} />
        )}
        {approvedSeller && (
          <Route path='/finished-orders' element={<SellersFinishedOrders />} />
        )}
        {approvedSeller && (
          <Route path='/finished-orders/:id' element={<OrderDetails />} />
        )}
        {approvedSeller && (
          <Route path='/pending-orders' element={<SellersPendingOrders />} />
        )}
        {approvedSeller && (
          <Route path='/pending-orders/:id' element={<OrderDetails />} />
        )}
        {approvedSeller && (
          <Route path='/articles/:name' element={<ArticleDetails />} />
        )}
        {approvedSeller && (
          <Route path='/new-article' element={<NewArticle />} />
        )}
        {role === 'customer' && (
          <Route path='/articles' element={<CustomerArticles />} />
        )}
        {role === 'customer' && (
          <Route path='/finished-orders' element={<CustomerFinishedOrders />} />
        )}
        {role === 'customer' && (
          <Route path='/finished-orders/:id' element={<OrderDetails />} />
        )}
        {role === 'customer' && (
          <Route path='/pending-orders' element={<CustomerPendingOrders />} />
        )}
        {role === 'customer' && (
          <Route path='/pending-orders/:id' element={<OrderDetails />} />
        )}
        {role === 'customer' && orderContext.hasItems() && (
          <Route path='/order' element={<Order />} />
        )}
        <Route path='*' element={<NotFound />} />
      </Routes>
    </Box>
  );
}

export default App;
