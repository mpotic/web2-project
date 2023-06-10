import Table from '@mui/material/Table';
import TableBody from '@mui/material/TableBody';
import TableCell from '@mui/material/TableCell';
import TableContainer from '@mui/material/TableContainer';
import TableHead from '@mui/material/TableHead';
import TableRow from '@mui/material/TableRow';
import Paper from '@mui/material/Paper';
import { Button, Container, Typography } from '@mui/material';

import styles from '../../style/centerFormStyles';

import { getDateString } from '../../utils/dateTimeUtils';
import NoData from '../../components/NoData';

const Orders = ({ role, data, hasButton, buttonCallback, buttonText }) => {
  console.log(data);

  return (
    <>
      {data && data.length > 0 && (
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
                  <TableCell align='center'>Id </TableCell>
                  <TableCell align='center'>Comment </TableCell>
                  <TableCell align='center'>Address</TableCell>
                  <TableCell align='center'>Total price</TableCell>
                  <TableCell align='center'>Time placed</TableCell>
                  {role === 'admin' && (
                    <TableCell align='center'>Status</TableCell>
                  )}
                  {role !== 'admin' && (
                    <TableCell align='center'>Remaining time</TableCell>
                  )}
                  {hasButton && <TableCell>Action</TableCell>}
                </TableRow>
              </TableHead>
              <TableBody>
                {data.map((row) => (
                  <TableRow
                    key={row.id}
                    sx={{ '&:last-child td, &:last-child th': { border: 0 } }}
                  >
                    <TableCell component='th' align='center' scope='row'>
                      {row.id}
                    </TableCell>
                    <TableCell align='center'>{row.comment}</TableCell>
                    <TableCell align='center'>{row.address}</TableCell>
                    <TableCell align='center'>{row.totalPrice}</TableCell>
                    <TableCell align='center'>
                      {getDateString(row.placedTime)}
                    </TableCell>
                    {role === 'admin' && (
                      <TableCell align='center'>
                        {row.remainingTime === 0 ? 'Finished' : 'In progress'}
                      </TableCell>
                    )}
                    {role !== 'admin' && (
                      <TableCell align='center'>
                        {getDateString(row.remainingTime)}
                      </TableCell>
                    )}
                    {hasButton && (
                      <TableCell>
                        <Button
                          onClick={(e) => {
                            buttonCallback(e);
                          }}
                        >
                          {buttonText}
                        </Button>
                      </TableCell>
                    )}
                  </TableRow>
                ))}
              </TableBody>
            </Table>
          </TableContainer>
        </Container>
      )}
      {!data || (data.length === 0 && <NoData>No orders to show...</NoData>)}
    </>
  );
};

export default Orders;
