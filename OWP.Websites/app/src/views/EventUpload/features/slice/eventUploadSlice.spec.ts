import { UploadRequest, ValidationResponse } from '../../typeNative';
import reducer, {
  setEventDate,
  setIsUploadFMAndAIO,
  setNextStep,
  setBackStep,
  setStep,
  setFile,
  setResetValidationResult,
  uploadEventThunk
} from './eventUploadSlice';

const validationResultDefault = {
  hasError: false,
  sessionId: '',
  recordCount: 0,
  validationResults: [],
  isSuccessful: false,
  exception: ''
} as ValidationResponse;

describe('eventUploadSlice under test', () => {
  describe('reducers under test', () => {
    it('should return the initial state', () => {
      const actual = reducer(undefined, {
        type: undefined
      });
      expect({
        isLoading: false,
        currentStep: 1,
        request: { isUploadFMAndAIO: false } as UploadRequest,
        validationResponse: validationResultDefault,
        submitResponse: { isSuccessful: false, message: '' }
      }).toEqual(actual);
    });

    it('should call setNextStep', () => {
      const action = {
        type: setNextStep.type
      };
      const state = reducer(undefined, action);

      expect(state.currentStep).toEqual(2);
    });

    it('should call setBackStep', () => {
      const action = {
        type: setBackStep.type
      };
      const state = reducer(undefined, action);

      expect(state.currentStep).toEqual(0);
    });

    it('should call setStep', () => {
      const action = {
        type: setStep.type,
        payload: 3
      };
      const state = reducer(undefined, action);

      expect(state.currentStep).toEqual(3);
    });

    it('should call setEventDate', () => {
      const action = {
        type: setEventDate.type,
        payload: 'Nov 11, 2022'
      };
      const state = reducer(undefined, action);

      expect(state.request.eventDate).toEqual('Nov 11, 2022');
    });

    it('should call setIsUploadFMAndAIO', () => {
      const action = {
        type: setIsUploadFMAndAIO.type
      };
      const state = reducer(undefined, action);

      expect(state.request.isUploadFMAndAIO).toEqual(true);
    });

    it('should call setFile', () => {
      const action = {
        type: setFile.type,
        payload: new File([], 'name.xlsx')
      };
      const state = reducer(undefined, action);

      expect(state.request.file).toEqual(new File([], 'name.xlsx'));
    });

    it('should call setResetValidationResult', () => {
      const action = {
        type: setResetValidationResult.type,
        payload: 3
      };
      const state = reducer(undefined, action);

      expect(state.validationResponse).toEqual({
        hasError: false,
        sessionId: '',
        recordCount: 0,
        validationResults: [],
        isSuccessful: false,
        exception: ''
      } as ValidationResponse);
    });
  });

  describe('extraReducers', () => {
    it('should handle uploadEventThunk.pending', () => {
      const action = {
        type: uploadEventThunk.pending.type
      };
      const actual = reducer(undefined, action);
      expect(actual.isLoading).toEqual(true);
      expect(actual.validationResponse).toEqual(validationResultDefault);
    });

    it('should handle uploadEventThunk.fulfilled', () => {
      const action = {
        type: uploadEventThunk.fulfilled.type,
        payload: {
          isSuccessful: true,
          exception: 'error'
        }
      };
      const actual = reducer(undefined, action);
      expect(actual.isLoading).toEqual(false);
      expect(actual.validationResponse).toEqual({
        isSuccessful: true,
        exception: 'error'
      });
    });

    it('should handle uploadEventThunk.rejected', () => {
      const action = {
        type: uploadEventThunk.rejected.type
      };
      const actual = reducer(undefined, action);
      expect(actual.isLoading).toEqual(false);
      expect(actual.validationResponse).toEqual(validationResultDefault);
    });
  });
});
