import { AccountMultipleIcon, PersonIcon } from 'mdi-react';
import { TicketTypeRecipientEnum } from '../../enum/TicketRecipientEnum';
import SectionIcon from '../../recipientSelect/SectionIcon';
import { TicketSettingRecipient } from '../TypeNative';

type PropsType = {
  suggestions: TicketSettingRecipient[];
  handleAddRecipient: Function;
};

const SuggestionList = (data: PropsType) => {
  const renderSuggestion = (recipient: TicketSettingRecipient) => {
    const icons = new Map();
    icons.set(TicketTypeRecipientEnum.User, <PersonIcon />);
    icons.set(TicketTypeRecipientEnum.Group, <AccountMultipleIcon />);
    icons.set(TicketTypeRecipientEnum.Section, <SectionIcon />);

    const handleOnClickAdd = () => {
      data.handleAddRecipient(recipient);
    };

    const nickname =
      recipient.recipientNickname !== null
        ? ` (${recipient.recipientNickname})`
        : '';

    return (
      <div className='suggestion-item'>
        {icons.get(recipient.recipientType)}{' '}
        {`${recipient.recipientName}${nickname}`}
        <button className='btn-add' onClick={handleOnClickAdd}>
          Add
        </button>
      </div>
    );
  };

  const renderRecipient = () =>
    data.suggestions.map(recipient => {
      const key =
        (recipient.recipientId + recipient.recipientType).toString() +
        recipient.recipientName;

      return <li key={key}>{renderSuggestion(recipient)}</li>;
    });

  return <ul className='recipient-suggestion'>{renderRecipient()}</ul>;
};

export default SuggestionList;
