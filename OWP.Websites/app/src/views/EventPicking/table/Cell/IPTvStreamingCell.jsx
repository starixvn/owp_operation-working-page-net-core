import PropTypes from 'prop-types';

const propTypes = {
  dataSource: PropTypes.array.isRequired,
  rowIndex: PropTypes.number.isRequired
};

const IPTvStreamingCell = props => {
  const { rowIndex, dataSource } = props;
  const { tv, iptv, channel, wb } = dataSource[rowIndex];

  const renderTv = () =>
    tv && (
      <span>
        <strong>Tv: </strong>
        {tv}
      </span>
    );

  const renderIpTv = () =>
    iptv && (
      <span>
        <strong>IPTV: </strong>
        {iptv}
      </span>
    );

  const renderChanel = () =>
    channel && (
      <span>
        <strong>Channel: </strong>
        {channel}
      </span>
    );

  const renderWb = () =>
    wb && (
      <span>
        <strong>Streaming Info(WB): </strong>
        {wb}
      </span>
    );

  return (
    <div className='streaming-info-cell'>
      {renderTv()}
      {renderIpTv()}
      {renderChanel()}
      {renderWb()}
    </div>
  );
};

IPTvStreamingCell.propTypes = propTypes;
export default IPTvStreamingCell;
