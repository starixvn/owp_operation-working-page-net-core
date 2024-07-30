import { useSelector, useDispatch, useStore } from 'react-redux';

const reduxEnv = (store = {}, dispatchReturnValue = {}, selectorReturnValue = {}) => {
  jest.mock('react-redux', () => ({
    useSelector: jest.fn(),
    useDispatch: jest.fn(),
    useStore: jest.fn()
  }));

  const mockStore = {
    getState: () => store
  };

  const mockedDispatch = jest.fn().mockReturnValue(dispatchReturnValue);

  useStore.mockReturnValue(mockStore);
  useDispatch.mockReturnValue(mockedDispatch);
  useSelector.mockImplementation(selectorFn => selectorFn(selectorReturnValue));
  return {
    mockedDispatch
  };
};

export default reduxEnv;
