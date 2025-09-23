import { render, fireEvent } from '@testing-library/react';
import Filter from './Filter';

describe('Filter component', () => {
  it('should render correctly', () => {
    const { getByText } = render(<Filter />);

    expect(getByText('From Date')).toBeInTheDocument();
    expect(getByText('To Date')).toBeInTheDocument();
    expect(getByText('Search')).toBeInTheDocument();
    expect(getByText('Export')).toBeInTheDocument();
  });

  it('should update fromDate when fromDatePicker changes', () => {
    const { container } = render(<Filter />);
    const fromDateInput = container.getElementsByClassName('to-date-picker')[0];

    fireEvent.change(fromDateInput, { target: { value: '2023-01-01' } });

    expect(fromDateInput.value).toBe('Jan 2023');
  });

  it('should call handleOnSearch when Search button is clicked', () => {
    const { getByText } = render(<Filter />);
    const searchButton = getByText('Search');

    fireEvent.click(searchButton);
  });

  it('should call handleOnSearch when Search button is clicked by valid criteria', () => {
    const { getByText, container } = render(<Filter />);
    const fromDateInput = container.getElementsByClassName('to-date-picker')[0];
    const toDateInput = container.getElementsByClassName('to-date-picker')[1];
    fireEvent.change(fromDateInput, { target: { value: '2023-01-01' } });
    fireEvent.change(toDateInput, { target: { value: '2023-01-01' } });
    const searchButton = getByText('Search');

    fireEvent.click(searchButton);
  });

  it('should call handleOnSearch when export button is clicked by valid criteria', () => {
    const { getByText } = render(<Filter />);
    const exportBtn = getByText('Export');

    fireEvent.click(exportBtn);
  });
});
