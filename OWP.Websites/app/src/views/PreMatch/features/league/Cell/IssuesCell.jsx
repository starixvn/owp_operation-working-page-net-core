import PropTypes from 'prop-types';
import IconCreateMessage from './IconCreateMessage';
import IconOpenMessage from './IconOpenMessage';

const propTypes = {
  dataSource: PropTypes.array.isRequired,
  rowIndex: PropTypes.number.isRequired
};

const IssuesCell = props => {
  const { dataSource, rowIndex } = props;
  const { HasMessages } = dataSource[rowIndex];

  return (
    <>
      {HasMessages ? (
        <IconOpenMessage {...props} />
      ) : (
        <IconCreateMessage {...props} />
      )}
    </>
  );
};

IssuesCell.propTypes = propTypes;
export default IssuesCell;
