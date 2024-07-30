import { createSelector } from '@reduxjs/toolkit';
import { RootState } from '../../app/storeNative';
import { Permission } from '../../type/TicketType';

const selectDataTaskList = createSelector(
  (state: RootState) => state.taskList.matches,
  (state: RootState) => state.taskList.queryTaskListLoading,
  (state: RootState) => state.taskList.hasDetailInformationPermission,
  (state: RootState) => state.taskList.isHideCompletedMatches,
  (state: RootState) => state.taskList.isShowUnreadMsg,
  (state: RootState) => state.pagePolicy.permission as Permission,
  (
    matches,
    loading,
    hasDetailInformationPermission,
    isHideCompletedMatches,
    isShowUnreadMsg,
    ticketPermission
  ) => ({
    matches,
    loading,
    hasDetailInformationPermission,
    isHideCompletedMatches,
    isShowUnreadMsg,
    ticketPermission
  })
);

export { selectDataTaskList };
