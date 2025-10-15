import { TCellModel } from '../type/type';
import { UncontrolledTooltip } from 'reactstrap';

const MatchInfo = (props: TCellModel) => {
  const { rowIndex, dataSource } = props;
  const event = dataSource[rowIndex];

  return (
    <>
      <p className='match-name'>
        {event.Home} -vs- {event.Away}
      </p>
      {event.NoOfHDP && (
        <>
          <span
            id={`matchInfo-noOfHdp-${rowIndex}-${event.NoOfHDPId}`}
            className={`badge-label badge-hdp-${event.NoOfHDPId}`}>
            {event.NoOfHDPShort || event.NoOfHDP}
          </span>
          <UncontrolledTooltip
            target={`matchInfo-noOfHdp-${rowIndex}-${event.NoOfHDPId}`}>
            {event.NoOfHDP}
          </UncontrolledTooltip>
        </>
      )}
      {event.MatchOffers.map((element, index) => (
        <>
          <span
            id={`matchInfo-matchOffer-${rowIndex}-${index}`}
            className={`badge-label ${
              element.MatchOfferValue.includes('OFF VR')
                ? 'off-vr-offer'
                : 'match-offer'
            } `}>
            {element.MatchOfferShortName || element.MatchOfferValue}
          </span>
          <UncontrolledTooltip
            target={`matchInfo-matchOffer-${rowIndex}-${index}`}>
            {element.MatchOfferValue}
          </UncontrolledTooltip>
        </>
      ))}
      {event.AIOBT && (
        <>
          <span
            id={`matchInfo-aiobt-${rowIndex}-${
              event.AIOBTShortName ? 'short' : 'full'
            }`}
            className='badge-label badge-aiobt'>
            {event.AIOBTShortName || event.AIOBT}
          </span>
          <UncontrolledTooltip
            target={`matchInfo-aiobt-${rowIndex}-${
              event.AIOBTShortName ? 'short' : 'full'
            }`}>
            {event.AIOBT}
          </UncontrolledTooltip>
        </>
      )}
      {event.MatchCode && <span className='match-code'>{event.MatchCode}</span>}
    </>
  );
};

export default MatchInfo;
