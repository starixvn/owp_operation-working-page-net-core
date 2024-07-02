import { render, screen } from '@testing-library/react';
import { useDispatch, useSelector } from 'react-redux';
import { REQUEST_STATUS } from '../../../Shared/Constants';
import TaskListTable from './TaskListTable';
import { useAppSelector, useAppDispatch } from '../../app/hooks';

jest.mock('react-redux', () => ({
  useSelector: jest.fn()
}));

jest.mock('../../app/hooks', () => ({
  useAppSelector: jest.fn(),
  useAppDispatch: jest.fn()
}));

describe('TaskListTable', () => {
  beforeEach(() => {
    (useSelector as unknown as jest.Mock).mockClear();
  });

  test('renders loading component when loading status is pending', () => {
    const mockMatches = [];
    const mockLoading = REQUEST_STATUS.pending;
    const mockHasDetailInformationPermission = true;

    (useSelector as unknown as jest.Mock).mockReturnValue({
      matches: mockMatches,
      loading: mockLoading,
      hasDetailInformationPermission: mockHasDetailInformationPermission
    });

    const { container } = render(<TaskListTable />);

    expect(container.querySelector('.loading-process')).toBeInTheDocument();
    expect(useAppSelector).not.toBeCalled();
    expect(useAppDispatch).not.toBeCalled();
  });
});
