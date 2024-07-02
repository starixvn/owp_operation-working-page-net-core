import { fireEvent, render } from '@testing-library/react';
import { useDispatch, useSelector } from 'react-redux';
import TableMaxbetGeneral from './TableMaxbetGeneral';

jest.mock('react-redux', () => ({
  useSelector: jest.fn(),
  useDispatch: jest.fn(),
}));

const mockDispatch = jest.fn();

const setUp = pending => {
  useSelector.mockImplementation(selectorFn =>
    selectorFn({
      maxbetGeneralReducer: {
        maxbetGeneralData: [],
        maxbetGeneralPending: pending,
      },
      showHidePopupFilterReducer: {
        filterKey: '',
        isDisplay: '',
      },
    })
  );
};
const { ResizeObserver } = window;
describe('test TableMaxbetGeneral component', () => {
  beforeEach(() => {
    useDispatch.mockReturnValue(mockDispatch);
    //@ts-ignore
    delete window.ResizeObserver;
    window.ResizeObserver = jest.fn().mockImplementation(() => ({
      observe: jest.fn(),
      unobserve: jest.fn(),
      disconnect: jest.fn(),
    }));
  });
  afterEach(() => {
    window.ResizeObserver = ResizeObserver;
    jest.restoreAllMocks();
  });
  const props = {
    title: 'abc',
    filterKey: 'a',
    keyword: 'b',
  };
  it('render correct loading component', () => {
    setUp(true);
    const { container } = render(<TableMaxbetGeneral {...props} />);

    expect(container.querySelector('.loading-process')).toBeInTheDocument();
  });

  it('render correct table component', () => {
    setUp(false);
    const { container } = render(<TableMaxbetGeneral {...props} />);
    expect(container.querySelector('.table')).toBeInTheDocument();
  });
});
