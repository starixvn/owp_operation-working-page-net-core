import React from 'react';
import { render } from '@testing-library/react';
import ReadonlyCommonCell from './ReadonlyCommonCell';
import { UncontrolledTooltip } from 'reactstrap';

jest.mock('reactstrap', () => ({
  UncontrolledTooltip: jest.fn(() => <div>Tooltip</div>)
}));

describe('ReadonlyCommonCell', () => {
  const defaultProps = {
    className: 'test-class',
    value: 'Test Value',
    index: 1,
    isCanToolTip: true
  };

  it('should render correctly', () => {
    const { getByText } = render(<ReadonlyCommonCell {...defaultProps} />);

    expect(getByText('Test Value')).toBeInTheDocument();
  });

  it('should render tooltip when isCanToolTip is true and value is provided', () => {
    const { getByText } = render(<ReadonlyCommonCell {...defaultProps} />);

    expect(UncontrolledTooltip).toHaveBeenCalledWith(
      expect.objectContaining({
        placement: 'top',
        target: 'test-class-1'
      }),
      {}
    );
    expect(getByText('Tooltip')).toBeInTheDocument();
  });

  it('should not render tooltip when isCanToolTip is false', () => {
    const props = {
      ...defaultProps,
      isCanToolTip: false
    };
    const { queryByText } = render(<ReadonlyCommonCell {...props} />);

    expect(queryByText('Tooltip')).not.toBeInTheDocument();
  });

  it('should not render tooltip when value is empty', () => {
    const props = {
      ...defaultProps,
      value: ''
    };
    const { queryByText } = render(<ReadonlyCommonCell {...props} />);

    expect(queryByText('Tooltip')).not.toBeInTheDocument();
  });
});
