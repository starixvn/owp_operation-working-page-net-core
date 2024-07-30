import { useCallback } from 'react';
import Router from 'react-router-dom';
import { Provider } from 'react-redux';
import { render, fireEvent, screen } from '@testing-library/react';
import configureStore from 'redux-mock-store';
import Condition from './Condition';
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
  },

  conditionReducer: {
    conditionPending: true,
    leagueSettingId: 12,
    tv: '12',
    round: '12',
    division: 12
  }
};
const store = mockStore(initialState);
jest.spyOn(store, 'dispatch');

jest.mock('react-router-dom', () => ({
  ...jest.requireActual('react-router-dom'),
  useParams: jest.fn()
}));

jest.spyOn(Router, 'useParams').mockReturnValue({ id: 13 });

const Wrapper = ({ children }) => <Provider store={store}>{children}</Provider>;

describe('test Condition component', () => {
  const props = {
    leagueSettingId: 23,
    onClose: jest.fn()
  };
  afterAll(() => {
    jest.clearAllMocks();
  });
  it('render correct loading component', () => {
    const { container } = render(
      <Wrapper>
        {' '}
        <Condition {...props} />
      </Wrapper>
    );
    expect(container.querySelector('.loading-process')).toBeInTheDocument();
  });
});
