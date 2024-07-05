import AssignmentUpload from './AssignmentUpload';
import { render, fireEvent } from '@testing-library/react';
import '@testing-library/jest-dom';

const mockSetState = jest.fn();

jest.mock('react', () => ({
  ...jest.requireActual('react'),
  useState: initial => [initial, mockSetState]
}));

describe('AssignmentUpload under test', () => {
  beforeEach(() => {
    jest.clearAllMocks();
  });

  it('should not display wizard control', async () => {
    const { container } = render(<AssignmentUpload />);
    const wizardControl = container.querySelector('.wizard-container');

    expect(wizardControl).toBeNull();
  });

  it('should setState when click button', async () => {
    const { container } = render(<AssignmentUpload />);
    const button = container.querySelector('.button-upload-other-sport');

    fireEvent.click(button);

    expect(mockSetState).toHaveBeenCalledWith(true);
  });
});
