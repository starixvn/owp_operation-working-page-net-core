import { fireEvent, render } from '@testing-library/react';
import { Dispatch, SetStateAction, useState } from 'react';
import ToastrWrapper from '../../../components/ToastrWrapper';
import RecipientSuggestion from '../../../Shared/TicketSetting/RecipientSuggestion/RecipientSuggestion';
import { post } from '../../Service/Service';

jest.mock('react', () => ({
  ...(jest.requireActual('react') as object),
  useState: jest.fn()
}));

jest.mock('react-redux', () => ({
  useEffect: jest.fn()
}));

const mockSetState = jest.fn() as Dispatch<SetStateAction<any>>;

(useState as jest.Mock).mockImplementation(init => [init, mockSetState]);

const fetchData = {
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

jest.mock('../../Service/Service');
(post as jest.Mock).mockResolvedValue(fetchData);
jest.mock('../../../components/ToastrWrapper');
(ToastrWrapper.showError as jest.Mock).mockImplementation(f => f());

const mockHandleAddRecipient = jest.fn();

const props = {
  currentRecipients: [
    {
      ticketTypeRecipientId: 1,
      recipientId: 1,
      recipientType: 1,
      recipientName: 'sa',
      recipientNickname: 'sa_nick'
    }
  ],
  handleAddRecipient: mockHandleAddRecipient,
  isIncludeGroup: false
};

describe('RecipientSuggestion under test', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });

  it('should query recipients when mounted', () => {
    render(<RecipientSuggestion siteId={0} {...props} />);
    expect(post).toHaveBeenCalledTimes(1);
    expect(mockSetState).toHaveBeenCalledTimes(1);
    expect(mockSetState).toHaveBeenCalledWith([]);
  });

  it('should set search value when changing recipient', () => {
    const { container } = render(<RecipientSuggestion siteId={0} {...props} />);
    const input = container.querySelector('.input-field');
    input && fireEvent.change(input, { target: { value: 'sa' } });
    expect(mockSetState).toHaveBeenCalledTimes(3);
    expect(mockSetState).toHaveBeenCalledWith('sa');
  });
});
