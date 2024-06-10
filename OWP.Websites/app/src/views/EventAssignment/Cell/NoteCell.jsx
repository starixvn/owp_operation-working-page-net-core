import { PureComponent } from 'react';
import { v4 as uuidv4 } from 'uuid';
import NotePopover from '../NotePopover';
import PropTypes from 'prop-types';

class NoteCell extends PureComponent {
  render() {
    const { notePolicy, data, rowIndex, closePopup } = this.props;
    return (
      <NotePopover
        key={closePopup ? uuidv4() : rowIndex}
        policy={notePolicy}
        event={data[rowIndex]}
        onShowNote={show => this.props.onShowNote(show)}
      />
    );
  }
}

const propTypesNote = {
  notePolicy: PropTypes.object.isRequired,
  data: PropTypes.array.isRequired,
  rowIndex: PropTypes.number.isRequired,
  closePopup: PropTypes.bool.isRequired,
  onShowNote: PropTypes.func.isRequired
};

NoteCell.propTypes = propTypesNote;
export default NoteCell;
