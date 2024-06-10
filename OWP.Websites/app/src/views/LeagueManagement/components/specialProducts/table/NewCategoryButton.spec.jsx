import { useCallback } from 'react';
import { Provider } from 'react-redux';
import configureMockStore from 'redux-mock-store';
import { render, fireEvent, screen } from '@testing-library/react';
import NewCategoryButton from './NewCategoryButton';

const middleWares = [];
const mockStore = configureMockStore(middleWares);

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
        HasWrite: true
      }
    }
  },
  specialProductReducer: {
    specialProductPending: true,
    specialProductData: []
  }
};
const store = mockStore(initialState);
jest.spyOn(store, 'dispatch');
const Wrapper = ({ children }) => <Provider store={store}>{children}</Provider>;

describe('NewCategoryButton text', () => {
  it('should render correct', () => {
    const { container, getByText } = render(
      <Wrapper>
        <NewCategoryButton />
      </Wrapper>
    );
    const button = getByText('New Category');
    expect(button).toBeInTheDocument();
    fireEvent.click(button);

    expect(store.dispatch).toBeCalled();
  });
});
