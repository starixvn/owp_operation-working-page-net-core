import { renderHook } from '@testing-library/react-hooks';
import { useSelector, useDispatch } from 'react-redux';
import useBuildFilterDate from './useBuildFilterDate';

jest.mock('react-redux', () => ({
  useSelector: jest.fn(),
  useDispatch: jest.fn(),
  useEffect: jest.fn()
}));

const mockedDispatch = jest.fn();
useSelector.mockImplementation(selectorFn => selectorFn({ loading: true }));
useDispatch.mockReturnValue(mockedDispatch);

describe('test useBuildFilterDate', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });
  it('should call dispatch', () => {
    renderHook(() => useBuildFilterDate());
    expect(mockedDispatch).toHaveBeenCalledTimes(1);
  });
});
