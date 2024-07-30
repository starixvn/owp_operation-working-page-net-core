import { render, fireEvent } from '@testing-library/react';
import DropdownDefaultNative from './DropdownDefaultNative';

const onSelectionChanged = jest.fn();

const defaultProps = {
  onSelectionChanged,
  selected: 2,
  options: [
    { value: 1, label: 'A' },
    { value: 2, label: 'B' }
  ],
  disabled: false
};

describe('DropdownDefaultNative under test', () => {
  it('match snapshot', () => {
    const { container } = render(<DropdownDefaultNative {...defaultProps} />);
    expect(container).toMatchSnapshot();
  });

  it('should call onSelectionChanged when change item is selected', () => {
    const { container } = render(<DropdownDefaultNative {...defaultProps} />);

    const dropDown = container.querySelector('#dropdown');
    fireEvent.change(dropDown, {
      target: { value: 1, selectedIndex: 0 }
    });
    expect(onSelectionChanged).toBeCalled();
  });
});
