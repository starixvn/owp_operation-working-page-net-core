import renderSingleHeader from './renderSingleHeader';
import ColumnConst from '../../../constants/ColumnConst';
import renderCondition from './renderCondition';
import renderSpread from './renderSpread';
import renderMaxbetWeekday from './renderMaxbetWeekday';
import renderMaxbetFT1X2 from './renderMaxbetFT1X2';
import { columnExists } from '../../../helper/toggleColumnHelper';

const { columnLeagueDetail } = ColumnConst;
const {
  category,
  time,
  condition,
  remark,
  lstNote,
  spread,
  maxbetWeekday,
  maxbetWeekend,
  maxbetFT1X2,
  isApplied,
  lastUpdated,
  action
} = columnLeagueDetail;

const pushIfSelected = (
  columnKeys,
  columns,
  dataSource,
  renderFunction,
  headerInfo
) => {
  if (columnExists(columnKeys, headerInfo.columnKey)) {
    const content = renderFunction(dataSource, headerInfo);
    columns.push(content);
  }
};

const renderTableBody = (
  dataSource,
  columnKeys,
  hasWriteGeneral,
  hasWriteOnCondition
) => {
  const columns = [];
  columns.push(renderSingleHeader(dataSource, category));
  columns.push(renderSingleHeader(dataSource, time));
  columns.push(renderCondition(dataSource, condition, hasWriteOnCondition));

  pushIfSelected(columnKeys, columns, dataSource, renderSingleHeader, remark);
  pushIfSelected(columnKeys, columns, dataSource, renderSingleHeader, lstNote);
  pushIfSelected(columnKeys, columns, dataSource, renderSpread, spread);
  pushIfSelected(
    columnKeys,
    columns,
    dataSource,
    renderMaxbetWeekday,
    maxbetWeekday
  );
  pushIfSelected(
    columnKeys,
    columns,
    dataSource,
    renderMaxbetWeekday,
    maxbetWeekend
  );
  pushIfSelected(
    columnKeys,
    columns,
    dataSource,
    renderMaxbetFT1X2,
    maxbetFT1X2
  );

  columns.push(renderSingleHeader(dataSource, isApplied));
  columns.push(renderSingleHeader(dataSource, lastUpdated));
  hasWriteGeneral && columns.push(renderSingleHeader(dataSource, action));
  return columns;
};

export default renderTableBody;
