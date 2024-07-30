import PropTypes from 'prop-types';
import changeLogColumn from './changeLogColumn';
import { isEqual } from 'lodash';
import { logAction } from '../league/constants/preMatchConst';

const propTypes = {
  children: PropTypes.node
};
const CellLogCommonFormatter = props => (
  <div className='text-left nomalcell'>{props.children}</div>
);
CellLogCommonFormatter.propTypes = propTypes;

const renderNormalCell = value => (
  <CellLogCommonFormatter>{value}</CellLogCommonFormatter>
);

const renderParseHtmlCell = value => {
  return (
    <div className='text-left'>
      <span className='spanClassName'>
        <b>{value}</b>
      </span>{' '}
    </div>
  );
};

const cellLogFormatSelector = (cellName, value, action) => {
  switch (cellName) {
    case changeLogColumn.Columns[2].id:
    case changeLogColumn.Columns[3].id:
      return isEqual(action, logAction.updateAssignment) ||
        isEqual(action, logAction.updateOtherAssignment)
        ? renderParseHtmlCell(value)
        : renderNormalCell(value);
    default:
      return renderNormalCell(value);
  }
};

export default cellLogFormatSelector;
