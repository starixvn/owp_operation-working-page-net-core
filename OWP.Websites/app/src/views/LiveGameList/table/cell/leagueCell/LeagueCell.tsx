import { CellProps } from '../../../type/TableType';
import { OFF_VR } from '../../../LiveGameListConst.json';
import { UncontrolledTooltip } from 'reactstrap';

const LeagueCell = ({ rowIndex, dataSource }: CellProps): JSX.Element => {
  const {
    League,
    MatchOffers,
    NoOfHDP,
    NoOfHDPId,
    LeagueGroup,
    NoOfHDPShort,
    AIOBT,
    AIOBTShortName,
    SOP
  } = dataSource[rowIndex];

  const renderNoOfHdpInfo = () =>
    NoOfHDP && (
      <>
        <span
          id={`noOfHdp-${rowIndex}-${NoOfHDPId}`}
          className={`badge-label badge-hdp-${NoOfHDPId}`}>
          {NoOfHDPShort || NoOfHDP.trim().replace(' ', '')}
        </span>
        <UncontrolledTooltip target={`noOfHdp-${rowIndex}-${NoOfHDPId}`}>
          {NoOfHDP}
        </UncontrolledTooltip>
      </>
    );

  const renderMatchOffer = () =>
    MatchOffers.map((x, index) => (
      <span key={`matchOffer-${x.OWPMatchId}-${index}-${rowIndex}`}>
        <span
          id={`matchOffer-${x.OWPMatchId}-${index}-${rowIndex}`}
          className={`badge-label ${
            x.MatchOfferValue.includes(OFF_VR) ? 'off-vr-offer' : 'match-offer'
          } `}>
          {x.MatchOfferShortName || x.MatchOfferValue}
        </span>
        <UncontrolledTooltip
          target={`matchOffer-${x.OWPMatchId}-${index}-${rowIndex}`}>
          {x.MatchOfferValue}
        </UncontrolledTooltip>
      </span>
    ));

  const renderAIOBT = () =>
    AIOBT && (
      <>
        <span
          id={`aiobt-${rowIndex}-${AIOBTShortName ? 'short' : 'full'}`}
          className={`badge-label badge-aiobt`}>
          {AIOBTShortName || AIOBT}
        </span>
        <UncontrolledTooltip
          target={`aiobt-${rowIndex}-${AIOBTShortName ? 'short' : 'full'}`}>
          {AIOBT}
        </UncontrolledTooltip>
      </>
    );

  return (
    <div className='league-cell'>
      <p>
        {LeagueGroup && (
          <span className='league-group league-type'>{LeagueGroup}</span>
        )}
        <span className='league-group league-name'>{League}</span>
      </p>
      <p>
        {renderNoOfHdpInfo()}
        {renderMatchOffer()}
        {renderAIOBT()}
      </p>
    </div>
  );
};

export default LeagueCell;
