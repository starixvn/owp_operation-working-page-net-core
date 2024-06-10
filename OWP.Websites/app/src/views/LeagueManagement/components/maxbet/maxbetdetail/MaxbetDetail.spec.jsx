import { useCallback } from 'react';
import Router from 'react-router-dom';
import { Provider } from 'react-redux';
import { render, fireEvent, screen } from '@testing-library/react';
import configureStore from 'redux-mock-store';
import MaxbetDetail from './MaxbetDetail';
import { BrowserRouter, Route, Link } from 'react-router-dom';
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
  leagueDetailReducer: { leagueDetailData: [] },
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
        HasRead: true
      }
    }
  },
  toggleColumnReducer: { columnKeys: [] },
  matchOffersReducer: {
    matchOffers: [
      { Id: 1, Value: '1s Accept' },
      { Id: 2, Value: '3s Accept' }
    ]
  }
};
const store = mockStore(initialState);
jest.spyOn(store, 'dispatch');

jest.mock('react-router-dom', () => ({
  ...jest.requireActual('react-router-dom'),
  useParams: jest.fn()
}));

jest
  .spyOn(Router, 'useParams')
  .mockReturnValue({ leagueName: 'àe', isActive: 'True' });

const Wrapper = ({ children }) => <Provider store={store}>{children}</Provider>;
const { ResizeObserver } = window;

describe('test MaxbetDetail component', () => {
  beforeEach(() => {
    delete window.ResizeObserver;
    window.ResizeObserver = jest.fn().mockImplementation(() => ({
      observe: jest.fn(),
      unobserve: jest.fn(),
      disconnect: jest.fn()
    }));
  });
    afterEach(() => {
    window.ResizeObserver = ResizeObserver;
    jest.restoreAllMocks();
  });

  const props = {
    history: {},
    isActive: 'True'
  };

  it('should render MaxbetDetail component', () => {
    jest
      .spyOn(Router, 'useParams')
      .mockReturnValue({ id: 1, isActive: 'true' });
    const { getByText, container } = render(
      <Wrapper>
        <BrowserRouter>
          <MaxbetDetail {...props} />
        </BrowserRouter>
      </Wrapper>
    );
    expect(getByText('League list')).toBeInTheDocument();
  });
});
