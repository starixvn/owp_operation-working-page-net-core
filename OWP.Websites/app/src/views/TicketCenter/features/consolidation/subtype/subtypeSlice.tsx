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
      subTypeId: -1 as number|null,
      label: ''
    }
  },
  reducers: {
    setSelect(state, action: PayloadAction<{typeId: number, subTypeId: number|null, label: string}>) {
      const { subTypeId, typeId, label } = action.payload;
      state.subTypeSelected.typeId = typeId;
      state.subTypeSelected.subTypeId = subTypeId;
      state.subTypeSelected.label = label;
      const subtypeRow=state.entities.find(e=>e.typeId===typeId && e.subTypeId===subTypeId);
      if(subtypeRow){
        subtypeRow.isRead= true;
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
const { setSelect } = actions;

export { setSelect };
export default reducer;
