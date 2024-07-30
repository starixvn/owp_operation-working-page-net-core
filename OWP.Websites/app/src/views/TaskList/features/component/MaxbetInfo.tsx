import { TAssignment, TMatchSettingInfo } from '../../type/TTaskListTable';
import Assignment from './Assignment';
import OtherAssignment from './OtherAssignment';

type Props = {
  name: string | null;
  maxbet: TMatchSettingInfo | null;
  traders: TAssignment[];
  others: TAssignment[];
  rowSpan?: number;
};

function MaxbetInfo({ name, maxbet, traders, others, rowSpan }: Props) {
  return (
    <>
      {name && (
        <td rowSpan={rowSpan} className={`name-max-bet bg-${name}`}>
          {name}
        </td>
      )}
      <td rowSpan={rowSpan} className='spread'>
        {maxbet?.MaxbetSpreadToDisplay}
      </td>
      <td rowSpan={rowSpan} className='assignee-group'>
        <Assignment assignment={traders} />
        <OtherAssignment assignment={others} />
      </td>
    </>
  );
}

export default MaxbetInfo;
