// ConfirmProviderCell.test.tsx
import React from 'react';
import { render, screen, fireEvent } from '@testing-library/react';
import ConfirmProviderCell from './ConfirmProviderCell';
import { useAppDispatch, useAppSelector } from '../../../app/hookNative';
import { MultiSelect } from 'react-multi-select-component';
import { saveConfirmProvider } from '../tableThunk';
import { setConfirmProvider } from '../tableSlice';

// Mocks
jest.mock('../../../app/hookNative', () => ({
  useAppDispatch: jest.fn(),
  useAppSelector: jest.fn(),
}));
jest.mock('../tableThunk', () => ({
  saveConfirmProvider: jest.fn(),
}));
jest.mock('../tableSlice', () => ({
  setConfirmProvider: jest.fn(),
}));
jest.mock('react-multi-select-component', () => ({
  MultiSelect: jest.fn(() => <div data-testid="multi-select">Mock MultiSelect</div>)
}));

describe('ConfirmProviderCell', () => {
  const mockDispatch = jest.fn();

  const mockConfirmProviderOption = [
    { label: 'Agent A', value: '1' },
    { label: 'Agent B', value: '2' },
  ];

  const selected = [{ label: 'Agent A', value: '1' }];

  beforeEach(() => {
    jest.clearAllMocks();

    (useAppDispatch as jest.Mock).mockReturnValue(mockDispatch);
    (useAppSelector as jest.Mock).mockImplementation(selector =>
      selector({
        events: { confirmProviderOption: mockConfirmProviderOption },
        providerPolicy: { permission: { HasWrite: true } }
      })
    );
  });

  it('renders with correct props', () => {
    render(
      <table>
        <tbody>
          <ConfirmProviderCell
            confirmProvider={selected}
            owpMatchId={123}
            canEditDate={true}
          />
        </tbody>
      </table>
    );

    expect(screen.getByTestId('multi-select')).toBeInTheDocument();
  });

  it('dispatches correct actions on change when editable', () => {
    // Replace `MultiSelect` mock to allow `onChange` test
    (MultiSelect as unknown as jest.Mock).mockImplementation(({ onChange }) => {
      return (
        <button
          onClick={() => onChange([
            { label: 'Agent B', value: '2' }
          ])}
        >
          Trigger Change
        </button>
      );
    });

    render(
      <table>
        <tbody>
          <ConfirmProviderCell
            confirmProvider={selected}
            owpMatchId={456}
            canEditDate={true}
          />
        </tbody>
      </table>
    );

    fireEvent.click(screen.getByText('Trigger Change'));

    expect(saveConfirmProvider).toHaveBeenCalledWith({
      owpMatchId: 456,
      agentSelected: '2',
    });

    expect(setConfirmProvider).toHaveBeenCalledWith({
      owpMatchId: 456,
      value: [{ label: 'Agent B', value: '2' }]
    });

    expect(mockDispatch).toHaveBeenCalledTimes(2);
  });

  it('does not dispatch on change when canEditDate is false', () => {
    (MultiSelect as unknown as jest.Mock).mockImplementation(({ onChange }) => {
      return (
        <button
          onClick={() => onChange([
            { label: 'Agent B', value: '2' }
          ])}
        >
          Trigger Change
        </button>
      );
    });

    render(
      <table>
        <tbody>
          <ConfirmProviderCell
            confirmProvider={selected}
            owpMatchId={789}
            canEditDate={false}
          />
        </tbody>
      </table>
    );

    fireEvent.click(screen.getByText('Trigger Change'));

    expect(mockDispatch).toHaveBeenCalledTimes(0);
  });
});
