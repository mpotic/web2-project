import { useEffect, useState } from 'react';

import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs';
import { LocalizationProvider } from '@mui/x-date-pickers/LocalizationProvider';
import { DatePicker } from '@mui/x-date-pickers/DatePicker';
import { Box, FormControl } from '@mui/material';
import { getDateString } from '../utils/dateTimeUtils';
import dayjs from 'dayjs';

const Calendar = ({
  sx,
  label,
  callback,
  error,
  helperText,
  disableFuture,
  value,
}) => {
  const [selectedDate, setSelectedDate] = useState(null);

  useEffect(() => {
    if (!value) {
      return;
    }

    setSelectedDate(new dayjs(getDateString(value)));
  }, [value]);

  return (
    <Box sx={{ ...sx }}>
      <LocalizationProvider dateAdapter={AdapterDayjs}>
        <FormControl error={error}>
          <DatePicker
            value={selectedDate}
            onChange={(date) => {
              setSelectedDate(date);
              callback(date);
            }}
            slotProps={{
              textField: {
                helperText: helperText,
                error: error,
              },
            }}
            label={label}
            sx={{ padding: '0' }}
            disableFuture={disableFuture}
          />
        </FormControl>
      </LocalizationProvider>
    </Box>
  );
};

export default Calendar;
