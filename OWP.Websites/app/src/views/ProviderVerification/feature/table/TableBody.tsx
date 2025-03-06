import ReadonlyCommonCell from './cell/ReadonlyCommonCell';
import { formatTime } from '../../../Shared/DateTime/dateTimeHelper';
import TicketCell from './cell/TicketCell';
import ConfirmProviderCell from './cell/ConfirmProviderCell';
import { TDataSource } from '../../type/table';
import MatchCell from './cell/MatchCell';
import RemarkCell from './cell/RemarkCell';

const TableBody = ({ dataSource }: { dataSource: TDataSource[]; }) => {

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

          <ReadonlyCommonCell
            className='lst-book-provider'
            value={event.LstBookProvider}
            index={index}
            isCanToolTip={true}
          />

          <ReadonlyCommonCell
            className='inhouse-providerId'
            value={event.InHouseProviderId}
            index={index}
            isCanToolTip={false}
          />

          <ConfirmProviderCell
            confirmProvider={event.AgentConfirm.map(a => ({
              value: a.Value,
              label: a.Label
            }))}
            owpMatchId={event.OWPMatchId}
          />

          <RemarkCell
            className='remark'
            event={event}
            index={index}
          />

          <RemarkCell
            className='remark__by'
            event={event}
            index={index}
          />

          <td>
            <TicketCell
              dataSource={dataSource}
              HasTicket={event.HasTicket}
              IsRead={event.IsRead}
              rowIndex={index}
            />
          </td>
        </tr>
      ))}
    </tbody>
  );
};

export default TableBody;
