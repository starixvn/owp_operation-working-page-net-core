import { useSelector, useDispatch, Provider } from 'react-redux';
import configureStore from 'redux-mock-store';
import { render, screen } from '@testing-library/react';
import MessageList from './MessageList';

const middleWares = [];
const mockStore = configureStore(middleWares);

const initialState = {};
const store = mockStore(initialState);

const Wrapper = ({ children }) => <Provider store={store}>{children}</Provider>;

jest.mock('react-redux', () => ({
  useSelector: jest.fn(),
  useDispatch: jest.fn()
}));
jest.mock('reactstrap');

const mockedDispatch = jest.fn();

const messagesData = [
  {
    messageId: 0,
    message: 'Hello SP',
    userId: 8,
    username: 'sa_nickname',
    fileId: null,
    isFile: false,
    createdDate: '2021-09-29T16:10:45.133'
  }
];

useDispatch.mockReturnValue(mockedDispatch);
const setUp = (status, message = messagesData, actorId) => {
  useSelector.mockImplementation(selectorFn =>
    selectorFn({
      chatBoard: {
        loading: status,
        ticketDetail: {
          ticketId: '12',
          ticketTypeId: '',
          ticketTypeName: '',
          messages: message,
          owpLeagueId: '',
          owpLeagueName: '',
          owpHomeId: '',
          owpAwayId: '',
          homeName: '',
          awayName: '',
          subject: '',
          creatorInfo: {
            id: 0,
            userName: '',
            nickName: ''
          }
        }
      },
      policy: {
        permission: {
          Username: 'sa',
          ActorId: actorId,
          HasWrite: true
        }
      }
    })
  );
};

describe('test MessageList', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });

  it('status is loading, should contain Loading Mask', () => {
    setUp('pending');
    const { container } = render(<MessageList />);
    const loading = container.querySelector('.loading-process');
    expect(loading).toBeInTheDocument();
  });

  it('status is idle, should contain AutoSizer', () => {
    setUp('idle', messagesData, 8);
    const { container } = render(<MessageList />);
    const list = container.querySelector('.message-row');
    expect(list).toBeInTheDocument();
    const message = container.querySelector('.my-message');
    expect(message).toBeInTheDocument();
  });
});
