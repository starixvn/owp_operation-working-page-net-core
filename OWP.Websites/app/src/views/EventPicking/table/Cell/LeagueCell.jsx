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
  const { leagueName, matchOffer, noHDP } = dataSource[rowIndex];
  const matchOfferLst = matchOffer && matchOffer.split(',');
  const refMatchOffer = useRef(null);
  const refNoHdp = useRef(null);
  return (
    <div className='league-cell'>
      <div className='league-name'>{leagueName}</div>
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
