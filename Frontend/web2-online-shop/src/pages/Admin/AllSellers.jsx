import { useState, useEffect } from 'react';

import Table from '@mui/material/Table';
import TableBody from '@mui/material/TableBody';
import TableCell from '@mui/material/TableCell';
import TableContainer from '@mui/material/TableContainer';
import TableHead from '@mui/material/TableHead';
import TableRow from '@mui/material/TableRow';
import Paper from '@mui/material/Paper';
import { Box, Button, Container, Typography } from '@mui/material';

import styles from '../../style/centerFormStyles';

import useServices from '../../services/useServices';
import MyBackdrop from '../../components/MyBackdrop';
import { toasterUtil as toaster } from '../../utils/toasterUtil';
import { getDateString } from '../../utils/dateTimeUtils';

const AllSellers = () => {
  const {
    data,
    error,
    statusCode,
    isLoading,
    getAllSellersRequest,
    updateSellerStatusRequest,
    clearRequest,
  } = useServices();
  const [updatingStatus, setUpdatingStatus] = useState(false);
  const [fetchingSellers, setFetchingSellers] = useState(true);
  const [sellers, setSellers] = useState([]);

  useEffect(() => {
    getAllSellersRequest();
  }, [getAllSellersRequest]);

  useEffect(() => {
    if (isLoading) {
      return;
    } else if (statusCode === 200 && !error && updatingStatus) {
      setUpdatingStatus(false);
      getAllSellersRequest();
      setFetchingSellers(true);
      clearRequest();
      toaster.handleSuccess('Successfully changed status!');
    } else if (statusCode === 200 && !error && data && fetchingSellers) {
      setFetchingSellers(false);
      data?.sellers.forEach((seller) => {
        seller.sellerProfileImage =
          'data:image/*;base64,' + seller.sellerProfileImage;
      });
      setSellers(data?.sellers);
      clearRequest();
    } else if (statusCode !== 200 && error) {
      toaster.handleError(statusCode, error);
    }
  }, [
    isLoading,
    statusCode,
    error,
    updatingStatus,
    data,
    fetchingSellers,
    getAllSellersRequest,
    clearRequest,
  ]);

  return (
    <>
      <Container
        sx={{
          ...styles.container,
          margin: '0,60,0,0',
        }}
      >
        <TableContainer component={Paper} elevation={4}>
          <Table
            sx={{ padding: '5px', width: '100%' }}
            aria-label='simple table'
          >
            <TableHead>
              <TableRow>
                <TableCell align='center'>Profile image </TableCell>
                <TableCell align='center'>Username </TableCell>
                <TableCell align='center'>Email</TableCell>
                <TableCell align='center'>Firstname</TableCell>
                <TableCell align='center'>Lastname</TableCell>
                <TableCell align='center'>Address</TableCell>
                <TableCell align='center'>Birthdate</TableCell>
                <TableCell align='center' colSpan={2}>
                  Status
                </TableCell>
              </TableRow>
            </TableHead>
            <TableBody>
              {sellers.map((row) => (
                <TableRow
                  key={row.username}
                  sx={{ '&:last-child td, &:last-child th': { border: 0 } }}
                >
                  <TableCell component='th' align='center' scope='row'>
                    <Box
                      sx={{
                        display: 'flex',
                        justifyContent: 'center',
                        alignItems: 'center',
                      }}
                    >
                      <img
                        src={row.sellerProfileImage}
                        alt=''
                        style={{ maxWidth: '120px', maxHeight: '120px' }}
                      />
                    </Box>
                  </TableCell>
                  <TableCell component='th' align='center' scope='row'>
                    {row.username}
                  </TableCell>
                  <TableCell align='center'>{row.email}</TableCell>
                  <TableCell align='center'>{row.firstname}</TableCell>
                  <TableCell align='center'>{row.lastname}</TableCell>
                  <TableCell align='center'>{row.address}</TableCell>
                  <TableCell align='center'>
                    {getDateString(row.birthdate)}
                  </TableCell>
                  {row.approvalStatus > 0 && (
                    <TableCell align='center' colSpan={2}>
                      <Typography
                        color={
                          getStatusString(row.approvalStatus) === 'Approved'
                            ? 'primary'
                            : 'secondary'
                        }
                      >
                        {getStatusString(row.approvalStatus)}
                      </Typography>
                    </TableCell>
                  )}
                  {getStatusString(row.approvalStatus) === 'Pending' && (
                    <TableCell align='center'>
                      <Button
                        variant='contained'
                        onClick={(e) => {
                          setUpdatingStatus(true);
                          updateSellerStatusRequest({
                            SellerApprovalStatus: true,
                            SellerUsername: row.username,
                          });
                        }}
                      >
                        Approve
                      </Button>
                    </TableCell>
                  )}
                  {getStatusString(row.approvalStatus) === 'Pending' && (
                    <TableCell align='center'>
                      <Button
                        variant='contained'
                        color='secondary'
                        onClick={(e) => {
                          setUpdatingStatus(true);
                          updateSellerStatusRequest({
                            SellerApprovalStatus: false,
                            SellerUsername: row.username,
                          });
                        }}
                      >
                        Deny
                      </Button>
                    </TableCell>
                  )}
                </TableRow>
              ))}
            </TableBody>
          </Table>
        </TableContainer>
      </Container>
      <MyBackdrop open={isLoading}></MyBackdrop>
    </>
  );
};

const getStatusString = (approval) => {
  switch (approval) {
    case 0:
      return 'Pending';
    case 1:
      return 'Approved';
    case 2:
      return 'Denied';
    default:
      return 'Unknown';
  }
};

// class Seller {
//   constructor(
//     username,
//     email,
//     firstname,
//     lastname,
//     status,
//     address,
//     birthdate
//   ) {
//     this.username = username;
//     this.email = email;
//     this.firstname = firstname;
//     this.lastname = lastname;
//     this.status = status;
//     this.address = address;
//     this.birthdate = birthdate;
//   }
// }

export default AllSellers;
