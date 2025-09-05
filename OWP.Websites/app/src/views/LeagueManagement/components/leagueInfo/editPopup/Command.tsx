import ToastrWrapper from '../../../../components/ToastrWrapper';
import { useAppDispatch } from '../app/hooks';
import store from '../app/storeNative';
import { fetchLeagueSettingInfo } from '../table/leagueInfoTableSlice/leagueInfoThunk';
import { setFocusSettingId } from '../table/leagueInfoTableSlice/reducer';
import { updateSettingService } from '../service/service';
import { AxiosError } from 'axios';
import { TExpectedResponseType } from '../component/Type';
import { message } from '../../../constants/Const';
import { haInvalidDivision } from '../helper/helper';

const Command = (props: { closePopup: Function }) => {
  const { closePopup } = props;
  const dispatch = useAppDispatch();
  const {
    editSettingSlice: {
      leagueSettingId,
      league,
      owpLeagueId,
      leagueGroupId,
      oldLeagueGroupId,
      leagueGroup,
      spread,
      maxbetWD_HDP,
      wD_NoOfHDP,
      maxbetWK_HDP,
      wK_NoOfHDP,
      lstNote,
      remark2,
      hasTv,
      hasIpTv,
      remark,
      round,
      division,
      isBlackList,
      noOfferLive,
      comment
    },
    leagueInfoTableSlice: { leagueFilterHeader }
  } = store.getState();

  const handleSave = async () => {
    try {
      if (
        haInvalidDivision(division)) {
        ToastrWrapper.showError('Invalid format for Pairwise Contains. Enter values as pairs like D1,D2. Each item must be separated by commas.');
        return;
      }
      if (oldLeagueGroupId !== leagueGroupId) {
        const confirmResult = await ToastrWrapper.show({
          type: 'warning',
          html: message.changeLeagueGroup,
          showCloseButton: true,
          showCancelButton: true
        });

        if (!confirmResult.value) {
          return;
        }
      }

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
        hasTv,
        hasIpTv,
        remark,
        round: round.filter(d => d.value) || [],
        division: division.filter(d => d.value) || [],
        isBlackList,
        noOfferLive,
        comment
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
        const { message: errorMessage, duplicatedLeagueName } = (error as AxiosError).response
          ?.data as TExpectedResponseType;
        await ToastrWrapper.showDuplicateLeagueSetting(
          errorMessage,
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
