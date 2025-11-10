type TticketList = {
  IsRead: true | false;
  TicketId: Required<number>;
  SubtypeName: string;
  LatestUsername: Required<string>;
  LatestCreatedDate: Required<Date>;
  LatestMessage: string;
  Home: string;
  Away: string;
  EventDate: Required<Date>;
  IsRC: boolean;
  DiscussionName: string;
};

type TPopup = {
  HasWrite: boolean;
  isMatch: boolean;
  allowQuickAdd: boolean;
  ticketList: Array<TticketList> | null;
  handleOpenCreateTicket: () => void;
  handleOpenCreateDiscussion: () => void;
  handleQuickAddTicket: () => void;
  handleOpenMessage(
    event: React.ChangeEvent<HTMLTableRowElement>,
    T: TticketList
  ): void;
  onCloseTicketList: () => void;
  handleCreateDiscussionRC: () => void;
  authToken: string;
};

export { TPopup, TticketList };
