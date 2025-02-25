import { createSlice } from '@reduxjs/toolkit';
import type { PayloadAction } from '@reduxjs/toolkit';

import {
  SubmitResponse,
  UploadRequest,
  ValidationResponse
} from '../../type/typeNative';
import { uploadProviderThunk, submitProviderThunk } from './providerUploadServiceNative';

const validationResultDefault = {
  hasError: false,
  sessionId: '',
  recordCount: 0,
  validationResults: [],
  isSuccessful: false,
  exception: ''
} as ValidationResponse;

const eventUploadSlice = createSlice({
  name: 'eventUploadSlice',
  initialState: {
    isLoading: false,
    showTable: false,
    currentStep: 1,
    file: null,
    validationResponse: validationResultDefault,
    submitResponse: { isSuccessful: false, message: '' } as SubmitResponse
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
    resetResponse: state => {
      state.submitResponse = { isSuccessful: false, message: '' };
    },
    setShowTable: (state, action) => {
      state.showTable = action.payload;
    }
  },
  extraReducers: builder => {
    builder.addCase(uploadProviderThunk.pending, state => {
      state.isLoading = true;
    })
      .addCase(uploadProviderThunk.fulfilled, (state, { payload }) => {
        state.validationResponse = payload;
        state.isLoading = false;
      }).addCase(uploadProviderThunk.rejected, state => {
        state.isLoading = false;
      }).addCase(submitProviderThunk.pending, state => {
        state.isLoading = true;
      }).addCase(submitProviderThunk.fulfilled, (state, { payload }) => {
        state.submitResponse = payload;
        state.isLoading = false;
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
  resetResponse,
  setResetValidationResult,
  setShowTable
} = actions;

export {
  setNextStep,
  setBackStep,
  setStep,
  setFile,
  resetResponse,
  setResetValidationResult,
  uploadProviderThunk,
  submitProviderThunk,
  setShowTable
};

export default reducer;
