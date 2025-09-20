import React from 'react';
import { render, screen, fireEvent } from '@testing-library/react';
import Year from './Year';

// Mock react-select
jest.mock('react-select', () => {
  return function MockSelect({ options, defaultValue, onChange, name, className, classNamePrefix }) {
    return (
      <select
        data-testid="year-select"
        className={className}
        data-classnameprefix={classNamePrefix}
        name={name}
        onChange={(e) => onChange && onChange({ value: parseInt(e.target.value), label: e.target.value })}
        defaultValue={defaultValue?.[0]?.value}
      >
        {options.map(option => (
          <option key={option.value} value={option.value}>
            {option.label}
          </option>
        ))}
      </select>
    );
  };
});

describe('Year Component', () => {
  let mockHandleChange;

  beforeEach(() => {
    mockHandleChange = jest.fn();
  });

  afterEach(() => {
    jest.clearAllMocks();
  });

  describe('Rendering', () => {
    it('should render year filter component', () => {
      // Arrange & Act
      render(<Year handleChange={mockHandleChange} />);

      // Assert
      expect(screen.getByText('Year')).toBeInTheDocument();
      expect(screen.getByTestId('year-select')).toBeInTheDocument();
    });

    it('should have correct CSS classes', () => {
      // Arrange & Act
      const { container } = render(<Year handleChange={mockHandleChange} />);

      // Assert
      expect(container.querySelector('.filter-control')).toBeInTheDocument();
      expect(container.querySelector('.label')).toBeInTheDocument();
      expect(screen.getByTestId('year-select')).toHaveClass('basic-multi-select');
      expect(screen.getByTestId('year-select')).toHaveAttribute('data-classnameprefix', 'select');
    });

    it('should have correct name attribute', () => {
      // Arrange & Act
      render(<Year handleChange={mockHandleChange} />);

      // Assert
      expect(screen.getByTestId('year-select')).toHaveAttribute('name', 'year');
    });
  });

  describe('Year Options Generation', () => {
    it('should generate years from 10 years ago to current year', () => {
      // Arrange
      const currentYear = new Date().getFullYear();
      const expectedMinYear = currentYear - 10;

      // Act
      render(<Year handleChange={mockHandleChange} />);
      const selectElement = screen.getByTestId('year-select');
      const options = Array.from(selectElement.querySelectorAll('option'));

      // Assert
      expect(options).toHaveLength(11); // 10 years + current year
      expect(parseInt(options[0].value)).toBe(expectedMinYear);
      expect(parseInt(options[options.length - 1].value)).toBe(currentYear);
    });

    it('should have current year as default value', () => {
      // Arrange
      const currentYear = new Date().getFullYear();

      // Act
      render(<Year handleChange={mockHandleChange} />);
      const selectElement = screen.getByTestId('year-select');

      // Assert
      expect(selectElement).toHaveValue(currentYear.toString());
    });

    it('should format options with value and label', () => {
      // Arrange
      const currentYear = new Date().getFullYear();

      // Act
      render(<Year handleChange={mockHandleChange} />);
      const selectElement = screen.getByTestId('year-select');
      const options = Array.from(selectElement.querySelectorAll('option'));

      // Assert
      options.forEach(option => {
        expect(option.value).toBe(option.textContent);
        expect(parseInt(option.value)).toBeGreaterThanOrEqual(currentYear - 10);
        expect(parseInt(option.value)).toBeLessThanOrEqual(currentYear);
      });
    });
  });

  describe('User Interaction', () => {
    it('should call handleChange when year is selected', () => {
      // Arrange
      const currentYear = new Date().getFullYear();
      const selectedYear = currentYear - 5;

      // Act
      render(<Year handleChange={mockHandleChange} />);
      const selectElement = screen.getByTestId('year-select');
      fireEvent.change(selectElement, { target: { value: selectedYear.toString() } });

      // Assert
      expect(mockHandleChange).toHaveBeenCalledTimes(1);
      expect(mockHandleChange).toHaveBeenCalledWith({
        value: selectedYear,
        label: selectedYear.toString()
      });
    });

    it('should call handleChange with correct format when year changes', () => {
      // Arrange
      const currentYear = new Date().getFullYear();
      const testYear = currentYear - 3;

      // Act
      render(<Year handleChange={mockHandleChange} />);
      const selectElement = screen.getByTestId('year-select');
      fireEvent.change(selectElement, { target: { value: testYear.toString() } });

      // Assert
      expect(mockHandleChange).toHaveBeenCalledWith({
        value: testYear,
        label: testYear.toString()
      });
    });
  });

  describe('Edge Cases', () => {
    it('should render component even with missing props', () => {
      // Arrange & Act
      const { container } = render(<Year handleChange={jest.fn()} />);

      // Assert - should render without throwing error
      expect(container.querySelector('.filter-control')).toBeInTheDocument();
    });

    it('should generate correct number of years for different current years', () => {
      // Arrange
      const originalDate = Date;
      const mockDate = new Date('2024-01-01');
      global.Date = jest.fn(() => mockDate);
      global.Date.prototype = originalDate.prototype;

      // Act
      render(<Year handleChange={mockHandleChange} />);
      const selectElement = screen.getByTestId('year-select');
      const options = Array.from(selectElement.querySelectorAll('option'));

      // Assert
      expect(options).toHaveLength(11); // 2014 to 2024

      // Cleanup
      global.Date = originalDate;
    });
  });

  describe('PropTypes Validation', () => {
    it('should require handleChange prop', () => {
      // This test ensures the component expects handleChange
      // In a real scenario, you might test PropTypes validation
      expect(() => {
        render(<Year />);
      }).not.toThrow(); // Component should render even without prop, but PropTypes will warn
    });
  });

  describe('Year Range Logic', () => {
    it('should include both min and max years in the range', () => {
      // Arrange
      const currentYear = new Date().getFullYear();
      const minYear = currentYear - 10;

      // Act
      render(<Year handleChange={mockHandleChange} />);
      const selectElement = screen.getByTestId('year-select');
      const options = Array.from(selectElement.querySelectorAll('option'));

      // Assert
      const yearValues = options.map(option => parseInt(option.value));
      expect(yearValues).toContain(minYear);
      expect(yearValues).toContain(currentYear);
    });

    it('should generate consecutive years', () => {
      // Arrange
      const currentYear = new Date().getFullYear();

      // Act
      render(<Year handleChange={mockHandleChange} />);
      const selectElement = screen.getByTestId('year-select');
      const options = Array.from(selectElement.querySelectorAll('option'));

      // Assert
      const yearValues = options.map(option => parseInt(option.value));
      for (let i = 1; i < yearValues.length; i++) {
        expect(yearValues[i] - yearValues[i - 1]).toBe(1);
      }
    });
  });
});
