import React from 'react';
import { render } from '@testing-library/react';
import configureStore from 'redux-mock-store';
import { Provider } from 'react-redux';
import TicketType from './TicketType';

const initialState = {
  filter: {
    entities: {
      multipleSelect: {
        ticketTypeOptions: [
          { key: '1', title: 'Option 1' },
          { key: '2', title: 'Option 2' }
        ],
        ticketTypeSelected: ['1']
      }
    }
  }
};

const mockStore = configureStore([]);

describe('TicketType component', () => {
  let store;
  let component;

  beforeEach(() => {
    store = mockStore(initialState);
  });

  afterEach(() => {
    store.clearActions();
  });

  it('renders TicketType component correctly', () => {
    component = render(
      <Provider store={store}>
        <TicketType />
      </Provider>
    );

    const labelElement = component.getByText('Ticket Type - Subtype');
    expect(labelElement).toBeInTheDocument();
  });
});
