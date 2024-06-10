import { fireEvent, render } from '@testing-library/react';
import EditableCell from './EditableCell';
import { Table } from 'reactstrap';

describe('EditableCell', () => {
  const onChange = jest.fn();
  const onKeyPress = jest.fn();
  describe('isEditing is true', () => {
    const isEditing = true;

    it('should render "content" class when value is not null', () => {
      const props = {
        value: '123',
        isEditing,
        onChange,
        refName: 'refEditableCell'
      };
      const { container } = render(
        <Table>
          <tbody>
            <tr>
              <EditableCell {...props} />
            </tr>
          </tbody>
        </Table>
      );
      expect(container.querySelector('.content')).toBeInTheDocument();
    });

    it("should trigger onChange when change value's value", () => {
      onChange.mockClear();
      const props = {
        value: '123',
        isEditing,
        onChange,
        refName: 'refEditableCell'
      };
      const { container } = render(
        <Table>
          <tbody>
            <tr>
              <EditableCell {...props} />
            </tr>
          </tbody>
        </Table>
      );
      const textbox = container.querySelector('.input-field');
      fireEvent.change(textbox, { target: { value: 'abc' } });
      expect(onChange).toBeCalled();
    });

    it("should trigger onKeyPress Enter when change value's value", () => {
      onKeyPress.mockClear();
      const props = {
        value: '123',
        isEditing,
        onChange,
        onKeyPress,
        refName: 'refEditableCell'
      };
      const { container } = render(
        <Table>
          <tbody>
            <tr>
              <EditableCell {...props} />
            </tr>
          </tbody>
        </Table>
      );
      const textbox = container.querySelector('.input-field');
      fireEvent.keyPress(textbox, { key: 'Enter', code: 13, charCode: 13 });
      expect(onKeyPress).toBeCalled();
    });
  });

  describe('isEditing is false', () => {
    it('should not render "content" when isEditing false', () => {
      const props = {
        value: '123',
        isEditing: false,
        onChange,
        refName: 'refEditableCell'
      };
      const { container } = render(
        <Table>
          <tbody>
            <tr>
              <EditableCell {...props} />
            </tr>
          </tbody>
        </Table>
      );
      expect(container.querySelector('.content')).toBeNull();
    });
  });
});
