import React from 'react';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import '@testing-library/jest-dom';
import { useAppDispatch, useAppSelector } from '../../../app/hooks';
import { closeEditMode, setEditIndex } from '../slice/quickMessageSlice';
import { deleteQuickMessageThunk, queryQuickMessageThunk, saveQuickMessageThunk } from '../slice/quickMessageThunk';
import ToastrWrapper from '../../../../components/ToastrWrapper';
import Action from './Action';
import { TQuickMessage } from '../../../Type';

// Mocking external dependencies
jest.mock('../../../app/hooks', () => ({
  useAppDispatch: jest.fn(),
  useAppSelector: jest.fn(),
}));
jest.mock('../slice/quickMessageSlice', () => ({
  closeEditMode: jest.fn(),
  setEditIndex: jest.fn(),
}));
jest.mock('../slice/quickMessageThunk', () => ({
  deleteQuickMessageThunk: jest.fn(),
  queryQuickMessageThunk: jest.fn(),
  saveQuickMessageThunk: jest.fn(),
}));
jest.mock('../../../../components/ToastrWrapper', () => ({
  showError: jest.fn(),
}));

const mockDispatch = jest.fn();
const mockUseAppSelector = useAppSelector as jest.Mock;
const mockUseAppDispatch = useAppDispatch as jest.Mock;
let container: HTMLElement;
const setUp = (isEdit: boolean) => {
  container = render(
    <table>
      <tbody>
        <tr>
          <Action isEdit={isEdit} index={0} />
        </tr>
      </tbody>
    </table>
  ).container;
};

describe('Action Component', () => {
  beforeEach(() => {
    jest.clearAllMocks();
    mockUseAppDispatch.mockReturnValue(mockDispatch);
    setUp(false);
  });

  test('renders edit and delete buttons when not in edit mode', () => {
    mockUseAppSelector.mockImplementation(callback =>
      callback({
        quickMessageSlice: {
          editModel: {} as TQuickMessage,
          quickMessage: [{ id: 1, shortcut: 'A', message: 'Test' }],
        },
      })
    );

    expect(container.querySelector('.edit-action')).toBeInTheDocument();
    expect(container.querySelector('.delete-action')).toBeInTheDocument();
  });

  test('renders save and cancel buttons when in edit mode', () => {
    setUp(true);

    expect(container.querySelector('.save-action')).toBeInTheDocument();
    expect(container.querySelector('.cancel-action')).toBeInTheDocument();
  });

  test('dispatches setEditIndex on edit button click', () => {
    fireEvent.click(container.querySelector('.edit-action')!);

    expect(setEditIndex).toHaveBeenCalledWith(0);
  });

  test('dispatches deleteQuickMessageThunk and queryQuickMessageThunk on delete button click', async () => {
    mockUseAppSelector.mockImplementation(callback =>
      callback({
        quickMessageSlice: {
          editModel: {} as TQuickMessage,
          quickMessage: [{ id: 1, shortcut: 'A', message: 'Test' }],
        },
      })
    );

    fireEvent.click(container.querySelector('.delete-action')!);

    await waitFor(() => {
      expect(deleteQuickMessageThunk).toHaveBeenCalledWith({ id: 1 });
      expect(queryQuickMessageThunk).toHaveBeenCalled();
    });
  });

  test('dispatches saveQuickMessageThunk, closeEditMode, and queryQuickMessageThunk on save button click', async () => {
    const mockEditModel = { shortcut: 'A', message: 'Test', id: 1 };
    mockUseAppSelector.mockImplementation(callback =>
      callback({
        quickMessageSlice: {
          editModel: mockEditModel,
          quickMessage: [{ id: 1, shortcut: 'A', message: 'Test' }],
        },
      })
    );
    setUp(true);

    fireEvent.click(container.querySelector('.save-action')!);

    await waitFor(() => {
      expect(saveQuickMessageThunk).toHaveBeenCalledWith(mockEditModel);
      expect(closeEditMode).toHaveBeenCalled();
      expect(queryQuickMessageThunk).toHaveBeenCalled();
    });
  });

  test('shows error if validation fails on save button click', async () => {
    const mockEditModel = { shortcut: '', message: '', id: 1 };
    mockUseAppSelector.mockImplementation(callback =>
      callback({
        quickMessageSlice: {
          editModel: mockEditModel,
          quickMessage: [{ id: 1, shortcut: 'A', message: 'Test' }],
        },
      })
    );

    setUp(true);

    fireEvent.click(container.querySelector('.save-action')!);

    await waitFor(() => {
      expect(ToastrWrapper.showError).toHaveBeenCalledWith('You must fill in all the text boxes');
    });
  });

  test('shows error 2 if validation fails on save button click', async () => {
    const mockEditModel = { shortcut: 'tyvrm', message: 'message', id: 1 };
    mockUseAppSelector.mockImplementation(callback =>
      callback({
        quickMessageSlice: {
          editModel: mockEditModel,
          quickMessage: [{ id: 2, shortcut: 'tyvrm', message: 'Test' }],
        },
      })
    );

    setUp(true);

    fireEvent.click(container.querySelector('.save-action')!);

    await waitFor(() => {
      expect(ToastrWrapper.showError).toHaveBeenCalledWith('This shortcut already exists');
    });
  });

  test('dispatches closeEditMode on cancel button click', () => {
    setUp(true);

    fireEvent.click(container.querySelector('.cancel-action')!);

    expect(closeEditMode).toHaveBeenCalled();
  });
});
