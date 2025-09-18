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
  return (
    <div className='league-cell'>
      <p>
        <span className='league-name'>test</span>
      </p>
    </div>
  );
};

LSTNote.propTypes = propTypes;
export default LSTNote;
