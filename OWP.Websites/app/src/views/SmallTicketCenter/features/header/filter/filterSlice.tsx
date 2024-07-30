import { createSlice } from "@reduxjs/toolkit";
import { buildTicketTypeFilter, getDefaultDateRange } from "../../../Helper/TicketCenterHelper";
import { getTicketTypeOption } from "./filterThunkNative";
import { REQUEST_STATUS } from "../../../../Shared/Constants";
import type { PayloadAction } from '@reduxjs/toolkit';
import { TTicketTypeOptions, TTicketTypeResponse, TUpdateFilterParams, TUpdateFilterTimeRange } from "../../../type";

const { fromDateDefault, toDateDefault } = getDefaultDateRange();

const filterSlice = createSlice({
  name: 'filterSlice',
  initialState: {
    loadingTicketTypeOptions: REQUEST_STATUS.pending,
    ticketTypeOptionReady: false,
    entities:{
      multipleSelect:{
        ticketTypeSelected: [] as Array<string>,
        ticketTypeOptions: [] as Array<TTicketTypeOptions>
      },
      filterInfo:{
        actorId: -1,
        marketKey:1,
        fromDateFilter: fromDateDefault,
        toDateFilter: toDateDefault,
        keyword: '',
        ticketTypeId: '',
        ticketStatuses: 'Open'
      }
    }
  },
  reducers: {
    setFilterParam: (state, action: PayloadAction<TUpdateFilterParams>) => {
      const {key, value}= action.payload;
      state.entities.filterInfo[key]=value;
    },
    setFilterTimeRange: (state, action: PayloadAction<TUpdateFilterTimeRange>) => {
      const {key, value}= action.payload;
      state.entities.filterInfo[key]=value;
    }
  },
    extraReducers: builder=> {
      builder.addCase(getTicketTypeOption.pending, state=>{
        state.loadingTicketTypeOptions= REQUEST_STATUS.pending;
      })
      .addCase(getTicketTypeOption.fulfilled, (state, action: PayloadAction<TTicketTypeResponse>)=>{
        state.loadingTicketTypeOptions= REQUEST_STATUS.idle;
        state.ticketTypeOptionReady= true;
        const { parents, children }=action.payload;
        const { parentOption, childrenOption, emptyOption } = buildTicketTypeFilter(parents, children);
        
        state.entities.multipleSelect.ticketTypeSelected=[...childrenOption, ...emptyOption].map(
          i => i.value
        );
        state.entities.filterInfo.ticketTypeId=state.entities.multipleSelect.ticketTypeSelected.join(',');
        state.entities.multipleSelect.ticketTypeOptions=[...parentOption, ...childrenOption, ...emptyOption];
      }).addCase(getTicketTypeOption.rejected, state=>{
        state.loadingTicketTypeOptions= REQUEST_STATUS.idle;
      });
    },
  }
);

const { actions, reducer } = filterSlice;
const { setFilterParam, setFilterTimeRange } = actions;
export { setFilterParam, setFilterTimeRange };
export default reducer;