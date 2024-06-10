import { useCallback } from 'react';
import Router, {BrowserRouter} from 'react-router-dom';
import { Provider } from 'react-redux';
import { render, fireEvent, screen } from '@testing-library/react';
import configureStore from 'redux-mock-store';
import LeagueCell from './LeagueCell';
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
    status: {
      value: 1
    }
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
        HasRead: true
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

describe('test LeagueCell component', () => {
  afterAll(() => {
    jest.clearAllMocks();
  });

  it('should render LeagueCell component', () => {
    const props = {
      dataSource: [{ OWPLeagueId: 1, IsActive: true }],
      rowIndex: 0
    };
    const { getByText, container } = render(
      <Wrapper>
        <BrowserRouter> <LeagueCell {...props} /></BrowserRouter>
      </Wrapper>
    );
    expect(
      container.querySelector('.fixedDataTableCellLayout_wrap1')
    ).toBeInTheDocument();
  });

  it('should render BlackList', () => {
    const props = {
      dataSource: [
        {
          OWPLeagueId: 1,
          IsActive: true,
          IsBlackList: true
        }
      ],
      rowIndex: 0
    };
    const { getByText, container } = render(
      <Wrapper>
        <BrowserRouter> <LeagueCell {...props} /></BrowserRouter>
      </Wrapper>
    );

    expect(container.querySelector('.icon-alert')).toBeInTheDocument();
  });
});
