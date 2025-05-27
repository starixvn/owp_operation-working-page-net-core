import React from 'react';
import { render, screen } from '@testing-library/react';
import ProviderVerificationAppNative from './ProviderVerificationAppNative';

jest.mock('./app/hookNative', () => ({
  useAppDispatch: jest.fn(),
  useAppSelector: jest.fn(),
}));

jest.mock('./feature/table/tableThunk', () => ({
  getConfirmProvider: jest.fn(() => ({ type: 'dummy' })),
  getLSTBookProvider: jest.fn(() => ({ type: 'dummy' })),
}));

jest.mock('../Shared/customHook/usePermission', () => jest.fn());

jest.mock('../components/Loading', () => () => <div>Loading...</div>);
jest.mock('./feature/filter/Filter', () => () => <div>Filter Component</div>);
jest.mock('./feature/table/Table', () => () => <div>Table Component</div>);
jest.mock('../Shared/createTicketNew/CreateTicketLGAView', () => () => <div>CreateTicketLGAView Component</div>);
jest.mock('../Shared/chatBoardNew/TicketDetailBoard', () => () => <div>TicketDetailBoard Component</div>);

import { useAppDispatch, useAppSelector } from './app/hookNative';
import { getConfirmProvider, getLSTBookProvider } from './feature/table/tableThunk';
import usePermission from '../Shared/customHook/usePermission';

describe('ProviderVerificationAppNative', () => {
  const mockDispatch = jest.fn();

  beforeEach(() => {
    jest.clearAllMocks();

    (useAppDispatch as jest.Mock).mockReturnValue(mockDispatch);
  });

  it('dispatches getConfirmProvider on mount', () => {
    (useAppSelector as jest.Mock).mockImplementation(selector =>
      selector({
        common: { confirmProvider: { loadingConfirmProviderOption: 'idle' } },
        filterState: { filter: { eventDate: '2023-01-01' } }
      })
    );

    (usePermission as jest.Mock).mockReturnValue({
      permission: { Username: 'user1', ActorId: 'actor1' }
    });

    render(<ProviderVerificationAppNative />);

    expect(mockDispatch).toHaveBeenCalledWith(getConfirmProvider());
  });


  it('dispatches getLSTBookProvider with eventDate on mount', () => {
    (useAppSelector as jest.Mock).mockReturnValue({
      loadingConfirmProviderOption: 'idle',
      eventDate: '2023-01-01',
    });
    (usePermission as jest.Mock).mockReturnValue({
      permission: { Username: 'user1', ActorId: 'actor1' },
    });

    render(<ProviderVerificationAppNative />);

    expect(mockDispatch).toHaveBeenCalledWith(getLSTBookProvider({ eventDate: new Date('2023-01-01') }));
  });

  it('renders Loading when loadingConfirmProviderOption is pending', () => {
    (useAppSelector as jest.Mock).mockReturnValue({
      loadingConfirmProviderOption: 'pending',
      eventDate: '2023-01-01',
    });
    (usePermission as jest.Mock).mockReturnValueOnce({
      permission: { Username: 'user1', ActorId: 'actor1' },
    }).mockReturnValueOnce({
      permission: { Username: 'user1' },
    });

    render(<ProviderVerificationAppNative />);

    expect(screen.getByText(/Loading/i)).toBeInTheDocument();
  });

  it('renders Loading when Username is missing', () => {
    (useAppSelector as jest.Mock).mockReturnValue({
      loadingConfirmProviderOption: 'idle',
      eventDate: '2023-01-01',
    });
    (usePermission as jest.Mock).mockReturnValueOnce({
      permission: { Username: '', ActorId: 'actor1' },
    }).mockReturnValueOnce({
      permission: { Username: '' },
    });

    render(<ProviderVerificationAppNative />);

    expect(screen.getByText(/Loading/i)).toBeInTheDocument();
  });

  it('renders main components when permissions and loading are valid', () => {
    (useAppSelector as jest.Mock).mockReturnValue({
      loadingConfirmProviderOption: 'idle',
      eventDate: '2023-01-01',
    });
    (usePermission as jest.Mock).mockReturnValueOnce({
      permission: { Username: 'user1', ActorId: 'actor1' },
    }).mockReturnValueOnce({
      permission: { Username: 'user1', ActorId: 'actor1' },
    });

    render(<ProviderVerificationAppNative />);

    expect(screen.getByText('Filter Component')).toBeInTheDocument();
    expect(screen.getByText('Table Component')).toBeInTheDocument();
    expect(screen.getByText('CreateTicketLGAView Component')).toBeInTheDocument();
    expect(screen.getByText('TicketDetailBoard Component')).toBeInTheDocument();
  });
});
