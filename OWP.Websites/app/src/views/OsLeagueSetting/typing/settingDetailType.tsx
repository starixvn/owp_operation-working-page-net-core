interface ISettingDetail {
  leagueSettingId?: number;
  leagueName?: string;
  isActive?: boolean;
  isActiveMaxBet?: boolean;
  region?: number | null;
  category?: number | null;
  weekMatch?: number;
  betDelay?: Array<string | number> | null;
  koFromTime?: string;
  koToTime?: string;
  remark?: string;
  isManualUpdateScore?: boolean;
  isIptvOnly?: boolean;
  spreadHDP?: number | null;
  spreadOU?: number | null;
  maxbetHDP?: number | null;
  maxbetOU?: number | null;
  rounds?: Array<string | number> | null;
  offerLines?: Array<string | number> | null;
  providers?: Array<string | number> | null;
  betTypes?: Array<string | number> | null;
  children?: JSX.Element;
}

export { ISettingDetail };
