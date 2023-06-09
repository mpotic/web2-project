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

import toasterStyle from './style/ToasterStyle.module.css';
import UserContext from './context/UserContext';

function App() {
  const userContext = useContext(UserContext);

  useEffect(() => {
    userContext.loadUser();
  }, []);

  return (
    <Box sx={{ width: '100%', height: '100%' }}>
      <ToastContainer
        autoClose={3000}
        toastClassName={`${toasterStyle['toaster-customization']}`}
      />
      <Navbar />
      <Routes>
        <Route path='/' element={<Home />} />
        <Route path='/login' element={<Login />} />
        <Route path='/register' element={<Register />} />
        <Route path='*' element={<NotFound />} />
      </Routes>
    </Box>
  );
}

export default App;
