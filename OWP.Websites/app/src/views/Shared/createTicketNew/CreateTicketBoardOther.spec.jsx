import { useState, createRef } from 'react';
import CreateTicketBoardOther from './CreateTicketBoardOther';
import usePermission from '../customHook/usePermission';
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
  isOpenPopup,
  isOthers,
  mockPermission,
  valueSearch,
  valueOther,
  selectedRecipients,
  valueEvent,
  errorOther,
  errorInput,
  errorRecipient,
  errorTextField,
  subTypeOption,
  subTypeSelected
) => {
  const initialState = {
    createTicketBoard: {
      isOpenPopup: isOpenPopup,
      isOthers: isOthers,
      valueSearch: valueSearch,
      valueOther: valueOther,
      selectedRecipients: selectedRecipients,
      valueEvent: valueEvent,
      errorOther: errorOther,
      errorInput: errorInput,
      errorRecipient: errorRecipient,
      errorTextField: errorTextField,
      subTypeOption: subTypeOption,
      subTypeSelected: subTypeSelected
    }
  };

  useState.mockImplementation(init => [init, setState]);
  useSelector.mockImplementation(selectorFn => selectorFn(initialState));
  useDispatch.mockReturnValue(mockDispatch);
  usePermission.mockReturnValue({
    permission: mockPermission,
    error: null
  });
  createRef.mockReturnValue(ref);
};

const flushPromises = () => {
  return new Promise(resolve => setImmediate(resolve));
};
const subTypeSelected = 1;
const subTypeOption = [
  { value: null, label: '' },
  { value: 1, label: 'abc' },
  { value: 2, label: 'xyz' }
];
const recipientByCreatorId = {
  userName:'sa',
  ticketTypeRecipientId: 1,
  recipientId: 1,
  recipientType: 1,
  recipientName: 'sa',
  recipientNickname: 'sa_nick'
};
const inputOtherLargerMaximum =
  'ddddddddddddaaaaaaaaaaaaaaadddddddddddddddddadadadddddsssssssssssssssssddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaassssdadaddddddddddddddddddddddddddddddddddddddddddddddddddddddddd';
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
describe('CreateTicketBoard under test', () => {
  beforeEach(() => {
    Service.event.mockResolvedValue(fetchDataEvent);
    Service.getSubType.mockResolvedValue(responseDataSubType);
  });
  afterEach(() => {
    jest.clearAllMocks();
  });

  it('should return empty when not open', () => {
    const mockPermission = {
      Username: 'sa',
      ActorId: 8,
      HasRead: true,
      HasWrite: true
    };
    setup(false, false, mockPermission);

    const { container } = render(
      <CreateTicketBoardOther creatorInfo={recipientByCreatorId} />
    );
    const buttonBlock = container.querySelector('.chat-board-popup');
    expect(buttonBlock).not.toBeInTheDocument();
  });
 
  it('should dispatch close popup when clicking close icon', () => {
    const mockPermission = {
      Username: 'sa',
      ActorId: 8,
      HasRead: true,
      HasWrite: true
    };
    setup(
      true,
      true,
      mockPermission,
      'sa',
      '',
      recipientNull,
      null,
      false,
      false,
      false,
      'abc',
      subTypeOption
    );

    const { container } = render(
      <CreateTicketBoardOther creatorInfo={recipientByCreatorId} />
    );
    const button = container.querySelector('.close-chat-board-popup');
    fireEvent.click(button);
    expect(mockDispatch).toHaveBeenCalledTimes(2);
  });

  it('should dispatch close popup when clicking cancel button', () => {
    const mockPermission = {
      Username: 'sa',
      ActorId: 8,
      HasRead: true,
      HasWrite: true
    };
    setup(
      true,
      true,
      mockPermission,
      'sa',
      '',
      recipientNull,
      null,
      false,
      false,
      false,
      'abc',
      subTypeOption
    );

    const { container } = render(
      <CreateTicketBoardOther creatorInfo={recipientByCreatorId} />
    );
    const button = container.querySelector('.button-close');
    fireEvent.click(button);
    expect(mockDispatch).toHaveBeenCalledTimes(2);
  });

  it('Should not dispatch create when subject is empty', () => {
    const mockPermission = {
      Username: 'sa',
      ActorId: 8,
      HasRead: true,
      HasWrite: true
    };
    setup(
      true,
      true,
      mockPermission,
      'sa',
      '',
      recipientNull,
      null,
      false,
      false,
      false,
      'abc',
      subTypeOption
    );

    const { container } = render(
      <CreateTicketBoardOther creatorInfo={recipientByCreatorId} />
    );
    const button = container.querySelector('.button-submit');
    fireEvent.click(button);
    expect(mockDispatch).toHaveBeenCalledTimes(4);
  });

  it('Should dispatch create other when submit with null recipient and empty subject', async () => {
    const mockPermission = {
      Username: 'sa',
      ActorId: 8,
      HasRead: true,
      HasWrite: true
    };
    setup(
      true,
      true,
      mockPermission,
      'sa',
      '',
      recipientNull,
      null,
      false,
      false,
      false,
      'abc',
      subTypeOption,
      subTypeSelected
    );
    const { container } = render(
      <CreateTicketBoardOther creatorInfo={recipientByCreatorId} />
    );
    const button = container.querySelector('.button-submit');
    fireEvent.click(button);
    await flushPromises();
    expect(post).toHaveBeenCalledTimes(0);
    expect(mockDispatch).toHaveBeenCalledTimes(4);
  });
  it('Should dispatch create other when submit with null recipient and larger maximum length subject', async () => {
    const mockPermission = {
      Username: 'sa',
      ActorId: 8,
      HasRead: true,
      HasWrite: true
    };
    setup(
      true,
      true,
      mockPermission,
      'sa',
      inputOtherLargerMaximum,
      recipientNull,
      null,
      false,
      false,
      false,
      'abc',
      subTypeOption
    );
    const { container } = render(
      <CreateTicketBoardOther creatorInfo={{
        ...recipientByCreatorId,
        userName:'sa'
      }} />
    );
    const button = container.querySelector('.button-submit');
    fireEvent.click(button);
    await flushPromises();
    expect(post).toHaveBeenCalledTimes(0);
    expect(mockDispatch).toHaveBeenCalledTimes(4);
  });
});
