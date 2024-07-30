import reducer, {
  setErrorMessage,
  setFromDate,
  setToDate,
  setWarningMessage
} from './fileLoaderSlice';

describe('fileLoaderSlice under test', () => {
  it('should return the initial state', () => {
    const actual = reducer(undefined, {});
    expect({
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
    }).toEqual(actual);
  });

  it('should call setErrorMessage', () => {
    const action = {
      type: setErrorMessage.type,
      payload: {
        hasError: true,
        message: 'File extension not allowed!'
      }
    };
    const state = reducer(undefined, action);

    expect(state).toEqual({
      fromDate: '',
      toDate: '',
      validationResult: {
        hasError: true,
        message: 'File extension not allowed!'
      },
      warning: {
        hasWarning: false,
        messageWarning: ''
      }
    });
  });
  it('should call setWarningMessage', () => {
    const action = {
      type: setWarningMessage.type,
      payload: {
        hasWarning: true,
        messageWarning: 'Warning!'
      }
    };
    const state = reducer(undefined, action);

    expect(state).toEqual({
      fromDate: '',
      toDate: '',
      validationResult: {
        hasError: false,
        message: ''
      },
      warning: {
        hasWarning: true,
        messageWarning: 'Warning!'
      }
    });
  });
  it('should call setFromDate', () => {
    const action = {
      type: setFromDate.type,
      payload: '20-12-2022'
    };
    const state = reducer(undefined, action);

    expect(state).toEqual({
      fromDate: '20-12-2022',
      toDate: '',
      validationResult: {
        hasError: false,
        message: ''
      },
      warning: {
        hasWarning: false,
        messageWarning: ''
      }
    });
  });
  it('should call setToDate', () => {
    const action = {
      type: setToDate.type,
      payload: '20-12-2022'
    };
    const state = reducer(undefined, action);

    expect(state).toEqual({
      fromDate: '',
      toDate: '20-12-2022',
      validationResult: {
        hasError: false,
        message: ''
      },
      warning: {
        hasWarning: false,
        messageWarning: ''
      }
    });
  });
});
