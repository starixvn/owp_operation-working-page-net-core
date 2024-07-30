import PropTypes from 'prop-types';
import { Cell } from 'fixed-data-table-2';
import { nanoid } from '@reduxjs/toolkit';

const propTypes = {
  rowIndex: PropTypes.number.isRequired,
  dataSource: PropTypes.array.isRequired
};

const LeagueGroupCell = props => {
  const { rowIndex, dataSource } = props;
  const { LeagueGroup } = dataSource[rowIndex];
  return (
    <Cell key={nanoid()} className='header-no'>
      {LeagueGroup && <div className='no'>{LeagueGroup}</div>}
    </Cell>
  );
};

LeagueGroupCell.propTypes = propTypes;
export default LeagueGroupCell;
