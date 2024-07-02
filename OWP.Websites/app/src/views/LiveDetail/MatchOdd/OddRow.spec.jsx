import OddRow from './OddRow';
import { MajorBetTypes } from '../../Shared/Constants';
import { fireEvent, render, screen } from '@testing-library/react';

describe('OddRow', () => {
  const onEdit = jest.fn();
  const onDelete = jest.fn();
  const onSave = jest.fn();
  const onCancel = jest.fn();
  const onRowChange = jest.fn();
  const defaultProps = {
    policy: {},
    index: 1,
    isCreating: true,
    isEditing: true,
    allowEdit: true,
    home: 'Home',
    away: 'Away',
    teamChoice: 1,
    row: {
      TeamChoice: 1
    },
    onEdit,
    onDelete,
    onSave,
    onCancel,
    onRowChange,
    HasWrite: true
  };

  describe('test BetTypeCell', () => {
    const row = {
      BetType: MajorBetTypes.Handicap,
      TeamChoice: 1
    };
    const props = {
      ...defaultProps,
      betTypeOptions: [1, 3, 5, 7, 8, 15],
      row
    };

    it('should update state when change bettype', () => {
      const { container, getByText } = render(
        <table>
          <tbody>
            <OddRow {...props} />
          </tbody>
        </table>
      );
      fireEvent.focus(container.querySelector('input'));
      fireEvent.keyDown(container.querySelector('input'), {
        key: 'ArrowDown',
        code: 40
      });

      fireEvent.click(getByText('1H 1X2'));

      expect(getByText('1H 1X2')).toBeInTheDocument();
    });
  });

  describe('test GoalCell', () => {
    it('should update state when change handicap value', () => {
      const row = {
        BetType: MajorBetTypes.Handicap,
        HDP: 1.23,
        TeamChoice: 1
      };
      const props = {
        ...defaultProps,
        betTypeOptions: [],
        row
      };
      const { container } = render(
        <table>
          <tbody>
            <OddRow {...props} />
          </tbody>
        </table>
      );

      const inputGoal = container.querySelector('.goal');

      fireEvent.change(inputGoal, {
        target: {
          value: 2.23
        }
      });

      expect(inputGoal.value).toEqual('2.23');
    });
  });

  describe('test OddCell at column Home', () => {
    const row = {
      BetType: MajorBetTypes.Handicap,
      HomeOdd: 1.23,
      TeamChoice: 1
    };
    const props = {
      betTypeOptions: [
        { 1: 'Handicap' },
        { 3: 'Over/Under' },
        { 5: 'FT.1X2' },
        { 7: '1H Handicap' },
        { 8: '1H Over/Under' },
        { 15: '1H 1X2' }
      ],
      ...defaultProps,
      row
    };

    it('should update state when change odds value', () => {
      const { container } = render(
        <table>
          <tbody>
            <OddRow {...props} />
          </tbody>
        </table>
      );

      const odd = container.querySelectorAll('.input-invalid');
      fireEvent.change(odd[1], {
        target: {
          value: '4.6'
        }
      });
      expect(odd[1].value).toEqual('4.6');
    });
  });

  describe('test OddCell at column Away', () => {
    const row = {
      BetType: MajorBetTypes.Handicap,
      AwayOdd: 1.23,
      TeamChoice: 1
    };
    const props = {
      betTypeOptions: [
        { 1: 'Handicap' },
        { 3: 'Over/Under' },
        { 5: 'FT.1X2' },
        { 7: '1H Handicap' },
        { 8: '1H Over/Under' },
        { 15: '1H 1X2' }
      ],
      ...defaultProps,
      row
    };

    it('should update state when change odds value', () => {
      const { container } = render(
        <table>
          <tbody>
            <OddRow {...props} />
          </tbody>
        </table>
      );
      const odd = container.querySelectorAll('.input-field')[2];
      expect(odd.value).toEqual('1.23');
    });
  });

  describe('test OddCell at column Draw', () => {
    it('should update state when change odds value', () => {
      const row = {
        BetType: MajorBetTypes.OneXTwo,
        DrawOdd: 1.23,
        TeamChoice: 1
      };
      const props = {
        betTypeOptions: [
          { 1: 'Handicap' },
          { 3: 'Over/Under' },
          { 5: 'FT.1X2' },
          { 7: '1H Handicap' },
          { 8: '1H Over/Under' },
          { 15: '1H 1X2' }
        ],
        ...defaultProps,
        row
      };
      const { container } = render(
        <table>
          <tbody>
            <OddRow {...props} />
          </tbody>
        </table>
      );

      const odd = container.querySelectorAll('.input-field')[2];

      fireEvent.change(odd, {
        target: {
          value: '123'
        }
      });

      expect(odd.value).toEqual('123');
    });
  });
});
