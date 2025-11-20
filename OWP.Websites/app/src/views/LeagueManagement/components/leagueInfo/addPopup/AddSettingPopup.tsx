import { useEffect } from 'react';
import { Modal, ModalBody, ModalFooter } from 'reactstrap';
import { useAppDispatch, useAppSelector } from '../app/hooks';
import ConditionOfAdd from './content/ConditionOfAdd';
import FtSpreadOfAdd from './content/FtSpreadOfAdd';
import MBOfAdd from './content/MBOfAdd';
import MiscMatchOfAdd from './content/MiscMatchOfAdd';
import CommandOfAdd from './content/CommandOfAdd';
import LeagueInfoOfAdd from './content/LeagueInfoOfAdd';
import { getLeagueGroupOptionsThunk } from '../component/commonThunk';
import NoOfferLive from './content/NoOfferLive';

const AddSettingPopup = () => {
  const dispatch = useAppDispatch();

  const { leagueGroupOptions } = useAppSelector(state => state.addSettingSlice);
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
        <NoOfferLive />
        <FtSpreadOfAdd />
        <MBOfAdd />
        <MiscMatchOfAdd />
      </ModalBody>
      <ModalFooter>
        <CommandOfAdd />
      </ModalFooter>
    </Modal>
  );
};

export default AddSettingPopup;
