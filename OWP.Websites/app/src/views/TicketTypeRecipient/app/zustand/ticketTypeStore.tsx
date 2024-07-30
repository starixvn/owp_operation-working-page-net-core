import { create } from 'zustand';
import { Recipient, Subtype } from '../../features/TypeNative';
import axios from 'axios';
import { saveTicketTypeRecipientsRoute } from '../../Const.json';
import { netCoreURLTicketTypeRecipient } from '../../apiUrl';

type ticketTypeState = {
  subtypeSelected: Subtype;
  subtypes: Array<Subtype>;
  recipients: Array<Recipient>;
  setSubtypeSelected: (subtypeSelected: Subtype) => void;
  setRecipients: (recipient: Array<Recipient>) => void;
  setSubtypes: (subtypes: Array<Subtype>) => void;
  saveData: (ticketTypeId: Required<number>) => Promise<boolean>;
};

const useStore = create<ticketTypeState>((set, get) => ({
  subtypeSelected: {
    subtypeId: 0,
    subtypeName: '',
    typeId: 0
  },
  subtypes: [],
  recipients: [],
  setSubtypeSelected: (subtypeSelected: Subtype) =>
    set({
      subtypeSelected
    }),
  setRecipients: (recipients: Array<Recipient>) =>
    set({
      recipients
    }),
  setSubtypes: (subtypes: Array<Subtype>) =>
    set({
      subtypes
    }),
  saveData: async (ticketTypeId: Required<number>): Promise<boolean> => {
    const url = `${netCoreURLTicketTypeRecipient}/${saveTicketTypeRecipientsRoute}`;

    const { recipients, subtypes } = get();

    if (subtypes.some(subtype => !subtype.subtypeName)) {
      return false;
    }
    const postModel = {
      ticketTypeId,
      recipients,
      subtypes
    };
    await axios.post(url, postModel);
    return true;
  }
}));

export default useStore;
