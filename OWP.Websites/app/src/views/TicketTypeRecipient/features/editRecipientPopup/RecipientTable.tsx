import { DeleteIcon, AccountMultipleIcon, PersonIcon } from 'mdi-react';
import { Recipient } from '../TypeNative';
import SectionIcon from '../../../Shared/recipientSelect/SectionIcon';
import MatchAssigneeIcon from '../../../Shared/recipientSelect/MatchAssigneeIcon';
import { TicketTypeRecipientEnum } from '../../../Shared/enum/TicketRecipientEnum';
import { orderBy } from 'lodash';

interface Props {
  recipients: Recipient[];
  handleDeleteRecipient: Function;
}

const RecipientTable = (props: Props) => {
  const renderDeleteIcon = (recipient: Recipient) => (
    <span
      className='icon-edit'
      onClick={() => props.handleDeleteRecipient(recipient)}>
      <DeleteIcon size={12} />
    </span>
  );

  const getIcon = (recipient: Recipient) => {
    const icons = new Map();
    icons.set(TicketTypeRecipientEnum.User, <PersonIcon />);
    icons.set(TicketTypeRecipientEnum.Group, <AccountMultipleIcon />);
    icons.set(TicketTypeRecipientEnum.Section, <SectionIcon />);
    icons.set(TicketTypeRecipientEnum.Assignee, <MatchAssigneeIcon />);
    return icons.get(recipient.recipientType);
  };

  const renderRecipient = () => {
    const sortedRecipients = orderBy(
      props.recipients,
      ['recipientType', 'recipientName'],
      ['desc', 'asc']
    );
    return sortedRecipients.map(recipient => (
      <tr key={`${recipient.recipientId} - ${recipient.recipientType}`}>
        <td>
          {getIcon(recipient)} {recipient.recipientName}
        </td>
        <td>{renderDeleteIcon(recipient)}</td>
      </tr>
    ));
  };

  return (
    <table className='recipient-table'>
      <thead>
        <tr>
          <th>Recipient</th>
          <th>Action</th>
        </tr>
      </thead>
      <tbody>{renderRecipient()}</tbody>
    </table>
  );
};

export default RecipientTable;
