import React from 'react';
import { render, fireEvent } from '@testing-library/react';
import configureStore from 'redux-mock-store';
import { Provider } from 'react-redux';
import Event from './Event';
import { setFilterParam } from './filterSlice';

const mockStore = configureStore([]);

describe('Event component', () => {
  let store;
  let component;

  beforeEach(() => {
    store = mockStore({});
  });

  afterEach(() => {
    store.clearActions();
  });

  it('dispatches setFilterParam action on input blur', () => {
    component = render(
      <Provider store={store}>
        <Event />
      </Provider>
    );

    const inputElement = component.getByPlaceholderText('League/Match/Subject');
    fireEvent.blur(inputElement);

    const actions = store.getActions();
  });
});
