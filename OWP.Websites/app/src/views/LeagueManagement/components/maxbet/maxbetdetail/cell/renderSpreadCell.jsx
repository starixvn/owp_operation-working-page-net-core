import ColumnConst from '../../../../constants/ColumnConst';
import { buildPropsMaxbetSpreadCell } from '../../../../helper/commonHelper';
import SpreadCell from './SpreadCell';

const { columnLeagueDetail } = ColumnConst;
const { spread } = columnLeagueDetail;

const renderSpreadCell = propsSpreadCell => {
  const { data, columnKey, rowIndex } = propsSpreadCell;
  switch (columnKey) {
    case spread.subHeader.hdp.columnKey: {
      const props = buildPropsMaxbetSpreadCell(data, rowIndex, 'Spread_HDP');
      return <SpreadCell {...props} />;
    }
    case spread.subHeader.onehHdp.columnKey: {
      const props = buildPropsMaxbetSpreadCell(data, rowIndex, 'Spread_1HHDP');
      return <SpreadCell {...props} />;
    }
    case spread.subHeader.onehOu.columnKey: {
      const props = buildPropsMaxbetSpreadCell(data, rowIndex, 'Spread_1HOU');
      return <SpreadCell {...props} />;
    }
    case spread.subHeader.ou.columnKey: {
      const props = buildPropsMaxbetSpreadCell(data, rowIndex, 'Spread_OU');
      return <SpreadCell {...props} />;
    }
  }
  return undefined;
};

export default renderSpreadCell;
