import { fireEvent, render, screen } from '@testing-library/react';
import { TicketTypeRecipientEnum } from '../../enum/TicketRecipientEnum';
import { TicketSettingRecipient } from '../TypeNative';
import SuggestionList from './SuggestionList';

const mockHandleAddRecipient = jest.fn();

const suggestions: TicketSettingRecipient[] = [
  {
    recipientId: 1,
    recipientType: TicketTypeRecipientEnum.User,
    recipientName: 'sa',
    recipientNickname: ''
  },
  {
    recipientId: 2,
    recipientType: TicketTypeRecipientEnum.Group,
    recipientName: 'CSD',
    recipientNickname: ''
  }
];

const props = {
  suggestions,
  handleAddRecipient: mockHandleAddRecipient
};

describe('SuggestionList under test', () => {
  it('should render 2 rows', () => {
    const { container } = render(<SuggestionList {...props} />);
    expect(container.querySelectorAll('.suggestion-item').length).toEqual(2);
  });

  it('should call props.handleAddRecipient when click Add a recipient', () => {
    const { container } = render(<SuggestionList {...props} />);
    fireEvent.click(screen.getAllByText('Add')[0]);
    expect(mockHandleAddRecipient).toHaveBeenCalledTimes(1);
    expect(mockHandleAddRecipient).toHaveBeenCalledWith(suggestions[0]);
  });
});
