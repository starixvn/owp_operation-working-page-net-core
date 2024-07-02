import { fireEvent, render } from '@testing-library/react';
import { TicketTypeRecipientEnum } from '../../../Shared/enum/TicketRecipientEnum';
import { Recipient } from '../TypeNative';
import RecipientTable from './RecipientTable';
import { DeleteIcon, AccountMultipleIcon, PersonIcon } from 'mdi-react';

jest.mock('mdi-react', () => ({
  DeleteIcon: jest.fn(),
  AccountMultipleIcon: jest.fn(),
  PersonIcon: jest.fn()
}));

const recipients: Recipient[] = [
  {
    recipientId: 1,
    recipientName: 'sa',
    recipientNickname: '',
    recipientType: TicketTypeRecipientEnum.User,
    ticketTypeRecipientId: 1,
    subtypeName: 'sub1'
  },
  {
    recipientId: 2,
    recipientName: 'sa2',
    recipientNickname: '',
    recipientType: TicketTypeRecipientEnum.User,
    ticketTypeRecipientId: 2,
    subtypeName: 'sub1'
  },
  {
    recipientId: 1,
    recipientName: 'LST',
    recipientNickname: '',
    recipientType: TicketTypeRecipientEnum.Group,
    ticketTypeRecipientId: 3,
    subtypeName: 'sub1'
  }
];

const handleDeleteRecipient = jest.fn();

const props = {
  recipients,
  handleDeleteRecipient
};

describe('RecipientTable under test', () => {
  it('should render 3 rows', () => {
    const { container } = render(<RecipientTable {...props} />);
    const element = container.querySelector(`[class="recipient-table"]`);

    expect(element!).toBeInTheDocument();
    expect(DeleteIcon).toBeCalledTimes(3);
    expect(PersonIcon).toBeCalledTimes(2);
    expect(AccountMultipleIcon).toBeCalledTimes(1);
  });

  it('should call props.handleDeleteRecipient if DeleteIcon is clicked', () => {
    const { container } = render(<RecipientTable {...props} />);
    const element = container.querySelector(`[class="icon-edit"]`);
    fireEvent.click(element!);

    expect(props.handleDeleteRecipient).toHaveBeenCalledTimes(1);
  });
});
