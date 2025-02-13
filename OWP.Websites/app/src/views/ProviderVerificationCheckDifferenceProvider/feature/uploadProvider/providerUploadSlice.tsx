import { createSlice } from '@reduxjs/toolkit';
import type { PayloadAction } from '@reduxjs/toolkit';

/* import {
  SubmitResponse,
  UploadRequest,
  ValidationResponse
} from '../../typeNative'; */
//import { submitEventThunk, uploadEventThunk } from './eventUploadServiceNative';

/* const validationResultDefault = {
  hasError: false,
  sessionId: '',
  recordCount: 0,
  validationResults: [],
  isSuccessful: false,
  exception: ''
} as ValidationResponse; */

const eventUploadSlice = createSlice({
  name: 'eventUploadSlice',
  initialState: {
    isLoading: false,
    currentStep: 1
    //validationResponse: validationResultDefault,
    //submitResponse: { isSuccessful: false, message: '' } as SubmitResponse
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
    }
  },
  extraReducers: builder => {
    /*  builder.addCase(uploadEventThunk.pending, state => {
      state.isLoading = true;
    })
    .addCase(uploadEventThunk.fulfilled, (state, { payload }) => {
      state.validationResponse = payload;
      state.isLoading = false;
    }).addCase(uploadEventThunk.rejected, state => {
      state.isLoading = false;
    }).addCase(submitEventThunk.pending, state => {
      state.isLoading = true;
    }).addCase(submitEventThunk.fulfilled, (state, { payload }) => {
      state.submitResponse = payload;
      state.isLoading = false;
    }).addCase(submitEventThunk.rejected, state => {
      state.isLoading = false;
    }); */
  }
});

const { reducer, actions } = eventUploadSlice;
const { setNextStep, setBackStep, setStep } = actions;

export { setNextStep, setBackStep, setStep };

export default reducer;
