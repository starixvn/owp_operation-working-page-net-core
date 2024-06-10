import { useCallback } from 'react';
import { Provider } from 'react-redux';
import { render, fireEvent, screen } from '@testing-library/react';
import configureStore from 'redux-mock-store';
import Maxbet from './Maxbet';
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
    maxbetGeneralData: []
  },
  tvListReducer: {
    tvListPending: true,
    tvListExpression: []
  },
  policyReducer: {
    policyData: {
      tvListPolicy: {
        HasRead: true
      }
    }
  }
};
const store = mockStore(initialState);
jest.spyOn(store, 'dispatch');
const Wrapper = ({ children }) => <Provider store={store}>{children}</Provider>;
const { ResizeObserver } = window;
describe('test LeagueCategory component', () => {
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

  it('should render Maxbet component', () => {
    const { getByText, container } = render(
      <Wrapper>
        <Maxbet />
      </Wrapper>
    );

    expect(getByText('Category')).toBeInTheDocument();
  });

  it('should render renderTVList  component', () => {
    const { getByText, container } = render(
      <Wrapper>
        <Maxbet />
      </Wrapper>
    );
    const button = container.querySelector('.button-working-shift');
    fireEvent.click(button);

    expect(container.querySelector('.loading-process')).toBeInTheDocument();
  });

  it('should render renderTVList  component', () => {
    const { getByText, container } = render(
      <Wrapper>
        <Maxbet />
      </Wrapper>
    );
    const button = container.querySelector('.button-submit');
    fireEvent.click(button);

    expect(container.querySelector('.tbl-maxbet-wrapper')).toBeInTheDocument();
  });
});
