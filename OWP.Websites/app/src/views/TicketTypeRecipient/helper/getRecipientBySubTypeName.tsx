import { Recipient } from '../features/TypeNative';

const getRecipientBySubtypeName = (
  recipients: Recipient[],
  subtypeName: string
) => {
  const findRecipientList = recipients.filter(
    x => x.subtypeName === subtypeName
  );

  return findRecipientList;
};

export { getRecipientBySubtypeName };
