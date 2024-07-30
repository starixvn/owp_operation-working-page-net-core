import { useState } from 'react';
import OpenTicketListHandler from './OpenTicketListHandler';
import { render } from '@testing-library/react';
import {  useStore } from 'react-redux';

jest.mock('react-redux', () => ({
  useSelector: jest.fn(),
  useDispatch: jest.fn(),
  useStore: jest.fn(),
  useEffect: jest.fn()
}));
jest.mock('react', () => ({
  ...jest.requireActual('react'),
  useState: jest.fn(),
  createRef: jest.fn(),
  useEffect: jest.fn()
}));

jest.mock('../../createTicketNew/Helper/HelperCreateTicket');

const mockSetState = jest.fn();
useStore.mockImplementation(() => {
  return {
    getState:jest.fn().mockImplementation(() => {
      return {
        pagePolicy: {
          permission: {
            HasWrite: true,
            ActorId: 8
          }
        }
      };
    })
  }});
useState.mockImplementation(init => [init, mockSetState]);

describe('OpenTicketListHandler test', () => {
  const props = {
    onShowTicketList: jest.fn(),
    updateDataSources: jest.fn(),
    dataSources: [
      {
        OWPMatchId: 123456,
        IsRead: true,
        HasTicket: true
      },
      { OWPMatchId: 1251214, IsRead: true, HasTicket: true }
    ],
    index: 0
  };
  it('open ticket list', async () => {
    const { container } = render(<OpenTicketListHandler {...props} />);
    const button = container.querySelector('.icon-message');
    expect(button).toBeTruthy();
  });
});
