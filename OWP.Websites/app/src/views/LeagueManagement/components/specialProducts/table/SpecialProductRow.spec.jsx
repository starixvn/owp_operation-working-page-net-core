import { useCallback } from 'react';
import { Provider } from 'react-redux';
import configureMockStore from 'redux-mock-store';
import { render, fireEvent, screen } from '@testing-library/react';
import SpecialProductRow from './SpecialProductRow';

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

describe('SpecialProductRow text', () => {
  const props = {
    data: {
      Maxbet_FTHDP: 100
    }
  };
  it('should render correct', () => {
    const { container } = render(
      <Wrapper>
        <SpecialProductRow {...props} />
      </Wrapper>
    );
    expect(container.querySelector('.input-value')).toBeInTheDocument();
  });
});
