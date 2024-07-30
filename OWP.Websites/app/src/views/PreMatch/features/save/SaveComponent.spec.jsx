import { fireEvent, render, screen } from '@testing-library/react';
import SaveComponent from './SaveComponent';

const mockOnCancel = jest.fn();
const mockOnSave = jest.fn();
describe('SaveComponent under test', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });

  it('should render 2 btn cancel, save', () => {
    const { container } = render(
      <SaveComponent onCancel={mockOnCancel} onSave={mockOnSave} />
    );
    const element = container.querySelector('.button-cancel-save');
    expect(element).toBeInTheDocument();
  });
  it('should call back when click cancel', () => {
    const { container } = render(
      <SaveComponent onCancel={mockOnCancel} onSave={mockOnSave} />
    );
    const element = container.querySelector('.button-cancel');
    fireEvent.click(element);
    expect(mockOnCancel).toHaveBeenCalledTimes(1);
  });
  it('should call back when click save', () => {
    const { container } = render(
      <SaveComponent onCancel={mockOnCancel} onSave={mockOnSave} />
    );
    const element = container.querySelector('.button-submit');
    fireEvent.click(element);
    expect(mockOnSave).toHaveBeenCalledTimes(1);
  });
});
