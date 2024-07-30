import { fireEvent, render, screen } from '@testing-library/react';
import { useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import Select from 'react-select';
import TicketStatus from './TicketStatus';

jest.mock('react-redux', () => ({
  useDispatch: jest.fn(),
  useEffect: jest.fn(),
  useSelector: jest.fn()
}));

jest.mock('react', () => ({
  ...jest.requireActual('react'),
  useState: jest.fn()
}));

const setState = jest.fn();
const mockDispatch = jest.fn();

useState.mockImplementation(init => [init, setState]);
useDispatch.mockReturnValue(mockDispatch);
useSelector.mockImplementation(f =>
  f({
    policy: {
      permission: { Username: 'sa', UserId: 8, HasWrite: true }
    },
    chatBoard: {
      ticketDetail: {
        ticketId: '12',
        ticketStatus: 'Closed'
      }
    }
  })
);

describe('Status Popup under test', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });
  it('change select, should call dispatch, setState 1 time ', () => {
    const { container } = render(<TicketStatus policy='policy' />);
    const element = container.querySelector('#react-select-2-input');
    fireEvent.change(element, {
      target: { label: 'Closed', value: 'Closed' }
    });
    expect(setState).toHaveBeenCalledTimes(1);
  });

  it('should render Select', () => {
    const { container } = render(<TicketStatus policy='policy' />);
    const element = container.querySelector('.select');
    expect(element).toBeInTheDocument();
  });
  it('cover case event equal with default status', () => {
    const { container } = render(<TicketStatus policy='policy' />);

    const element = container.querySelector('input');
    fireEvent.change(element, {
      target: { label: 'Open', value: 'Open' }
    });
  });
});
