import { SubTypeOption, TRecipient } from './Type';
import { subtypeMatchCheck, subjectMaxLength } from './logWorkConst.json';

const getSubjectBySubtype = (subtype: SubTypeOption | undefined) =>
  subtype && subtype.value !== subtypeMatchCheck ? subtype.label : '';

const isMatchCheckType = (subtype: SubTypeOption | undefined) =>
  subtype && subtype?.value === subtypeMatchCheck;

const isValidMatchId = (matchId: number) => {
  const minInt32Value = -2147483648;
  const maxInt32Value = 2147483647;

  return (
    Number.isInteger(matchId) &&
    matchId >= minInt32Value &&
    matchId <= maxInt32Value
  );
};
const isErrorInput = (
  recipient: TRecipient[],
  subTypeSelected: SubTypeOption | undefined,
  subject: string
) =>
  !recipient.length ||
  (!isMatchCheckType(subTypeSelected) &&
    (!subject.length || subject.length > subjectMaxLength));

const isErrorMatchCheck = (
  subTypeSelected: SubTypeOption | undefined,
  adminSiteId: string
) => isMatchCheckType(subTypeSelected) && !adminSiteId;

export {
  getSubjectBySubtype,
  isMatchCheckType,
  isValidMatchId,
  isErrorInput,
  isErrorMatchCheck
};
