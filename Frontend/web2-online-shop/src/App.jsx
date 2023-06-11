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
import BuyersArticles from './pages/Buyer/BuyersArticles';
import BuyersFinishedOrders from './pages/Buyer/BuyersFinishedOrders';
import BuyersPendingOrders from './pages/Buyer/BuyersPendingOrders';

function App() {
  const { loadUser, ...userContext } = useContext(UserContext);

  useEffect(() => {
    loadUser();
  }, [loadUser]);

  const isLoggedin = userContext.isLoggedin;
  const role = isLoggedin && userContext.role.toLowerCase();
  const approvedSeller =
    role === 'seller' && userContext.status?.toLowerCase() === 'approved';

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
          <Route path='/articles' element={<BuyersArticles />} />
        )}
        {role === 'customer' && (
          <Route path='/finished-orders' element={<BuyersFinishedOrders />} />
        )}
        {role === 'customer' && (
          <Route path='/pending-orders' element={<BuyersPendingOrders />} />
        )}
        <Route path='*' element={<NotFound />} />
      </Routes>
    </Box>
  );
}

export default App;
