import { createAsyncThunk } from '@reduxjs/toolkit';
import { TPolicy } from '../type/type';

import { FunctionKeys } from '../../Shared/Constants';
import axios from 'axios';
import AuthorizeService from '../../Authorize/Service';

const queryPolicyThunk = createAsyncThunk<TPolicy>('policy/query', async () => {
  const [
    fullBetTypesPolicy,
    pHBetTypesPolicy,
    detailInformationPolicy,
    eventPickingPolicy,
    dailyWorkingShiftPolicy,
    ticketCenterPolicy,
    notePolicy,
    oddsPolicy
  ] = await axios.all([
    AuthorizeService.authorize(FunctionKeys.LiveGameAssignment_FullBetTypes),
    AuthorizeService.authorize(FunctionKeys.LiveGameAssignment_PHBetTypes),
    AuthorizeService.authorize(FunctionKeys.DetailsInformation),
    AuthorizeService.authorize(FunctionKeys.EventPicking),
    AuthorizeService.authorize(FunctionKeys.DailyWorkingShift),
    AuthorizeService.authorize(FunctionKeys.TicketCenter),
    AuthorizeService.authorize(FunctionKeys.Note),
    AuthorizeService.authorize(FunctionKeys.OddsCompiler)
  ]);

  return {
    UserId: fullBetTypesPolicy.UserId,
    Username: fullBetTypesPolicy.Username,
    FullBetTypes: fullBetTypesPolicy,
    PHBetTypes: pHBetTypesPolicy,
    detailInformationPolicy,
    EventPicking: eventPickingPolicy,
    dailyWorkingShiftPolicy,
    MyTicket: ticketCenterPolicy,
    Note: notePolicy,
    Odds: oddsPolicy
  };
});

export default queryPolicyThunk;
