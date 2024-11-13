import { createSlice } from '@reduxjs/toolkit';
import { ASSIGNMENT_TYPE, BETTYPE } from '../../const/const';

const autoSlice = createSlice({
    name: 'prematchAutoAssignTrader',
    initialState: {
        isAuto: false,
        majorPairs: [
            [
                { userId: 0, username: '', assignmentType: ASSIGNMENT_TYPE.MAIN, bettype: BETTYPE.HDP },
                { userId: 0, username: '', assignmentType: ASSIGNMENT_TYPE.MAIN, bettype: BETTYPE.OU }
            ]
        ],
        majorTakeOverPairs: [
            [
                { userId: 0, username: '', assignmentType: ASSIGNMENT_TYPE.MAIN, bettype: BETTYPE.HDP },
                { userId: 0, username: '', assignmentType: ASSIGNMENT_TYPE.MAIN, bettype: BETTYPE.OU }
            ]
        ],
        earlyPairs: [
            [
                { userId: 0, username: '', assignmentType: ASSIGNMENT_TYPE.MAIN, bettype: BETTYPE.HDP },
                { userId: 0, username: '', assignmentType: ASSIGNMENT_TYPE.MAIN, bettype: BETTYPE.OU }
            ]
        ],
        mainPairs: [
            [
                { userId: 0, username: '', assignmentType: ASSIGNMENT_TYPE.MAIN, bettype: BETTYPE.HDP },
                { userId: 0, username: '', assignmentType: ASSIGNMENT_TYPE.MAIN, bettype: BETTYPE.OU }
            ]
        ]
    },
    reducers: {
        setAutoAssign: (state, action) => {
            state.isAuto = action.payload;
        },
        addAssignmentType: (state, action) => {
            const { fieldName, pairIndex, bettype, assignmentType } = action.payload;
            state[fieldName][pairIndex].push({ userId: 0, username: '', bettype, assignmentType });
        },
        removeAssignmentType: (state, action) => {
            const { fieldName, pairIndex, bettype, assignmentType } = action.payload;
            const index = state[fieldName][pairIndex].findIndex(p => p.bettype === bettype && p.assignmentType === assignmentType);
            state[fieldName][pairIndex].splice(index, 1);
        },
        addPairs: (state, action) => {
            const { fieldName } = action.payload;
            state[fieldName].push([
                { userId: 0, username: '', assignmentType: ASSIGNMENT_TYPE.MAIN, bettype: BETTYPE.HDP },
                { userId: 0, username: '', assignmentType: ASSIGNMENT_TYPE.MAIN, bettype: BETTYPE.OU }
            ]);
        },
        updatePairs: (state, action) => {
            const { fieldName, pairIndex, bettype, assignmentType, userId, username } = action.payload;
            const index = state[fieldName][pairIndex].findIndex(p => p.bettype === bettype && p.assignmentType === assignmentType);
            state[fieldName][pairIndex][index].userId = userId;
            state[fieldName][pairIndex][index].username = username;
        },
        removePairs: (state, action) => {
            const { fieldName, pairIndex } = action.payload;
            state[fieldName].splice(pairIndex, 1);
        }
    }
});

const { actions, reducer } = autoSlice;

export const {
    setAutoAssign, newMajorPairs, updatePairs,
    addAssignmentType, removeAssignmentType, addPairs, removePairs
} = actions;

export default reducer;
