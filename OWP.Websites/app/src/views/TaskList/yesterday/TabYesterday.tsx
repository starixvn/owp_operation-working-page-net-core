import useQueryTaskList from '../features/customHook/useQueryTaskList';
import useSetUserId from '../features/customHook/useSetUserId';
import Header from '../features/header/Header';
import TaskListTable from '../features/table/TaskListTable';
import { TabProps } from '../type/TTaskListTable';
import useSetRefreshModel from './../features/customHook/useSetRefreshModel';

const TabYesterday = ({ eventDate, userId }: TabProps) => {
  useQueryTaskList({
    eventDateTime: eventDate
  });
  useSetUserId(userId);
  useSetRefreshModel({ filterDate: eventDate, isToday: false });
  return (
    <>
      <Header />
      <TaskListTable />
    </>
  );
};

export default TabYesterday;
