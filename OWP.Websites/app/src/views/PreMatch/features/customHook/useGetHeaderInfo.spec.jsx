import { renderHook } from '@testing-library/react-hooks';
import { useSelector, useDispatch } from 'react-redux';
import useGetHeaderInfo from './useGetHeaderInfo';

jest.mock('../../app/store');
jest.mock('react-redux', () => ({
  useSelector: jest.fn(),
  useDispatch: jest.fn(),
  useEffect: jest.fn()
}));

const mockedDispatch = jest.fn();

useDispatch.mockReturnValue(mockedDispatch);

const setUp = isSetFilterDate => {
  const props = {
    filter: {
      filterDate: isSetFilterDate
        ? new Date('2020-09-16T08:36:55.768Z')
        : undefined
    },
    header: {
      info: {
        UpdatedTime: new Date('2020-09-16T08:36:55.768Z'),
        UserName: 'Carlos'
      }
    }
  };
  useSelector.mockImplementation(selectorFn => selectorFn({ ...props }));
};

describe('test useGetHeaderInfo', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });
  it('should call dispatch', () => {
    setUp(true);
    renderHook(() => useGetHeaderInfo(true));
    expect(mockedDispatch).toHaveBeenCalledTimes(1);
  });

  it('should notify state via selector', () => {
    setUp(true);
    const { result } = renderHook(() => useGetHeaderInfo(true));
    const { current } = result;
    expect(current.filterDate.toString()).toEqual(
      'Wed Sep 16 2020 15:36:55 GMT+0700 (Indochina Time)'
    );
    expect(JSON.stringify(current.headerState)).toEqual(
      JSON.stringify({
        info: {
          UpdatedTime: '2020-09-16T08:36:55.768Z',
          UserName: 'Carlos'
        }
      })
    );
  });
});
