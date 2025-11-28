import { render, screen } from '@testing-library/react';
import { useAppSelector } from '../app/hooks';
import EventPickingTable from './EventPickingTable';
import { REQUEST_STATUS } from '../../Shared/Constants';
import Loading from '../../components/Loading';

jest.mock('../app/hooks', () => ({
  useAppSelector: jest.fn()
}));

jest.mock('../../components/Loading', () => {
  return jest.fn(() => <div data-testid='loading'>Loading</div>);
});

jest.mock('./tableHeader/TableHeader', () => {
  return jest.fn(() => <thead data-testid='table-header'>TableHeader</thead>);
});

jest.mock('./tableBody/TableBody', () => {
  return jest.fn(() => <tbody data-testid='table-body'>TableBody</tbody>);
});

jest.mock('../../Shared/tableCommon/CustomTable', () => {
  return jest.fn(
    ({ children, tableHeader, data, classNameTable, itemRowHeight }) => (
      <table
        className={classNameTable}
        data-testid='custom-table'
        data-item-row-height={itemRowHeight}>
        {tableHeader}
        {children}
      </table>
    )
  );
});

const mockedUseAppSelector = useAppSelector as jest.Mock;

describe('EventPickingTable under test', () => {
  beforeEach(() => {
    jest.clearAllMocks();
  });

  it('should render table successfully when loading is idle', () => {
    mockedUseAppSelector.mockImplementation(selectorFn =>
      selectorFn({
        eventPickingTable: {
          entities: [],
          loading: REQUEST_STATUS.idle
        }
      })
    );

    render(<EventPickingTable />);

    expect(screen.getByTestId('custom-table')).toBeInTheDocument();
    expect(screen.getByTestId('table-header')).toBeInTheDocument();
    expect(screen.getByTestId('table-body')).toBeInTheDocument();
    expect(screen.queryByTestId('loading')).not.toBeInTheDocument();
  });

  it('should show Loading component when loading is pending', () => {
    mockedUseAppSelector.mockImplementation(selectorFn =>
      selectorFn({
        eventPickingTable: {
          entities: [],
          loading: REQUEST_STATUS.pending
        }
      })
    );

    render(<EventPickingTable />);

    expect(screen.getByTestId('loading')).toBeInTheDocument();
    expect(Loading).toHaveBeenCalled();
  });

  it('should render table with correct className', () => {
    mockedUseAppSelector.mockImplementation(selectorFn =>
      selectorFn({
        eventPickingTable: {
          entities: [],
          loading: REQUEST_STATUS.idle
        }
      })
    );

    render(<EventPickingTable />);

    const customTable = screen.getByTestId('custom-table');
    expect(customTable).toHaveClass('table-event-picking');
  });
});
