import { useSelector, useDispatch, createSelectorHook } from 'react-redux';
import { render, waitFor } from '@testing-library/react';
import '@testing-library/jest-dom';
import OSTable from './OSTable';

jest.mock('react-redux', () => ({
  useSelector: jest.fn(),
  useDispatch: jest.fn(),
  createSelectorHook: jest.fn()
}));

const mockedDispatch = jest.fn();

useDispatch.mockReturnValue(mockedDispatch);
const { ResizeObserver } = window;
describe('os table under test', () => {
  beforeEach(() => {
    useSelector.mockImplementation(selectorFn =>
      selectorFn({
        osAssignmentTable: {
          entities: [],
          sortState: {
            orderType: '',
            columnKey: '',
          },
          filterState: [],
          collapseExpandAllRowState: {
            columnKey: 'Trader',
            mode: 'expand',
          },
        },
      })
    );
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
  it('render window resize', async () => {
    let aResizeEventListenerWasAddedToWindow = false;

    const originalMethod = window.addEventListener;
    const spy = jest.spyOn(window, 'addEventListener');
    spy.mockImplementation((...args) => {
      originalMethod(...args);
      const [eventType] = args;
      if (eventType === 'resize') {
        aResizeEventListenerWasAddedToWindow = true;
      }
    });

    render(<OSTable />);
    await waitFor(() =>
      expect(aResizeEventListenerWasAddedToWindow).toBeTruthy()
    );
  });
});
