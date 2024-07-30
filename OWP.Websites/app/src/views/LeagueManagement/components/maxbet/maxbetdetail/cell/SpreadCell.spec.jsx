import { useCallback } from 'react';
import Router from 'react-router-dom';
import { Provider } from 'react-redux';
import { render, fireEvent, screen } from '@testing-library/react';
import configureStore from 'redux-mock-store';
import SpreadCell from './SpreadCell';
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

describe('test SpreadCell component', () => {
  const props = {
    data: {
      spread: 1
    },
    modelKey: 'spread',
    rowIndex: 0
  };
  afterAll(() => {
    jest.clearAllMocks();
  });
  it('render correct', () => {
    const { container, getByText } = render(
      <Wrapper>
        <SpreadCell {...props} />
      </Wrapper>
    );
    expect(getByText('%')).toBeInTheDocument();
  });

  it('should render input when click cell', () => {
    const { container } = render(
      <Wrapper>
        <SpreadCell {...props} />
      </Wrapper>
    );

    const input = container.querySelector('.input-field');
    expect(input).toBeInTheDocument();

    fireEvent.change(input, { target: { value: 2 } });
    fireEvent.blur(input);

    expect(store.dispatch).toBeCalled();
  });
});
