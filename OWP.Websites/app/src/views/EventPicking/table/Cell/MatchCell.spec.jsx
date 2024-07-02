import MatchCell from './MatchCell';
import { render, screen } from '@testing-library/react';

const props = {
  dataSource: [
    {
      homeName: 'Man City',
      rowColor: 'red'
    }
  ],
  rowIndex: 0
};

describe('MatchCell under test', () => {
  it('render cell', () => {
    render(<MatchCell {...props} />);
    const text = screen.getAllByText('Man City');
    expect(text).toBeTruthy();
  });
});
