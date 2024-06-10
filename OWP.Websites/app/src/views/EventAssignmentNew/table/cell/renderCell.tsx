import MatchNoCell from './NoCell';
import ProviderCell from './ProviderCell';
import KOTimeCell from './KOTimeCell';
import EventCell from './EventCell';
import TicketCell from './TicketCell';
import NoteCell from './NoteCell';
import IPTVCell from './IPTVCell';
import MBCell from './MBCell';
import RemarkCell from './RemarkCell';
import { TBettype, TCellSchema, TDataTable, TNoteCell } from '../../type/type';
import { TABLE } from '../../const.json';
import BettypeCell from '../../components/BettypeCell';
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
    REMARK
  }
} = TABLE;

const renderCell = (cellInfo: TCellSchema) => {
  const { onShowPopup, columnKey, ...rest } = cellInfo;

  switch (columnKey) {
    case MATCH_NO.COLUMN_KEY:
      return <MatchNoCell {...rest} />;
    case KO_TIME.COLUMN_KEY:
      return <KOTimeCell {...rest} />;
    case EVENT.COLUMN_KEY:
      return <EventCell {...rest} />;
    case TICKET.COLUMN_KEY:
      return <TicketCell {...rest} onShowTicketList={onShowPopup} />;
    case NOTE.COLUMN_KEY:
      return <NoteCell {...rest} onShowNote={onShowPopup} />;
    case IP_TV.COLUMN_KEY:
      return <IPTVCell {...rest} />;
    case PROVIDER.COLUMN_KEY:
      return <ProviderCell {...rest} />;
    case MB.COLUMN_KEY:
      return <MBCell {...rest} />;
    case REMARK.COLUMN_KEY:
      return <RemarkCell {...rest} />;
    default:
      return <BettypeCell {...rest} bettypeName={columnKey as TBettype} />;
  }
};

export default renderCell;
