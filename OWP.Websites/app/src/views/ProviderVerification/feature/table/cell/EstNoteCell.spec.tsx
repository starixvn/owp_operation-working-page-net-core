import { fireEvent, render, screen } from '@testing-library/react';
import React from 'react';
import { Provider } from 'react-redux';
import configureStore from 'redux-mock-store';
import EstNoteCell from './EstNoteCell';

jest.mock('../tableThunk', () => ({
  saveESTNote: jest.fn()
}));

jest.mock('../tableSlice', () => ({
  setValue: jest.fn()
}));

const mockStore = configureStore([]);
const initialState = {
  providerPolicy: {
    permission: { HasWrite: true }
  },
  events: {
    // Mock any other state slices if necessary
  }
};

const renderComponent = (props = {}) => {
  const store = mockStore(initialState);
  return render(
    <Provider store={store}>
      <table>
        <tbody>
          <tr>
            <EstNoteCell
              rowIndex={0}
              value="initial value"
              owpMatchId={1}
              columnKey="estNote"
              {...props}
            />
          </tr>
        </tbody>
      </table>
    </Provider>
  );
};

describe('EstNoteCell', () => {
  beforeEach(() => {
    jest.clearAllMocks();
  });

  it('should render initial value', () => {
    renderComponent();
    expect(screen.getByText('initial value')).toBeInTheDocument();
  });

  it('should not enable editing on click if HasWrite is false', () => {
    initialState.providerPolicy.permission.HasWrite = false;
    renderComponent();
    fireEvent.click(screen.getByText('initial value'));
    expect(screen.queryByRole('textbox')).not.toBeInTheDocument();
  });
});
