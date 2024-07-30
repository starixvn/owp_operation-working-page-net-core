import { fireEvent, render } from '@testing-library/react';
import { useDispatch, useSelector } from 'react-redux';
import FilterHeader from './FilterHeader';

jest.mock('react-redux', () => ({
  useSelector: jest.fn(),
  useDispatch: jest.fn()
}));

const mockDispatch = jest.fn();
useDispatch.mockReturnValue(mockDispatch);
const setUp = isClearAll => {
  useSelector.mockImplementation(selectorFn =>
    selectorFn({
      reportReducer: {
        pending: false,
        isDisplayTable: true,
        isClearAll
      }
    })
  );
};

describe('FilterHeader under test', () => {
  it('FilterHeader should have search icon when click', () => {
    setUp(false);
    const { container } = render(
      <FilterHeader columnId='columnId' columnName='columnName' />
    );
    const tableElement = container.querySelector('#columnId');
    fireEvent.click(tableElement);
    const search = container.querySelector('.btn-search');
    fireEvent.click(search);
    expect(search).toBeInTheDocument();
  });
});
