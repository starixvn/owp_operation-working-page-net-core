import { render, screen } from '@testing-library/react';
import ReadyCell from './ReadyCell';

describe('ReadyCell under testing', () => {
  it('should render cell', () => {
    const props = {
      rowIndex: 0,
      readyDateTimeDisplay: '4:00 PM'
    };
    const { container } = render(<ReadyCell {...props} />);
    const icon = container.querySelectorAll('.icon-success');
    expect(icon).toBeTruthy();
  });
});
