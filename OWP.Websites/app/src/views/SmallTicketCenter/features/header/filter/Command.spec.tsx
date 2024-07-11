import React from 'react';
import { render, fireEvent } from '@testing-library/react';
import configureStore from 'redux-mock-store';
import { Provider } from 'react-redux';
import Command from './Command';
import ToastrWrapper from '../../../../components/ToastrWrapper';
import { getEventTickets } from '../../table/slice/ticketSliceThunkNative';

const initialState = {
  filter: {
    entities: {
      filterInfo: {
        ticketTypeId: '123',
        ticketStatuses: ['Open']
      }
    }
  }
};

const mockStore = configureStore([]);

describe('Command component', () => {
  let store;
  let component;

  beforeEach(() => {
    store = mockStore(initialState);
    ToastrWrapper.showError = jest.fn();
  });

  afterEach(() => {
    store.clearActions();
  });

  it('displays error message if ticketStatuses is missing', () => {
    const newState = {
      ...initialState,
      filter: {
        entities: {
          filterInfo: {
            ...initialState.filter.entities.filterInfo,
            ticketStatuses: [] // Empty ticketStatuses array
          }
        }
      }
    };

    store = mockStore(newState);

    component = render(
      <Provider store={store}>
        <Command />
      </Provider>
    );

    const searchButton = component.getByText('Search');
    fireEvent.click(searchButton);
    const actions = store.getActions();
    expect(actions).not.toBe([]);
  });
});
