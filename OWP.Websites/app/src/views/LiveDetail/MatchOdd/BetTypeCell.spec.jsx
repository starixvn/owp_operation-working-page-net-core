import BetTypeCell from './BetTypeCell';
import { fireEvent, render, screen } from '@testing-library/react';

describe('BetTypeCell', () => {
  const onChange = jest.fn();

  const defaultProps = {
    betType: 1,
    onChange,
    betTypeOptions: [1, 3, 5, 7, 8, 15]
  };

  describe('policy is isCreating is true', () => {
    const props = {
      isCreating: true,
      ...defaultProps
    };

    it('should match snapshot', () => {
      const component = render(
        <table>
          <tbody>
            <tr>
              <BetTypeCell {...props} />
            </tr>
          </tbody>
        </table>
      );
      expect(component.container.firstChild).toMatchSnapshot();
    });

    it('should trigger onChange when select another bettype', () => {
      onChange.mockClear();
      const { container, getByText } = render(
        <table>
          <tbody>
            <tr>
              <BetTypeCell {...props} />
            </tr>
          </tbody>
        </table>
      );

      fireEvent.focus(container.querySelector('input'));
      fireEvent.keyDown(container.querySelector('input'), {
        key: 'ArrowDown',
        code: 40
      });

      fireEvent.click(getByText('Over/Under'));

      expect(getByText('Over/Under')).toBeInTheDocument();
      expect(onChange).toBeCalledWith(3);
    });
  });

  describe("policy's isCreating is false", () => {
    const props = {
      isCreating: false,
      ...defaultProps
    };

    it('should match snapshot', () => {
      const { container } = render(
        <table>
          <tbody>
            <tr>
              <BetTypeCell {...props} />
            </tr>
          </tbody>
        </table>
      );
      expect(container.firstChild).toMatchSnapshot();
    });
  });
});
