type TLeague = {
  leagueId: number;
  leagueName: string;
  leagueGroupName: string;
  leagueGroupId: number;
  isExpanded: boolean;
  teams: TTeam[];
};

type TTeam = {
  teamId: number;
  teamName: string;
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
