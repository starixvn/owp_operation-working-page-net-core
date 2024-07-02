import { act, fireEvent, render } from '@testing-library/react';
import axios from 'axios';
import { useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import ToastrWrapper from '../../../components/ToastrWrapper';
import Review from './Review';

jest.mock('react-redux', () => ({
  useSelector: jest.fn(),
  useDispatch: jest.fn(),
  useEffect: jest.fn()
}));
jest.mock('react', () => ({
  ...jest.requireActual('react'),
}));
jest.mock('../../../components/ToastrWrapper');
jest.mock('axios');

const mockDispatch = jest.fn();
const mockErrorToast = jest.fn();
const mockInfoToast = jest.fn();

ToastrWrapper.showError = mockErrorToast;
ToastrWrapper.showInfo = mockInfoToast;

const validationResult = {
  hasError: false,
  sessionId: '',
  recordCount: 0,
  validationResults: [
    {
      row: 1,
      message: ''
    }
  ],
  isSuccessful: false,
  exception: ''
};
const toastValueDefault = {
  value1: true,
  value2: true
}
const submitResponseDefault = { isSuccessful: false, message: '' };
const setup = ({
  isLoading = false,
  validationResponse = validationResult,
  toastValue = toastValueDefault,
  submitResponse = submitResponseDefault
}) => {
  const state = {
    uploadReducer: {
      request: {
        isUploadFMAndAIO: false,
        eventDate: 'Nov 11, 2022'
      },
      isLoading,
      validationResponse,
      submitResponse
    }
  };
  (useDispatch as jest.Mock).mockReturnValue(mockDispatch);
  (useSelector as jest.Mock).mockImplementation(selectorFn => selectorFn({ ...state }));
  (ToastrWrapper.show as jest.Mock)
    .mockImplementationOnce(() => { return { value: toastValue.value1 } })
    .mockImplementationOnce(() => { return { value: toastValue.value2 } });
};

describe('Review under test', () => {
  afterEach(() => {
    jest.clearAllMocks();
    jest.resetAllMocks();
    jest.resetModules();
  });

  it('isLoading=true, should call mockDispatch and render Loading', () => {
    setup({ isLoading: true });
    const { container } = render(<Review />);
    const element = container.querySelector('.loading-process');
    expect(element).toBeInTheDocument();
    expect(mockDispatch).toBeCalled();
  });

  it('isLoading=false, should render wizard-buttons', () => {
    setup({});
    const { container } = render(<Review />);
    const element = container.querySelector('.wizard-buttons');
    expect(element).toBeInTheDocument();
  });

  it('fail validate and exception is empty, should render success-info', () => {
    setup({});
    const { container } = render(<Review />);
    const element = container.querySelector('.error-info');
    expect(element).toBeInTheDocument();
  });

  it('fail validate and exception, should not render success-info', () => {
    setup({ isLoading: false, validationResponse: { ...validationResult, exception: 'exception' } });
    const { container } = render(<Review />);
    const element = container.querySelector('.error-info');
    expect(element).toBeNull();
    expect(mockErrorToast).toBeCalledTimes(1);
  });

  it('successful validate, should render success-info', () => {
    setup({ isLoading: false, validationResponse: { ...validationResult, isSuccessful: true } });
    const { container } = render(<Review />);
    const element = container.querySelector('.success-info');
    expect(element).toBeInTheDocument();
    expect(mockErrorToast).toBeCalledTimes(0);
  });

  it('{successful validate, recordCount=0} click Submit button, response error, should show ToastrWrapper', async () => {
    setup({
      validationResponse: { ...validationResult, isSuccessful: true },
      submitResponse: { isSuccessful: false, message: 'error' }
    });
    (axios.post as jest.Mock).mockResolvedValue({ data: 'error' });

    const { container } = render(<Review />);

    const element = container.querySelector('.btn-submit');
    expect(element).toBeInTheDocument();

    fireEvent.click(element!);

    await act(async () => {
      await Promise.resolve();
    });

    expect(mockErrorToast).toBeCalled();
  });

  it('click Submit button, No confirm, response error, should not show ToastrWrapper', async () => {
    setup({
      validationResponse: { ...validationResult, isSuccessful: true },
      toastValue: { value1: true, value2: false }
    });
    (axios.post as jest.Mock).mockResolvedValue({ data: 'error' });

    const { container } = render(<Review />);

    const element = container.querySelector('.btn-submit');
    expect(element).toBeInTheDocument();

    fireEvent.click(element!);
    await act(async () => {
      await Promise.resolve();
    });

    expect(mockErrorToast).toHaveBeenCalledTimes(0);
    expect(mockInfoToast).toHaveBeenCalledTimes(0);
  });

  it('click Submit button, No confirm, should no action', async () => {
    setup({
      validationResponse: { ...validationResult, isSuccessful: true },
      toastValue: { value1: false, value2: false }
    });
    (axios.post as jest.Mock).mockResolvedValue({ data: 'error' });

    const { container } = render(<Review />);

    const element = container.querySelector('.btn-submit');
    expect(element).toBeInTheDocument();

    fireEvent.click(element!);
    await act(async () => {
      await Promise.resolve();
    });

    expect(mockErrorToast).toHaveBeenCalledTimes(0);
    expect(mockInfoToast).toHaveBeenCalledTimes(0);
  });

  it('{successful validate, recordCount=0} click Submit button, successful response, should call dispatch', async () => {
    setup({
      validationResponse: { ...validationResult, isSuccessful: true },
      submitResponse: { isSuccessful: true, message: '' }
    });
    (axios.post as jest.Mock).mockResolvedValue({ data: '' });

    const { container } = render(<Review />);
    expect(mockDispatch).toBeCalledTimes(4);

    const element = container.querySelector('.btn-submit');
    expect(element).toBeInTheDocument();

    fireEvent.click(element!);
    await act(async () => {
      await Promise.resolve();
    });

    expect(mockErrorToast).toBeCalledTimes(0);
    expect(mockInfoToast).toBeCalledTimes(1);
    expect(mockDispatch).toBeCalledTimes(6);
  });

  it('{recordCount > 0} click Submit button, successful response, should call dispatch', async () => {
    setup({
      validationResponse: { ...validationResult, isSuccessful: true, recordCount: 10 },
      submitResponse: { isSuccessful: true, message: 'success' }
    });
    (axios.post as jest.Mock).mockResolvedValue({ data: '' });

    const { container } = render(<Review />);
    expect(mockDispatch).toBeCalledTimes(4);

    const element = container.querySelector('.btn-submit');
    expect(element).toBeInTheDocument();

    fireEvent.click(element!);
    await act(async () => {
      await Promise.resolve();
    });

    expect(mockErrorToast).toBeCalledTimes(0);
    expect(mockInfoToast).toBeCalledTimes(1);
    expect(mockDispatch).toBeCalledTimes(6);
  });

  it('click Back button, should call dispatch', async () => {
    setup({});

    const { container } = render(<Review />);
    expect(mockDispatch).toBeCalledTimes(3);

    const element = container.querySelector('.btn-back');
    expect(element).toBeInTheDocument();

    fireEvent.click(element!);

    expect(mockDispatch).toBeCalledTimes(6);
  });
});
