import React from 'react';
import { render, screen } from '@testing-library/react';
import { Provider } from 'react-redux';
import configureStore from 'redux-mock-store';
import thunk from 'redux-thunk';
import SummarizeProviderTable from './SummarizeProviderTable';
import { sumProvider } from '../summarizeProviderSlice';
import { TDataSource, Option } from '../../type/table';
import { TProviderCount } from '../../../type/provider';

const middlewares = [thunk];
const mockStore = configureStore(middlewares);

describe('SummarizeProviderTable', () => {
  let store: any;

  beforeEach(() => {
    store = mockStore({
      events: {
        dataSource: [] as TDataSource[],
        confirmProviderOption: [] as Option[],
      },
      providerSum: {
        sumTable: [] as TProviderCount[],
      },
    });

    store.dispatch = jest.fn();
  });

  it('should render the SummarizeProviderTable component', () => {
    render(
      <Provider store={store}>
        <SummarizeProviderTable />
      </Provider>
    );

    // Check that the table is rendered
    const table = screen.getByRole('table');
    expect(table).toBeInTheDocument();
  });

  it('should dispatch sumProvider action on mount', () => {
    render(
      <Provider store={store}>
        <SummarizeProviderTable />
      </Provider>
    );

    expect(store.dispatch).toHaveBeenCalledWith(sumProvider({
      data: [],
      agentOptions: []
    }));
  });

  it('should display summarized data in the table', () => {
    const mockData = [
      { agentName: 'Agent1', count: 2 },
      { agentName: 'Agent2', count: 3 }
    ];

    store = mockStore({
      events: {
        dataSource: [] as TDataSource[],
        confirmProviderOption: [] as Option[],
      },
      providerSum: {
        sumTable: mockData,
      },
    });

    render(
      <Provider store={store}>
        <SummarizeProviderTable />
      </Provider>
    );

    // Check that the data is displayed in the table
    expect(screen.getByText('Agent1')).toBeInTheDocument();
    expect(screen.getByText('2')).toBeInTheDocument();
    expect(screen.getByText('Agent2')).toBeInTheDocument();
    expect(screen.getByText('3')).toBeInTheDocument();
  });

  it('should calculate and display the total sum correctly', () => {
    const mockData = [
      { agentName: 'Agent1', count: 2 },
      { agentName: 'Agent2', count: 3 }
    ];

    store = mockStore({
      events: {
        dataSource: [] as TDataSource[],
        confirmProviderOption: [] as Option[],
      },
      providerSum: {
        sumTable: mockData,
      },
    });

    render(
      <Provider store={store}>
        <SummarizeProviderTable />
      </Provider>
    );

    // Check that the total sum is calculated and displayed correctly
    expect(screen.getByText('Total')).toBeInTheDocument();
    expect(screen.getByText('5')).toBeInTheDocument();
  });
});
