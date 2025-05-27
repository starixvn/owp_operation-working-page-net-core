import React from 'react';
import { render, screen } from '@testing-library/react';
import { Provider } from 'react-redux';
import configureStore from 'redux-mock-store';
import NoteCell from './NoteCell';
import { TRemarkCell } from '../../../type/table';

// Mock the NotePopover component
jest.mock('../components/NotePopover', () => {
  return function MockNotePopover(props: any) {
    return <div
      data-testid="note-popover"
      data-haswrite={props.hasWrite}
      data-isremark={props.isRemark}
      data-caneditdate={props.canEditDate}
      data-value={props.value || ''}
      {...props}
    />;
  };
});

const mockStore = configureStore([]);

describe('NoteCell', () => {
  const defaultProps: TRemarkCell = {
    className: 'test-class',
    OWPMatchId: 123,
    value: 'Test note',
    index: 0,
    isRemark: true,
    canEditDate: true
  };

  it('renders NotePopover with correct props', () => {
    const store = mockStore({
      providerPolicy: {
        permission: {
          HasWrite: true
        }
      }
    });

    render(
      <Provider store={store}>
        <NoteCell {...defaultProps} />
      </Provider>
    );

    const notePopover = screen.getByTestId('note-popover');
    expect(notePopover).toBeInTheDocument();
    expect(notePopover.className).toBe('test-class');
    expect(notePopover.getAttribute('data-haswrite')).toBe('true');
    expect(notePopover.getAttribute('data-isremark')).toBe('true');
    expect(notePopover.getAttribute('data-caneditdate')).toBe('true');
    expect(notePopover).toHaveAttribute('value', 'Test note');
    expect(notePopover).toHaveAttribute('OWPMatchId', '123');
    expect(notePopover).toHaveAttribute('index', '0');
  });

  it('passes correct write permission to NotePopover', () => {
    const store = mockStore({
      providerPolicy: {
        permission: {
          HasWrite: false
        }
      }
    });

    render(
      <Provider store={store}>
        <NoteCell {...defaultProps} />
      </Provider>
    );

    const notePopover = screen.getByTestId('note-popover');
    expect(notePopover.getAttribute('data-haswrite')).toBe('false');
  });

  it('handles undefined value', () => {
    const store = mockStore({
      providerPolicy: {
        permission: {
          HasWrite: true
        }
      }
    });

    const propsWithoutValue = { ...defaultProps, value: undefined };
    render(
      <Provider store={store}>
        <NoteCell {...propsWithoutValue} />
      </Provider>
    );

    const notePopover = screen.getByTestId('note-popover');
    expect(notePopover.getAttribute('data-value')).toBe('');
  });
}); 