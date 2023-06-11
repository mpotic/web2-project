import { useState, useEffect } from 'react';

import {
  Container,
  Paper,
  Box,
  TextField,
  Typography,
  Button,
} from '@mui/material';

import styles from '../../style/centerFormStyles';

import { toasterUtil as toaster } from '../../utils/toasterUtil';
import MyBackdrop from '../../components/MyBackdrop';
import useServices from '../../services/useServices';
import UploadButton from '../../components/UploadButton';

const NewArticle = () => {
  const [article, setArticle] = useState(articleInit);
  const [productImage, setProductImage] = useState(null);
  const [validity, setValidity] = useState(fieldValidity);
  const { postArticleRequest, clearRequest, isLoading, error, statusCode } =
    useServices();

  useEffect(() => {
    if (isLoading) {
      return;
    } else if (statusCode === 200 && !error) {
      clearRequest();
      toaster.handleSuccess('Successfully added a new article!');
    } else if (statusCode !== 200 && error) {
      toaster.handleError(statusCode, error);
      clearRequest();
    }
  }, [isLoading, statusCode, error, clearRequest]);

  const handleAddArticle = (event) => {
    event.preventDefault();

    setValidity(validateFields(article));

    for (const field in validity) {
      if (validity[field].error) {
        return;
      }
    }

    const requestBody = {
      ...article,
      productImage,
    };

    postArticleRequest(requestBody);
  };

  return (
    <>
      <Container
        sx={{
          ...styles.container,
          gap: '40px',
        }}
      >
        <Box
          sx={{
            display: 'flex',
            flexDirection: 'column',
            gap: '25px',
            justifyItems: 'center',
            alignItems: 'center',
            width: '30%',
            borderRadius: '3px',
            padding: '10px',
            boxShadow:
              '0px 2px 4px -1px rgba(0,0,0,0.2), 0px 4px 5px 0px rgba(0,0,0,0.14), 0px 1px 10px 0px rgba(0,0,0,0.12)',
            margin: '0px',
          }}
        >
          <Typography sx={{ fontSize: '24px', padding: '0px' }}>
            Product image
          </Typography>
          <UploadButton
            width='100%'
            maxHeightPerc='250px'
            maxWidthPerc='100%'
            image={productImage}
            buttonText='Reupload'
            direction='column'
            alternativeToNoImage='No product image...'
            doubleClickCallback={() => {
              setProductImage(null);
            }}
            uploadCallback={(file) => {
              setProductImage(file);
            }}
          />
        </Box>
        <Paper sx={{ ...styles.paper }} elevation={4}>
          <Typography variant='h4'>New Article</Typography>
          <TextField
            id='name'
            label='Name'
            value={article?.name}
            error={validity.name.error}
            helperText={validity.name.helper}
            sx={{ ...styles.textField, width: '100%' }}
            onChange={(e) => {
              setArticle((old) => {
                return {
                  ...old,
                  name: e.target.value,
                };
              });
            }}
          />
          <Box sx={{ ...styles.rowBox, width: '100%' }}>
            <TextField
              id='description'
              label='Description'
              value={article?.description}
              error={validity.description.error}
              helperText={validity.description.helper}
              sx={{ ...styles.textField, width: '100%' }}
              onChange={(e) => {
                setArticle((old) => {
                  return { ...old, description: e.target.value };
                });
              }}
              multiline
            />
          </Box>
          <Box sx={{ ...styles.rowBox, width: '100%' }}>
            <TextField
              id='quantity'
              label='Quantity'
              type='number'
              value={article?.quantity}
              error={validity.quantity.error}
              helperText={validity.quantity.helper}
              sx={{ ...styles.textField, width: '100%' }}
              onChange={(e) => {
                setArticle((old) => {
                  return { ...old, quantity: e.target.value };
                });
              }}
            />
          </Box>
          <Box sx={{ ...styles.rowBox, width: '100%' }}>
            <TextField
              id='price'
              label='Price'
              type='number'
              value={article?.price}
              error={validity.price.error}
              helperText={validity.price.helper}
              sx={{ ...styles.textField, width: '100%' }}
              onChange={(e) => {
                setArticle((old) => {
                  return { ...old, price: e.target.value };
                });
              }}
            />
          </Box>
          <Button
            sx={{ width: '70%', marginTop: '15px', alignSelf: 'center' }}
            variant='contained'
            type='submit'
            onClick={(event) => {
              handleAddArticle(event);
            }}
          >
            Add article
          </Button>
        </Paper>
      </Container>
      <MyBackdrop open={isLoading} />
    </>
  );
};

const fieldValidity = {
  name: {
    error: false,
    helper: '',
  },
  description: {
    error: false,
    helper: '',
  },
  quantity: {
    error: false,
    helper: '',
  },
  price: {
    error: false,
    helper: '',
  },
};

const articleInit = {
  name: '',
  description: '',
  quantity: 0,
  price: 0,
};

const validateFields = (article) => {
  const updatedFieldValidity = { ...fieldValidity };

  const requiredFields = Object.keys(fieldValidity);

  requiredFields.forEach((field) => {
    if (!article[field]) {
      updatedFieldValidity[field].error = true;
      updatedFieldValidity[field].helper = 'Field is required';
    } else if (article[field].length < 3) {
      updatedFieldValidity[field].error = true;
      updatedFieldValidity[field].helper = 'Too short';
    } else {
      updatedFieldValidity[field].error = false;
      updatedFieldValidity[field].helper = '';
    }
  });

  return updatedFieldValidity;
};

export default NewArticle;
