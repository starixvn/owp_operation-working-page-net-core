import React from 'react';
import { render, screen, fireEvent } from '@testing-library/react';
import CompareAgent from './CompareAgentIcon';
import CompareAgentPopup from './CompareAgentPopup';

jest.mock('./CompareAgentPopup', () => jest.fn(() => <div data-testid="compare-agent-popup">Compare Agent Popup</div>));

describe('CompareAgent', () => {
  it('should render the CompareAgent component and open the popup on click', () => {
    const { container } = render(<CompareAgent />);

    const svgIcon = container.querySelector('.compare-icon');
    expect(svgIcon).toBeInTheDocument();

    fireEvent.click(svgIcon!);

    const popup = screen.getByTestId('compare-agent-popup');
    expect(popup).toBeInTheDocument();
  });
});
