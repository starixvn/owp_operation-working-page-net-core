import { Component } from 'react';
import PropTypes from 'prop-types';
import { isEqual, find } from 'lodash';
import PlusIcon from 'mdi-react/PlusIcon';

import { Button, Table, Input } from 'reactstrap';
import Service from './Service';
import EditableRow from './EditableRow';
import Swal from 'sweetalert2';
import withReactContent from 'sweetalert2-react-content';
const Toastr = withReactContent(Swal);
const duplicateMessage = 'Value is duplicated';

const propTypes = {
  policy: PropTypes.object.isRequired,
  refName: PropTypes.string.isRequired,
  dataSource: PropTypes.array.isRequired
};

class ResourceTable extends Component {
  constructor(props) {
    super(props);
    this.state = {
      isLoading: true,
      isCreating: false,
      isEditing: false,
      editingIndex: 0,
      valueText: '',
      rows: this.props.dataSource
    };
  }

  static getDerivedStateFromProps(nextProps, prevState) {
    const { isLoading, isCreating, isEditing, editingIndex, valueText } =
      prevState;
    if (nextProps.dataSource !== prevState.dataSource) {
      return {
        isLoading,
        isCreating,
        isEditing,
        editingIndex,
        valueText,
        rows: nextProps.dataSource
      };
    }
    return null;
  }

  onEdit(index) {
    this.setState({
      isEditing: true,
      editingIndex: index
    });
  }

  renderRows(rows) {
    const { policy, refName } = this.props;
    const { isCreating, isEditing, editingIndex } = this.state;
    return rows.map((row, index) => {
      const { Value } = row;
      const isRenderingEditingRow = index === editingIndex;
      const key =
        isCreating && isRenderingEditingRow ? 'new' : `${Value}-${index}`;
      return (
        <EditableRow
          key={key}
          policy={policy}
          index={index}
          isCreating={isCreating && isRenderingEditingRow}
          isEditing={isEditing && isRenderingEditingRow}
          allowEdit={!isEditing || isRenderingEditingRow}
          value={Value}
          refName={refName}
          row={row}
          onEdit={event => this.onEdit(event)}
          onSave={event => this.onSave(event)}
          onCancel={() => this.onCancel()}
          onKeyPress={event => this.onSave(event)}
        />
      );
    });
  }

  renderBody() {
    const { rows } = this.state;
    return (
      <tbody>
        {rows.length === 0 && (
          <tr>
            <td colSpan='3'>There is no data</td>
          </tr>
        )}
        {rows.length > 0 && this.renderRows(rows)}
      </tbody>
    );
  }

  onSave(newRow) {
    const { rows, editingIndex } = this.state;
    if (newRow.Value) {
      const row = rows[editingIndex];
      (async () => {
        if (isEqual(row.Value, newRow.Value)) {
          this.setState({
            isCreating: false,
            isEditing: false
          });
        } else {
          const findExists = find(
            rows,
            ele =>
              ele.Value.trim().toLowerCase() ===
                newRow.Value.trim().toLowerCase() &&
              ele.Value.trim().toLowerCase() !== row.Value.trim().toLowerCase()
          );
          if (findExists) {
            Toastr.fire({
              icon: 'error',
              html: duplicateMessage,
              showCloseButton: true
            });
            return;
          }
          const model = {
            Id: row.Id,
            Value: newRow.Value.trim(),
            RefName: this.props.refName
          };
          const response = await Service.update(model);
          newRow.Id = response.Id;
          rows[editingIndex] = newRow;
          this.setState({
            isCreating: false,
            isEditing: false,
            rows
          });
        }
      })();
    }
  }

  onCancel() {
    let { rows } = this.state;
    rows = rows.slice();
    this.setState({
      isCreating: false,
      isEditing: false,
      rows
    });
  }

  inputValueChange(evt) {
    this.setState({
      valueText: evt.target.value
    });
  }

  inputValueKeyPress(evt) {
    if (evt.key === 'Enter') {
      this.addNew();
    }
  }

  async addNew() {
    const { valueText, rows } = this.state;
    if (valueText) {
      const findExists = find(
        rows,
        row => row.Value.toLowerCase() === valueText.trim().toLowerCase()
      );
      if (findExists) {
        Toastr.fire({
          icon: 'error',
          html: duplicateMessage,
          showCloseButton: true
        });
        return;
      }

      const model = {
        Id: 0,
        Value: valueText.trim(),
        RefName: this.props.refName
      };
      const response = await Service.create(model);
      model.Id = response.Id;
      rows.push(model);
      this.setState({
        isCreating: false,
        isEditing: false,
        rows
      });
    }
  }

  render() {
    const { policy, refName } = this.props;
    const { isCreating, isEditing } = this.state;
    return (
      <div className='option-table'>
        {policy.HasWrite && (
          <div className='add-value'>
            <Input
              className='value-textinput'
              type='text'
              placeholder='Add a value'
              disabled={isCreating || isEditing}
              maxLength={Service.getMaxLength(refName)}
              onChange={evt => this.inputValueChange(evt)}
              onKeyPress={evt => this.inputValueKeyPress(evt)}
            />
            <Button
              className='btn-add btn btn-secondary'
              onClick={evt => this.addNew(evt)}
              disabled={isCreating || isEditing}>
              <PlusIcon size={15} />
              <span>Add</span>
            </Button>
          </div>
        )}
        <Table bordered>
          <thead>
            <tr>
              <th className='id-col'>ID</th>
              <th>Value</th>
              <th className='action-col'>Action</th>
            </tr>
          </thead>
          {this.renderBody()}
        </Table>
      </div>
    );
  }
}

ResourceTable.propTypes = propTypes;
export default ResourceTable;
