import LeagueCell from './LeagueCell';
import { render, screen } from '@testing-library/react';

const props = {
  dataSource: [
    {
      leagueName: 'EPL',
      rowColor: 'red'
    }
  ],
  rowIndex: 0
};

describe('LeagueCell under test', () => {
  it('render cell', () => {
    render(<LeagueCell {...props} />);
    const text = screen.getAllByText('EPL');
    expect(text).toBeTruthy();
  });
});
