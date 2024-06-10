import { useCallback } from 'react';
import Router from 'react-router-dom';
import { Provider } from 'react-redux';
import { render, fireEvent, screen } from '@testing-library/react';
import configureStore from 'redux-mock-store';
import TimeCell from './TimeCell';
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

describe('test TimeCell component', () => {
  const props = {
    data: {
      TimeDesc: 'abc'
    },
    rowIndex: 0
  };
  afterAll(() => {
    jest.clearAllMocks();
  });
  it('render correct', () => {
    const { container, getByText } = render(
      <Wrapper>
        <TimeCell {...props} />
      </Wrapper>
    );
    expect(getByText('abc')).toBeInTheDocument();
  });

  it('should render input when click cell', () => {
    const { container } = render(
      <Wrapper>
        <TimeCell {...props} />
      </Wrapper>
    );
    fireEvent.click(container.querySelector('.col-time'));

    const input = container.querySelector('.input-field');
    expect(input).toBeInTheDocument();

    fireEvent.change(input, { target: { value: 'av' } });
    fireEvent.blur(input);

    expect(store.dispatch).toBeCalled();
  });
});
