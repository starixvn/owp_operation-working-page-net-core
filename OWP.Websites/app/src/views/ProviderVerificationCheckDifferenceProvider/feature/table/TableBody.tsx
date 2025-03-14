import ReadonlyCommonCell from './cell/ReadonlyCommonCell';
import { formatTime } from '../../../Shared/DateTime/dateTimeHelper';
import TicketCell from './cell/TicketCell';
import ConfirmProviderCell from './cell/ConfirmProviderCell';
import { TDataSource } from '../../type/table';
import MatchCell from './cell/MatchCell';
import NoteCell from '../../../ProviderVerification/feature/table/cell/NoteCell';

const TableBody = ({ dataSource }: { dataSource: TDataSource[]; }) => {
  return (
    <tbody>
      {dataSource.map((event, index) => (
        <tr key={index} style={{ backgroundColor: event.rowColor }}>
          <ReadonlyCommonCell
            className='match_no'
            value={event.matchNo}
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

          <ReadonlyCommonCell
            className='ProviderMatchId'
            value={event.providerMatchId}
            index={index}
            isCanToolTip={false}
          />

          <ReadonlyCommonCell
            className='provider'
            value={event.provider}
            index={index}
            isCanToolTip={false}
          />

          {event.owpMatchId ? (
            <>
              <ConfirmProviderCell
                confirmProvider={event.agentConfirm?.map(({ value, label }) => ({
                  value: value,
                  label: label
                })) ?? []}
                owpMatchId={event.owpMatchId}
              />

              <NoteCell
                className='remark'
                OWPMatchId={event.owpMatchId}
                value={event.remark}
                index={index}
                isRemark={true}
              />

              <NoteCell
                className='remark__by'
                OWPMatchId={event.owpMatchId}
                value={event.remarkBy}
                index={index}
                isRemark={false}
              />

              <td>
                <TicketCell
                  dataSource={dataSource}
                  HasTicket={event.HasTicket}
                  IsRead={event.IsRead}
                  rowIndex={index}
                />
              </td>
            </>
          ) : (
            <>
              <td />
              <td />
              <td />
              <td />
            </>
          )}
        </tr>
      ))}
    </tbody>
  );
};

export default TableBody;
