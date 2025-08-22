import MultipleFilterHeader from './MultipleFilterHeader';
import FilterHeader from './FilterHeader';
import { COLUMN_KEY, classNameCells } from './TableSchema.json';
import { TColumnKey } from './TableType';

const {
  League_Group,
  League,
  Tv,
  Round,
  League_Division,
  Remark,
  Spread_HDP_OU,
  MB_WK_HDP,
  MB_WK_NO_HDP,
  MB_WD_HDP,
  MB_WD_NO_HDP,
  LST_Note,
  Remark2,
  Comment
} = COLUMN_KEY;

const TableHeader = () => {
  return (
    <>
      <tr>
        <th
          style={{ width: `${classNameCells.noCell.width}` }}
          rowSpan={2}
          className='blue-header'>
          <div className='header-table'>
            <span>#</span>
          </div>
        </th>
        <th
          style={{
            width: `${classNameCells.leagueGroupCell.width}`,
            textAlign: 'center'
          }}
          rowSpan={2}
          className='blue-header'>
          <div className='header-table'>
            <span>League Group</span>
            <MultipleFilterHeader columnKey={League_Group as TColumnKey} />
          </div>
        </th>
        <th
          style={{ width: `${classNameCells.leagueCell.width}` }}
          rowSpan={2}
          className='blue-header'>
          <div className='header-table'>
            <span>League</span>
            <FilterHeader columnKey={League as TColumnKey} />
          </div>
        </th>
        <th rowSpan={1} colSpan={4} className='grey-header'>
          Condition
        </th>
        <th
          style={{ width: `${classNameCells.spreadHDP_OUCell.width}` }}
          rowSpan={2}
          className='blue-header'>
          <div className='header-table'>
            <span>
              Spread
              <br />
              HDP - OU
            </span>
            <MultipleFilterHeader columnKey={Spread_HDP_OU as TColumnKey} />
          </div>
        </th>
        <th rowSpan={1} colSpan={2} className='light-blue-header'>
          MB Weekday
        </th>
        <th rowSpan={1} colSpan={2} className='pink-header'>
          MB Weekend
        </th>
        <th rowSpan={1} colSpan={2} className='blue-header'>
          Display in Mis match file
        </th>
        <th
          style={{ width: `${classNameCells.commentCell.width}` }}
          rowSpan={2}
          className='grey-header'>
          <div className='header-table last-header'>
            <span>Comment</span>
            <MultipleFilterHeader columnKey={Comment as TColumnKey} />
          </div>
        </th>
        <th
          style={{
            width: `${classNameCells.action.width}`,
            textAlign: 'center'
          }}
          rowSpan={2}
          className='blue-header'>
          <div className='header-table'>
            <span>Action</span>
          </div>
        </th>
      </tr>
      <tr>
        <th
          style={{ width: `${classNameCells.tvCell.width}` }}
          rowSpan={1}
          className='blue-header'>
          <div className='header-table'>
            <span>TV</span>
            <MultipleFilterHeader columnKey={Tv as TColumnKey} />
          </div>
        </th>
        <th
          style={{ width: `${classNameCells.roundCell.width}` }}
          rowSpan={1}
          className='blue-header'>
          <div className='header-table'>
            <span>Round</span>
            <MultipleFilterHeader columnKey={Round as TColumnKey} />
          </div>
        </th>
        <th
          style={{ width: `${classNameCells.leagueDivisionCell.width}` }}
          rowSpan={1}
          className='blue-header'>
          <div className='header-table'>
            <span>League Division</span>
            <MultipleFilterHeader columnKey={League_Division as TColumnKey} />
          </div>
        </th>
        <th
          style={{ width: `${classNameCells.remarkCell.width}` }}
          rowSpan={1}
          className='blue-header'>
          <div className='header-table'>
            <span>Remark</span>
            <MultipleFilterHeader columnKey={Remark as TColumnKey} />
          </div>
        </th>
        <th
          style={{ width: `${classNameCells.mbWeekdayHDPCell.width}` }}
          rowSpan={1}
          className='light-blue-header'>
          <div className='header-table'>
            <span>HDP(K)</span>
            <MultipleFilterHeader columnKey={MB_WD_HDP as TColumnKey} />
          </div>
        </th>
        <th
          style={{ width: `${classNameCells.mbWeekdayNoHDPCell.width}` }}
          rowSpan={1}
          className='light-blue-header'>
          <div className='header-table'>
            <span>NO.HDP</span>
            <MultipleFilterHeader columnKey={MB_WD_NO_HDP as TColumnKey} />
          </div>
        </th>
        <th
          style={{ width: `${classNameCells.mbWeekendHDPCell.width}` }}
          rowSpan={1}
          className='pink-header'>
          <div className='header-table'>
            <span>HDP</span>
            <MultipleFilterHeader columnKey={MB_WK_HDP as TColumnKey} />
          </div>
        </th>
        <th
          style={{ width: `${classNameCells.mbWeekendNoHDPCell.width}` }}
          rowSpan={1}
          className='pink-header'>
          <div className='header-table'>
            <span>NO.HDP</span>
            <MultipleFilterHeader columnKey={MB_WK_NO_HDP as TColumnKey} />
          </div>
        </th>
        <th
          style={{ width: `${classNameCells.lstNoteCell.width}` }}
          rowSpan={1}
          className='blue-header'>
          <div className='header-table'>
            <span>LST Note (Show under League)</span>
            <MultipleFilterHeader columnKey={LST_Note as TColumnKey} />
          </div>
        </th>
        <th
          style={{ width: `${classNameCells.remark2Cell.width}` }}
          rowSpan={1}
          className='blue-header'>
          <div className='header-table'>
            <span>Remark 2 (Show under match)</span>
            <MultipleFilterHeader columnKey={Remark2 as TColumnKey} />
          </div>
        </th>
      </tr>
    </>
  );
};
export default TableHeader;
