import PropTypes from 'prop-types';
import { Cell } from 'fixed-data-table-2';
import { nanoid } from '@reduxjs/toolkit';
import { UncontrolledTooltip } from 'reactstrap';
import useStableDispatch from '../../../../../Shared/customHook/useStableDispatch';
import {
  collapseOneLeague,
  expandOneLeague
} from '../../shiftSlice/shiftSlice';
import { getMatchActiveByCondition } from '../../shiftSlice/shiftHelper';
import { useSelector } from 'react-redux';

const propTypes = {
  dataSource: PropTypes.array.isRequired,
  rowIndex: PropTypes.number.isRequired
};

const LeagueCell = props => {
  const { dataSource, rowIndex } = props;
  const { LeagueName, PreMatchLeagueId, isExpand, MatchCount, IsEarlyOffered } =
    dataSource[rowIndex];
  const leagueWithTimeCondition = useSelector(
    state => state.dayShift?.leagueWithTimeCondition
  );

  const matchActiveByCondition = getMatchActiveByCondition(
    leagueWithTimeCondition,
    PreMatchLeagueId
  );

  const stableDispatch = useStableDispatch();
  const handleClickExpandCollapse = () => {
    isExpand
      ? stableDispatch(collapseOneLeague(PreMatchLeagueId))
      : stableDispatch(expandOneLeague(PreMatchLeagueId));
  };

  return (
    <Cell key={nanoid()}>
      <div className='league-row'>
        <label id={`name${rowIndex}`}>{LeagueName}</label>
        {IsEarlyOffered && <div className='green-dot'></div>}
        <span>
          {leagueWithTimeCondition.leagueInfos.length > 0 ? (
            <b className='matchCountActive'>
              {`${matchActiveByCondition}/${MatchCount}`}
            </b>
          ) : (
            <b className='matchCount'>({MatchCount})</b>
          )}

          <span className='icon-expand' onClick={handleClickExpandCollapse}>
            {isExpand ? '\u25BC' : '\u25BA'}
          </span>
        </span>

        <UncontrolledTooltip placement='top' target={`name${rowIndex}`}>
          {LeagueName} <b>({MatchCount})</b>
        </UncontrolledTooltip>
      </div>
    </Cell>
  );
};

LeagueCell.propTypes = propTypes;
export default LeagueCell;
