import OddCell from './OddCell';
import { fireEvent, render, screen } from '@testing-library/react';

describe('OddCell', () => {
  const onChange = jest.fn();

  describe('isEditing is true', () => {
    const isEditing = true;

    it('should match snapshot when odd is not null', () => {
      const props = {
        odd: 9,
        isEditing,
        onChange
      };
      const { container } = render(<table><tbody><tr>
        <OddCell {...props} /></tr></tbody></table>);
      expect(container.firstChild).toMatchSnapshot();
    });

    it('should match snapshot when odd is null', () => {
      const props = {
        odd: null,
        isEditing,
        onChange
      };
      const { container } = render(<table><tbody><tr>
        <OddCell {...props} /></tr></tbody></table>);
      expect(container.firstChild).toMatchSnapshot();
    });

    it("should trigger onChange when change odd's value", () => {
      onChange.mockClear();
      const props = {
        odd: -1.23,
        isEditing,
        onChange
      };
      const component = render(<table><tbody><tr>
        <OddCell {...props} /></tr></tbody></table>);

      fireEvent.change(component.container.querySelector('.input-field'), {
        target: {
          value: '123'
        }
      });

      expect(onChange).toBeCalledWith(123);
    });
  });

  describe('isEditing is false', () => {
    const isEditing = false;
    it('should match snapshot when odd is positive', () => {
      const props = {
        odd: 1.23,
        isEditing,
        onChange
      };
      const component = render(<table><tbody><tr>
        <OddCell {...props} /></tr></tbody></table>);

      expect(component.container.firstChild).toMatchSnapshot();
    });

    it('should match snapshot when odd is negative', () => {
      const props = {
        odd: -1.23,
        isEditing,
        onChange
      };
      const component = render(<table><tbody><tr>
        <OddCell {...props} /></tr></tbody></table>);

      expect(component.container.firstChild).toMatchSnapshot();
    });
  });
});
