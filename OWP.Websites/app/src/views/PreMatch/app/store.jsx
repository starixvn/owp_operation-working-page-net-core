import { configureStore } from '@reduxjs/toolkit';
import shiftSlice from '../features/league/shiftSlice/shiftSlice';
import editReducer from '../features/edit/editSlice';
import autoReducer from '../features/auto/autoSlice';
import filterReducer from '../features/filter/filterSlice';
import deleteLeagueReducer from '../features/deletedLeague/deleteLeagueSlice';
import changedLogReducer from '../features/changeLog/ChangeLogSlice';
import workingShiftReducer from '../features/workingShift/WorkingShiftSlice';
import copyReducer from '../features/copy/copySlice';
import headerReducer from '../features/header/headerSlice';
import policyReducer from '../../Shared/permission/policySlice';
import chatBoardNewReducer from '../../Shared/chatBoardNew/chatBoardSlice';
import signalRReducer from '../features/signalR/signalRSlice';
import noteSlice from '../features/note/NoteSlice';
import createTicketBoardReducerNew from '../../Shared/createTicketNew/createTicketBoardSliceNew';
import directionTicketReducer from '../features/directionTicket/directionTicketSlice';

const store = configureStore({
  reducer: {
    dayShift: shiftSlice,
    editor: editReducer,
    filter: filterReducer,
    deletedLeague: deleteLeagueReducer,
    changedLog: changedLogReducer,
    workingShift: workingShiftReducer,
    copy: copyReducer,
    header: headerReducer,
    policy: policyReducer,
    pagePolicy: policyReducer,
    chatBoard: chatBoardNewReducer,
    signalR: signalRReducer,
    note: noteSlice,
    auto: autoReducer,
    createTicketBoard: createTicketBoardReducerNew,
    directionTicket: directionTicketReducer
  },
  middleware: getDefaultMiddleware =>
    getDefaultMiddleware({
      serializableCheck: false
    }),
  devTools: true
});

export default store;
