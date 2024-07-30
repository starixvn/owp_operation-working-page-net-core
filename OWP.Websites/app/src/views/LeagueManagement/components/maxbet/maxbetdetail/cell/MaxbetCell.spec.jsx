import { useCallback } from 'react';
import Router from 'react-router-dom';
import { Provider } from 'react-redux';
import { render, fireEvent, screen } from '@testing-library/react';
import configureStore from 'redux-mock-store';
import MaxbetCell from './MaxbetCell';
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
        HasRead: true,
        HasWrite: true
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

describe('test MatchOfferCell component', () => {
  const props = {
    data: {
      aa: 'ABC'
    },
    rowIndex: 0,
    modelKey: 'aa'
  };
  afterAll(() => {
    jest.clearAllMocks();
  });
  it('render correct', () => {
    const { container } = render(
      <Wrapper>
        <MaxbetCell {...props} />
      </Wrapper>
    );
    expect(container.querySelector('.input-value')).toBeInTheDocument();
  });

  it('should call dispatch when blur input', () => {
    const { container } = render(
      <Wrapper>
        <MaxbetCell {...props} />
      </Wrapper>
    );
    const input = container.querySelector('.input-field');

    fireEvent.change(input, { target: { value: 1000 } });
    fireEvent.blur(input);

    expect(store.dispatch).toBeCalled();
  });
});
