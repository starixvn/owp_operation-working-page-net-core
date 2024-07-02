import { render, fireEvent } from '@testing-library/react';
import CheckboxCell from './CheckboxCell';

const onChangeChecked = jest.fn();
const defaultProps = {
  defaultCheck: true,
  onChangeChecked,
  id: '123',
  hasWrite: true
};

describe('CheckboxCell', () => {
  it('Should render checkbox checked', () => {
    const { container } = render(<CheckboxCell {...defaultProps} />);

    expect(container.querySelector('.form-check-input')).toBeInTheDocument();
  });

  it('Should disable checkbox when hasWrite is false', () => {
    const props = {
      ...defaultProps,
      hasWrite: false
    };
    const { container } = render(<CheckboxCell {...props} />);
    const checkbox = container.querySelector('.form-check-input');
    expect(checkbox.getAttribute('disabled')).toBe('');
  });

  it('onChangeChecked should to be call, when change check', () => {
    const { container } = render(<CheckboxCell {...defaultProps} />);
    const checkbox = container.querySelector('.form-check-input');
    fireEvent.click(checkbox, { target: { checked: false } });
    expect(onChangeChecked).toBeCalled();
  });
});
