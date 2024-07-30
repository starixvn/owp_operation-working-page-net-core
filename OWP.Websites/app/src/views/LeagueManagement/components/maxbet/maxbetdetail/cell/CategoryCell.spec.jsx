import { useCallback } from 'react';
import Router from 'react-router-dom';
import { Provider } from 'react-redux';
import { render, fireEvent, screen } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import configureStore from 'redux-mock-store';
import CategoryCell from './CategoryCell';
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
    leagueGroupOption: [{ value: '1', label: '12' }],
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

describe('test CategoryCell component', () => {
  afterAll(() => {
    jest.clearAllMocks();
  });

  it('should render CategoryCell component', () => {
    const props = {
      data: { OWPLeagueId: 1, IsActive: true, Category: 'Live' },
      rowIndex: 0
    };
    const { getByText, container } = render(
      <Wrapper>
        <CategoryCell {...props} />
      </Wrapper>
    );
    expect(
      container.querySelector('.fixedDataTableCellLayout_wrap1')
    ).toBeInTheDocument();
  });

  it('should handle Change LeagueGroup', async () => {
    const props = {
      data: { OWPLeagueId: 1, IsActive: true, Category: 'Live' },
      rowIndex: 0
    };
    const { container } = render(
      <Wrapper>
        <CategoryCell {...props} />
      </Wrapper>
    );

    const select = container.querySelector('select');
    fireEvent.change(select);
    expect(store.dispatch).toBeCalledTimes(1);
  });
});
