import { createReducer } from './reducerUtilities';
import {
  QUERY_CONDITION,
  ACTION_ROUND,
  ACTION_DIVISION,
  ACTION_TV
} from '../constants/action-type';
import Converter from '../../Shared/Converter';
import { generateKeyTVList } from '../helper/commonHelper';

const defaultModel = {
  conditionPending: true,
  tv: {},
  round: [],
  division: []
};

const handlePending = state => {
  return {
    ...state,
    conditionPending: true
  };
};

const handleQuerySuccess = (state, action) => {
  const { LeagueSettingId, HasTV, TV, Rounds, Divisions } = action.data;
  return {
    ...state,
    leagueSettingId: LeagueSettingId,
    tv: {
      hasTV: HasTV,
      tv: TV
    },
    round: Converter.convertResponseToRound(Rounds),
    division: Converter.convertResponseToRound(Divisions),
    conditionPending: false
  };
};

const handleUpdateTV = (state, action) => {
  const { key, value } = action.payload;
  return {
    ...state,
    tv: {
      ...state.tv,
      [key]: value
    }
  };
};

const handleAddRound = (state, action) => {
  const { round } = action.payload;
  const newRounds = state.round.slice();
  newRounds.push(round);
  return {
    ...state,
    round: generateKeyTVList(newRounds)
  };
};

const handleDeleteRound = (state, action) => {
  const { index } = action.payload;
  const newRounds = state.round.slice();
  newRounds.splice(index, 1);
  return {
    ...state,
    round: generateKeyTVList(newRounds)
  };
};

const updateState = props => {
  const { collection, action } = props;
  const { key, value } = action.payload;
  const result = collection.map((item, index) => {
    if (index !== action.payload.index) {
      return item;
    }
    return {
      ...item,
      [key]: value
    };
  });
  return result;
};

const handleUpdateRounds = (state, action) => {
  const props = {
    collection: state.round,
    action
  };
  const round = updateState(props);
  return {
    ...state,
    round
  };
};

const handleUpdateDivision = (state, action) => {
  const props = {
    collection: state.division,
    action
  };
  const division = updateState(props);
  return {
    ...state,
    division
  };
};

const handleAddDivision = (state, action) => {
  const { division } = action.payload;
  const newDivisions = state.division.slice();
  newDivisions.push(division);
  return {
    ...state,
    division: generateKeyTVList(newDivisions)
  };
};

const handleDeleteDivision = (state, action) => {
  const { index } = action.payload;
  const newDivisions = state.division.slice();
  newDivisions.splice(index, 1);
  return {
    ...state,
    division: generateKeyTVList(newDivisions)
  };
};

const conditionReducer = createReducer(defaultModel, {
  [QUERY_CONDITION.PENDING]: handlePending,
  [QUERY_CONDITION.SUCCESS]: handleQuerySuccess,
  [ACTION_TV.UPDATE]: handleUpdateTV,
  [ACTION_ROUND.UPDATE]: handleUpdateRounds,
  [ACTION_ROUND.ADD]: handleAddRound,
  [ACTION_ROUND.DELETE]: handleDeleteRound,
  [ACTION_DIVISION.UPDATE]: handleUpdateDivision,
  [ACTION_DIVISION.ADD]: handleAddDivision,
  [ACTION_DIVISION.DELETE]: handleDeleteDivision
});

export default conditionReducer;
