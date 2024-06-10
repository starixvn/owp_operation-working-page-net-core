import GoalCell from './GoalCell';
import { MajorBetTypes } from '../../Shared/Constants';
import { fireEvent, render, screen } from '@testing-library/react';

describe('GoalCell', () => {
  const onChange = jest.fn();
  const defaultProps = {
    handicap: 1,
    teamChoice: 1,
    home: 'Home',
    away: 'Away',
    onChange
  };

  describe("policy's isEditing is true", () => {
    const isEditing = true;

    it('should match snapshot with bettype Handicap', () => {
      const props = {
        betType: MajorBetTypes.Handicap,
        isEditing,
        ...defaultProps
      };
      const { container } = render(
        <table>
          <tbody>
            <tr>
              <GoalCell {...props} />
            </tr>
          </tbody>
        </table>
      );
      expect(container.firstChild).toMatchSnapshot();
    });

    it('should match snapshot with bettype OverUnder', () => {
      const props = {
        betType: MajorBetTypes.OverUnder,
        isEditing,
        ...defaultProps
      };
      const { container } = render(
        <table>
          <tbody>
            <tr>
              <GoalCell {...props} />
            </tr>
          </tbody>
        </table>
      );
      expect(container.firstChild).toMatchSnapshot();
    });

    it('should trigger onChange when change handicap value', () => {
      onChange.mockClear();
      const props = {
        betType: MajorBetTypes.Handicap,
        isEditing,
        ...defaultProps
      };
      const { container } = render(
        <table>
          <tbody>
            <tr>
              <GoalCell {...props} />
            </tr>
          </tbody>
        </table>
      );

      fireEvent.change(container.querySelector('.input-field'), {
        target: {
          value: 123
        }
      });

      expect(onChange).toBeCalled();
    });
  });

  describe("policy's isEditing is false", () => {
    const isEditing = false;

    it('should match snapshot by betType', () => {
      const betTypes = [MajorBetTypes.Handicap, MajorBetTypes.OverUnder];
      betTypes.forEach(betType => {
        const props = {
          betType: betType,
          isEditing,
          ...defaultProps
        };
        const component = render(
          <table>
            <tbody>
              <tr>
                <GoalCell {...props} />
              </tr>
            </tbody>
          </table>
        );
        expect(component.container.firstChild).toMatchSnapshot();
      });
    });

    it('should match snapshot by teamChoice is away and negative handicap', () => {
      const props = {
        ...defaultProps,
        betType: MajorBetTypes.Handicap,
        teamChoice: 2,
        handicap: -1.23,
        isEditing
      };
      const component = render(
        <table>
          <tbody>
            <tr>
              <GoalCell {...props} />
            </tr>
          </tbody>
        </table>
      );
      expect(component.container.firstChild).toMatchSnapshot();
    });
  });
});
