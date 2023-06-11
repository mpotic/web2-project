import Table from '@mui/material/Table';
import TableBody from '@mui/material/TableBody';
import TableCell from '@mui/material/TableCell';
import TableContainer from '@mui/material/TableContainer';
import TableHead from '@mui/material/TableHead';
import TableRow from '@mui/material/TableRow';
import Paper from '@mui/material/Paper';
import { Button, Container, Box } from '@mui/material';

import styles from '../../style/centerFormStyles';

import NoData from '../NoData';

const Articles = ({ data, hasButton, buttonCallback, buttonText }) => {
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
                  <TableCell align='center'>Product image</TableCell>
                  <TableCell align='center'>Name</TableCell>
                  <TableCell align='center'>Description</TableCell>
                  <TableCell align='center'>Quantity</TableCell>
                  <TableCell align='center'>Price</TableCell>
                  {hasButton && <TableCell align='center'>Action</TableCell>}
                </TableRow>
              </TableHead>
              <TableBody>
                {data.map((row) => (
                  <TableRow
                    key={row.id}
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
                          src={row.productImage}
                          alt=''
                          style={{ maxWidth: '120px', maxHeight: '120px' }}
                        />
                      </Box>
                    </TableCell>
                    <TableCell align='center'>{row.name}</TableCell>
                    <TableCell align='center'>{row.description}</TableCell>
                    <TableCell align='center'>{row.quantity}</TableCell>
                    <TableCell align='center'>{row.price}</TableCell>
                    {hasButton && (
                      <TableCell align='center'>
                        <Button
                          variant='contained'
                          onClick={(e) => {
                            buttonCallback(row.name);
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
      {data.length === 0 && <NoData>No articles to show...</NoData>}
    </>
  );
};

export default Articles;
