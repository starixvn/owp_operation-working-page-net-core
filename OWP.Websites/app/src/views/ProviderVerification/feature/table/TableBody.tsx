import ReadonlyCommonCell from './cell/ReadonlyCommonCell';
import { formatTime } from '../../../Shared/DateTime/dateTimeHelper';
import TicketCell from './cell/TicketCell';
import ConfirmProviderCell from './cell/ConfirmProviderCell';
import { TDataSource } from '../../type/table';
import MatchCell from './cell/MatchCell';
import NoteCell from './cell/NoteCell';
import { useAppSelector } from '../../app/hookNative';
import tableSchema from './TableSchema.json';
import { VirtualItem, Virtualizer } from '@tanstack/react-virtual';

interface TableBodyProps {
  dataSource: TDataSource[];
  virtualizer: Virtualizer<HTMLDivElement, Element>;
}

const renderCell = (header: {
  HEADER_KEY: string;
  HEADER_NAME: string;
  WIDTH: string;
  IS_FILTER: boolean;
} | {
  HEADER_KEY?: string;
  HEADER_NAME: string;
  WIDTH: string;
  IS_FILTER?: boolean;
}, event: TDataSource, virtualRow: VirtualItem, canEditDate: boolean, dataSource: TDataSource[]) => {
  switch (header.HEADER_KEY) {
    case 'MatchNo':
      return (
        <ReadonlyCommonCell
          className='match_no'
          value={event.matchNo}
          index={virtualRow.index}
          isCanToolTip={false}
        />
      );
    case 'MatchStatus':
      return (
        <ReadonlyCommonCell
          className='match_status'
          value={event.matchStatus}
          index={virtualRow.index}
          isCanToolTip={false}
        />
      );
    case 'KoTime':
      return (
        <ReadonlyCommonCell
          className='ko_time'
          value={formatTime(
            event.eventDateTime,
            'minute',
            'hh:mm A'
          )}
          index={virtualRow.index}
          isCanToolTip={false}
        />
      );
    case 'League':
      return (
        <ReadonlyCommonCell
          className='league'
          value={event.league}
          index={virtualRow.index}
          isCanToolTip={false}
        />
      );
    case 'Match':
      return (
        <MatchCell
          className='match'
          home={event.home}
          away={event.away}
        />
      );
    case 'LstBookProvider':
      return (
        <ReadonlyCommonCell
          className='lst-book-provider'
          value={event.lstBookProvider}
          index={virtualRow.index}
          isCanToolTip={true}
        />
      );
    case 'OtherProvider':
      return (
        <ReadonlyCommonCell
          className='other-provider'
          value={event.otherProvider}
          index={virtualRow.index}
          isCanToolTip={false}
        />
      );
    case 'InHouseProviderId':
      return (
        <ReadonlyCommonCell
          className='inhouse-providerId'
          value={
            event.matchStatus?.toLowerCase() === 'cancel'
              ? ''
              : event.inHouseProviderId
          }
          index={virtualRow.index}
          isCanToolTip={false}
        />
      );
    case 'MaxBet':
      return (
        <ReadonlyCommonCell
          className='max-bet'
          value={event.maxBetToDisplay}
          index={virtualRow.index}
          isCanToolTip={false}
        />
      );
    case 'ConfirmProvider':
      return (
        <ConfirmProviderCell
          confirmProvider={event.agentConfirm.map(a => ({
            value: a.value,
            label: a.label
          }))}
          owpMatchId={event.owpMatchId}
          canEditDate={canEditDate}
        />
      );
    case 'Remark':
      return (
        <NoteCell
          className='remark'
          OWPMatchId={event.owpMatchId}
          value={event.remark}
          index={virtualRow.index}
          isRemark={true}
          canEditDate={canEditDate}
        />
      );
    case 'RemarkBy':
      return (
        <NoteCell
          className='remark__by'
          OWPMatchId={event.owpMatchId}
          value={event.remarkBy}
          index={virtualRow.index}
          isRemark={false}
          canEditDate={canEditDate}
        />
      );
    default:
      if (header.HEADER_NAME === 'Ticket') {
        return (
          <TicketCell
            dataSource={dataSource}
            hasTicket={event.hasTicket}
            isRead={event.isRead}
            rowIndex={virtualRow.index}
          />
        );
      }
      return null;
  }
};

const TableBody = ({ dataSource, virtualizer }: TableBodyProps) => {
  const eventDate = useAppSelector(store => store.filterState.filter.eventDate);
  const minDateEdit = new Date();
  minDateEdit.setMonth(minDateEdit.getMonth() - 1);
  const canEditDate = eventDate >= minDateEdit;

  return (
    <tbody>
      <tr style={{ height: `${virtualizer.getTotalSize()}px` }}>
        <td colSpan={13} style={{ padding: 0 }}>
          <div className='table-content-wrapper'>
            <div
              className='table-virtual-content'
              style={{
                position: 'absolute',
                top: 0,
                left: 0,
                right: 0,
                transform: `translateY(${virtualizer.getVirtualItems()[0]?.start ?? 0
                  }px)`
              }}>
              {virtualizer.getVirtualItems().map(virtualRow => {
                const event = dataSource[virtualRow.index];
                return (
                  <div
                    key={event.owpMatchId}
                    className='table-row'
                    style={{
                      backgroundColor: event.rowColor,
                      height: `${virtualRow.size}px`
                    }}>
                    <div className='table-row table-row-content'>
                      {tableSchema.TABLE_HEADER.map((header, idx) => (
                        <div
                          key={header.HEADER_KEY || 'ticket'}
                          className='table-cell'
                          style={{
                            width: header.WIDTH
                          }}>
                          {renderCell(header, event, virtualRow, canEditDate, dataSource)}
                        </div>
                      ))}
                    </div>
                  </div>
                );
              })}
            </div>
          </div>
        </td>
      </tr>
    </tbody>
  );
};

export default TableBody;
