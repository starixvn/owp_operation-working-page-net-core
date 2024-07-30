import React from 'react';
import { render, fireEvent } from '@testing-library/react';
import configureStore from 'redux-mock-store';
import { Provider } from 'react-redux';
import Status from './Status';
import { setFilterParam } from './filterSlice';
import { STATUS } from '../../../const/const.json';

const initialState = {
  filter: {
    entities: {
      filterInfo: {
        ticketStatuses: ['Open', 'InProgress']
      }
    }
  }
};

const mockStore = configureStore([]);

describe('Status component', () => {
  let store;
  let component;

  beforeEach(() => {
    store = mockStore(initialState);
  });

  afterEach(() => {
    store.clearActions();
  });

  it('renders Status component correctly', () => {
    component = render(
      <Provider store={store}>
        <Status />
      </Provider>
    );

    const labelElement = component.getByText('Status');
    expect(labelElement).toBeInTheDocument();
  });
});
