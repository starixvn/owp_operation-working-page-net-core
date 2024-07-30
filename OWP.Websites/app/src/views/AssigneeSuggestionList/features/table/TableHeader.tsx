import { getLocaleDateTime } from '../../../Shared/DateTime/dateTimeHelper';
import { timeLineColumn } from '../../const/Const.json';
import { TimeLine } from '../TypeNative';
import { getCurrentTimeLine } from '../helper/AssigneeSuggestionHelper';
type Props = {
  eventDate: Date;
  filterDate: Date;
};

const TableHeader = ({ filterDate, eventDate }: Props) => {
  const currentTimeLine = getCurrentTimeLine(filterDate, eventDate);

  return (
    <tr>
      <th key='working-schedule' className='working-scheduler-header'>
        <span>
          Working Schedule - {getLocaleDateTime(filterDate, 'MMM DD, YYYY')}
        </span>
      </th>
      {timeLineColumn.map((x: TimeLine) => (
        <th
          key={x.id}
          className={
            currentTimeLine?.id === x.id ? 'current-timeline' : 'timeline'
          }>
          <span style={{ color: x.color }}>{x.title} </span>
        </th>
      ))}
    </tr>
  );
};

export default TableHeader;
