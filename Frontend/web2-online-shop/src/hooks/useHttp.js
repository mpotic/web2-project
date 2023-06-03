import { useCallback, useState } from 'react';

const useHttp = (resolve) => {
  const [data, setData] = useState(null);
  const [isLoading, setIsLoading] = useState(null);
  const [error, setError] = useState(null);
  const [statusCode, setStatusCode] = useState(null);

  const getErrorMsg = (method, statusCode) => {
    switch (statusCode) {
      case 401:
        return 'Invalid credentials!';
      default:
        if (method === 'GET') {
          return 'Failed to fetch data from the server!';
        } else if (method === 'POST') {
          return 'Failed to post data to the server!';
        } else if (method === 'PUT') {
          return 'Failed to update data on the server!';
        } else {
          return 'Failed to process the request!';
        }
    }
  };

  const getRequest = useCallback(
    (url) => {
      setIsLoading(true);
      fetch(url, { method: 'GET', mode: 'cors', credentials: 'include' })
        .then((response) => {
          setStatusCode(response.status);
          if (!response.ok)
            throw new Error(getErrorMsg('GET', response.status));
          return response.json();
        })
        .then((data) => {
          setData(data);
          setError(null);
        })
        .catch((error) => {
          setError('Error doing GET request! ' + error);
        })
        .finally(() => {
          setIsLoading(false);
          resolve && resolve();
        });
    },
    [resolve]
  );

  const postRequest = useCallback(
    (url, data) => {
      setIsLoading(true);
      fetch(url, {
        method: 'post',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(data),
        mode: 'cors',
        credentials: 'include',
      })
        .then((response) => {
          setStatusCode(response.status);
          if (!response.ok)
            throw new Error(getErrorMsg('POST', response.status));
          return response.json();
        })
        .then((data) => {
          setData(data);
          setError(null);
        })
        .catch((error) => {
          setError('Error doing POST request! ' + error.message);
        })
        .finally(() => {
          setIsLoading(false);
          resolve && resolve();
        });
    },
    [resolve]
  );

  const putRequest = useCallback(
    (url, data) => {
      setIsLoading(true);
      fetch(url, {
        method: 'put',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(data),
        mode: 'cors',
        credentials: 'include',
      })
        .then((response) => {
          setStatusCode(response.status);
          if (!response.ok)
            throw new Error(getErrorMsg('PUT', response.status));
          return response.json();
        })
        .then((data) => {
          setData(data);
          setError(null);
        })
        .catch((error) => {
          setError('Error doing PUT request! ' + error.message);
        })
        .finally(() => {
          setIsLoading(false);
          resolve && resolve();
        });
    },
    [resolve]
  );

  const putRequestFormData = useCallback(
    (url, data) => {
      setIsLoading(true);

      const formData = new FormData();
      for (const key in data) {
        formData.append(key, data[key]);
      }

      fetch(url, {
        method: 'put',
        body: formData,
        mode: 'cors',
        credentials: 'include',
      })
        .then((response) => {
          setStatusCode(response.status);
          if (!response.ok) {
            throw new Error(getErrorMsg('PUT', response.status));
          }
          return response.json();
        })
        .then((data) => {
          setData(data);
          setError(null);
        })
        .catch((error) => {
          setError('Error doing PUT request! ' + error.message);
        })
        .finally(() => {
          setIsLoading(false);
          resolve && resolve();
        });
    },
    [resolve]
  );

  const postRequestFormData = useCallback(
    (url, data) => {
      setIsLoading(true);

      const formData = new FormData();
      for (const key in data) {
        formData.append(key, data[key]);
      }

      console.log(formData);

      fetch(url, {
        method: 'post',
        body: formData,
        mode: 'cors',
        credentials: 'include',
      })
        .then((response) => {
          setStatusCode(response.status);
          if (!response.ok) {
            throw new Error(getErrorMsg('POST', response.status));
          }
          return response.json();
        })
        .then((data) => {
          setData(data);
          setError(null);
        })
        .catch((error) => {
          setError('Error doing POST request! ' + error.message);
        })
        .finally(() => {
          setIsLoading(false);
          resolve && resolve();
        });
    },
    [resolve]
  );

  const resetHttp = () => {
    setData(null);
    setError(null);
    setIsLoading(false);
    setStatusCode(null);
  };

  return {
    data,
    isLoading,
    error,
    statusCode,
    getRequest,
    postRequest,
    postRequestFormData,
    putRequest,
    putRequestFormData,
    resetHttp,
  };
};

export default useHttp;
