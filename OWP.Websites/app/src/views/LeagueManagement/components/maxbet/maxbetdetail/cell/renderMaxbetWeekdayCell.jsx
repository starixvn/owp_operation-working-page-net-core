import ColumnConst from '../../../../constants/ColumnConst';
import MaxbetCell from './MaxbetCell';
import NoOfHdpCell from './NoOfHdpCell';
import MatchOfferCell from './MatchOfferCell';
import { buildPropsMaxbetSpreadCell } from '../../../../helper/commonHelper';
const { columnLeagueDetail } = ColumnConst;
const { maxbetWeekday } = columnLeagueDetail;

const renderMaxbetWeekdayCell = propsMaxbetWeekday => {
  const { data, columnKey, rowIndex } = propsMaxbetWeekday;
  switch (columnKey) {
    case maxbetWeekday.subHeader.hdp.columnKey: {
      const props = buildPropsMaxbetSpreadCell(data, rowIndex, 'MaxbetWD_HDP');
      return <MaxbetCell {...props} />;
    }
    case maxbetWeekday.subHeader.ou.columnKey: {
      const props = buildPropsMaxbetSpreadCell(data, rowIndex, 'MaxbetWD_OU');
      return <MaxbetCell {...props} />;
    }
    case maxbetWeekday.subHeader.oneh.columnKey: {
      const props = buildPropsMaxbetSpreadCell(data, rowIndex, 'MaxbetWD_1H');
      return <MaxbetCell {...props} />;
    }
    case maxbetWeekday.subHeader.noHdp.columnKey: {
      const props = buildPropsMaxbetSpreadCell(
        data,
        rowIndex,
        'MaxbetWD_NoOfHDP'
      );
      return <NoOfHdpCell {...props} />;
    }
    case maxbetWeekday.subHeader.matchOffer.columnKey: {
      const props = buildPropsMaxbetSpreadCell(
        data,
        rowIndex,
        'MaxbetWD_MatchOffer'
      );
      return <MatchOfferCell {...props} />;
    }
  }
  return undefined;
};

export default renderMaxbetWeekdayCell;
