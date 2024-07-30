import { useCallback } from 'react';
import { Provider } from 'react-redux';
import { render, fireEvent, screen } from '@testing-library/react';
import configureStore from 'redux-mock-store';
import Filter from './Filter';
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
  }
};
const store = mockStore(initialState);
jest.spyOn(store, 'dispatch');
const Wrapper = ({ children }) => <Provider store={store}>{children}</Provider>;

describe('test LeagueCategory component', () => {
  afterAll(() => {
    jest.clearAllMocks();
  });

  it('should render Filter component', () => {
    const { getByText } = render(
      <Wrapper>
        <Filter displayMaxbetGeneralTable={() => {}} />
      </Wrapper>
    );
    expect(getByText('Category')).toBeInTheDocument();
  });

  it('should click button search', () => {
    const { container } = render(
      <Wrapper>
        <Filter displayMaxbetGeneralTable={() => {}} />
      </Wrapper>
    );

    const button = container.querySelector('.button-submit');
    fireEvent.click(button);

    expect(store.dispatch).toBeCalled();
  });
});
