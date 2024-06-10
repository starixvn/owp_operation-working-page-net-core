import { fireEvent, render, screen } from '@testing-library/react';
import EditableRow from './EditableRow';
import { Table } from 'reactstrap';

const onEdit = jest.fn();
const onSave = jest.fn();
const onCancel = jest.fn();
const onKeyPress = jest.fn();

const defaultProps = {
  policy: {},
  index: 1,
  isCreating: true,
  isEditing: true,
  allowEdit: true,
  value: 'value',
  row: {},
  onEdit,
  onSave,
  onCancel,
  onKeyPress,
  refName: 'refEditableCell'
};

const row = {
  Id: '1',
  Value: 'Value'
};
const props = {
  ...defaultProps,
  row
};

describe('EditableRow', () => {
  it('should render center class', () => {
    const { container } = render(
      <Table>
        <tbody>
          <EditableRow {...props} />
        </tbody>
      </Table>
    );
    expect(container.querySelector('.center')).toBeInTheDocument();
  });

  describe('test Editable', () => {
    it('should update state when change text', () => {
      const { container } = render(
        <Table>
          <tbody>
            <EditableRow {...props} />
          </tbody>
        </Table>
      );
      const editableCell = container.querySelector('.input-field');
      fireEvent.change(editableCell, { target: { value: '123' } });
      expect(screen.getByDisplayValue('123')).toBeInTheDocument();
    });

    it('should trigger save when keyPress Enter textbox', () => {
      const { container } = render(
        <Table>
          <tbody>
            <EditableRow {...props} />
          </tbody>
        </Table>
      );
      const editableCell = container.querySelector('.input-field');
      fireEvent.keyPress(editableCell, {
        key: 'Enter',
        code: 13,
        charCode: 13
      });
      expect(onSave).toBeCalledTimes(1);
    });

    it('should not trigger save when keyPress Enter textbox', () => {
      const { container } = render(
        <Table>
          <tbody>
            <EditableRow {...props} />
          </tbody>
        </Table>
      );
      const editableCell = container.querySelector('.input-field');
      fireEvent.keyPress(editableCell, {
        key: 'Anything',
        code: 13,
        charCode: 13
      });
      expect(onSave).toBeCalledTimes(1);
    });
  });

  describe('test ActionCell', () => {
    it('should trigger onEdit when click button Edit', () => {
      onEdit.mockClear();
      const propsEdit = {
        policy: {},
        index: 1,
        isCreating: true,
        isEditing: false,
        allowEdit: true,
        value: 'value',
        row: {},
        onEdit,
        onSave,
        onCancel,
        onKeyPress,
        refName: 'refEditableCell',
        row
      };
      const { container } = render(
        <Table>
          <tbody>
            <EditableRow {...propsEdit} />
          </tbody>
        </Table>
      );
      const actionCell = container.querySelector('.icon-edit');
      fireEvent.click(actionCell);
      expect(onEdit).toBeCalledTimes(1);
    });

    it('should trigger onCancel when click button Cancel', () => {
      onCancel.mockClear();
      const { container } = render(
        <Table>
          <tbody>
            <EditableRow {...props} />
          </tbody>
        </Table>
      );
      const actionCell = container.querySelector('.icon-cancel');
      fireEvent.click(actionCell);
      expect(screen.getByDisplayValue('Value')).toBeInTheDocument;
      expect(onCancel).toBeCalledTimes(1);
    });

    it('should trigger onSave when click button Save on valid data', () => {
      onSave.mockClear();
      const { container } = render(
        <Table>
          <tbody>
            <EditableRow {...props} />
          </tbody>
        </Table>
      );
      const actionCell = container.querySelector('.icon-save');
      fireEvent.click(actionCell);
      expect(onSave).toBeCalledTimes(1);
    });
  });
});
