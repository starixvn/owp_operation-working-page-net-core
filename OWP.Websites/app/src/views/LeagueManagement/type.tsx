import { AxiosError } from 'axios';
interface ITeamInfo {
  loading: 'idle' | 'pending';
  entity: Array<{
    OWPTeamId: Required<number>;
    OWPTeamName: Required<string>;
  }>;
  error: Error | AxiosError | undefined;
}

interface IPropsTeamInfo {
  owpLeagueId: Required<number>;
  onClose(): void;
  leagueName: Required<string>;
}

export { IPropsTeamInfo, ITeamInfo };
