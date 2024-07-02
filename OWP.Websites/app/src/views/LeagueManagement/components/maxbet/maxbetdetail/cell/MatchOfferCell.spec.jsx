import { useCallback } from 'react';
import Router from 'react-router-dom';
import { Provider } from 'react-redux';
import { render, fireEvent, screen } from '@testing-library/react';
import configureStore from 'redux-mock-store';
import MatchOfferCell from './MatchOfferCell';
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
  },
  matchOffersReducer: {
    matchOffers: [
      { Id: 1, Value: '1s Accept' },
      { Id: 2, Value: '3s Accept' }
    ]
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
    const { getByText } = render(
      <Wrapper>
        <MatchOfferCell {...props} />
      </Wrapper>
    );
    expect(getByText('ABC')).toBeInTheDocument();
  });

  it('should render modal when click', () => {
    const { container } = render(
      <Wrapper>
        <MatchOfferCell {...props} />
      </Wrapper>
    );

    const cell = container.querySelector('.icon-edit');
    fireEvent.click(cell);
    expect(screen.getByText('Save')).toBeInTheDocument();
  });

  it('should close modal when close button', () => {
    const { container, getByText } = render(
      <Wrapper>
        <MatchOfferCell {...props} />
      </Wrapper>
    );
    fireEvent.click(container.querySelector('.icon-edit'));
    fireEvent.click(screen.getByText('Cancel'));
    expect(container.querySelector('.btn-cancel')).not.toBeInTheDocument();
  });

  it('should render modal & handle input change', async () => {
    const { container } = render(
      <Wrapper>
        <MatchOfferCell {...props} />
      </Wrapper>
    );

    const cell = container.querySelector('.icon-edit');
    fireEvent.click(cell);

    const exampleInput = screen.getByText('3s Accept');
    fireEvent.change(exampleInput, { target: { checked: true } });
  });

  it('should render modal when click save call dispatch', () => {
    const { container } = render(
      <Wrapper>
        <MatchOfferCell {...props} />
      </Wrapper>
    );

    const cell = container.querySelector('.icon-edit');
    fireEvent.click(cell);
    fireEvent.click(screen.getByText('Save'));
    expect(store.dispatch).toBeCalled();
  });
});
