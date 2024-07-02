import { useState, createRef } from 'react';
import { fireEvent, render, waitFor } from '@testing-library/react';
import TicketListPopup from './TicketListPopup';
import { useDispatch, useSelector } from 'react-redux';

jest.mock('react-redux', () => ({
  useSelector: jest.fn(),
  useDispatch: jest.fn(),
  useStore: jest.fn()
}));
jest.mock('react', () => ({
  ...jest.requireActual('react'),
  useState: jest.fn(),
  createRef: jest.fn()
}));

const mockDispatch = jest.fn();
useDispatch.mockReturnValue(mockDispatch);
useSelector.mockImplementation(selectorFn =>
  selectorFn({
    policy: {
      permission: {
        HasWrite: true
      }
    }
  })
);

describe('TicketListPopup under test', () => {
  const props = {
    ticketList: [],
    onCloseTicketList: jest.fn(),
    dataSource: [
      {
        PreMatchLeagueId: 1,
        InfoSiteMatchId: 1
      }
    ],
    rowIndex: 0
  };
  it('render correct', () => {
    const { container } = render(<TicketListPopup {...props} />);
    expect(container.querySelector('.ticket-list-popup')).toBeInTheDocument();
  });

  it('should call dispatch when click submit', () => {
    const { container } = render(<TicketListPopup {...props} />);
    const button = container.querySelector('.button-submit');

    fireEvent.click(button);

    expect(mockDispatch).toBeCalled();
  });
});
