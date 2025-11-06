import React from 'react';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import '@testing-library/jest-dom';
import Filter from './Filter';
import * as exportHelper from '../../../Shared/common/ExportExcel/Helper';
import { VIEW_BY_OPTION } from '../../Const.json';
import dayjs from 'dayjs';

jest.mock('../../../Shared/common/ExportExcel/Helper', () => ({
  exportExcel: jest.fn()
}));

jest.mock('../../../Shared/DateTime/dateTimeHelper', () => ({
  buildTimeRangeForFromDate: jest.fn(() => ({
    minFromDate: new Date('2023-01-01')
  })),
  buildDefaultFilterDateProviderVerification: jest.fn(
    () => new Date('2025-12-31')
  )
}));

describe('Filter component', () => {
  beforeEach(() => {
    jest.clearAllMocks();
  });

  it('renders View By dropdown and Export button', () => {
    render(<Filter />);
    expect(screen.getByText('View By')).toBeInTheDocument();
    expect(screen.getByText('Export')).toBeInTheDocument();
  });

  it('should export with "by-day" URL and file name when viewBy is "By Day"', () => {
    const exportSpy = jest
      .spyOn(exportHelper, 'exportExcel')
      .mockResolvedValue();

    render(<Filter />);
    fireEvent.click(screen.getByText('Export'));

    expect(exportSpy).toHaveBeenCalled();

    const [calledUrl, fileName] = exportSpy.mock.calls[0];

    expect(calledUrl).toContain('export/by-day');
    expect(fileName).toContain('Monthly_Total_Summaries_');
  });

  it('should export with "by-league" URL and file name when viewBy is "By League"', async () => {
    const exportSpy = jest
      .spyOn(exportHelper, 'exportExcel')
      .mockResolvedValue();

    render(<Filter />);

    const select = screen.getByText('Day');
    fireEvent.mouseDown(select);

    await waitFor(() => {
      expect(screen.getByText('League')).toBeInTheDocument();
    });
    fireEvent.click(screen.getByText('League'));
    fireEvent.click(screen.getByText('Export'));

    expect(exportSpy).toHaveBeenCalled();

    const [calledUrl, fileName] = exportSpy.mock.calls[0];

    expect(calledUrl).toContain('export/by-league');
    expect(fileName).toContain('Soccer_Provider_Usage_Summaries_By_Leagues');
  });

  it('changes View By selection', () => {
    render(<Filter />);

    const select = screen.getByText(VIEW_BY_OPTION[0].label);
    fireEvent.keyDown(select, { key: 'ArrowDown' });

    const newOption = screen.getByText(VIEW_BY_OPTION[1].label);
    fireEvent.click(newOption);

    expect(screen.getByText(VIEW_BY_OPTION[1].label)).toBeInTheDocument();
  });

  it('should update fromDate when a new date is selected', async () => {
    const { container } = render(<Filter />);
    const user = userEvent.setup();

    const fromDateInput = container.querySelector(
      'input.from-date-picker'
    ) as HTMLInputElement;
    if (!fromDateInput) throw new Error('Input not found');

    await user.click(fromDateInput);

    // Click on October 10th, 2025 (a date that exists in the calendar)
    const dayButton = screen.getByRole('button', {
      name: /Choose .* October 10th, 2025/i
    });

    await user.click(dayButton);

    expect(fromDateInput.value).toBe('Oct 10, 2025');
  });

  it('should update toDate when a new date is selected', async () => {
    const { container } = render(<Filter />);
    const user = userEvent.setup();

    const toDateInput = container.querySelector(
      'input.to-date-picker'
    ) as HTMLInputElement;
    if (!toDateInput) throw new Error('Input not found');

    await user.click(toDateInput);

    // Click on November 10th, 2025 (a date that exists in the calendar)
    const dayButton = screen.getByRole('button', {
      name: /Choose .* November 10th, 2025/i
    });

    await user.click(dayButton);

    expect(toDateInput.value).toBe('Nov 10, 2025');
  });

  it('should open datepicker from date when calendar icon is clicked', async () => {
    const { container } = render(<Filter />);
    const user = userEvent.setup();

    const calendarIcon = container.querySelector(
      '#icon-calendar-from-date'
    ) as HTMLElement;
    expect(calendarIcon).toBeInTheDocument();

    await user.click(calendarIcon);

    expect(container.querySelector('.react-datepicker')).toBeInTheDocument();
  });

  it('should open datepicker to date when calendar icon is clicked', async () => {
    const { container } = render(<Filter />);
    const user = userEvent.setup();

    const calendarIcon = container.querySelector(
      '#icon-calendar-to-date'
    ) as HTMLElement;
    expect(calendarIcon).toBeInTheDocument();

    await user.click(calendarIcon);

    expect(container.querySelector('.react-datepicker')).toBeInTheDocument();
  });
});
