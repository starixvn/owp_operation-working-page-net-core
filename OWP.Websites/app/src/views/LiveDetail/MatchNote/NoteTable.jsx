// eslint-disable-next-line no-warning-comments
/* eslint-disable react/jsx-handler-names */
import { createRef, PureComponent } from 'react';
import PropTypes from 'prop-types';
import { map } from 'lodash';
import NoteRow from './NoteRow';
import Service from './Service';
import dayjs from 'dayjs';
import utc from 'dayjs/plugin/utc';

dayjs.extend(utc);
const propTypes = {
  matchId: PropTypes.number.isRequired,
  policy: PropTypes.object.isRequired,
  rows: PropTypes.array.isRequired,
  onSave: PropTypes.func.isRequired
};

class NoteTable extends PureComponent {
  constructor(props) {
    super(props);
    this.addInputRef = createRef();
    this.onSave = this.onSave.bind(this);
    this.onDelete = this.onDelete.bind(this);
    this.onCreate = this.onCreate.bind(this);
  }

  async onSave(index, note) {
    const { policy, rows, onSave } = this.props;
    const row = rows[index];
    const lastUpdated = dayjs(new Date())
      .utc()
      .add(8, 'hour');
    row.LastUpdated = lastUpdated;
    row.Username = policy.Username;
    row.Note = note;
    const { MatchNoteId, Note } = row;
    await Service.update({ MatchNoteId, Note });
    onSave(rows);
  }

  async onDelete(index) {
    const { rows, onSave } = this.props;
    const row = rows[index];
    const { MatchNoteId } = row;
    await Service.delete({ MatchNoteId });
    const leftRows = rows.slice(0, index);
    const rightRows = rows.slice(index + 1);
    const newRows = leftRows.concat(rightRows);
    onSave(newRows);
  }

  async onCreate(event) {
    event.preventDefault();
    const { matchId, rows, onSave } = this.props;
    const note = this.addInputRef.current.value.trim();
    if (note === '') {
      return;
    }

    this.addInputRef.current.value = '';
    const data = await Service.create({
      OWPMatchId: matchId,
      Note: note
    });
    const lastUpdated = dayjs(new Date())
      .utc()
      .add(8, 'hour');
    const newRow = { ...data, Note: note, LastUpdated: lastUpdated };
    const newRows = [newRow].concat(rows);
    onSave(newRows);
  }

  renderTable() {
    const { rows } = this.props;
    if (rows.length === 0) {
      return <div className='no-data'>There is no data</div>;
    } else {
      return <ul className='note-list'>{this.renderRows(rows)}</ul>;
    }
  }

  renderRows(rows) {
    const { policy } = this.props;
    return map(rows, (row, index) => {
      return (
        <NoteRow
          key={row.MatchNoteId}
          policy={policy}
          index={index}
          row={row}
          onDelete={() => this.onDelete(index)}
          onSave={note => this.onSave(index, note)}
        />
      );
    });
  }

  render() {
    const { policy } = this.props;
    return (
      <div>
        {policy.HasWrite && (
          <form className='create-note' onSubmit={this.onCreate}>
            <input
              data-testid="note-testId"
              className='note-textinput'
              type='text'
              placeholder='Write a note'
              ref={this.addInputRef}
            />
          </form>
        )}
        {this.renderTable()}
      </div>
    );
  }
}

NoteTable.propTypes = propTypes;
export default NoteTable;
