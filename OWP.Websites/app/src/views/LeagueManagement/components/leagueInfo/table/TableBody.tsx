import { uniqueId } from 'lodash';
import { useAppSelector } from '../app/hooks';
import { classNameCells, itemRowHeight } from './TableSchema.json';
import { TDataSource } from './TableType';
import CommentCell from './cell/CommentCell';
import CommonCell from './cell/CommonCell';
import LeagueCell from './cell/LeagueCell';
import RemoveCell from './cell/RemoveCell';


const TableBody = ({ data = [], displayStart = 0, displayEnd = 0 }) => {
  const rows = [];
  const focusSettingId = useAppSelector(
    state => state.leagueInfoTableSlice.focusSettingId
  );

  rows.push(
    <tr
      key='startRowFiller'
      style={{ height: displayStart * itemRowHeight }}></tr>
  );
  for (let i = displayStart; i < displayEnd; ++i) {
    const row = data[i] as TDataSource;
    const buildValueForTvCell = () => {
      const tv = row.condition.hasTv ? 'TV' : '';
      const iptv = row.condition.hasIpTv ? 'IPTV' : '';
      const linkVerb = tv.length && iptv.length ? '/' : '';
      return `${tv}${linkVerb}${iptv}`.trim();
    };
    if (row) {
      const className =
        focusSettingId === row.leagueSettingId ? 'is-focus-row' : '';
      rows.push(
        <tr key={uniqueId()} className={className}>
          <CommonCell
            className={classNameCells.noCell.className}
            value={(i + 1).toString()}
          />
          <CommonCell
            className={classNameCells.leagueGroupCell.className}
            value={row.leagueGroup}
          />
          <LeagueCell
            className={classNameCells.leagueCell.className}
            dataSource={row}
          />
          <CommonCell
            className={classNameCells.tvCell.className}
            value={buildValueForTvCell()}
          />
          <CommonCell
            className={classNameCells.roundCell.className}
            value={row.condition.round
              .map(x => (x.type === 'Contains' ? `*${x.value}*` : x.value))
              .join('/')}
          />
          <CommonCell
            className={classNameCells.leagueDivisionCell.className}
            value={row.condition.division
              .map(x => (x.type === 'Contains' ? `*${x.value}*` : x.value))
              .join('/')}
          />
          <CommonCell
            className={classNameCells.remarkCell.className}
            value={row.condition.remark}
          />
          <CommonCell
            className={classNameCells.spreadHDP_OUCell.className}
            value={row.spreadShow}
          />
          <CommonCell
            className={classNameCells.mbWeekdayHDPCell.className}
            value={row.maxbetWD_HDP_Show}
          />
          <CommonCell
            className={classNameCells.mbWeekdayNoHDPCell.className}
            value={row.wD_NoOfHDP_Show}
          />
          <CommonCell
            className={classNameCells.mbWeekendHDPCell.className}
            value={row.maxbetWK_HDP_Show}
          />
          <CommonCell
            className={classNameCells.mbWeekendHDPCell.className}
            value={row.wK_NoOfHDP_Show}
          />
          <CommonCell
            className={classNameCells.lstNoteCell.className}
            value={row.lstNote}
          />
          <CommonCell
            className={classNameCells.remark2Cell.className}
            value={row.remark2}
          />
          <CommentCell
            className={classNameCells.commentCell.className}
            value={row.comment}
          />
          <RemoveCell
            leagueSettingId={row.leagueSettingId}
          />
        </tr>
      );
    }
  }
  rows.push(
    <tr
      key='endRowFiller'
      style={{ height: (data.length - displayEnd) * itemRowHeight }}></tr>
  );

  return <>{rows}</>;
};

export default TableBody;
