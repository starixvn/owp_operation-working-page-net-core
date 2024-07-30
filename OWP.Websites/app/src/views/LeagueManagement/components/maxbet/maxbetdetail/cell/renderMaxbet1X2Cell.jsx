import ColumnConst from '../../../../constants/ColumnConst';
import MaxbetCell from './MaxbetCell';
import MatchOfferCell from './MatchOfferCell';
import SpreadCell from './SpreadCell';
import { buildPropsMaxbetSpreadCell } from '../../../../helper/commonHelper';
const { columnLeagueDetail } = ColumnConst;
const { maxbetFT1X2 } = columnLeagueDetail;

const renderMaxbet1X2Cell = propsMaxbet1X2 => {
  const { data, columnKey, rowIndex } = propsMaxbet1X2;
  switch (columnKey) {
    case maxbetFT1X2.subHeader.spread.columnKey: {
      const props = buildPropsMaxbetSpreadCell(
        data,
        rowIndex,
        'MaxBet_1X2Spread'
      );
      return <SpreadCell {...props} />;
    }
    case maxbetFT1X2.subHeader.ft1X2.columnKey: {
      const props = buildPropsMaxbetSpreadCell(data, rowIndex, 'MaxBet_FT1X2');
      return <MaxbetCell {...props} />;
    }
    case maxbetFT1X2.subHeader.oneh1X2.columnKey: {
      const props = buildPropsMaxbetSpreadCell(data, rowIndex, 'MaxBet_1H1X2');
      return <MaxbetCell {...props} />;
    }
    case maxbetFT1X2.subHeader.matchOffer.columnKey: {
      const props = buildPropsMaxbetSpreadCell(
        data,
        rowIndex,
        'MaxBet_1X2_MatchOffer'
      );
      return <MatchOfferCell {...props} />;
    }
  }
  return undefined;
};

export default renderMaxbet1X2Cell;
