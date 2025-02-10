import ReadonlyCommonCell from './cell/ReadonlyCommonCell';
import { formatTime } from '../../../Shared/DateTime/dateTimeHelper';
import TicketCell from './cell/TicketCell';
import AgentConfirmCell from './cell/AgentConfirmCell';
import { TDataSource } from '../../type/table';
import MatchCell from './cell/MatchCell';

const TableBody = ({ dataSource }: { dataSource: TDataSource[] }) => {
  return (
    <tbody>
      {dataSource.map((event, index) => (
        <tr key={event.OWPMatchId} style={{ backgroundColor: event.RowColor }}>
          <ReadonlyCommonCell
            className='match_no'
            value={event.MatchNo}
            index={index}
            isCanToolTip={false}
          />

          <ReadonlyCommonCell
            className='match_status'
            value={event.MatchStatus}
            index={index}
            isCanToolTip={false}
          />

          <ReadonlyCommonCell
            className='ko_time'
            value={formatTime(event.EventDateTime, 'minute', 'hh:mm A')}
            index={index}
            isCanToolTip={false}
          />

          <ReadonlyCommonCell
            className='league'
            value={event.League}
            index={index}
            isCanToolTip={false}
          />

          <MatchCell className='match' home={event.Home} away={event.Away} />

          <td>
            <TicketCell
              dataSource={dataSource}
              HasTicket={event.HasTicket}
              IsRead={event.IsRead}
              rowIndex={index}
            />
          </td>

          <ReadonlyCommonCell
            className='provider'
            value={event.Provider}
            index={index}
            isCanToolTip={true}
          />

          <ReadonlyCommonCell
            className='other_provider'
            value={event.OtherProvider}
            index={index}
            isCanToolTip={true}
          />

          <ReadonlyCommonCell
            index={index}
            isCanToolTip={true}
            className='agent'
            value={event.AgentEst}
          />

          <ReadonlyCommonCell
            className='remark'
            value={event.RemarkEst}
            index={index}
            isCanToolTip={true}
          />

          <ReadonlyCommonCell
            className='remark__by'
            value={event.RemarkBy}
            index={index}
            isCanToolTip={true}
          />

          <AgentConfirmCell
            agentConfirm={event.AgentConfirm.map(a => ({
              value: a.Value,
              label: a.Label
            }))}
            owpMatchId={event.OWPMatchId}
          />
        </tr>
      ))}
    </tbody>
  );
};

export default TableBody;
