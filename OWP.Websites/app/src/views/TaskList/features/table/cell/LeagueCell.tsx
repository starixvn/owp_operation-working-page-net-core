import { CellProps } from '../../../type/TTaskListTable';
import { OFF_VR } from '../../const/TableSchema.json';

const LeagueCell = ({ rowIndex, dataSource }: CellProps): JSX.Element => {
  const { League, MatchOffers, NoOfHDP, NoOfHDPId, LeagueGroup } = dataSource;

  const renderNoOfHdpInfo = () =>
    NoOfHDP && (
      <span className={`badge-label badge-hdp-${NoOfHDPId}`}>
        {NoOfHDP.trim().replace(' ', '')}
      </span>
    );

  const renderMatchOffer = () =>
    MatchOffers.map((x, index) => (
      <span
        key={`matchOffer-${x.MatchOfferValue}-${rowIndex}-${index}`}
        className={`badge-label ${
          x.MatchOfferValue.includes(OFF_VR) ? 'off-vr-offer' : 'match-offer'
        } `}>
        {x.MatchOfferValue}
      </span>
    ));

  return (
    <>
      <div className='league-cell'>
        <p>
          {LeagueGroup && (
            <span className='league-group league-badge'>{LeagueGroup}</span>
          )}
          <span className='league-group league-name'>{League}</span>
        </p>
        <p>
          {renderNoOfHdpInfo()}
          {renderMatchOffer()}
        </p>
      </div>
    </>
  );
};

export default LeagueCell;
