import FileLoader from './FileLoader';
import { useSelector, useDispatch } from 'react-redux';
import ToastrWrapper from '../../components/ToastrWrapper';
import { render, fireEvent } from '@testing-library/react';

import '@testing-library/jest-dom';
jest.mock('../../components/ToastrWrapper');
ToastrWrapper.showError = jest.fn().mockReturnValue('OK');
jest.mock('react-redux', () => ({
  useSelector: jest.fn(),
  useDispatch: jest.fn()
}));
jest.mock('react', () => ({
  useState: jest.fn()
}));

const mockedDispatch = jest.fn();
useSelector.mockImplementation(selectorFn =>
  selectorFn({
    fileLoader: {
      validationResult: {
        hasError: false
      }
    }
  })
);
useDispatch.mockReturnValue(mockedDispatch);

const defaultProps = {
  onFileChange: jest.fn()
};

const mockSetState = jest.fn();
jest.mock('react', () => ({
  ...jest.requireActual('react'),
  useState: initial => [initial, mockSetState]
}));

describe('FileLoader under test', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });
  it('should render correct', () => {
    const { container } = render(<FileLoader {...defaultProps} />);
    expect(container.querySelector('.control-field')).toMatchSnapshot();
  });
  it('should call onFileChange', () => {
    const { container } = render(<FileLoader {...defaultProps} />);
    const input = container.querySelector('#myFile');
    fireEvent.change(input);

    expect(defaultProps.onFileChange).toHaveBeenCalled();
  });

  it('should call handleFileChange', async () => {
    const { container } = render(<FileLoader {...defaultProps} />);
    const input = container.querySelector('#myFile');
    const file = new File([new ArrayBuffer(6000000)], 'file.xlsx');

    fireEvent.change(input, { target: { files: [file] } });

    expect(defaultProps.onFileChange).toHaveBeenCalledTimes(2);
  });

  it('should call setState when file changes', async () => {
    const { container } = render(<FileLoader {...defaultProps} />);
    const input = container.querySelector('#myFile');
    const file = new File([new ArrayBuffer(6000000)], 'file.xlsx');

    fireEvent.change(input, { target: { files: [file] } });

    expect(mockSetState).toHaveBeenCalledWith('file.xlsx');
  });

  it('should show error when has error', async () => {
    jest.clearAllMocks();
    useSelector.mockImplementation(selectorFn =>
      selectorFn({
        fileLoader: {
          validationResult: {
            hasError: true,
            message: 'No file selected'
          }
        }
      })
    );
    const { container } = render(<FileLoader {...defaultProps} />);
    const input = container.querySelector('#myFile');
    const file = new File([new ArrayBuffer(6000000)], 'file.xlsx');

    fireEvent.change(input, { target: { files: [file] } });

    expect(ToastrWrapper.showError).toHaveBeenCalled();
  });
});
