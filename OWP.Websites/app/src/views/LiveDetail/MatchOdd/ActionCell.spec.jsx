import ActionCell from './ActionCell';
import { fireEvent, render, screen } from '@testing-library/react';

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

  describe("policy's hasWrite is false", () => {
    const policy = { HasWrite: false };
    const props = {
      policy,
      disabled: true,
      isEditing: true,
      ...defaultProps
    };

    it('should match snapshot', () => {
      const { container } = render(
        <table>
          <tbody>
            <tr>
              <ActionCell {...props} />
            </tr>
          </tbody>
        </table>
      );
      expect(container.firstChild).toMatchSnapshot();
    });
  });

  describe("policy's hasWrite is true", () => {
    const policy = { HasWrite: true };

    it('test isEditing=true & disabled=true to match snapshot', () => {
      const props = {
        policy,
        isEditing: true,
        disabled: false,
        ...defaultProps
      };
      const { container } = render(
        <table>
          <tbody>
            <tr>
              <ActionCell {...props} />
            </tr>
          </tbody>
        </table>
      );
      expect(container.firstChild).toMatchSnapshot();
    });

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
      expect(container.firstChild).toMatchSnapshot();

      fireEvent.click(container.querySelector('.icon-save'));
    });

    it('test shoyuld trigger onSave', () => {
      const props = {
        policy,
        isEditing: true,
        disabled: false,
        ...defaultProps
      };
      onSave.mockClear();
      const { container } = render(
        <table>
          <tbody>
            <tr>
              <ActionCell {...props} />
            </tr>
          </tbody>
        </table>
      );

      fireEvent.click(container.querySelector('.icon-save'));
      expect(onSave).toBeCalledTimes(1);
    });

    it('test shoyuld trigger onCancel', () => {
      const props = {
        policy,
        isEditing: true,
        disabled: false,
        ...defaultProps
      };
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

      fireEvent.click(container.querySelector('.icon-cancel'));
      expect(onCancel).toBeCalledTimes(1);
    });

    it('test isEditing=false & disabled=true to match snapshot', () => {
      const props = {
        policy,
        isEditing: false,
        disabled: true,
        ...defaultProps
      };
      const { container } = render(
        <table>
          <tbody>
            <tr>
              <ActionCell {...props} />
            </tr>
          </tbody>
        </table>
      );
      expect(container.firstChild).toMatchSnapshot();
    });

    it('test isEditing=false & disabled=false should call onEdit', () => {
      const props = {
        policy,
        isEditing: false,
        disabled: false,
        ...defaultProps
      };
      onEdit.mockClear();
      const { container } = render(
        <table>
          <tbody>
            <tr>
              <ActionCell {...props} />
            </tr>
          </tbody>
        </table>
      );

      fireEvent.click(container.querySelector('.icon-edit'));
      expect(onEdit).toBeCalledTimes(1);
    });

    it('test isEditing=false & disabled=false should call onDelete', () => {
      const props = {
        policy,
        isEditing: false,
        disabled: false,
        ...defaultProps
      };
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

      fireEvent.click(container.querySelector('.icon-delete'));
      expect(onDelete).toBeCalledTimes(1);
    });
  });
});
