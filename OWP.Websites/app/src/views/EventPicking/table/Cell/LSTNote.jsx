import { Fragment, useRef } from 'react';
import PropTypes from 'prop-types';
import { UncontrolledTooltip } from 'reactstrap';
import { uniqueId } from 'lodash';

const propTypes = {
  dataSource: PropTypes.array.isRequired,
  rowIndex: PropTypes.number.isRequired
};

const LSTNote = props => {
  const { rowIndex, dataSource } = props;
  const { lstNote } = dataSource[rowIndex];

  const renderLstNote = () =>
    lstNote && (
      <span>
        <strong>LST Note: </strong>
        {lstNote}
      </span>
    );
  return (
    <>
      <div className='remark-cell' id={`lstNote${rowIndex}`}>
        {renderLstNote()}
      </div>
      <UncontrolledTooltip
        className='tooltip-remark'
        target={`lstNote${rowIndex}`}>
        {renderLstNote()}
      </UncontrolledTooltip>
    </>
  );
};

LSTNote.propTypes = propTypes;
export default LSTNote;
