import IPTvStreamingCell from './IPTvStreamingCell';
import { render, screen } from '@testing-library/react';

const props = {
  dataSource: [
    {
      tv: 'acb',
      iptv: '123',
      channel: 'abc',
      wb: 'abc',
      rowColor: 'red'
    }
  ],
  rowIndex: 0
};

describe('IPTvStreamingCell under test', () => {
  it('render cell', () => {
    render(<IPTvStreamingCell {...props} />);
    const text = screen.getAllByText('123');
    expect(text).toBeTruthy();
  });
});
