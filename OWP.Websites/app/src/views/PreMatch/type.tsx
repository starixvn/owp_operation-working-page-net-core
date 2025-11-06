type TTicketInfo = {
  no: string;
  ghostName: number | undefined;
  ghostDetail: string | undefined;
  betType: number | undefined;
  betOn: string | undefined;
  hdpOu: number | undefined;
  odds: string | undefined;
  note: string | undefined;
  remark: string | undefined;
  errorMessage?: TErrorMessage;
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
  matchId: number;
  leagueGroup: string;
  includeTrader: boolean;
  ticketInfo: Array<TTicketInfo>;
};

type TErrorMessage = {
  no: string;
  ghostName?: string;
  ghostDetail?: string;
  bettype?: string;
  betOn?: string;
  hdp_ou?: string;
  odds?: string;
  note?: string;
};

type TValidationResult = {
  valid: boolean;
  message: TErrorMessage[];
}

type TValidateSingleTicketResult = {
  valid: boolean;
  message: TErrorMessage;
}

type TDirectionTicketLeagueInfo = {
  league: string;
  event: string;
  matchId: number;
  leagueGroup: string;
};

export type {
  TDirectionTicket,
  TTicketInfo,
  TRemoveTicketInfo,
  TUpdateTicketInfo,
  TRenderTicketInfo,
  TErrorMessage,
  TValidationResult,
  TValidateSingleTicketResult,
  TDirectionTicketLeagueInfo
};