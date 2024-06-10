import ToggleColumn from './ToggleColumn';
import Utils from '../Utils';
import { fireEvent, render, screen, waitFor } from '@testing-library/react';

jest.mock('../Utils');
const data = [
  { label: '1', value: 'One' },
  { label: '2', value: 'Two' },
  { label: '3', value: 'Three' }
];
const defaultProps = {
  options: data
};
const mockOnSelected = jest.fn();
const props = {
  ...defaultProps,
  onSelected: mockOnSelected
};

describe('ToggleColumn', () => {
  it('should show `All items are selected` when selected all item', () => {
    render(<ToggleColumn {...props} />);
    expect(screen.getByText(/All items are selected/)).toBeInTheDocument();
  });
});
