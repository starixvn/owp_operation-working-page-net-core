import { useState, createRef } from 'react';
import CreateTicketBoardEvent from './CreateTicketBoardEvent';
import { useSelector, useDispatch } from 'react-redux';
import { render, fireEvent } from '@testing-library/react';
import Spinner from '../../components/Spinner';
import { noop } from 'lodash';
import { post } from '../Service/Service';
import Service from './Service';
import ToastrWrapper from '../../components/ToastrWrapper';
import { setImmediate } from 'timers';

const setState = jest.fn();
const mockDispatch = jest.fn();
const handleBuildModel = jest.fn();
jest.mock('react-redux', () => ({
  useSelector: jest.fn(),
  useDispatch: jest.fn(),
  createRef: jest.fn()
}));
jest.mock('react', () => ({
  ...jest.requireActual('react'),
  useState: jest.fn(),
  createRef: jest.fn()
}));
jest.mock('../customHook/usePermission');
jest.mock('../../components/Spinner');
Spinner.show.mockImplementation(() => noop());
Spinner.hide.mockImplementation(() => noop());

jest.mock('../Service/Service');
jest.mock('./Service');
jest.mock('../../components/ToastrWrapper');
ToastrWrapper.showInfo.mockImplementation(() => noop());
ToastrWrapper.showError.mockImplementation(() => noop());
const ref = { current: { value: 'test' } };

const setup = (
  valueEvent,
  selectedRecipients,
  recipientAssigned,
  subTypeSelected,
  typeTicketText,
  valueSearch
) => {
  const initialState = {
    createTicketBoard: {
      valueEvent,
      selectedRecipients,
      recipientAssigned,
      subTypeSelected,
      typeTicketText,
      valueSearch
    }
  };

  useState.mockImplementation(init => [init, setState]);
  useSelector.mockImplementation(selectorFn => selectorFn(initialState));
  useDispatch.mockReturnValue(mockDispatch);
  createRef.mockReturnValue(ref);
};

const flushPromises = () => new Promise(resolve => setImmediate(resolve));
const subTypeSelected = 1;
const subTypeOption = [
  { value: null, label: '' },
  { value: 1, label: 'abc' },
  { value: 2, label: 'xyz' }
];
const event = {
  owpLeagueId: 32459,
  leagueName: '*UEFA EUROPA LEAGUE',
  homeId: 123,
  awayId: 141,
  homeName: 'A',
  awayName: 'B',
  eventDateTimeGmt8: '2021-10-01T00:45:00',
  isLeague: false
};
const recipientByCreatorId = {
  ticketTypeRecipientId: 1,
  recipientId: 1,
  recipientType: 1,
  recipientName: 'sa',
  recipientNickname: 'sa_nick'
};
const recipientNull = [];
const recipient = [
  {
    ticketTypeRecipientId: 1,
    recipientId: 1,
    recipientType: 1,
    recipientName: 'sa',
    recipientNickname: 'sa_nick'
  },
  {
    ticketTypeRecipientId: 2,
    recipientId: 2,
    recipientType: 1,
    recipientName: 'sa2',
    recipientNickname: 'sa_nick2'
  }
];
const valueEvent = {
  owpMatchId: 869324,
  home: 'B',
  away: 'C',
  league: 'A',
  eventDateTime: '2022-10-21T13:01:00-04:00'
};
const fetchDataEvent = {
  data: [
    {
      owpLeagueId: 32459,
      leagueName: '*UEFA EUROPA LEAGUE',
      homeId: 123,
      awayId: 141,
      homeName: 'A',
      awayName: 'B',
      eventDateTimeGmt8: '2021-10-01T00:45:00',
      isLeague: false
    },
    {
      owpLeagueId: 12312,
      leagueName: '*UEFA EUROPA LEAGUE',
      homeId: 0,
      awayId: 0,
      homeName: null,
      awayName: null,
      eventDateTimeGmt8: '2021-10-01T00:45:00',
      isLeague: false
    }
  ]
};
const responseDataSubType = {
  data: [
    {
      subTypeId: 11,
      subTypeName: 'Verify League M UPDATE11'
    }
  ]
};
const selectedRecipients = [
  {
    ticketTypeRecipientId: 1,
    recipientId: 1,
    recipientType: 1,
    recipientName: 'sa',
    recipientNickname: 'sa_nick'
  }
];
const recipientData = {
  data: [
    {
      ticketTypeRecipientId: 1,
      recipientId: 1,
      recipientType: 1,
      recipientName: 'sa',
      recipientNickname: 'sa_nick'
    },
    {
      ticketTypeRecipientId: 2,
      recipientId: 2,
      recipientType: 1,
      recipientName: 'sa2',
      recipientNickname: 'sa_nick2'
    }
  ]
};
describe('CreateTicketBoard under test', () => {
  beforeEach(() => {
    Service.event.mockResolvedValue(fetchDataEvent);
    Service.getSubType.mockResolvedValue(responseDataSubType);
    Service.queryAssigneesByOwpMatchId.mockResolvedValue(recipientData);
  });
  afterEach(() => {
    jest.clearAllMocks();
  });

  it('Should dispatch create event when submit', async () => {
    setup(
      valueEvent,
      selectedRecipients,
      recipient,
      subTypeSelected,
      'Live Incident',
      'sad'
    );

    const { container } = render(
      <CreateTicketBoardEvent
        creatorInfo={recipientByCreatorId}
        handleBuildModel={handleBuildModel}
        ticketType={4}
      />
    );
    const button = container.querySelector('.button-submit');
    fireEvent.click(button);
    await flushPromises();
    expect(post).toHaveBeenCalledTimes(1);
    expect(handleBuildModel).toHaveBeenCalledTimes(1);
    expect(mockDispatch).toHaveBeenCalledTimes(3);
  });
  it('Should dispatch error', async () => {
    setup(null, [], recipient, subTypeSelected, 'Event Checking', 'sad');

    const { container } = render(
      <CreateTicketBoardEvent
        creatorInfo={recipientByCreatorId}
        handleBuildModel={handleBuildModel}
        ticketType={2}
      />
    );
    const button = container.querySelector('.button-submit');
    fireEvent.click(button);
    await flushPromises();
    expect(mockDispatch).toHaveBeenCalledTimes(2);
  });
});
