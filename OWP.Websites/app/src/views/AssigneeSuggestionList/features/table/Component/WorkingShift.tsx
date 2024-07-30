import { timeLineColumn } from '../../../const/Const.json';
import {
  AssignmentCell,
  TimeLine,
  UserWorkingSchedule
} from '../../TypeNative';
import TimeLineCell from '../Cell/TimeLineCell';
import { uniqueId } from 'lodash';

type Props = {
  index: number;
  data: UserWorkingSchedule;
};

const WorkingShift = (props: Props) => {
  const assignmentTimeLine = (assignments: AssignmentCell[]) => (
    <div className='overlap-timeline'>
      {assignments.map((assignment, index) => (
        <TimeLineCell
          color={assignment?.color}
          text={assignment?.text}
          textColor={assignment?.textColor}
          key={uniqueId()}
          borderLeft={index > 0
            ? '1px solid #c8ced3'
            : 'none'} />
      ))}
    </div>
  );

  return (
    <>
      {timeLineColumn.map((x: TimeLine) => {
        const assignments = props.data[
          x.id as keyof UserWorkingSchedule
        ] as AssignmentCell[];
        return (
          <td key={`${props.index}-${x.id}`}>
            {assignments && assignmentTimeLine(assignments)}
          </td>
        );
      })}
    </>
  );
};

export default WorkingShift;
