import { useEffect } from 'react';
import { Modal, ModalBody, ModalFooter } from 'reactstrap';
import { useAppDispatch, useAppSelector } from '../app/hooks';
import ConditionOfAdd from './content/ConditionOfAdd';
import FtSpreadOffAdd from './content/FtSpreadOffAdd';
import MBOfAdd from './content/MBOfAdd';
import MiscMatchOffAdd from './content/MiscMatchOffAdd';
import CommentOfAdd from './content/CommentOfAdd';
import CommandOfAdd from './content/CommandOfAdd';
import LeagueInfoOfAdd from './content/LeagueInfoOfAdd';
import { getLeagueGroupOptionsThunk } from '../component/commonThunk';
const AddSettingPopup = () => {
  const dispatch = useAppDispatch();

  const { leagueGroupOptions } = useAppSelector(
    state => state.editSettingSlice
  );
  useEffect(() => {
    !leagueGroupOptions.length && dispatch(getLeagueGroupOptionsThunk());
  }, [dispatch, leagueGroupOptions]);

  return (
    <Modal
      isOpen
      scrollable
      className='edit-setting-popup'
      modalClassName='edit-modal'
      centered>
      <ModalBody>
        <LeagueInfoOfAdd />
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
