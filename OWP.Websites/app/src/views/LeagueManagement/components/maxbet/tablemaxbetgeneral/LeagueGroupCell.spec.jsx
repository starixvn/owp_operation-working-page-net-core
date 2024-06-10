import { Provider } from 'react-redux';
import { render, fireEvent, act } from '@testing-library/react';
import configureStore from 'redux-mock-store';
import LeagueGroupCell from './LeagueGroupCell';
import ToastsWrapper from '../../../../components/ToastrWrapper';
import '@testing-library/jest-dom';

jest.mock('../../../../components/ToastrWrapper');
ToastsWrapper.show = jest.fn().mockReturnValue({ value: true });

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

describe('test LeagueGroupCell component', () => {
  afterAll(() => {
    jest.clearAllMocks();
  });

  it('should render LeagueGroupCell component', () => {
    const props = {
      dataSource: [{ OWPLeagueId: 1, IsActive: true }],
      rowIndex: 0
    };
    const { container } = render(
      <Wrapper>
        <LeagueGroupCell {...props} />
      </Wrapper>
    );
    expect(
      container.querySelector('.fixedDataTableCellLayout_wrap1')
    ).toBeInTheDocument();
  });

  it('should change select', async () => {
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
    const { container } = render(
      <Wrapper>
        <LeagueGroupCell {...props} />
      </Wrapper>
    );

    const select = container.querySelector('select');
    fireEvent.change(select);
    await act(async () => {
      await Promise.resolve();
    })
    expect(store.dispatch).toBeCalledTimes(1);
  });
});
