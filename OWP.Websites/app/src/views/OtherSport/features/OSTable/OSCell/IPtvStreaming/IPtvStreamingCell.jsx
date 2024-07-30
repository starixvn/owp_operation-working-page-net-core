import PropTypes from 'prop-types';
import { TooltipButton } from '../../../../../Shared/common/Tooltip';

const propTypes = {
  dataSource: PropTypes.array.isRequired,
  rowIndex: PropTypes.number.isRequired
};

const IPtvStreamingCell = props => {
  const { dataSource, rowIndex } = props;
  const { iptv, wb } = dataSource[rowIndex];

  return (
    <div className='iptv-streaming-Cell'>
      {iptv && (
        <TooltipButton
          rowIndex={rowIndex}
          innerClassName='iptv'
          btnName='IPTV'
          toolTipContent={iptv}
        />
      )}
      {wb && (
        <TooltipButton
          rowIndex={rowIndex}
          innerClassName='wb'
          btnName='Streaming'
          toolTipContent={wb}
        />
      )}
    </div>
  );
};

IPtvStreamingCell.propTypes = propTypes;
export default IPtvStreamingCell;
