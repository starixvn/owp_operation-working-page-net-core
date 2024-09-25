import PropTypes from 'prop-types';
import { UncontrolledTooltip } from 'reactstrap';
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
    <>
      <div className='round-cell' id={`round${rowIndex}`}>
        {renderDivision()}
        {renderRound()}
        {renderEtPk()}
      </div>
      <UncontrolledTooltip
        className='tooltip-round'
        target={`round${rowIndex}`}>
        {renderDivision()}
        {renderRound()}
        {renderEtPk()}
      </UncontrolledTooltip>
    </>
  );
};

RoundCell.propTypes = propTypes;
export default RoundCell;
