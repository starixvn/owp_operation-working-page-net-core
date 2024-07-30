import useQueryTaskList from '../features/customHook/useQueryTaskList';
import useSetUserId from '../features/customHook/useSetUserId';
import TaskListTable from '../features/table/TaskListTable';
import Header from '../features/header/Header';
import IconRefresh from '../features/component/IconRefresh';
import { TabProps } from '../type/TTaskListTable';
import useSetRefreshModel from '../features/customHook/useSetRefreshModel';

const TabToday = ({ eventDate, userId }: TabProps) => {
  useQueryTaskList({
    eventDateTime: eventDate
  });
  useSetUserId(userId);
  useSetRefreshModel({ filterDate: eventDate, isToday: true });
  return (
    <>
      <Header refreshComponent={<IconRefresh eventDate={eventDate} />} />
      <TaskListTable />
    </>
  );
};

export default TabToday;
