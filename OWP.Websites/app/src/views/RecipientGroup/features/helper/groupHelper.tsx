import { isEqual } from 'lodash';
import ToastrWrapper from '../../../components/ToastrWrapper';
import { GroupDataSource, RecipientModel } from '../Type';
import {
  getRecipientGroupsKey,
  saveRecipientGroupsKey,
  maxLengthGroupName
} from '../../Const.json';

const validateGroupName = (
  groupId: number,
  groupName: string,
  groups: GroupDataSource[]
) => {
  if (isEqual(groupName, '')) {
    ToastrWrapper.showError('Group Name field is required');
    return false;
  }

  const existedGroupNames = groups
    .filter(item => item.groupId !== groupId)
    .map(item => item.groupName);
  if (existedGroupNames.includes(groupName)) {
    ToastrWrapper.showError(
      'A group or account with this name already exists. Please try again'
    );
    return false;
  }

  if (groupName.length > maxLengthGroupName) {
    ToastrWrapper.showError(
      `The maximum of characters is ${maxLengthGroupName}. Please try again`
    );
    return false;
  }

  return true;
};

const validateRecipients = (recipients: RecipientModel[]) => {
  if (recipients.length === 0) {
    ToastrWrapper.showError('Please select at least one recipient');
    return false;
  }

  return true;
};

const buildRequestURL = (key: string) => `/api/RecipientGroup/${key}`;

const buildGetRecipientGroupsURL = () => buildRequestURL(getRecipientGroupsKey);
const buildSaveRecipientGroupsURL = () =>
  buildRequestURL(saveRecipientGroupsKey);

export {
  validateGroupName,
  validateRecipients,
  buildGetRecipientGroupsURL,
  buildSaveRecipientGroupsURL
};
