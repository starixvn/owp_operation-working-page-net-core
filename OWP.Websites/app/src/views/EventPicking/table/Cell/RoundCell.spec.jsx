import RoundCell from './RoundCell';
import { render, screen } from '@testing-library/react';

const props = {
  dataSource: [
    {
      round: 'abc',
      rowColor: 'red'
    }
  ],
  rowIndex: 0
};

describe('RoundCell under test', () => {
  it('render cell', () => {
    render(<RoundCell {...props} />);
    const text = screen.getAllByText('abc');
    expect(text).toBeTruthy();
  });
});
