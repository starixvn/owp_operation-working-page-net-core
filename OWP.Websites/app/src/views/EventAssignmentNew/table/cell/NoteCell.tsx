import { v4 as uuidv4 } from 'uuid';
import NotePopover from '../../components/NotePopover';
import { TNoteCell } from '../../type/type';
import { useAppSelector } from '../../app/hooks';

const NoteCell = ({
  dataSource,
  rowIndex,
  onShowNote
}: TNoteCell) => {
  const notePolicy = useAppSelector(state => state.policy.entities.Note);
  return (
    <NotePopover
      key={uuidv4()}
      policy={notePolicy}
      event={dataSource[rowIndex]}
      onShowNote={onShowNote}
    />
  );
};

export default NoteCell;
