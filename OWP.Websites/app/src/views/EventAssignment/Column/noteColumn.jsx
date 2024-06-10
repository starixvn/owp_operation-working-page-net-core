import { Column } from 'fixed-data-table-2';
import FilterHeader from '../../Shared/FilterHeader/FilterHeaderNative';
import NoteCell from '../Cell/NoteCell';
import {
  Note
} from '../constants/Column';
import { getKeyword } from '../FilterHelper';
import { v4 as uuidv4 } from 'uuid';


const renderNoteColumn = (
  dataSource,
  notePolicy,
  filterModel,
  onShowNote,
  closePopup
) => {
  if (!notePolicy.HasRead) {
    return null;
  }
  const { onClickFilter, onClearTextFilter, columnFilters } = filterModel;
  return (
    <Column
      key={Note.columnKey}
      columnKey={Note.columnKey}
      header={
        <FilterHeader
          key={closePopup ? uuidv4() : Note.columnKey}
          name={Note.name}
          title={Note.title}
          keyword={getKeyword(columnFilters, Note.name)}
          onClear={() => onClearTextFilter([])}
          onSubmit={(name, value) => onClickFilter(name, value)}
        />
      }
      width={Note.width}
      cell={({ rowIndex }) => (
        <NoteCell
          notePolicy={notePolicy}
          data={dataSource}
          rowIndex={rowIndex}
          closePopup={closePopup}
          onShowNote={show => onShowNote(show, rowIndex)}
        />
      )}
    />
  );
};


export {
  renderNoteColumn
};
