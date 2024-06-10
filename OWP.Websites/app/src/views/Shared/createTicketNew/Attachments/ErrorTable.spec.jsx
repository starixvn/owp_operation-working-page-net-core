import { render, screen } from '@testing-library/react';
import ErrorTable from './ErrorTable';

const props = {
  errors: ['KOtime invalid']
};

describe('ErrorTable under test', () => {
  it('should match snapshot', () => {
    const { container } = render(<ErrorTable {...props} />);
    expect(container).toMatchSnapshot();
  });

  it('should render table error', () => {
    render(<ErrorTable {...props} />);
    const span = screen.getByText('KOtime invalid');
    expect(span).toBeInTheDocument();
  });
});
