import ToastrWrapper from '../../../../components/ToastrWrapper';
import { useAppDispatch } from '../app/hooks';
import store from '../app/storeNative';
import { fetchLeagueSettingInfo } from '../table/leagueInfoTableSlice/leagueInfoThunk';
import { setFocusSettingId } from '../table/leagueInfoTableSlice/reducer';
import { updateSettingService } from '../service/service';
import { AxiosError } from 'axios';
import { TExpectedResponseType } from '../component/Type';

const Command = (props: { closePopup: Function }) => {
  const { closePopup } = props;
  const dispatch = useAppDispatch();
  const {
    editSettingSlice: {
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
      division
    },
    leagueInfoTableSlice: { leagueFilterHeader }
  } = store.getState();

  const handleSave = async () => {
    try {
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
        await ToastrWrapper.showInfo(
          'The setting has been edited successfully!'
        );
        dispatch(fetchLeagueSettingInfo(leagueFilterHeader, false));
        dispatch(setFocusSettingId(leagueSettingId));
      }

      closePopup();
    } catch (error) {
      if ((error as AxiosError).response?.status === 400) {
        const { message, duplicatedLeagueName } = (error as AxiosError).response
          ?.data as TExpectedResponseType;
        await ToastrWrapper.showDuplicateLeagueSetting(
          message,
          duplicatedLeagueName
        );
      }
    }
  };
  return (
    <div className='list-btn'>
      <button className='btn-cancel' onClick={() => closePopup()}>
        Cancel
      </button>
      <button className='button-export' onClick={handleSave}>
        Save
      </button>
    </div>
  );
};

export default Command;
