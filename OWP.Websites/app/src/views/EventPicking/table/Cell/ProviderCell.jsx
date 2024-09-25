import PropTypes from 'prop-types';
import { UncontrolledTooltip } from 'reactstrap';

const propTypes = {
  dataSource: PropTypes.array.isRequired,
  rowIndex: PropTypes.number.isRequired
};

const ProviderCell = props => {
  const { rowIndex, dataSource } = props;
  const { otherProvider, agent, venue, provider } = dataSource[rowIndex];

  const renderProvider = () =>
    provider && (
      <span>
        <strong>Provider: </strong>
        {provider}
      </span>
    );

  const renderOtherProvider = () =>
    otherProvider && (
      <span>
        <strong>Other Provider: </strong>
        {otherProvider}
      </span>
    );

  const renderAgent = () =>
    agent && (
      <span>
        <strong>Agent: </strong>
        {agent}
      </span>
    );

  const renderVenue = () =>
    venue && (
      <span>
        <strong>Venue: </strong>
        {venue}
      </span>
    );

  return (
    <>
      <div className='provider-cell' id={`provider${rowIndex}`}>
        {renderProvider()}
        {renderOtherProvider()}
        {renderAgent()}
        {renderVenue()}
      </div>
      <UncontrolledTooltip
        className='tooltip-provider'
        target={`provider${rowIndex}`}>
        {renderProvider()}
        {renderOtherProvider()}
        {renderAgent()}
        {renderVenue()}
      </UncontrolledTooltip>
    </>
  );
};

ProviderCell.propTypes = propTypes;
export default ProviderCell;
