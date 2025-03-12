import { TRemarkCell } from '../../../type/table';
import { useAppSelector } from '../../../app/hookNative';
import NotePopover from '../components/NotePopover';

const NoteCell = ({
  className,
  OWPMatchId,
  value,
  index,
  isRemark
}: TRemarkCell) => {
  const { HasWrite } = useAppSelector(state => state.providerPolicy.permission);

  return (
    <NotePopover
      className={className}
      hasWrite={HasWrite}
      value={value}
      OWPMatchId={OWPMatchId}
      index={index}
      isRemark={isRemark}
    />
  );
};

export default NoteCell;
