import { render } from '@testing-library/react';
import { Provider } from 'react-redux';
import configureStore from 'redux-mock-store';
import AuthorizeService from '../Authorize/Service';
import EventInformation from './EventAssignment';
import EventAssignmentService from './Service';
jest.mock('./Service');
jest.mock('../Authorize/Service');
const middleWares = [];
const mockStore = configureStore(middleWares);
const store = mockStore({
  chatBoard: { modelByRowIndex: {} },
  createTicketBoard: {
    selectedRecipients: [],
    subTypeSelected: null,
    isOpenCreateTicketInLGA: true
  },
  tableReducer: {
    dataEvent: []
  }
});
const { ResizeObserver } = window;
jest.spyOn(store, 'dispatch');
const Wrapper = ({ children }) => <Provider store={store}>{children}</Provider>;
describe('EventAssignment', () => {
  beforeEach(() => {
    //@ts-ignore
    delete window.ResizeObserver;
    window.ResizeObserver = jest.fn().mockImplementation(() => ({
      observe: jest.fn(),
      unobserve: jest.fn(),
      disconnect: jest.fn()
    }));
    const policy = { HasRead: false, HasWrite: false };
    const events = [
      {
        OWPMatchId: 1,
        MatchNo: '1'
      },
      {
        OWPMatchId: 2,
        MatchNo: '2'
      }
    ];
    AuthorizeService.authorize.mockResolvedValue(policy);
    EventAssignmentService.query.mockResolvedValue(events);
  });
  afterEach(() => {
    window.ResizeObserver = ResizeObserver;
    jest.restoreAllMocks();
  });
  it('should match snapshot on init', () => {
    const utils = render(
      <Wrapper>
        <EventInformation date={new Date()} />
      </Wrapper>
    );
    expect(
      utils.container.getElementsByClassName('event-assignment')
    ).toHaveLength(1);
  });
});
