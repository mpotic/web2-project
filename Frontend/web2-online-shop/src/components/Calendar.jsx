import { useState } from 'react';

import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs';
import { LocalizationProvider } from '@mui/x-date-pickers/LocalizationProvider';
import { DatePicker } from '@mui/x-date-pickers/DatePicker';
import { Box, FormControl } from '@mui/material';

const Calendar = ({ sx, label, callback, error, helperText }) => {
  const [selectedDate, setSelectedDate] = useState(null);

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
            disableFuture
          />
        </FormControl>
      </LocalizationProvider>
    </Box>
  );
};

export default Calendar;
