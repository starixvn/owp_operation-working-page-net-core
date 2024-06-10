import PropTypes from 'prop-types';
import { Cell } from 'fixed-data-table-2';
import { nanoid } from '@reduxjs/toolkit';
import { isEqual } from 'lodash';

const propTypes = {
  rowIndex: PropTypes.number.isRequired,
  dataSource: PropTypes.array.isRequired
};

const NoCell = props => {
  const { rowIndex, dataSource } = props;
  const { No, MatchOffer } = dataSource[rowIndex];

  return (
    <Cell key={nanoid()} className='header-no'>
      {No && <div className='no'>{No}</div>}
      <div className='match-offer'>
        {MatchOffer && isEqual(MatchOffer, 'Opened') && (
          <span className='icon-completed' />
        )}
        {MatchOffer && !isEqual(MatchOffer, 'Opened') && (
          <span>{MatchOffer}</span>
        )}
      </div>
    </Cell>
  );
};

NoCell.propTypes = propTypes;
export default NoCell;
