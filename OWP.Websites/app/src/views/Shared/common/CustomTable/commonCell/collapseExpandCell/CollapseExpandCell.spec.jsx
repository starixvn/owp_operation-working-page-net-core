import { fireEvent, render } from '@testing-library/react';
import CollapseExpandCell from './CollapseExpandCell';

const onCollapse = jest.fn();
const onExpand = jest.fn();

describe('CollapseExpandCell under test', () => {
  const defaultProps = {
    headerName: 'Trader',
    mode: 'collapse',
    onCollapse,
    onExpand
  };
  it('should to match snapshot', () => {
    const component = render(<CollapseExpandCell {...defaultProps} />);
    expect(component).toMatchSnapshot();
  });

  it('should call onExpand', () => {
    const { container } = render(<CollapseExpandCell {...defaultProps} />);
    const expandIcon = container.querySelector('.icon-expand');
    fireEvent.click(expandIcon);
    expect(onExpand).toBeCalledTimes(1);
  });
  it('should call collapse', () => {
    const props = {
      ...defaultProps,
      mode: 'expand'
    };
    const { container } = render(<CollapseExpandCell {...props} />);
    const expandIcon = container.querySelector('.icon-expand');
    fireEvent.click(expandIcon);
    expect(onExpand).toBeCalledTimes(1);
  });
});
