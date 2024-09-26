import PropTypes from 'prop-types';
import store from '../../app/store';
import NoOfHdpViewCell from './NoOfHdpViewCell';
import { VIEW, EDIT } from '../../mode/mode.json';
import NoOfHdpEditableCell from './NoOfHdpEditableCell';

const propTypes = {
  dataSource: PropTypes.array.isRequired,
  rowIndex: PropTypes.number.isRequired
};

const NoOfHdpCell = props => {
  const { mode } = store.getState().mode;
  const { rowIndex, dataSource } = props;
  const {
    noOfHDP,
    infoSiteMatchId,
    leagueName,
    minorBettypeId,
    isChildMatch,
    isShowCheckButton
  } = dataSource[rowIndex];

  const component = {
    [VIEW]: (
      <NoOfHdpViewCell
        value={noOfHDP}
        leagueName={leagueName}
        isChildMatch={isChildMatch}
      />
    ),
    [EDIT]: (
      <NoOfHdpEditableCell
        isChildMatch={isChildMatch}
        infoSiteMatchId={infoSiteMatchId}
        value={noOfHDP}
        leagueName={leagueName}
        minorBettypeId={minorBettypeId}
        isShowCheckButton={isShowCheckButton}
      />
    )
  };
  return component[mode];
};

NoOfHdpCell.propTypes = propTypes;
export default NoOfHdpCell;
