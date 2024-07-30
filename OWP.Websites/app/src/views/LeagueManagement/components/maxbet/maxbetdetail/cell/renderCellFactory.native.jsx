import ColumnConst from '../../../../constants/ColumnConst';
import CategoryCell from './CategoryCell';
import TimeCell from './TimeCell';
import renderConditionCell from './renderConditionCell';
import LstNoteCell from './LstNoteCell';
import RemarkCell from './RemarkCell';
import renderSpreadCell from './renderSpreadCell';
import renderMaxbetWeekdayCell from './renderMaxbetWeekdayCell';
import renderMaxbetWeekendCell from './renderMaxbetWeekendCell';
import renderMaxbet1X2Cell from './renderMaxbet1X2Cell';
import IsAppliedCell from './IsAppliedCell';
import ActionCell from './ActionCell';
import LastModifiedCell from './LastModifiedCell';

const { columnLeagueDetail } = ColumnConst;
const {
  category,
  time,
  condition,
  lstNote,
  remark,
  spread,
  maxbetWeekday,
  maxbetWeekend,
  maxbetFT1X2,
  isApplied,
  action,
  lastUpdated
} = columnLeagueDetail;

const renderCellFactory = propsCell => {
  const { dataSource, columnKey, rowIndex } = propsCell;
  const data = dataSource[rowIndex];
  const propMaxbetSpread = { data, rowIndex, columnKey };
  switch (columnKey) {
    case category.columnKey:
      return <CategoryCell rowIndex={rowIndex} data={data} />;
    case time.columnKey:
      return <TimeCell rowIndex={rowIndex} data={data} />;
    case condition.subHeader.hasTV.columnKey:
    case condition.subHeader.round.columnKey:
    case condition.subHeader.division.columnKey:
    case condition.subHeader.edit.columnKey:
      return renderConditionCell(data, columnKey);
    case lstNote.columnKey:
      return <LstNoteCell rowIndex={rowIndex} data={data} />;
    case remark.columnKey:
      return <RemarkCell rowIndex={rowIndex} data={data} />;
    case spread.subHeader.hdp.columnKey:
    case spread.subHeader.onehHdp.columnKey:
    case spread.subHeader.onehOu.columnKey:
    case spread.subHeader.ou.columnKey:
      return renderSpreadCell(propMaxbetSpread);
    case maxbetWeekday.subHeader.hdp.columnKey:
    case maxbetWeekday.subHeader.ou.columnKey:
    case maxbetWeekday.subHeader.oneh.columnKey:
    case maxbetWeekday.subHeader.matchOffer.columnKey:
    case maxbetWeekday.subHeader.noHdp.columnKey:
      return renderMaxbetWeekdayCell(propMaxbetSpread);
    case maxbetWeekend.subHeader.hdp.columnKey:
    case maxbetWeekend.subHeader.ou.columnKey:
    case maxbetWeekend.subHeader.oneh.columnKey:
    case maxbetWeekend.subHeader.matchOffer.columnKey:
    case maxbetWeekend.subHeader.noHdp.columnKey:
      return renderMaxbetWeekendCell(propMaxbetSpread);
    case maxbetFT1X2.subHeader.spread.columnKey:
    case maxbetFT1X2.subHeader.ft1X2.columnKey:
    case maxbetFT1X2.subHeader.oneh1X2.columnKey:
    case maxbetFT1X2.subHeader.matchOffer.columnKey:
      return renderMaxbet1X2Cell(propMaxbetSpread);
    case isApplied.columnKey:
      return <IsAppliedCell rowIndex={rowIndex} data={data} />;
    case lastUpdated.columnKey:
      return <LastModifiedCell lastModifiedBy={data.LastModifiedBy} />;
    case action.columnKey:
      return <ActionCell data={data} rowIndex={rowIndex} />;
  }
  return undefined;
};
export default renderCellFactory;
