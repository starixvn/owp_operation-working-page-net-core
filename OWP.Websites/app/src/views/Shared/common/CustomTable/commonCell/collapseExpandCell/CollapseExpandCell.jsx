import PropTypes from 'prop-types';
import { collapse, expand } from './collapseExpandConst.json';

const propTypes = {
  headerName: PropTypes.string.isRequired,
  mode: PropTypes.string.isRequired,
  onCollapse: PropTypes.func.isRequired,
  onExpand: PropTypes.func.isRequired
};

const CollapseExpandCell = props => {
  const {
    headerName,
    mode,
    onCollapse,
    onExpand
  } = props;

  const collapseExpand = {
    [collapse]: {
      title: '\u25BA',
      handler: onExpand
    },
    [expand]: {
      title: '\u25BC',
      handler: onCollapse
    }
  };

  const { title, handler } = collapseExpand[mode];

  return (
    <div>
      <span>{headerName}</span>
      <span className='icon-expand expand-trader' onClick={handler}>
        {title}
      </span>
    </div>
  );
};

CollapseExpandCell.propTypes = propTypes;
export default CollapseExpandCell;
