import StepControl from './StepControl';
import { render, fireEvent, act } from '@testing-library/react';
import '@testing-library/jest-dom';

describe('StepControl under test', () => {
  it('should match snapshot', () => {
    const { container } = render(<StepControl currentStep={2} />);
    expect(container).toMatchSnapshot();
  });

  it('should render active step upload asignment', () => {
    const { container } = render(<StepControl currentStep={2} />);
    const stepUploadIsActive = container.querySelectorAll('.active');
    expect(stepUploadIsActive.length).toEqual(3);
  });

  it('should render active step upload asignment & validation', () => {
    const { container } = render(<StepControl currentStep={1} />);
    const stepUploadIsActive = container.querySelectorAll('.active');
    expect(stepUploadIsActive.length).toEqual(2);
  });

  it('should render active step upload asignment & validation & review', () => {
    const { container } = render(<StepControl currentStep={2} />);
    const stepUploadIsActive = container.querySelectorAll('.active');
    expect(stepUploadIsActive.length).toEqual(3);
  });
});
