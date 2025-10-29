import PropTypes from 'prop-types';
import store from '../../app/store';
import MaxBetViewCell from './MaxBetViewCell';
import MaxbetEditableCell from './MaxbetEditableCell';
import { VIEW, EDIT } from '../../mode/mode.json';
import Utils from '../../../Shared/Utils';

const { convertThousandToK } = Utils;

const propTypes = {
  dataSource: PropTypes.array.isRequired,
  rowIndex: PropTypes.number.isRequired
};

const MaxBetCellHdp = props => {
  const { mode } = store.getState().mode;
  const { rowIndex, dataSource } = props;
  const {
    maxbetHdp,
    infoSiteMatchId,
    leagueName,
    minorBettypeId,
    isChildMatch,
    isShowCheckButton
  } = dataSource[rowIndex];


  const component = {
    [VIEW]: (
      <MaxBetViewCell
        maxBet={convertThousandToK(maxbetHdp, true) || undefined}
        leagueName={leagueName}
        isChildMatch={isChildMatch}
      />
    ),
    [EDIT]: (
      <MaxbetEditableCell
        infoSiteMatchId={infoSiteMatchId}
        maxBet={convertThousandToK(maxbetHdp, false) || undefined}
        leagueName={leagueName}
        minorBettypeId={minorBettypeId}
        isChildMatch={isChildMatch}
        isShowCheckButton={isShowCheckButton}
      />
    )
  };
  return component[mode];
};

MaxBetCellHdp.propTypes = propTypes;
export default MaxBetCellHdp;
