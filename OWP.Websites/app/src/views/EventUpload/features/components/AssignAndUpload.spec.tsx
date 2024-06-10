import { fireEvent, render } from '@testing-library/react';
import { useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import ToastrWrapper from '../../../components/ToastrWrapper';
import AssignAndUpload from './AssignAndUpload';

jest.mock('react-redux', () => ({
  useSelector: jest.fn(),
  useDispatch: jest.fn()
}));
jest.mock('react', () => ({
  ...jest.requireActual('react'),
  useState: jest.fn()
}));
jest.mock('../../../components/ToastrWrapper');

const mockDispatch = jest.fn();
const mockSetState = jest.fn();
const mockErrorToast = jest.fn();

ToastrWrapper.showError = mockErrorToast;

const setup = () => {
  const state = {
    uploadReducer: {
      request: {
        isUploadFMAndAIO: false,
        eventDate: 'Nov 11, 2022'
      }
    }
  };
  (useState as jest.Mock).mockImplementation(init => [init, mockSetState]);
  (useDispatch as jest.Mock).mockReturnValue(mockDispatch);
  (useSelector as jest.Mock).mockImplementation(selectorFn => selectorFn({ ...state }));
};

describe('AssignAndUpload under test', () => {
  beforeEach(() => {
    setup();
  })
  afterEach(() => {
    jest.clearAllMocks();
  });

  it('should render upload-file class', () => {
    const { container } = render(<AssignAndUpload />);
    const element = container.querySelector('.upload-file');
    expect(element).toBeInTheDocument();
  });

  it('when click Next button, file is empty, should call mockErrorToast', () => {
    const { container } = render(<AssignAndUpload />);
    const element = container.querySelector('.btn-next');
    fireEvent.click(element!);
    expect(element).toBeInTheDocument();
    expect(mockErrorToast).toBeCalled();
  });

  it('when click Back button', () => {
    const { container } = render(<AssignAndUpload />);
    const element = container.querySelector('.btn-back');
    fireEvent.click(element!);
    expect(element).toBeInTheDocument();
  });

  it('when click Next button, file has value, should call mockDispatch', () => {
    (useState as jest.Mock)
      .mockReturnValueOnce(['', {}])
      .mockReturnValueOnce([new File([], 'excel.xlsx'), {}]);
    const { container } = render(<AssignAndUpload />);
    const element = container.querySelector('.btn-next');
    fireEvent.click(element!);
    expect(mockDispatch).toBeCalled();
  });

  it('when click check box, should call mockDispatch', () => {
    const { container } = render(<AssignAndUpload />);
    const element = container.querySelector('.check-input');
    fireEvent.click(element!);
    expect(mockDispatch).toBeCalled();
  });

  it('when change input file, should call mockSetState', () => {
    const { container } = render(<AssignAndUpload />);
    const input = container.querySelector('#myFile');
    const file = new File([new ArrayBuffer(6000000)], 'file.xlsx');

    fireEvent.change(input!, { target: { files: [file] } });
    expect(mockSetState).toBeCalledTimes(3);
  });
});
