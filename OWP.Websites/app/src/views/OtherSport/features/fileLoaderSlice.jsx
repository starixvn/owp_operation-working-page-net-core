import { createSlice } from '@reduxjs/toolkit';

const fileLoaderSlice = createSlice({
  name: 'fileLoader',
  initialState: {
    fromDate: '',
    toDate: '',
    validationResult: {
      hasError: false,
      message: ''
    },
    warning: {
      hasWarning: false,
      messageWarning: ''
    }
  },
  reducers: {
    setFromDate: (state, action) => {
      state.fromDate = action.payload;
    },
    setToDate: (state, action) => {
      state.toDate = action.payload;
    },
    setErrorMessage: (state, action) => {
      const { hasError, message } = action.payload;
      state.validationResult.hasError = hasError;
      state.validationResult.message = message;
    },
    setWarningMessage: (state, action) => {
      const { hasWarning, messageWarning } = action.payload;
      state.warning.hasWarning = hasWarning;
      state.warning.messageWarning = messageWarning;
    }
  }
});

const { reducer, actions } = fileLoaderSlice;
const { setErrorMessage, setFromDate, setToDate, setWarningMessage } = actions;
export { setErrorMessage, setFromDate, setToDate, setWarningMessage };

export default reducer;
