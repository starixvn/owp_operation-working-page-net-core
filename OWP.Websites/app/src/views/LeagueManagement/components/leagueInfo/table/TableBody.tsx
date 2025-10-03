import { uniqueId } from 'lodash';
import { classNameCells, itemRowHeight } from './TableSchema.json';
import { TDataSource } from './TableType';
import CommonCell from './cell/CommonCell';
import LeagueCell from './cell/LeagueCell';
import ActionCell from './cell/ActionCell';
import Remark2Cell from './cell/Remark2Cell';
import { convertConditionResponseToDisplayValue } from '../helper/helper';

type TProp = {
  data: TDataSource[];
  displayStart: number;
  displayEnd: number;
};
const TableBody = ({ data = [], displayStart = 0, displayEnd = 0 }: TProp) => {
  const rows = [];

  rows.push(
    <tr
      key='startRowFiller'
      style={{ height: displayStart * itemRowHeight }}></tr>
  );
  for (let i = displayStart; i < displayEnd; ++i) {
    const row = data[i];
    const buildValueForTvCell = () => {
      const tv = row.condition.hasTv ? 'TV' : '';
      const iptv = row.condition.hasIpTv ? 'IPTV' : '';
      const linkVerb = tv.length && iptv.length ? '/' : '';
      return `${tv}${linkVerb}${iptv}`.trim();
    };

    if (row) {
      const className = [row.isBlackList || row.noOfferLive ? 'color-gray' : '']
        .filter(Boolean)
        .join(' ');
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
          <CommonCell value={row.shortLeague} />
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
            value={convertConditionResponseToDisplayValue(row.condition.round)}
          />
          <CommonCell
            className={classNameCells.leagueDivisionCell.className}
            value={convertConditionResponseToDisplayValue(row.condition.division)}
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
          <Remark2Cell
            className={classNameCells.remark2Cell.className}
            remark2={row.remark2}
          />

          <ActionCell
            leagueSettingId={row.leagueSettingId}
            dataSource={row}
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
