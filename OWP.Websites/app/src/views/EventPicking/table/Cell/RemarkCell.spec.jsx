import RemarkCell from './RemarkCell';
import { render, screen } from '@testing-library/react';

const props = {
  dataSource: [
    {
      remark: 'test',
      homeRemark: 'abc',
      rowColor: 'red'
    }
  ],
  rowIndex: 0
};

describe('RemarkCell under test', () => {
  it('render cell', () => {
    render(<RemarkCell {...props} />);
    const text = screen.getAllByText('abc');
    expect(text).toBeTruthy();
  });
});
