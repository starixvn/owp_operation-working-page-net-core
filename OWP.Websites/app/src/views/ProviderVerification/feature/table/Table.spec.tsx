import React from 'react';
import { render, screen } from '@testing-library/react';
import { useAppSelector } from '../../app/hookNative';
import { REQUEST_STATUS } from '../../../Shared/Constants';
import Table from './Table';

jest.mock('../../app/hookNative');
jest.mock('../../../Shared/customHook/useLayoutTable', () => () => ({
  maxHeight: '500px',
  maxWidth: '1000px'
}));
jest.mock('../../../components/Loading', () => () => <div>Loading...</div>);
jest.mock('./TableHeader', () => () => <thead><tr><th>Header</th></tr></thead>);
jest.mock('./TableBody', () => ({ dataSource }) => (
  <tbody>
    {dataSource.map((event, index) => (
      <tr key={index}><td>{event.MatchNo}</td></tr>
    ))}
  </tbody>
));

describe('Table component', () => {
  const mockUseAppSelector = useAppSelector as jest.Mock;

  it('renders Loading component when data is loading', () => {
    mockUseAppSelector.mockReturnValue({ dataSource: [], loadingDataTable: REQUEST_STATUS.pending });

    render(<Table />);

    expect(screen.getByText('Loading...')).toBeInTheDocument();
  });

  it('renders TableHeader and TableBody when data is loaded', () => {
    const mockDataSource = [
      { OWPMatchId: 1, MatchNo: '1' },
      { OWPMatchId: 2, MatchNo: '2' }
    ];
    mockUseAppSelector.mockReturnValue({ dataSource: mockDataSource, loadingDataTable: REQUEST_STATUS.idle });

    render(<Table />);

    expect(screen.getByText('Header')).toBeInTheDocument();
    expect(screen.getByText('Total record: 2')).toBeInTheDocument();
    expect(screen.getByText('1')).toBeInTheDocument();
    expect(screen.getByText('2')).toBeInTheDocument();
  });

  it('applies correct styles to the table wrapper', () => {
    mockUseAppSelector.mockReturnValue({ dataSource: [], loadingDataTable: REQUEST_STATUS.idle });

    const { container } = render(<Table />);
    const tableWrap = container.querySelector('.table-wrap');

    expect(tableWrap).toHaveStyle('maxHeight: 500px');
    expect(tableWrap).toHaveStyle('maxWidth: 1000px');
    expect(tableWrap).toHaveStyle('minHeight: 450px');
  });
});
