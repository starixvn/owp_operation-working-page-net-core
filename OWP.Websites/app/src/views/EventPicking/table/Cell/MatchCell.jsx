import PropTypes from 'prop-types';

const propTypes = {
  dataSource: PropTypes.array.isRequired,
  rowIndex: PropTypes.number.isRequired
};

const MatchCell = props => {
  const { rowIndex, dataSource } = props;
  const { homeName, awayName } = dataSource[rowIndex];

  return (
    <>
      <div className='match-cell'>
        <div className='home'>{homeName}</div>
        <div className='away'>{awayName}</div>
      </div>
    </>
  );
};

MatchCell.propTypes = propTypes;
export default MatchCell;
