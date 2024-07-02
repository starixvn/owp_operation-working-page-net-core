import { render, fireEvent } from '@testing-library/react';
import RenderHeaderCell from './renderHeader';
import { useDispatch } from 'react-redux';

jest.mock('react-redux', () => ({
  useDispatch: jest.fn()
}));

const mockDispatch = jest.fn();
useDispatch.mockReturnValue(mockDispatch);
const mockFunction = jest.fn();
describe('renderHeader under test', () => {
  it('when isCanSort = true, renderHeaderCell should render sort header ', () => {
    const normalHeaderModel = {
      headerName: 'League/Match Name',
      columnKey: 'LeagueName',
      sortState: {},
      isCanSort: true,
      sortFunc: mockFunction
    };

    const { container } = render(<RenderHeaderCell {...normalHeaderModel} />);
    fireEvent.click(container.querySelector('.sort-icon'));
    expect(mockFunction).toHaveBeenCalledTimes(1);
  });
});
