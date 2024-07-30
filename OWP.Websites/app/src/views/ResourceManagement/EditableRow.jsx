import { Component } from 'react';
import PropTypes from 'prop-types';
import EditableCell from './EditableCell';
import ActionCell from './ActionCell';

const propTypes = {
  policy: PropTypes.object.isRequired,
  index: PropTypes.number.isRequired,
  isEditing: PropTypes.bool.isRequired,
  allowEdit: PropTypes.bool.isRequired,
  refName: PropTypes.string.isRequired,
  row: PropTypes.object.isRequired,
  onEdit: PropTypes.func.isRequired,
  onSave: PropTypes.func.isRequired,
  onCancel: PropTypes.func.isRequired
};

class EditableRow extends Component {
  constructor(props) {
    super(props);
    const { row } = this.props;
    this.state = { value: row.Value, id: row.Id };
  }

  render() {
    const { policy, allowEdit, isEditing, row } = this.props;

    return (
      <tr>
        <td className='center'>{row.Id}</td>
        {this.renderEditablecell()}
        <ActionCell
          policy={policy}
          isEditing={isEditing}
          disabled={!allowEdit}
          onSave={() => this.onSave()}
          onCancel={() => this.onCancel()}
          onEdit={() => this.onEdit()}
        />
      </tr>
    );
  }

  renderEditablecell() {
    const { isEditing, refName } = this.props;
    const { value } = this.state;

    return (
      <EditableCell
        value={value}
        refName={refName}
        isEditing={isEditing}
        onChange={evt => this.onChangeHandle(evt)}
        onKeyPress={evt => this.onKeyPressHandle(evt)}
      />
    );
  }

  onChangeHandle(evt) {
    this.setState({ value: evt.target.value });
  }

  onKeyPressHandle(evt) {
    if (evt.key === 'Enter') {
      this.onSave();
    }
  }

  onEdit() {
    const { index, onEdit } = this.props;
    onEdit(index);
  }

  onSave() {
    const { onSave } = this.props;
    const { id, value } = this.state;
    const newRow = {
      Id: id,
      Value: value
    };
    onSave(newRow);
  }

  onCancel() {
    const { onCancel, row } = this.props;
    this.setState({ value: row.Value });
    onCancel();
  }
}

EditableRow.propTypes = propTypes;
export default EditableRow;
