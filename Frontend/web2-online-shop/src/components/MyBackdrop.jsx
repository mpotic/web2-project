import { Backdrop, CircularProgress } from '@mui/material';

const MyBackdrop = ({ open }) => {
  return (
    <Backdrop
      sx={{ color: '#fff', zIndex: (theme) => theme.zIndex.drawer + 1 }}
      open={open}
    >
      <CircularProgress color='inherit' size={60} />
    </Backdrop>
  );
};

export default MyBackdrop;
