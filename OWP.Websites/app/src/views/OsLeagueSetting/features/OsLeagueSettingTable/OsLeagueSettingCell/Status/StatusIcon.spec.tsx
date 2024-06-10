import { render, screen } from '@testing-library/react';
import StatusIcon from './StatusIcon';

describe('ActiveCell under test', () => {
  it('Should render not checked', () => {
    const { container } = render(<StatusIcon isActive={false} />);
    const input = container.querySelector('svg');
    expect(input).toBeInTheDocument();
  });
  it('Should render checked', () => {
    const { container } = render(<StatusIcon isActive={true} />);
    const input = container.querySelector('svg');
    expect(input).toBeInTheDocument();
  });
});
