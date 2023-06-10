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

function App() {
  const { loadUser, ...userContext } = useContext(UserContext);

  useEffect(() => {
    loadUser();
  }, [loadUser]);

  const isLoggedin = userContext.isLoggedin;
  //const isApprovedSeller =
  //isLoggedin && userContext.role === 'Seller' && userContext.status;
  const role = isLoggedin && userContext.role.toLowerCase();

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
        {role === 'admin' && (
          <Route path='/all-orders' element={<AllOrders />} />
        )}
        <Route path='*' element={<NotFound />} />
      </Routes>
    </Box>
  );
}

export default App;
