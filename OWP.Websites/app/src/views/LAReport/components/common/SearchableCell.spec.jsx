import SearchableCell from './SearchableCell';
import { fireEvent, render, screen } from '@testing-library/react';

describe('SearchableCell under test', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });
  it('SearchableCell should have FilterIcon for click', () => {
    const mockOnSearchCallBack = jest.fn();
    const mockOnClearAllFiltersCallBack = jest.fn();

    const { container } = render(
      <SearchableCell
        columnId='columnId'
        columnName='columnName'
        onSearch={mockOnSearchCallBack}
        onClearAllFilters={mockOnClearAllFiltersCallBack}
        isClearALL={false}
      />
    );
    const tableElement = container.querySelector('#columnId');
    fireEvent.click(tableElement);
    const search = screen.getByText('Search');
    fireEvent.click(search);
    expect(search).toBeInTheDocument();
    expect(tableElement).toBeInTheDocument();
  });
});
