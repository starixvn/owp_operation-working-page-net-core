import { useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { fireEvent, render } from '@testing-library/react';
import IconOpenMessage from './IconOpenMessage';

jest.mock('react-redux', () => ({
  useSelector: jest.fn(),
  useDispatch: jest.fn(),
  useEffect: jest.fn()
}));

jest.mock('react', () => ({
  ...jest.requireActual('react'),
  useState: jest.fn()
}));

const mockDispatch = jest.fn();
const mockSetState = jest.fn();

const setup = () => {
  const state = {
    policy: {
      permission: {
        HasWrite: true
      }
    }
  };
  useState.mockImplementation(() => [true, mockSetState]);
  useDispatch.mockReturnValue(mockDispatch);
  useSelector.mockImplementation(selectorFn => selectorFn({ ...state }));
};
const mockHandleShowTicket = jest.fn();
const createProps = IsRead => {
  const dataSource = [
    {
      IsRead,
      TicketList: []
    }
  ];
  const rowIndex = 0;
  return { dataSource, rowIndex, handleShowTicket: mockHandleShowTicket };
};

describe('IconOpenMessage under test', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });

  it('isRead=true, should render className `icon-message`', () => {
    setup();
    const props = createProps(true);
    const { container } = render(<IconOpenMessage {...props} />);
    const element = container.querySelector('.icon-message');
    expect(element).toBeInTheDocument();
  });

  it('isRead=true, should render className `icon-unread-message`', () => {
    setup();
    const props = createProps(false);
    const { container } = render(<IconOpenMessage {...props} />);
    const element = container.querySelector('.icon-unread-message');
    expect(element).toBeInTheDocument();
  });

  it('when click Icon message, should render TicketListPopup', () => {
    setup();
    const props = createProps(true);
    const { container } = render(<IconOpenMessage {...props} />);
    const button = container.querySelector('.icon-message');
    fireEvent.click(button);
    const popup = container.querySelector('.ticket-list-popup');
    expect(popup).toBeInTheDocument();
  });
});
