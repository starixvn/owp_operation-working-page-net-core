import { useState, createRef } from 'react';
import ViewCreateTicketBoard from './ViewCreateTicketBoard';
import { useDispatch, useSelector } from 'react-redux';
import { fireEvent, render, waitFor } from '@testing-library/react';
import Spinner from '../../../components/Spinner';
import { noop } from 'lodash';
import { post } from '../../../Shared/Service/Service';
import Service from './Service/Service';
import ServiceCreateTicket from '../../../Shared/createTicketNew/Service';
import ToastrWrapper from '../../../components/ToastrWrapper';
import {
  filterFeedbackSubtype,
  getSubTypeByType
} from '../../../Shared/createTicketNew/Helper/HelperCreateTicket';

const setState = jest.fn();
const mockDispatch = jest.fn();

jest.mock('react-redux', () => ({
  useSelector: jest.fn(),
  useDispatch: jest.fn(),
  createRef: jest.fn()
}));

jest.mock('../../../Shared/customHook/useStableDispatch', () => ({
  __esModule: true,
  default: () => mockDispatch
}));

jest.mock('react', () => ({
  ...jest.requireActual('react'),
  useState: jest.fn(),
  createRef: jest.fn()
}));
jest.mock('../../../components/Spinner');
Spinner.show.mockImplementation(() => noop());
Spinner.hide.mockImplementation(() => noop());

jest.mock('./Service/Service');
jest.mock('../../../Shared/Service/Service');
jest.mock('../../../Shared/createTicketNew/Service');
jest.mock('../../../components/ToastrWrapper');
jest.mock('../../../Shared/createTicketNew/Helper/HelperCreateTicket');
ToastrWrapper.showInfo.mockImplementation(() => noop());
ToastrWrapper.showError.mockImplementation(() => noop());

// Mock helper functions
const {
  formatRecipientModel,
  saveFileToBackEnd
} = require('../../../Shared/createTicketNew/Helper/HelperCreateTicket');
formatRecipientModel.mockImplementation((recipients, creatorInfo) =>
  recipients.map(r => ({ ...r, creatorInfo }))
);
saveFileToBackEnd.mockResolvedValue([]);
const ref = { current: { value: 'test' } };

const setup = (
  selectedRecipients,
  errorRecipient,
  subTypeOption,
  subTypeSelected
) => {
  const initialState = {
    createTicketBoard: {
      selectedRecipients: selectedRecipients,
      errorRecipient: errorRecipient,
      subTypeOption: subTypeOption,
      subTypeSelected: subTypeSelected
    }
  };
  useState.mockImplementation(init => [init, setState]);
  useDispatch.mockReturnValue(mockDispatch);
  useSelector.mockImplementation(selectorFn => selectorFn(initialState));
  createRef.mockReturnValue(ref);
};

const fetchDataRecipientPreMatch = {
  data: {
    ticketTypeRecipientId: 1,
    recipientId: 2,
    recipientType: 1,
    recipientName: 'sa2',
    recipientNickname: 'sa2_nick'
  }
};
const fetchDataInfoSite = {
  data: {
    OwpLeagueId: 32459,
    leagueName: '*UEFA EUROPA LEAGUE',
    OwpHomeId: 123,
    OwpAwayId: 141,
    eventDateTimeGmt8: '2021-10-01T00:45:00',
    isLeague: false
  }
};
const subTypeOption = [
  { value: null, label: '' },
  { value: 1, label: 'abc' },
  { value: 2, label: 'xyz' }
];
const responseDataSubType = {
  data: [
    {
      subTypeId: 11,
      subTypeName: 'Verify League M UPDATE11'
    }
  ]
};

describe('CreateTicketBoard under test', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });

  it('should handle when clicking submit button', async () => {
    Service.GetRecipientPreMatch.mockResolvedValue(fetchDataRecipientPreMatch);
    Service.GetInfoSiteMatch.mockResolvedValue(fetchDataInfoSite);
    ServiceCreateTicket.getSubType.mockResolvedValue(responseDataSubType);
    getSubTypeByType.mockReturnValue([{ subTypeId: 1, subTypeName: 'event' }]);
    filterFeedbackSubtype.mockReturnValue([
      { subTypeId: 1, subTypeName: 'event' }
    ]);
    const selectedRecipients = [
      {
        ticketTypeRecipientId: 1,
        recipientId: 2,
        recipientType: 1,
        recipientName: 'sa2',
        recipientNickname: 'sa2_nick'
      },
      {
        ticketTypeRecipientId: 1,
        recipientId: 1,
        recipientType: 1,
        recipientName: 'sa',
        recipientNickname: 'sa_nick'
      }
    ];
    setup(selectedRecipients, false, subTypeOption, { value: 1, label: 'abc' });
    const props = {
      subTypeSelected: { value: 1, label: 'abc' },
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
          recipientName: 'CPHSAN',
          recipientNickname: 'CPHSan',
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
      }
    };

    const { container } = render(<ViewCreateTicketBoard {...props} />);
    const button = container.querySelector('.button-submit');
    fireEvent.click(button);

    await waitFor(() => {
      expect(post).toHaveBeenCalledTimes(1);
    });
  });

  it('should handle when clicking submit button not selected recipient call dispatchActionRecipient ', async () => {
    Service.GetRecipientPreMatch.mockResolvedValue(fetchDataRecipientPreMatch);
    Service.GetInfoSiteMatch.mockResolvedValue(fetchDataInfoSite);
    ServiceCreateTicket.getSubType.mockResolvedValue(responseDataSubType);
    getSubTypeByType.mockReturnValue([{ subTypeId: 1, subTypeName: 'event' }]);
    filterFeedbackSubtype.mockReturnValue([
      { subTypeId: 1, subTypeName: 'event' }
    ]);
    setup([], false, subTypeOption, { value: null, label: '' });
    const props = {
      subTypeSelected: { value: 1, label: 'abc' },
      creatorInfo: {
        groupName: null,
        nickName: 'sa',
        recipientId: 8,
        recipientType: 1,
        userName: 'sa'
      },
      selectedRecipients: [],
      modelByRowIndex: {
        LeagueId: 1,
        PreMatchIssueId: 3456,
        LeagueName: 'League',
        EventDateTime: new Date(
          'Fri Sep 11 2020 15:04:38 GMT+0700 (Indochina Time)'
        ),
        HomeName: 'Home',
        AwayName: 'Away'
      }
    };

    const { container } = render(<ViewCreateTicketBoard {...props} />);
    const button = container.querySelector('.button-submit');
    fireEvent.click(button);
    await waitFor(() => {
      expect(mockDispatch).toBeCalled();
    });
  });

  it('should handle when clicking submit button & show info created ticket', async () => {
    Service.GetRecipientPreMatch.mockResolvedValue(fetchDataRecipientPreMatch);
    Service.GetInfoSiteMatch.mockResolvedValue(fetchDataInfoSite);
    ServiceCreateTicket.getSubType.mockResolvedValue(responseDataSubType);
    getSubTypeByType.mockReturnValue([{ subTypeId: 1, subTypeName: 'event' }]);
    filterFeedbackSubtype.mockReturnValue([
      { subTypeId: 1, subTypeName: 'event' }
    ]);
    post.mockResolvedValue(true);
    const selectedRecipients = [
      {
        ticketTypeRecipientId: 1,
        recipientId: 2,
        recipientType: 1,
        recipientName: 'sa2',
        recipientNickname: 'sa2_nick'
      },
      {
        ticketTypeRecipientId: 1,
        recipientId: 1,
        recipientType: 1,
        recipientName: 'sa',
        recipientNickname: 'sa_nick'
      }
    ];
    setup(selectedRecipients, false, subTypeOption, { value: 1, label: 'abc' });
    const props = {
      subTypeSelected: { value: 1, label: 'abc' },
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
          recipientName: 'CPHSAN',
          recipientNickname: 'CPHSan',
          recipientType: 1
        }
      ],
      modelByRowIndex: {
        LeagueId: 1,
        PreMatchIssueId: 3456,
        LeagueName: 'League',
        EventDateTime: new Date(
          'Fri Sep 11 2020 15:04:38 GMT+0700 (Indochina Time)'
        ),
        HomeName: 'Home',
        AwayName: 'Away'
      }
    };

    const { container } = render(<ViewCreateTicketBoard {...props} />);
    const button = container.querySelector('.button-submit');
    fireEvent.click(button);
    await waitFor(() => {
      expect(ToastrWrapper.showInfo).toBeCalled();
    });
  });
});
