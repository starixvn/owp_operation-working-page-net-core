import store from '../../app/store';
import SpreadViewCell from './SpreadViewCell';
import SpreadEditableCell from './SpreadEditableCell';
import { VIEW, EDIT } from '../../mode/mode.json';
import Utils from '../../../Shared/Utils';

const { convertSpreadToPercent, convertToPercent } = Utils;

const SpreadCell = ({
  spread,
  spreadKey,
  infoSiteMatchId,
  leagueName,
  isChildMatch
}: {
  spread: number | string | undefined;
  spreadKey: 'maxbetHdp' | 'spreadOu' | 'spreadHdp';
  infoSiteMatchId: number;
  leagueName: string;
  isChildMatch: boolean;
}) => {
  const spreadValue = convertSpreadToPercent(spread, false);
  const { mode } = store.getState().mode;

  const component = {
    [VIEW]: (
      <SpreadViewCell
        spread={spreadValue}
        leagueName={leagueName}
        isChildMatch={isChildMatch}
      />
    ),
    [EDIT]: (
      <SpreadEditableCell
        spreadKey={spreadKey}
        infoSiteMatchId={infoSiteMatchId}
        spread={convertToPercent(spread)}
        leagueName={leagueName}
      />
    )
  };
  return component[mode];
};

export default SpreadCell;
