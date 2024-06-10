import PropTypes from 'prop-types';

const propTypes = {
  dataSource: PropTypes.array.isRequired,
  rowIndex: PropTypes.number.isRequired
};

const RoundCell = props => {
  const { rowIndex, dataSource } = props;
  const { division, round, etpk } = dataSource[rowIndex];

  const renderDivision = () =>
    division && (
      <span>
        <strong>Division: </strong>
        {division}
      </span>
    );

  const renderRound = () =>
    round && (
      <span>
        <strong>Round: </strong>
        {round}
      </span>
    );

  const renderEtPk = () =>
    etpk && (
      <span>
        <strong>ET_PK: </strong>
        {etpk}
      </span>
    );

  return (
    <div className='round-cell'>
      {renderDivision()}
      {renderRound()}
      {renderEtPk()}
    </div>
  );
};

RoundCell.propTypes = propTypes;
export default RoundCell;
