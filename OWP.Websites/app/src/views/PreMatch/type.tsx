type TTicketInfo = {
  no: string;
  ghostNameId: number;
  ghostDetail: string;
  bettypeId: number;
  betOnId: number;
  hdp_ou: number;
  odds: number;
  note: string;
  remark: string;
};
type TRemoveTicketInfo = {
  no: Required<string>;
};

type TUpdateTicketInfo = {
  index: Required<number>;
  ticketInfo: TTicketInfo;
};

type TDirectionTicket = {
  league: string;
  event: string;
  matchId: number;
  leagueGroupId: number;
  includeTrader: boolean;
  ticketInfo: Array<TTicketInfo>;
};

export type {
  TDirectionTicket,
  TTicketInfo,
  TRemoveTicketInfo,
  TUpdateTicketInfo
};