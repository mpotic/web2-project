import { useRef, useState } from 'react';
import Button from '@mui/material/Button';
import { Box } from '@mui/material';

export default function UploadButton({
  callback,
  width,
  maxWidthPerc,
  maxHeightPerc,
}) {
  const [imageUrl, setImageUrl] = useState(null);
  const inputRef = useRef();

  maxWidthPerc = maxWidthPerc ?? '50%';
  maxHeightPerc = maxHeightPerc ?? '50%';

  const handleFileUpload = (event) => {
    try {
      const file = event.target.files[0];
      if (file == null) {
        return;
      }

      callback(file);

      const reader = new FileReader();
      reader.onloadend = () => {
        setImageUrl(reader.result);
      };

      reader.readAsDataURL(file);
    } catch (e) {
      console.log(e);
    }
  };

  return (
    <Box
      direction='row'
      display='flex'
      alignItems='center'
      justifyContent={imageUrl == null ? 'center' : 'space-evenly'}
      sx={{ width: width ?? '50%' }}
    >
      {imageUrl && (
        <img
          src={imageUrl}
          alt='Uploaded'
          style={{ maxWidth: maxWidthPerc, maxHeight: maxHeightPerc }}
          onDoubleClick={(e) => {
            setImageUrl(null);
            callback(null);
            inputRef.current.value = null;
          }}
        />
      )}
      <label htmlFor='upload-image'>
        <Button variant='contained' component='span'>
          Upload
        </Button>
        <input
          ref={inputRef}
          id='upload-image'
          hidden
          accept='image/*'
          type='file'
          onChange={handleFileUpload}
        />
      </label>
    </Box>
  );
}