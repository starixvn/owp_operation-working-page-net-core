import ReadonlyCommonCell from './cell/ReadonlyCommonCell';
import { formatTime } from '../../../Shared/DateTime/dateTimeHelper';
import TicketCell from './cell/TicketCell';
import ConfirmProviderCell from './cell/ConfirmProviderCell';
import { TDataSource } from '../../type/table';
import MatchCell from './cell/MatchCell';
import NoteCell from './cell/NoteCell';
import { useAppSelector } from "../../app/hookNative";

const TableBody = ({ dataSource }: { dataSource: TDataSource[]; }) => {

  const eventDate = useAppSelector(store => store.filterState.filter.eventDate);
  const minDateEdit = new Date();
  minDateEdit.setDate(minDateEdit.getDate() - 14);
  const canEditDate = eventDate >= minDateEdit;

  return (
    <tbody>
      {dataSource.map((event, index) => (
        <tr key={event.owpMatchId} style={{ backgroundColor: event.rowColor }}>
          <ReadonlyCommonCell
            className='match_no'
            value={event.matchNo}
            index={index}
            isCanToolTip={false}
          />

          <ReadonlyCommonCell
            className='match_status'
            value={event.matchStatus}
            index={index}
            isCanToolTip={false}
          />

          <ReadonlyCommonCell
            className='ko_time'
            value={formatTime(event.eventDateTime, 'minute', 'hh:mm A')}
            index={index}
            isCanToolTip={false}
          />

          <ReadonlyCommonCell
            className='league'
            value={event.league}
            index={index}
            isCanToolTip={false}
          />

          <MatchCell className='match' home={event.home} away={event.away} />

          <ReadonlyCommonCell
            className='lst-book-provider'
            value={event.lstBookProvider}
            index={index}
            isCanToolTip={true}
          />

          <ReadonlyCommonCell
            className='inhouse-providerId'
            value={event.inHouseProviderId}
            index={index}
            isCanToolTip={false}
          />

          <ConfirmProviderCell
            confirmProvider={event.agentConfirm.map(a => ({
              value: a.value,
              label: a.label
            }))}
            owpMatchId={event.owpMatchId}
            canEditDate={canEditDate}
          />

          <NoteCell
            className='remark'
            OWPMatchId={event.owpMatchId}
            value={event.remark}
            index={index}
            isRemark={true}
            canEditDate={canEditDate}
          />

          <NoteCell
            className='remark__by'
            OWPMatchId={event.owpMatchId}
            value={event.remarkBy}
            index={index}
            isRemark={false}
            canEditDate={canEditDate}
          />

          <td>
            <TicketCell
              dataSource={dataSource}
              hasTicket={event.hasTicket}
              isRead={event.isRead}
              rowIndex={index}
            />
          </td>
        </tr>
      ))}
    </tbody>
  );
};

export default TableBody;
