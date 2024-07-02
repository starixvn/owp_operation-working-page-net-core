import { render, waitFor } from '@testing-library/react';
import { useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import CreateTicketPlusCircle from './CreateTicketPlusCircle';

jest.mock('react', () => ({
  ...jest.requireActual('react'),
  useState: jest.fn()
}));
jest.mock('react-redux', () => ({
  useSelector: jest.fn(),
  useDispatch: jest.fn()
}));

const mockedDispatch = jest.fn();
const mockSetState = jest.fn();
const setUp = isOpenTicket => {
  useSelector.mockImplementation(selectorFn =>
    selectorFn({
      chatBoard: {
        modelByRowIndex: {
          LeagueId: 1,
          InfoSiteMatchId: 123,
          PreMatchIssueId: 3456,
          LeagueName: 'League',
          EventDateTime: new Date(
            'Fri Sep 11 2020 15:04:38 GMT+0700 (Indochina Time)'
          ),
          HomeName: 'Home',
          AwayName: 'Away'
        },
        isOpenTicket
      },
      createTicketBoard: {
        selectedRecipients: [
          {
            recipientId: 25334034,
            groupName: null,
            userName: 'CPHSAN',
            nickName: 'CPHSan',
            recipientType: 1
          }
        ]
      },
      subTypeSelected: 1
    })
  );
  useDispatch.mockReturnValue(mockedDispatch);
  useState.mockImplementation(init => [init, mockSetState]);
};

describe('test CreateTicketPlusCircle', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });
  it('isOpenPopup = true, should render ViewCreateTicketBoard', () => {
    setUp(true);
    const props = {
      creatorInfo: {
        groupName: null,
        nickName: 'sa',
        recipientId: 8,
        recipientType: 1,
        userName: 'sa'
      },
      selectedRecipients: [
        {
          recipientId: 25334034,
          groupName: null,
          userName: 'CPHSAN',
          nickName: 'CPHSan',
          recipientType: 1
        }
      ],
      modelByRowIndex: {
        LeagueId: 1,
        InfoSiteMatchId: 123,
        PreMatchIssueId: 3456,
        LeagueName: 'League',
        EventDateTime: new Date(
          'Fri Sep 11 2020 15:04:38 GMT+0700 (Indochina Time)'
        ),
        HomeName: 'Home',
        AwayName: 'Away'
      },
      subTypeSelected: 1
    };
    const { container } = render(<CreateTicketPlusCircle {...props} />);
    const element = container.querySelector('.chat-board-popup');
    waitFor(() => expect(element).toBeInTheDocument());
  });
});
