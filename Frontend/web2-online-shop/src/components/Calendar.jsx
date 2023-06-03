import { useState } from 'react';

import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs';
import { LocalizationProvider } from '@mui/x-date-pickers/LocalizationProvider';
import { DatePicker } from '@mui/x-date-pickers/DatePicker';
import { Box } from '@mui/material';

const Calendar = ({ sx, label, callback }) => {
  const [selectedDate, setSelectedDate] = useState(null);

  return (
    <Box sx={{ ...sx }}>
      <LocalizationProvider dateAdapter={AdapterDayjs}>
        <DatePicker
          value={selectedDate}
          onChange={(date) => {
            setSelectedDate(date);
            callback(date);
          }}
          label={label}
          sx={{ padding: '0' }}
        />
      </LocalizationProvider>
    </Box>
  );
};

export default Calendar;
