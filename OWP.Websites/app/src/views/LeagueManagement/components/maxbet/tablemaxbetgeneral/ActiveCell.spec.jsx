import { useCallback } from 'react';
import Router from 'react-router-dom';
import { Provider } from 'react-redux';
import { render, fireEvent, screen } from '@testing-library/react';
import configureStore from 'redux-mock-store';
import ActiveCell from './ActiveCell';
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

describe('test ActiveCell component', () => {
  afterAll(() => {
    jest.clearAllMocks();
  });

  const props = {
    dataSource: [{ OWPLeagueId: 1, IsActive: true }],
    rowIndex: 0
  };

  it('should render ActiveCell component', () => {
    const { getByText, container } = render(
      <Wrapper>
        <ActiveCell {...props} />
      </Wrapper>
    );
    expect(
      container.querySelector('.fixedDataTableCellLayout_wrap1')
    ).toBeInTheDocument();
  });

  it('should handle checked input', () => {
    const { getByText, container } = render(
      <Wrapper>
        <ActiveCell {...props} />
      </Wrapper>
    );
    const input = container.querySelector('.form-check-input');
    fireEvent.click(input);

    expect(store.dispatch).toBeCalled();
  });
});
