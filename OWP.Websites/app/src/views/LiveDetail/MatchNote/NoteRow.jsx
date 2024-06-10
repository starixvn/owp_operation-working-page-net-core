// eslint-disable-next-line no-warning-comments
/* eslint-disable react/jsx-handler-names */
import { PureComponent } from 'react';
import PropTypes from 'prop-types';
import PencilIcon from 'mdi-react/PencilIcon';
import DeleteIcon from 'mdi-react/DeleteIcon';
import CheckIcon from 'mdi-react/CheckIcon';
import CloseIcon from 'mdi-react/CloseIcon';
import TextareaAutosize from 'react-autosize-textarea';
import dayjs from 'dayjs';
import ToastrWrapper from '../../components/ToastrWrapper';

const propTypes = {
  policy: PropTypes.object.isRequired,
  row: PropTypes.object.isRequired,
  onDelete: PropTypes.func.isRequired,
  onSave: PropTypes.func.isRequired
};

class NoteRow extends PureComponent {
  constructor(props) {
    super(props);
    const { row } = this.props;
    this.state = {
      isEditing: false,
      note: row.Note
    };
    this.onChange = this.onChange.bind(this);
    this.onEdit = this.onEdit.bind(this);
    this.onSave = this.onSave.bind(this);
    this.onCancel = this.onCancel.bind(this);
    this.onDelete = this.onDelete.bind(this);
  }

  onChange(event) {
    this.setState({ note: event.target.value });
  }

  renderNoteSection() {
    const { row } = this.props;
    const { note, isEditing } = this.state;
    const { Username, LastUpdated } = row;
    if (isEditing) {
      return (
        <div className='content-edit'>
          <TextareaAutosize
            className='note-textinput'
            value={note}
            maxLength='4000'
            onChange={this.onChange}
          />
        </div>
      );
    } else {
      return (
        <div className='content'>
          <div className='note-content'>
            <strong className='user-name'>@{Username}</strong> {note}
          </div>
          <span className='date-time'>
            {LastUpdated
              ? dayjs(LastUpdated, 'YYYY-MM-DDTHH:mm:ss').format(
                'MM/DD/YYYY hh:mm A'
              )
              : ''}
          </span>
        </div>
      );
    }
  }

  onEdit() {
    this.setState({ isEditing: true });
  }

  async onDelete() {
    const result = await ToastrWrapper.confirm(
      'Are you sure you want to permanently delete this note ?'
    );
    if (result.value) {
      const { onDelete } = this.props;
      onDelete();
    }
  }

  onCancel() {
    this.setState({
      isEditing: false,
      note: this.props.row.Note
    });
  }

  onSave() {
    const { row, onSave } = this.props;
    const { note } = this.state;
    this.setState({ isEditing: false });
    if (row.Note !== note) {
      onSave(note);
    }
  }

  renderActionSection() {
    const { policy } = this.props;
    if (!policy.HasWrite) {
      return <div className='icon-button' />;
    }

    const { isEditing } = this.state;
    if (isEditing) {
      return (
        <div className='icon-button'>
          <button className='icon-save' onClick={this.onSave}>
            <CheckIcon size={12} />
          </button>
          <button className='icon-cancel' onClick={this.onCancel}>
            <CloseIcon size={12} />
          </button>
        </div>
      );
    }

    return (
      <div className='icon-button'>
        <button className='icon-edit' onClick={this.onEdit}>
          <PencilIcon size={12} />
        </button>
        <button className='icon-delete' onClick={this.onDelete}>
          <DeleteIcon size={12} />
        </button>
      </div>
    );
  }

  render() {
    return (
      <li className='note-item'>
        {this.renderNoteSection()}
        {this.renderActionSection()}
      </li>
    );
  }
}

NoteRow.propTypes = propTypes;
export default NoteRow;
