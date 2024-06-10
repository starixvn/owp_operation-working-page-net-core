import MatchSettingTable from './MatchSettingTable';
import { render } from '@testing-library/react';

describe('<MatchSettingTable/>', () => {
  const defaultProps = {
    policy: {
      HasWrite: true
    },
    isCreating: true,
    isEditing: true,
    editingIndex: 0,
    onEdit: jest.fn(),
    onDelete: jest.fn(),
    onSave: jest.fn(),
    onCancel: jest.fn(),
    minorBettypeId: 1,
    matchSettings: []
  };

  it('should render <MatchSettingTable/> component success', () => {
    const props = {
      ...defaultProps
    };
    const { container } = render(<MatchSettingTable {...props} />);
    expect(container.firstChild).toMatchSnapshot();
  });

  it('should render table with data', () => {
    const props = {
      ...defaultProps,
      policy: {
        HasWrite: true
      },
      matchSettings: [
        {
          BettypeId: 1,
          Maxbet: 3,
          Spread: 2,
        },
        {
          BettypeId: -1,
          Maxbet: 5,
          Spread: 6,
        }
      ]
    };
    const { container } = render(<MatchSettingTable {...props} />);
    expect(container.firstChild).toMatchSnapshot();
  });
});
