import React from 'react';
import { render, screen, fireEvent } from '@testing-library/react';
import '@testing-library/jest-dom';
import Condition from './Condition';
import { useAppDispatch } from '../../app/hooks';
import {
  updateObject,
  updateExpression,
  addRowExpressions,
  removeRowExpressions
} from '../slice/editSettingSlice';

jest.mock('../../app/hooks', () => ({
  useAppDispatch: jest.fn()
}));

jest.mock('../slice/editSettingSlice', () => ({
  updateObject: jest.fn(),
  updateExpression: jest.fn(),
  addRowExpressions: jest.fn(),
  removeRowExpressions: jest.fn()
}));

jest.mock('../../component/CommonTextArea', () => {
  return function MockCommonTextArea({ value, handleOnChange }: any) {
    return (
      <textarea
        data-testid='remark-textarea'
        value={value}
        onChange={handleOnChange}
      />
    );
  };
});

jest.mock('../../component/RowsExpression', () => {
  return function MockRowsExpression({
    title,
    expressionList,
    property,
    handleAddRow,
    handleDeleteRow
  }: any) {
    return (
      <div data-testid={`rows-expression-${property}`}>
        <div>{title}</div>
        <button
          data-testid={`add-${property}`}
          onClick={e => handleAddRow(e, property)}>
          Add {property}
        </button>
        {expressionList.map((_: any, index: number) => (
          <button
            key={index}
            data-testid={`delete-${property}-${index}`}
            onClick={e => handleDeleteRow(e, property, index)}>
            Delete {index}
          </button>
        ))}
      </div>
    );
  };
});

jest.mock('../../helper/helper', () => ({
  convertResponseToCondition: jest.fn(data => data || []),
  convertDivisionResponseToCondition: jest.fn(data => data || [])
}));

const mockDispatch = jest.fn();
const mockUseAppDispatch = useAppDispatch as jest.Mock;
const mockUpdateObject = updateObject as jest.Mock;
const mockUpdateExpression = updateExpression as jest.Mock;
const mockAddRowExpressions = addRowExpressions as jest.Mock;
const mockRemoveRowExpressions = removeRowExpressions as jest.Mock;

describe('Condition Component', () => {
  const defaultProps = {
    condition: {
      hasTv: false,
      hasIpTv: false,
      round: [],
      division: [],
      remark: ''
    }
  };

  beforeEach(() => {
    jest.clearAllMocks();
    mockUseAppDispatch.mockReturnValue(mockDispatch);
    mockUpdateObject.mockReturnValue({ type: 'updateObject' });
    mockUpdateExpression.mockReturnValue({ type: 'updateExpression' });
    mockAddRowExpressions.mockReturnValue({ type: 'addRowExpressions' });
    mockRemoveRowExpressions.mockReturnValue({ type: 'removeRowExpressions' });
  });

  describe('Rendering', () => {
    it('should render title correctly', () => {
      render(<Condition {...defaultProps} />);

      expect(screen.getByText('Condition')).toBeInTheDocument();
    });

    it('should render TV section', () => {
      render(<Condition {...defaultProps} />);

      expect(screen.getByText('TV')).toBeInTheDocument();
      expect(screen.getByText('TV (check in TV list)')).toBeInTheDocument();
      expect(screen.getByText('IPTV')).toBeInTheDocument();
    });

    it('should render Round and Division sections', () => {
      render(<Condition {...defaultProps} />);

      expect(screen.getByTestId('rows-expression-round')).toBeInTheDocument();
      expect(
        screen.getByTestId('rows-expression-division')
      ).toBeInTheDocument();
    });

    it('should render Remark section', () => {
      render(<Condition {...defaultProps} />);

      expect(screen.getByText('Remark')).toBeInTheDocument();
      expect(screen.getByTestId('remark-textarea')).toBeInTheDocument();
    });
  });

  describe('TV Checkboxes', () => {
    it('should render TV checkbox as unchecked when hasTv is false', () => {
      render(<Condition {...defaultProps} />);

      const checkboxes = screen.getAllByRole('checkbox');
      expect(checkboxes[0]).not.toBeChecked();
    });

    it('should render TV checkbox as checked when hasTv is true', () => {
      render(
        <Condition condition={{ ...defaultProps.condition, hasTv: true }} />
      );

      const checkboxes = screen.getAllByRole('checkbox');
      expect(checkboxes[0]).toBeChecked();
    });

    it('should dispatch updateObject when TV checkbox is changed', () => {
      render(<Condition {...defaultProps} />);

      const checkboxes = screen.getAllByRole('checkbox');
      fireEvent.click(checkboxes[0]);

      expect(mockDispatch).toHaveBeenCalled();
      expect(mockUpdateObject).toHaveBeenCalledWith({
        property: 'hasTv',
        value: true
      });
    });

    it('should dispatch updateObject when IPTV checkbox is changed', () => {
      render(<Condition {...defaultProps} />);

      const checkboxes = screen.getAllByRole('checkbox');
      fireEvent.click(checkboxes[1]);

      expect(mockDispatch).toHaveBeenCalled();
      expect(mockUpdateObject).toHaveBeenCalledWith({
        property: 'hasIpTv',
        value: true
      });
    });
  });

  describe('Remark Textarea', () => {
    it('should display remark value', () => {
      render(
        <Condition
          condition={{ ...defaultProps.condition, remark: 'Test remark' }}
        />
      );

      const textarea = screen.getByTestId('remark-textarea');
      expect(textarea).toHaveValue('Test remark');
    });

    it('should dispatch updateObject when remark changes', () => {
      render(<Condition {...defaultProps} />);

      const textarea = screen.getByTestId('remark-textarea');
      fireEvent.change(textarea, { target: { value: 'New remark' } });

      expect(mockDispatch).toHaveBeenCalled();
      expect(mockUpdateObject).toHaveBeenCalledWith({
        property: 'remark',
        value: 'New remark'
      });
    });

    it('should NOT dispatch when remark value is unchanged', () => {
      render(
        <Condition condition={{ ...defaultProps.condition, remark: 'Test' }} />
      );

      const textarea = screen.getByTestId('remark-textarea');
      fireEvent.change(textarea, { target: { value: 'Test' } });

      expect(mockDispatch).not.toHaveBeenCalled();
    });
  });

  describe('Add/Delete Rows', () => {
    it('should dispatch addRowExpressions when add button is clicked for round', () => {
      render(<Condition {...defaultProps} />);

      const addButton = screen.getByTestId('add-round');
      fireEvent.click(addButton);

      expect(mockDispatch).toHaveBeenCalled();
      expect(mockAddRowExpressions).toHaveBeenCalledWith('round');
    });

    it('should dispatch addRowExpressions when add button is clicked for division', () => {
      render(<Condition {...defaultProps} />);

      const addButton = screen.getByTestId('add-division');
      fireEvent.click(addButton);

      expect(mockDispatch).toHaveBeenCalled();
      expect(mockAddRowExpressions).toHaveBeenCalledWith('division');
    });

    it('should dispatch removeRowExpressions when delete button is clicked', () => {
      const props = {
        condition: {
          ...defaultProps.condition,
          round: [{ type: 'Contains', value: 'Test' }]
        }
      };
      render(<Condition {...props} />);

      const deleteButton = screen.getByTestId('delete-round-0');
      fireEvent.click(deleteButton);

      expect(mockDispatch).toHaveBeenCalled();
      expect(mockRemoveRowExpressions).toHaveBeenCalledWith({
        index: 0,
        property: 'round'
      });
    });
  });
});
