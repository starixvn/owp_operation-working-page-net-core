export type TParentsResponse={
  id: number,
  name: string
}


export type TTicketTypeOptions = {
    value: string;
     title: string;
      key: string;
       showTitle: string;
  };

export type TChildrenResponse={
  parentId: number,
  subTypeId: number,
  subTypeName: string
}
export type TTicketTypeResponse={
  parents:Array<TParentsResponse>,
  children:Array<TChildrenResponse>
}

export type TUpdateFilterParams={
  key: 'keyword'|'ticketTypeId'|'ticketStatuses',
  value: string
}

export type TUpdateFilterTimeRange={
  key: 'fromDateFilter'|'toDateFilter',
  value: Date
}

export type TTicket={
  ticketId: number,
  homeName: string,
  owpLeagueName: string,
  leagueName: string,
  awayName: string,
  koTime: string,
  subject: string,
  isRead: boolean,
  isChild: boolean,
  eventType: number
}

export type TFilerParam={
  actorId: number,
  fromDateFilter: string,
  toDateFilter: string,
  keyword: string,
  ticketTypeId: string,
  ticketStatuses: string,
  marketKey: number
}
