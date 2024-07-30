import { render, fireEvent } from '@testing-library/react';
import { Provider } from 'react-redux';
import configureStore from 'redux-mock-store';
import TicketList from './TicketList';

// Mock Redux store
const mockStore = configureStore([]);

describe('TicketList Component', () => {
  let store: any;

  beforeEach(() => {
    const initialState = {
      tickerSlice: {
        entities: [
          {
            ticketId: 1,
            eventType: 1
          },
          {
            ticketId: 2,
            eventType: 2
          }
        ]
      },
      pagePolicy: {
        permission: {
          ActorId: '123'
        }
      }
    };

    store = mockStore(initialState);
  });

  it('should render TicketList component', () => {
    const { getByText } = render(
      <Provider store={store}>
        <TicketList />
      </Provider>
    );

    expect(getByText('Conversations')).toBeInTheDocument();
  });

  it('should handle onClick event correctly', () => {
    const { container } = render(
      <Provider store={store}>
        <TicketList />
      </Provider>
    );

    const ticketRows = container.getElementsByClassName(
      'ticket-list-cell-item'
    )[0];

    fireEvent.click(ticketRows);
  });
});
