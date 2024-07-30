import { renderHook } from '@testing-library/react-hooks';
import usePermission from './usePermission';
import { useSelector, useDispatch, Provider } from 'react-redux';
import configureStore from 'redux-mock-store';
import PropTypes from 'prop-types';

const middlewares = [];
const mockStore = configureStore(middlewares);

const initialState = {};
const store = mockStore(initialState);

const Wrapper = ({ children }) => <Provider store={store}>{children}</Provider>;

jest.mock('react-redux', () => ({
  useSelector: jest.fn(),
  useDispatch: jest.fn()
}));

const mockedDispatch = jest.fn();
useSelector.mockImplementation(selectorFn => selectorFn({ policy: 'abc' }));
useDispatch.mockReturnValue(mockedDispatch);

describe('test usePermission', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });
  it('should call dispatch', () => {
    renderHook(() => usePermission(''), { Wrapper });
    expect(mockedDispatch).toHaveBeenCalledTimes(1);
  });

  it('should notify state via selector', () => {
    const { result } = renderHook(() => usePermission(''), { Wrapper });
    expect(result.current).toEqual('abc');
  });
});

const propTypes = {
  children: PropTypes.node
};

Wrapper.propTypes = propTypes;
