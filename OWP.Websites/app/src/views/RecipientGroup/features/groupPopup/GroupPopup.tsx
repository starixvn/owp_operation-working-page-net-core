import { Dispatch, SetStateAction, useRef, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { Modal, ModalBody } from 'reactstrap';
import { post } from '../../../Shared/Service/Service';
import RecipientSuggestion from '../../../Shared/TicketSetting/RecipientSuggestion/RecipientSuggestion';
import { Permission } from '../../../Shared/TicketSetting/TypeNative';
import { getValueRef } from '../../../Shared/Utils';
import { RootState } from '../../app/storeNative';
import { getRecipientGroup } from '../groupThunkNative';
import {
  buildSaveRecipientGroupsURL,
  validateGroupName,
  validateRecipients
} from '../helper/groupHelper';
import { GroupDataSource, RecipientModel } from '../Type';
import GroupName from './GroupName';
import RecipientTable from './RecipientTable';

interface Props {
  title: string;
  model: GroupDataSource;
  setIsOpenPopup: Dispatch<SetStateAction<boolean>>;
}

const GroupPopup = (props: Props) => {
  const { title, setIsOpenPopup, model } = props;
  const { groupId, groupName } = model;
  const dispatch = useDispatch();
  const groupRef = useRef<HTMLInputElement>(null);
  const [recipients, setRecipients] = useState<RecipientModel[]>(
    model.recipients
  );
  const groups = useSelector((state: RootState) => state.recipientGroup.groups);
  const permission = useSelector((state: RootState) => state.policy.permission);
  const siteId = (permission as Permission).SiteId;

  const saveRequest = async (groupNameRef: string) => {
    const postModel = {
      groupId,
      groupName: groupNameRef,
      recipients,
      url: buildSaveRecipientGroupsURL()
    };

    await post(postModel);
  };

  const handleSave = async () => {
    const groupNameRef = getValueRef(groupRef);

    if (
      validateGroupName(groupId, groupNameRef, groups) &&
      validateRecipients(recipients)
    ) {
      await saveRequest(groupNameRef);
      setIsOpenPopup(false);
      dispatch(getRecipientGroup());
    }
  };

  const handleAddRecipient = (recipient: RecipientModel) => {
    setRecipients([...recipients, recipient]);
  };

  return (
    <Modal isOpen scrollable className='recipient-modal' centered>
      <ModalBody>
        <h2 className='title'>{title}</h2>
        <GroupName ref={groupRef} defaultValue={groupName} />
        <RecipientSuggestion
          currentRecipients={recipients}
          isIncludeGroup={false}
          isIncludeSection={false}
          siteId={siteId}
          handleAddRecipient={handleAddRecipient}
        />
        <RecipientTable recipients={recipients} setRecipients={setRecipients} />
        <div className='list-btn'>
          <button
            className='button-cancel'
            onClick={() => setIsOpenPopup(false)}>
            Cancel
          </button>
          <button className='button-export' onClick={handleSave}>
            Save
          </button>
        </div>
      </ModalBody>
    </Modal>
  );
};

export default GroupPopup;
