import { TCondition } from "../table/TableType";

type TOption = {
  value: number;
  label: string | null;
};

type TAddExpressionKey = {
  round: Array<TCondition>;
  division: Array<TCondition>;
}

type TEditSetting = {
  leagueGroupOptions: TOption[],
  loading: string,
  leagueSettingId: number;
  league: string;
  owpLeagueId: number;
  leagueGroupId: number;
  leagueGroup: string;
  spread: string;
  maxbetWD_HDP: string;
  wD_NoOfHDP: string;
  maxbetWK_HDP: string;
  wK_NoOfHDP: string;
  lstNote: string;
  remark2: string;
  comment: string;
  hasTv: boolean;
  hasIpTv: boolean;
  remark: string;
} & TAddExpressionKey;

type TUpdateSettingSender = Omit<TEditSetting, "leagueGroupOptions" | "loading">

type TExpressionRow = {
  key: string;
  condition: {
    value: number;
    label: string;
  };
  value: string;
}

type TLeagueCategory = {
  RefName: string;
  Id: number;
  Value: string;
}

export { TEditSetting, TExpressionRow, TOption, TLeagueCategory, TUpdateSettingSender, TAddExpressionKey };
