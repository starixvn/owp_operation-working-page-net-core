import { useCallback } from 'react';
import { Provider } from 'react-redux';
import { render, fireEvent, screen } from '@testing-library/react';
import configureStore from 'redux-mock-store';
import IsAppliedCell from './IsAppliedCell';
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
        HasRead: true
      }
    }
  }
};
const store = mockStore(initialState);
jest.spyOn(store, 'dispatch');

const Wrapper = ({ children }) => <Provider store={store}>{children}</Provider>;

describe('test IsAppliedCell component', () => {
  const props = {
    data: {},
    rowIndex: 0
  };
  afterAll(() => {
    jest.clearAllMocks();
  });
  it('render correct', () => {
    const { container } = render(
      <Wrapper>
        {' '}
        <IsAppliedCell {...props} />
      </Wrapper>
    );
    expect(container.querySelector('.form-check-input')).toBeInTheDocument();
  });

  it('should call dispatch when click check input', () => {
    const { container } = render(
      <Wrapper>
        <IsAppliedCell {...props} />
      </Wrapper>
    );

    const checkInput = container.querySelector('.form-check-input');

    fireEvent.click(checkInput);

    expect(store.dispatch).toBeCalled();
  });
});
