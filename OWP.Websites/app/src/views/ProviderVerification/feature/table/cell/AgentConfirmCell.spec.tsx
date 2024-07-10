import { fireEvent, getByText, render, screen, waitFor } from '@testing-library/react';
import React, { act } from 'react';
import { Provider } from 'react-redux';
import configureStore from 'redux-mock-store';
import AgentConfirmCell from './AgentConfirmCell';
import { useAppDispatch, useAppSelector } from '../../../app/hookNative';

jest.mock('../../../../app/hookNative');
const mockStore = configureStore([]);
const mockDispatch = jest.fn();

describe('AgentConfirmCell', () => {
  let store: any;
  let container: HTMLElement;

  const setUp = (store: any, props: any) => {
    container = render(
      <Provider store={store}>
        <table>
          <tbody>
            <tr>
              <AgentConfirmCell {...props} />
            </tr>
          </tbody>
        </table>
      </Provider>
    ).container;
  };

  beforeEach(() => {
    jest.clearAllMocks();
    store = mockStore({
      events: {
        agentConfirmOption: [
          { label: 'Agent 1', value: '1' },
          { label: 'Agent 2', value: '2' }
        ]
      },
      providerPolicy: {
        permission: { HasWrite: true }
      }
    });

    (useAppDispatch as jest.Mock).mockReturnValue(mockDispatch);
    (useAppSelector as jest.Mock).mockImplementation((selector) => selector(store.getState()));
  });

  const defaultProps = {
    agentConfirm: [{ label: 'Agent 1', value: '1' }],
    owpMatchId: 1
  };

  it('should render the component', () => {
    setUp(store, defaultProps);

    const selectElement = container.querySelector('.multi-select');
    expect(selectElement).toBeInTheDocument();
    expect(screen.getByText('Agent 1')).toBeInTheDocument();
  });

  it('should handle selection change', async () => {
    setUp(store, defaultProps);
    fireEvent.click(container.querySelector('.dropdown-heading-dropdown-arrow')!);
    const newSelection = screen.getByText('Agent 2');
    fireEvent.click(newSelection);
    waitFor(() => {
      expect(mockDispatch).toHaveBeenCalled();
    });
  });

  it('should disable selection if HasWrite is false', () => {
    store = mockStore({
      events: {
        agentConfirmOption: [
          { label: 'Agent 1', value: '1' },
          { label: 'Agent 2', value: '2' }
        ]
      },
      providerPolicy: {
        permission: { HasWrite: false }
      }
    });

    setUp(store, defaultProps);
    waitFor(() => {
      const selectElement = container.querySelector('.multi-select');
      expect(selectElement).toBeDisabled();
    });
  });
});
