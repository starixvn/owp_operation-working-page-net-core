import PropTypes from 'prop-types';

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
      <div className='provider-cell'>
        {renderProvider()}
        {renderOtherProvider()}
        {renderAgent()}
        {renderVenue()}
      </div>
    </>
  );
};

ProviderCell.propTypes = propTypes;
export default ProviderCell;
