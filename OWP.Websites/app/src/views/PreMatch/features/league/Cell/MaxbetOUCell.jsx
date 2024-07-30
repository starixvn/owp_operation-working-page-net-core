import PropTypes from 'prop-types';
import { Cell } from 'fixed-data-table-2';
import { nanoid } from '@reduxjs/toolkit';
import { getMaxbetSpreadValue } from '../PrematchTableHelper';

const propTypes = {
  dataSource: PropTypes.array.isRequired,
  rowIndex: PropTypes.number.isRequired
};

const MaxbetOUCell = props => {
  const { dataSource, rowIndex } = props;
  const { InfoSiteMatchId, MaxBetOu } = dataSource[rowIndex];

  if (InfoSiteMatchId) {
    return (
      <Cell key={nanoid()} className='header-Maxbet'>
        <div>{getMaxbetSpreadValue(MaxBetOu)}</div>
      </Cell>
    );
  }
  return (
    <Cell key={nanoid()} className='header-Maxbet'>
      <div>-</div>
    </Cell>
  );
};

MaxbetOUCell.propTypes = propTypes;
export default MaxbetOUCell;
