import { fireEvent, render, waitFor, waitForElementToBeRemoved } from '@testing-library/react';
import axios from 'axios';
import { useSelector } from 'react-redux';
import useStableDispatch from '../../../Shared/customHook/useStableDispatch';
import { ProviderData } from '../TypeNative';
import ActionHeader from './ActionHeader';

jest.mock('axios');
jest.mock('react-redux', () => ({
  useSelector: jest.fn()
}));
jest.mock('react', () => ({
  ...(jest.requireActual('react') as object),
}));
jest.mock('../../../Shared/customHook/useStableDispatch');
const mockDispatch = jest.fn();
const mockSetState = jest.fn();
const setup = (isEdit, providers, originalProviders) => {
  const initialState = {
    providerReducer: {
      logData: [
        {
          action: 123,
          modifyBy: 'sa',
          modifyDate: '12-08-2022',
          newValue: ['abc', 'xyz'],
          oldValue: ['abc', 'xyz']
        }
      ],
      isEdit,
      providers,
      originalProviders
    }
  };
  (useSelector as jest.Mock).mockImplementation(selectorFn =>
    selectorFn(initialState)
  );
  (useStableDispatch as jest.Mock).mockReturnValue(mockDispatch);
};
const responseData = { data: { HasWrite: true } };
(axios.post as jest.Mock).mockResolvedValue(responseData);

describe('Modal Log History', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });

  it('Should call dispatch when click cancel button', async () => {
    setup(true, null, null);
    const { container } = render(<ActionHeader />);
    const icon = container.querySelector(`[class="icon-loading"]`);
    expect(icon).toBeInTheDocument();
    await waitForElementToBeRemoved(icon);

    const closeButton = container.querySelector('.btn-cancel');
    closeButton && fireEvent.click(closeButton);
    expect(closeButton).toBeInTheDocument();
    expect(mockDispatch).toBeCalledTimes(2);
  });

  it('Should call dispatch when click edit button', async () => {
    setup(false, null, null);
    const { container } = render(<ActionHeader />);
    const icon = container.querySelector(`[class="icon-loading"]`);
    expect(icon).toBeInTheDocument();
    await waitForElementToBeRemoved(icon);

    const editButton = container.querySelector('.btn-edit');
    editButton && fireEvent.click(editButton);
    expect(editButton).toBeInTheDocument();
    expect(mockDispatch).toBeCalledTimes(2);
  });

  it('Should call dispatch 3 times when click save button', async () => {
    const providers: ProviderData[] = [
      {
        id: 1,
        order: 3,
        value: 'abc'
      },
      {
        id: 2,
        order: 1,
        value: 'xyz'
      },
      {
        id: 3,
        order: 2,
        value: 'test'
      }
    ];
    const originalProviders: ProviderData[] = [
      {
        id: 1,
        order: 1,
        value: 'abc'
      },
      {
        id: 2,
        order: 2,
        value: 'xyz'
      },
      {
        id: 3,
        order: 3,
        value: 'test'
      }
    ];
    setup(true, providers, originalProviders);
    const { container } = render(<ActionHeader />);
    const icon = container.querySelector(`[class="icon-loading"]`);
    expect(icon).toBeInTheDocument();
    await waitForElementToBeRemoved(icon);

    const saveButton = container.querySelector('.btn-save');
    saveButton && fireEvent.click(saveButton);
    expect(saveButton).toBeInTheDocument();
    expect(mockDispatch).toBeCalledTimes(3);
  });

  it('Should call dispatch 2 times when click save button', async () => {
    const providers: ProviderData[] = [
      {
        id: 1,
        order: 1,
        value: 'abc'
      },
      {
        id: 2,
        order: 2,
        value: 'xyz'
      },
      {
        id: 3,
        order: 3,
        value: 'test'
      }
    ];
    const originalProviders: ProviderData[] = [
      {
        id: 1,
        order: 1,
        value: 'abc'
      },
      {
        id: 2,
        order: 2,
        value: 'xyz'
      },
      {
        id: 3,
        order: 3,
        value: 'test'
      }
    ];
    setup(true, providers, originalProviders);
    const { container } = render(<ActionHeader />);
    const icon = container.querySelector(`[class="icon-loading"]`);
    expect(icon).toBeInTheDocument();
    await waitForElementToBeRemoved(icon);

    const saveButton = container.querySelector('.btn-save');
    saveButton && fireEvent.click(saveButton);
    expect(saveButton).toBeInTheDocument();
    expect(mockDispatch).toBeCalledTimes(2);
  });
});
