import PropTypes from 'prop-types';
import { restoreDeletedLeague } from '../league/shiftSlice/shiftSlice';
import { refeshDeletedLeagueTable } from './deleteLeagueSlice';
import RestoreIcon from 'mdi-react/RestoreIcon';
import useStableDispatch from '../../../Shared/customHook/useStableDispatch';
import { LAST_UPDATED, simpleApiQueryUrl } from '../../const/const';
import { getLastUpdate } from '../header/headerSlice';

const propTypes = {
  PreMatchLeagueId: PropTypes.number.isRequired
};

const RecoverCell = props => {
  const { PreMatchLeagueId } = props;
  const stableDispatch = useStableDispatch();
  const model = {
    url: simpleApiQueryUrl,
    apiKey: LAST_UPDATED.apiKey
  };
  const handleRestore = () => {
    stableDispatch(restoreDeletedLeague(PreMatchLeagueId));
    stableDispatch(refeshDeletedLeagueTable(PreMatchLeagueId));
    stableDispatch(getLastUpdate(model));
  };
  return <RestoreIcon className='icon-restore' onClick={handleRestore} />;
};
RecoverCell.propTypes = propTypes;
export default RecoverCell;
