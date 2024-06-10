import { TicketSettingRecipient } from '../../Shared/TicketSetting/TypeNative';

type TicketType = {
  typeId: number;
  typeName: string;
};

interface Child {
  parentId: number;
  subtypeId: number;
  subtypeName: string;
}

interface Subtype {
  subtypeId: number;
  typeId: number;
  subtypeName: string;
}
interface Recipient extends TicketSettingRecipient {
  ticketTypeRecipientId: number;
  subtypeName: string;
}

type TicketTypeRecipientDataSource = {
  ticketType: TicketType;
  recipients: Array<Recipient>;
  subtypes: Array<Subtype>;
};

type CellModel = {
  dataSource: Array<TicketTypeRecipientDataSource>;
  columnKey: number;
};

export {
  CellModel,
  TicketTypeRecipientDataSource,
  Recipient,
  TicketType,
  Subtype,
  Child
};
