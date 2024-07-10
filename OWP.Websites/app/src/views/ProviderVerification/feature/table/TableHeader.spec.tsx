import React from 'react';
import { render, screen } from '@testing-library/react';
import TableHeader from './TableHeader';
import { TColumnKey } from '../../type/table';
import { TABLE_HEADER } from './TableSchema.json';

jest.mock('./MultipleFilterHeader', () => ({ columnKey }: { columnKey: TColumnKey }) => (
  <div data-testid={`filter-header-${columnKey}`}>Filter for {columnKey}</div>
));

describe('TableHeader', () => {
  it('renders correctly with headers and filters', () => {
    render(<table><TableHeader /></table>);

    TABLE_HEADER.forEach((item, index) => {
      const headerElement = screen.getByText(item.HEADER_NAME);
      expect(headerElement).toBeInTheDocument();

      if (item.IS_FILTER) {
        const filterHeaderElement = screen.getByTestId(`filter-header-${item.HEADER_KEY}`);
        expect(filterHeaderElement).toBeInTheDocument();
      } else {
        expect(screen.queryByTestId(`filter-header-${item.HEADER_KEY}`)).toBeNull();
      }
    });
  });

  it('applies the correct width styles', () => {
    render(<table><TableHeader /></table>);

    TABLE_HEADER.forEach((item, index) => {
      const headerElement = screen.getAllByRole('columnheader')[index];
      expect(headerElement).toHaveStyle(`width: ${item.WIDTH}`);
    });
  });
});
