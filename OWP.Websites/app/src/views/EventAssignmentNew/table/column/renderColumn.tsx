import { Column } from 'fixed-data-table-2';
import FilterHeader from '../../../Shared/FilterHeader/FilterHeaderNative';
import renderCell from '../cell/renderCell';
import { TDataTable } from '../../type/type';
import { onClearFilter, onColumnFilter } from '.././eventAssignmentTableSlice';
import store from '../../app/store';
import { TABLE } from '../../const.json';
import RenderHeaderDropdown from './renderHeaderDropdown';
const {
  HEADER: {
    MATCH_NO,
    KO_TIME,
    EVENT,
    TICKET,
    NOTE,
    IP_TV,
    PROVIDER,
    MB,
    REMARK,
    HDP,
    OU,
    FIRST_HALF_HDP,
    FIRST_HALF_OU,
    ONE_X_TWO,
    FIRST_HALF_ONE_X_TWO,
    FIFTEEN_MINS,
    TEN_MINS,
    FM,
    AIO,
    LA
  }
} = TABLE;

const renderColumn = (
  dataSource: TDataTable,
  onShowPopup: (show: boolean) => void,
  matchesSelected: TDataTable
) => {
  const { getState, dispatch } = store;
  const { filterState } = getState().eventAssignmentTable;
  const firstMatchCheckedIsSaba = matchesSelected[0]?.IsSaBa;
  return [
    MATCH_NO,
    KO_TIME,
    EVENT,
    TICKET,
    IP_TV,
    MB,
    PROVIDER,
    REMARK,
    NOTE,
    HDP,
    OU,
    FIRST_HALF_HDP,
    FIRST_HALF_OU,
    ONE_X_TWO,
    FIRST_HALF_ONE_X_TWO,
    FIFTEEN_MINS,
    TEN_MINS,
    FM,
    AIO,
    LA
  ].map(col => <Column
    key={col.COLUMN_KEY}
    columnKey={col.COLUMN_KEY}
    header={col.IS_CAN_FILTER
      ?
      (col.COLUMN_KEY === MATCH_NO.COLUMN_KEY
        ? <RenderHeaderDropdown
          firstMatchCheckedIsSaba={firstMatchCheckedIsSaba}
          matchesSelected={matchesSelected}
          name={col.NAME}
          title={col.HEADER_NAME}
          keyword={filterState.find(x => x.columnKey === col.COLUMN_KEY)?.keyword ?? ''}
          onClear={() => dispatch(onClearFilter())}
          onSubmit={(columnKey: string, keyword: string) => dispatch(onColumnFilter({
            columnKey,
            keyword
          }))}
        />
        : <FilterHeader
          key={col.COLUMN_KEY}
          isFirstColumn={false}
          name={col.NAME}
          title={col.HEADER_NAME}
          keyword={filterState.find(x => x.columnKey === col.COLUMN_KEY)?.keyword ?? ''}
          onClear={() => dispatch(onClearFilter())}
          onSubmit={(columnKey: string, keyword: string) => dispatch(onColumnFilter({
            columnKey,
            keyword
          }))}
        />)
      : col.HEADER_NAME
    }
    width={col.WIDTH}
    fixed={col.FIXED}
    cell={({ rowIndex }) =>
      renderCell({
        columnKey:
          col.COLUMN_KEY,
        dataSource,
        rowIndex,
        onShowPopup
      })
    }
  />);
};

export default renderColumn;
