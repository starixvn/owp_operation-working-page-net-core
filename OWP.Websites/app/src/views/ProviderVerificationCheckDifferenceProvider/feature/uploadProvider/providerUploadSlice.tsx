import { createSlice } from '@reduxjs/toolkit';
import type { PayloadAction } from '@reduxjs/toolkit';
import { ValidationResponse } from '../../type/typeNative';
import { uploadProviderThunk, submitProviderThunk } from './providerUploadServiceNative';

const validationResultDefault = {
  isSuccess: false,
  value: {
    originalRecordCount: 0,
    sessionId: '',
  }
} as ValidationResponse;

const eventUploadSlice = createSlice({
  name: 'eventUploadSlice',
  initialState: {
    isLoading: false,
    showTable: false,
    currentStep: 1,
    file: null,
    validationResponse: validationResultDefault,
    eventDate: new Date()
  },
  reducers: {
    setNextStep: state => {
      state.currentStep += 1;
    },
    setBackStep: state => {
      state.currentStep -= 1;
    },
    setStep: (state, action: PayloadAction<number>) => {
      state.currentStep = action.payload;
    },
    setFile: (state, { payload }) => {
      state.file = payload;
    },
    setResetValidationResult: state => {
      state.validationResponse = validationResultDefault;
    },
    setShowTable: (state, action) => {
      state.showTable = action.payload;
    },
    setEventDate: (state, action) => {
      state.eventDate = action.payload;
    }
  },
  extraReducers: builder => {
    builder
      .addCase(uploadProviderThunk.pending, state => {
        state.isLoading = true;
      })
      .addCase(uploadProviderThunk.fulfilled, (state, { payload }) => {
        state.validationResponse = payload;
        state.isLoading = false;
      }).addCase(uploadProviderThunk.rejected, state => {
        state.isLoading = false;
      }).addCase(submitProviderThunk.pending, state => {
        state.isLoading = true;
      }).addCase(submitProviderThunk.rejected, state => {
        state.isLoading = false;
      });
  }
});

const { reducer, actions } = eventUploadSlice;
const {
  setNextStep,
  setBackStep,
  setStep,
  setFile,
  setResetValidationResult,
  setShowTable,
  setEventDate
} = actions;

export {
  setNextStep,
  setBackStep,
  setStep,
  setFile,
  setResetValidationResult,
  uploadProviderThunk,
  submitProviderThunk,
  setShowTable,
  setEventDate
};

export default reducer;
