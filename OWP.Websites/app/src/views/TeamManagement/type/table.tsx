type TDataSource = {
  no: string;
  leagueId: number;
  league: string;
  teamId: number;
  team: string;
  leagueGroup: string;
  leagueGroupId: number;
};

type TCommonCell = {
  className?: string;
  value: string | null | undefined;
  index: number;
  isCanToolTip: boolean;
};

export {
  TDataSource,
  TCommonCell
};
