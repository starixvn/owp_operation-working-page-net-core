import React from 'react';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import { Provider } from 'react-redux';
import configureStore from 'redux-mock-store';
import EditableCommonCell from '../EditableCommonCell';
import { TEditableCell } from '../../../../type/table';

const mockStore = configureStore([]);

describe('EditableCommonCell', () => {
  let store: any;
  let container: HTMLElement;
  const setUp = (store: any) => {
    container = render(
      <Provider store={store}>
        <table>
          <tbody>
            <tr>
              <EditableCommonCell {...defaultProps} />
            </tr>
          </tbody>
        </table>
      </Provider >
    ).container;
  };

  beforeEach(() => {
    jest.clearAllMocks();
    store = mockStore({
      providerPolicy: {
        permission: { HasWrite: true }
      }
    });
  });

  const defaultProps: TEditableCell = {
    rowIndex: 0,
    className: 'test-class',
    value: 'test-value',
    onBlur: jest.fn(),
    columnKey: 'test-column-key',
    owpMatchId: 1,
  };

  it('should render the component', () => {
    setUp(store);

    const cell = screen.getByText('test-value');
    expect(cell).toBeInTheDocument();
    expect(container.querySelector('.test-class')).toBeInTheDocument();
  });

  it('should enter edit mode on click if HasWrite is true', () => {
    setUp(store);

    const cell = container.querySelector('.test-class')?.firstChild;
    fireEvent.click(cell!);
    waitFor(() =>
      expect(container.querySelector('.input-field')).toBeInTheDocument()
    );
  });

  it('should not enter edit mode on click if HasWrite is false', () => {
    store = mockStore({
      providerPolicy: {
        permission: { HasWrite: false }
      }
    });

    setUp(store);

    const cell = screen.getByText('test-value');
    fireEvent.click(cell);

    const input = screen.queryByDisplayValue('test-value');
    expect(input).not.toBeInTheDocument();
  });

  it('should call onBlur with the correct value when input loses focus', () => {
    setUp(store);

    const cell = screen.getByText('test-value');
    fireEvent.click(cell);
    waitFor(() => {
      const input = screen.getByDisplayValue('test-value');
      fireEvent.blur(input);

      expect(defaultProps.onBlur).toHaveBeenCalledWith('test-value');
    });
  });
});
