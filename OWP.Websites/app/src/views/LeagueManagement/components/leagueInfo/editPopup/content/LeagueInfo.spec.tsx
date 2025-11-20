import React from 'react';
import { render, screen } from '@testing-library/react';
import '@testing-library/jest-dom';
import LeagueInfo from './LeagueInfo';

describe('LeagueInfo Component', () => {
  const defaultProps = {
    league: 'Premier League',
    shortLeague: 'PL',
    isBlackList: false
  };

  describe('Rendering', () => {
    it('should render all sections correctly', () => {
      render(<LeagueInfo {...defaultProps} />);

      expect(screen.getByText('League Information')).toBeInTheDocument();
      expect(screen.getByText('League Name')).toBeInTheDocument();
      expect(screen.getByText('Short League')).toBeInTheDocument();
      expect(screen.getByText('League Type')).toBeInTheDocument();
      expect(screen.getByText('Blacklist')).toBeInTheDocument();
    });

    it('should display league name correctly', () => {
      render(<LeagueInfo {...defaultProps} />);

      expect(screen.getByText('Premier League')).toBeInTheDocument();
    });

    it('should display short league correctly', () => {
      render(<LeagueInfo {...defaultProps} />);

      expect(screen.getByText('PL')).toBeInTheDocument();
    });

    it('should render required indicator for league name', () => {
      render(<LeagueInfo {...defaultProps} />);

      const requiredIndicator = screen.getByText('*');
      expect(requiredIndicator).toBeInTheDocument();
      expect(requiredIndicator).toHaveStyle({ color: 'red' });
    });
  });

  describe('Checkbox State', () => {
    it('should render checkbox as unchecked when isBlackList is false', () => {
      render(<LeagueInfo {...defaultProps} />);

      const checkbox = screen.getByRole('checkbox');
      expect(checkbox).not.toBeChecked();
      expect(checkbox).toBeDisabled();
    });

    it('should render checkbox as checked when isBlackList is true', () => {
      render(<LeagueInfo {...defaultProps} isBlackList={true} />);

      const checkbox = screen.getByRole('checkbox');
      expect(checkbox).toBeChecked();
      expect(checkbox).toBeDisabled();
    });

    it('should have correct checkbox attributes', () => {
      render(<LeagueInfo {...defaultProps} />);

      const checkbox = screen.getByRole('checkbox');
      expect(checkbox).toHaveAttribute('id', 'chb-blacklist-league');
      expect(checkbox).toHaveAttribute('name', 'oldLeague');
      expect(checkbox).toHaveAttribute('type', 'checkbox');
    });
  });

  describe('Props Variations', () => {
    it('should handle empty string values', () => {
      render(<LeagueInfo league='' shortLeague='' isBlackList={false} />);

      expect(screen.getByText('League Information')).toBeInTheDocument();
    });

    it('should handle long league names', () => {
      const longName = 'Very Long League Name That Might Wrap';
      render(<LeagueInfo league={longName} shortLeague='VLN' isBlackList={false} />);

      expect(screen.getByText(longName)).toBeInTheDocument();
    });

    it('should handle special characters in league name', () => {
      render(<LeagueInfo league='League & Co.' shortLeague='L&C' isBlackList={false} />);

      expect(screen.getByText('League & Co.')).toBeInTheDocument();
    });
  });

  describe('Component Structure', () => {
    it('should have correct CSS classes', () => {
      const { container } = render(<LeagueInfo {...defaultProps} />);

      expect(container.querySelector('.list')).toBeInTheDocument();
      expect(container.querySelector('.title')).toBeInTheDocument();
      expect(container.querySelectorAll('.content').length).toBe(3);
      expect(container.querySelector('.form-check-league-type')).toBeInTheDocument();
    });
  });
});

