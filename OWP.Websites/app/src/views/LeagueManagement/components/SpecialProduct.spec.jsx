import { Provider } from 'react-redux';
import { render, fireEvent } from '@testing-library/react';
import configureMockStore from 'redux-mock-store';
import SpecialProduct from './SpecialProduct';
import { filter } from '../constants/SpecialProductConst';

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
    specialProductPending: false,
    specialProductData: []
  }
};
const store = mockStore(initialState);
jest.spyOn(store, 'dispatch');
const Wrapper = ({ children }) => <Provider store={store}>{children}</Provider>;

describe('My Connected React-Redux Component', () => {
  const { getByText } = render(
    <Wrapper>
      <SpecialProduct />
    </Wrapper>
  );
  it('render correct', () => {
    expect(getByText('New Category')).toBeInTheDocument();
  });
});
