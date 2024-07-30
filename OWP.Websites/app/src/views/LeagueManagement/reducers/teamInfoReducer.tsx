import { createReducer } from './reducerUtilities';
import { QUERY_TEAM_INFO } from '../constants/action-type';
import { ITeamInfo } from '../type';
import { AxiosError } from 'axios';

const defaultModel = {
  loading: 'idle',
  entity: [],
  error: undefined
} as ITeamInfo;

type Action = {
  type: string;
  data: Array<{
    OWPTeamId: Required<number>;
    OWPTeamName: Required<string>;
  }>;
  error: Error | AxiosError;
};

const handlePending = (state: ITeamInfo): ITeamInfo => {
  return {
    ...state,
    loading: 'pending'
  };
};

const handleQuerySuccess = (state: ITeamInfo, action: Action): ITeamInfo => {
  return {
    ...state,
    entity: action.data,
    loading: 'idle'
  };
};

const handleError = (state: ITeamInfo, action: Action): ITeamInfo => {
  return {
    ...state,
    loading: 'idle',
    error: action.error
  };
};

const teamInfoReducer = createReducer(defaultModel, {
  [QUERY_TEAM_INFO.PENDING]: handlePending,
  [QUERY_TEAM_INFO.SUCCESS]: handleQuerySuccess,
  [QUERY_TEAM_INFO.ERROR]: handleError
});

export default teamInfoReducer;
