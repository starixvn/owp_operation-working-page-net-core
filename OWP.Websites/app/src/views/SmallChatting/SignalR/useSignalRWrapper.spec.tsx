import React from 'react';
import { render } from '@testing-library/react';
import '@testing-library/jest-dom/extend-expect';
import useSignalRWrapper from './useSignalRWrapper';
import NetCoreSignalR from '../../Shared/NetCoreSignalR/NetCoreSignalR';
import handleNewMessageIncome from './newMessageIncomeHandler';
import handleChangeTicketStatus from './changeStatusIncomeHandler';
import changeSubTypeIncomeHandler from './changeSubTypeIncomeHandler';
import { changeMessageIconHandler } from './changeMessageIconHandler';

jest.mock('../../Shared/NetCoreSignalR/NetCoreSignalR', () =>
  jest.fn(() => null)
);
jest.mock('./newMessageIncomeHandler');
jest.mock('./changeStatusIncomeHandler');
jest.mock('./changeSubTypeIncomeHandler');
jest.mock('./changeMessageIconHandler');

describe('useSignalRWrapper', () => {
  beforeEach(() => {
    NetCoreSignalR.mockClear();
  });

  test('renders NetCoreSignalR with correct handlers', () => {
    render(<>{useSignalRWrapper()}</>);

    const expectedHandlers = [
      {
        method: 'NotifyUpdateReactIconMessage',
        handler: changeMessageIconHandler
      },
      { method: 'NotifyNewMessageIncome', handler: handleNewMessageIncome },
      { method: 'NotifyChangeStatusIncome', handler: handleChangeTicketStatus },
      {
        method: 'NotifyChangeSubTypeIncome',
        handler: changeSubTypeIncomeHandler
      }
    ];

    expect(NetCoreSignalR).toHaveBeenCalledWith(
      expect.objectContaining({
        handlers: expectedHandlers,
        hubName: 'ticketCenterNotifyHub'
      }),
      {}
    );
  });
});
