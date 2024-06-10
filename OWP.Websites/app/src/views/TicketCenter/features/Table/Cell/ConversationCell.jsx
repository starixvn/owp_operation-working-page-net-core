import PropTypes from 'prop-types';
import LeagueCellFactory from '../../../../Shared/common/CustomTable/commonCell/LeagueCellNew/LeagueCellFactory';

const propTypes = {
  rowIndex: PropTypes.number.isRequired,
  dataSource: PropTypes.array.isRequired
};

const ConversationCell = ({ dataSource, rowIndex }) => (
  <LeagueCellFactory data={dataSource[rowIndex]} />
);

ConversationCell.propTypes = propTypes;
export default ConversationCell;
