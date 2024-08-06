import { AxiosError } from 'axios';
import { useAppDispatch, useAppSelector } from '../../app/hooks';
import {
  setIsOpenAddNewSetting,
  setFocusSettingId
} from '../../table/leagueInfoTableSlice/reducer';
import { addSettingService } from '../../service/service';
import ToastrWrapper from '../../../../../components/ToastrWrapper';
import { fetchLeagueSettingInfo } from '../../table/leagueInfoTableSlice/leagueInfoThunk';
import { setValueDefault } from '../slice/addSettingSlice';

type ExpectedResponseType = {
  message: string;
  duplicatedLeagueName: string;
};

const CommandOfAdd = () => {
  const leagueFilterHeader = useAppSelector(
    state => state.leagueInfoTableSlice.leagueFilterHeader
  );
  const leagueSetting = useAppSelector(state => state.addSettingSlice);
  const {
    leagueSettingId,
    league,
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
  } = leagueSetting;
  const dispatch = useAppDispatch();

  const handleSave = async () => {
    if (league.length === 0) {
      await ToastrWrapper.showError('League Name field is required!');
      return;
    }
    try {
      const response = await addSettingService({
        leagueSettingId,
        league,
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
          'The setting has been added successfully!'
        );
        dispatch(fetchLeagueSettingInfo(leagueFilterHeader, false));
        dispatch(setFocusSettingId(leagueSettingId));
        dispatch(setIsOpenAddNewSetting());
        dispatch(setValueDefault());
      }
    } catch (error) {
      if ((error as AxiosError).response?.status === 400) {
        const { message, duplicatedLeagueName } = (error as AxiosError).response
          ?.data as ExpectedResponseType;
        await ToastrWrapper.showDuplicateLeagueSetting(
          message,
          duplicatedLeagueName
        );
      }
    }
  };
  return (
    <div className='list-btn'>
      <button
        className='btn-cancel'
        onClick={() => dispatch(setIsOpenAddNewSetting())}>
        Cancel
      </button>
      <button className='button-export' onClick={handleSave}>
        Save
      </button>
    </div>
  );
};

export default CommandOfAdd;
