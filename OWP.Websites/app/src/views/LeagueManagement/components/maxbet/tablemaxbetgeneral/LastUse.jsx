import { Cell } from 'fixed-data-table-2';
import PropTypes from 'prop-types';
import { getMMDDYYYYStringFromDateTime } from '../../../../Shared/DateTime/dateTimeHelper';

const propTypes = {
  lastUseDate: PropTypes.string
};

const LastUse = ({ lastUseDate }) => {
  const date = lastUseDate ? getMMDDYYYYStringFromDateTime(new Date(lastUseDate)) : '';
  return <Cell className='last-use'>{date}</Cell>;
};

LastUse.propTypes = propTypes;
export default LastUse;
