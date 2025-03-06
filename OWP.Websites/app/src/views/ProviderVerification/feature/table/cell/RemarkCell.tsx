import { TRemarkCell } from '../../../type/table';
import { useAppSelector } from '../../../app/hookNative';
import RemarkPopover from '../components/RemarkPopover';

const RemarkCell = ({
  className,
  event,
  index
}: TRemarkCell) => {
  const { HasWrite } = useAppSelector(state => state.pagePolicy.permission);

  return (
    <td className={className} id={`${className}-${index}`}>
      <RemarkPopover
        hasWrite={HasWrite}
        event={event}
        index={index}
      />
    </td>
  );
};

export default RemarkCell;
