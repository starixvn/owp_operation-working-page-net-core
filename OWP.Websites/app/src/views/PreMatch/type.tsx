type TTicketInfo = {
  ghostNameId: number | undefined;
  ghostDetail: string | undefined;
  bettypeId: number | undefined;
  betOnId: string | undefined;
  hdp_ou: number | undefined;
  odds: string | undefined;
  note: string | undefined;
  remark: string | undefined;
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
  field: keyof TTicketInfo;
  value: number | string;
};

type TDirectionTicket = {
  league: string;
  event: string;
  matchId: number | undefined;
  leagueGroupId: number | undefined;
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