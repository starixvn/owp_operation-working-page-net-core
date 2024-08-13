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

const MaxBetCell = props => {
  const { mode } = store.getState().mode;
  const { rowIndex, dataSource } = props;
  const { maxbetHdp, infoSiteMatchId, leagueName, isChildMatch } =
    dataSource[rowIndex];

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
      />
    )
  };
  return component[mode];
};

MaxBetCell.propTypes = propTypes;
export default MaxBetCell;
