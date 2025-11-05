type TTicketInfo = {
  ghostNameId: number;
  ghostDetail: string;
  bettypeId: number;
  betOnId: string;
  hdp_ou: number;
  odds: string;
  note: string;
  remark: string;
};

type TRenderTicketInfo = {
  index: number;
  isRenderRemoveCommand: boolean;
  isRenderAddCommand: boolean;
  ticketInfo: TTicketInfo;
}
type TRemoveTicketInfo = {
  index: Required<number>;
};

type TUpdateTicketInfo = {
  index: Required<number>;
  key: keyof TTicketInfo;
  value: number | string;
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
  TUpdateTicketInfo,
  TRenderTicketInfo
};