type TLeague = {
  LeagueId: number;
  LeagueName: string;
  LeagueGroupName: string;
  LeagueGroupId: number;
  IsExpanded: boolean;
  Teams: TTeam[];
};

type TTeam = {
  TeamId: number;
  TeamName: string;
};

type TCommonCell = {
  className?: string;
  value: string | null | undefined;
  index: number;
  isCanToolTip: boolean;
};

export {
  TLeague,
  TCommonCell,
  TTeam
};
