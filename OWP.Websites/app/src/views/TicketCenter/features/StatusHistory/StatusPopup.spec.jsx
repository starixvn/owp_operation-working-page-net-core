import { useState } from 'react';
import { getStatusHistory } from '../../services/service';
import StatusPopup from './StatusPopup';
import { setImmediate } from 'timers';
import { render, screen } from '@testing-library/react';

jest.mock('../../services/service');
jest.mock('react-redux', () => ({
  useEffect: jest.fn()
}));

function flushPromises() {
  return new Promise(resolve => setImmediate(resolve));
}

jest.mock('react', () => ({
  ...jest.requireActual('react'),
  useState: jest.fn()
}));

const setState = jest.fn();
const mockHandleClosePopup = jest.fn();

const mockData = [
  {
    logId: 1,
    logDate: '2021-12-03 19:02:02.980',
    modifiedBy: 'sa',
    oldValue: 'open',
    newValue: 'closed'
  },
  {
    logId: 2,
    logDate: '2021-12-03 19:02:02.980',
    modifiedBy: 'mira',
    oldValue: 'open',
    newValue: 'closed'
  }
];
useState.mockImplementation(init => [mockData, setState]);
getStatusHistory.mockResolvedValue(mockData);

describe('Status Popup under test', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });
  it('should render table status-history-table', async () => {
    render(
      <StatusPopup ticketId={1} handleClosePopup={mockHandleClosePopup} />
    );
    expect(setState).toHaveBeenCalledTimes(0);
    await flushPromises();
    expect(screen.queryByText('mira')).toBeInTheDocument();
    expect(setState).toHaveBeenCalledTimes(1);
  });
});
