import { useCallback } from 'react';
import Router from 'react-router-dom';
import { Provider } from 'react-redux';
import { render, fireEvent, screen } from '@testing-library/react';
import configureStore from 'redux-mock-store';
import NoOfHdpCell from './NoOfHdpCell';
import '@testing-library/jest-dom';

const middleWares = [];
const mockStore = configureStore(middleWares);

const initialState = {
  leagueCategoryReducer: {
    leagueCategory: ''
  },
  leagueReducer: {
    league: ''
  },
  leagueGroupReducer: {
    leagueGroupPending: true,
    leagueGroupOption: [],
    leagueGroupBackup: {},
    leagueGroupError: {}
  },
  statusReducer: {
    status: ''
  },
  displayOldLeagueReducer: {
    displayOldLeague: ''
  },
  maxbetGeneralReducer: {
    displayMaxbetGeneralTable: {}
  },
  policyReducer: {
    policyData: {
      tvListPolicy: {
        HasRead: true
      },
      generalPolicy: {
        Username: 'sa',
        HasRead: true
      },
      conditionPolicy: {
        HasRead: true
      },
      lstNotePolicy: {
        HasRead: true,
        HasWrite: true
      }
    }
  }
};
const store = mockStore(initialState);
jest.spyOn(store, 'dispatch');

jest.mock('react-router-dom', () => ({
  ...jest.requireActual('react-router-dom'),
  useParams: jest.fn()
}));

const Wrapper = ({ children }) => <Provider store={store}>{children}</Provider>;

describe('test NoOfHdpCell component', () => {
  const props = {
    data: {
      aa: 'ABC'
    },
    rowIndex: 0,
    modelKey: 'aa'
  };
  afterAll(() => {
    jest.clearAllMocks();
  });
  it('render correct', () => {
    const { container } = render(
      <Wrapper>
        <NoOfHdpCell {...props} />
      </Wrapper>
    );
    expect(container.querySelector('.league-group')).toBeInTheDocument();
  });

  it('should call dispatch when onchange select', () => {
    const { container } = render(
      <Wrapper>
        <NoOfHdpCell {...props} />
      </Wrapper>
    );
    const select = container.querySelector('select');
    fireEvent.change(select);
    expect(store.dispatch).toBeCalledTimes(1);
  });
});
