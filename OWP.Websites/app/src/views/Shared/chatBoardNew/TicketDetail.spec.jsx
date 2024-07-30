import { fireEvent, render } from '@testing-library/react';
import { useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import TicketDetail from './TicketDetail';

const mockSetState = jest.fn();

jest.mock('react-redux', () => ({
  useSelector: jest.fn(),
  useDispatch: jest.fn()
}));
jest.mock('react', () => ({
  ...jest.requireActual('react'),
  useState: jest.fn()
}));
const mockedDispatch = jest.fn();
useState.mockImplementation(init => [init, mockSetState]);
useDispatch.mockReturnValue(mockedDispatch);
useSelector.mockImplementation(f =>
  f({
    chatBoard: {
      isDisplayCloseButton: true,
      ticketDetail: {
        koTime: new Date(),
        ticketId: 0,
        ticketTypeId: '',
        ticketTypeName: '',
        messages: [],
        owpLeagueId: '',
        owpLeagueName: '',
        owpHomeId: '',
        owpAwayId: '',
        homeName: '',
        awayName: '',
        subject: '',
        creatorInfo: {
          id: 0,
          userName: '',
          nickName: ''
        }
      },
      modelQuoteMessage: {
        message: 'ahihi',
        messageId: 42699,
        userId: 360418,
        createdDate: '2023-09-13T17:43:08.77'
      }
    },
    policy: {
      permission: { Username: 'sa', UserId: 8, HasWrite: true }
    }
  })
);

describe('test with TicketDetail component', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });

  it('should render correct', async () => {
    const { container } = render(<TicketDetail />);
    const showInfo = container.querySelector('.show-info');
    fireEvent.click(showInfo);
    expect(mockSetState).toBeCalled();
  });

  it('should call dispatch fn when click close icon', () => {
    const { getByTestId } = render(<TicketDetail />);
    const element = getByTestId('close-testId');

    fireEvent.click(element);

    expect(mockedDispatch).toBeCalled();
  });
});
