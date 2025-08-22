import { useEffect } from 'react';
import { Modal, ModalBody, ModalFooter } from 'reactstrap';
import { useAppDispatch, useAppSelector } from '../app/hooks';
import Command from './Command';
import Comment from './content/Comment';
import Condition from './content/Condition';
import FtSpread from './content/FtSpread';
import Header from './content/Header';
import Maxbet from './content/Maxbet';
import MicsMatch from './content/MicsMatch';
import { getLeagueGroupOptionsThunk } from '../component/commonThunk';
import LeagueInfo from './content/LeagueInfo';

const EditSettingPopup = (props: { closePopup: () => void }) => {
  const { closePopup } = props;
  const dispatch = useAppDispatch();
  const {
    leagueGroupOptions,
    league,
    leagueGroupId,
    hasTv,
    hasIpTv,
    round,
    division,
    remark,
    spread,
    maxbetWD_HDP,
    wD_NoOfHDP,
    maxbetWK_HDP,
    wK_NoOfHDP,
    lstNote,
    remark2,
    shortLeague,
    isBlackList,
    noOfferLive
  } = useAppSelector(state => state.editSettingSlice);

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
        <LeagueInfo
          leagueGroupId={leagueGroupId}
          league={league}
          shortLeague={shortLeague}
          isBlackList={isBlackList}
          noOfferLive={noOfferLive}
        />
        <Condition
          condition={{
            hasTv,
            hasIpTv,
            round,
            division,
            remark
          }}
        />
        <FtSpread spread={spread} />
        <Maxbet
          maxbetWD_HDP={maxbetWD_HDP}
          wD_NoOfHDP={wD_NoOfHDP}
          maxbetWK_HDP={maxbetWK_HDP}
          wK_NoOfHDP={wK_NoOfHDP}
        />
        <MicsMatch lstNote={lstNote} remark2={remark2} />
      </ModalBody>
      <ModalFooter>
        <Command closePopup={closePopup} />
      </ModalFooter>
    </Modal>
  );
};

export default EditSettingPopup;
