type TticketList = {
  IsRead: true | false;
  TicketId: Required<number>;
  SubtypeName: string;
  LatestUsername: Required<string>;
  LatestCreatedDate: Required<Date>;
  LatestMessage: string;
};

type TPopup = {
  HasWrite: boolean;
  ticketList: Array<TticketList> | null;
  handleOpenCreateTicket: () => void;
  handleOpenMessage(
    event: React.ChangeEvent<HTMLTableRowElement>,
    T: TticketList
  ): void;
  onCloseTicketList: () => void;
};

export { TPopup, TticketList };
