import { useCallback } from 'react';
import { useSelector, useDispatch } from 'react-redux';

import { render, fireEvent, screen } from '@testing-library/react';
import HistoryTable from './HistoryTable';

jest.mock('react-redux', () => ({
  useSelector: jest.fn(),
  useDispatch: jest.fn()
}));

const setUp = (pending = true) => {
  useSelector.mockImplementation(selectorFn =>
    selectorFn({
      logHistoryReducer: {
        Pending: pending,
        Data: []
      }
    })
  );
};

describe('test EditCell component', () => {
  afterAll(() => {
    jest.clearAllMocks();
  });
  it('render correct loading component', () => {
    setUp();
    const { container } = render(<HistoryTable />);
    expect(container.querySelector('.log-loading'));
  });

  it('render correct Table component', () => {
    setUp(false);
    const { container } = render(<HistoryTable />);
    expect(container.querySelector('.product'));
  });
});
