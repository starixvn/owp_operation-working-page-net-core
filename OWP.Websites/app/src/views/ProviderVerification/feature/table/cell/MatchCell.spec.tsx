import React from 'react';
import { render } from '@testing-library/react';
import MatchCell from './MatchCell';
import { TMatchCell } from '../../../type/table';

describe('MatchCell', () => {
  const defaultProps: TMatchCell = {
    className: 'test-class',
    home: 'Team A',
    away: 'Team B'
  };

  it('should render with the correct class name', () => {
    const { container } = render(<MatchCell {...defaultProps} />);
    expect(container.querySelector('div')).toHaveClass('test-class');
  });

  it('should render the home team name', () => {
    const { getByText } = render(<MatchCell {...defaultProps} />);
    expect(getByText('Team A')).toBeInTheDocument();
  });

  it('should render the away team name', () => {
    const { getByText } = render(<MatchCell {...defaultProps} />);
    expect(getByText('Team B')).toBeInTheDocument();
  });

  it('should render home and away team names in separate spans', () => {
    const { container } = render(<MatchCell {...defaultProps} />);
    const spans = container.querySelectorAll('span');
    expect(spans).toHaveLength(2);
    expect(spans[0]).toHaveClass('home');
    expect(spans[1]).toHaveClass('away');
  });
});
