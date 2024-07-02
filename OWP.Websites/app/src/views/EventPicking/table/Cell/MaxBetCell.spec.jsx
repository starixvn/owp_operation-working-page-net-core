import MaxBetCell from './MaxBetCell';
import { render, screen } from '@testing-library/react';

const props = {
  dataSource: [
    {
      maxbetHdp: '10K',
      maxBetOU: '10K',
      rowColor: 'red'
    }
  ],
  rowIndex: 0
};

describe('MaxBetCell under test', () => {
  it('render cell', () => {
    render(<MaxBetCell {...props} />);
    const text = screen.getAllByText('10K');
    expect(text).toBeTruthy();
  });
});
