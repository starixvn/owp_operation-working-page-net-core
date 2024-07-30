import PropTypes from 'prop-types';
import { TooltipText } from '../../../../../Shared/common/Tooltip';

const propTypes = {
  dataSource: PropTypes.array.isRequired,
  rowIndex: PropTypes.number.isRequired
};
const RemarkCell = props => {
  const { dataSource, rowIndex } = props;
  const { remark } = dataSource[rowIndex];
  const renderContent = () => {
    const remarkSplitByComma = remark.split(',');
    return <ul>
      {remarkSplitByComma.map((item, index) => <li key={index}>{item}</li>)}
    </ul>;
  };

  const content = renderContent();
  return (
    <div className='remark-cell'>
      <TooltipText
        text={content}
        toolTipContent={content}
        innerClassName='remark'
        rowIndex={rowIndex}
      />
    </div>
  );
};

RemarkCell.propTypes = propTypes;
export default RemarkCell;
