import { createSlice } from '@reduxjs/toolkit';
import { ASSIGNMENT_TYPE, BETTYPE } from '../../const/const';

const autoSlice = createSlice({
  name: 'auto',
  initialState: {
    isAuto: false,
    pairs:{
      major:[],
      main:[],
      early:[],
      majorTakeOver:[]
    }
   
  },
  reducers: {
    setAutoAssign: {
      reducer: (state, action) => {
        state.isAuto = action.payload;
      }
    },
    newMajorPairs:{
      reducer: (state, action) => {
        const {
          userId, userName, assignmentType,
          bettype
        }= action.payload;
        state.pairs.major.push({
          ...action.payload
        });
      }
    },
    updateMajorPairs:{
      reducer: (state, action) => {
        const {bettype, assignmentType, userId, userName}=action.payload;
        const pair= state.pairs.major.find(p=>p.bettype===bettype && p.assignmentType===assignmentType);
        pair.userId= userId;
        pair.userName= userName;
      }
    },

    removeMajorPairs:{
      reducer: (state, action) => {
        const {bettype, assignmentType}=action.payload;
        if(assignmentType===ASSIGNMENT_TYPE.MAIN){
          if(bettype===BETTYPE.HDP)
          {
            state.pairs.major= state.pairs.major.filter(p=>p.bettype!==BETTYPE.HDP);
          }
          if(bettype===BETTYPE.OU){
            state.pairs.major= state.pairs.major.filter(p=>p.bettype!==BETTYPE.OU);
          }
        }

        if(assignmentType===ASSIGNMENT_TYPE.OTHER){
          if(bettype===BETTYPE.HDP)
            {
              state.pairs.major= state.pairs.major.filter(p=>p.bettype!==BETTYPE.HDP && p.assignmentType!==ASSIGNMENT_TYPE.OTHER);
            }
            if(bettype===BETTYPE.OU){
              state.pairs.major= state.pairs.major.filter(p=>p.bettype!==BETTYPE.OU&& p.assignmentType!==ASSIGNMENT_TYPE.OTHER);
            }
        }
      }
    }
  }
});

const { actions, reducer } = autoSlice;
const { setAutoAssign, newMajorPairs, removeMajorPairs, updateMajorPairs } = actions;
export { setAutoAssign, newMajorPairs, removeMajorPairs, updateMajorPairs };
export default reducer;
