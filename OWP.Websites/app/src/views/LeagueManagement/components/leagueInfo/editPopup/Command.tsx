import ToastrWrapper from '../../../../components/ToastrWrapper';
import { useAppDispatch } from '../app/hooks';
import store from '../app/storeNative';
import { fetchLeagueSettingInfo } from '../table/leagueInfoTableSlice/leagueInfoThunk';
import { setFocusSettingId } from '../table/leagueInfoTableSlice/reducer';
import { updateSettingService } from './slice/editSettingThunk';

const Command = (props: { closePopup: Function; }) => {
  const { closePopup } = props;
  const dispatch = useAppDispatch();
  const { editSettingSlice: {
    leagueSettingId,
    league,
    owpLeagueId,
    leagueGroupId,
    leagueGroup,
    spread,
    maxbetWD_HDP,
    wD_NoOfHDP,
    maxbetWK_HDP,
    wK_NoOfHDP,
    lstNote,
    remark2,
    comment,
    hasTv,
    hasIpTv,
    remark,
    round,
    division,
  }, leagueInfoTableSlice: { leagueFilterHeader } } = store.getState();

  const handleSave = async () => {
    const response = await updateSettingService({
      leagueSettingId,
      league,
      owpLeagueId,
      leagueGroupId,
      leagueGroup,
      spread,
      maxbetWD_HDP,
      wD_NoOfHDP,
      maxbetWK_HDP,
      wK_NoOfHDP,
      lstNote,
      remark2,
      comment,
      hasTv,
      hasIpTv,
      remark,
      round: round.filter(d => d.value) || [],
      division: division.filter(d => d.value) || []
    });
    if (response.status === 200) {
      await ToastrWrapper.showInfo('The setting has been edited successfully!');
      dispatch(fetchLeagueSettingInfo(leagueFilterHeader, false));
      dispatch(setFocusSettingId(leagueSettingId));
    }

    closePopup();
  };
  return (
    <div className='list-btn'>
      <button
        className='btn-cancel'
        onClick={() => closePopup()}>
        Cancel
      </button>
      <button className='button-export' onClick={handleSave}>
        Save
      </button>
    </div>
  );
};

export default Command;