import { CellProps } from '../../../type/TTaskListTable';
import { OFF_VR } from '../../const/TableSchema.json';
import { UncontrolledTooltip } from 'reactstrap';

const LeagueCell = ({ rowIndex, dataSource }: CellProps): JSX.Element => {
  const {
    League,
    MatchOffers,
    NoOfHDP,
    NoOfHDPShort,
    NoOfHDPId,
    LeagueGroup,
    AIOBT,
    AIOBTShortName
  } = dataSource;

  const renderNoOfHdpInfo = () =>
    NoOfHDP && (
      <>
        <span
          id={`taskList-noOfHdp-${rowIndex}-${NoOfHDPId}`}
          className={`badge-label badge-hdp-${NoOfHDPId}`}>
          {NoOfHDPShort || NoOfHDP.trim().replace(' ', '')}
        </span>
        <UncontrolledTooltip
          target={`taskList-noOfHdp-${rowIndex}-${NoOfHDPId}`}>
          {NoOfHDP}
        </UncontrolledTooltip>
      </>
    );

  const renderMatchOffer = () =>
    MatchOffers.map((x, index) => (
      <>
        <span
          key={`taskList-matchOffer-${rowIndex}-${index}`}
          id={`taskList-matchOffer-${rowIndex}-${index}`}
          className={`badge-label ${
            x.MatchOfferValue.includes(OFF_VR) ? 'off-vr-offer' : 'match-offer'
          } `}>
          {x.MatchOfferShortName || x.MatchOfferValue}
        </span>
        <UncontrolledTooltip
          target={`taskList-matchOffer-${rowIndex}-${index}`}>
          {x.MatchOfferValue}
        </UncontrolledTooltip>
      </>
    ));

  const renderAIOBT = () =>
    AIOBT && (
      <>
        <span
          id={`taskList-aiobt-${rowIndex}-${AIOBTShortName ? 'short' : 'full'}`}
          className={`badge-label badge-aiobt`}>
          {AIOBTShortName || AIOBT}
        </span>
        <UncontrolledTooltip
          target={`taskList-aiobt-${rowIndex}-${
            AIOBTShortName ? 'short' : 'full'
          }`}>
          {AIOBT}
        </UncontrolledTooltip>
      </>
    );

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
          {renderAIOBT()}
        </p>
      </div>
    </>
  );
};

export default LeagueCell;
