import { useState, useEffect, useContext } from 'react';

import {
  Container,
  Paper,
  Box,
  TextField,
  Typography,
  Button,
} from '@mui/material';
import Table from '@mui/material/Table';
import TableBody from '@mui/material/TableBody';
import TableCell from '@mui/material/TableCell';
import TableContainer from '@mui/material/TableContainer';
import TableHead from '@mui/material/TableHead';
import TableRow from '@mui/material/TableRow';

import styles from '../../style/centerFormStyles';

import NoData from '../NoData';
import OrderContext from '../../context/OrderContext';
import { toasterUtil as toaster } from '../../utils/toasterUtil';
import { useNavigate } from 'react-router-dom';
import useServices from '../../services/useServices';
import MyBackdrop from '../MyBackdrop';

const CustomerOrder = () => {
  const { updateOrder, ...orderContext } = useContext(OrderContext);
  const [order, setOrder] = useState(orderContext.order);
  const {
    postCustomerOrderRequest,
    clearRequest,
    isLoading,
    error,
    statusCode,
  } = useServices();
  const [addressValidity, setAddressValidity] = useState({
    error: false,
    helper: '',
  });
  const navigate = useNavigate();

  useEffect(() => {
    updateOrder(order);
  }, [order, updateOrder]);

  useEffect(() => {
    if (!orderContext.hasItems()) {
      orderContext.removeOrder();
      navigate('/articles');
    }
  }, [orderContext.order, orderContext, navigate]);

  useEffect(() => {
    if (isLoading) {
      return;
    } else if (statusCode === 200 && !error) {
      toaster.handleSuccess('Successfully placed order!');
      orderContext.removeOrder();
      navigate('/pending-orders');
      clearRequest();
    } else if (statusCode !== 200 && error) {
      toaster.handleError(statusCode, error);
      clearRequest();
    }
  }, [isLoading, error, statusCode, navigate, orderContext, clearRequest]);

  const validate = () => {
    if (order.address.length < 5) {
      setAddressValidity({ error: true, helper: 'Too short' });
      return false;
    } else {
      setAddressValidity({
        error: false,
        helper: '',
      });
    }

    if (Object.keys(order.items).length < 0) {
      toaster.handleError('No items in order!');
      return false;
    }

    return true;
  };

  const placeOrder = () => {
    if (validate()) {
      postCustomerOrderRequest(orderContext.getOrderDto());
    }
  };

  var totalPrice = 0;
  Object.values(order.articles).forEach((article) => {
    totalPrice += article.price * order.items[article.id];
  });

  return (
    <>
      {order && (
        <Container sx={{ ...styles.container }}>
          <Paper sx={{ ...styles.paper, width: '40%' }} elevation={4}>
            <Typography variant='h4'>Order {order?.id}</Typography>
            <Box sx={{ ...styles.rowBox, width: '100%' }}>
              <TextField
                id='comment'
                label='Comment'
                value={order?.comment}
                sx={{ ...styles.textField, width: '100%' }}
                multiline
                rows={3}
                onChange={(e) => {
                  setOrder((old) => {
                    return { ...old, comment: e.target.value };
                  });
                }}
              />
            </Box>
            <TextField
              id='address'
              label='Address'
              error={addressValidity.error}
              helperText={addressValidity.helper}
              value={order?.address}
              sx={{ ...styles.textField, width: '100%' }}
              onChange={(e) => {
                setOrder((old) => {
                  return { ...old, address: e.target.value };
                });
              }}
            />
            <TextField
              id='totalPrice'
              label='Total price'
              value={totalPrice}
              disabled
              inputProps={{
                readOnly: true,
                style: {
                  textAlign: 'center',
                  fontSize: '20px',
                },
              }}
              sx={{
                ...styles.textField,
                width: '100%',
                '.MuiInputBase-input.Mui-disabled': {
                  WebkitTextFillColor: 'white',
                },
              }}
              type='email'
              aria-readonly='true'
              variant='outlined'
            />
            <Button
              sx={{ width: '70%', marginTop: '15px', alignSelf: 'center' }}
              variant='contained'
              type='submit'
              onClick={(event) => {
                placeOrder();
              }}
            >
              Place order
            </Button>
          </Paper>
        </Container>
      )}
      {Object.keys(order.articles).length > 0 && (
        <Container
          sx={{
            ...styles.container,
            marginTop: '30px',
          }}
        >
          <TableContainer component={Paper} elevation={4} sx={{ width: '80%' }}>
            <Table
              sx={{ padding: '5px', width: '100%' }}
              aria-label='simple table'
            >
              <TableHead>
                <TableRow>
                  <TableCell align='center' sx={{ fontSize: '20px' }}>
                    Id
                  </TableCell>
                  <TableCell align='center' sx={{ fontSize: '20px' }}>
                    Product image
                  </TableCell>
                  <TableCell align='center' sx={{ fontSize: '20px' }}>
                    Name
                  </TableCell>
                  <TableCell align='center' sx={{ fontSize: '20px' }}>
                    Price per unit
                  </TableCell>
                  <TableCell align='center' sx={{ fontSize: '20px' }}>
                    Quantity
                  </TableCell>
                </TableRow>
              </TableHead>
              <TableBody>
                {Object.values(order.articles).map((row) => (
                  <TableRow
                    key={row.id}
                    sx={{ '&:last-child td, &:last-child th': { border: 0 } }}
                  >
                    <TableCell
                      component='th'
                      align='center'
                      scope='row'
                      sx={{ fontSize: '18px' }}
                    >
                      {row.id}
                    </TableCell>
                    <TableCell align='center'>
                      <Box
                        sx={{
                          display: 'flex',
                          justifyContent: 'center',
                          alignItems: 'center',
                        }}
                      >
                        <img
                          src={row.productImage}
                          alt=''
                          style={{ maxWidth: '120px', maxHeight: '120px' }}
                        />
                      </Box>
                    </TableCell>
                    <TableCell align='center' sx={{ fontSize: '18px' }}>
                      {row.name}
                    </TableCell>
                    <TableCell align='center' sx={{ fontSize: '18px' }}>
                      {row.price}
                    </TableCell>
                    <TableCell align='center' sx={{ fontSize: '18px' }}>
                      <TextField
                        value={order.items[row.id]}
                        type='number'
                        onChange={(e) => {
                          var quantity =
                            row.quantity > e.target.value
                              ? e.target.value
                              : row.quantity;
                          setOrder((old) => {
                            const newOrder = { ...old };
                            newOrder.items[row.id] = quantity;
                            return newOrder;
                          });
                        }}
                      ></TextField>
                    </TableCell>
                  </TableRow>
                ))}
              </TableBody>
            </Table>
          </TableContainer>
        </Container>
      )}
      {!order && <NoData>No order...</NoData>}
      {order && Object.keys(order.items).length === 0 && (
        <NoData>No items to show...</NoData>
      )}
      <MyBackdrop open={isLoading}></MyBackdrop>
    </>
  );
};

export default CustomerOrder;
