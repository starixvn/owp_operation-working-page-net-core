import PropTypes from 'prop-types';
import { UncontrolledTooltip } from 'reactstrap';

const propTypes = {
  rowIndex: PropTypes.number.isRequired,
  dataSource: PropTypes.array.isRequired
};

const ReadOnlyRemark = props => {
  const { rowIndex, dataSource } = props;
  const { Remark } = dataSource[rowIndex];
  return (
    <>
      <label id={`Remark${rowIndex}`}>{Remark}</label>
      {Remark && (
        <UncontrolledTooltip placement='top' target={`Remark${rowIndex}`}>
          {Remark}
        </UncontrolledTooltip>
      )}
    </>
  );
};

ReadOnlyRemark.propTypes = propTypes;
export default ReadOnlyRemark;
