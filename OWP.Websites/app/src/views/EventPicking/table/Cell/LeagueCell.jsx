import { Fragment, useRef } from 'react';
import PropTypes from 'prop-types';
import { UncontrolledTooltip } from 'reactstrap';
import { uniqueId } from 'lodash';

const propTypes = {
  dataSource: PropTypes.array.isRequired,
  rowIndex: PropTypes.number.isRequired
};

const LeagueCell = props => {
  const { rowIndex, dataSource } = props;
  const { leagueName, leagueGroup, matchOffer, noHDP } = dataSource[rowIndex];
  const matchOfferLst = matchOffer && matchOffer.split(',');
  const refMatchOffer = useRef(null);
  const refNoHdp = useRef(null);
  return (
    <div className='league-cell'>
      <p>
        {leagueGroup && <span className='league-group'>{leagueGroup}</span>}
        <span className='league-name'>{leagueName}</span>
      </p>
      <div className='offer'>
        {matchOffer &&
          matchOfferLst.map(x => (
            <Fragment key={uniqueId()}>
              <span ref={refMatchOffer}>{x}</span>
            </Fragment>
          ))}
        {noHDP && (
          <>
            <span ref={refNoHdp}>{noHDP}</span>
            <UncontrolledTooltip placement='top-start' target={refNoHdp}>
              {noHDP}
            </UncontrolledTooltip>
          </>
        )}
      </div>
    </div>
  );
};

LeagueCell.propTypes = propTypes;
export default LeagueCell;
