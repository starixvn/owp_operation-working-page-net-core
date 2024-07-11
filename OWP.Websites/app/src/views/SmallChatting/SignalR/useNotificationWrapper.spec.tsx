import React from 'react';
import { render } from '@testing-library/react';
import useNotificationWrapper from './useNotificationWrapper';
import NetCoreSignalR from '../../Shared/NetCoreSignalR/NetCoreSignalR';

jest.mock('../../Shared/NetCoreSignalR/NetCoreSignalR', () =>
  jest.fn(() => null)
);

describe('useNotificationWrapper', () => {
  it('renders NetCoreSignalR with correct props', () => {
    render(<>{useNotificationWrapper()}</>);

    expect(NetCoreSignalR).toHaveBeenCalledTimes(1);
  });
});
