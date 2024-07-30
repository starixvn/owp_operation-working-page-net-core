import { useCallback } from 'react';
import Router from 'react-router-dom';
import { Provider } from 'react-redux';
import { render, fireEvent, screen } from '@testing-library/react';
import configureStore from 'redux-mock-store';
import TableLeagueDetail from './TableLeagueDetail';
import '@testing-library/jest-dom';

const middleWares = [];
const mockStore = configureStore(middleWares);

jest.mock('react-router-dom', () => ({
  ...jest.requireActual('react-router-dom'),
  useParams: jest.fn(),
}));

const initialState = {
  toggleColumnReducer: {
    columnKeys: 'A',
  },
  leagueDetailReducer: {
    leagueDetailPending: 'true',
  },
  policyReducer: {
    policyData: {
      tvListPolicy: {
        HasRead: true,
      },
      generalPolicy: {
        Username: 'sa',
        HasRead: true,
      },
      conditionPolicy: {
        HasRead: true,
      },
      lstNotePolicy: {
        HasRead: true,
      },
    },
  },
};
const store = mockStore(initialState);
jest.spyOn(store, 'dispatch');

const Wrapper = ({ children }) => <Provider store={store}>{children}</Provider>;
const { ResizeObserver } = window;
describe('test TableLeagueDetail component', () => {
  const props = {
    containerWidth: 111,
  };
  beforeEach(() => {
    jest.spyOn(Router, 'useParams').mockReturnValue({ id: 13 });
    //@ts-ignore
    delete window.ResizeObserver;
    window.ResizeObserver = jest.fn().mockImplementation(() => ({
      observe: jest.fn(),
      unobserve: jest.fn(),
      disconnect: jest.fn(),
    }));
  });
  afterEach(() => {
    window.ResizeObserver = ResizeObserver;
    jest.restoreAllMocks();
  });

  it('render correct loading component', () => {
    const initialState = {
      toggleColumnReducer: {
        columnKeys: 'A',
      },
      leagueDetailReducer: {
        leagueDetailPending: 'true',
      },
      policyReducer: {
        policyData: {
          tvListPolicy: {
            HasRead: true,
          },
          generalPolicy: {
            Username: 'sa',
            HasRead: true,
          },
          conditionPolicy: {
            HasRead: true,
          },
          lstNotePolicy: {
            HasRead: true,
          },
        },
      },
      matchOffersReducer: {
        matchOffers: [{ Id: 1, Value: '1s Accept' }],
      },
    };
    const store = mockStore(initialState);
    jest.spyOn(store, 'dispatch');

    const Wrapper = ({ children }) => (
      <Provider store={store}>{children}</Provider>
    );
    const { container } = render(
      <Wrapper>
        <TableLeagueDetail {...props} />
      </Wrapper>
    );
    expect(container.querySelector('.loading-process')).toBeInTheDocument();
  });

  it('render correct table component', () => {
    const initialState = {
      toggleColumnReducer: {
        columnKeys: ['A'],
      },
      leagueDetailReducer: {
        leagueDetailPending: false,
      },
      policyReducer: {
        policyData: {
          tvListPolicy: {
            HasRead: true,
          },
          generalPolicy: {
            Username: 'sa',
            HasRead: true,
          },
          conditionPolicy: {
            HasRead: true,
          },
          lstNotePolicy: {
            HasRead: true,
          },
        },
      },
      matchOffersReducer: {
        matchOffers: [{ Id: 1, Value: '1s Accept' }],
      },
    };
    const store = mockStore(initialState);
    jest.spyOn(store, 'dispatch');

    const Wrapper = ({ children }) => (
      <Provider store={store}>{children}</Provider>
    );
    const { container } = render(
      <Wrapper>
        <TableLeagueDetail {...props} />
      </Wrapper>
    );
    expect(container.querySelector('.table')).toBeInTheDocument();
  });
});
