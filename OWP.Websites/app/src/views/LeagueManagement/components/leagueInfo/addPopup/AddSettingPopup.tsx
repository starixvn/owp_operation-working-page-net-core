import { Modal, ModalBody, ModalFooter } from 'reactstrap';
import { useAppSelector } from '../app/hooks';
import ConditionOfAdd from './content/ConditionOfAdd';
import FtSpreadOffAdd from './content/FtSpreadOffAdd';
import MBOfAdd from './content/MBOfAdd';
import MiscMatchOffAdd from './content/MiscMatchOffAdd';
import CommentOfAdd from './content/CommentOfAdd';
import CommandOfAdd from './content/CommandOfAdd';
const AddSettingPopup = () => {
  const { isOpenAddNewSetting } = useAppSelector(
    state => state.leagueInfoTableSlice
  );

  return (
    <Modal
      isOpen={isOpenAddNewSetting}
      scrollable
      className='edit-setting-popup'
      modalClassName='edit-modal'
      centered>
      <ModalBody>
        {/* <Header league={league} leagueGroupId={leagueGroupId} /> */}
        <ConditionOfAdd />
        <FtSpreadOffAdd />
        <MBOfAdd />
        <MiscMatchOffAdd />
        <CommentOfAdd />
      </ModalBody>
      <ModalFooter>
        <CommandOfAdd />
      </ModalFooter>
    </Modal>
  );
};

export default AddSettingPopup;
