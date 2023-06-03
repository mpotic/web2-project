import { Routes, Route } from 'react-router-dom';
import Navbar from './components/Navbar';
import { Box } from '@mui/material';
import Home from './pages/Home';
import Login from './pages/Auth/Login';
import Register from './pages/Auth/Register';

function App() {
  return (
    <Box sx={{ width: '100%', height: '100%' }}>
      <Navbar />
      <Routes>
        <Route path='/' element={<Home />} />
        <Route path='/login' element={<Login />} />
        <Route path='/register' element={<Register />} />
      </Routes>
    </Box>
  );
}

export default App;
