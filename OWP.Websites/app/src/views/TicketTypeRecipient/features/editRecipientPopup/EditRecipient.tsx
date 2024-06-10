import RecipientTable from './RecipientTable';
import { Recipient } from '../TypeNative';
import RecipientSuggestion from '../../../Shared/TicketSetting/RecipientSuggestion/RecipientSuggestion';
import { useSelector } from 'react-redux';
import { RootState } from '../../app/storeNative';
import { Permission } from '../../../Shared/TicketSetting/TypeNative';
import useStore from '../../app/zustand/ticketTypeStore';
import { getRecipientBySubtypeName } from '../../helper/getRecipientBySubTypeName';

const EditRecipient = () => {
  const permission = useSelector((state: RootState) => state.policy.permission);
  const siteId = (permission as Permission).SiteId;
  const recipients = useStore(state => state.recipients);
  const subtypeSelected = useStore(state => state.subtypeSelected);
  const storeRecipientExternal = useStore(state => state.setRecipients);
  const recipientBySubtypes = getRecipientBySubtypeName(
    recipients,
    subtypeSelected.subtypeName
  );

  const handleAddRecipient = (recipient: Recipient) => {
    const assignRecipient = { ...recipient };
    assignRecipient.subtypeName = subtypeSelected.subtypeName;
    const newRecipients = [...recipients, assignRecipient];
    storeRecipientExternal(newRecipients);
  };

  const handleDeleteRecipient = (recipient: Recipient) => {
    const newRecipients = recipients.filter(
      x => !isRecipientEqual(x, recipient)
    );
    storeRecipientExternal(newRecipients);
  };

  const isRecipientEqual = (a: Recipient, b: Recipient) =>
    a.recipientId === b.recipientId &&
    a.recipientType === b.recipientType &&
    a.subtypeName === b.subtypeName;

  return (
    <>
      <RecipientSuggestion
        currentRecipients={recipientBySubtypes}
        isIncludeGroup
        isIncludeSection
        siteId={siteId}
        handleAddRecipient={handleAddRecipient}
      />
      <div className='tableFixHead'>
        <RecipientTable
          recipients={recipientBySubtypes}
          handleDeleteRecipient={handleDeleteRecipient}
        />
      </div>
    </>
  );
};

export default EditRecipient;
