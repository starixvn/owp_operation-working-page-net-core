import { useCallback } from 'react';
import Router, {BrowserRouter} from 'react-router-dom';
import { Provider } from 'react-redux';
import { render, fireEvent, screen } from '@testing-library/react';
import configureStore from 'redux-mock-store';
import Header from './Header';
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

describe('test Header component', () => {
  afterAll(() => {
    jest.clearAllMocks();
  });

  const props = {
    history: {}
  };

  it('should render Header component', () => {
    jest
      .spyOn(Router, 'useParams')
      .mockReturnValue({ leagueName: 'àe', isActive: 'True' });
    const { getByText, container } = render(
      <Wrapper>
        <BrowserRouter> <Header {...props} /></BrowserRouter>
      </Wrapper>
    );
    expect(getByText('League list')).toBeInTheDocument();
  });

  it('should render button inactive', () => {
    jest
      .spyOn(Router, 'useParams')
      .mockReturnValue({ leagueName: 'ae', isActive: 'false' });
    const { getByText, container } = render(
      <Wrapper>
        <BrowserRouter> <Header {...props} /></BrowserRouter>
      </Wrapper>
    );
    expect(getByText('Inactive')).toBeInTheDocument();
  });
});
