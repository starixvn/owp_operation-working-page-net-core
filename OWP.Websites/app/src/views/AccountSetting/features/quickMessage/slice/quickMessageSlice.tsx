import { createSlice } from '@reduxjs/toolkit';
import { REQUEST_STATUS } from '../../../../Shared/Constants';
import { TQuickMessage } from '../../../Type';
import { queryQuickMessageThunk } from './quickMessageThunk';
import type { PayloadAction } from '@reduxjs/toolkit';

const quickMessageSlice = createSlice({
  name: 'quickMessageSlice',
  initialState: {
    editIndex: -1,
    quickMessage: [] as Array<TQuickMessage>,
    editModel: {} as TQuickMessage,
    isCreating: false,
    loading: REQUEST_STATUS.pending
  },
  reducers: {
    setEditIndex: (state, action) => {
      const index = action.payload;
      if (state.editIndex === -1) {
        state.editIndex = index;
        state.editModel = state.quickMessage[index];
      }
    },
    closeEditMode: state => {
      state.editIndex = -1;
      if (state.isCreating) {
        state.quickMessage.pop();
      }
    },
    setEditModel: (state, action: PayloadAction<{ property: 'shortcut' | 'message', value: string }>) => {
      const { property, value } = action.payload;
      state.editModel[property] = value.trim();
    },
    addQuickMessage: state => {
      if (state.editIndex === -1) {
        state.editIndex = state.quickMessage.length;
        const newQuickMessage: TQuickMessage = {
          id: 0,
          shortcut: '',
          message: ''
        };
        state.editModel = newQuickMessage;
        state.quickMessage = [...state.quickMessage, newQuickMessage];
        state.isCreating = true;
      }
    },
  },
  extraReducers: builder => {
    builder
      .addCase(queryQuickMessageThunk.pending, state => {
        state.loading = REQUEST_STATUS.pending;
      })
      .addCase(queryQuickMessageThunk.fulfilled, (state, { payload }) => {
        state.loading = REQUEST_STATUS.idle;
        state.quickMessage = payload;
      })
      .addCase(queryQuickMessageThunk.rejected, state => {
        state.loading = REQUEST_STATUS.pending;
      });
  }
});
const { reducer, actions } = quickMessageSlice;
const { setEditIndex, closeEditMode, setEditModel, addQuickMessage } = actions;
export { closeEditMode, setEditIndex, setEditModel, addQuickMessage };

export default reducer;
