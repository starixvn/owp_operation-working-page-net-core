import PropTypes from 'prop-types';

const propTypes = {
  event: PropTypes.object,
  componentId: PropTypes.string
};

const MatchInfo = ({ event, componentId }) => {
  return (
    <>
      <p className='match-name'>
        {event.Home} -vs- {event.Away}
      </p>
      {event.NoOfHDP && (
        <span className={`badge-label badge-hdp-${event.NoOfHDPId}`}>
          {event.NoOfHDP}
        </span>
      )}
      {event.MatchOffers.map((element, index) => (
        <span
          key={`${componentId}${index}`}
          className={`badge-label ${
            element.MatchOfferValue.includes('OFF VR')
              ? 'off-vr-offer'
              : 'match-offer'
          } `}>
          {element.MatchOfferValue}
        </span>
      ))}
      {event.MatchCode && <span className='match-code'>{event.MatchCode}</span>}
    </>
  );
};

MatchInfo.propTypes = propTypes;
export default MatchInfo;
