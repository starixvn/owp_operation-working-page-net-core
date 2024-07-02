import { useState, createRef } from 'react';
import CreateTicketLGAView from './CreateTicketLGAView';
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
  selectedRecipients,
  subTypeSelected,
  isOpenCreateTicketInLGA,
  modelByRowIndex
) => {
  const initialState = {
    createTicketBoard: {
      selectedRecipients: selectedRecipients,
      subTypeSelected: subTypeSelected,
      isOpenCreateTicketInLGA: isOpenCreateTicketInLGA
    },
    chatBoard: {
      modelByRowIndex: modelByRowIndex
    }
  };
  useState.mockImplementation(init => [init, setState]);
  useSelector.mockImplementation(selectorFn => selectorFn(initialState));
  useDispatch.mockReturnValue(mockDispatch);
  createRef.mockReturnValue(ref);
};
const flushPromises = () => new Promise(resolve => setImmediate(resolve));
const recipient = {
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
const subTypeSelected = 1;
const subTypeOption = [
  { value: null, label: '' },
  { value: 1, label: 'abc' },
  { value: 2, label: 'xyz' }
];
const modelByRowIndex = {
  OWPMatchId: 869324,
  Home: 'B',
  Away: 'C',
  League: 'A',
  EventDateTime: '2022-10-21T13:01:00-04:00'
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
const responseDataSubType = {
  data: [
    {
      subTypeId: 11,
      subTypeName: 'Verify League M UPDATE11'
    }
  ]
};
describe('CreateTicketLGAView under test', () => {
  beforeEach(() => {
    Service.getSubType.mockResolvedValue(responseDataSubType);
    Service.queryAssigneesByOwpMatchId.mockResolvedValue(recipient);
  });
  afterEach(() => {
    jest.clearAllMocks();
  });

  it('Should dispatch create event when submit', async () => {
    const creatorInfo = {
      recipientId: 1,
      recipientType: 1,
      recipientName: 'sa',
      recipientNickname: 'sa_nick'
    };
    setup(selectedRecipients, subTypeSelected, true, modelByRowIndex);
    const { container } = render(
      <CreateTicketLGAView creatorInfo={creatorInfo} />
    );
    const button = container.querySelector('.button-submit');
    fireEvent.click(button);
    await flushPromises();
    expect(post).toHaveBeenCalledTimes(1);
    expect(mockDispatch).toHaveBeenCalledTimes(2);
  });
  it('Should dispatch error', async () => {
    const creatorInfo = {
      recipientId: 1,
      recipientType: 1,
      recipientName: 'sa',
      recipientNickname: 'sa_nick'
    };
    setup([], subTypeSelected, true, modelByRowIndex);
    const { container } = render(
      <CreateTicketLGAView creatorInfo={creatorInfo} />
    );
    const button = container.querySelector('.button-submit');
    fireEvent.click(button);
    await flushPromises();
    expect(mockDispatch).toHaveBeenCalledTimes(1);
  });
});
