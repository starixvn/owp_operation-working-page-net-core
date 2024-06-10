import { useCallback, useEffect } from 'react';
import League from './League';
import Status from './Status';
import LeagueGroup from './LeagueGroup';
import LeagueCategory from './LeagueCategory';
import OldLeague from './OldLeague';
import { useStore, useDispatch } from 'react-redux';
import { postDataWithExtraModel } from '../../../actions/action';
import MaxbetConst from '../../../constants/MaxbetConst';
import { buildModelFilter } from '../../../helper/commonHelper';
import PropTypes from 'prop-types';
import { QUERY_MAXBETGENARAL_INFO } from '../../../constants/action-type';

const propTypes = {
  displayMaxbetGeneralTable: PropTypes.func.isRequired
};

const Filter = ({ displayMaxbetGeneralTable }) => {
  const store = useStore();
  const dispatch = useDispatch();
  const stableDispatch = useCallback(dispatch, []);
  const stableStore = useCallback(store, []);
  const stableDisplayMaxbetGeneralTable = useCallback(
    displayMaxbetGeneralTable,
    []
  );
  const handleSearch = useCallback(() => {
    const state = stableStore.getState();
    stableDisplayMaxbetGeneralTable();
    const model = buildModelFilter(state);
    const extraModel = {
      displayOldLeague: state.displayOldLeagueReducer.displayOldLeague,
      displayMaxbetGeneralTable: true
    };
    model.apiKey = MaxbetConst.maxbetGetAllLeagueAPIKey;
    stableDispatch(
      postDataWithExtraModel(
        MaxbetConst.getAllLeagueUrl,
        model,
        extraModel,
        QUERY_MAXBETGENARAL_INFO
      )
    );
  }, [stableDisplayMaxbetGeneralTable, stableDispatch, stableStore]);

  useEffect(() => {
    const displayTable =
      stableStore.getState().maxbetGeneralReducer.displayMaxbetGeneralTable;
    displayTable && handleSearch();
  }, [handleSearch, stableStore]);

  const renderSearchButton = () => {
    return (
      <div className='filter-control btn-block'>
        <button className='button-submit' onClick={handleSearch}>
          Search
        </button>
      </div>
    );
  };
  return (
    <>
      <League />
      <LeagueGroup />
      <Status />
      <LeagueCategory />
      <OldLeague />
      {renderSearchButton()}
    </>
  );
};

Filter.propTypes = propTypes;
export default Filter;
