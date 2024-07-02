import { fireEvent, render } from '@testing-library/react';
import ActionCell from './ActionCell';

describe('ActionCell', () => {
  const onSave = jest.fn();
  const onCancel = jest.fn();
  const onEdit = jest.fn();
  const onDelete = jest.fn();
  const defaultProps = {
    onSave,
    onCancel,
    onEdit,
    onDelete
  };

  describe("policy's hasWrite is true", () => {
    const policy = { HasWrite: true };

    it('test isEditing=true & disabled=false to match snapshot', () => {
      const props = {
        policy,
        isEditing: true,
        disabled: false,
        ...defaultProps
      };
      onSave.mockClear();
      onCancel.mockClear();
      const { container } = render(
        <table>
          <tbody>
            <tr>
              <ActionCell {...props} />
            </tr>
          </tbody>
        </table>
      );
      expect(container).toMatchSnapshot();
      const saveButton = container.querySelector('.icon-save');
      fireEvent.click(saveButton);
      expect(onSave).toBeCalledTimes(1);
      const cancelButton = container.querySelector('.icon-cancel');
      fireEvent.click(cancelButton);
      expect(onCancel).toBeCalledTimes(1);
    });

    it('test isEditing=false & disabled=false to match snapshot', () => {
      const props = {
        policy,
        isEditing: false,
        disabled: false,
        ...defaultProps
      };
      onEdit.mockClear();
      onDelete.mockClear();
      const { container } = render(
        <table>
          <tbody>
            <tr>
              <ActionCell {...props} />
            </tr>
          </tbody>
        </table>
      );
      expect(container).toMatchSnapshot();
      const editButton = container.querySelector('.icon-edit');
      fireEvent.click(editButton);
      expect(onEdit).toBeCalledTimes(1);
    });
  });
});
