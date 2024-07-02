import OrderHeader from './OrderHeader';
import { act, fireEvent, render } from '@testing-library/react';

describe('FilterHeader', () => {
  afterEach(() => {
    jest.resetAllMocks();
  });

  const onOrder = jest.fn();
  const defaultProps = {
    color: 'bg-color1',
    name: 'ESoccers',
    onOrder,
    subColId: '1',
    orderName: ''
  };

  it('should handle sort asc when click order by', () => {
    const props = { orderName: 'sort-asc' };
    const { container } = render(<OrderHeader {...defaultProps} {...props} />);
    const spanOrder = container.querySelector('.sort-icon');
    expect(spanOrder).toBeInTheDocument();
    fireEvent.click(spanOrder);
    expect(onOrder).toHaveBeenCalledTimes(1);
  });

  it('should handle sort desc when click order by', () => {
    const props = { orderName: 'sort-desc' };
    const { container } = render(<OrderHeader {...defaultProps} {...props} />);
    const spanOrder = container.querySelector('.sort-icon');
    fireEvent.click(spanOrder);
    expect(onOrder).toHaveBeenCalledTimes(1);
  });
});
