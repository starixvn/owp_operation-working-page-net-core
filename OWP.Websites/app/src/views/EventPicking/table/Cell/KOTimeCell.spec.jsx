import KOTimeCell from './KOTimeCell';
import { render, screen } from '@testing-library/react';

const props = {
  dataSource: [
    {
      eventDateTime: '2022-11-07T20:00:00',
      rowColor: 'red'
    }
  ],
  rowIndex: 0
};

describe('KOTimeCell under test', () => {
  it('render cell', () => {
    render(<KOTimeCell {...props} />);
    const text = screen.getAllByText('08:00 PM');
    expect(text).toBeTruthy();
  });
});
