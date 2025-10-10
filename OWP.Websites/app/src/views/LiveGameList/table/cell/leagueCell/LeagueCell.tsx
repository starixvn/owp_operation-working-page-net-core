import { CellProps } from '../../../type/TableType';
import { OFF_VR } from '../../../LiveGameListConst.json';

const LeagueCell = ({ rowIndex, dataSource }: CellProps): JSX.Element => {
  const { League, MatchOffers, NoOfHDP, NoOfHDPId, LeagueGroup, NoOfHDPShort, AIOBTShortName, SOP } =
    dataSource[rowIndex];

  const renderNoOfHdpInfo = () =>
    NoOfHDP && (
      <span className={`badge-label badge-hdp-${NoOfHDPId}`}>
        {NoOfHDPShort || NoOfHDP.trim().replace(' ', '')}
      </span>
    );

  const renderMatchOffer = () =>
    MatchOffers.map(x => (
      <span
        key={`matchOffer-${x.OWPMatchId}-${x.MatchOfferValue}-${rowIndex}`}
        className={`badge-label ${
          x.MatchOfferValue.includes(OFF_VR) ? 'off-vr-offer' : 'match-offer'
        } `}>
        {x.MatchOfferShortName || x.MatchOfferValue}
      </span>
    ));
    
    const renderAIOBT = () => 
      AIOBTShortName && (
        <span className={`badge-label badge-hdp-${AIOBTShortName}`}>
          {AIOBTShortName}
        </span>
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
        {renderAIOBT()}
        {renderNoOfHdpInfo()}
        {renderMatchOffer()}
      </p>
    </div>
  );
};

export default LeagueCell;
