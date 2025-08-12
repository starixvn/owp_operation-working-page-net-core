import React from 'react';
import LiveGameListTable from './LiveGameListTable';
import { useSelector, useDispatch } from 'react-redux';
import { fakeEntities } from './tableSlice/tableSliceConst';
import { render, waitFor } from '@testing-library/react';

jest.mock('react-redux', () => ({
  useSelector: jest.fn(),
  useDispatch: jest.fn(),
  createSelectorHook: jest.fn()
}));

// Mock the store that MatchCell imports
jest.mock('../app/storeNative', () => ({
  default: {
    getState: () => ({
      tradingPageInfo: {
        info: {
          laTradingPage: 'http://localhost:3000',
          tradingPage: 'http://localhost:3001',
          servers: [
            {
              id: 1,
              name: 'Server 1',
              url: 'http://localhost:3000',
              laPort: 3000,
              tradingPort: 3001
            }
          ]
        },
        serverSetingId: 1
      }
    })
  }
}));
const mockedDispatch = jest.fn();

(useSelector as jest.Mock).mockImplementation(selectorFn =>
  selectorFn({
    events: {
      entities: fakeEntities,
      filterState: [],
      sortState: {
        columnKey: 'KO Time',
        orderType: '',
      },
      matchesSelected: []
    },
    pagePolicy: {
      permission: {
        HasWrite: true
      }
    }
  })
);
(useDispatch as jest.Mock).mockReturnValue(mockedDispatch);

const { ResizeObserver } = window;

describe('test LiveGameListTable', () => {
  beforeEach(() => {
    //@ts-ignore
    delete window.ResizeObserver;
    window.ResizeObserver = jest.fn().mockImplementation(() => ({
      observe: jest.fn(),
      unobserve: jest.fn(),
      disconnect: jest.fn()
    }));
  });
  afterEach(() => {
    window.ResizeObserver = ResizeObserver;
    jest.restoreAllMocks();
  });

  it('should render table content, ReactTable', async () => {
    const { container } = render(<LiveGameListTable />);
    await waitFor(() => {
      expect(container.querySelector('.table-content')).toBeInTheDocument;
    });
  });
});
