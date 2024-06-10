import ColumnConst from '../../../../constants/ColumnConst';
import MaxbetCell from './MaxbetCell';
import NoOfHdpCell from './NoOfHdpCell';
import MatchOfferCell from './MatchOfferCell';
import { buildPropsMaxbetSpreadCell } from '../../../../helper/commonHelper';
const { columnLeagueDetail } = ColumnConst;
const { maxbetWeekend } = columnLeagueDetail;

const renderMaxbetWeekendCell = propsMaxbetWeekend => {
  const { data, columnKey, rowIndex } = propsMaxbetWeekend;
  switch (columnKey) {
    case maxbetWeekend.subHeader.hdp.columnKey: {
      const props = buildPropsMaxbetSpreadCell(data, rowIndex, 'MaxbetWK_HDP');
      return <MaxbetCell {...props} />;
    }
    case maxbetWeekend.subHeader.ou.columnKey: {
      const props = buildPropsMaxbetSpreadCell(data, rowIndex, 'MaxbetWK_OU');
      return <MaxbetCell {...props} />;
    }
    case maxbetWeekend.subHeader.oneh.columnKey: {
      const props = buildPropsMaxbetSpreadCell(data, rowIndex, 'MaxbetWK_1H');
      return <MaxbetCell {...props} />;
    }
    case maxbetWeekend.subHeader.noHdp.columnKey: {
      const props = buildPropsMaxbetSpreadCell(
        data,
        rowIndex,
        'MaxbetWK_NoOfHDP'
      );
      return <NoOfHdpCell {...props} />;
    }
    case maxbetWeekend.subHeader.matchOffer.columnKey: {
      const props = buildPropsMaxbetSpreadCell(
        data,
        rowIndex,
        'MaxbetWK_MatchOffer'
      );
      return <MatchOfferCell {...props} />;
    }
  }
  return undefined;
};

export default renderMaxbetWeekendCell;
