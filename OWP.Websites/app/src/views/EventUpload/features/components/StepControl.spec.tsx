import { fireEvent, render } from '@testing-library/react';
import { useDispatch } from 'react-redux';
import StepControl from './StepControl';

jest.mock('react-redux', () => ({
  useDispatch: jest.fn(),
}));
const mockDispatch = jest.fn();
(useDispatch as jest.Mock).mockReturnValue(mockDispatch);

describe('StepControl under test', () => {
  it('should render wizard-steps class', () => {
    const { container } = render(<StepControl currentStep={1} />);
    const element = container.querySelector('.wizard-steps');
    expect(element).toBeInTheDocument();
  });

  it('click get event text', () => {
    const { container, getByText } = render(<StepControl currentStep={1} />);
    container.querySelector('.wizard-steps');

    fireEvent.click(getByText('ASSIGN & UPLOAD'));
    expect(mockDispatch).toBeCalled();
  });

  it('click pearl-icon icon', () => {
    const { container, getByText } = render(<StepControl currentStep={1} />);
    container.querySelector('.wizard-steps');

    fireEvent.click(container.querySelector('.pearl-icon')!);
    expect(mockDispatch).toBeCalled();
  });
});