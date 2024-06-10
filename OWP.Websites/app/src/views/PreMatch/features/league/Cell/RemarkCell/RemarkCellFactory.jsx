import PropTypes from 'prop-types';
import { Cell } from 'fixed-data-table-2';
import { nanoid } from '@reduxjs/toolkit';
import ReadOnlyRemark from './ReadOnlyRemark';
import EditableRemark from './EditableRemark';

const propTypes = {
  isEditing: PropTypes.bool.isRequired,
  rowIndex: PropTypes.number.isRequired,
  dataSource: PropTypes.array.isRequired
};

const RemarkCellFactory = props => {
  const { isEditing, rowIndex, dataSource } = props;
  const { InfoSiteMatchId, TeamRound } = dataSource[rowIndex];

  if (InfoSiteMatchId) {
    return (
      <Cell key={nanoid()} className='header-Remark'>
        {TeamRound && (
          <label>
            <b>Round (H-A/T)</b>: {TeamRound}
          </label>
        )}
      </Cell>
    );
  }

  return (
    <Cell key={nanoid()} className='header-Remark'>
      {isEditing ? (
        <EditableRemark {...props} />
      ) : (
        <ReadOnlyRemark {...props} />
      )}
    </Cell>
  );
};
RemarkCellFactory.propTypes = propTypes;
export default RemarkCellFactory;
