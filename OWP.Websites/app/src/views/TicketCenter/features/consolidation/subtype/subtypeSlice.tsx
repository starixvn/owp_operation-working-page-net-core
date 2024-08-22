import { createSlice } from '@reduxjs/toolkit';
import type { PayloadAction } from '@reduxjs/toolkit';
import { TSubTypeRow } from '../../../type';
import { REQUEST_STATUS } from '../../../../Shared/Constants';
import { querySubtypeThunk } from './subtypeThunk';

const subtypeSlice = createSlice({
  name: 'subtypeSlice',
  initialState: {
    loading: REQUEST_STATUS.pending,
    entities: [] as TSubTypeRow[],
    subTypeSelected: {
      typeId: -1,
      subTypeId: -1 as number | null,
      label: '',
      eventType: -1
    }
  },
  reducers: {
    setSelect(state, action: PayloadAction<{ typeId: number, subTypeId: number | null, label: string, eventType: number }>) {
      const { subTypeId, typeId, label, eventType } = action.payload;
      state.subTypeSelected.typeId = typeId;
      state.subTypeSelected.subTypeId = subTypeId;
      state.subTypeSelected.label = label;
      state.subTypeSelected.eventType = eventType;
      const subtypeRow = state.entities.find(e => e.typeId === typeId && e.subTypeId === subTypeId);
      if (subtypeRow) {
        subtypeRow.isRead = true;
      }
    },
    highlightSubtype(state, action: PayloadAction<{ subtype: TSubTypeRow }>) {
      const { subTypeId, typeId } = action.payload.subtype;

      const subtypeRowIndex = state.entities.findIndex(e => e.typeId === typeId && e.subTypeId === subTypeId);
      if (subtypeRowIndex !== -1) {
        state.entities[subtypeRowIndex].isRead = false;
        state.entities.unshift(state.entities.splice(subtypeRowIndex, 1)[0]);
      } else {
        state.entities.unshift(action.payload.subtype);
      }
    }
  },
  extraReducers(builder) {
    builder
      .addCase(querySubtypeThunk.pending, state => {
        state.loading = REQUEST_STATUS.pending;
      })
      .addCase(querySubtypeThunk.fulfilled, (state, { payload }) => {
        state.loading = REQUEST_STATUS.idle;
        state.entities = payload;
      })
      .addCase(querySubtypeThunk.rejected, state => {
        state.loading = REQUEST_STATUS.idle;
      });
  }
});

const { reducer, actions } = subtypeSlice;
const { setSelect, highlightSubtype } = actions;

export { setSelect, highlightSubtype };
export default reducer;
