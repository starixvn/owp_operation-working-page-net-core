import ProviderCell from './ProviderCell';
import { render, screen } from '@testing-library/react';

const props = {
  dataSource: [
    {
      otherProvider: 'test',
      agent: 'abc',
      rowColor: 'red'
    }
  ],
  rowIndex: 0
};

describe('ProviderCell under test', () => {
  it('render cell', () => {
    render(<ProviderCell {...props} />);
    const text = screen.getAllByText('test');
    expect(text).toBeTruthy();
  });
});
