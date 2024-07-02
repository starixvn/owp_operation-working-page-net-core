import { render, screen } from '@testing-library/react';
import SelectFilterFactory from './SelectFilterFactory';

const onSelectedChange = jest.fn();
const props = {
  option: [
    { value: 1, label: 'All' },
    { value: 2, label: 'Early' },
    { value: 3, label: 'Today' }
  ],
  onSelectedChange: onSelectedChange
};

describe('test SelectFilterFactory', () => {
  it('should render SelectFilterFactory', () => {
    render(<SelectFilterFactory {...props} />);
    expect(screen.queryByText('All')).toBeInTheDocument();
  });
});
