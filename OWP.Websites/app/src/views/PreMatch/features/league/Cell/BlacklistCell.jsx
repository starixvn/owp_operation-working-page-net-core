import PropTypes from 'prop-types';
import { Cell } from 'fixed-data-table-2';
import { nanoid } from '@reduxjs/toolkit';

const propTypes = {
  dataSource: PropTypes.array.isRequired,
  rowIndex: PropTypes.number.isRequired
};

const BlacklistCell = props => {
  const { dataSource, rowIndex } = props;
  const { Blacklist, InfoSiteMatchId } = dataSource[rowIndex];
  if (InfoSiteMatchId) {
    return <Cell className='empty-cell' />;
  }
  return (
    <Cell key={nanoid()} className='header-Blacklist'>
      <div>{Blacklist || '-'}</div>
    </Cell>
  );
};

BlacklistCell.propTypes = propTypes;
export default BlacklistCell;
