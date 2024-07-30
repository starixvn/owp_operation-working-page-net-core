import { fireEvent, render } from '@testing-library/react';
import { createRef, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import EventUploadContent from './EventUploadContent';

jest.mock('react-redux', () => ({
  useSelector: jest.fn(),
  useDispatch: jest.fn(),
  useEffect: jest.fn()
}));

const mockDispatch = jest.fn();

const setup = () => {
  const state = {
    uploadReducer: {
      currentStep: 1,
      request: {
        isUploadFMAndAIO: false,
        eventDate: 'Nov 11, 2022'
      }
    }
  };
  (useDispatch as jest.Mock).mockReturnValue(mockDispatch);
  (useSelector as jest.Mock).mockImplementation(selectorFn => selectorFn({ ...state }));
};

describe('EventUploadContent under test', () => {
  beforeEach(() => {
    setup();
  })
  afterEach(() => {
    jest.clearAllMocks();
  });

  it('should render wizard-steps', () => {
    const { container } = render(<EventUploadContent />);
    const element = container.querySelector('.wizard-steps');
    expect(element).toBeInTheDocument();
    expect(mockDispatch).toBeCalled();
  });
});
