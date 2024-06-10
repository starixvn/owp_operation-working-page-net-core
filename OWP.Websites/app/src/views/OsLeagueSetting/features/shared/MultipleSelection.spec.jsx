import { render, screen, fireEvent, act } from "@testing-library/react";
import MultipleSelection from './MultipleSelection';

const defaultOptions = [{
  value: 1, label: 'Option 1'
}, {
  value: 2, label: 'Option 2'
}, {
  value: 3, label: 'Option 3'
}];

const defaultProp = {
  label: 'Category',
  options: defaultOptions,
  value: defaultOptions,
  hasSelectAll: true,
  labelledBy: 'Select...',
  disableSearch: true,
  onChange: jest.fn()
};

describe('MultipleSelection under test', () => {
  it('should display "All items are selected."', () => {
    const { getByText } = render(<MultipleSelection {...defaultProp} />);
    expect(getByText('Option 1, Option 2, Option 3')).toBeInTheDocument();
  });

  it('should call onChange when change selected item', async () => {
    const props = {
      ...defaultProp,
      value: []
    }
    const { container, getByText } = render(<MultipleSelection {...props} />);
    fireEvent.click(container.querySelector('.dropdown-heading-dropdown-arrow'));
    fireEvent.click(getByText('Option 1'));
    await act(async () => await Promise.resolve());
    expect(defaultProp.onChange).toBeCalledWith(
      [
        { "label": "Option 1", "value": 1 }
      ]
    );
  })
});