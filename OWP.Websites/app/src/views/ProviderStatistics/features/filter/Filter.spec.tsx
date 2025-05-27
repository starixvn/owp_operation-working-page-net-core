import React from 'react';
import { render, screen, fireEvent } from '@testing-library/react';
import Filter from './Filter';
import * as exportHelper from '../../../Shared/common/ExportExcel/Helper';
import { VIEW_BY_OPTION } from '../../Const.json';

// Mock exportExcel to track calls
jest.mock('../../../Shared/common/ExportExcel/Helper', () => ({
  exportExcel: jest.fn(),
}));

// Mock helper functions if needed
jest.mock('../../../Shared/DateTime/dateTimeHelper', () => ({
  buildTimeRangeForFromDate: jest.fn(() => ({
    minFromDate: new Date('2023-01-01'),
  })),
  buildDefaultFilterDateProviderVerification: jest.fn(() => new Date('2025-12-31')),
}));

describe('Filter component', () => {
  it('renders View By dropdown and Export button', () => {
    render(<Filter />);
    expect(screen.getByText('View By')).toBeInTheDocument();
    expect(screen.getByText('Export')).toBeInTheDocument();
  });

  it('calls exportExcel with correct params on Export click', () => {
    render(<Filter />);

    // Click export button
    fireEvent.click(screen.getByText('Export'));

    // Assert exportExcel was called
    expect(exportHelper.exportExcel).toHaveBeenCalledTimes(1);

    const [calledUrl, calledFileName] = (exportHelper.exportExcel as jest.Mock).mock.calls[0];

    expect(calledUrl).toMatch(/export\/by-day/);
    expect(calledFileName).toMatch(/Monthly_Total_Summaries_/);
  });

  it('changes View By selection', () => {
    render(<Filter />);

    const select = screen.getByText(VIEW_BY_OPTION[0].label);
    fireEvent.keyDown(select, { key: 'ArrowDown' });

    const newOption = screen.getByText(VIEW_BY_OPTION[1].label);
    fireEvent.click(newOption);

    expect(screen.getByText(VIEW_BY_OPTION[1].label)).toBeInTheDocument();
  });
});
