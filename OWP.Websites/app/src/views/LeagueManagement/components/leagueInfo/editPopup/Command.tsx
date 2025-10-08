import ToastrWrapper from '../../../../components/ToastrWrapper';
import { useAppDispatch } from '../app/hooks';
import store from '../app/storeNative';
import { fetchLeagueSettingInfo } from '../table/leagueInfoTableSlice/leagueInfoThunk';
import { setFocusSettingId, setIsSaveAction, setFilterChange } from '../table/leagueInfoTableSlice/reducer';
import { updateSettingService } from '../service/service';
import { AxiosError } from 'axios';
import { TExpectedResponseType } from '../component/Type';
import { message } from '../../../constants/Const';
import { hasInvalidDivision } from '../helper/helper';
import { TDataSource } from '../table/TableType';

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
      comment,
      oldData
    },
    leagueInfoTableSlice: { leagueFilterHeader, multipleFilter, filterState }
  } = store.getState();

  const compareDataAndDetectChanges = (oldData: TDataSource | null, newData: any) => {
    if (!oldData) {
      return [];
    }

    const changes: string[] = [];
    if (oldData.leagueGroup !== newData.leagueGroup) {
      changes.push('League_Group');
    }
    if (oldData.condition.hasTv !== newData.hasTv || oldData.condition.hasIpTv !== newData.hasIpTv) {
      changes.push('Tv');
    }
    if (JSON.stringify(oldData.condition.round) !== JSON.stringify(newData.round.filter((item: any) => item.value !== ''))) {
      changes.push('Round');
    }
    if (JSON.stringify(oldData.condition.division) !== JSON.stringify(newData.division.filter((item: any) => item.value !== ''))) {
      changes.push('League_Division');
    }
    if (oldData.spreadShow !== newData.spread) {
      changes.push('Spread_HDP_OU');
    }
    if (oldData.maxbetWK_HDP_Show !== newData.maxbetWK_HDP) {
      changes.push('MB_WK_HDP');
    }
    if (oldData.wK_NoOfHDP_Show !== newData.wK_NoOfHDP) {
      changes.push('MB_WK_NO_HDP');
    }
    if (oldData.maxbetWD_HDP_Show !== newData.maxbetWD_HDP) {
      changes.push('MB_WD_HDP');
    }
    if (oldData.wD_NoOfHDP_Show !== newData.wD_NoOfHDP) {
      changes.push('MB_WD_NO_HDP');
    }
    if (oldData.comment !== newData.comment) {
      changes.push('Comment');
    }
    if (oldData.condition.remark !== newData.remark) {
      changes.push('Remark');
    }
    if(oldData.lstNote !== newData.lstNote) {
      changes.push('LST_Note');
    }
    
    return changes;
  };

  const handleFilterResetAfterDataChange = (oldData: TDataSource | null, newData: any) => {
    const changedFields = compareDataAndDetectChanges(oldData, newData);
    
    if (changedFields.length > 0) {
      const activeChangedFields = changedFields.filter(field => {
        const isInFilterState = filterState.some(filter => filter.columnKey === field);
        const isInMultipleFilter = multipleFilter[field as keyof typeof multipleFilter]?.isFilter;
        
        return isInFilterState || isInMultipleFilter;
      });
      
      if (activeChangedFields.length > 0) {
        dispatch(setFilterChange(activeChangedFields.map(field => ({
          columnKey: field,
          keyword: ''
        }))));
      }
    }
  };

  const handleSave = async () => {
    try {
      if (hasInvalidDivision(division)) {
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

        const newData = {
          leagueGroup,
          hasTv,
          hasIpTv,
          round,
          division,
          spread,
          maxbetWK_HDP,
          wK_NoOfHDP,
          maxbetWD_HDP,
          wD_NoOfHDP,
          comment,
          remark,
          lstNote
        };

        dispatch(setIsSaveAction(true));
        dispatch(fetchLeagueSettingInfo(leagueFilterHeader, false));
        dispatch(setFocusSettingId(leagueSettingId));
        handleFilterResetAfterDataChange(oldData, newData);
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
