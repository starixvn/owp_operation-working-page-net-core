import { useCallback } from 'react';
import Router from 'react-router-dom';
import { Provider } from 'react-redux';
import { render, fireEvent, screen } from '@testing-library/react';
import configureStore from 'redux-mock-store';
import LstNoteCell from './LstNoteCell';
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

describe('test LstNoteCell component', () => {
  const props = {
    data: {
      LSTNote: 'ABC'
    },
    rowIndex: 0
  };
  afterAll(() => {
    jest.clearAllMocks();
  });
  it('render correct', () => {
    const { container } = render(
      <Wrapper>
        <LstNoteCell {...props} />
      </Wrapper>
    );
    expect(container.querySelector('#LstNote0')).toBeInTheDocument();
  });

  it('should render input when click', () => {
    const { container } = render(
      <Wrapper>
        <LstNoteCell {...props} />
      </Wrapper>
    );

    const cell = container.querySelector('#LstNote0');
    fireEvent.click(cell);
    expect(container.querySelector('.input-field')).toBeInTheDocument();
  });

  it('should render input when click', () => {
    const { container } = render(
      <Wrapper>
        <LstNoteCell {...props} />
      </Wrapper>
    );
    fireEvent.click(container.querySelector('#LstNote0'));

    const input = container.querySelector('.input-field');
    fireEvent.change(input, { target: { value: 'ABZ' } });
    fireEvent.blur(input);

    expect(store.dispatch).toBeCalled();
  });
});
