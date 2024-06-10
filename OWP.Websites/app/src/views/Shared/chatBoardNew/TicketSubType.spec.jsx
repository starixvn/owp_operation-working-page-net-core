import { TicketSubType } from './TicketSubType';
import { render, screen, fireEvent } from '@testing-library/react';
import { useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import Select from 'react-select';

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
        ticketSubTypeId: 1,
        ticketSubTypeName: 'League'
      },
      ticketSubTypeOptions: []
    }
  })
);

describe('test for TicketSubType', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });
  it('TicketSubType should render correct', () => {
    const options = [
      {
        value: 1,
        label: 'League'
      }
    ];
    const { container, getByText } = render(
      <TicketSubType options={options} />
    );
    fireEvent.focus(container.querySelector('input'));
    fireEvent.keyDown(container.querySelector('input'), {
      key: 'ArrowDown',
      code: 40
    });
    fireEvent.click(getByText('League'));
    expect(container.querySelector('input')).toBeInTheDocument();
  });

  it('change select should call dispatch', () => {
    const options = [
      {
        value: 2,
        label: 'ABC'
      }
    ];
    const { container } = render(<TicketSubType options={options} />);

    const selectComp = container.querySelector('#react-select-3-input');
    fireEvent.change(selectComp, {
      target: {
        label: 'DEF',
        value: 3
      }
    });
    expect(setState).toHaveBeenCalledTimes(1);
  });
});
