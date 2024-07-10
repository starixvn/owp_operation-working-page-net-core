import { fireEvent, render, screen } from '@testing-library/react';
import React from 'react';
import SummarizeProviderIcon from './SummarizeProviderIcon';

jest.mock('./SummarizeProviderTable', () => jest.fn(() => <div data-testid="summarize-provider-table">Summarize Provider Table</div>));
jest.mock('reactstrap', () => ({
  Tooltip: jest.fn(({ isOpen, target, children }) => (isOpen ? <div data-testid={`tooltip-${target}`}>{children}</div> : null))
}));

describe('SummarizeProviderIcon', () => {
  it('should render the SummarizeProviderIcon component and toggle the tooltip on click', () => {
    const { container } = render(<SummarizeProviderIcon />);

    const icon = container.querySelector('.sum-provider');
    expect(icon).toBeInTheDocument();

    fireEvent.click(icon!);

    const tooltip = screen.getByTestId('tooltip-icon-summarize');
    expect(tooltip).toBeInTheDocument();
    expect(screen.getByTestId('summarize-provider-table')).toBeInTheDocument();

    fireEvent.click(icon!);

    expect(screen.queryByTestId('tooltip-icon-summarize')).not.toBeInTheDocument();
  });

  it('should hide the tooltip when clicking outside', () => {
    const { container } = render(<SummarizeProviderIcon />);

    const icon = container.querySelector('.sum-provider');
    fireEvent.click(icon!);

    const tooltip = screen.getByTestId('tooltip-icon-summarize');
    expect(tooltip).toBeInTheDocument();
    expect(screen.getByTestId('summarize-provider-table')).toBeInTheDocument();

    fireEvent.mouseDown(document);
    fireEvent.click(document);

    expect(screen.queryByTestId('tooltip-icon-summarize')).not.toBeInTheDocument();
  });
});
