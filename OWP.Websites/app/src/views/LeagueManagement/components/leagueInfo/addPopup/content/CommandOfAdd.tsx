import axios, { AxiosError } from 'axios';
import { useAppDispatch, useAppSelector } from '../../app/hooks';
import {
  setIsOpenAddNewSetting,
  setFocusSettingId
} from '../../table/leagueInfoTableSlice/reducer';
import { addSettingService } from '../../service/service';
import ToastrWrapper from '../../../../../components/ToastrWrapper';
import { fetchLeagueSettingInfo } from '../../table/leagueInfoTableSlice/leagueInfoThunk';

type MyExpectedResponseType = {
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
  } = leagueSetting;
  const dispatch = useAppDispatch();

  const handleSave = async () => {
    try {
      await addSettingService({
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
      await ToastrWrapper.showInfo('The setting has been edited successfully!');
      dispatch(fetchLeagueSettingInfo(leagueFilterHeader, false));
      dispatch(setFocusSettingId(leagueSettingId));
      dispatch(setIsOpenAddNewSetting());
    } catch (error) {
      if ((error as AxiosError).code === 'ERR_BAD_REQUEST') {
        const { message, duplicatedLeagueName } = (error as AxiosError).response
          ?.data as MyExpectedResponseType;
        ToastrWrapper.showDuplicateLeagueSetting(message, duplicatedLeagueName);
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
